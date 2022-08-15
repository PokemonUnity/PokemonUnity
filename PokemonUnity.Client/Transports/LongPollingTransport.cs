using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using PokemonUnity.Client.Http;

namespace PokemonUnity.Client.Transports
{
	public class LongPollingTransport : HttpBasedTransport
	{
		private static readonly TimeSpan m_errorDelay = TimeSpan.FromSeconds(2);

		public TimeSpan ReconnectDelay { get; set; }

		public LongPollingTransport()
			: this(new DefaultHttpClient())
		{
		}

		public LongPollingTransport(IHttpClient httpClient)
			: base(httpClient, "longPolling")
		{
			ReconnectDelay = TimeSpan.FromSeconds(5);
		}

		protected override void OnStart(IConnection connection, string data, System.Action initializeCallback, Action<Exception> errorCallback)
		{
			PollingLoop(connection, data, initializeCallback, errorCallback, false);
		}

		private void PollingLoop(IConnection connection, string data, System.Action initializeCallback, Action<Exception> errorCallback, bool raiseReconnect)
		{
			string _url = connection.Url;
			var _reconnectTokenSource = new CancellationTokenSource();
			int _reconnectFired = 0;

			if (connection.MessageId == null)
				_url += "connect";
			else if (raiseReconnect)
				_url += "reconnect";

			_url += GetReceiveQueryString(connection, data);

			Debug.WriteLine(string.Format("LP: {0}", _url));

			var _signal = m_httpClient.PostAsync(
				_url,
				PrepareRequest(connection),
				new Dictionary<string, string> { 
					{ "groups", GetSerializedGroups(connection) } 
				});

			_signal.Finished += (sender, e) =>
			{
				// Clear the pending request
				connection.Items.Remove(c_httpRequestKey);

				bool _shouldRaiseReconnect = false;
				bool _disconnectedReceived = false;

				try
				{
					if (!e.Result.IsFaulted)
					{
						if (raiseReconnect)
							// If the timeout for the reconnect hasn't fired as yet just fire the 
							// event here before any incoming messages are processed
							FireReconnected(connection, _reconnectTokenSource, ref _reconnectFired);

						// Get the response
						var _raw = e.Result.ReadAsString();

						Debug.WriteLine(string.Format("LP Receive: {0}", _raw));

						if (!String.IsNullOrEmpty(_raw))
							ProcessResponse(connection, _raw, out _shouldRaiseReconnect, out _disconnectedReceived);
					}
				}
				finally
				{
					if (_disconnectedReceived)
						connection.Stop();
					else
					{
						bool _requestAborted = false;

						if (e.Result.IsFaulted)
						{
							// Cancel the previous reconnect event
							_reconnectTokenSource.Cancel();

							// Raise the reconnect event if we successfully reconnect after failing
							_shouldRaiseReconnect = true;

							// Get the underlying exception
							Exception exception = e.Result.Exception.GetBaseException();

							// If the error callback isn't null then raise it and don't continue polling
							if (errorCallback != null)
							{
								// Raise on error
								connection.OnError(exception);

								// Call the callback
								errorCallback(exception);
							}
							else
							{
								// Figure out if the request was aborted
								_requestAborted = IsRequestAborted(exception);

								// Sometimes a connection might have been closed by the server before we get to write anything
								// so just try again and don't raise OnError.
								if (!_requestAborted && !(exception is IOException))
								{
									// Raise on error
									connection.OnError(exception);

									// If the connection is still active after raising the error event wait for 2 seconds
									// before polling again so we aren't hammering the server
									Thread.Sleep(m_errorDelay);
									if (connection.IsActive)
									{
										PollingLoop(
											connection,
											data,
											null, // initializeCallback
											null, // errorCallback
											_shouldRaiseReconnect); // raiseReconnect
									}
								}
							}
						}
						else
						{
							// Continue polling if there was no error
							if (connection.IsActive)
							{
								PollingLoop(
									connection,
									data,
									null, // initializeCallback
									null, // errorCallback
									_shouldRaiseReconnect); // raiseReconnect
							}
						}
					}
				}
			};

			if (initializeCallback != null)
				initializeCallback();

			if (raiseReconnect)
			{
				Thread.Sleep(ReconnectDelay);

				// Fire the reconnect event after the delay. This gives the 
				FireReconnected(connection, _reconnectTokenSource, ref _reconnectFired);
			}
		}

		private static void FireReconnected(IConnection connection,
			CancellationTokenSource reconnectTokenSource,
			ref int reconnectedFired)
		{
			if (!reconnectTokenSource.IsCancellationRequested
				&& Interlocked.Exchange(ref reconnectedFired, 1) == 0)
				connection.OnReconnected();
		}
	}
}
