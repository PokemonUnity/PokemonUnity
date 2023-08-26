using System.Collections;

namespace PokemonUnity
{
	public static class AbilityExtension
	{
		/// <summary>
		/// </summary>
		/// <param name="ability"></param>
		/// <param name="text"></param>
		/// <returns>Returns the lookup identifier for localization dictionary</returns>
		/// <remarks>Requires <seealso cref="Game._INTL(string, object[])"/> to retrieve actual value</remarks>
		public static string ToString(this PokemonUnity.Abilities ability, TextScripts text)
		{
			if (text == TextScripts.Name)
				return string.Format("ABILITY_NAME_{0}", (int)ability);
			if (text == TextScripts.Description)
				return string.Format("ABILITY_DESC_{0}", (int)ability);
			//create a switch, and return Locale Name or Description
			return ability.ToString();
		}
	}
}