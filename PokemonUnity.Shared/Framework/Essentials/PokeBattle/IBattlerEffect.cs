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
		bool CanSleep(IBattler attacker, bool showMessages, IBattleMove move = null, bool ignorestatus = false);

		bool CanSleepYawn();

		void Sleep(string msg = null);

		void SleepSelf(int duration = -1);
		#endregion

		#region Poison
		bool CanPoison(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool CanPoisonSynchronize(IBattler opponent);

		bool CanPoisonSpikes(bool moldbreaker = false);

		void Poison(IBattler attacker, string msg = null, bool toxic = false);
		#endregion

		#region Burn
		bool CanBurn(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool CanBurnSynchronize(IBattler opponent);

		void Burn(IBattler attacker, string msg = null);
		#endregion

		#region Paralyze
		bool CanParalyze(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool CanParalyzeSynchronize(IBattler opponent);

		void Paralyze(IBattler attacker, string msg = null);
		#endregion

		#region Freeze
		bool CanFreeze(IBattler attacker, bool showMessages, IBattleMove move = null);

		void Freeze(string msg = null);
		#endregion

		#region Generalised status displays
		void ContinueStatus(bool showAnim= true);

		void CureStatus(bool showMessages= true);
		#endregion

		#region Confuse
		bool CanConfuse(IBattler attacker = null, bool showMessages = true, IBattleMove move = null);

		bool CanConfuseSelf(bool showMessages);

		void Confuse();

		void ConfuseSelf();

		void ContinueConfusion();

		void CureConfusion(bool showMessages = true);
		#endregion

		#region Attraction
		bool CanAttract(IBattler attacker, bool showMessages = true);

		void Attract(IBattler attacker, string msg = null);

		void AnnounceAttract(IBattler seducer);

		void ContinueAttract();

		void CureAttract();
		#endregion

		#region Flinching
		bool Flinch(IBattler attacker);
		#endregion

		#region Increase stat stages
		bool TooHigh(Stats stat);

		bool CanIncreaseStatStage(Stats stat, IBattler attacker = null, bool showMessages = false, IBattleMove move = null, bool moldbreaker = false, bool ignoreContrary = false);

		int IncreaseStatBasic(Stats stat, int increment, IBattler attacker = null, bool moldbreaker = false, bool ignoreContrary = false);

		bool IncreaseStat(Stats stat, int increment, IBattler attacker, bool showMessages, IBattleMove move = null, bool upanim = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool IncreaseStatWithCause(Stats stat, int increment, IBattler attacker, string cause, bool showanim = true, bool showmessage = true, bool moldbreaker = false, bool ignoreContrary = false);
		#endregion

		#region Decrease stat stages
		bool TooLow(Stats stat);

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
		bool CanReduceStatStage(Stats stat, IBattler attacker = null, bool showMessages = false, IBattleMove move = null, bool moldbreaker = false, bool ignoreContrary = false);

		int ReduceStatBasic(Stats stat, int increment, IBattler attacker = null, bool moldbreaker = false, bool ignoreContrary = false);

		bool ReduceStat(Stats stat, int increment, IBattler attacker, bool showMessages, IBattleMove move = null, bool downanim = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool ReduceStatWithCause(Stats stat, int increment, IBattler attacker, string cause, bool showanim = true, bool showmessage = true, bool moldbreaker = false, bool ignoreContrary = false);

		bool ReduceAttackStatIntimidate(IBattler opponent);
		#endregion
	}
}