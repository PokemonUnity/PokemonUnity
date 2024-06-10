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
		IBattle battle				{ get; }
		IPokemon pokemon			{ get; }
		string Name					{ get; set; }
		int Index					{ get; }
		int pokemonIndex			{ get; set; }
		int TotalHP					{ get; }
		//bool Fainted				{ get; }
		IList<int> lastAttacker		{ get; }
		int turncount				{ get; set; }
		IEffectsBattler effects		{ get; }
		Pokemons Species			{ get; }
		Types Type1					{ get; set; }
		Types Type2					{ get; set; }
		Abilities Ability			{ get; set; } //ToDo: use a Func to replace setter?
		bool? Gender				{ get; set; }
		int ATK						{ get; set; }
		int DEF						{ get; set; }
		int SPA						{ get; set; }
		int SPD						{ get; set; }
		int SPE						{ get; set; }
		int[] stages				{ get; }
		int[] IV					{ get; }
		IBattleMove[] moves			{ get; }
		IList<int> participants		{ get; set; }
		bool tookDamage				{ get; set; }
		int lastHPLost				{ get; set; }
		Moves lastMoveUsed			{ get; set; }
		Types lastMoveUsedType		{ get; }
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
		Natures Nature				{ get; }
		int Happiness				{ get; }
		//int pokerusStage			{ get; }
		bool? PokerusStage			{ get; }
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
		float Weight(IBattler attacker = null);
		//string name				{ get; }
		int displayGender			{ get; }
		bool IsShiny				{ get; }
		bool owned					{ get; }
		#endregion

		#region Creating a battler
		IBattler initialize(IBattle btl, int index);

		void InitPokemon(IPokemon pkmn, int pkmnIndex);

		void InitDummyPokemon(IPokemon pkmn, int pkmnIndex);

		void InitBlank();

		void InitPermanentEffects();

		void InitEffects(bool batonpass);

		void Update(bool fullchange = false);

		IBattler Initialize(IPokemon pkmn, int index, bool batonpass);

		/// <summary>
		/// Used only to erase the battler of a Shadow Pokémon that has been snagged.
		/// </summary>
		IBattler Reset();

		/// <summary>
		/// Update Pokémon who will gain EXP if this battler is defeated
		/// </summary>
		void UpdateParticipants();
		#endregion

		#region About this battler
		string ToString(bool lowercase = false);

		bool HasType(Types type);

		bool HasMove(Moves id);

		bool HasMoveType(Types type);

		bool HasMoveFunction(int code);

		bool hasMovedThisRound();

		bool isFainted();

		bool hasMoldBreaker();

		bool hasWorkingAbility(Abilities ability, bool ignorefainted = false);

		bool hasWorkingItem(Items item, bool ignorefainted = false);

		bool isAirborne(bool ignoreability = false);

		int Speed { get; }
		#endregion

		#region Change HP
		int ReduceHP(int amt, bool anim = false, bool registerDamage = true);

		int RecoverHP(int amt, bool anim = false);

		bool Faint(bool showMessage = true);
		#endregion

		#region Find other battlers/sides in relation to this battler
		/// <summary>
		/// Returns the data structure for this battler's side
		/// </summary>
		/// <returns>Player: 0 and 2; Foe: 1 and 3</returns>
		IEffectsSide OwnSide { get; }

		/// <summary>
		/// Returns the data structure for the opposing Pokémon's side
		/// </summary>
		/// <returns>Player: 1 and 3; Foe: 0 and 2</returns>
		IEffectsSide OpposingSide { get; }

		/// <summary>
		/// Returns whether the position belongs to the opposing Pokémon's side
		/// </summary>
		/// <param name="i"></param>
		/// <returns></returns>
		bool IsOpposing(int i);

		/// <summary>
		/// Returns the battler's partner
		/// </summary>
		/// <returns></returns>
		IBattler Partner { get; }

		/// <summary>
		/// Returns the battler's first opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler Opposing1 { get; }

		/// <summary>
		/// Returns the battler's second opposing Pokémon
		/// </summary>
		/// <returns></returns>
		IBattler Opposing2 { get; }

		IBattler OppositeOpposing { get; }

		IBattler OppositeOpposing2 { get; }

		int NonActivePokemonCount { get; }
		#endregion

		#region Forms
		void CheckForm();

		void ResetForm();
		#endregion

		#region Ability effects
		void AbilitiesOnSwitchIn(bool onactive);

		void EffectsOnDealingDamage(IBattleMove move, IBattler user, IBattler target, int damage);

		void EffectsAfterHit(IBattler user, IBattler target, IBattleMove thismove, IEffectsMove turneffects);

		void AbilityCureCheck();
		#endregion

		#region Held item effects
		void ConsumeItem(bool recycle = true, bool pickup = true);

		bool ConfusionBerry(PokemonUnity.Inventory.Plants.Flavours flavor, string message1, string message2);

		bool StatIncreasingBerry(PokemonUnity.Combat.Stats stat, string berryname);

		void ActivateBerryEffect(Items berry = Items.NONE, bool consume = true);

		void BerryCureCheck(bool hpcure = false);
		#endregion

		#region Move user and targets
		IBattler FindUser(IBattleChoice choice, IList<IBattler> targets);

		IBattler ChangeUser(IBattleMove thismove, IBattler user);

		PokemonUnity.Attack.Targets Target(IBattleMove move);

		bool AddTarget(IList<IBattler> targets, IBattler target);

		void RandomTarget(IList<IBattler> targets);

		bool ChangeTarget(IBattleMove thismove, IBattler[] userandtarget, IBattler[] targets);
		#endregion

		#region Move PP
		void SetPP(IBattleMove move, int pp);

		bool ReducePP(IBattleMove move);

		void ReducePPOther(IBattleMove move);
		#endregion

		#region Using a move
		bool ObedienceCheck(IBattleChoice choice);

		bool SuccessCheck(IBattleMove thismove, IBattler user, IBattler target, IEffectsMove turneffects, bool accuracy = true);

		bool TryUseMove(IBattleChoice choice, IBattleMove thismove, IEffectsMove turneffects);

		void ConfusionDamage();

		void UpdateTargetedMove(IBattleMove thismove, IBattler user);

		void ProcessMoveAgainstTarget(IBattleMove thismove, IBattler user, IBattler target, int numhits, IEffectsMove turneffects, bool nocheck = false, int[] alltargets = null, bool showanimation = true);

		/// <summary>
		/// </summary>
		/// <param name="moveid">"Use move"</param>
		/// <param name="index">Index of move to be used in user's moveset</param>
		/// <param name="target">Target (-1 means no target yet)</p
		void UseMoveSimple(Moves moveid, int index = -1, int target = -1);

		void UseMove(IBattleChoice choice, bool specialusage = false);

		void CancelMoves();
		#endregion

		#region Turn processing
		void BeginTurn(IBattleChoice choice);

		void EndTurn(IBattleChoice choice);

		bool ProcessTurn(IBattleChoice choice);
		#endregion
	}
}