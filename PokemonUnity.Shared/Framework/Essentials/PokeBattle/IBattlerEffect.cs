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
	/// Partial extension of existing entity <seealso cref="IBattler"/>
	/// </summary>
	public interface IBattlerEffect
	{
		#region Sleep
		bool pbCanSleep(IBattler attacker, bool showMessages, IBattleMove move = null, bool ignorestatus = false);

		bool pbCanSleepYawn();

		void pbSleep(string msg = null);

		void pbSleepSelf(int duration = -1);
		#endregion

		#region Poison
		bool pbCanPoison(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool pbCanPoisonSynchronize(IBattler opponent);

		bool pbCanPoisonSpikes(bool moldbreaker = false);

		void pbPoison(IBattler attacker, string msg = null, bool toxic = false);
		#endregion

		#region Burn
		bool pbCanBurn(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool pbCanBurnSynchronize(IBattler opponent);

		void pbBurn(IBattler attacker, string msg = null);
		#endregion

		#region Paralyze
		bool pbCanParalyze(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool pbCanParalyzeSynchronize(IBattler opponent);

		void pbParalyze(IBattler attacker, string msg = null);
		#endregion

		#region Freeze
		bool pbCanFreeze(IBattler attacker, bool showMessages, IBattleMove move = null);

		void pbFreeze(string msg = null);
		#endregion

		#region Generalised status displays
		void pbContinueStatus(bool showAnim= true);

		void pbCureStatus(bool showMessages= true);
		#endregion

		#region Confuse
		bool pbCanConfuse(IBattler attacker = null, bool showMessages = true, IBattleMove move = null);

		bool pbCanConfuseSelf(bool showMessages);

		void pbConfuse();

		void pbConfuseSelf();

		void pbContinueConfusion();

		void pbCureConfusion(bool showMessages = true);
		#endregion

		#region Attraction
		bool pbCanAttract(IBattler attacker, bool showMessages = true);

		void pbAttract(IBattler attacker, string msg = null);

		void pbAnnounceAttract(IBattler seducer);

		void pbContinueAttract();

		void pbCureAttract();
		#endregion

		#region Flinching
		void pbFlinch(IBattler attacker);
		#endregion

		#region Increase stat stages
		bool pbTooHigh(Stats stat);

		bool pbCanIncreaseStatStage(Stats stat, IBattler attacker = null, bool showMessages = false, IBattleMove move = null, bool moldbreaker = false, bool ignoreContrary = false);

		int pbIncreaseStatBasic(Stats stat, int increment, IBattler attacker = null, bool moldbreaker = false, bool ignoreContrary = false);

		bool pbIncreaseStat(Stats stat, int increment, IBattler attacker, bool showMessages, IBattleMove move = null, bool upanim = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool pbIncreaseStatWithCause(Stats stat, int increment, IBattler attacker, string cause, bool showanim = true, bool showmessage = true, bool moldbreaker = false, bool ignoreContrary = false);
		#endregion

		#region Decrease stat stages
		bool pbTooLow(Stats stat);

		/// <summary>
		/// Tickle (<see cref="Moves.TICKLE"/>) and Noble Roar (<see cref="Moves.NOBLE_ROAR"/>) can't use this, but replicate it instead.
		/// (Reason is they lowers more than 1 stat independently, and therefore could
		/// show certain messages twice which is undesirable.)
		/// </summary>
		/// <param name="stat"></param>
		/// <param name="attacker"></param>
		/// <param name="showMessages"></param>
		/// <param name="move"></param>
		/// <param name="moldbreaker"></param>
		/// <param name="ignoreContrary"></param>
		/// <returns></returns>
		bool pbCanReduceStatStage(Stats stat, IBattler attacker = null, bool showMessages = false, IBattleMove move = null, bool moldbreaker = false, bool ignoreContrary = false);

		int pbReduceStatBasic(Stats stat, int increment, IBattler attacker = null, bool moldbreaker = false, bool ignoreContrary = false);

		bool pbReduceStat(Stats stat, int increment, IBattler attacker, bool showMessages, IBattleMove move = null, bool downanim = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool pbReduceStatWithCause(Stats stat, int increment, IBattler attacker, string cause, bool showanim = true, bool showmessage = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool pbReduceAttackStatIntimidate(IBattler opponent);
		#endregion
	}
}