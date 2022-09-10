using System.Net;

namespace PokemonUnity.Client.Http
{
	public interface IRequest
	{
		string UserAgent { get; set; }

		ICredentials Credentials { get; set; }

		CookieContainer CookieContainer { get; set; }

		string Accept { get; set; }

		void Abort();
	}
}
