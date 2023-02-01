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
	public interface IGameObject
	{
		bool visible { get; set; }
		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
		void update();
	}

	#region Pokemon Battle
	public interface IPokeBattle_SceneIE : PokemonEssentials.Interface.Screen.IPokeBattle_Scene, IScene, IHasDisplayMessageIE//, IHasChatter
	{
		//// <summary>
		//// </summary>
		//// <param name="index"></param>
		//// <param name="texts"></param>
		//// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		//IEnumerator pbCommandMenuEx(int index, string[] texts, int mode, System.Action<int> result);
		#region Override `void` Animations with Unity API and Coroutines
		new IPokeBattle_SceneIE initialize();
		//void pbUpdate();
		//void pbGraphicsUpdate();
		//void pbInputUpdate();
		//void pbShowWindow(int windowtype);
		//void pbSetMessageMode(bool mode);
		new IEnumerator pbWaitMessage();
		//void pbDisplay(string msg, bool brief = false);
		new IEnumerator pbDisplay(string msg, bool brief = false);
		//new void pbDisplayMessage(string msg, bool brief = false);
		//new void pbDisplayPausedMessage(string msg);
		//new bool pbDisplayConfirmMessage(string msg);
		//void pbShowCommands(string msg, string[] commands, int defaultValue);
		IEnumerator pbShowCommands(string msg, string[] commands, int defaultValue, System.Action<int> result);
		//new void pbShowCommands(string msg, string[] commands, bool canCancel);
		//IEnumerator pbShowCommands(string msg, string[] commands, bool canCancel, System.Action<int> result);
		void pbFrameUpdate(IGameObject cw);
		//void pbRefresh();
		//IIconSprite pbAddSprite(string id, float x, float y, string filename, IViewport viewport);
		//void pbAddPlane(string id, string filename, IViewport viewport);
		//void pbDisposeSprites();
		//new void pbBeginCommandPhase();
		//new IEnumerator pbShowOpponent(int index);
		//new IEnumerator pbHideOpponent();
		//new IEnumerator pbShowHelp(string text);
		//new IEnumerator pbHideHelp();
		//new IEnumerator pbBackdrop();
		/// <summary>
		/// Returns whether the party line-ups are currently appearing on-screen
		/// </summary>
		/// <returns></returns>
		//bool inPartyAnimation { get; }
		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		new IEnumerator partyAnimationUpdate();
		//new IEnumerator pbStartBattle(IBattle battle);
		//new IEnumerator pbEndBattle(BattleResults result);
		//new IEnumerator pbRecall(int battlerindex);
		//new IEnumerator pbTrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		//new IEnumerator pbSendOut(int battlerindex, IPokemon pkmn);
		//IEnumerator pbTrainerWithdraw(IBattle battle, IBattler pkmn);
		//IEnumerator pbWithdraw(IBattle battle, IBattler pkmn);
		//new string pbMoveString(string move);
		//new IEnumerator pbBeginAttackPhase();
		new IEnumerator pbSafariStart();
		//void pbResetCommandIndices();
		//void pbResetMoveIndex(int index);
		//int pbSafariCommandMenu(int index);
		IEnumerator pbSafariCommandMenu(int index, System.Action<int> result);
		/// <summary>
		/// Use this method to display the list of commands and choose
		/// a command for the player.
		/// </summary>
		/// 0 - Fight, 1 - Pokémon, 2 - Bag, 3 - Run
		/// <param name="index">Index of battler (use e.g. @battle.battlers[index] to 
		/// access the battler)</param>
		/// <returns> Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		//new int pbCommandMenu(int index);
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="texts"></param>
		/// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		//int pbCommandMenuEx(int index, string[] texts, int mode = 0);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		//new int pbFightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		//new KeyValuePair<Items, int> pbItemMenu(int index);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.  The function
		/// should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="moveToLearn"></param>
		//new int pbForgetMove(IPokemon pokemon, Moves moveToLearn);
		/// <summary>
		/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
		/// </summary>
		/// <param name=""></param>
		/// <param name="message"></param>
		//int pbChooseMove(IPokemon pokemon, string message);
		IEnumerator pbChooseMove(IPokemon pokemon, string message, System.Action<int> result);
		//string pbNameEntry(string helptext, IPokemon pokemon);
		IEnumerator pbNameEntry(string helptext, IPokemon pokemon, System.Action<string> result);
		//new IEnumerator pbSelectBattler(int index, int selectmode = 1);
		//int pbFirstTarget(int index, int targettype);
		//int pbFirstTarget(int index, PokemonUnity.Attack.Data.Targets targettype);
		//void pbUpdateSelected(int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype">Which targets are selectable as option</param>
		//new int pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype);
		//new int pbSwitch(int index, bool lax, bool cancancel);
		//void pbDamageAnimation(IBattler pkmn, float effectiveness);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		//IEnumerator pbHPChanged(int pkmn, int oldhp, bool anim = false);
		//IEnumerator HPChanged(int index, int oldhp, bool animate = false);
		//IEnumerator pbHPChanged(IBattler pkmn, int oldhp, bool animate = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name=""></param>
		//IEnumerator pbFainted(int pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		//new void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		//new int pbChooseNewEnemy(int index, IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new IEnumerator pbWildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new IEnumerator pbTrainerBattleSuccess();
		//new void pbEXPBar(IBattler battler, IPokemon pokemon, int startexp, int endexp, int tempexp1, int tempexp2);
		new IEnumerator pbShowPokedex(Pokemons species, int form = 0);
		//IEnumerator pbChangePokemon(IBattlerIE attacker, IPokemon pokemon);
		//void pbSaveShadows(Action action = null);
		//IEnumerator pbFindAnimation(Moves moveid, int userIndex, int hitnum);
		IEnumerator pbCommonAnimation(string animname, IBattlerIE user, IBattlerIE target, int hitnum = 0);
		//new IEnumerator pbAnimation(Moves moveid, IBattler user, IBattler target, int hitnum = 0);
		IEnumerator pbAnimationCore(string animation, IBattlerIE user, IBattlerIE target, bool oppmove = false);
		//new IEnumerator pbLevelUp(IBattler battler, IPokemon pokemon, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef);
		new IEnumerator pbThrowAndDeflect(Items ball, int targetBattler);
		new IEnumerator pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false);
		new IEnumerator pbThrowSuccess();
		new IEnumerator pbHideCaptureBall();
		new IEnumerator pbThrowBait();
		new IEnumerator pbThrowRock();
		#endregion
		#region Override NoGraphics with Unity API and Coroutines
		//new void pbDisplayMessage(string msg,bool brief= false);
		new IEnumerator pbDisplayMessage(string msg, bool brief = false);
		//new void pbDisplayPausedMessage(string msg);
		new IEnumerator pbDisplayPausedMessage(string msg);
		//new void pbDisplayConfirmMessage(string msg);
		IEnumerator pbDisplayConfirmMessage(string msg, System.Action<bool> result);
		//new IEnumerator pbFrameUpdate(IViewport cw);
		//void pbRefresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		//new IEnumerator pbBeginCommandPhase();
		/// <summary>
		/// Called whenever the battle begins
		/// </summary>
		/// <remarks>
		/// This is used to animate the battle intro 
		/// (trainer appearing in frame, the challenge dialog, 
		/// pokemons appearing, and HUD setup)
		/// </remarks>
		/// <param name="battle"></param>
		//new void pbStartBattle(IBattle battle);
		IEnumerator pbStartBattle(IBattleIE battle); //ToDo: Can be void, if using StartCoroutine...
		//new void pbEndBattle(PokemonUnity.Combat.BattleResults result);
		new IEnumerator pbEndBattle(PokemonUnity.Combat.BattleResults result);
		//new void pbTrainerSendOut(IBattle battle,IPokemon pkmn);
		new IEnumerator pbTrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		//new void pbSendOut(IBattle battle,IPokemon pkmn);
		new IEnumerator pbSendOut(int battlerindex, IPokemon pkmn);
		//new void pbTrainerWithdraw(IBattle battle,IPokemon pkmn);
		IEnumerator pbTrainerWithdraw(IBattle battle, IBattlerIE pkmn);
		//new void pbWithdraw(IBattle battle,IPokemon pkmn);
		IEnumerator pbWithdraw(IBattle battle, IBattlerIE pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		IEnumerator pbForgetMove(IPokemon pkmn,Moves move, System.Action<int> result);
		new IEnumerator pbBeginAttackPhase();
		/// <summary>
		/// Use this method to display the list of commands.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		//new void pbCommandMenu(int index);
		IEnumerator pbCommandMenu(int index, System.Action<MenuCommands> result);
		/// <summary>
		/// </summary>
		/// <param name="pkmn"></param>
		//void pbPokemonString(IPokemon pkmn);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		//new void pbFightMenu(int index);
		IEnumerator pbFightMenu(int index, System.Action<int> result);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		//new void pbItemMenu(int index);
		IEnumerator pbItemMenu(int index, System.Action<Items> result);
		//new IEnumerator pbFirstTarget(int index, int targettype);
		new IEnumerator pbNextTarget(int cur, int index);
		new IEnumerator pbPrevTarget(int cur, int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		//new void pbChooseTarget(int index,int targettype);
		IEnumerator pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype, System.Action<int> result);
		//new IEnumerator pbSwitch(int index,bool lax,bool cancancel);
		IEnumerator pbSwitch(int index,bool lax,bool cancancel, System.Action<int> result);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		IEnumerator pbHPChanged(IBattlerIE pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		//new void pbFainted(IPokemon pkmn);
		IEnumerator pbFainted(IBattlerIE pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		//new void pbChooseEnemyCommand(int index);
		//new IEnumerator pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		//new IEnumerator pbChooseNewEnemy(int index,IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new IEnumerator pbWildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new IEnumerator pbTrainerBattleSuccess();
		IEnumerator pbEXPBar(IBattlerIE battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		IEnumerator pbLevelUp(IBattlerIE battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		//new void pbShowOpponent(int opp);
		new IEnumerator pbShowOpponent(int index);
		//new void pbHideOpponent();
		new IEnumerator pbHideOpponent();
		new IEnumerator pbRecall(int battlerindex);
		IEnumerator pbDamageAnimation(IBattlerIE pkmn,TypeEffective effectiveness);
		IEnumerator pbAnimation(Moves moveid,IBattlerIE attacker,IBattlerIE opponent,int hitnum= 0);
		#endregion
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

		//void pbAbort();
		//IEnumerator pbDebugUpdate();
		//int pbRandom(int x);
		//int pbAIRandom(int x);

		#region Initialize battle class.
		new IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent);
		new IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		//IBattle initialize(IScene scene, IList<IPokemon> p1, IList<IPokemon> p2, IList<ITrainer> player, IList<ITrainer> opponent);
		#endregion

		#region Info about battle.
		//bool pbDoubleBattleAllowed();
		//Weather pbWeather { get; }
		#endregion

		#region Get battler info.
		//bool pbIsOpposing(int index);
		//bool pbOwnedByPlayer(int index);
		//bool pbIsDoubleBattler(int index);
		/// <summary>
		/// Only used for Wish
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pokemonindex"></param>
		/// <returns></returns>
		//string pbThisEx(battlerindex, pokemonindex);
		//string ToString(int battlerindex, int pokemonindex);
		/// <summary>
		/// Checks whether an item can be removed from a Pokémon.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		//bool pbIsUnlosableItem(IBattlerIE pkmn, Items item);
		//IBattlerIE pbCheckGlobalAbility(Abilities a);
		//int nextPickupUse { get; }
		#endregion

		#region Player-related info.
		//ITrainer pbPlayer();
		//Items[] pbGetOwnerItems(int battlerIndex);
		//void pbSetSeen(IPokemon pokemon);
		//string pbGetMegaRingName(int battlerIndex);
		//bool pbHasMegaRing(int battlerIndex);
		#endregion

		#region Get party info, manipulate parties.
		//int pbPokemonCount(IPokemon[] party);
		//bool pbAllFainted(IPokemon[] party);
		//int pbMaxLevel(IPokemon[] party);
		//int pbMaxLevelFromIndex(int index);
		///// <summary>
		///// Gets player party of selected battler
		///// </summary>
		///// <param name="index"></param>
		///// <returns>Returns the trainer party of pokemon at this index?</returns>
		//IPokemon[] pbParty(int index);
		//IPokemon[] pbOpposingParty(int index);
		//int pbSecondPartyBegin(int battlerIndex);
		//int pbPartyLength(int battlerIndex);
		//int pbFindNextUnfainted(IPokemon[] party, int start, int finish = -1);
		//int pbGetLastPokeInTeam(int index);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="pkmnIndex"></param>
		/// <returns></returns>
		new IBattlerIE pbFindPlayerBattler(int pkmnIndex);
		//bool pbIsOwner(int battlerIndex, int partyIndex);
		//ITrainer pbGetOwner(int battlerIndex);
		//ITrainer pbGetOwnerPartner(int battlerIndex);
		//int pbGetOwnerIndex(int battlerIndex);
		//bool pbBelongsToPlayer(int battlerIndex);
		//ITrainer pbPartyGetOwner(int battlerIndex, int partyIndex);
		//void pbAddToPlayerParty(IPokemon pokemon);
		//void pbRemoveFromParty(int battlerIndex, int partyIndex);
		#endregion

		#region Check whether actions can be taken.
		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		//bool pbCanShowCommands(int idxPokemon);
		#endregion

		#region Attacking.
		/// <summary>
		/// Check whether the fight selection prompt can be shown for pokemon.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		IEnumerator pbCanShowFightMenu(int idxPokemon, Action<bool> result = null);
		IEnumerator pbCanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false, System.Action<bool> result = null);
		new IEnumerator pbAutoChooseMove(int idxPokemon, bool showMessages = true);
		IEnumerator pbRegisterMove(int idxPokemon, int idxMove, bool showMessages = true, System.Action<bool> result = null);
		//bool pbChoseMove(int i, Moves move);
		//bool pbChoseMoveFunctionCode(int i, PokemonUnity.Attack.Data.Effects code);
		//bool pbRegisterTarget(int idxPokemon, int idxTarget);
		new IBattlerIE[] pbPriority(bool ignorequickclaw = false, bool log = false);
		//IEnumerator pbPriority(bool ignorequickclaw = false, bool log = false, System.Action<IBattlerIE[]> result = null);
		#endregion

		#region Switching Pokémon.
		IEnumerator pbCanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages, System.Action<bool> result = null);
		IEnumerator pbCanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false, System.Action<bool> result = null);
		IEnumerator pbRegisterSwitch(int idxPokemon, int idxOther, System.Action<bool> result = null);
		//bool pbCanChooseNonActive(int index);
		new IEnumerator pbSwitch(bool favorDraws= false);
		new IEnumerator pbSendOut(int index, IPokemon pokemon);
		new IEnumerator pbReplace(int index, int newpoke, bool batonpass = false);
		IEnumerator pbRecallAndReplace(int index, int newpoke, int newpokename = -1, bool batonpass = false, bool moldbreaker = false, System.Action<bool> result = null);
		new IEnumerator pbMessagesOnReplace(int index, int newpoke, int newpokename= -1);
		IEnumerator pbSwitchInBetween(int index, bool lax, bool cancancel, System.Action<int> result = null);
		IEnumerator pbSwitchPlayer(int index, bool lax, bool cancancel, System.Action<int> result = null);
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
		bool pbUseItemOnBattler(Items item, int index, IBattlerIE userPkmn, IHasDisplayMessageIE scene);
		IEnumerator pbRegisterItem(int idxPokemon, Items idxItem, int? idxTarget = null, System.Action<bool> result = null);
		IEnumerator pbEnemyUseItem(Items item, IBattlerIE battler);
		#endregion

		#region Fleeing from battle.
		//bool pbCanRun(int idxPokemon);
		/// <summary>
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <param name="duringBattle"></param>
		/// <returns>1 = success; -1 = failed</returns>
		IEnumerator pbRun(int idxPokemon, bool duringBattle=false, System.Action<int> result = null);
		#endregion

		#region Mega Evolve battler.
		//bool pbCanMegaEvolve(int index);
		//void pbRegisterMegaEvolution(int index);
		new IEnumerator pbMegaEvolve(int index);
		#endregion

		#region Primal Revert battler.
		new IEnumerator pbPrimalReversion(int index);
		#endregion

		#region Call battler.
		new IEnumerator pbCall(int index);
		#endregion

		#region Gaining Experience.
		new IEnumerator pbGainEXP();
		IEnumerator pbGainExpOne(int index, IBattlerIE defeated, int partic, int expshare, bool haveexpall, bool showmessages = true);
		#endregion

		#region Learning a move.
		new IEnumerator pbLearnMove(int pkmnIndex, Moves move);
		#endregion

		#region Abilities.
		new IEnumerator pbOnActiveAll();
		IEnumerator pbOnActiveOne(IBattlerIE pkmn, bool onlyabilities = false, bool moldbreaker = false, System.Action<bool> result = null);
		new IEnumerator pbPrimordialWeather();
		#endregion

		#region Judging.
		//void pbJudgeCheckpoint(IBattlerIE attacker, IBattleMove move = null);
		//BattleResults pbDecisionOnTime();
		//BattleResults pbDecisionOnTime2();
		//BattleResults pbDecisionOnDraw();
		//void pbJudge();
		#endregion

		#region Messages and animations.
		new IEnumerator pbDisplay(string msg);
		new IEnumerator pbDisplayPaused(string msg);
		new IEnumerator pbDisplayBrief(string msg);
		IEnumerator pbDisplayConfirm(string msg, System.Action<bool> result);
		new IEnumerator pbShowCommands(string msg, string[] commands, bool cancancel = true);
		IEnumerator pbAnimation(Moves move, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0);
		IEnumerator pbCommonAnimation(string name, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0);
		#endregion

		#region Battle core.
		//IEnumerator pbStartBattle(bool canlose= false, System.Action<BattleResults> result = null);
		//new IEnumerator pbStartBattle(bool canlose= false);
		new IEnumerator pbStartBattleCore(bool canlose);
		#endregion

		#region Command phase.
		IEnumerator pbCommandMenu(int i, System.Action<MenuCommands> result);
		IEnumerator pbItemMenu(int i, System.Action<KeyValuePair<Items, int?>> result);
		//bool pbAutoFightMenu(int i);
		new IEnumerator pbCommandPhase();
		#endregion

		#region Attack phase.
		new IEnumerator pbAttackPhase();
		#endregion

		#region End of round.
		new IEnumerator pbEndOfRoundPhase();
		#endregion

		#region End of battle.
		//BattleResults pbEndOfBattle(bool canlose = false);
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
		IEnumerator pbEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result = null);
		IEnumerator pbEffectAfterHit(IBattlerIE attacker, IBattlerIE opponent, IEffectsMove turneffects);
		#endregion

		#region Using the move
		bool pbOnStartUse(IBattlerIE attacker);
		void pbAddTarget(IList<IBattlerIE> targets, IBattlerIE attacker);
		void pbAddTarget(ref IList<IBattlerIE> targets, IBattlerIE attacker);
		int pbDisplayUseMessage(IBattlerIE attacker);
		IEnumerator pbShowAnimation(Moves id, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
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
	public interface ICanCaptureAnimationIE
	{
		/// <summary>
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		/// <param name="ball"></param>
		/// <param name="shakes"></param>
		/// <param name="critical"></param>
		/// <param name="targetBattler"></param>
		/// <param name="scene"></param>
		/// <param name="battler"></param>
		/// <param name="burst"></param>
		/// <param name="showplayer"></param>
		IEnumerator pokeballThrow(Items ball, int shakes, bool critical, IBattlerIE targetBattler, IScene scene, IBattlerIE battler, int burst = -1, bool showplayer = false);
	}
	#endregion
}