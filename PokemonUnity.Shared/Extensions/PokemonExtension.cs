using System.Collections;

namespace PokemonUnity
{
	public static class PokemonExtension
	{
		public static bool IsNotNullOrNone(this PokemonUnity.Monster.Pokemon pokemon)
		{
			return pokemon != null || pokemon.Species != Pokemons.NONE; 
		}
		public static bool IsNotNullOrNone(this PokemonUnity.Battle.Pokemon pokemon)
		{
			return pokemon != null || pokemon.Species != Pokemons.NONE; 
		}
		public static string ToString(this PokemonUnity.Monster.Stats stat, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Battle.Stats stat, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Monster.Forms form, TextScripts text)
		{
			//create an operator and return Locale Name
			return form.ToString();
		}
		public static string ToString(this PokemonUnity.Pokemons pokemon, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return pokemon.ToString();
		}
	}
}