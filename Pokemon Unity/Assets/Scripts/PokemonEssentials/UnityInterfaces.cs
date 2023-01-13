using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Localization;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;
using UnityEngine.UI;
using PokemonUnity.Combat;

namespace PokemonUnity.UX
{
	#region Pokemon Battle
	public interface IPokeBattle_SceneIE : PokemonEssentials.Interface.Screen.IPokeBattle_Scene, IScene, IHasDisplayMessageIE//, IHasChatter
	{
		new IEnumerator pbDisplay(string msg, bool brief = false);
		new IEnumerator pbDisplayMessage(string msg, bool brief = false);
		new IEnumerator pbDisplayPausedMessage(string msg);
		IEnumerator pbDisplayConfirmMessage(string msg, System.Action<bool> result);
		IEnumerator pbShowCommands(string msg, string[] commands, int defaultValue, System.Action<int> result);
		//IEnumerator pbShowCommands(string msg, string[] commands, bool canCancel, System.Action<int> result);
		new IEnumerator pbShowOpponent(int index);
		new IEnumerator pbHideOpponent();
		IEnumerator pbTrainerSendOut(int battlerindex, Monster.Pokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		IEnumerator pbSendOut(int battlerindex, Monster.Pokemon pkmn);
		IEnumerator pbTrainerWithdraw(Combat.Battle battle, Combat.Pokemon pkmn);
		IEnumerator pbWithdraw(Combat.Battle battle, Combat.Pokemon pkmn);
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="texts"></param>
		/// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		IEnumerator pbCommandMenuEx(int index, string[] texts, int mode, System.Action<int> result);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		IEnumerator pbFightMenu(int index, System.Action<int> result);
		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name=""></param>
		IEnumerator pbFainted(int pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		IEnumerator pbChooseEnemyCommand(int index);
	}

	/// <summary>
	/// Main battle class.
	/// </summary>
	public interface IBattleIE : PokemonEssentials.Interface.PokeBattle.IBattle
	{
		#region Variable
		/*// <summary>
		/// Scene object for this battle
		/// </summary>
		IScene scene { get; }
		/// <summary>
		/// Decision: 0=undecided; 1=win; 2=loss; 3=escaped; 4=caught
		/// </summary>
		BattleResults decision { get; set; }
		/// <summary>
		/// Internal battle flag
		/// </summary>
		bool internalbattle { get; set; }
		/// <summary>
		/// Double battle flag
		/// </summary>
		bool doublebattle { get; set; }
		/// <summary>
		/// True if player can't escape
		/// </summary>
		bool cantescape { get; set; }
		/// <summary>
		/// Shift/Set "battle style" option
		/// </summary>
		bool shiftStyle { get; set; }
		/// <summary>
		/// "Battle scene" option
		/// </summary>
		bool battlescene { get; set; }
		/// <summary>
		/// Debug flag
		/// </summary>
		bool debug { get; }
		/// <summary>
		/// Player trainer
		/// </summary>
		ITrainer[] player { get; }
		/// <summary>
		/// Opponent trainer
		/// </summary>
		ITrainer[] opponent { get; }
		/// <summary>
		/// Player's Pokémon party
		/// </summary>
		IPokemon[] party1 { get; }
		/// <summary>
		/// Foe's Pokémon party
		/// </summary>
		IPokemon[] party2 { get; }
		/// <summary>
		/// Order of Pokémon in the player's party
		/// </summary>
		IList<int> party1order { get; }
		/// <summary>
		/// Order of Pokémon in the opponent's party
		/// </summary>
		IList<int> party2order { get; }
		/// <summary>
		/// True if player's party's max size is 6 instead of 3
		/// </summary>
		bool fullparty1 { get; }
		/// <summary>
		/// True if opponent's party's max size is 6 instead of 3
		/// </summary>
		bool fullparty2 { get; }*/
		/// <summary>
		/// Currently active Pokémon
		/// </summary>
		new IBattlerIE[] battlers { get; }
		/*// <summary>
		/// Items held by opponents
		/// </summary>
		Items[][] items { get; set; }
		/// <summary>
		/// Effects common to each side of a battle
		/// </summary>
		IEffectsSide[] sides { get; }
		/// <summary>
		/// Effects common to the whole of a battle
		/// </summary>
		IEffectsField field { get; }
		/// <summary>
		/// Battle surroundings
		/// </summary>
		PokemonUnity.Overworld.Environments environment { get; set; }
		/// <summary>
		/// Current weather, custom methods should use <see cref="pbWeather"/> instead
		/// </summary>
		Weather weather { get; set; }
		/// <summary>
		/// Duration of current weather, or -1 if indefinite
		/// </summary>
		int weatherduration { get; set; }
		/// <summary>
		/// True if during the switching phase of the round
		/// </summary>
		bool switching { get; }
		/// <summary>
		/// True if Future Sight is hitting
		/// </summary>
		bool futuresight { get; }
		/// <summary>
		/// The Struggle move
		/// </summary>
		IBattleMove struggle { get; }
		/// <summary>
		/// Choices made by each Pokémon this round
		/// </summary>
		IBattleChoice[] choices { get; }
		/// <summary>
		/// Success states
		/// </summary>
		ISuccessState[] successStates { get; }
		/// <summary>
		/// Last move used
		/// </summary>
		Moves lastMoveUsed { get; set; }
		/// <summary>
		/// Last move user
		/// </summary>
		int lastMoveUser { get; set; }
		/// <summary>
		/// Battle index of each trainer's Pokémon to Mega Evolve
		/// </summary>
		int[][] megaEvolution { get; }
		/// <summary>
		/// Whether Amulet Coin's effect applies
		/// </summary>
		bool amuletcoin { get; }
		/// <summary>
		/// Money gained in battle by using Pay Day
		/// </summary>
		int extramoney { get; set; }
		/// <summary>
		/// Whether Happy Hour's effect applies
		/// </summary>
		bool doublemoney { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		string endspeech { get; set; }
		/// <summary>
		/// Speech by opponent when player wins
		/// </summary>
		string endspeech2 { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		string endspeechwin { get; set; }
		/// <summary>
		/// Speech by opponent when opponent wins
		/// </summary>
		string endspeechwin2 { get; set; }
		IDictionary<string, bool> rules { get; }
		int turncount { get; set; }
		bool controlPlayer { get; set; }*/
		#endregion

		void pbAbort();
		IEnumerator pbDebugUpdate();
		int pbRandom(int x);
		int pbAIRandom(int x);

		#region Initialize battle class.
		IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent);
		IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		//IBattle initialize(IScene scene, IList<IPokemon> p1, IList<IPokemon> p2, IList<ITrainer> player, IList<ITrainer> opponent);
		#endregion

		#region Info about battle.
		bool pbDoubleBattleAllowed();
		Weather pbWeather { get; }
		#endregion

		#region Get battler info.
		bool pbIsOpposing(int index);
		bool pbOwnedByPlayer(int index);
		bool pbIsDoubleBattler(int index);
		/// <summary>
		/// Only used for Wish
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pokemonindex"></param>
		/// <returns></returns>
		//string pbThisEx(battlerindex, pokemonindex);
		string ToString(int battlerindex, int pokemonindex);
		/// <summary>
		/// Checks whether an item can be removed from a Pokémon.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		bool pbIsUnlosableItem(IBattler pkmn, Items item);
		IBattler pbCheckGlobalAbility(Abilities a);
		int nextPickupUse { get; }
		#endregion

		#region Player-related info.
		ITrainer pbPlayer();
		Items[] pbGetOwnerItems(int battlerIndex);
		void pbSetSeen(IPokemon pokemon);
		string pbGetMegaRingName(int battlerIndex);
		bool pbHasMegaRing(int battlerIndex);
		#endregion

		#region Get party info, manipulate parties.
		int pbPokemonCount(IPokemon[] party);
		bool pbAllFainted(IPokemon[] party);
		int pbMaxLevel(IPokemon[] party);
		int pbMaxLevelFromIndex(int index);
		/// <summary>
		/// Gets player party of selected battler
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Returns the trainer party of pokemon at this index?</returns>
		IPokemon[] pbParty(int index);
		IPokemon[] pbOpposingParty(int index);
		int pbSecondPartyBegin(int battlerIndex);
		int pbPartyLength(int battlerIndex);
		int pbFindNextUnfainted(IPokemon[] party, int start, int finish = -1);
		int pbGetLastPokeInTeam(int index);
		IBattler pbFindPlayerBattler(int pkmnIndex);
		bool pbIsOwner(int battlerIndex, int partyIndex);
		ITrainer pbGetOwner(int battlerIndex);
		ITrainer pbGetOwnerPartner(int battlerIndex);
		int pbGetOwnerIndex(int battlerIndex);
		bool pbBelongsToPlayer(int battlerIndex);
		ITrainer pbPartyGetOwner(int battlerIndex, int partyIndex);
		void pbAddToPlayerParty(IPokemon pokemon);
		void pbRemoveFromParty(int battlerIndex, int partyIndex);
		#endregion

		#region Check whether actions can be taken.
		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		bool pbCanShowCommands(int idxPokemon);
		#endregion

		#region Attacking.
		bool pbCanShowFightMenu(int idxPokemon);
		IEnumerator pbCanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false, System.Action<bool> result = null);
		void pbAutoChooseMove(int idxPokemon, bool showMessages = true);
		bool pbRegisterMove(int idxPokemon, int idxMove, bool showMessages = true);
		bool pbChoseMove(int i, Moves move);
		bool pbChoseMoveFunctionCode(int i, PokemonUnity.Attack.Data.Effects code);
		bool pbRegisterTarget(int idxPokemon, int idxTarget);
		new IBattlerIE[] pbPriority(bool ignorequickclaw = false, bool log = false);
		#endregion

		#region Switching Pokémon.
		bool pbCanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages);
		bool pbCanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false);
		bool pbRegisterSwitch(int idxPokemon, int idxOther);
		bool pbCanChooseNonActive(int index);
		void pbSwitch(bool favorDraws= false);
		void pbSendOut(int index, IPokemon pokemon);
		void pbReplace(int index, int newpoke, bool batonpass = false);
		bool pbRecallAndReplace(int index, int newpoke, int newpokename = -1, bool batonpass = false, bool moldbreaker = false);
		void pbMessagesOnReplace(int index, int newpoke, int newpokename= -1);
		int pbSwitchInBetween(int index, bool lax, bool cancancel);
		int pbSwitchPlayer(int index, bool lax, bool cancancel);
		#endregion

		#region Using an item.
		/// <summary>
		/// Uses an item on a Pokémon in the player's party.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="pkmnIndex"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		IEnumerator pbUseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result);
		/// <summary>
		/// Uses an item on an active Pokémon.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		bool pbUseItemOnBattler(Items item, int index, IBattler userPkmn, IHasDisplayMessage scene);
		bool pbRegisterItem(int idxPokemon, Items idxItem, int? idxTarget = null);
		void pbEnemyUseItem(Items item, IBattler battler);
		#endregion

		#region Fleeing from battle.
		bool pbCanRun(int idxPokemon);
		/// <summary>
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <param name="duringBattle"></param>
		/// <returns>1 = success; -1 = failed</returns>
		int pbRun(int idxPokemon, bool duringBattle= false);
		#endregion

		#region Mega Evolve battler.
		bool pbCanMegaEvolve(int index);
		void pbRegisterMegaEvolution(int index);
		void pbMegaEvolve(int index);
		#endregion

		#region Primal Revert battler.
		void pbPrimalReversion(int index);
		#endregion

		#region Call battler.
		void pbCall(int index);
		#endregion

		#region Gaining Experience.
		void pbGainEXP();
		void pbGainExpOne(int index, IBattler defeated, int partic, int expshare, bool haveexpall, bool showmessages = true);
		#endregion

		#region Learning a move.
		void pbLearnMove(int pkmnIndex, Moves move);
		#endregion

		#region Abilities.
		void pbOnActiveAll();
		bool pbOnActiveOne(IBattler pkmn, bool onlyabilities = false, bool moldbreaker = false);
		IEnumerator pbPrimordialWeather();
		#endregion

		#region Judging.
		void pbJudgeCheckpoint(IBattler attacker, IBattleMove move = null);
		BattleResults pbDecisionOnTime();
		BattleResults pbDecisionOnTime2();
		BattleResults pbDecisionOnDraw();
		void pbJudge();
		#endregion

		#region Messages and animations.
		new IEnumerator pbDisplay(string msg);
		new IEnumerator pbDisplayPaused(string msg);
		new IEnumerator pbDisplayBrief(string msg);
		bool pbDisplayConfirm(string msg);
		void pbShowCommands(string msg, string[] commands, bool cancancel = true);
		void pbAnimation(Moves move, IBattler attacker, IBattler opponent, int hitnum = 0);
		void pbCommonAnimation(string name, IBattler attacker, IBattler opponent, int hitnum = 0);
		#endregion

		#region Battle core.
		BattleResults pbStartBattle(bool canlose= false);
		void pbStartBattleCore(bool canlose);
		#endregion

		#region Command phase.
		MenuCommands pbCommandMenu(int i);
		KeyValuePair<Items, int?> pbItemMenu(int i);
		bool pbAutoFightMenu(int i);
		void pbCommandPhase();
		#endregion

		#region Attack phase.
		void pbAttackPhase();
		#endregion

		#region End of round.
		void pbEndOfRoundPhase();
		#endregion

		#region End of battle.
		BattleResults pbEndOfBattle(bool canlose = false);
		#endregion
	}
	#endregion

	#region Combat Match Recorder
	/// <summary>
	/// Recording of a Pokemon Battle
	/// </summary>
	public interface IPokeBattle_RecordedBattleModule<out TBattle> //: PokemonUnity.Combat.IPokeBattle_RecordedBattleModule<out TBattle>
	{
		IEnumerator pbAutoChooseMove(int i1, bool showMessages = true);
		IEnumerator pbRegisterMove(int i1, int i2, bool showMessages, System.Action<bool> result);
	}

	/// <summary>
	/// Playback Recorded Pokemon Battle
	/// </summary>
	/// <typeparam name="IRecord"></typeparam>
	public interface IPokeBattle_BattlePlayerModule<out IRecord> //: PokemonUnity.Combat.IPokeBattle_BattlePlayerModule<out IRecord>
	{
		IEnumerator pbCommandPhaseCore();
	}
	#endregion

	#region
	public interface IBattleMoveIE : PokemonEssentials.Interface.PokeBattle.IBattleMove
	{
		//Moves id { get; set; }
		new IBattleIE battle { get; set; }
		/*string Name { get; }
		//int function { get; set; }
		PokemonUnity.Attack.Data.Effects Effect { get; }
		int basedamage { get; set; }
		Types Type { get; set; }
		int Accuracy { get; set; }
		int AddlEffect { get; }
		PokemonUnity.Attack.Category Category { get; } //ToDo: Move to application layer
		PokemonUnity.Attack.Data.Targets Target { get; set; }
		int Priority { get; set; }
		PokemonUnity.Attack.Data.Flag Flags { get; set; }
		IMove thismove { get; }
		int PP { get; set; }
		int TotalPP { get; set; }*/

		#region Creating a move
		IBattleMoveIE initialize(IBattleIE battle, IMove move);

		/// <summary>
		/// This is the code actually used to generate a <see cref="IBattleMove"/> object.  
		/// </summary>
		/// <param name=""></param>
		/// <param name=""></param>
		/// <returns></returns>
		/// The object generated is a subclass of this one which depends on the move's
		/// function code (found in the script section PokeBattle_MoveEffect).
		IBattleMoveIE pbFromPBMove(IBattleIE battle, IMove move);
		#endregion

		#region About the move
		//int totalpp();
		//int addlEffect();

		//int ToInt();
		Types pbModifyType(Types type, IBattlerIE attacker, IBattlerIE opponent);
		Types pbType(Types type, IBattlerIE attacker, IBattlerIE opponent);
		/*bool pbIsPhysical(Types type);
		bool pbIsSpecial(Types type);
		bool pbIsStatus { get; }
		bool pbIsDamaging();*/
		bool pbTargetsMultiple(IBattlerIE attacker);
		int pbPriority(IBattlerIE attacker);
		int pbNumHits(IBattlerIE attacker);
		//bool pbIsMultiHit();
		bool pbTwoTurnAttack(IBattlerIE attacker);
		void pbAdditionalEffect(IBattlerIE attacker, IBattlerIE opponent);
		/*bool pbCanUseWhileAsleep();
		bool isHealingMove();
		bool isRecoilMove();
		bool unusableInGravity();
		bool isContactMove();
		bool canProtectAgainst();
		bool canMagicCoat();
		bool canSnatch();
		bool canMirrorMove();
		bool canKingsRock();
		bool canThawUser();
		bool hasHighCriticalRate();
		bool isBitingMove();
		bool isPunchingMove();
		bool isSoundBased();
		bool isPowderMove();
		bool isPulseMove();
		bool isBombMove();
		bool tramplesMinimize(int param = 1);
		bool successCheckPerHit();*/
		bool ignoresSubstitute(IBattlerIE attacker);
		#endregion

		#region This move's type effectiveness
		bool pbTypeImmunityByAbility(Types type, IBattlerIE attacker, IBattlerIE opponent);
		float pbTypeModifier(Types type, IBattlerIE attacker, IBattlerIE opponent);
		double pbTypeModMessages(Types type, IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region This move's accuracy check
		int pbModifyBaseAccuracy(int baseaccuracy, IBattlerIE attacker, IBattlerIE opponent);
		bool pbAccuracyCheck(IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region Damage calculation and modifiers
		bool pbCritialOverride(IBattlerIE attacker, IBattlerIE opponent);
		bool pbIsCritical(IBattlerIE attacker, IBattlerIE opponent);
		int pbBaseDamage(int basedmg, IBattlerIE attacker, IBattlerIE opponent);
		double pbBaseDamageMultiplier(double damagemult, IBattlerIE attacker, IBattlerIE opponent);
		double pbModifyDamage(double damagemult, IBattlerIE attacker, IBattlerIE opponent);
		int pbCalcDamage(IBattlerIE attacker, IBattlerIE opponent, params byte[] options); //= new int[] { 0 }
		int pbReduceHPDamage(int damage, IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region Effects
		void pbEffectMessages(IBattlerIE attacker, IBattlerIE opponent, bool ignoretype = false, int[] alltargets = null);
		int pbEffectFixedDamage(int damage, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
		int pbEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
		void pbEffectAfterHit(IBattlerIE attacker, IBattlerIE opponent, IEffectsMove turneffects);
		#endregion

		#region Using the move
		bool pbOnStartUse(IBattlerIE attacker);
		void pbAddTarget(IList<IBattlerIE> targets, IBattlerIE attacker);
		void pbAddTarget(ref IList<IBattlerIE> targets, IBattlerIE attacker);
		int pbDisplayUseMessage(IBattlerIE attacker);
		void pbShowAnimation(Moves id, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
		void pbOnDamageLost(int damage, IBattlerIE attacker, IBattlerIE opponent);
		bool pbMoveFailed(IBattlerIE attacker, IBattlerIE opponent);
		#endregion
	}
	#endregion

	#region
	public interface IHasDisplayMessageIE
	{
		IEnumerator pbDisplay(string v);
		//ToDo: rename to "pbDisplayConfirmMessage"?
		IEnumerator pbDisplayConfirm(string v, System.Action<bool> result);
	}
	#endregion

	#region
	#endregion
}