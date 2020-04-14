using System.Collections;

namespace PokemonUnity.Attack
{
	public static class MoveExtension
	{
		public static bool IsNotNullOrNone(this PokemonUnity.Attack.Move move)
		{
			return move != null || move.MoveId != Moves.NONE;
		}
		public static bool IsNotNullOrNone(this PokemonUnity.Battle.Move move)
		{
			return move != null || move.MoveId != Moves.NONE;
		}
		public static string ToString(this PokemonUnity.Moves move, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return move.ToString();
		}
	}
}