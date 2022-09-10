using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Threading;
using PokemonUnity.Client.Http;
using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client
{
	public class Connection : IConnection
	{
		private static Version m_assemblyVersion;
		private IClientTransport m_transport;
		private bool m_initialized;
		public event Action<string> Received;
		public event Action<Exception> Error;
		public event Action Closed;
		public event Action Reconnected;

		public CookieContainer CookieContainer { get; set; }
		public ICredentials Credentials { get; set; }
		public IEnumerable<string> Groups { get; set; }
		public System.Func<string> Sending { get; set; }
		public string Url { get; private set; }
		public bool IsActive { get; private set; }
		public string MessageId { get; set; }
		public string ConnectionId { get; set; }
		public IDictionary<string, object> Items { get; private set; }
		public string QueryString { get; private set; }
		public string ConnectionToken { get; set; }
		public string GroupsToken { get; set; }

		public Connection(string url)
			: this(url, (string)null)
		{
		}

		public Connection(string url, IDictionary<string, string> queryString)
			: this(url, CreateQueryString(queryString))
		{
		}

		public Connection(string url, string queryString)
		{
			if (url.Contains("?"))
				throw new ArgumentException("Url cannot contain QueryString directly. Pass QueryString values in using available overload.", "url");

			if (!url.EndsWith("/"))
				url += "/";

			Url = url;
			QueryString = queryString;
			Groups = new List<string>();
			Items = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
		}

		public void Start()
		{
			// Pick the best transport supported by the client
			Start(new DefaultHttpClient());
		}

		public void Start(IHttpClient httpClient)
		{
			Start(new AutoTransport(httpClient));
		}

		public virtual void Start(IClientTransport transport)
		{
			if (IsActive)
				return;

			IsActive = true;
			m_transport = transport;
			Negotiate(transport);
		}

		private void Negotiate(IClientTransport transport)
		{
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);

			var signal = transport.Negotiate(this);
			signal.Finished += (sender, e) =>
			{
				VerifyProtocolVersion(e.Result.ProtocolVersion);

				ConnectionId = e.Result.ConnectionId;
				ConnectionToken = e.Result.ConnectionToken;

				if (Sending != null)
				{
					var data = Sending();
					StartTransport(data);
					manualResetEvent.Set();
				}
				else
				{
					StartTransport(null);
					manualResetEvent.Set();
				}
			};
			manualResetEvent.WaitOne();
			m_initialized = true;
		}

		private void StartTransport(string data)
		{
			m_transport.Start(this, data);
		}

		private void VerifyProtocolVersion(string versionString)
		{
			Version version;
			if (String.IsNullOrEmpty(versionString) ||
				!TryParseVersion(versionString, out version) ||
				!(version.Major == 1 && version.Minor == 2))
			{
				throw new InvalidOperationException("Incompatible protocol version.");
			}
		}

		public virtual void Stop()
		{
			try
			{
				// Do nothing if the connection was never started
				if (!m_initialized)
					return;

				m_transport.Stop(this);

				if (Closed != null)
					Closed();
			}
			finally
			{
				IsActive = false;
				m_initialized = false;
			}
		}

		public EventSignal<object> Send(string data)
		{
			return Send<object>(data);
		}

		public EventSignal<T> Send<T>(string data)
		{
			if (!m_initialized)
				throw new InvalidOperationException("Start must be called before data can be sent");

			return m_transport.Send<T>(this, data);
		}

		void IConnection.OnReceived(Newtonsoft.Json.Linq.JToken message)
		{
			OnReceived(message);
		}

		protected virtual void OnReceived(Newtonsoft.Json.Linq.JToken message)
		{
			if (Received != null)
				Received(message.ToString());
		}

		void IConnection.OnError(Exception error)
		{
			if (Error != null)
				Error(error);
		}

		void IConnection.OnReconnected()
		{
			if (Reconnected != null)
				Reconnected();
		}

		void IConnection.PrepareRequest(IRequest request)
		{
			request.UserAgent = CreateUserAgentString("SignalR.Client");
			if (Credentials != null)
				request.Credentials = Credentials;

			if (CookieContainer != null)
				request.CookieContainer = CookieContainer;
		}

		private static string CreateUserAgentString(string client)
		{
			if (m_assemblyVersion == null)
				m_assemblyVersion = new AssemblyName(typeof(Connection).Assembly.FullName).Version;

			return String.Format(
				CultureInfo.InvariantCulture, 
				"{0}/{1} ({2})", 
				client, 
				m_assemblyVersion, 
				Environment.OSVersion);
		}

		private static bool TryParseVersion(string versionString, out Version version)
		{
			try
			{
				version = new Version(versionString);
				return true;
			}
			catch (ArgumentException)
			{
				version = new Version();
				return false;
			}
		}

		private static string CreateQueryString(IDictionary<string, string> queryString)
		{
			var _stringList = new List<string>();
			foreach (var keyValue in queryString)
			{
				_stringList.Add(keyValue.Key + "=" + keyValue.Value);
			}
			return String.Join("&", _stringList.ToArray());
		}
	}
}
