using System.Collections;

namespace PokemonUnity.Monster
{
	public static class AbilityExtension
	{
		public static string ToString(this PokemonUnity.Abilities ability, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return ability.ToString();
		}
	}
}