using PokemonUnity;
//using PokemonUnity.Pokemon;
using PokemonUnity.Inventory;
//using PokemonUnity.Attack;
using PokemonUnity.Battle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonUnity.Battle
{
	/// <summary>
	/// </summary>
	public partial class Battle 
	{
		/// <summary>
		/// Options made on a given turn, per pokemon.
		/// </summary>
		/// ToDo: Make a logger of this as a List<> to document a match history.
		/// ToDo: If making logger, consider documenting math/results as well...
		public class Choice
		{
			public ChoiceAction Action { get; private set; }
			/// <summary>
			/// Index of Action being used
			/// </summary>
			public int Index { get; private set; }
			public Move Move { get; private set; }
			public int Target { get; private set; }

			/// <summary>
			/// If action you're choosing to take is to Attack with a Move
			/// </summary>
			/// <param name="action"></param>
			/// <param name="move"></param>
			/// <param name="target"></param>
			public Choice (ChoiceAction action, int moveIndex, Move move, int target = -1)
			{
				Action = action;
				Index = moveIndex;
				Move = move;
				Target = target;
			}

			/// <summary>
			/// If action you're choosing to take is to Switch Pkmns
			/// </summary>
			/// <param name="action"></param>
			/// <param name="pkmnIndex"></param>
			public Choice (ChoiceAction action, int pkmnIndex)
			{
				Action = action;
				Index = pkmnIndex;
			}

			/// <summary>
			/// If action you're choosing to take is to Use an Item on a Pkmn
			/// </summary>
			/// <param name="action"></param>
			/// <param name="itemIndex"></param>
			/// <param name="pkmnTarget"></param>
			public Choice (ChoiceAction action, int itemIndex, int pkmnTarget)
			{
				Action = action;
				Index = itemIndex;
				Target = pkmnTarget;
			}

			/// <summary>
			/// If action you're choosing to take is to Flee, Call Pokemon, or Nothing
			/// </summary>
			public Choice (ChoiceAction action = 0)
			{
				Action = action;
			}
		}
	}
}