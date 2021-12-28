using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattler
	{
		IBattle battle				{ get; set; }
		IPokemon pokemon			{ get; set; }
		string Name					{ get; set; }
		int Index					{ get; set; }
		int pokemonIndex			{ get; set; }
		int TotalHP					{ get; set; }
		bool fainted				{ get; set; }
		IList<int> lastAttacker		{ get; set; }
		int turncount				{ get; set; }
		IEffectsBattler effects		{ get; set; }
		Pokemons Species			{ get; set; }
		Types Type1					{ get; set; }
		Types Type2					{ get; set; }
		Abilities Ability			{ get; set; }
		int gender					{ get; set; }
		int attack					{ get; set; }
		int defense					{ get; set; }
		int spatk					{ get; set; }
		int spdef					{ get; set; }
		int speed					{ get; set; }
		int[] stages				{ get; }
		int[] IV					{ get; }
		IBattleMove[] moves			{ get; }
		IList<int> participants		{ get; set; }
		int tookDamage				{ get; set; }
		int lastHPLost				{ get; set; }
		Moves lastMoveUsed			{ get; set; }
		Types lastMoveUsedType		{ get; set; }
		Moves lastMoveUsedSketch	{ get; set; }
		Moves lastRegularMoveUsed	{ get; set; }
		int? lastRoundMoved			{ get; }
		IList<Moves> movesUsed		{ get; }
		Moves currentMove			{ get; set; }
		IDamageState damagestate	{ get; set; }
		bool captured				{ get; set; }
		bool inHyperMode			{ get; }
		bool isShadow				{ get; }

		#region Complex accessors
		//int defense				{ get; }
		//int spdef					{ get; }
		Natures nature				{ get; }
		int happiness				{ get; }
		int pokerusStage			{ get; }
		int form					{ get; set; }
		bool hasMega				{ get; }
		bool isMega					{ get; }
		bool hasPrimal				{ get; }
		bool isPrimal				{ get; }
		int Level					{ get; set; }
		Status Status				{ get; set; }
		int StatusCount				{ get; set; }
		int HP						{ get; set; }
		Items Item					{ get; set; }
		int weight(IBattler attacker = null);
		//string name				{ get; }
		int displayGender			{ get; }
		bool isShiny				{ get; }
		bool owned					{ get; }
		#endregion

		#region Creating a battler
		IBattler initialize(IBattle btl, int index);

		void pbInitPokemon(IPokemon pkmn, int pkmnIndex);

		void pbInitDummyPokemon(IPokemon pkmn, int pkmnIndex);

		void pbInitBlank();

		void pbInitPermanentEffects();

		void pbInitEffects(bool batonpass);

		void pbUpdate(bool fullchange = false);

		IBattler pbInitialize(IPokemon pkmn, int index, bool batonpass);

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
		string ToString(bool lowercase = false);

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
		IEffectsSide pbOwnSide { get; }

		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// <returns>Player: 1 and 3; Foe: 0 and 2</returns>
		IEffectsSide pbOpposingSide { get; }

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
		IBattler pbPartner { get; }

		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler pbOpposing1 { get; }

		/// <summary>
		/// Returns the battler's second opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler pbOpposing2 { get; }

		IBattler pbOppositeOpposing { get; }

		IBattler pbOppositeOpposing2 { get; }

		int pbNonActivePokemonCount { get; }
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

		bool pbStatIncreasingBerry(PokemonUnity.Combat.Stats stat, string berryname);

		void pbActivateBerryEffect(Items berry = Items.NONE, bool consume = true);

		void pbBerryCureCheck(bool hpcure = false);
		#endregion

		#region Move user and targets
		IBattler pbFindUser(IBattleChoice choice, IBattler[] targets);

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