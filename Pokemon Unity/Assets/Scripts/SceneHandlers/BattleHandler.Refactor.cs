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
	public GameAudioPlay AudioHandler;
	public IWindow MessageWindow;
	public IList<IWindow> pkmnwindows;
	public IDictionary<string, IWindow> sprites;
	public bool aborted;
	public bool abortable;
	public bool battlestart;
	public bool messagemode;
	public bool briefmessage;
	public bool enablePartyAnim;
	public int partyAnimPhase;
	public int xposplayer;
	public int xposenemy;
	public IViewport viewport;
	private MenuCommands[] lastcmd;
	private int[] lastmove;
	private PokemonEssentials.Interface.PokeBattle.IBattle battle;
	public const int BLANK = 0;
	public const int MESSAGEBOX = 1;
	public const int COMMANDBOX = 2;
	public const int FIGHTBOX = 3;
	/// <summary>
	/// 
	/// </summary>
	public const string UI_MESSAGEBOX = "messagebox";
	/// <summary>
	/// 
	/// </summary>
	public const string UI_MESSAGEWINDOW = "messagewindow";
	/// <summary>
	/// Select turn action; Fight, Bag, Item, Run...
	/// </summary>
	public const string UI_COMMANDWINDOW = "commandwindow";
	/// <summary>
	/// Select Pokemon's attack move for given turn
	/// </summary>
	public const string UI_FIGHTWINDOW = "fightwindow";


	bool IPokeBattle_Scene.inPartyAnimation { get { return @enablePartyAnim && @partyAnimPhase < 3; } }

	int IScene.Id { get { return 0; } }

	void IPokeBattle_DebugSceneNoGraphics.initialize()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		battle = null;
		lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
		lastmove = new int[] { 0, 0, 0, 0 };
		pkmnwindows = new IWindow[] { null, null, null, null };
		sprites = new Dictionary<string, IWindow>();
		battlestart = true;
		messagemode = false;
		abortable = true;
		aborted = false;
	}

	void IPokeBattle_Scene.pbUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		((IPokeBattle_Scene)this).partyAnimationUpdate();
		//if (sprites["battlebg"] is GameObject g) g.pbUpdate();
	}

	void IPokeBattle_Scene.pbGraphicsUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		((IPokeBattle_Scene)this).partyAnimationUpdate();
		//if (sprites["battlebg"] is GameObject g) g.pbUpdate();
		//Graphics.Update();
	}

	void IPokeBattle_Scene.pbInputUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//Input.Update();
		if (UnityEngine.Input.GetButtonDown("Select") && abortable && !aborted)
		{
			aborted = true;
			battle.pbAbort();
		}
	}

	void IPokeBattle_Scene.pbShowWindow(int windowtype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@sprites["messagebox"].visible = (windowtype == MESSAGEBOX ||
										  windowtype == COMMANDBOX ||
										  windowtype == FIGHTBOX ||
										  windowtype == BLANK);
		@sprites["messagewindow"].visible = (windowtype == MESSAGEBOX);
		@sprites["commandwindow"].visible = (windowtype == COMMANDBOX);
		@sprites["fightwindow"].visible = (windowtype == FIGHTBOX);
	}

	void IPokeBattle_Scene.pbSetMessageMode(bool mode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@messagemode = mode;
		IWindow msgwindow = @sprites[UI_MESSAGEWINDOW];
		if (mode)
		{       // Within Pokémon command
			msgwindow.baseColor = PokeBattle_SceneConstants.MENUBASECOLOR;
			msgwindow.shadowColor = PokeBattle_SceneConstants.MENUSHADOWCOLOR;
			msgwindow.opacity = 255;
			msgwindow.x = 16;
			msgwindow.width = Graphics.width;
			msgwindow.height = 96;
			msgwindow.y = Graphics.height - msgwindow.height + 2;
		
		}
		else 
		{
			msgwindow.baseColor = PokeBattle_SceneConstants.MESSAGEBASECOLOR;
			msgwindow.shadowColor = PokeBattle_SceneConstants.MESSAGESHADOWCOLOR;
			msgwindow.opacity = 0;
			msgwindow.x = 16;
			msgwindow.width = Graphics.width - 32;
			msgwindow.height = 96;
			msgwindow.y = Graphics.height - msgwindow.height + 2;
		}
	}

	void IPokeBattle_Scene.pbWaitMessage()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (@briefmessage)
		{
			((IPokeBattle_Scene)this).pbShowWindow(MESSAGEBOX);
			IWindow cw = @sprites["messagewindow"];
			int i = 0;
			do
			{
				((IPokeBattle_Scene)this).pbGraphicsUpdate();
				((IPokeBattle_Scene)this).pbInputUpdate();
				((IPokeBattle_Scene)this).pbFrameUpdate(cw);
				i++;
			} while (i < 60);
			cw.text = "";
			cw.visible = false;
			@briefmessage = false;
		}
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

	void IPokeBattle_DebugSceneNoGraphics.pbDisplayMessage(string msg, bool brief)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		((IPokeBattle_Scene)this).pbWaitMessage();
		((IPokeBattle_Scene)this).pbRefresh();
		((IPokeBattle_Scene)this).pbShowWindow(MESSAGEBOX);
		IWindow cw = @sprites["messagewindow"];
		cw.text = msg;
		int i = 0;
		do //begin coroutine
		{
			((IPokeBattle_Scene)this).pbGraphicsUpdate();
			((IPokeBattle_Scene)this).pbInputUpdate();
			((IPokeBattle_Scene)this).pbFrameUpdate(cw);
			if (i == 40)
			{
				//end dialog window, after 40 ticks.
				//MessageWindow.Text = "";
				//MessageWindow.IsActive(false);
				cw.text = "";
				cw.visible = false;
				return; //yield return null;
			}
			if (UnityEngine.Input.GetButtonDown("Start") || abortable)
			{
				if (cw.pause)
				{
					if (!abortable) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					cw.resume();
				}
				MessageWindow.Text = msg;
			}
			if (!cw.busy)
			{
				if (brief)
				{
					briefmessage = true;
					return; //yield return null;
				}
				i++;
			}
		} while (true);
	}

	void IPokeBattle_DebugSceneNoGraphics.pbDisplayPausedMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	bool IPokeBattle_DebugSceneNoGraphics.pbDisplayConfirmMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		return ((IPokeBattle_Scene)this).pbShowCommands(msg, new string[] { Game._INTL("Yes"), Game._INTL("No") }, 1) == 0;
	}

	void IPokeBattle_Scene.partyAnimationUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (!inPartyAnimation) return;
		int ballmovedist = 16; // How far a ball moves each frame
		//  Bar slides on
		if (@partyAnimPhase == 0)
		{
			@sprites["partybarfoe"].x += 16;
			@sprites["partybarplayer"].x -= 16;
			if (@sprites["partybarfoe"].x + @sprites["partybarfoe"].bitmap.width >= PokeBattle_SceneConstants.FOEPARTYBAR_X)
			{
				@sprites["partybarfoe"].x = PokeBattle_SceneConstants.FOEPARTYBAR_X - @sprites["partybarfoe"].bitmap.width;
				@sprites["partybarplayer"].x = PokeBattle_SceneConstants.PLAYERPARTYBAR_X;
				@partyAnimPhase = 1;
			}
			return;
		}
		//  Set up all balls ready to slide on
		if (@partyAnimPhase == 1)
		{
			@xposplayer = PokeBattle_SceneConstants.PLAYERPARTYBALL1_X;
			int counter = 0;
			//  Make sure the ball starts off-screen
			while (@xposplayer < Graphics.width)
			{
				counter += 1; @xposplayer += ballmovedist;
			}
			@xposenemy = PokeBattle_SceneConstants.FOEPARTYBALL1_X - counter * ballmovedist;
			for (int i = 0; i < 6; i++)
			{
				//  Choose the ball's graphic (player's side)
				string ballgraphic = "Graphics/Pictures/ballempty";
				if (i < @battle.party1.Length && @battle.party1[i].IsNotNullOrNone())
				{
					if (@battle.party1[i].HP <= 0 || @battle.party1[i].isEgg)
					{
						ballgraphic = "Graphics/Pictures/ballfainted";
					}
					else if (@battle.party1[i].Status > 0)
					{
						ballgraphic = "Graphics/Pictures/ballstatus";
					}
					else
					{
						ballgraphic = "Graphics/Pictures/ballnormal";
					}
				}
				pbAddSprite("player#{i}",
				   @xposplayer + i * ballmovedist * 6, PokeBattle_SceneConstants.PLAYERPARTYBALL1_Y,
				   ballgraphic, @viewport);
				@sprites["player#{i}"].z = 41;
				//  Choose the ball's graphic (opponent's side)
				ballgraphic = "Graphics/Pictures/ballempty";
				int enemyindex = i;
				if (@battle.doublebattle && i >= 3)
				{
					enemyindex = (i % 3) + @battle.pbSecondPartyBegin(1);
				}
				if (enemyindex < @battle.party2.Length && @battle.party2[enemyindex].IsNotNullOrNone())
				{
					if (@battle.party2[enemyindex].HP <= 0 || @battle.party2[enemyindex].isEgg ?)
					{
						ballgraphic = "Graphics/Pictures/ballfainted";
					}
					else if (@battle.party2[enemyindex].Status > 0)
					{
						ballgraphic = "Graphics/Pictures/ballstatus";
					}
					else
					{
						ballgraphic = "Graphics/Pictures/ballnormal";
					}
				}
				pbAddSprite("enemy#{i}",
				   @xposenemy - i * ballmovedist * 6, PokeBattle_SceneConstants.FOEPARTYBALL1_Y,
				   ballgraphic, @viewport);
				@sprites["enemy#{i}"].z = 41;
			}
			@partyAnimPhase = 2;
		}
		//  Balls slide on
		if (@partyAnimPhase == 2)
		{
			for (int i = 0; i < 6; i++)
			{
				if (@sprites["enemy#{i}"].x < PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
				{
					@sprites["enemy#{i}"].x += ballmovedist;
					@sprites["player#{i}"].x -= ballmovedist;
					if (@sprites["enemy#{i}"].x >= PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
					{
						@sprites["enemy#{i}"].x = PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP;
						@sprites["player#{i}"].x = PokeBattle_SceneConstants.PLAYERPARTYBALL1_X + i * PokeBattle_SceneConstants.PLAYERPARTYBALL_GAP;
						if (i == 5)
						{
							@partyAnimPhase = 3;
						}
					}
				}
			}
		}
	}

	void IPokeBattle_Scene.ChangePokemon()
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

	int IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, bool defaultValue)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	int IPokeBattle_DebugSceneNoGraphics.pbShowCommands(string msg, string[] commands, int defaultValue)
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

	void IPokeBattle_Scene.pbUpdateSelected(int index)
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


	private static void GameDebug_OnLog(object sender, OnDebugEventArgs e)
	{
		if (e != null || e != System.EventArgs.Empty)
			if (e.Error == true)
				//System.Console.WriteLine("[ERR]: " + e.Message);
				UnityEngine.Debug.LogError("[ERR] " + UnityEngine.Time.frameCount + e.Message);
			else if (e.Error == false)
				//System.Console.WriteLine("[WARN]: " + e.Message);
				UnityEngine.Debug.LogWarning("[WARN] " + UnityEngine.Time.frameCount + e.Message);
			else
				//System.Console.WriteLine("[LOG]: " + e.Message);
				UnityEngine.Debug.Log("[LOG] " + UnityEngine.Time.frameCount + e.Message);
	}
}