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
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.EventArg;

namespace PokemonEssentials.Interface.Screen
{
	#region Pokemon Battle
	public interface ISceneHasChatter
	{
		void pbChatter(IBattler attacker,IBattler opponent);
	}
	public interface IPokeBattle_Scene : IScene, ISceneHasChatter, IPokeBattle_DebugScene
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
		void pbStartBattle(IBattle battle);
		void pbEndBattle(BattleResults result);
		void pbRecall(int battlerindex);
		void pbTrainerSendOut(int battlerindex, IPokemon pkmn);
		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		void pbSendOut(int battlerindex, IPokemon pkmn);
		void pbTrainerWithdraw(IBattle battle, IBattler pkmn);
		void pbWithdraw(IBattle battle, IBattler pkmn);
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
		int pbFirstTarget(int index, PokemonUnity.Attack.Data.Targets targettype);
		void pbUpdateSelected(int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="targettype">Which targets are selectable as option</param>
		int pbChooseTarget(int index, PokemonUnity.Attack.Data.Targets targettype);
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
		void pbChangePokemon(IBattler attacker, PokemonUnity.Monster.Forms pokemon);
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

	public interface IPokeBattleArena_Scene : IScene, IPokeBattle_Scene
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
	
	public interface IPokeBattle_DebugScene : IScene, IPokeBattle_DebugSceneNoLogging
	{
		new void pbDisplayMessage(string msg,bool brief= false);
		new void pbDisplayPausedMessage(string msg);
		new void pbDisplayConfirmMessage(string msg);
		void pbFrameUpdate(object cw);
		//void pbRefresh();
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		new void pbBeginCommandPhase();
		new void pbStartBattle(IBattle battle);
		new void pbEndBattle(PokemonUnity.Combat.BattleResults result);
		new void pbTrainerSendOut(IBattle battle,IPokemon pkmn);
		new void pbSendOut(IBattle battle,IPokemon pkmn);
		new void pbTrainerWithdraw(IBattle battle,IPokemon pkmn);
		new void pbWithdraw(IBattle battle,IPokemon pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move. It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		new void pbForgetMove(IPokemon pkmn,Moves move);
		new void pbBeginAttackPhase();
		new void pbCommandMenu(int index);
		void pbPokemonString(IPokemon pkmn);
		void pbMoveString(string move);
		/// <summary>
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		new void pbFightMenu(int index);
		/// <summary>
		/// Use this method to display the inventory
		/// The return value is the item chosen, or 0 if the choice was canceled.
		/// </summary>
		/// <param name="index"></param>
		new void pbItemMenu(int index);
		void pbFirstTarget(int index,int targettype);
		void pbNextTarget(int cur,int index);
		void pbPrevTarget(int cur,int index);
		/// <summary>
		/// Use this method to make the player choose a target 
		/// for certain moves in double battles.
		/// </summary>
		/// <param name="index"></param>
		/// <param name=""></param>
		new void pbChooseTarget(int index,int targettype);
		new void pbSwitch(int index,bool lax,bool cancancel);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		new void pbHPChanged(IPokemon pkmn,int oldhp,bool anim= false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		new void pbFainted(IPokemon pkmn);
		new void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		new void pbChooseNewEnemy(int index,IPokemon[] party);
		/// <summary>
		/// This method is called when the player wins a wild Pokémon battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new void pbWildBattleSuccess();
		/// <summary>
		/// This method is called when the player wins a Trainer battle.
		/// This method can change the battle's music for example.
		/// </summary>
		new void pbTrainerBattleSuccess();
		new void pbEXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		new void pbLevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		new void pbShowOpponent(ITrainer opp);
		new void pbHideOpponent();
		new void pbRecall(int battlerindex);
		new void pbDamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		new void pbAnimation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
	}
	public interface IPokeBattle_SceneNonInteractive : IScene
	{
		void pbCommandMenu(int index);
		void pbFightMenu(int index);
		void pbItemMenu(int index);
		void pbChooseTarget(int index,int targettype);
		int pbSwitch(int index,bool lax,bool cancancel);
		void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		void pbChooseNewEnemy(int index,IPokemon[] party);
	}
	public interface IPokeBattle_DebugSceneNoLogging : IScene, ISceneHasChatter, IPokeBattle_DebugSceneNoGraphics
	{
		new void pbDisplayMessage(string msg,bool brief= false);
		new void pbDisplayPausedMessage(string msg);
		new bool pbDisplayConfirmMessage(string msg);
		new int pbShowCommands(string msg,string[] commands,bool defaultValue);
		new void pbBeginCommandPhase();
		new void pbStartBattle(IBattle battle);
		new void pbEndBattle(BattleResults result);
		new void pbTrainerSendOut(IBattle battle,IPokemon pkmn);
		new void pbSendOut(IBattle battle,IPokemon pkmn);
		new void pbTrainerWithdraw(IBattle battle,IPokemon pkmn);
		new void pbWithdraw(IBattle battle,IPokemon pkmn);
		new int pbForgetMove(IPokemon pkmn,Moves move);
		new void pbBeginAttackPhase();
		new int pbCommandMenu(int index);
		new int pbFightMenu(int index);
		new int pbItemMenu(int index);
		new int pbChooseTarget(int index,int targettype);
		//void pbRefresh();
		new int pbSwitch(int index,bool lax,bool cancancel);
		new void pbHPChanged(IPokemon pkmn,int oldhp,bool anim= false);
		new void pbFainted(IPokemon pkmn);
		new void pbChooseEnemyCommand(int index);
		new void pbChooseNewEnemy(int index,IPokemon[] party);
		new void pbWildBattleSuccess();
		new void pbTrainerBattleSuccess();
		new void pbEXPBar(IBattler battler,IPokemon thispoke,int startexp,int endexp,int tempexp1,int tempexp2);
		new void pbLevelUp(IBattler battler,IPokemon thispoke,int oldtotalhp,int oldattack,
			int olddefense,int oldspeed,int oldspatk,int oldspdef);
		new int pbBlitz(int keys);
		//new void pbChatter(IBattler attacker,IBattler opponent);
		new void pbShowOpponent(ITrainer opp);
		new void pbHideOpponent();
		new void pbRecall(int battlerindex);
		new void pbDamageAnimation(IPokemon pkmn,TypeEffective effectiveness);
		new void pbBattleArenaJudgment(IBattle b1,IBattle b2, int[] r1, int[] r2);
		new void pbBattleArenaBattlers(IBattle b1,IBattle b2);
		new void pbCommonAnimation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
		new void pbAnimation(Moves moveid,IBattler attacker,IBattler opponent,int hitnum= 0);
	}
	public interface IPokeBattle_DebugSceneNoGraphics : IScene, ISceneHasChatter, IPokeBattle_SceneNonInteractive
	{
		void initialize();
		void pbDisplayMessage(string msg, bool brief = false);
		void pbDisplayPausedMessage(string msg);
		bool pbDisplayConfirmMessage(string msg);
		int pbShowCommands(string msg, string[] commands, bool defaultValue);
		/// <summary>
		/// Called whenever a new round begins.
		/// </summary>
		void pbBeginCommandPhase();
		/// <summary>
		/// Called whenever the battle begins
		/// </summary>
		/// <param name="battle"></param>
		void pbStartBattle(IBattle battle);
		void pbEndBattle(BattleResults result);
		void pbTrainerSendOut(IBattle battle, IPokemon pkmn);
		void pbSendOut(IBattle battle, IPokemon pkmn);
		void pbTrainerWithdraw(IBattle battle, IPokemon pkmn);
		void pbWithdraw(IBattle battle, IPokemon pkmn);
		/// <summary>
		/// Called whenever a Pokémon should forget a move.  It should return -1 if the
		/// selection is canceled, or 0 to 3 to indicate the move to forget.
		/// The function should not allow HM moves to be forgotten.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="move"></param>
		int pbForgetMove(IPokemon pkmn, Moves move);
		void pbBeginAttackPhase();
		new int pbCommandMenu(int index);
		new int pbFightMenu(int index);
		new int pbItemMenu(int index);
		new int pbChooseTarget(int index, int targettype);
		//void pbRefresh();
		new int pbSwitch(int index, bool lax, bool cancancel);
		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		void pbHPChanged(IPokemon pkmn, int oldhp, bool anim = false);
		/// <summary>
		/// This method is called whenever a Pokémon faints
		/// </summary>
		/// <param name="pkmn"></param>
		void pbFainted(IPokemon pkmn);
		new void pbChooseEnemyCommand(int index);
		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		new void pbChooseNewEnemy(int index, IPokemon[] party);
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
		void pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2);
		void pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack,
			int olddefense, int oldspeed, int oldspatk, int oldspdef);
		int pbBlitz(int keys);
		//void pbChatter(IBattler attacker, IBattler opponent);
		void pbShowOpponent(ITrainer opp);
		void pbHideOpponent();
		void pbRecall(int battlerindex);
		void pbDamageAnimation(IPokemon pkmn, TypeEffective effectiveness);
		void pbBattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2);
		void pbBattleArenaBattlers(IBattle b1, IBattle b2);
		void pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0);
		void pbAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum = 0);
	}
}