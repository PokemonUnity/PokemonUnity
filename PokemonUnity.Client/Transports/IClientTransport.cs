using System;

namespace PokemonUnity.Client.Transports
{
	public interface IClientTransport
	{
		void Start(IConnection connection, string data);

		EventSignal<T> Send<T>(IConnection connection, string data);

		void Stop(IConnection connection);

		EventSignal<NegotiationResponse> Negotiate(IConnection connection);
	}
}
