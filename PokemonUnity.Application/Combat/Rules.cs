using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Combat;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Combat
{
	// ===============================================================================
	// This script modifies the battle system to implement battle rules.
	// ===============================================================================

	public partial class Battle : IBattleClause {
		//unless (@__clauses__aliased) {
		//  alias __clauses__DecisionOnDraw DecisionOnDraw;
		//  alias __clauses__EndOfRoundPhase EndOfRoundPhase;
		//  @__clauses__aliased=true;
		//}

		public virtual BattleResults DecisionOnDraw() { return (this as IBattleClause).DecisionOnDraw(); }
		BattleResults IBattleClause.DecisionOnDraw() {
			if (@rules[BattleRule.SELFKOCLAUSE]) {
				if (this.lastMoveUser<0) {
					// in extreme cases there may be no last move user
					return BattleResults.DRAW; // game is a draw
				} else if (IsOpposing(this.lastMoveUser)) {
					return BattleResults.LOST; // loss
				} else {
					return BattleResults.WON; // win
				}
			}
			//return _DecisionOnDraw();
			return (this as IBattle).DecisionOnDraw();
		}

		public virtual void JudgeCheckpoint(IBattler attacker,IBattleMove move=null) {
			if (@rules["drawclause"]) {															// Note: Also includes Life Orb (not implemented)
				if (!(move.IsNotNullOrNone() && move.Effect==Attack.Effects.x15A)) {		// Not a draw if fainting occurred due to Liquid Ooze
					if (AllFainted(@party1) && AllFainted(@party2)) {
						@decision=IsOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
					}
				}
			} else if (@rules[BattleRule.MODIFIEDSELFDESTRUCTCLAUSE] && move.IsNotNullOrNone() && 
				move.Effect==Attack.Effects.x008) { // Selfdestruct
				if (AllFainted(@party1) && AllFainted(@party2)) {
					@decision=IsOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
				}
			}
		}

		public virtual void EndOfRoundPhase() { (this as IBattleClause).EndOfRoundPhase(); }
		void IBattleClause.EndOfRoundPhase() {
			//_EndOfRoundPhase();
			(this as IBattle).EndOfRoundPhase();
			if (@rules[BattleRule.SUDDENDEATH] && @decision==BattleResults.ABORTED) {
				if (PokemonCount(@party1)>PokemonCount(@party2)) {
					@decision=BattleResults.LOST; // loss
				} else if (PokemonCount(@party1)<PokemonCount(@party2)) {
					@decision=BattleResults.WON; // win
				}
			}
		}
	}

	public partial class Pokemon : IBattlerClause
	{
		//unless (@__clauses__aliased) {
		//  alias __clauses__CanSleep? CanSleep?;
		//  alias __clauses__CanSleepYawn? CanSleepYawn?;
		//  alias __clauses__CanFreeze? CanFreeze?;
		//  alias __clauses__UseMove UseMove;
		//  @__clauses__aliased=true;
		//}

		public bool HasStatusPokemon (Status status) {
			int count=0;
			IPokemon[] party=@battle.Party(this.Index);
			for (int i = 0; i < party.Length; i++) {
				if (party[i].IsNotNullOrNone() && !party[i].isEgg &&
					party[i].Status==status) {
					count+=1;
				}
			}
			return (count>0);
		}

		public bool CanSleepYawn() { return (this as IBattlerClause).CanSleepYawn(); }
		bool IBattlerClause.CanSleepYawn() {
			if ((@battle.rules[BattleRule.SLEEPCLAUSE] || @battle.rules[BattleRule.MODIFIEDSLEEPCLAUSE]) && 
				HasStatusPokemon(Status.SLEEP)) {
				return false;
			}
			//return _CanSleepYawn();
			return (this as IBattlerEffect).CanSleepYawn();
		}

		public bool CanFreeze (IBattler attacker,bool showMessages,IBattleMove move=null) { return (this as IBattlerClause).CanFreeze(attacker,showMessages,move); }
		bool IBattlerClause.CanFreeze(IBattler attacker,bool showMessages,IBattleMove move) {
			if (@battle.rules[BattleRule.FREEZECLAUSE] && HasStatusPokemon(Status.FROZEN)) {
				return false;
			}
			//return _CanFreeze(attacker,showMessages,move);
			return (this as IBattlerEffect).CanFreeze(attacker,showMessages,move);
		}

		public bool CanSleep (IBattler attacker,bool showMessages,IBattleMove move=null,bool ignorestatus=false) { return (this as IBattlerClause).CanSleep(attacker,showMessages,move,ignorestatus); }
		bool IBattlerClause.CanSleep(IBattler attacker,bool showMessages,IBattleMove move,bool ignorestatus) {
			bool selfsleep=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
			if (((@battle.rules[BattleRule.MODIFIEDSLEEPCLAUSE]) || (!selfsleep && @battle.rules[BattleRule.SLEEPCLAUSE])) && 
				HasStatusPokemon(Status.SLEEP)) {
				if (showMessages) {
					@battle.Display(Game._INTL("But {1} couldn't sleep!",this.ToString(true)));
				}
				return false;
			}
			//return _CanSleep(attacker,showMessages,move,ignorestatus);
			return (this as IBattlerEffect).CanSleep(attacker,showMessages,move,ignorestatus);
		}
	}

	#region Rules that Override Move Effect
	public partial class PokeBattle_Move_022 : IBattleMove_MoveFailed { // Double Team
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.EVASIONCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_034 : IBattleMove_MoveFailed { // Minimize
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.EVASIONCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_067 : IBattleMove_MoveFailed { // Skill Swap
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SKILLSWAPCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06A : IBattleMove_MoveFailed { // Sonicboom
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SONICBOOMCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06B : IBattleMove_MoveFailed { // Dragon Rage
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SONICBOOMCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_070 : IBattleMove_MoveFailed { // OHKO moves
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.OHKOCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_0E0 : IBattleMove_MoveFailed { // Selfdestruct
		//unless (@__clauses__aliased) {
		//  alias __clauses__OnStartUse OnStartUse;
		//  @__clauses__aliased=true;
		//}

		public override bool OnStartUse(IBattler attacker) {
			if (@battle.rules[BattleRule.SELFKOCLAUSE]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.NonActivePokemonCount;
				count+=attacker.OppositeOpposing.NonActivePokemonCount;
				if (count==0) {
					@battle.Display("But it failed!");
					return false;
				}
			}
			if (@battle.rules[BattleRule.SELFDESTRUCTCLAUSE]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.NonActivePokemonCount;
				count+=attacker.OppositeOpposing.NonActivePokemonCount;
				if (count==0) {
					@battle.Display(Game._INTL("{1}'s team was disqualified!",attacker.ToString()));
					@battle.decision=@battle.IsOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
					return false;
				}
			}
			return (this as IBattleMove).OnStartUse(attacker);
		}
	}

	public partial class PokeBattle_Move_0E5 : IBattleMove_MoveFailed { // Perish Song
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.PERISHSONGCLAUSE] && attacker.NonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}

	public partial class PokeBattle_Move_0E7 : IBattleMove_MoveFailed { // Destiny Bond
		public override bool MoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.PERISHSONGCLAUSE] && attacker.NonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}
	#endregion
}