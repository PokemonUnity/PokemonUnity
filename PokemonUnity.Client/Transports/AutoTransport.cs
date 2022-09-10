using System;
using PokemonUnity.Client.Http;

namespace PokemonUnity.Client.Transports
{
	public class AutoTransport : IClientTransport
	{
		private IClientTransport m_transport; // Transport that's in use
		private readonly IClientTransport[] m_transports; // List of transports in fallback order
		private readonly IHttpClient m_httpClient;

		public AutoTransport(IHttpClient httpClient)
		{
			m_httpClient = httpClient;
			m_transports = new IClientTransport[] { 
				new ServerSentEventsTransport(httpClient), 
				new LongPollingTransport(httpClient) 
			};
		}

		public EventSignal<NegotiationResponse> Negotiate(IConnection connection)
		{
			return HttpBasedTransport.GetNegotiationResponse(m_httpClient, connection);
		}

		public void Start(IConnection connection, string data)
		{
			// Resolve the transport
			ResolveTransport(connection, data, 0);
		}

		private void ResolveTransport(IConnection connection, string data, int index)
		{
			// Pick the current transport
			IClientTransport _transport = m_transports[index];

			try
			{
				_transport.Start(connection, data);
				m_transport = _transport;
			}
			catch (Exception)
			{
				var _next = index + 1;
				if (_next < m_transports.Length)
				{
					// Try the next transport
					ResolveTransport(connection, data, _next);
				}
				else
				{
					// If there's nothing else to try then just fail
					throw new NotSupportedException("The transports available were not supported on this client.");
				}
			}
		}

		public EventSignal<T> Send<T>(IConnection connection, string data)
		{
			return m_transport.Send<T>(connection, data);
		}

		public void Stop(IConnection connection)
		{
			m_transport.Stop(connection);
		}
	}
}
