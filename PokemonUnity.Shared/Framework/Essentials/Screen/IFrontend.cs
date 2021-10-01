using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	#region Text Entry
	public interface IPokemonEntry : IScreen
	{
		void initialize(IPokemonEntryScene scene);
		string pbStartScreen(string helptext, int minlength, int maxlength, string initialText, TextEntryTypes mode = 0, IPokemon pokemon = null);
	}

	/// <summary>
	/// Text entry screen - free typing.
	/// </summary>
	public interface IPokemonEntryScene : IScene
	{
		void pbStartScene(string helptext, int minlength, int maxlength, string initialText, TextEntryTypes subject = 0, IPokemon pokemon = null);
		void pbEndScene();
		string pbEntry();
		//string pbEntry1();
		//string pbEntry2();
	}

	/// <summary>
	/// Text entry screen - arrows to select letter.
	/// </summary>
	public interface IPokemonEntryScene2 : IScene, IPokemonEntryScene
	{
		//void pbStartScene(string helptext, int minlength, int maxlength, string initialText, int subject = 0, Pokemon pokemon = null);
		//void pbEndScene();
		//string pbEntry();
		void pbUpdate();
		void pbChangeTab(int newtab = 0);
		bool pbColumnEmpty(int m);
		void pbUpdateOverlay();
		void pbDoUpdateOverlay();
		void pbDoUpdateOverlay2();
		bool pbMoveCursor();
		int wrapmod(int x, int y);
	}
	#endregion

	#region Pokemon Battle
	public interface ISceneHasChatter
	{
		void pbChatter(IBattler attacker,Combat.Trainer opponent);
	}
	public interface IPokeBattle_Scene : IScene, ISceneHasChatter
	{
		//event EventHandler<PokeballThrowTargetArgs> OnPokeballThrown;
		/*
		-  def pbChooseNewEnemy(int index,party)
		Use this method to choose a new Pokémon for the enemy
		The enemy's party is guaranteed to have at least one 
		choosable member.
		index - Index to the battler to be replaced (use e.g. @battle.battlers[index] to 
		access the battler)
		party - Enemy's party

		- def pbWildBattleSuccess
		This method is called when the player wins a wild Pokémon battle.
		This method can change the battle's music for example.

		- def pbTrainerBattleSuccess
		This method is called when the player wins a Trainer battle.
		This method can change the battle's music for example.

		- def pbFainted(pkmn)
		This method is called whenever a Pokémon faints.
		pkmn - PokeBattle_Battler object indicating the Pokémon that fainted

		- def pbChooseEnemyCommand(int index)
		Use this method to choose a command for the enemy.
		index - Index of enemy battler (use e.g. @battle.battlers[index] to 
		access the battler)
		*/

		void initialize();
		void pbUpdate();
		void pbGraphicsUpdate();
		void pbInputUpdate();
		void pbShowWindow(int windowtype);
		void pbSetMessageMode(bool mode);
		void pbWaitMessage();
		void pbDisplay(string msg, bool brief = false);
		void pbDisplayMessage(string msg, bool brief = false);
		void pbDisplayPausedMessage(string msg);
		bool pbDisplayConfirmMessage(string msg);
		//void pbShowCommands(string msg, string[] commands, int defaultValue);
		void pbShowCommands(string msg, string[] commands, bool canCancel);
		void pbFrameUpdate(object cw = null);
		//void pbRefresh();
		void pbAddSprite(string id, double x, double y, string filename, int viewport);
		void pbAddPlane(int id, string filename, int viewport);
		void pbDisposeSprites();
		void pbBeginCommandPhase();
		IEnumerator pbShowOpponent(int index);
		IEnumerator pbHideOpponent();
		void pbShowHelp(string text);
		void pbHideHelp();
		void pbBackdrop();
		/// <summary>
		/// Returns whether the party line-ups are currently appearing on-screen
		/// </summary>
		/// <returns></returns>
		bool inPartyAnimation { get; }
		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		void partyAnimationUpdate();
		void pbStartBattle(PokemonUnity.Combat.Battle battle);
		void pbEndBattle(BattleResults result);
		void pbRecall(int battlerindex);
		void pbTrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		void pbSendOut(int battlerindex, IPokemon pkmn);
		void pbTrainerWithdraw(Combat.Battle battle, IBattler pkmn);
		void pbWithdraw(Combat.Battle battle, IBattler pkmn);
		string pbMoveString(string move);
		void pbBeginAttackPhase();
		void pbSafariStart();
		void pbResetCommandIndices();
		void pbResetMoveIndex(int index);
		int pbSafariCommandMenu(int index);
		/// <summary>
		/// Use this method to display the list of commands and choose
		/// a command for the player.
		/// </summary>
		/// 0 - Fight, 1 - Pokémon, 2 - Bag, 3 - Run
		/// <param name="index">Index of battler (use e.g. @battle.battlers[index] to 
		/// access the battler)</param>
		/// <returns> Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		int pbCommandMenu(int index);
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="texts"></param>
		/// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		int pbCommandMenuEx(int index, string[] texts, int mode = 0);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		int pbFightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		KeyValuePair<Items, int> pbItemMenu(int index);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.  The function
		/// should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="moveToLearn"></param>
		int pbForgetMove(IPokemon pokemon, Moves moveToLearn);
		/// <summary>
		/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
		/// </summary>
		/// <param name=""></param>
		/// <param name="message"></param>
		int pbChooseMove(IPokemon pokemon, string message);
		string pbNameEntry(string helptext, IPokemon pokemon);
		void pbSelectBattler(int index, int selectmode = 1);
		//int pbFirstTarget(int index, int targettype);
		int pbFirstTarget(int index, Attack.Data.Targets targettype);
		void pbUpdateSelected(int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype">Which targets are selectable as option</param>
		int pbChooseTarget(int index, Attack.Data.Targets targettype);
		int pbSwitch(int index, bool lax, bool cancancel);
		void pbDamageAnimation(IBattler pkmn, float effectiveness);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		//void pbHPChanged(int pkmn, int oldhp, bool anim = false);
		//void HPChanged(int index, int oldhp, bool animate = false);
		IEnumerator pbHPChanged(IBattler pkmn, int oldhp, bool animate = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name=""></param>
		IEnumerator pbFainted(int pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		int pbChooseNewEnemy(int index, IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		void pbWildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		void pbTrainerBattleSuccess();
		void pbEXPBar(IPokemon pokemon, IBattler battler, int startexp, int endexp, int tempexp1, int tempexp2);
		void pbShowPokedex(Pokemons species, int form = 0);
		void pbChangeSpecies(IBattler attacker, Pokemons species);
		void ChangePokemon();
		void pbChangePokemon(IBattler attacker, Monster.Forms pokemon);
		void pbSaveShadows();
		void pbFindAnimation(Moves moveid, int userIndex, int hitnum);
		void pbCommonAnimation(string animname, IBattler user, IBattler target, int hitnum = 0);
		void pbAnimation(Moves moveid, IBattler user, IBattler target, int hitnum = 0);
		void pbAnimationCore(string animation, IBattler user, IBattler target, bool oppmove = false);
		void pbLevelUp(IPokemon pokemon, IBattler battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef);
		void pbThrowAndDeflect(Items ball, int targetBattler);
		void pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false);
		void pbThrowSuccess();
		void pbHideCaptureBall();
		void pbThrowBait();
		void pbThrowRock();
	}

	public interface IPokeBattleArena_Scene : PokemonUnity.IPokeBattle_Scene
	{
		void pbBattleArenaBattlers(IBattler battler1, IBattler battler2);
		void pbBattleArenaJudgment(IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);
		/// <summary>
		/// </summary>
		/// <param name="window">infowindow as `SpriteWindow_Base` to display the results</param>
		/// <param name="phase"></param>
		/// <param name="battler1"></param>
		/// <param name="battler2"></param>
		/// <param name="ratings1"></param>
		/// <param name="ratings2"></param>
		void updateJudgment(IWindow window, int phase, IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);
	}
	#endregion

	#region Evolution
	public interface IPokemonEvolutionScene : IScene
	{
		void pbEndScreen();
		void pbEvolution(bool cancancel = true);
		void pbFlashInOut(bool canceled,string oldstate,string oldstate2);
		void pbStartScreen(IPokemon pokemon, Pokemons newspecies);
		void pbUpdate(bool animating = false);
		void pbUpdateExpandScreen();
		void pbUpdateNarrowScreen();
	}
	#endregion

	#region Pokedex
	public interface IPokemonPokedex : IScreen
	{
		void initialize(IPokemonPokedexScene scene);
		void pbDexEntry(Pokemons species);
		void pbStartScreen();
	}
	public interface IPokemonPokedexScene : IScene
	{
		void pbUpdate();
		void pbEndScene();
		void pbStartScene();
		void pbStartDexEntryScene(Pokemons species);
		void pbPokedex();
		void pbDexEntry(int index);
		int pbDexSearch();
		void pbCloseSearch();
		IEnumerable<Monster.Data.PokemonData> pbSearchDexList(params object[] param);
		List<Monster.Data.PokemonData> pbGetDexList();
		void pbRefreshDexList(int index = 0);
		void pbRefreshDexSearch(params string[] param);
		bool pbCanAddForModeList(int mode, Pokemons nationalSpecies);
		bool pbCanAddForModeSearch(int mode, Pokemons nationalSpecies);
		void pbChangeToDexEntry(Pokemons species);
		int pbDexSearchCommands(string[] commands, int selitem, string[] helptexts = null);
		int pbGetPokedexRegion();
		int pbGetSavePositionIndex();
		void pbMiddleDexEntryScene();
		void setIconBitmap(Pokemons species);
	}
	/// <summary>
	/// Shows the "Nest" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonNestMapScene : IScene
	{
		void pbUpdate();
		void pbEndScene();
		void pbStartScene(Pokemons species,int regionmap= -1);
		/// <summary>
		/// </summary>
		/// <param name="listlimits">an enum that represents end of list</param>
		/// <returns></returns>
		int pbMapScene(int listlimits);
	}
	public interface IPokemonNestMap : IScreen
	{
		void initialize(IPokemonNestMapScene scene);
		void pbStartScreen(Pokemons species,int region,int listlimits);
	}
	/// <summary>
	/// Shows the "Form" page of the Pokédex entry screen.
	/// </summary>
	public interface IPokemonFormScene : IScene
	{
		void pbUpdate();
		//void pbRefresh();
		List<PokemonUnity.Monster.Forms> pbGetAvailable(); //returns [Name, Gender, Form] 
		List<string> pbGetCommands();
		void pbChooseForm();
		void pbEndScene();
		void pbStartScene(Pokemons species);
		int pbControls(int listlimits);
	}
	public interface IPokemonForm : IScreen
	{
		void initialize(IPokemonFormScene scene);
		void pbStartScreen(Pokemons species,int listlimits);
	}
	#endregion

	#region Pause Menu
	public interface IPokemonMenu_Scene : IScene
	{
		void pbEndScene();
		void pbHideMenu();
		//void pbRefresh();
		void pbShowCommands(string[] commands);
		void pbShowHelp(string text);
		void pbShowInfo(string text);
		void pbShowMenu();
		void pbStartScene();
	}

	public interface IPokemonMenu : IScreen
	{
		void initialize(IPokemonMenu_Scene scene);
		void pbShowMenu();
		void pbStartPokemonMenu();
	}
	#endregion

	#region Bag
	public interface IPokemonBag_Scene : IScene
	{
		void update();
		void pbStartScene(Bag bag);
		void pbEndScene();
		int pbChooseNumber(string helptext, int maximum);
		void pbDisplay(string msg, bool brief = false);
		void pbConfirm(string msg);
		void pbShowCommands(string helptext, int commands);
		//void pbRefresh();

		// Called when the item screen wants an item to be chosen from the screen
		Items pbChooseItem(bool lockpocket = false);
	}

	public interface IPokemonBagScreen : IScreen
	{
		IPokemonBagScreen initialize(IPokemonBag_Scene scene, Bag bag);
		void pbDisplay(string text);
		void pbConfirm(string text);

		// UI logic for the item screen when an item is to be held by a Pokémon.
		Items pbGiveItemScreen();

		// UI logic for the item screen when an item is used on a Pokémon from the party screen.
		Items pbUseItemScreen(Pokemons pokemon);

		// UI logic for the item screen for choosing an item
		Items pbChooseItemScreen();

		// UI logic for the item screen for choosing a Berry
		Items pbChooseBerryScreen();

		// UI logic for tossing an item in the item screen.
		void pbTossItemScreen();

		// UI logic for withdrawing an item in the item screen.
		void pbWithdrawItemScreen();

		// UI logic for depositing an item in the item screen.
		void pbDepositItemScreen();
		Items pbStartScreen();
	}
	#endregion

	#region PC Pokemon Storage
	public interface IPokemonStorageScene : IScene
	{
		//void drawMarkings(bitmap , float x, float y, float width, float height, bool[] markings);
		void drawMarkings(bool[] markings);
		string[] getMarkingCommands(bool[] markings);
		IPokemonStorageScene initialize();
		void pbBoxName(string helptext, int minchars, int maxchars);
		void pbChangeBackground(int wp);
		int pbChangeSelection(int key, int selection);
		void pbChooseBox(string msg);
		Items pbChooseItem(IBattler bag);
		void pbCloseBox();
		//void pbDisplay(string message);
		void pbDropDownPartyTab();
		void pbHardRefresh();
		void pbHidePartyTab();
		void pbHold(KeyValuePair<int, int> selected);
		void pbJumpToBox(int newbox);
		void pbMark(KeyValuePair<int, int> selected, Pokemon heldpoke);
		int pbPartyChangeSelection(int key, int selection);
		void pbPartySetArrow(int selection);//arrow , 
		void pbPlace(KeyValuePair<int, int> selected, Pokemon heldpoke);
		//void pbRefresh();
		void pbRelease(KeyValuePair<int, int> selected, Pokemon heldpoke);
		int[] pbSelectBox(Pokemon[] party);
		int[] pbSelectBoxInternal(Pokemon[] party);
		int pbSelectParty(Pokemon[] party);
		int pbSelectPartyInternal(Pokemon[] party, bool depositing);
		void pbSetArrow(int selection);//arrow , 
		void pbSetMosaic(int selection);
		int pbShowCommands(string message, string[] commands, int index = 0);
		void pbStartBox(IPokemonStorageScreen screen, int command);
		void pbStore(KeyValuePair<int, int> selected, Pokemon heldpoke, int destbox, int firstfree);
		void pbSummary(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void pbSwap(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void pbSwitchBoxToLeft(int newbox);
		void pbSwitchBoxToRight(int newbox);
		void pbUpdateOverlay(int selection, Pokemon[] party = null);
		void pbWithdraw(KeyValuePair<int, int> selected, Pokemon heldpoke, int partyindex);
	}

	public interface IPokemonStorageScreen : IScreen
	{
		int pbAbleCount { get; }
		Pokemon pbHeldPokemon { get; }
		IPokemonStorageScene scene { get; }
		Character.PokemonStorage storage { get; }

		void debugMenu(KeyValuePair<int, int> selected, Pokemon pkmn, Pokemon heldpoke);
		void IPokemonStorageScreen(IPokemonStorageScene scene, Character.PokemonStorage storage);
		bool pbAble(Pokemon pokemon);
		void pbBoxCommands();
		int? pbChoosePokemon(Pokemon[] party = null);
		bool pbConfirm(string str);
		void pbDisplay(string message);
		void pbHold(KeyValuePair<int, int> selected);
		void pbItem(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void pbMark(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void pbPlace(KeyValuePair<int, int> selected);
		void pbRelease(KeyValuePair<int, int> selected, Pokemon heldpoke);
		int pbShowCommands(string msg, string[] commands);
		void pbStartScreen(int command);
		void pbStore(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void pbSummary(KeyValuePair<int, int> selected, Pokemon heldpoke);
		bool pbSwap(KeyValuePair<int, int> selected);
		bool pbWithdraw(KeyValuePair<int, int> selected, Pokemon heldpoke);
		void selectPokemon(int index);
	}
	#endregion

	#region Summary
	public interface IPokemonSummaryScene : IScene
	{
		//IPokemonSummaryScene initialize();
		//void drawMarkings(bitmap,int x,int y,int width,int height, bool[] markings);
		void drawPageOne(IPokemon pokemon);
		void drawPageOneEgg(IPokemon pokemon);
		void drawPageTwo(IPokemon pokemon);
		void drawPageThree(IPokemon pokemon);
		void drawPageFour(IPokemon pokemon);
		void drawPageFive(IPokemon pokemon);
		void drawMoveSelection(IPokemon pokemon, Moves moveToLearn);
		void drawSelectedMove(IPokemon pokemon, Moves moveToLearn, Moves moveid);
		void pbChooseMoveToForget(Moves moveToLearn);
		void pbEndScene();
		void pbGoToNext();
		void pbGoToPrevious();
		void pbMoveSelection();
		void pbPokerus(IPokemon pkmn);
		void pbScene();
		void pbStartForgetScene(IPokemon party, int partyindex, Moves moveToLearn);
		void pbStartScene(IPokemon party, int partyindex);
		void pbUpdate();
	}

	public interface IPokemonSummary : IScreen
	{
		IPokemonSummary initialize(IPokemonSummaryScene scene);
		void pbStartScreen(IPokemon[] party, int partyindex);
		//int pbStartForgetScreen(IPokemon[] party, int partyindex, Moves moveToLearn);
		int pbStartForgetScreen(IPokemon party, int partyindex, Moves moveToLearn);
		void pbStartChooseMoveScreen(IPokemon[] party, int partyindex, string message);
	}
	#endregion

	#region Pokemon Party
	public interface IPartyDisplayScene : IScene
	{
		//IPartyDisplayScene initialize();
		void pbShowCommands(string helptext, string[] commands,int index= 0);
		void update();
		void pbSetHelpText(string helptext);
		void pbStartScene(IPokemon[] party,string starthelptext,string[] annotations= null,bool multiselect= false);
		void pbEndScene();
		/// <summary>
		/// </summary>
		/// <param name="key">Controller Input Key</param>
		/// <param name="currentsel"></param>
		void pbChangeSelection(int key,int currentsel);
		//void pbRefresh();
		void pbHardRefresh();
		void pbPreSelect(IPokemon pkmn);
		void pbChoosePokemon(bool switching= false, int initialsel= -1);
		void pbSelect(Items item);
		//void pbDisplay(string text);
		void pbSwitchBegin(int oldid,int newid);
		void pbSwitchEnd(int oldid,int newid);
		void pbDisplayConfirm(string text);
		void pbAnnotate(string[] annot);
		void pbSummary(int pkmnid);
		void pbChooseItem(Items[] bag);
		void pbUseItem(Items[] bag,IPokemon pokemon); 
		void pbMessageFreeText(string text,string startMsg,int maxlength);
	}

	public interface IPartyDisplayScreen : IScreen
	{
		IPartyDisplayScreen initialize(IPartyDisplayScene scene, IPokemon[] party);
		void pbHardRefresh();
		void pbRefresh();
		void pbRefreshSingle(int i);
		void pbDisplay(string text);
		void pbConfirm(string text);
		void pbSwitch(int oldid,int newid);
		void pbMailScreen(Items item, IPokemon pkmn, int pkmnid);
		void pbTakeMail(IPokemon pkmn);
		void pbGiveMail(Items item,IPokemon pkmn,int pkmnid= 0);
		void pbPokemonGiveScreen(Items item);
		void pbPokemonGiveMailScreen(int mailIndex);
		void pbStartScene(string helptext,bool doublebattle,string[] annotations= null);
		int pbChoosePokemon(string helptext= null);
		void pbChooseMove(IPokemon pokemon,string helptext);
		void pbEndScene();
		/// <summary>
		/// Checks for identical species
		/// </summary>
		/// <param name=""></param>
		void pbCheckSpecies(Pokemons[] array);
		/// <summary>
		/// Checks for identical held items
		/// </summary>
		/// <param name=""></param>
		void pbCheckItems(Items[] array);
		void pbPokemonMultipleEntryScreenEx(string[] ruleset);
		int pbChooseAblePokemon(Func<IPokemon,bool> ableProc,bool allowIneligible= false);
		void pbRefreshAnnotations(bool ableProc);
		void pbClearAnnotations();
		void pbPokemonDebug(IPokemon pkmn, int pkmnid);
		void pbPokemonScreen();
	}
	#endregion
}