using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Combat;
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

namespace PokemonUnity.Interface.UnityEngine
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
		//IEnumerator CommandMenuEx(int index, string[] texts, int mode, System.Action<int> result);
		#region Override `void` Animations with Unity API and Coroutines
		new IPokeBattle_SceneIE initialize();
		//void Update();
		//void GraphicsUpdate();
		//void InputUpdate();
		//void ShowWindow(int windowtype);
		//void SetMessageMode(bool mode);
		new IEnumerator WaitMessage();
		//void Display(string msg, bool brief = false);
		new IEnumerator Display(string msg, bool brief = false);
		//new void DisplayMessage(string msg, bool brief = false);
		//new void DisplayPausedMessage(string msg);
		//new bool DisplayConfirmMessage(string msg);
		//void ShowCommands(string msg, string[] commands, int defaultValue);
		IEnumerator ShowCommands(string msg, string[] commands, int defaultValue, System.Action<int> result);
		//new void ShowCommands(string msg, string[] commands, bool canCancel);
		//IEnumerator ShowCommands(string msg, string[] commands, bool canCancel, System.Action<int> result);
		void FrameUpdate(IGameObject cw);
		//void Refresh();
		//IIconSprite AddSprite(string id, float x, float y, string filename, IViewport viewport);
		//void AddPlane(string id, string filename, IViewport viewport);
		//void DisposeSprites();
		//new void BeginCommandPhase();
		//new IEnumerator ShowOpponent(int index);
		//new IEnumerator HideOpponent();
		//new IEnumerator ShowHelp(string text);
		//new IEnumerator HideHelp();
		//new IEnumerator Backdrop();
		/// <summary>
		/// Returns whether the party line-ups are currently appearing on-screen
		/// </summary>
		/// <returns></returns>
		//bool inPartyAnimation { get; }
		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		new IEnumerator partyAnimationUpdate();
		//new IEnumerator StartBattle(IBattle battle);
		//new IEnumerator EndBattle(BattleResults result);
		//new IEnumerator Recall(int battlerindex);
		//new IEnumerator TrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		//new IEnumerator SendOut(int battlerindex, IPokemon pkmn);
		//IEnumerator TrainerWithdraw(IBattle battle, IBattler pkmn);
		//IEnumerator Withdraw(IBattle battle, IBattler pkmn);
		//new string MoveString(string move);
		//new IEnumerator BeginAttackPhase();
		new IEnumerator SafariStart();
		//void ResetCommandIndices();
		//void ResetMoveIndex(int index);
		//int SafariCommandMenu(int index);
		IEnumerator SafariCommandMenu(int index, System.Action<int> result);
		/// <summary>
		/// Use this method to display the list of commands and choose
		/// a command for the player.
		/// </summary>
		/// 0 - Fight, 1 - Pokémon, 2 - Bag, 3 - Run
		/// <param name="index">Index of battler (use e.g. @battle.battlers[index] to
		/// access the battler)</param>
		/// <returns> Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		//new int CommandMenu(int index);
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="texts"></param>
		/// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		//int CommandMenuEx(int index, string[] texts, int mode = 0);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		//new int FightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		//new KeyValuePair<Items, int> ItemMenu(int index);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.  The function
		/// should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="moveToLearn"></param>
		//new int ForgetMove(IPokemon pokemon, Moves moveToLearn);
		/// <summary>
		/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
		/// </summary>
		/// <param name=""></param>
		/// <param name="message"></param>
		//int ChooseMove(IPokemon pokemon, string message);
		IEnumerator ChooseMove(IPokemon pokemon, string message, System.Action<int> result);
		//string NameEntry(string helptext, IPokemon pokemon);
		IEnumerator NameEntry(string helptext, IPokemon pokemon, System.Action<string> result);
		//new IEnumerator SelectBattler(int index, int selectmode = 1);
		//int FirstTarget(int index, int targettype);
		//int FirstTarget(int index, PokemonUnity.Attack.Targets targettype);
		//void UpdateSelected(int index);
		/// <summary>
		/// Use this method to make the player choose a target
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype">Which targets are selectable as option</param>
		//new int ChooseTarget(int index, PokemonUnity.Attack.Targets targettype);
		//new int Switch(int index, bool lax, bool cancancel);
		//void DamageAnimation(IBattler pkmn, float effectiveness);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		//IEnumerator HPChanged(int pkmn, int oldhp, bool anim = false);
		//IEnumerator HPChanged(int index, int oldhp, bool animate = false);
		//IEnumerator HPChanged(IBattler pkmn, int oldhp, bool animate = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name=""></param>
		//IEnumerator Fainted(int pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		//new void ChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		//new int ChooseNewEnemy(int index, IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new IEnumerator WildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new IEnumerator TrainerBattleSuccess();
		//new void EXPBar(IBattler battler, IPokemon pokemon, int startexp, int endexp, int tempexp1, int tempexp2);
		new IEnumerator ShowPokedex(Pokemons species, int form = 0);
		//IEnumerator ChangePokemon(IBattlerIE attacker, IPokemon pokemon);
		//void SaveShadows(Action action = null);
		//IEnumerator FindAnimation(Moves moveid, int userIndex, int hitnum);
		IEnumerator CommonAnimation(string animname, IBattlerIE user, IBattlerIE target, int hitnum = 0);
		//new IEnumerator Animation(Moves moveid, IBattler user, IBattler target, int hitnum = 0);
		IEnumerator AnimationCore(string animation, IBattlerIE user, IBattlerIE target, bool oppmove = false);
		//new IEnumerator LevelUp(IBattler battler, IPokemon pokemon, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef);
		new IEnumerator ThrowAndDeflect(Items ball, int targetBattler);
		new IEnumerator Throw(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false);
		new IEnumerator ThrowSuccess();
		new IEnumerator HideCaptureBall();
		new IEnumerator ThrowBait();
		new IEnumerator ThrowRock();
		#endregion
		#region Override NoGraphics with Unity API and Coroutines
		//new void DisplayMessage(string msg,bool brief= false);
		new IEnumerator DisplayMessage(string msg, bool brief = false);
		//new void DisplayPausedMessage(string msg);
		new IEnumerator DisplayPausedMessage(string msg);
		//new void DisplayConfirmMessage(string msg);
		IEnumerator DisplayConfirmMessage(string msg, System.Action<bool> result);
		//new IEnumerator FrameUpdate(IViewport cw);
		//void Refresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		//new IEnumerator BeginCommandPhase();
		/// <summary>
		/// Called whenever the battle begins
		/// </summary>
		/// <remarks>
		/// This is used to animate the battle intro
		/// (trainer appearing in frame, the challenge dialog,
		/// pokemons appearing, and HUD setup)
		/// </remarks>
		/// <param name="battle"></param>
		//new void StartBattle(IBattle battle);
		IEnumerator StartBattle(IBattleIE battle); //ToDo: Can be void, if using StartCoroutine...
		//new void EndBattle(PokemonUnity.Combat.BattleResults result);
		new IEnumerator EndBattle(PokemonUnity.Combat.BattleResults result);
		//new void TrainerSendOut(IBattle battle,IPokemon pkmn);
		new IEnumerator TrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		//new void SendOut(IBattle battle,IPokemon pkmn);
		new IEnumerator SendOut(int battlerindex, IPokemon pkmn);
		//new void TrainerWithdraw(IBattle battle,IPokemon pkmn);
		IEnumerator TrainerWithdraw(IBattle battle, IBattlerIE pkmn);
		//new void Withdraw(IBattle battle,IPokemon pkmn);
		IEnumerator Withdraw(IBattle battle, IBattlerIE pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		IEnumerator ForgetMove(IPokemon pkmn,Moves move, System.Action<int> result);
		new IEnumerator BeginAttackPhase();
		/// <summary>
		/// Use this method to display the list of commands.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		//new void CommandMenu(int index);
		IEnumerator CommandMenu(int index, System.Action<MenuCommands> result);
		/// <summary>
		/// </summary>
		/// <param name="pkmn"></param>
		//void PokemonString(IPokemon pkmn);
		/// <summary>
		/// Update selected command
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		//new void FightMenu(int index);
		IEnumerator FightMenu(int index, System.Action<int> result);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		//new void ItemMenu(int index);
		IEnumerator ItemMenu(int index, System.Action<Items> result);
		//new IEnumerator FirstTarget(int index, int targettype);
		new IEnumerator NextTarget(int cur, int index);
		new IEnumerator PrevTarget(int cur, int index);
		/// <summary>
		/// Use this method to make the player choose a target
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		//new void ChooseTarget(int index,int targettype);
		IEnumerator ChooseTarget(int index, PokemonUnity.Attack.Targets targettype, System.Action<int> result);
		//new IEnumerator Switch(int index,bool lax,bool cancancel);
		IEnumerator Switch(int index,bool lax,bool cancancel, System.Action<int> result);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		IEnumerator HPChanged(IBattlerIE pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		//new void Fainted(IPokemon pkmn);
		IEnumerator Fainted(IBattlerIE pkmn);
		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		//new void ChooseEnemyCommand(int index);
		//new IEnumerator ChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		//new IEnumerator ChooseNewEnemy(int index,IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new IEnumerator WildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new IEnumerator TrainerBattleSuccess();
		IEnumerator EXPBar(IBattlerIE battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		IEnumerator LevelUp(IBattlerIE battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		//new void ShowOpponent(int opp);
		new IEnumerator ShowOpponent(int index);
		//new void HideOpponent();
		new IEnumerator HideOpponent();
		new IEnumerator Recall(int battlerindex);
		IEnumerator DamageAnimation(IBattlerIE pkmn,TypeEffective effectiveness);
		IEnumerator Animation(Moves moveid,IBattlerIE attacker,IBattlerIE opponent,int hitnum= 0);
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
		/// Current weather, custom methods should use <see cref="Weather"/> instead
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

		//void Abort();
		//IEnumerator DebugUpdate();
		//int Random(int x);
		//int AIRandom(int x);

		#region Initialize battle class.
		new IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer player, ITrainer opponent);
		new IBattleIE initialize(IScene scene, IPokemon[] p1, IPokemon[] p2, ITrainer[] player, ITrainer[] opponent);
		//IBattle initialize(IScene scene, IList<IPokemon> p1, IList<IPokemon> p2, IList<ITrainer> player, IList<ITrainer> opponent);
		#endregion

		#region Info about battle.
		//bool DoubleBattleAllowed();
		//Weather Weather { get; }
		#endregion

		#region Get battler info.
		//bool IsOpposing(int index);
		//bool OwnedByPlayer(int index);
		//bool IsDoubleBattler(int index);
		/// <summary>
		/// Only used for Wish
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pokemonindex"></param>
		/// <returns></returns>
		//string ThisEx(battlerindex, pokemonindex);
		//string ToString(int battlerindex, int pokemonindex);
		/// <summary>
		/// Checks whether an item can be removed from a Pokémon.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		//bool IsUnlosableItem(IBattlerIE pkmn, Items item);
		//IBattlerIE CheckGlobalAbility(Abilities a);
		//int nextPickupUse { get; }
		#endregion

		#region Player-related info.
		//ITrainer Player();
		//Items[] GetOwnerItems(int battlerIndex);
		//void SetSeen(IPokemon pokemon);
		//string GetMegaRingName(int battlerIndex);
		//bool HasMegaRing(int battlerIndex);
		#endregion

		#region Get party info, manipulate parties.
		//int PokemonCount(IPokemon[] party);
		//bool AllFainted(IPokemon[] party);
		//int MaxLevel(IPokemon[] party);
		//int MaxLevelFromIndex(int index);
		///// <summary>
		///// Gets player party of selected battler
		///// </summary>
		///// <param name="index"></param>
		///// <returns>Returns the trainer party of pokemon at this index?</returns>
		//IPokemon[] Party(int index);
		//IPokemon[] OpposingParty(int index);
		//int SecondPartyBegin(int battlerIndex);
		//int PartyLength(int battlerIndex);
		//int FindNextUnfainted(IPokemon[] party, int start, int finish = -1);
		//int GetLastPokeInTeam(int index);
		/// <summary>
		///
		/// </summary>
		/// <param name="pkmnIndex"></param>
		/// <returns></returns>
		new IBattlerIE FindPlayerBattler(int pkmnIndex);
		//bool IsOwner(int battlerIndex, int partyIndex);
		//ITrainer GetOwner(int battlerIndex);
		//ITrainer GetOwnerPartner(int battlerIndex);
		//int GetOwnerIndex(int battlerIndex);
		//bool BelongsToPlayer(int battlerIndex);
		//ITrainer PartyGetOwner(int battlerIndex, int partyIndex);
		//void AddToPlayerParty(IPokemon pokemon);
		//void RemoveFromParty(int battlerIndex, int partyIndex);
		#endregion

		#region Check whether actions can be taken.
		/// <summary>
		/// Check whether actions can be taken.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		//bool CanShowCommands(int idxPokemon);
		#endregion

		#region Attacking.
		/// <summary>
		/// Check whether the fight selection prompt can be shown for pokemon.
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <returns></returns>
		IEnumerator CanShowFightMenu(int idxPokemon, Action<bool> result = null);
		IEnumerator CanChooseMove(int idxPokemon, int idxMove, bool showMessages, bool sleeptalk = false, System.Action<bool> result = null);
		new IEnumerator AutoChooseMove(int idxPokemon, bool showMessages = true);
		IEnumerator RegisterMove(int idxPokemon, int idxMove, bool showMessages = true, System.Action<bool> result = null);
		//bool ChoseMove(int i, Moves move);
		//bool ChoseMoveFunctionCode(int i, PokemonUnity.Attack.Effects code);
		//bool RegisterTarget(int idxPokemon, int idxTarget);
		new IBattlerIE[] Priority(bool ignorequickclaw = false, bool log = false);
		//IEnumerator Priority(bool ignorequickclaw = false, bool log = false, System.Action<IBattlerIE[]> result = null);
		#endregion

		#region Switching Pokémon.
		IEnumerator CanSwitchLax(int idxPokemon, int pkmnidxTo, bool showMessages, System.Action<bool> result = null);
		IEnumerator CanSwitch(int idxPokemon, int pkmnidxTo, bool showMessages, bool ignoremeanlook = false, System.Action<bool> result = null);
		IEnumerator RegisterSwitch(int idxPokemon, int idxOther, System.Action<bool> result = null);
		//bool CanChooseNonActive(int index);
		new IEnumerator Switch(bool favorDraws= false);
		new IEnumerator SendOut(int index, IPokemon pokemon);
		new IEnumerator Replace(int index, int newpoke, bool batonpass = false);
		IEnumerator RecallAndReplace(int index, int newpoke, int newpokename = -1, bool batonpass = false, bool moldbreaker = false, System.Action<bool> result = null);
		new IEnumerator MessagesOnReplace(int index, int newpoke, int newpokename= -1);
		IEnumerator SwitchInBetween(int index, bool lax, bool cancancel, System.Action<int> result = null);
		IEnumerator SwitchPlayer(int index, bool lax, bool cancancel, System.Action<int> result = null);
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
		IEnumerator UseItemOnPokemon(Items item, int pkmnIndex, IBattlerIE userPkmn, IHasDisplayMessageIE scene, System.Action<bool> result);
		/// <summary>
		/// Uses an item on an active Pokémon.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="index"></param>
		/// <param name="userPkmn"></param>
		/// <param name="scene"></param>
		/// <returns></returns>
		bool UseItemOnBattler(Items item, int index, IBattlerIE userPkmn, IHasDisplayMessageIE scene);
		IEnumerator RegisterItem(int idxPokemon, Items idxItem, int? idxTarget = null, System.Action<bool> result = null);
		IEnumerator EnemyUseItem(Items item, IBattlerIE battler);
		#endregion

		#region Fleeing from battle.
		//bool CanRun(int idxPokemon);
		/// <summary>
		/// </summary>
		/// <param name="idxPokemon"></param>
		/// <param name="duringBattle"></param>
		/// <returns>1 = success; -1 = failed</returns>
		IEnumerator Run(int idxPokemon, bool duringBattle=false, System.Action<int> result = null);
		#endregion

		#region Mega Evolve battler.
		//bool CanMegaEvolve(int index);
		//void RegisterMegaEvolution(int index);
		new IEnumerator MegaEvolve(int index);
		#endregion

		#region Primal Revert battler.
		new IEnumerator PrimalReversion(int index);
		#endregion

		#region Call battler.
		new IEnumerator Call(int index);
		#endregion

		#region Gaining Experience.
		new IEnumerator GainEXP();
		IEnumerator GainExpOne(int index, IBattlerIE defeated, int partic, int expshare, bool haveexpall, bool showmessages = true);
		#endregion

		#region Learning a move.
		new IEnumerator LearnMove(int pkmnIndex, Moves move);
		#endregion

		#region Abilities.
		new IEnumerator OnActiveAll();
		IEnumerator OnActiveOne(IBattlerIE pkmn, bool onlyabilities = false, bool moldbreaker = false, System.Action<bool> result = null);
		new IEnumerator PrimordialWeather();
		#endregion

		#region Judging.
		//void JudgeCheckpoint(IBattlerIE attacker, IBattleMove move = null);
		//BattleResults DecisionOnTime();
		//BattleResults DecisionOnTime2();
		//BattleResults DecisionOnDraw();
		//void Judge();
		#endregion

		#region Messages and animations.
		new IEnumerator Display(string msg);
		new IEnumerator DisplayPaused(string msg);
		new IEnumerator DisplayBrief(string msg);
		IEnumerator DisplayConfirm(string msg, System.Action<bool> result);
		new IEnumerator ShowCommands(string msg, string[] commands, bool cancancel = true);
		IEnumerator Animation(Moves move, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0);
		IEnumerator CommonAnimation(string name, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0);
		#endregion

		#region Battle core.
		//IEnumerator StartBattle(bool canlose= false, System.Action<BattleResults> result = null);
		//new IEnumerator StartBattle(bool canlose= false);
		new IEnumerator StartBattleCore(bool canlose);
		#endregion

		#region Command phase.
		IEnumerator CommandMenu(int i, System.Action<MenuCommands> result);
		IEnumerator ItemMenu(int i, System.Action<KeyValuePair<Items, int?>> result);
		//bool AutoFightMenu(int i);
		new IEnumerator CommandPhase();
		#endregion

		#region Attack phase.
		new IEnumerator AttackPhase();
		#endregion

		#region End of round.
		new IEnumerator EndOfRoundPhase();
		#endregion

		#region End of battle.
		//BattleResults EndOfBattle(bool canlose = false);
		#endregion
	}
	#endregion

	#region Combat Match Recorder
	/// <summary>
	/// Recording of a Pokemon Battle
	/// </summary>
	public interface IPokeBattle_RecordedBattleModule<out TBattle> //: PokemonUnity.Combat.IPokeBattle_RecordedBattleModule<out TBattle>
	{
		IEnumerator AutoChooseMove(int i1, bool showMessages = true);
		IEnumerator RegisterMove(int i1, int i2, bool showMessages, System.Action<bool> result);
	}

	/// <summary>
	/// Playback Recorded Pokemon Battle
	/// </summary>
	/// <typeparam name="IRecord"></typeparam>
	public interface IPokeBattle_BattlePlayerModule<out IRecord> //: PokemonUnity.Combat.IPokeBattle_BattlePlayerModule<out IRecord>
	{
		IEnumerator CommandPhaseCore();
	}
	#endregion

	#region
	public interface IBattleMoveIE : PokemonEssentials.Interface.PokeBattle.IBattleMove
	{
		//Moves id { get; set; }
		new IBattleIE battle { get; set; }
		/*string Name { get; }
		//int function { get; set; }
		PokemonUnity.Attack.Effects Effect { get; }
		int basedamage { get; set; }
		Types Type { get; set; }
		int Accuracy { get; set; }
		int AddlEffect { get; }
		PokemonUnity.Attack.Category Category { get; } //ToDo: Move to application layer
		PokemonUnity.Attack.Targets Target { get; set; }
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
		IBattleMoveIE FromMove(IBattleIE battle, IMove move);
		#endregion

		#region About the move
		//int totalpp();
		//int addlEffect();

		//int ToInt();
		Types ModifyType(Types type, IBattlerIE attacker, IBattlerIE opponent);
		Types GetType(Types type, IBattlerIE attacker, IBattlerIE opponent);
		/*bool IsPhysical(Types type);
		bool IsSpecial(Types type);
		bool IsStatus { get; }
		bool IsDamaging();*/
		bool TargetsMultiple(IBattlerIE attacker);
		int GetPriority(IBattlerIE attacker);
		int NumHits(IBattlerIE attacker);
		//bool IsMultiHit();
		bool TwoTurnAttack(IBattlerIE attacker);
		void AdditionalEffect(IBattlerIE attacker, IBattlerIE opponent);
		/*bool CanUseWhileAsleep();
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
		bool TypeImmunityByAbility(Types type, IBattlerIE attacker, IBattlerIE opponent);
		float TypeModifier(Types type, IBattlerIE attacker, IBattlerIE opponent);
		double TypeModMessages(Types type, IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region This move's accuracy check
		int ModifyBaseAccuracy(int baseaccuracy, IBattlerIE attacker, IBattlerIE opponent);
		bool AccuracyCheck(IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region Damage calculation and modifiers
		bool CritialOverride(IBattlerIE attacker, IBattlerIE opponent);
		bool IsCritical(IBattlerIE attacker, IBattlerIE opponent);
		int BaseDamage(int basedmg, IBattlerIE attacker, IBattlerIE opponent);
		double BaseDamageMultiplier(double damagemult, IBattlerIE attacker, IBattlerIE opponent);
		double ModifyDamage(double damagemult, IBattlerIE attacker, IBattlerIE opponent);
		int CalcDamage(IBattlerIE attacker, IBattlerIE opponent, params byte[] options); //= new int[] { 0 }
		int ReduceHPDamage(int damage, IBattlerIE attacker, IBattlerIE opponent);
		#endregion

		#region Effects
		void EffectMessages(IBattlerIE attacker, IBattlerIE opponent, bool ignoretype = false, int[] alltargets = null);
		int EffectFixedDamage(int damage, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
		IEnumerator GetEffect(IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true, System.Action<int> result = null);
		IEnumerator EffectAfterHit(IBattlerIE attacker, IBattlerIE opponent, IEffectsMove turneffects);
		#endregion

		#region Using the move
		bool OnStartUse(IBattlerIE attacker);
		void AddTarget(IList<IBattlerIE> targets, IBattlerIE attacker);
		int DisplayUseMessage(IBattlerIE attacker);
		IEnumerator ShowAnimation(Moves id, IBattlerIE attacker, IBattlerIE opponent, int hitnum = 0, int[] alltargets = null, bool showanimation = true);
		void OnDamageLost(int damage, IBattlerIE attacker, IBattlerIE opponent);
		bool MoveFailed(IBattlerIE attacker, IBattlerIE opponent);
		#endregion
	}
	#endregion

	#region
	public interface IHasDisplayMessageIE
	{
		IEnumerator Display(string v);
		//ToDo: rename to "DisplayConfirmMessage"?
		IEnumerator DisplayConfirm(string v, System.Action<bool> result);
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