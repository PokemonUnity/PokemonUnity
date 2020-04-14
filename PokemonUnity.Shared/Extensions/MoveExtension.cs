using System.Collections;

namespace PokemonUnity.Attack
{
	public static class MoveExtension
	{		
		public static string ToString(this PokemonUnity.Moves move, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return move.ToString();
		}
	}
}