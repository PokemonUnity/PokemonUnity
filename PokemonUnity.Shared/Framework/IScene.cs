using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity.Battle;
using PokemonUnity.Inventory;

namespace PokemonUnity
{
	public interface IScene
	{
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

		- def pbCommandMenu(int index)
		Use this method to display the list of commands and choose
		a command for the player.
		index - Index of battler (use e.g. @battle.battlers[index] to 
		access the battler)
		Return values:
		0 - Fight
		1 - Pokémon
		2 - Bag
		3 - Run
		*/

		/// <summary>
		/// Shows the player's Poké Ball being thrown to capture a Pokémon.
		/// </summary>
		void pokeballThrow(Items ball, int shakes,bool critical,Battle.Pokemon targetBattler,IScene scene,Battle.Pokemon battler, int burst = -1, bool showplayer = false);
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
		void initialize(battler, viewport= null);
		double x; //@window.x;
		double y; //@window.y;
		double z; //@window.z;
		double ox; //@window.ox;
		double oy; //@window.oy;
		bool visible; //@window.visible;
		int color; //@window.color;
		bool disposed();
		void dispose();
		void battler;
		void setIndex(value);
		void refresh();
		void update();
	}

	interface IFightMenuButtons //: BitmapSprite
	{
		void initialize(int index = 0, moves= null, viewport= null)
	  void dispose()
	  void update(int index = 0, moves= null, int megaButton = 0)
	  void refresh(index, moves, megaButton)
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
		void initialize(battler, bool doublebattle, viewport= null);
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
		void initialize(sprite, spritehash, pkmn, illusionpoke, doublebattle);
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
		void initialize(sprite, spritehash, pkmn, illusionpoke, doublebattle);
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

	/// <summary>
	/// Battle scene (the visuals of the battle)
	/// </summary>
	public interface IPokeBattle_Scene : IScene
	{
		void initialize();
		void pbUpdate();
		void pbGraphicsUpdate();
		void pbInputUpdate();
		void pbShowWindow(int windowtype);
		void pbSetMessageMode(int mode);
		void pbWaitMessage();
		void pbDisplay(string msg, bool brief = false);
		void pbDisplayMessage(string msg, bool brief = false);
		void pbDisplayPausedMessage(string msg);
		void pbDisplayConfirmMessage(string msg);
		void pbShowCommands(string msg,string commands,bool defaultValue);
		void pbFrameUpdate(object cw= null);
		void pbRefresh();
		void pbAddSprite(int id,double x,double y, string filename,int viewport);
		void pbAddPlane(int id, string filename,int viewport);
		void pbDisposeSprites();
		void pbBeginCommandPhase();
		void pbShowOpponent(int index);
		void pbHideOpponent();
		void pbShowHelp(int text);
		void pbHideHelp();
		void pbBackdrop();
		/// <summary>
		/// Returns whether the party line-ups are currently appearing on-screen
		/// </summary>
		/// <returns></returns>
		bool inPartyAnimation();
		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		void partyAnimationUpdate();
		void pbStartBattle(PokemonUnity.Battle.Battle battle);
		void pbEndBattle(BattleResults result);
		void pbRecall(int battlerindex);
		void pbTrainerSendOut(int battlerindex,Battle.Pokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		void pbSendOut(int battlerindex,Battle.Pokemon pkmn);
		void pbTrainerWithdraw(Battle.Battle battle,Battle.Pokemon pkmn);
		void pbWithdraw(Battle.Battle battle,Battle.Pokemon pkmn);
		void pbMoveString(string move);
		void pbBeginAttackPhase();
		void pbSafariStart();
		void pbResetCommandIndices();
		void pbResetMoveIndex(int index);
		int pbSafariCommandMenu(int index);
		/// <summary>
		/// Use this method to display the list of commands.
		/// </summary>
		/// 0 - Fight, 1 - Pokémon, 2 - Bag, 3 - Run
		/// <param name="index"></param>
		/// <returns> Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		int pbCommandMenu(int index);
		/// <summary>
		/// </summary>
		/// <param name="index"></param>
		/// <param name="texts"></param>
		/// <param name="mode">0 - regular battle, 1 - Shadow Pokémon battle, 2 - Safari Zone, 3 - Bug Catching Contest</param>
		int pbCommandMenuEx(int index, string texts, int mode = 0);
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
		int[] pbItemMenu(int index);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.  The function
		/// should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="moveToLearn"></param>
		int pbForgetMove(Pokemon pokemon, Moves moveToLearn);
		/// <summary>
		/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
		/// </summary>
		/// <param name=""></param>
		/// <param name="message"></param>
		void pbChooseMove(Pokemon pokemon, string message);
		void pbNameEntry(string helptext,Pokemon pokemon);
		void pbSelectBattler(int index, int selectmode = 1);
		void pbFirstTarget(int index, int targettype);
		void pbUpdateSelected(int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype"></param>
		void pbChooseTarget(int index, int targettype);
		int pbSwitch(int index, bool lax, bool cancancel);
		void pbDamageAnimation(Battle.Pokemon pkmn,float effectiveness);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		void pbHPChanged(int pkmn, int oldhp, bool anim = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name=""></param>
		void Fainted(int pkmn);
		void pbFainted(int pkmn);
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
		int pbChooseNewEnemy(int index,Pokemon[] party);
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
		void pbEXPBar(Pokemon pokemon,Pokemon battler,int startexp,int endexp,int tempexp1,int tempexp2);
		void pbShowPokedex(Pokemons species, int form = 0);
		void pbChangeSpecies(Pokemon attacker,Pokemons species);
		void ChangePokemon();
		void pbChangePokemon(Pokemon attacker, Monster.Forms pokemon);
		void pbSaveShadows();
		void pbFindAnimation(Moves moveid, int userIndex,int hitnum);
		void pbCommonAnimation(string animname,Battle.Pokemon user,Battle.Pokemon target, int hitnum = 0);
		void pbAnimation(Moves moveid,Battle.Pokemon user,Battle.Pokemon target, int hitnum = 0);
		void pbAnimationCore(string animation,Battle.Pokemon user,Battle.Pokemon target, bool oppmove = false);
		void pbLevelUp(Pokemon pokemon,Pokemon battler,int oldtotalhp,int oldattack,int olddefense,int oldspeed,int oldspatk,int oldspdef);
		void pbThrowAndDeflect(Items ball,int targetBattler);
		void pbThrow(Items ball,int shakes,bool critical,int targetBattler, bool showplayer = false);
		void pbThrowSuccess();
		void pbHideCaptureBall();
		void pbThrowBait();
		void pbThrowRock();
		void HPChanged(int index, int oldhp, bool animate = false);
		void pbHPChanged(Pokemon pkmn, int oldhp, bool animate = false);
	}

	public class ItemHandlers 
	{ 
		public static bool triggerBattleUseOnPokemon(Items item,Battle.Pokemon pokemon,Battle.Pokemon battler,IPokeBattle_Scene scene) { return false; } 
		public static bool triggerBattleUseOnBattler(Items item,Battle.Pokemon pokemon, IPokeBattle_Scene scene) { return false; } 
		public static bool triggerBattleUseOnBattler(Items item,Battle.Pokemon pokemon,Battle.Battle battle) { return false; } 
		public static bool triggerUseInBattle(Items item,Battle.Pokemon pokemon,Battle.Battle battle) { return false; } 
		public static bool hasUseInBattle(Items item) { return false; }
	}
}