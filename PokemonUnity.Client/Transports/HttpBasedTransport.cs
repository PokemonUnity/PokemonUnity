using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PokemonUnity.Client.Http;

namespace PokemonUnity.Client.Transports
{
	public abstract class HttpBasedTransport : IClientTransport
	{
		// The receive query strings
		private const string c_receiveQueryStringWithGroups = "?transport={0}&connectionId={1}&messageId={2}&groups={3}&connectionData={4}{5}&connectionToken={6}&groupsToken={7}";
		private const string c_receiveQueryString = "?transport={0}&connectionId={1}&messageId={2}&connectionData={3}{4}&connectionToken={5}";

		private const string c_sendQueryString = "?transport={0}&connectionToken={1}{2}"; // The send query string
		protected readonly string m_transport; // The transport name
		protected const string c_httpRequestKey = "http.Request";
		protected readonly IHttpClient m_httpClient;

		public HttpBasedTransport(IHttpClient httpClient, string transport)
		{
			m_httpClient = httpClient;
			m_transport = transport;
		}

		public EventSignal<NegotiationResponse> Negotiate(IConnection connection)
		{
			return GetNegotiationResponse(m_httpClient, connection);
		}

		internal static EventSignal<NegotiationResponse> GetNegotiationResponse(
			IHttpClient httpClient,
			IConnection connection)
		{
			string _negotiateUrl = connection.Url + "negotiate";

			var _negotiateSignal = new EventSignal<NegotiationResponse>();
			var _signal = httpClient.GetAsync(_negotiateUrl, connection.PrepareRequest);

			_signal.Finished += (sender, e) =>
			{
				string _raw = e.Result.ReadAsString();
				if (_raw == null)
					throw new InvalidOperationException("Server negotiation failed.");

				_negotiateSignal.OnFinish(JsonConvert.DeserializeObject<NegotiationResponse>(_raw));
			};
			return _negotiateSignal;
		}

		public void Start(IConnection connection, string data)
		{
			OnStart(connection, data, () => { }, exception => { throw exception; });
		}

		protected abstract void OnStart(IConnection connection, string data, System.Action initializeCallback, Action<Exception> errorCallback);

		public EventSignal<T> Send<T>(IConnection connection, string data)
		{
			string _url = connection.Url + "send";
			string _customQueryString = GetCustomQueryString(connection);

			_url += String.Format(
				c_sendQueryString,
				m_transport,
				Uri.EscapeDataString(connection.ConnectionToken),
				_customQueryString);

			var _postData = new Dictionary<string, string> {
				{ "data", data },
			};

			var _returnSignal = new EventSignal<T>();
			var _postSignal = m_httpClient.PostAsync(_url, connection.PrepareRequest, _postData);

			_postSignal.Finished += (sender, e) =>
			{
				string _raw = e.Result.ReadAsString();

				if (String.IsNullOrEmpty(_raw))
				{
					_returnSignal.OnFinish(default(T));
					return;
				}

				_returnSignal.OnFinish(JsonConvert.DeserializeObject<T>(_raw));
			};
			return _returnSignal;
		}

		protected string GetReceiveQueryStringWithGroups(IConnection connection, string data)
		{
			return String.Format(
				c_receiveQueryStringWithGroups,
				m_transport,
				Uri.EscapeDataString(connection.ConnectionId),
				Convert.ToString(connection.MessageId),
				GetSerializedGroups(connection),
				data,
				GetCustomQueryString(connection),
				Uri.EscapeDataString(connection.ConnectionToken),
				connection.GroupsToken);
		}

		protected string GetSerializedGroups(IConnection connection)
		{
			return Uri.EscapeDataString(JsonConvert.SerializeObject(connection.Groups));
		}

		protected string GetReceiveQueryString(IConnection connection, string data)
		{
			return String.Format(
				c_receiveQueryString,
				m_transport,
				Uri.EscapeDataString(connection.ConnectionId),
				Convert.ToString(connection.MessageId),
				data,
				GetCustomQueryString(connection),
				Uri.EscapeDataString(connection.ConnectionToken));
		}

		protected virtual Action<IRequest> PrepareRequest(IConnection connection)
		{
			return request =>
			{
				// Setup the user agent along with any other defaults
				connection.PrepareRequest(request);
				connection.Items[c_httpRequestKey] = request;
			};
		}

		public static bool IsRequestAborted(Exception exception)
		{
			var _webException = exception as WebException;
			return (_webException != null && _webException.Status == WebExceptionStatus.RequestCanceled);
		}

		public void Stop(IConnection connection)
		{
			var _httpRequest = ConnectionExtensions.GetValue<IRequest>(connection, c_httpRequestKey);
			if (_httpRequest != null)
			{
				try
				{
					OnBeforeAbort(connection);
					_httpRequest.Abort();
				}
				catch (NotImplementedException)
				{
					// If this isn't implemented then do nothing
				}
			}
		}

		protected virtual void OnBeforeAbort(IConnection connection)
		{
		}

		public static void ProcessResponse(IConnection connection, string response, out bool timedOut, out bool disconnected)
		{
			timedOut = false;
			disconnected = false;
			Debug.WriteLine("ProcessResponse: " + response);

			if (String.IsNullOrEmpty(response))
				return;

			if (connection.MessageId == null)
				connection.MessageId = null;

			try
			{
				var _result = JValue.Parse(response);
				Debug.WriteLine("ProcessResponse: result parsed");

				if (!_result.HasValues)
					return;

				timedOut = _result.Value<bool>("TimedOut");
				disconnected = _result.Value<bool>("Disconnect");

				if (disconnected)
					return;

				var messages = _result["M"] as JArray;

				if (messages != null)
				{
					foreach (JToken message in messages)
					{
						try
						{
							Debug.WriteLine("ProcessResponse: before invoking OnReceived");
							connection.OnReceived(message);
						}
						catch (Exception ex)
						{
							Debug.WriteLine("ProcessResponse: exception in OnReceived event '" + ex.Message + "'.");
							connection.OnError(ex);
						}
					}

					connection.MessageId = Extensions.Value<string>(_result["C"]);

					var _transportData = _result["T"] as JObject;

					if (_transportData != null)
					{
						var groups = (JArray)_transportData["G"];
						if (groups != null)
						{
							var groupList = new List<string>();
							foreach (var groupFromTransport in groups)
							{
								groupList.Add(Extensions.Value<string>(groupFromTransport));
							}
							connection.Groups = groupList;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(string.Format("Failed to response: {0}", ex));
				connection.OnError(ex);
			}
		}

		private static string GetCustomQueryString(IConnection connection)
		{
			return String.IsNullOrEmpty(connection.QueryString)
				? ""
				: "&" + connection.QueryString;
		}
	}
}
