using PokemonUnity.Client.Transports;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PokemonUnity.Client.Http
{
	public class RequestState
	{
		public HttpWebRequest Request { get; set; }
		public EventSignal<CallbackDetail<HttpWebResponse>> Response { get; set; }
		public byte[] PostData { get; set; }
	}
}
