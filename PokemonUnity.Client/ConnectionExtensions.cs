using System;
using Newtonsoft.Json;
using PokemonUnity.Client.Hubs;

namespace PokemonUnity.Client
{
	public static class ConnectionExtensions
	{
		public static T GetValue<T>(IConnection connection, string key)
		{
			object _value;
			if (connection.Items.TryGetValue(key, out _value))
				return (T)_value;

			return default(T);
		}
	}
}
