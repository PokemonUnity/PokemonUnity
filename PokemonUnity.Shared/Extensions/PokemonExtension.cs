using System.Collections;

namespace PokemonUnity
{
	public static class PokemonExtension
	{
		public static string ToString(this PokemonUnity.Monster.Stats stat, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Combat.Stats stat, TextScripts text)
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

		public static string ToIdString(this PokemonUnity.Pokemons pokemon, int digits = 3)
		{
			return System.Text.RegularExpressions.Regex.Replace(((int)pokemon).ToString(), "[0-9]+", match => match.Value.PadLeft(digits, '0'));
		}
	}
}