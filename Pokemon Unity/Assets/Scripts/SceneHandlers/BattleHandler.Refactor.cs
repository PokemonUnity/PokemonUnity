//Original Scripts by IIColour (IIColour_Spectrum)
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;
//using DiscordPresence;

public partial class BattleHandler : IPokeBattle_Scene
{
	private PokemonEssentials.Interface.PokeBattle.IBattle battle;
	private bool aborted;
	private bool abortable;
	private bool battlestart;
	private bool messagemode;
	private MenuCommands[] lastcmd;
	private int[] lastmove;
	//private int messageCount = 0;
	private IDictionary<string, GameObject> sprites;
	private IList<GameObject> pkmnwindows;
	public GameObject MessageWindow;

	bool IPokeBattle_Scene.inPartyAnimation { get { throw new System.NotImplementedException(); } }

	int IScene.Id { get { throw new System.NotImplementedException(); } }

	void IPokeBattle_Scene.ChangePokemon()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.initialize()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		battle = null;
		lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
		lastmove = new int[] { 0, 0, 0, 0 };
		pkmnwindows = new GameObject[] { null, null, null, null };
		sprites = new Dictionary<string, GameObject>();
		battlestart = true;
		messagemode = false;
		abortable = true;
		aborted = false;
	}

	void IPokeBattle_Scene.partyAnimationUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbAddPlane(int id, string filename, int viewport)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbAddSprite(string id, double x, double y, string filename, int viewport)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbAnimationCore(string animation, IBattler user, IBattler target, bool oppmove)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbBackdrop()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaBattlers(IBattle b1, IBattle b2)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbBattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbBeginAttackPhase()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbBeginCommandPhase()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbBlitz(int keys)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbChangePokemon(IBattler attacker, Forms pokemon)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbChangeSpecies(IBattler attacker, Pokemons species)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void ISceneHasChatter.pbChatter(IBattler attacker, IBattler opponent)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_SceneNonInteractive.pbChooseEnemyCommand(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_Scene.pbChooseMove(IPokemon pokemon, string message)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbChooseNewEnemy(int index, IPokemon[] party)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbChooseTarget(int index, Targets targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbChooseTarget(int index, Targets targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbCommandMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbCommandMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_Scene.pbCommandMenuEx(int index, string[] texts, int mode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbCommonAnimation(string animname, IBattler user, IBattler target, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbDamageAnimation(IBattler pkmn, TypeEffective effectiveness)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbDisplay(string msg, bool brief)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		((IPokeBattle_DebugSceneNoGraphics)this).pbDisplayMessage(msg, brief);
	}

	void IHasDisplayMessage.pbDisplay(string v)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		((IPokeBattle_Scene)this).pbDisplay(v, false);
	}

	bool IPokeBattle_DebugSceneNoGraphics.pbDisplayConfirmMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbDisplayMessage(string msg, bool brief)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();

		//pbShowWindow(MESSAGEBOX);
		/*Dialog.DrawBlackFrame();
		//float startTime = Time.time;
		int i = 0;
		do //begin coroutine
		{
			if (i == 40)
			{
				//end dialog window, after 40 ticks.
				//MessageWindow.Text = "";
				//MessageWindow.IsActive(false);
				yield return null;
			}
			if (Input.GetButtonDown("Start") || abortable)
			{
				MessageWindow.Text = msg;
			}
			else
			{
				while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
				{
					yield return null;
				}
			}
		} while (true);
		if (silent)
		{
			yield return StartCoroutine(Dialog.DrawTextSilent(msg));
		}
		else
		{
			yield return StartCoroutine(Dialog.DrawText(msg));
		}

		if (lockedTime > 0)
		{
			while (Time.time < startTime + lockedTime)
			{
				yield return null;
			}
		}

		if (time > 0)
		{
			while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back") && Time.time < startTime + time)
			{
				yield return null;
			}
		}
		else
		{
			while (!Input.GetButtonDown("Select") && !Input.GetButtonDown("Back"))
			{
				yield return null;
			}
		}*/
	}

	void IPokeBattle_DebugSceneNoGraphics.pbDisplayPausedMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbDisposeSprites()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbEndBattle(BattleResults result)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbFainted(IBattler pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbFightMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbFightMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbFindAnimation(Moves moveid, int userIndex, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_Scene.pbFirstTarget(int index, Targets targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbFirstTarget(int index, int targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbForgetMove(IPokemon pkmn, Moves move)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbFrameUpdate(object cw)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbGraphicsUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbHideCaptureBall()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbHideHelp()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbHideOpponent()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbHPChanged(IBattler pkmn, int oldhp, bool anim)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbInputUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbItemMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbItemMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbLevelUp(IPokemon pokemon, IBattler battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbMoveString(string move)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	string IPokeBattle_Scene.pbNameEntry(string helptext, IPokemon pokemon)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbNextTarget(int cur, int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbPokemonString(IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugScene.pbPrevTarget(int cur, int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbRecall(int battlerindex)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IScene.pbRefresh()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbResetCommandIndices()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbResetMoveIndex(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_Scene.pbSafariCommandMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbSafariStart()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbSaveShadows()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbSelectBattler(int index, int selectmode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbSendOut(int battlerindex, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbSendOut(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbSetMessageMode(bool mode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, bool defaultValue)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbShowHelp(string text)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbShowOpponent(int opp)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbShowPokedex(Pokemons species, int form)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbShowWindow(int windowtype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbStartBattle(IBattle battle)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_SceneNonInteractive.pbSwitch(int index, bool lax, bool cancancel)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbThrowAndDeflect(Items ball, int targetBattler)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbThrowBait()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbThrowRock()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbThrowSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbTrainerBattleSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbTrainerSendOut(int battlerindex, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbTrainerSendOut(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbTrainerWithdraw(IBattle battle, IBattler pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbTrainerWithdraw(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbUpdateSelected(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_Scene.pbWaitMessage()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbWildBattleSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	void IPokeBattle_DebugSceneNoGraphics.pbWithdraw(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}
}