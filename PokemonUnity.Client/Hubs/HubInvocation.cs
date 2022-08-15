using System.Collections.Generic;
using Newtonsoft.Json;

namespace PokemonUnity.Client.Hubs
{
	public class HubInvocation
	{
		[JsonProperty("I")]
		public string CallbackId { get; set; }

		[JsonProperty("H")]
		public string Hub { get; set; }

		[JsonProperty("M")]
		public string Method { get; set; }

		[JsonProperty("A")]
		public object[] Args { get; set; }

		[JsonProperty("S", NullValueHandling = NullValueHandling.Ignore)]
		public Dictionary<string, object> State { get; set; }
	}
}
