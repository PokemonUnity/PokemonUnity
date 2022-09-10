using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle.Effects;

namespace PokemonEssentials.Interface.PokeBattle
{
	public interface IBattleCommon
	{
		void pbStorePokemon(IPokemon pokemon);

		void pbThrowPokeBall(int idxPokemon, Items ball, int? rareness = null, bool showplayer = false);
	}

	/// <summary>
	/// Main battle class.
	/// </summary>
	public interface IBattle : IBattleCommon
	{
		#region Variable
		/// <summary>
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
		bool fullparty2 { get; }
		/// <summary>
		/// Currently active Pokémon
		/// </summary>
		IBattler[] battlers { get; }
		/// <summary>
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
		bool controlPlayer { get; set; }
		#endregion

		void pbAbort();

		IEnumerator pbDebugUpdate();

		int pbRandom(int x);

		int pbAIRandom(int x);

		#region Initialise battle class.
		IBattle initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent);
		IBattle initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
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

		void pbAddToPlayerParty(IBattler pokemon);

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

		bool pbCanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false);

		void pbAutoChooseMove(int idxPokemon, bool showMessages = true);

		bool pbRegisterMove(int idxPokemon, int idxMove, bool showMessages = true);

		bool pbChoseMove(int i, Moves move);

		bool pbChoseMoveFunctionCode(int i, PokemonUnity.Attack.Data.Effects code);

		bool pbRegisterTarget(int idxPokemon, int idxTarget);

		IBattler[] pbPriority(bool ignorequickclaw = false, bool log = false);
		#endregion

		#region Switching Pokémon.
		bool pbCanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages);

		bool pbCanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false);

		bool pbRegisterSwitch(int idxPokemon, int idxOther);

		bool pbCanChooseNonActive(int index);

		void pbSwitch(bool favorDraws= false);

		void pbSendOut(int index, IBattler pokemon);

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
		bool pbUseItemOnPokemon(Items item, int pkmnIndex, IBattler userPkmn, IHasDisplayMessage scene);

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

		void pbPrimordialWeather();
		#endregion

		#region Judging.
		void pbJudgeCheckpoint(IBattler attacker, IBattleMove move = null);

		BattleResults pbDecisionOnTime();

		BattleResults pbDecisionOnTime2();

		BattleResults pbDecisionOnDraw();

		void pbJudge();
		#endregion

		#region Messages and animations.
		void pbDisplay(string msg);

		void pbDisplayPaused(string msg);

		void pbDisplayBrief(string msg);

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
}