using System.Collections;

namespace PokemonUnity.Monster
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
		public static string ToString(this PokemonUnity.Pokemons pokemon, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return pokemon.ToString();
		}
	}
}