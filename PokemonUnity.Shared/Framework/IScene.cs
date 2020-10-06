using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
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
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		//void pokeballThrow(Items ball, int shakes,bool critical,Combat.Pokemon targetBattler,IScene scene,Combat.Pokemon battler, int burst = -1, bool showplayer = false);
		//void pbDisplay(string v);
		//bool pbConfirm(string v);
	}
	/*
	/// <summary>
	/// Command menu (Fight/Pokémon/Bag/Run)
	/// </summary>
	interface ICommandMenuDisplay
	{
		void initialize(viewport= null);
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

	interface ICommandMenuButtons //: BitmapSprite
	{
		void initialize(int index = 0, int mode = 0, viewport= null);
		void dispose();
		void update(int index = 0, int mode = 0);
		void refresh(int index, int mode = 0);
	}

	/// <summary>
	/// Fight menu (choose a move)
	/// </summary>
	interface IFightMenuDisplay
	{
		float x		{ get; }
		float y		{ get; }
		float z		{ get; }
		float ox		{ get; }
		float oy		{ get; }
		bool visible	{ get; }
		int color		{ get; }
		Combat.Pokemon battler	{ get; }
		void initialize(Combat.Pokemon battler, viewport= null);
		bool disposed();
		void dispose();
		void setIndex(int value);
		void refresh();
		void update();
	}

	interface IFightMenuButtons //: BitmapSprite
	{
		void initialize(int index = 0,Moves[] moves= null, viewport= null);
		void dispose();
		void update(int index = 0,Moves[] moves= null, int megaButton = 0);
		void refresh(int index,Moves[] moves,int megaButton);
	}

	/// <summary>
	/// Data box for safari battles
	/// </summary>
	interface ISafariDataBox //: SpriteWrapper
	{
		void initialize(battle, viewport= null);
		void appear();
		void refresh();
		void update();
	}

	/// <summary>
	/// Data box for regular battles (both single and double)
	/// </summary>
	interface IPokemonDataBox //: SpriteWrapper
	{
		void initialize(Combat.Pokemon battler, bool doublebattle, viewport= null);
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
	interface IPokeballSendOutAnimation
	{
		void initialize(sprite, spritehash, pkmn, illusionpoke,bool doublebattle);
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
	interface IPokeballPlayerSendOutAnimation
	{
		void initialize(sprite, spritehash, pkmn, illusionpoke,bool doublebattle);
		bool disposed();
		bool animdone();
		void dispose();
		void update();
	}

	/// <summary>
	/// Shows the enemy trainer(s) and the enemy party lineup sliding off screen.
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	interface ITrainerFadeAnimation
	{
		void initialize(sprites);
		bool animdone();
		void update();
	}

	/// <summary>
	/// Shows the player (and partner) and the player party lineup sliding off screen.
	/// Shows the player's/partner's throwing animation (if they have one).
	/// Doesn't show the ball thrown or the Pokémon.
	/// </summary>
	interface IPlayerFadeAnimation
	{
		void initialize(sprites);
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
		void initialize();
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
	public interface IScene_Intro : IScreen
	{
		void initialize();
		void main();
	}
	/// <summary>
	/// Shows a help screen listing the keyboard controls.
	/// </summary>
	public interface IButtonEventScene : IEventScene {
		void pbOnScreen1();
	}
	public interface IPokeBattle_DebugScene {
		void pbDisplayMessage(string msg,bool brief= false);
		void pbDisplayPausedMessage(string msg);
		void pbDisplayConfirmMessage(string msg);
		void pbFrameUpdate(object cw);
		void pbRefresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		void pbBeginCommandPhase();
		void pbStartBattle(Combat.Battle battle);
		void pbEndBattle(Combat.BattleResults result);
		void pbTrainerSendOut(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbSendOut(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbTrainerWithdraw(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbWithdraw(Combat.Battle battle,Monster.Pokemon pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		void pbForgetMove(Monster.Pokemon pkmn,Moves move);
		void pbBeginAttackPhase();
		void pbCommandMenu(int index);
		void pbPokemonString(Monster.Pokemon pkmn);
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
		void pbHPChanged(Monster.Pokemon pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		void pbFainted(Monster.Pokemon pkmn);
		void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		void pbChooseNewEnemy(int index,Monster.Pokemon[] party);
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
		void pbEXPBar(Combat.Pokemon battler,Monster.Pokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		void pbLevelUp(Combat.Pokemon battler,Monster.Pokemon thispoke,int oldtotalhp,int oldattack,
				int olddefense,int oldspeed,int oldspatk,int oldspdef);
		void pbShowOpponent(Combat.Trainer opp);
		void pbHideOpponent();
		void pbRecall(int battlerindex);
		void pbDamageAnimation(Monster.Pokemon pkmn,TypeEffective effectiveness);
		void pbAnimation(int moveid,Combat.Pokemon attacker,Combat.Pokemon opponent,int hitnum= 0);
	}
	public interface IPokeBattle_SceneNonInteractive //: IPokeBattle_Scene
	{
		void pbCommandMenu(int index);
		void pbFightMenu(int index);
		void pbItemMenu(int index);
		void pbChooseTarget(int index,int targettype);
		void pbSwitch(int index,bool lax,bool cancancel);
		void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		void pbChooseNewEnemy(int index,Monster.Pokemon[] party);
	}
	/*public interface IPokeBattle_DebugSceneNoLogging : IPokeBattle_Scene
	{
		void pbDisplayMessage(string msg,bool brief= false);
		void pbDisplayPausedMessage(string msg);
		bool pbDisplayConfirmMessage(string msg);
		int pbShowCommands(string msg,string[] commands,bool defaultValue);
		void pbBeginCommandPhase();
		void pbStartBattle(Combat.Battle battle);
		void pbEndBattle(Combat.BattleResults result);
		void pbTrainerSendOut(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbSendOut(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbTrainerWithdraw(Combat.Battle battle,Monster.Pokemon pkmn);
		void pbWithdraw(Combat.Battle battle,Monster.Pokemon pkmn);
		int pbForgetMove(Monster.Pokemon pkmn,Moves move);
		void pbBeginAttackPhase();
		int pbCommandMenu(int index);
		int pbFightMenu(int index);
		int pbItemMenu(int index);
		int pbChooseTarget(int index,int targettype);
		void pbRefresh();
		int pbSwitch(int index,bool lax,bool cancancel);
		void pbHPChanged(Monster.Pokemon pkmn,int oldhp,bool anim= false);
		void pbFainted(Monster.Pokemon pkmn);
		void pbChooseEnemyCommand(int index);
		void pbChooseNewEnemy(int index,Monster.Pokemon[] party);
		void pbWildBattleSuccess();
		void pbTrainerBattleSuccess();
		void pbEXPBar(Combat.Pokemon battler,Monster.Pokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		void pbLevelUp(Combat.Pokemon battler,Monster.Pokemon thispoke,int oldtotalhp,int oldattack,
				int olddefense,int oldspeed,int oldspatk,int oldspdef);
		int pbBlitz(keys);
		void pbChatter(Combat.Battle attacker,Combat.Battle opponent);
		void pbShowOpponent(opp);
		void pbHideOpponent();
		void pbRecall(int battlerindex);
		void pbDamageAnimation(Monster.Pokemon pkmn,TypeEffective effectiveness);
		void pbBattleArenaJudgment(Combat.Battle b1,Combat.Battle b2, r1, r2);
		void pbBattleArenaBattlers(Combat.Battle b1,Combat.Battle b2);
		void pbCommonAnimation(int moveid,Combat.Battle attacker,Combat.Battle opponent,int hitnum= 0);
		void pbAnimation(int moveid,Combat.Battle attacker,Combat.Battle opponent,int hitnum= 0);
	}
	public interface IPokeBattle_DebugSceneNoGraphics { }
	public interface I { }
	public interface I { }
	public interface I { }
	public interface I { }*/
}