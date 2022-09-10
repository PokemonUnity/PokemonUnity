using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PokemonUnity.Client.Hubs
{
	public static class HubProxyExtensions
	{
		public static T GetValue<T>(IHubProxy proxy, string name)
		{
			object _value = proxy[name];
			return Convert<T>(_value);
		}

		private static T Convert<T>(object obj)
		{
			if (obj == null)
				return default(T);

			if (typeof(T).IsAssignableFrom(obj.GetType()))
				return (T)obj;

			return JsonConvert.DeserializeObject<T>(obj.ToString());
		}
	}
}
