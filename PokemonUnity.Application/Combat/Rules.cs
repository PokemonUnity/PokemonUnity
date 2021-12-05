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
			if (@rules["selfkoclause"]) {
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
			} else if (@rules["modifiedselfdestructclause"] && move.IsNotNullOrNone() && 
				move.Effect==Attack.Data.Effects.x008) { // Selfdestruct
				if (pbAllFainted(@party1) && pbAllFainted(@party2)) {
					@decision=isOpposing(@attacker.Index) ? BattleResults.WON : BattleResults.LOST;
				}
			}
		}

		public virtual void pbEndOfRoundPhase() { return (this as IBattleClause).pbEndOfRoundPhase(); }
		void IBattleClause.pbEndOfRoundPhase() {
			//_pbEndOfRoundPhase();
			(this as IBattle).pbEndOfRoundPhase();
			if (@rules["suddendeath"] && @decision==BattleResults.ABORTED) {
				if (pbPokemonCount(@party1)>pbPokemonCount(@party2)) {
					@decision=BattleResults.LOST; // loss
				} else if (pbPokemonCount(@party1)<pbPokemonCount(@party2)) {
					@decision=BattleResults.WON; // win
				}
			}
		}
	}


	public partial class Pokemon {
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

		public bool pbCanSleepYawn() {
			if ((@battle.rules["sleepclause"] || @battle.rules["modifiedsleepclause"]) && 
				pbHasStatusPokemon(Status.SLEEP)) {
				return false;
			}
			return _pbCanSleepYawn();
		}

		public bool pbCanFreeze (IBattler attacker,bool showMessages,IBattleMove move=null) {
			if (@battle.rules["freezeclause"] && pbHasStatusPokemon(Status.FROZEN)) {
				return false;
			}
			return _pbCanFreeze(attacker,showMessages,move);
		}

		public bool pbCanSleep (IBattler attacker,bool showMessages,IBattleMove move=null,bool ignorestatus=false) {
			bool selfsleep=(attacker.IsNotNullOrNone() && attacker.Index==this.Index);
			if (((@battle.rules["modifiedsleepclause"]) || (!selfsleep && @battle.rules["sleepclause"])) && 
				pbHasStatusPokemon(Status.SLEEP)) {
				if (showMessages) {
				@battle.pbDisplay(Game._INTL("But {1} couldn't sleep!",this.ToString(true)));
				}
				return false;
			}
			return _pbCanSleep(attacker,showMessages,move,ignorestatus);
		}
	}


	#region Rules that Override Move Effect
	public partial class PokeBattle_Move_022 { // Double Team
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["evasionclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_034 { // Minimize
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["evasionclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_067 { // Skill Swap
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["skillswapclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06A { // Sonicboom
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["sonicboomclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_06B { // Dragon Rage
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["sonicboomclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_070 { // OHKO moves
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["ohkoclause"]) return true;
			return false;
		}
	}

	public partial class PokeBattle_Move_0E0 { // Selfdestruct
		//unless (@__clauses__aliased) {
		//  alias __clauses__pbOnStartUse pbOnStartUse;
		//  @__clauses__aliased=true;
		//}

		public override bool pbOnStartUse(IBattler attacker) {
			if (@battle.rules["selfkoclause"]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.NonActivePokemonCount;
				count+=attacker.pbOppositeOpposing.NonActivePokemonCount;
				if (count==0) {
					@battle.pbDisplay("But it failed!");
					return false;
				}
			}
			if (@battle.rules["selfdestructclause"]) {
				// Check whether no unfainted Pokemon remain in either party
				int count=attacker.NonActivePokemonCount;
				count+=attacker.pbOppositeOpposing.NonActivePokemonCount;
				if (count==0) {
					@battle.pbDisplay(Game._INTL("{1}'s team was disqualified!",attacker.ToString()));
					@battle.decision=@battle.isOpposing(attacker.Index) ? BattleResults.WON : BattleResults.LOST;
					return false;
				}
			}
			return _pbOnStartUse(attacker);
		}
	}

	public partial class PokeBattle_Move_0E5 { // Perish Song
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["perishsongclause"] && attacker.NonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}

	public partial class PokeBattle_Move_0E7 { // Destiny Bond
		public override bool pbMoveFailed(IBattler attacker,IBattler opponent) {
			if (@battle.rules["perishsongclause"] && attacker.NonActivePokemonCount==0) {
				return true;
			}
			return false;
		}
	}
	#endregion
}