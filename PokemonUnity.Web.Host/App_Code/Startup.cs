using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PokemonUnity.Web.Startup))]

namespace PokemonUnity.Web
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			app.MapSignalR();
		}
	}
}