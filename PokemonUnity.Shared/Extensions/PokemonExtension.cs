using System.Collections;

namespace PokemonUnity
{
	public static class PokemonExtension
	{
		public static string ToString(this PokemonUnity.Monster.Stats stat, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("STAT_NAME_{0}", (int)stat);
			if (text == TextScripts.Description)
				return string.Format("STAT_DESC_{0}", (int)stat);
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Combat.Stats stat, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("STAT_NAME_{0}", (int)stat);
			if (text == TextScripts.Description)
				return string.Format("STAT_DESC_{0}", (int)stat);
			//create a switch, and return Locale Name or Description
			return stat.ToString();
		}
		public static string ToString(this PokemonUnity.Monster.Forms form, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("POKEMON-FORM_NAME_{0}", (int)form);
			if (text == TextScripts.Description)
				return string.Format("POKEMON-FORM_DESC_{0}", (int)form);
			//create an operator and return Locale Name
			return form.ToString();
		}
		public static string ToString(this PokemonUnity.Pokemons pokemon, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("POKEMON_NAME_{0}", (int)pokemon);
			if (text == TextScripts.Description)
				return string.Format("POKEMON_DESC_{0}", (int)pokemon);
			//create a switch, and return Locale Name or Description
			return pokemon.ToString();
		}

		public static string ToIdString(this PokemonUnity.Pokemons pokemon, int digits = 3)
		{
			return System.Text.RegularExpressions.Regex.Replace(((int)pokemon).ToString(), "[0-9]+", match => match.Value.PadLeft(digits, '0'));
		}
	}
}