using PokemonUnity.Client.Http;
using PokemonUnity.Client.Transports;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PokemonUnity.Client.Infrastructure
{
	internal class StreamState
	{
		public Stream Stream { get; set; }
		public byte[] Buffer { get; set; }
		public EventSignal<CallbackDetail<int>> Response { get; set; }
	}
}
