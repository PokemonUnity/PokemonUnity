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
		//alias __clauses__pbDecisionOnDraw pbDecisionOnDraw;
		//alias __clauses__pbEndOfRoundPhase pbEndOfRoundPhase;
		//@__clauses__aliased = true;

		BattleResults pbDecisionOnDraw();

		void pbJudgeCheckpoint(IBattler attacker, IBattleMove move= null);

		void pbEndOfRoundPhase();
	}

	/// <summary>
	/// Partial <seealso cref="IBattler"/>
	/// </summary>
	public interface IBattlerClause
	{
		//alias __clauses__pbCanSleep? pbCanSleep ?;
		//alias __clauses__pbCanSleepYawn? pbCanSleepYawn ?;
		//alias __clauses__pbCanFreeze? pbCanFreeze ?;
		//alias __clauses__pbUseMove pbUseMove;
		//@__clauses__aliased = true;

		bool pbHasStatusPokemon(Status status);

		bool pbCanSleepYawn();

		bool pbCanFreeze(IBattler attacker, bool showMessages, IBattleMove move = null);

		bool pbCanSleep(IBattler attacker, bool showMessages, IBattleMove move= null, bool ignorestatus= false);
	}

	public interface IBattleMove_MoveFailed
	{
		/// <summary>
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	/*public interface IPokeBattle_Move_022
	{
		/// <summary>
		/// Double Team
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_034
	{
		/// <summary>
		/// Minimize
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_067
	{
		/// <summary>
		/// Skill Swap
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_06A
	{
		/// <summary>
		/// Sonicboom
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_06B
	{
		/// <summary>
		/// Dragon Rage
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_070
	{
		/// <summary>
		/// OHKO moves
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="opponent"></param>
		/// <returns></returns>
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_0E0
	{ 
		//alias __clauses__pbOnStartUse pbOnStartUse;
		//@__clauses__aliased = true;
		// Selfdestruct

		bool pbOnStartUse(IBattler attacker);
	}

	public interface IPokeBattle_Move_0E5
	{ 
		// Perish Song
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}

	public interface IPokeBattle_Move_0E7
	{ 
		// Destiny Bond
		bool pbMoveFailed(IBattler attacker, IBattler opponent);
	}*/
}