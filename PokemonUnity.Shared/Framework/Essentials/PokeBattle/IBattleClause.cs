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
	/// This script modifies the battle system to implement battle rules.
	/// </summary>
	public interface IBattleClause
	{
		//alias __clauses__DecisionOnDraw DecisionOnDraw;
		//alias __clauses__EndOfRoundPhase EndOfRoundPhase;
		//@__clauses__aliased = true;

		BattleResults DecisionOnDraw();

		void JudgeCheckpoint(IBattler attacker, IBattleMove move= null);

		void EndOfRoundPhase();
	}

	/// <summary>
	/// Partial <seealso cref="IBattler"/>
	/// </summary>
	public interface IBattlerClause
	{
		//alias __clauses__CanSleep? CanSleep ?;
		//alias __clauses__CanSleepYawn? CanSleepYawn ?;
		//alias __clauses__CanFreeze? CanFreeze ?;
		//alias __clauses__UseMove UseMove;
		//@__clauses__aliased = true;

		bool HasStatusPokemon(Status status);

		bool CanSleepYawn();

		bool CanFreeze(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool CanSleep(IBattler attacker, bool showMessages, IBattleMove move= null, bool ignorestatus= false);
	}

	public interface IBattleMove_MoveFailed
	{
		/// <summary>
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	/*public interface IPokeBattle_Move_022
	{
		/// <summary>
		/// Double Team
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_034
	{
		/// <summary>
		/// Minimize
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_067
	{
		/// <summary>
		/// Skill Swap
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_06A
	{
		/// <summary>
		/// Sonicboom
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_06B
	{
		/// <summary>
		/// Dragon Rage
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_070
	{
		/// <summary>
		/// OHKO moves
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_0E0
	{
		//alias __clauses__OnStartUse OnStartUse;
		//@__clauses__aliased = true;
		// Selfdestruct

		bool OnStartUse(IBattler attacker);
	}

	public interface IPokeBattle_Move_0E5
	{
		// Perish Song
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_0E7
	{
		// Destiny Bond
		bool MoveFailed(IBattler attacker, IBattler opponent);
	}*/
}