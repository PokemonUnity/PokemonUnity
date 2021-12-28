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
		//  alias __clauses__pbDecisionOnDraw pbDecisionOnDraw;
		//  alias __clauses__pbEndOfRoundPhase pbEndOfRoundPhase;
		//  @__clauses__aliased=true;
		//}

		public virtual BattleResults pbDecisionOnDraw() { return (this as IBattleClause).pbDecisionOnDraw(); }
		BattleResults IBattleClause.pbDecisionOnDraw() {
			if (@rules[BattleRule.SELFKOCLAUSE]) {
				if (this.lastMoveUser<0) {
					// in extreme cases there may be no last move user
					return BattleResults.DRAW; // game is a draw
				} else if (isOpposing(this.lastMoveUser)) {
					return BattleResults.LOST; // loss
				} else {
					return BattleResults.WON; // win
				}
			}
			//return _pbDecisionOnDraw();
			return (this as IBattle).pbDecisionOnDraw();
		}

		public virtual void pbJudgeCheckpoint(IBattler attacker,IBattleMove move=null) {
			if (@rules["drawclause"]) {															// Note: Also includes Life Orb (not implemented)
				if (!(move.IsNotNullOrNone() && move.Effect==Attack.Data.Effects.x15A)) {		// Not a draw if fainting occurred due to Liquid Ooze
					if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
						@decision=isOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
					}
				}
			} else if (@rules[BattleRule.MODIFIEDSELFDESTRUCTCLAUSE] && move.IsNotNullOrNone() && 
				move.Effect==Attack.Data.Effects.x008) { // Selfdestruct
				if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
					@decision=isOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
				}
			}
		}

		public virtual void pbEndOfRoundPhase() { (this as IBattleClause).pbEndOfRoundPhase(); }
		void IBattleClause.pbEndOfRoundPhase() {
			//_pbEndOfRoundPhase();
			(this as IBattle).pbEndOfRoundPhase();
			if (@rules[BattleRule.SUDDENDEATH] && @decision==BattleResults.ABORTED) {
				if (pbPokemonCount(@party1)>pbPokemonCount(@party2)) {
					@decision=BattleResults.LOST; // loss
				} else if (pbPokemonCount(@party1)<pbPokemonCount(@party2)) {
					@decision=BattleResults.WON; // win
				}
			}
		}
	}

	public partial class Pokemon : IBattlerClause
	{
		//unless (@__clauses__aliased) {
		//  alias __clauses__pbCanSleep? pbCanSleep?;
		//  alias __clauses__pbCanSleepYawn? pbCanSleepYawn?;
		//  alias __clauses__pbCanFreeze? pbCanFreeze?;
		//  alias __clauses__pbUseMove pbUseMove;
		//  @__clauses__aliased=true;
		//}

		public bool pbHasStatusPokemon (Status status) {
			int count=0;
			IPokemon[] party=@battle.pbParty(this.Index);
			for (int i = 0; i < party.Length; i++) {
				if (party[i].IsNotNullOrNone() && !party[i].isEgg &&
					party[i].Status==status) {
					count+=1;
				}
			}
			return (count>0);
		}

		public bool pbCanSleepYawn() { return (this as IBattlerClause).pbCanSleepYawn(); }
		bool IBattlerClause.pbCanSleepYawn() {
			if ((@battle.rules[BattleRule.SLEEPCLAUSE] || @battle.rules[BattleRule.MODIFIEDSLEEPCLAUSE]) && 
				pbHasStatusPokemon(Status.SLEEP)) {
				return false;
			}
			//return _pbCanSleepYawn();
			return (this as IBattlerEffect).pbCanSleepYawn();
		}

		public bool pbCanFreeze (IBattler attacker,bool showMessages,IBattleMove move=null) { return (this as IBattlerClause).pbCanFreeze(attacker,showMessages,move); }
		bool IBattlerClause.pbCanFreeze(IBattler attacker,bool showMessages,IBattleMove move) {
			if (@battle.rules[BattleRule.FREEZECLAUSE] && pbHasStatusPokemon(Status.FROZEN)) {
				return false;
			}
			//return _pbCanFreeze(attacker,showMessages,move);
			return (this as IBattlerEffect).pbCanFreeze(attacker,showMessages,move);
		}

		public bool pbCanSleep (IBattler attacker,bool showMessages,IBattleMove move=null,bool ignorestatus=false) { return (this as IBattlerClause).pbCanSleep(attacker,showMessages,move,ignorestatus); }
		bool IBattlerClause.pbCanSleep(IBattler attacker,bool showMessages,IBattleMove move,bool ignorestatus) {
			bool selfsleep=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
			if (((@battle.rules[BattleRule.MODIFIEDSLEEPCLAUSE]) || (!selfsleep && @battle.rules[BattleRule.SLEEPCLAUSE])) && 
				pbHasStatusPokemon(Status.SLEEP)) {
				if (showMessages) {
					@battle.pbDisplay(Game._INTL("But {1} couldn't sleep!",this.ToString(true)));
				}
				return false;
			}
			//return _pbCanSleep(attacker,showMessages,move,ignorestatus);
			return (this as IBattlerEffect).pbCanSleep(attacker,showMessages,move,ignorestatus);
		}
	}

	#region Rules that Override Move Effect
	public partial class PokeBattle_Move_022 : IBattleMove_MoveFailed { // Double Team
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.EVASIONCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_034 : IBattleMove_MoveFailed { // Minimize
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.EVASIONCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_067 : IBattleMove_MoveFailed { // Skill Swap
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SKILLSWAPCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06A : IBattleMove_MoveFailed { // Sonicboom
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SONICBOOMCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06B : IBattleMove_MoveFailed { // Dragon Rage
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.SONICBOOMCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_070 : IBattleMove_MoveFailed { // OHKO moves
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.OHKOCLAUSE]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_0E0 : IBattleMove_MoveFailed { // Selfdestruct
		//unless (@__clauses__aliased) {
		//  alias __clauses__pbOnStartUse pbOnStartUse;
		//  @__clauses__aliased=true;
		//}

		public override bool pbOnStartUse(IBattler attacker) {
			if (@battle.rules[BattleRule.SELFKOCLAUSE]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.pbNonActivePokemonCount;
				count+=attacker.pbOppositeOpposing.pbNonActivePokemonCount;
				if (count==0) {
					@battle.pbDisplay("But it failed!");
					return false;
				}
			}
			if (@battle.rules[BattleRule.SELFDESTRUCTCLAUSE]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.pbNonActivePokemonCount;
				count+=attacker.pbOppositeOpposing.pbNonActivePokemonCount;
				if (count==0) {
					@battle.pbDisplay(Game._INTL("{1}'s team was disqualified!",attacker.ToString()));
					@battle.decision=@battle.pbIsOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
					return false;
				}
			}
			return (this as IBattleMove).pbOnStartUse(attacker);
		}
	}

	public partial class PokeBattle_Move_0E5 : IBattleMove_MoveFailed { // Perish Song
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.PERISHSONGCLAUSE] && attacker.pbNonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}

	public partial class PokeBattle_Move_0E7 : IBattleMove_MoveFailed { // Destiny Bond
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules[BattleRule.PERISHSONGCLAUSE] && attacker.pbNonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}
	#endregion
}