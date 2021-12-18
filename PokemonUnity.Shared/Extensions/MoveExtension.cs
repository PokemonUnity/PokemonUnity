using System.Collections;

namespace PokemonUnity.Extension
{
	public static class MoveExtension
	{
		#region Targets
		public static bool HasMultipleTargets(this PokemonUnity.Attack.Data.Targets target)
		{
			return
				//target == Attack.Data.Targets.ENTIRE_FIELD ||
				//target == Attack.Data.Targets.OPPONENTS_FIELD ||
				//target == Attack.Data.Targets.USERS_FIELD ||
				target == Attack.Data.Targets.ALL_OPPONENTS ||
				target == Attack.Data.Targets.ALL_OTHER_POKEMON ||
				target == Attack.Data.Targets.ALL_POKEMON;
		}
		public static bool TargetsOneOpponent(this PokemonUnity.Attack.Data.Targets target)
		{
			return
				target == Attack.Data.Targets.SELECTED_POKEMON ||
				target == Attack.Data.Targets.SELECTED_POKEMON_ME_FIRST ||
				target == Attack.Data.Targets.RANDOM_OPPONENT;
		}
		#endregion
		public static string ToString(this PokemonUnity.Moves move, TextScripts text)
		{
			//create a switch, and return Locale Name or Description
			return move.ToString();
		}
	}
}