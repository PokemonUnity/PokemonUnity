using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Interface;
using PokemonUnity.Combat;
using PokemonUnity.Character;
using PokemonUnity.Inventory;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	#region Battle UI Components
	/// <summary>
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	public interface ICommandMenuDisplay : IDisposable
	{
		int mode { get; set; }
		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
		float ox { get; set; }
		float oy { get; set; }
		bool visible { get; set; }
		IColor color { get; set; }

		ICommandMenuDisplay initialize(IViewport viewport = null);

		bool disposed { get; }

		//void dispose();

		int index { get; set; }

		void setTexts(params string[] value);

		void refresh();

		void update();
	}

	public interface ICommandMenuButtons : IBitmapSprite {
		new ICommandMenuButtons initialize(int index = 0, int mode = 0, IViewport viewport = null);

		//void dispose();

		void update(int index = 0, int mode = 0);

		void refresh(int index, int mode = 0);
	}

	/// <summary>
	/// Fight menu (choose a move)
	/// </summary>
	public interface IFightMenuDisplay : IDisposable
	{
		IBattler battler			{ get; set; }
		int index					{ get; }
		int megaButton				{ get; set; }

		float x { get; set; }
		float y { get; set; }
		float z { get; set; }
		float ox { get; set; }
		float oy { get; set; }
		bool visible { get; set; }
		IColor color { get; set; }

		IFightMenuDisplay initialize(IBattler battler, IViewport viewport = null);

		bool disposed { get; }

		//void dispose();

		bool setIndex(int value);

		void refresh();

		void update();
	}

	public interface IFightMenuButtons : IDisposable, IBitmapSprite
	{
		IFightMenuButtons initialize(int index = 0, IBattleMove[] moves = null, IViewport viewport = null);

		//void dispose();

		void update(int index= 0, IBattleMove[] moves= null, int megaButton= 0);

		void refresh(int index, IBattleMove[] moves, int megaButton);
	}

	/// <summary>
	/// Data box for safari battles
	/// </summary>
	public interface ISafariDataBox : ISpriteWrapper
	{
		int selected		{ get; }
		bool appearing		{ get; }

		ISafariDataBox initialize(IBattle battle, IViewport viewport = null);

		//void appear();

		void refresh();

		//void update();
	}

	/// <summary>
	/// Data box for regular battles (both single and double)
	/// </summary>
	public interface IPokemonDataBox : ISpriteWrapper, ISafariDataBox
	{
		IBattler battler		{ get; }
		new int selected		{ get; set; }
		//bool appearing			{ get; }
		bool animatingHP		{ get; }
		bool animatingEXP		{ get; }

		IPokemonDataBox initialize(IBattler battler, bool doublebattle, IViewport viewport = null);

		//void dispose();

		void refreshExpLevel();

		int Exp { get; }

		int HP { get; }

		void animateHP(int oldhp, int newhp);

		void animateEXP(int oldexp, int newexp);

		//void appear();

		//void refresh();

		//void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s)'s Pokémon being thrown out.  It appears at coords
	/// (@spritex,@spritey), and moves in y to @endspritey where it stays for the rest
	/// of the battle, i.e. the latter is the more important value.
	/// Doesn't show the ball itself being thrown.
	/// </summary>
	public interface IPokeballSendOutAnimation : IDisposable
	{
		//SPRITESTEPS=10;
		//STARTZOOM=0.125;

		//IPokeballSendOutAnimation initialize(ISprite sprite, int spritehash, IPokemon pkmn, IPokemon illusionpoke, bool doublebattle);
		//IPokeballSendOutAnimation initialize(IPokemonBattlerSprite sprite, IDictionary<string,ISprite> spritehash, IBattler pkmn, IPokemon illusionpoke, bool doublebattle);
		IPokeballSendOutAnimation initialize(IPokemonBattlerSprite sprite, IDictionary<string,object> spritehash, IBattler pkmn, IPokemon illusionpoke, bool doublebattle);

		bool disposed { get; }

		bool animdone { get; }

		void dispose();

		void update();
	}

	/// <summary>
	/// Shows the player's (or partner's) Pokémon being thrown out.  It appears at
	/// (@spritex,@spritey), and moves in y to @endspritey where it stays for the rest
	/// of the battle, i.e. the latter is the more important value.
	/// Doesn't show the ball itself being thrown.
	/// </summary>
	public interface IPokeballPlayerSendOutAnimation : IDisposable
	{
		//  Ball curve: 8,52; 22,44; 52, 96
		//  Player: new Color(16*8,23*8,30*8)
		//SPRITESTEPS=10;
		//STARTZOOM=0.125;

		IPokeballPlayerSendOutAnimation initialize(ISprite sprite, int spritehash, IPokemon pkmn, IPokemon illusionpoke, bool doublebattle);

		bool disposed { get; }

		bool animdone { get; }

		void dispose();

		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s) and the enemy party lineup sliding off screen.
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	public interface ITrainerFadeAnimation
	{
		//ITrainerFadeAnimation initialize(ISprite[] sprites);
		//ITrainerFadeAnimation initialize(IDictionary<string,ISprite> sprites);
		ITrainerFadeAnimation initialize(IDictionary<string,object> sprites);

		bool animdone { get; }

		void update();
	}

	/// <summary>
	/// Shows the player (and partner) and the player party lineup sliding off screen.
	/// Shows the player's/partner's throwing animation (if they have one).
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	public interface IPlayerFadeAnimation
	{
		IPlayerFadeAnimation initialize(ISprite[] sprites);

		bool animdone { get; }

		void update();
	}
	#endregion

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
	//void pokeballThrow(Items ball, int shakes, bool critical, IBattler targetBattler, IScene scene, IBattler battler, int burst = -1, bool showplayer = false);

	#region Pokemon Battle
	/// <summary>
	///
	/// </summary>
	public interface ISceneHasChatter
	{
		void Chatter(IBattler attacker,IBattler opponent);
	}

	public interface IPokeBattle_Scene : IScene, ISceneHasChatter, IPokeBattle_DebugScene
	{
		//event EventHandler<PokeballThrowTargetArgs> OnPokeballThrown;
		/*
		-  def ChooseNewEnemy(int index,party)
		Use this method to choose a new Pokémon for the enemy
		The enemy's party is guaranteed to have at least one
		choosable member.
		index - Index to the battler to be replaced (use e.g. @battle.battlers[index] to
		access the battler)
		party - Enemy's party

		- def WildBattleSuccess
		This method is called when the player wins a wild Pokémon battle.
		This method can change the battle's music for example.

		- def TrainerBattleSuccess
		This method is called when the player wins a Trainer battle.
		This method can change the battle's music for example.

		- def Fainted(pkmn)
		This method is called whenever a Pokémon faints.
		pkmn - PokeBattle_Battler object indicating the Pokémon that fainted

		- def ChooseEnemyCommand(int index)
		Use this method to choose a command for the enemy.
		index - Index of enemy battler (use e.g. @battle.battlers[index] to
		access the battler)
		*/

		new IPokeBattle_Scene initialize();
		void Update();
		void GraphicsUpdate();
		void InputUpdate();
		void ShowWindow(int windowtype);
		void SetMessageMode(bool mode);
		void WaitMessage();
		void Display(string msg, bool brief = false);
		//new void DisplayMessage(string msg, bool brief = false);
		//new void DisplayPausedMessage(string msg);
		//new bool DisplayConfirmMessage(string msg);
		//void ShowCommands(string msg, string[] commands, int defaultValue);
		//new void ShowCommands(string msg, string[] commands, bool canCancel);
		//new void FrameUpdate(object cw = null);
		//void Refresh();
		IIconSprite AddSprite(string id, float x, float y, string filename, IViewport viewport);
		void AddPlane(string id, string filename, IViewport viewport);
		void DisposeSprites();
		//new void BeginCommandPhase();
		//new IEnumerator ShowOpponent(int index);
		//new IEnumerator HideOpponent();
		void ShowHelp(string text);
		void HideHelp();
		void Backdrop();
		/// <summary>
		/// Returns whether the party line-ups are currently appearing on-screen
		/// </summary>
		/// <returns></returns>
		bool inPartyAnimation { get; }
		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		void partyAnimationUpdate();
		//new void StartBattle(IBattle battle);
		//new void EndBattle(BattleResults result);
		//new void Recall(int battlerindex);
		void TrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		void SendOut(int battlerindex, IPokemon pkmn);
		//void TrainerWithdraw(IBattle battle, IBattler pkmn);
		//void Withdraw(IBattle battle, IBattler pkmn);
		//new string MoveString(string move);
		//new void BeginAttackPhase();
		void SafariStart();
		void ResetCommandIndices();
		void ResetMoveIndex(int index);
		int SafariCommandMenu(int index);
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
		int ChooseMove(IPokemon pokemon, string message);
		string NameEntry(string helptext, IPokemon pokemon);
		void SelectBattler(int index, int selectmode = 1);
		//int FirstTarget(int index, int targettype);
		//int FirstTarget(int index, PokemonUnity.Attack.Targets targettype);
		void UpdateSelected(int index);
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
		//void HPChanged(int pkmn, int oldhp, bool anim = false);
		//void HPChanged(int index, int oldhp, bool animate = false);
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
		//new void WildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new void TrainerBattleSuccess();
		//new void EXPBar(IBattler battler, IPokemon pokemon, int startexp, int endexp, int tempexp1, int tempexp2);
		void ShowPokedex(Pokemons species, int form = 0);
		void ChangePokemon(IBattler attacker, IPokemon pokemon);
		//void ChangePokemon(IBattler attacker, PokemonUnity.Monster.Forms pokemon);
		void SaveShadows(Action action = null);
		KeyValuePair<string, bool>? FindAnimation(Moves moveid, int userIndex, int hitnum);
		void CommonAnimation(string animname, IBattler user, IBattler target, int hitnum = 0);
		//new void Animation(Moves moveid, IBattler user, IBattler target, int hitnum = 0);
		void AnimationCore(string animation, IBattler user, IBattler target, bool oppmove = false);
		//void LevelUp(IBattler battler, IPokemon pokemon, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef);
		void ThrowAndDeflect(Items ball, int targetBattler);
		void Throw(Items ball, int shakes, bool critical, int targetBattler, bool showplayer = false);
		void ThrowSuccess();
		void HideCaptureBall();
		void ThrowBait();
		void ThrowRock();
	}

	public interface IPokeBattleArena_Scene : IScene, IPokeBattle_Scene
	{
		void BattleArenaBattlers(IBattler battler1, IBattler battler2);
		void BattleArenaJudgment(IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);
		/// <summary>
		/// </summary>
		/// <param name="window">infoWindow as `SpriteWindow_Base` to display the results</param>
		/// <param name="phase"></param>
		/// <param name="battler1"></param>
		/// <param name="battler2"></param>
		/// <param name="ratings1"></param>
		/// <param name="ratings2"></param>
		void updateJudgment(IWindow window, int phase, IBattler battler1, IBattler battler2, int[] ratings1, int[] ratings2);
	}
	#endregion

	#region Debugging
	public interface IPokeBattle_DebugScene : IScene, IPokeBattle_DebugSceneNoLogging
	{
		//new void DisplayMessage(string msg,bool brief= false);
		//new void DisplayPausedMessage(string msg);
		//new void DisplayConfirmMessage(string msg);
		void FrameUpdate(IViewport cw);
		//void Refresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		//new void BeginCommandPhase();
		//new void StartBattle(IBattle battle);
		//new void EndBattle(PokemonUnity.Combat.BattleResults result);
		//new void TrainerSendOut(IBattle battle,IPokemon pkmn);
		//new void SendOut(IBattle battle,IPokemon pkmn);
		//new void TrainerWithdraw(IBattle battle,IPokemon pkmn);
		//new void Withdraw(IBattle battle,IPokemon pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		//new void ForgetMove(IPokemon pkmn,Moves move);
		//new void BeginAttackPhase();
		//new void CommandMenu(int index);
		/// <summary>
		/// </summary>
		/// <param name="pkmn"></param>
		void PokemonString(IPokemon pkmn);
		string MoveString(IBattleMove move);
		/// <summary>
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		//new void FightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		//new void ItemMenu(int index);

		//void FirstTarget(int index,int targettype);
		/// <summary>
		/// Returns the first selectable target of attack type
		/// from pool of pokemons active in battle scene
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype"></param>
		/// <returns>retuns the index slot of battler</returns>
		int FirstTarget(int index, PokemonUnity.Attack.Targets targettype);
		void NextTarget(int cur,int index);
		void PrevTarget(int cur,int index);
		/// <summary>
		/// Use this method to make the player choose a target
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		//new void ChooseTarget(int index,int targettype);
		//new void Switch(int index,bool lax,bool cancancel);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		//new void HPChanged(IBattler pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		//new void Fainted(IPokemon pkmn);
		//new void ChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		//new void ChooseNewEnemy(int index,IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new void WildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		//new void TrainerBattleSuccess();
		//new void EXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		//new void LevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
		//	int olddefense,int oldspeed,int oldspatk,int oldspdef);
		//new void ShowOpponent(int opp);
		//new void HideOpponent();
		//new void Recall(int battlerindex);
		//new void DamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		//new void Animation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
	}
	public interface IPokeBattle_SceneNonInteractive : IScene
	{
		int CommandMenu(int index);
		int FightMenu(int index);
		Items ItemMenu(int index);
		//int ChooseTarget(int index, int targettype);
		int ChooseTarget(int index, PokemonUnity.Attack.Targets targettype);
		int Switch(int index,bool lax,bool cancancel);
		void ChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		int ChooseNewEnemy(int index,IPokemon[] party);
	}
	public interface IPokeBattle_DebugSceneNoLogging : IScene, ISceneHasChatter, IPokeBattle_DebugSceneNoGraphics
	{
		//new void DisplayMessage(string msg,bool brief= false);
		//new void DisplayPausedMessage(string msg);
		//new bool DisplayConfirmMessage(string msg);
		//new int ShowCommands(string msg,string[] commands,bool defaultValue);
		//new void BeginCommandPhase();
		//new void StartBattle(IBattle battle);
		//new void EndBattle(BattleResults result);
		//new void TrainerSendOut(IBattle battle,IPokemon pkmn);
		//new void SendOut(IBattle battle,IPokemon pkmn);
		//new void TrainerWithdraw(IBattle battle,IPokemon pkmn);
		//new void Withdraw(IBattle battle,IPokemon pkmn);
		//new int ForgetMove(IPokemon pkmn,Moves move);
		//new void BeginAttackPhase();
		//new int CommandMenu(int index);
		//new int FightMenu(int index);
		//new int ItemMenu(int index);
		//new int ChooseTarget(int index,int targettype);
		//void Refresh();
		//new int Switch(int index,bool lax,bool cancancel);
		//new void HPChanged(IBattler pkmn,int oldhp,bool anim= false);
		//new void Fainted(IPokemon pkmn);
		//new void ChooseEnemyCommand(int index);
		//new void ChooseNewEnemy(int index,IPokemon[] party);
		//new void WildBattleSuccess();
		//new void TrainerBattleSuccess();
		//new void EXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		//new void LevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
		//	int olddefense,int oldspeed,int oldspatk,int oldspdef);
		//new int Blitz(int keys);
		//new void Chatter(IBattler attacker,IBattler opponent);
		//new void ShowOpponent(int opp);
		//new void HideOpponent();
		//new void Recall(int battlerindex);
		//new void DamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		//new void BattleArenaJudgment(IBattle b1,IBattle b2, int[] r1, int[] r2);
		//new void BattleArenaBattlers(IBattle b1,IBattle b2);
		//new void CommonAnimation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
		//new void Animation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
	}
	public interface IPokeBattle_DebugSceneNoGraphics : IScene, ISceneHasChatter, IPokeBattle_SceneNonInteractive
	{
		IPokeBattle_DebugSceneNoGraphics initialize();
		void DisplayMessage(string msg, bool brief = false);
		void DisplayPausedMessage(string msg);
		bool DisplayConfirmMessage(string msg);
		bool ShowCommands(string msg, string[] commands, bool defaultValue);
		int ShowCommands(string msg, string[] commands, int defaultValue);
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		void BeginCommandPhase();
		/// <summary>
		/// Called whenever the battle begins
		/// </summary>
		/// <param name="battle"></param>
		void StartBattle(IBattle battle);
		void EndBattle(BattleResults result);
		//void TrainerSendOut(IBattle battle, IPokemon pkmn);
		//void SendOut(IBattle battle, IPokemon pkmn);
		void TrainerWithdraw(IBattle battle, IBattler pkmn);
		void Withdraw(IBattle battle, IBattler pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		int ForgetMove(IPokemon pkmn, Moves move);
		void BeginAttackPhase();
		new int CommandMenu(int index);
		new int FightMenu(int index);
		new Items ItemMenu(int index);
		//new int ChooseTarget(int index, int targettype);
		new int ChooseTarget(int index, PokemonUnity.Attack.Targets targettype);
		//void Refresh();
		//new int Switch(int index, bool lax, bool cancancel);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		void HPChanged(IBattler pkmn, int oldhp, bool anim = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		void Fainted(IBattler pkmn);
		//new void ChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		//new void ChooseNewEnemy(int index, IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		void WildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		void TrainerBattleSuccess();
		void EXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2);
		void LevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack,
			int olddefense, int oldspeed, int oldspatk, int oldspdef);
		int Blitz(int keys);
		//void Chatter(IBattler attacker, IBattler opponent);
		void ShowOpponent(int opp);
		void HideOpponent();
		void Recall(int battlerindex);
		void DamageAnimation(IBattler pkmn, TypeEffective effectiveness);
		void BattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2);
		void BattleArenaBattlers(IBattle b1, IBattle b2);
		void CommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0);
		void Animation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0);
	}
	#endregion
}