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
		int GetMoveScore(IBattleMove move, IBattler attacker, IBattler opponent, int skill = 100);

		#region Get type effectiveness and approximate stats.
		float TypeModifier(Types type, IBattler attacker, IBattler opponent);

		float TypeModifier2(IBattler battlerThis, IBattler battlerOther);

		int RoughStat(IBattler battler, Stats stat, int skill);

		int BetterBaseDamage(IBattleMove move, IBattler attacker, IBattler opponent, int skill, int basedamage);

		int RoughDamage(IBattleMove move, IBattler attacker, IBattler opponent, int skill, double basedamage);

		int RoughAccuracy(IBattleMove move, IBattler attacker, IBattler opponent, int skill);
		#endregion

		#region Choose a move to use.
		/// <summary>
		/// Choose a move to use.
		/// </summary>
		/// <param name="index"></param>
		void ChooseMoves(int index);
		#endregion

		#region Decide whether the opponent should Mega Evolve their Pokémon.
		/// <summary>
		/// Decide whether the opponent should Mega Evolve their Pokémon.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool EnemyShouldMegaEvolve(int index);
		#endregion

		#region Decide whether the opponent should use an item on the Pokémon.
		bool EnemyShouldUseItem(int index);

		bool EnemyItemAlreadyUsed(int index, Items item, Items[] items);

		Items EnemyItemToUse(int index);
		#endregion

		#region Decide whether the opponent should switch Pokémon.
		bool EnemyShouldWithdraw(int index);

		bool EnemyShouldWithdrawEx(int index, bool alwaysSwitch);

		int DefaultChooseNewEnemy(int index, IPokemon[] party);

		int ChooseBestNewEnemy(int index, IPokemon[] party, int[] enemies);
		#endregion

		#region Choose an action.
		/// <summary>
		/// Choose an action.
		/// </summary>
		/// <param name="index"></param>
		void DefaultChooseEnemyCommand(int index);
		#endregion

		#region Other functions.
		bool DbgPlayerOnly(int idx);

		int StdDev(int[] scores);
		#endregion
	}
}