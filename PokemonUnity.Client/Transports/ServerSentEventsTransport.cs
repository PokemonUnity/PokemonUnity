using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using PokemonUnity.Client.Http;
using PokemonUnity.Client.Infrastructure;

namespace PokemonUnity.Client.Transports
{
	public class ServerSentEventsTransport : HttpBasedTransport
	{
		private const string m_readerKey = "sse.reader";
		private int m_initializedCalled;
		private static readonly TimeSpan m_reconnectDelay = TimeSpan.FromSeconds(2);
		private TimeSpan m_connectionTimeout; // Time allowed before failing the connect request
		private int m_connectionRetry;

		public ServerSentEventsTransport()
			: this(new DefaultHttpClient())
		{
		}

		public ServerSentEventsTransport(IHttpClient httpClient)
			: base(httpClient, "serverSentEvents")
		{
			m_connectionTimeout = TimeSpan.FromSeconds(2);
			m_connectionRetry = 30;
		}

		protected override void OnStart(IConnection connection,
			string data,
			Action initializeCallback,
			Action<Exception> errorCallback)
		{
			OpenConnection(connection, data, initializeCallback, errorCallback);
		}

		protected override void OnBeforeAbort(IConnection connection)
		{
			// Get the reader from the connection and stop it
			AsyncStreamReader _reader = ConnectionExtensions.GetValue<AsyncStreamReader>(connection, m_readerKey);
			if (_reader != null)
			{
				// Stop reading data from the stream
				_reader.StopReading(false);

				// Remove the reader
				connection.Items.Remove(m_readerKey);
			}
		}

		private void Reconnect(IConnection connection, string data)
		{
			if (!connection.IsActive)
				return;

			// Wait for a bit before reconnecting
			Thread.Sleep(m_reconnectDelay);

			// Now attempt a reconnect
			OpenConnection(connection, data, null, null);
		}

		private void OpenConnection(IConnection connection,
			string data,
			Action initializeCallback,
			Action<Exception> errorCallback)
		{
			// If we're reconnecting add /connect to the url
			bool _reconnecting = initializeCallback == null;
			string _url = (_reconnecting ? connection.Url : connection.Url + "connect");
			Action<IRequest> _prepareRequest = PrepareRequest(connection);
			EventSignal<IResponse> _signal;

			if (shouldUsePost(connection))
			{
				_url += GetReceiveQueryString(connection, data);
				Debug.WriteLine(string.Format("SSE: POST {0}", _url));

				_signal = m_httpClient.PostAsync(
					_url,
					request =>
					{
						_prepareRequest(request);
						request.Accept = "text/event-stream";
					},
					new Dictionary<string, string> { 
					{
						"groups", GetSerializedGroups(connection) } 
					});
			}
			else
			{
				_url += GetReceiveQueryStringWithGroups(connection, data);
				Debug.WriteLine(string.Format("SSE: GET {0}", _url));

				_signal = m_httpClient.GetAsync(
					_url,
					request =>
					{
						_prepareRequest(request);
						request.Accept = "text/event-stream";
					});
			}

			_signal.Finished += (sender, e) =>
			{
				if (e.Result.IsFaulted)
				{
					Exception _exception = e.Result.Exception.GetBaseException();

					if (!HttpBasedTransport.IsRequestAborted(_exception))
					{
						if (errorCallback != null &&
							Interlocked.Exchange(ref m_initializedCalled, 1) == 0)
							errorCallback(_exception);
						else if (_reconnecting)
							// Only raise the error event if we failed to reconnect
							connection.OnError(_exception);
					}

					if (_reconnecting)
					{
						// Retry
						Reconnect(connection, data);
						return;
					}
				}
				else
				{
					// Get the reseponse stream and read it for messages
					IResponse _response = e.Result;
					Stream _stream = _response.GetResponseStream();
					AsyncStreamReader _reader = new AsyncStreamReader(
						_stream,
						connection,
						() =>
						{
							if (Interlocked.CompareExchange(ref m_initializedCalled, 1, 0) == 0)
								initializeCallback();
						},
						() =>
						{
							_response.Close();
							Reconnect(connection, data);
						});

					if (_reconnecting)
						// Raise the reconnect event if the connection comes back up
						connection.OnReconnected();

					_reader.StartReading();

					// Set the reader for this connection
					connection.Items[m_readerKey] = _reader;
				}
			};

			if (initializeCallback != null)
			{
				int _tryed = 0;
				while (true)
				{
					_tryed++;
					Debug.Write("Checking if connection initialized for the '" + _tryed.ToString() + "' time: ");

					Thread.Sleep(m_connectionTimeout);
					if (Interlocked.CompareExchange(ref m_initializedCalled, 1, 0) == 0)
					{
						if (_tryed < m_connectionRetry)
						{
							Debug.WriteLine("failed.");
							continue;
						}

						Debug.WriteLine("giving up.");

						// Stop the connection
						Stop(connection);

						// Connection timeout occurred
						errorCallback(new TimeoutException());

						break;
					}
					else
					{
						Debug.WriteLine("success.");
						break;
					}
				}
			}
		}

		private static bool shouldUsePost(IConnection connection)
		{
			return new List<string>(connection.Groups).Count > 20;
		}
	}
}
