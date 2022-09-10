using System;
using System.Collections.Generic;
using PokemonUnity.Client.Infrastructure;
using PokemonUnity.Client.Transports;

namespace PokemonUnity.Client.Http
{
	public interface IHttpClient
	{
		EventSignal<IResponse> GetAsync(string url, Action<IRequest> prepareRequest);

		EventSignal<IResponse> PostAsync(string url, Action<IRequest> prepareRequest, Dictionary<string, string> postData);
	}
}
