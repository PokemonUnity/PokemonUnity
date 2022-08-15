using Microsoft.AspNet.SignalR;

namespace PokemonUnity.Web
{
	public class PokemonUnityHub : Hub
	{
		public void Send(string name, string message)
		{
			// Call the broadcastMessage method to update clients.
			Clients.All.broadcastMessage(name, message);
		}
	}
}