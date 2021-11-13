using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonEssentials.Interface.PokeBattle
{
	/// <summary>
	/// Partial <seealso cref="IBattle"/>
	/// </summary>
	public interface IBattleAI
	{
		/// <summary>
		/// Get a score for each move being considered (trainer-owned Pokémon only).
		/// Moves with higher scores are more likely to be chosen.
		/// </summary>
		/// <param name="move"></param>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <param name="skill"></param>
		/// <returns></returns>
		int pbGetMoveScore(IBattleMove move, IBattler attacker, IBattler opponent, int skill = 100);

		#region Get type effectiveness and approximate stats.
		float pbTypeModifier(Types type, IBattler attacker, IBattler opponent);

		float pbTypeModifier2(IBattler battlerThis, IBattler battlerOther);

		int pbRoughStat(IBattler battler, Stats stat, int skill);

		int pbBetterBaseDamage(IBattleMove move, IBattler attacker, IBattler opponent, int skill, int basedamage);

		int pbRoughDamage(IBattleMove move, IBattler attacker, IBattler opponent, int skill, double basedamage);

		int pbRoughAccuracy(IBattleMove move, IBattler attacker, IBattler opponent, int skill);
		#endregion

		#region Choose a move to use.
		/// <summary>
		/// Choose a move to use.
		/// </summary>
		/// <param name="index"></param>
		void pbChooseMoves(int index);
		#endregion

		#region Decide whether the opponent should Mega Evolve their Pokémon.
		/// <summary>
		/// Decide whether the opponent should Mega Evolve their Pokémon.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool pbEnemyShouldMegaEvolve(int index);
		#endregion

		#region Decide whether the opponent should use an item on the Pokémon.
		bool pbEnemyShouldUseItem(int index);

		bool pbEnemyItemAlreadyUsed(int index, Items item, Items[] items);

		Items pbEnemyItemToUse(int index);
		#endregion

		#region Decide whether the opponent should switch Pokémon.
		bool pbEnemyShouldWithdraw(int index);

		bool pbEnemyShouldWithdrawEx(int index, bool alwaysSwitch);

		int pbDefaultChooseNewEnemy(int index, IBattler[] party);

		int pbChooseBestNewEnemy(int index, Pokemon[] party, int[] enemies);
		#endregion

		#region Choose an action.
		/// <summary>
		/// Choose an action.
		/// </summary>
		/// <param name="index"></param>
		void pbDefaultChooseEnemyCommand(int index);
		#endregion

		#region Other functions.
		bool pbDbgPlayerOnly(int idx);

		int pbStdDev(int[] scores);
		#endregion
	}
}