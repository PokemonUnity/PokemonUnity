using PokemonUnity.Inventory;

namespace PokemonUnity.Combat.Data
{
	/// <summary>
	/// Options made on a given turn, per pokemon.
	/// </summary>
	public struct Choice
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
		/// If action you're choosing to take is to Attack with a Move
		/// </summary>
		/// <param name="action"></param>
		/// <param name="move"></param>
		/// <param name="target"></param>
		//ToDo: Attack.Move <= (Implicit/Explicit) => Battle.Move
		public Choice (ChoiceAction action, int moveIndex, Attack.Move move, int target = -1)
		{
			Action = action;
			Index = moveIndex;
			Move = null; //(Move)move;
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
			Target = -1;
			Move = null;
		}

		/// <summary>
		/// If action you're choosing to take is to Use an Item on a Pkmn
		/// </summary>
		/// <param name="action"></param>
		/// <param name="itemIndex"></param>
		/// <param name="pkmnTarget"></param>
		public Choice (ChoiceAction action, Items itemIndex, int pkmnTarget)
		{
			Action = action;
			Index = (int)itemIndex;
			Target = pkmnTarget;
			Move = null;
		}

		/// <summary>
		/// If action you're choosing to take is to Flee, Call Pokemon, or Nothing
		/// </summary>
		public Choice (ChoiceAction action = 0)
		{
			Action = action;
			Move = null;
			Target = -1;
			Index = 0;
		}
	}
}