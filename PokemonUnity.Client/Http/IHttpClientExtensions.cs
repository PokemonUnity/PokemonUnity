using System;
using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client.Http
{
	public static class IHttpClientExtensions
	{
		public static EventSignal<IResponse> PostAsync(
			IHttpClient client, 
			string url, 
			Action<IRequest> prepareRequest)
		{
			return client.PostAsync(url, prepareRequest, null);
		}
	}
}
