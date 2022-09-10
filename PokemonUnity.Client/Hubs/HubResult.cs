using System.Collections.Generic;

namespace PokemonUnity.Client.Hubs
{
	public class HubResult<T>
	{
		public T Result { get; set; }
		public string Error { get; set; }
		public IDictionary<string, object> State { get; set; }
	}
}