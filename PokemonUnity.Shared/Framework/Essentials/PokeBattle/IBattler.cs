using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattler
	{
		IBattle battle { get; set; }
		IPokemon pokemon { get; set; }
		string name { get; set; }
		int index { get; set; }
		int pokemonIndex { get; set; }
		int totalhp { get; set; }
		int fainted { get; set; }
		int lastAttacker { get; set; }
		int turncount { get; set; }
		int effects { get; set; }
		int species { get; set; }
		Types type1 { get; set; }
		Types type2 { get; set; }
		Abilities ability { get; set; }
		int gender { get; set; }
		int attack { get; set; }
		int defense { get; set; }
		int spatk { get; set; }
		int spdef { get; set; }
		int speed { get; set; }
		int[] stages { get; set; }
		int[] iv { get; set; }
		IBattleMove[] moves { get; set; }
		int participants { get; set; }
		int tookDamage { get; set; }
		int lastHPLost { get; set; }
		int lastMoveUsed { get; set; }
		int lastMoveUsedType { get; set; }
		int lastMoveUsedSketch { get; set; }
		int lastRegularMoveUsed { get; set; }
		int lastRoundMoved { get; set; }
		int movesUsed { get; set; }
		int currentMove { get; set; }
		int damagestate { get; set; }
		int captured { get; set; }
		bool inHyperMode { get; }
		bool isShadow { get; }

		#region Complex accessors
		//int defense { get; }
		//int spdef { get; }
		PokemonUnity.Monster.Natures nature { get; }
		int happiness { get; }
		int pokerusStage { get; }
		int form { get; set; }
		bool hasMega { get; }
		bool isMega { get; }
		bool hasPrimal { get; }
		bool isPrimal { get; }
		int level { get; set; }
		int status { get; set; }
		int statusCount { get; set; }
		int hp { get; set; }
		Items item { get; set; }
		int weight(IBattler attacker = null);
		//string name { get; }
		int displayGender { get; }
		bool isShiny { get; }
		bool owned { get; }
		#endregion

		#region Creating a battler
		IBattler initialize(IBattle btl, int index);

		void pbInitPokemon(IPokemon pkmn, int pkmnIndex);

		void pbInitDummyPokemon(IPokemon pkmn, int pkmnIndex);

		void pbInitBlank();

		void pbInitPermanentEffects();

		void pbInitEffects(bool batonpass);

		void pbUpdate(bool fullchange = false);

		void pbInitialize(IPokemon pkmn, int index, bool batonpass);

		/// <summary>
		/// Used only to erase the battler of a Shadow Pokémon that has been snagged.
		/// </summary>
		bool pbReset();

		/// <summary>
		/// Update Pokémon who will gain EXP if this battler is defeated
		/// </summary>
		void pbUpdateParticipants();
		#endregion

		#region About this battler
		string pbThis(bool lowercase = false);

		bool pbHasType(Types type);

		bool pbHasMove(Moves id);

		bool pbHasMoveType(Types type);

		bool pbHasMoveFunction(int code);

		bool hasMovedThisRound();

		bool isFainted();

		bool hasMoldBreaker();

		bool hasWorkingAbility(Abilities ability, bool ignorefainted = false);

		bool hasWorkingItem(Items item, bool ignorefainted = false);

		bool isAirborne(bool ignoreability = false);

		int pbSpeed { get; }
		#endregion

		#region Change HP
		int pbReduceHP(int amt, bool anim = false, bool registerDamage = true);

		int pbRecoverHP(int amt, bool anim = false);

		bool pbFaint(bool showMessage = true);
		#endregion

		#region Find other battlers/sides in relation to this battler
		/// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// <returns>Player: 0 and 2; Foe: 1 and 3</returns>
		bool pbOwnSide();

		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// <returns>Player: 1 and 3; Foe: 0 and 2</returns>
		bool pbOpposingSide();

		/// <summary>
		/// Returns whether the position belongs to the opposing Pokémon's side
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		bool pbIsOpposing(int i);

		/// <summary>
		/// Returns the battler's partner
		/// </summary>
		/// <returns></returns>
		IBattler pbPartner();

		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler pbOpposing1();

		/// <summary>
		/// Returns the battler's second opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler pbOpposing2();

		IBattler pbOppositeOpposing();

		IBattler pbOppositeOpposing2();

		int pbNonActivePokemonCount();
		#endregion

		#region Forms
		void pbCheckForm();

		void pbResetForm();
		#endregion

		#region Ability effects
		void pbAbilitiesOnSwitchIn(bool onactive);

		void pbEffectsOnDealingDamage(IBattleMove move, IBattler user, IBattler target, int damage);

		void pbEffectsAfterHit(IBattler user, IBattler target, IBattleMove thismove, IEffectsMove turneffects);

		void pbAbilityCureCheck();
		#endregion

		#region Held item effects
		void pbConsumeItem(bool recycle = true, bool pickup = true);

		bool pbConfusionBerry(PokemonUnity.Inventory.Plants.Flavours flavor, string message1, string message2);

		bool pbStatIncreasingBerry(Stats stat, string berryname);

		void pbActivateBerryEffect(Items berry = Items.NONE, bool consume = true);

		void pbBerryCureCheck(bool hpcure = false);
		#endregion

		#region Move user and targets
		IBattler pbFindUser(IBattleChoice choice, IBattler targets);

		IBattler pbChangeUser(IBattleMove thismove, IBattler user);

		PokemonUnity.Attack.Data.Targets pbTarget(IBattleMove move);

		bool pbAddTarget(IBattler[] targets, IBattler target);

		void pbRandomTarget(IBattler[] targets);

		bool pbChangeTarget(IBattleMove thismove, IBattler[] userandtarget, IBattler[] targets);
		#endregion

		#region Move PP
		void pbSetPP(IBattleMove move, int pp);

		bool pbReducePP(IBattleMove move);

		void pbReducePPOther(IBattleMove move);
		#endregion

		#region Using a move
		bool pbObedienceCheck(IBattleChoice choice);

		bool pbSuccessCheck(IBattleMove thismove, IBattler user, IBattler target, IEffectsMove turneffects, bool accuracy = true);

		bool pbTryUseMove(IBattleChoice choice, IBattleMove thismove, IEffectsMove turneffects);

		void pbConfusionDamage();

		void pbUpdateTargetedMove(IBattleMove thismove, IBattler user);

		void pbProcessMoveAgainstTarget(IBattleMove thismove, IBattler user, IBattler target, int numhits, IEffectsMove turneffects, bool nocheck = false, int[] alltargets = null, bool showanimation = true);

		void pbUseMoveSimple(Moves moveid, int index = -1, int target = -1);

		void pbUseMove(IBattleChoice choice, bool specialusage = false);

		void pbCancelMoves();
		#endregion

		#region Turn processing
		void pbBeginTurn(IBattleChoice choice);

		void pbEndTurn(IBattleChoice choice);

		bool pbProcessTurn(IBattleChoice choice);
		#endregion
	}
}