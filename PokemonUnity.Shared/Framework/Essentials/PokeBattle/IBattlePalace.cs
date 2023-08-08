using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Combat.Data;
using PokemonUnity.Character;
using PokemonUnity.Saving;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Utility;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattlePalace : IBattle
	{
		int[] BattlePalaceUsualTable { get; }
		int[] BattlePalacePinchTable { get; }

		//IBattlePalace initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		IBattlePalace initialize(PokemonEssentials.Interface.Screen.IPokeBattle_Scene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);

		/// <summary>
		/// </summary>
		/// <param name="move"></param>
		/// <returns>Returns Category?</returns>
		int MoveCategory(IBattleMove move);

		/// <summary>
		/// Different implementation of CanChooseMove, ignores Imprison/Torment/Taunt/Disable/Encore
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <param name="idxMove"></param>
		/// <returns></returns>
		bool CanChooseMovePartial(int idxPokemon, int idxMove);

		void PinchChange(int idxPokemon);

		bool EnemyShouldWithdraw(int index);

		new bool RegisterMove(int idxPokemon, int idxMove, bool showMessages = true);

		new bool AutoFightMenu(int idxPokemon);

		new void EndOfRoundPhase();
	}
}