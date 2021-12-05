using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Combat;
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
	/// <summary>
	/// </summary>
	/// Screen should be renamed to `State`, as it's more in line with FSM
	public interface IScreen
	{
		//IEnumerator update();
	}

	/// <summary>
	/// Common UI functions used in both the Bag and item storage screens.
	/// Allows the user to choose a number. The window _helpwindow_ will
	/// display the _helptext_.
	/// </summary>
	public interface UIHelper {
		int pbChooseNumber(IWindow helpwindow, string helptext, int maximum);

		void pbDisplayStatic(IWindow msgwindow, string message);

		/// <summary>
		/// Letter by letter display of the message <paramref name="msg"/> by the window <paramref name="helpwindow"/>.
		/// </summary>
		/// <param name="helpwindow"></param>
		/// <param name="msg"></param>
		/// <param name="brief"></param>
		/// <returns></returns>
		IEnumerator pbDisplay(IWindow helpwindow, string msg, bool brief);

		/// <summary>
		/// Letter by letter display of the message <paramref name="msg"/> by the window <paramref name="helpwindow"/>,
		/// used to ask questions.
		/// </summary>
		/// <param name="helpwindow"></param>
		/// <param name="msg"></param>
		/// <returns>Returns true if the user chose yes, false if no.</returns>
		bool pbConfirm(IWindow helpwindow, string msg);

		int pbShowCommands(IWindow helpwindow, string helptext, string[] commands);
	}

	public interface IEntity
	{
	}
	public interface IHasDisplayMessage
	{
		void pbDisplay(string v);
	}
	/// <summary>
	/// A scene basically represents unity (or any frontend) where code pauses 
	/// for user interaction (animation, and user key inputs).
	/// </summary>
	/// <remarks>
	/// When code has a scene variable calling a method in middle of script
	/// everything essentially comes to a hault as the frontend takes over 
	/// and the code awaits a result or response to begin again.
	/// </remarks>
	public interface IScene : IHasDisplayMessage
	{
		/// <summary>
		/// Represents the unique id for given scene.
		/// Used for loading scenes in unity.
		/// </summary>
		int Id { get; }

		void pbRefresh();

		/// <summary>
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		//void pokeballThrow(Items ball, int shakes,bool critical,IBattler targetBattler,IScene scene,IBattler battler, int burst = -1, bool showplayer = false);
		//void pbDisplay(string v);
		//bool pbConfirm(string v);
	}
	/*
	/// <summary>
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	public interface ICommandMenuDisplay : IDisposable
	{
		ICommandMenuDisplay initialize(IViewport viewport= null);
		double x; //@window.x;
		double y; //@window.y;
		double z; //@window.z;
		double ox; //@window.ox;
		double oy; //@window.oy;
		bool visible; //@window.visible;
		int color; //@window.color;
		bool disposed();
		void dispose();
		int index; //@window.index;

		void setTexts(string value);
		void refresh();
		void update();
	}

	public interface ICommandMenuButtons //: IBitmapSprite, IDisposable
	{
		ICommandMenuButtons initialize(int index = 0, int mode = 0, IViewport viewport= null);
		void dispose();
		void update(int index = 0, int mode = 0);
		void refresh(int index, int mode = 0);
	}

	/// <summary>
	/// Fight menu (choose a move)
	/// </summary>
	public interface IFightMenuDisplay : IDisposable
	{
		float x		{ get; }
		float y		{ get; }
		float z		{ get; }
		float ox		{ get; }
		float oy		{ get; }
		bool visible	{ get; }
		int color		{ get; }
		IBattler battler	{ get; }
		IFightMenuDisplay initialize(IBattler battler, IViewport viewport= null);
		bool disposed();
		void dispose();
		void setIndex(int value);
		void refresh();
		void update();
	}

	public interface IFightMenuButtons //: IBitmapSprite, IDisposable
	{
		IFightMenuButtons initialize(int index = 0,Moves[] moves= null, IViewport viewport= null);
		void dispose();
		void update(int index = 0,Moves[] moves= null, int megaButton = 0);
		void refresh(int index,Moves[] moves,int megaButton);
	}

	/// <summary>
	/// Data box for safari battles
	/// </summary>
	public interface ISafariDataBox //: ISpriteWrapper
	{
		ISafariDataBox initialize(IBattle battle, IViewport viewport= null);
		void appear();
		void refresh();
		void update();
	}

	/// <summary>
	/// Data box for regular battles (both single and double)
	/// </summary>
	public interface IPokemonDataBox //: ISpriteWrapper, IDisposable
	{
		IPokemonDataBox initialize(IBattler battler, bool doublebattle, IViewport viewport= null);
		void dispose();
		void refreshExpLevel();
		void exp();
		void hp();
		void animateHP(int oldhp, int newhp);
		void animateEXP(int oldexp, int newexp);
		void appear();
		void refresh();
		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s)'s Pokémon being thrown out.  It appears at coords
	/// (@spritex,@spritey), and moves in y to @endspritey where it stays for the rest
	/// of the battle, i.e. the latter is the more important value.
	/// Doesn't show the ball itself being thrown.
	/// </summary>
	public interface IPokeballSendOutAnimation : IDisposable
	{
		IPokeballSendOutAnimation initialize(ISprite sprite, spritehash, IPokemon pkmn, IPokemon illusionpoke,bool doublebattle);
		bool disposed();
		bool animdone();
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
		IPokeballPlayerSendOutAnimation initialize(ISprite sprite, spritehash, IPokemon pkmn, IPokemon illusionpoke,bool doublebattle);
		bool disposed();
		bool animdone();
		void dispose();
		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s) and the enemy party lineup sliding off screen.
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	public interface ITrainerFadeAnimation
	{
		ITrainerFadeAnimation initialize(ISprite sprites);
		bool animdone();
		void update();
	}

	/// <summary>
	/// Shows the player (and partner) and the player party lineup sliding off screen.
	/// Shows the player's/partner's throwing animation (if they have one).
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	public interface IPlayerFadeAnimation
	{
		IPlayerFadeAnimation initialize(ISprite sprites);
		bool animdone();
		void update();
	}*/

	public interface IEventScene {
		/// <summary>
		/// Action Button Pressed...
		/// </summary>
		bool onATrigger { get; set; }
		/// <summary>
		/// Back/Cancel Button Pressed...
		/// </summary>
		bool onBTrigger { get; set; }
		bool onUpdate { get; set; }
		IEnumerator main();
		IEventScene initialize();
		bool disposed { get; }
		void dispose();
		//void addBitmap(float x, float y, Bitmap bitmap);
		void addLabel(float x, float y, float width, string text);
		void addImage(float x, float y, string name);
		void getPicture(int num);
		void wait(int frames);
		void pictureWait(int extraframes = 0);
		//void addUserSprite(Sprite sprite);
		IEnumerator update();
	}
	/// <summary>
	/// First scene to load when game boots up.
	/// Displays logos and intro credits/sponsers
	/// </summary>
	public interface IIntroEventScene : IEventScene {
		void openPic();
		void timer();
		void closePic();
		void openSplash();
		/// <summary>
		/// </summary>
		/// If you press and hold special keys, go to delete screen
		void splashUpdate();
		/// <summary>
		/// Ends intro scene and transition to load/new game screen
		/// </summary>
		void closeSplash();
		/// <summary>
		/// Ends intro scene and transition to delete save data screen
		/// </summary>
		void closeSplashDelete();
	}
	public interface IIntroEventScreen : PokemonEssentials.Interface.Screen.IScreen
	{
		IIntroEventScreen initialize();
		void main();
	}
	/// <summary>
	/// Shows a help screen listing the keyboard controls.
	/// </summary>
	public interface IButtonEventScene : IEventScene {
		void pbOnScreen1();
	}
	public interface IPokeBattle_DebugScene : IScene {
		void pbDisplayMessage(string msg,bool brief= false);
		void pbDisplayPausedMessage(string msg);
		void pbDisplayConfirmMessage(string msg);
		void pbFrameUpdate(object cw);
		//void pbRefresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		void pbBeginCommandPhase();
		void pbStartBattle(IBattle battle);
		void pbEndBattle(PokemonUnity.Combat.BattleResults result);
		void pbTrainerSendOut(IBattle battle,IPokemon pkmn);
		void pbSendOut(IBattle battle,IPokemon pkmn);
		void pbTrainerWithdraw(IBattle battle,IPokemon pkmn);
		void pbWithdraw(IBattle battle,IPokemon pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		void pbForgetMove(IPokemon pkmn,Moves move);
		void pbBeginAttackPhase();
		void pbCommandMenu(int index);
		void pbPokemonString(IPokemon pkmn);
		void pbMoveString(string move);
		/// <summary>
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		void pbFightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		void pbItemMenu(int index);
		void pbFirstTarget(int index,int targettype);
		void pbNextTarget(int cur,int index);
		void pbPrevTarget(int cur,int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		void pbChooseTarget(int index,int targettype);
		void pbSwitch(int index,bool lax,bool cancancel);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		void pbHPChanged(IPokemon pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		void pbFainted(IPokemon pkmn);
		void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		void pbChooseNewEnemy(int index,IPokemon[] party);
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
		void pbEXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		void pbLevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		void pbShowOpponent(ITrainer opp);
		void pbHideOpponent();
		void pbRecall(int battlerindex);
		void pbDamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		void pbAnimation(int moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
	}
	public interface IPokeBattle_SceneNonInteractive : IPokeBattle_Scene
	{
		new void pbCommandMenu(int index);
		new void pbFightMenu(int index);
		new void pbItemMenu(int index);
		void pbChooseTarget(int index,int targettype);
		new void pbSwitch(int index,bool lax,bool cancancel);
		new void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		new void pbChooseNewEnemy(int index,IPokemon[] party);
	}
	/*public interface IPokeBattle_DebugSceneNoLogging : IPokeBattle_Scene
	{
		void pbDisplayMessage(string msg,bool brief= false);
		void pbDisplayPausedMessage(string msg);
		bool pbDisplayConfirmMessage(string msg);
		int pbShowCommands(string msg,string[] commands,bool defaultValue);
		void pbBeginCommandPhase();
		void pbStartBattle(IBattle battle);
		void pbEndBattle(Combat.BattleResults result);
		void pbTrainerSendOut(IBattle battle,IPokemon pkmn);
		void pbSendOut(IBattle battle,IPokemon pkmn);
		void pbTrainerWithdraw(IBattle battle,IPokemon pkmn);
		void pbWithdraw(IBattle battle,IPokemon pkmn);
		int pbForgetMove(IPokemon pkmn,Moves move);
		void pbBeginAttackPhase();
		int pbCommandMenu(int index);
		int pbFightMenu(int index);
		int pbItemMenu(int index);
		int pbChooseTarget(int index,int targettype);
		void pbRefresh();
		int pbSwitch(int index,bool lax,bool cancancel);
		void pbHPChanged(IPokemon pkmn,int oldhp,bool anim= false);
		void pbFainted(IPokemon pkmn);
		void pbChooseEnemyCommand(int index);
		void pbChooseNewEnemy(int index,IPokemon[] party);
		void pbWildBattleSuccess();
		void pbTrainerBattleSuccess();
		void pbEXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		void pbLevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		int pbBlitz(keys);
		void pbChatter(IBattle attacker,IBattle opponent);
		void pbShowOpponent(opp);
		void pbHideOpponent();
		void pbRecall(int battlerindex);
		void pbDamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		void pbBattleArenaJudgment(IBattle b1,IBattle b2, r1, r2);
		void pbBattleArenaBattlers(IBattle b1,IBattle b2);
		void pbCommonAnimation(int moveid,IBattle attacker,IBattle opponent,int hitnum= 0);
		void pbAnimation(int moveid,IBattle attacker,IBattle opponent,int hitnum= 0);
	}
	public interface IPokeBattle_DebugSceneNoGraphics { }*/
}