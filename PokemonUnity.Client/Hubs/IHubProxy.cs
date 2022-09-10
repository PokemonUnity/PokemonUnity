using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client.Hubs
{
	public interface IHubProxy
	{
		object this[string name] { get; set; }

		Subscription Subscribe(string eventName);

		EventSignal<object> Invoke(string method, params object[] args);

		EventSignal<T> Invoke<T>(string method, params object[] args);
	}
}
