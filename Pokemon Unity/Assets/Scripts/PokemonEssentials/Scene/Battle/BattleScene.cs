using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Localization;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Battle;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Serialization;

public class BattleScene : UnityEngine.MonoBehaviour, IScene, IPokeBattle_Scene
{
	#region Variable Property
	const int BLANK = 0;
	const int MESSAGEBOX = 1;
	const int COMMANDBOX = 2;
	const int FIGHTBOX = 3;
	/// <summary>
	/// 
	/// </summary>
	const string UI_MESSAGEBOX = "messagebox";
	/// <summary>
	/// 
	/// </summary>
	const string UI_MESSAGEWINDOW = "messagewindow";
	/// <summary>
	/// Select turn action; Fight, Bag, Item, Run...
	/// </summary>
	const string UI_COMMANDWINDOW = "commandwindow";
	/// <summary>
	/// Select Pokemon's attack move for given turn
	/// </summary>
	const string UI_FIGHTWINDOW = "fightwindow";
	private MenuCommands[] lastcmd;
	//private IList<int> lastcmd;
	private int[] lastmove;
	private PokemonEssentials.Interface.PokeBattle.IBattle battle;
	public GameAudioPlay AudioHandler;
	public ITrainerFadeAnimation fadeanim;
	public IPokeballSendOutAnimation sendout;
	public IWindow_CommandPokemon commandPokemon;
	public ICommandMenuDisplay commandWindow;
	public IFightMenuDisplay fightWindow;
	public IWindow_UnformattedTextPokemon helpWindow;
	public IWindow_AdvancedTextPokemon messageWindow;
	public IWindow_AdvancedTextPokemon messageBox;
	public IPartyDisplayScreen @switchscreen;
	public ISpriteWrapper player;
	public ISpriteWrapper playerB;
	public ISpriteWrapper trainer;
	public ISpriteWrapper trainer2;
	public ISpriteWrapper battlebg;
	public ISpriteWrapper enemybase;
	public ISpriteWrapper playerbase;
	public ISpriteWrapper partybarfoe;
	public ISpriteWrapper partybarplayer;
	public IPokemonDataBox battlebox0;
	public IPokemonDataBox battlebox1;
	public IPokemonDataBox battlebox2;
	public IPokemonDataBox battlebox3;
	/// <summary>
	/// Your side primary battle pokemon; slot one
	/// </summary>
	public IPokemonBattlerSprite pokemon0;
	/// <summary>
	/// Your side ally battle pokemon; slot two (only appears in double battles)
	/// </summary>
	public IPokemonBattlerSprite pokemon2;
	/// <summary>
	/// Enemy side primary battle pokemon; slot one
	/// </summary>
	public IPokemonBattlerSprite pokemon1;
	/// <summary>
	/// Enemy side ally battle pokemon; slot two (only appears in double battles)
	/// </summary>
	public IPokemonBattlerSprite pokemon3;
	public IIconSprite shadow0;
	public IIconSprite shadow1;
	public IIconSprite shadow2;
	public IIconSprite shadow3;
	/// <summary>
	/// pokeball thrown at pokemons for capture animation
	/// </summary>
	public IIconSprite spriteBall;
	public IList<IWindow> pkmnwindows;
	public IDictionary<string, ISpriteWrapper> sprites;
	public bool aborted;
	public bool abortable;
	public bool battlestart;
	public bool messagemode;
	public bool briefmessage;
	public bool showingplayer;
	public bool showingenemy;
	public bool enablePartyAnim;
	public int partyAnimPhase;
	public float xposplayer;
	public float xposenemy;
	public float foeyoffset;
	public float traineryoffset;
	public IViewport viewport;
	#endregion

	public bool inPartyAnimation { get { return @enablePartyAnim && @partyAnimPhase < 3; } }

	public int Id { get { return 0; } }

	private void Awake()
	{
		GameDebug.OnLog += GameDebug_OnLog;
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
		//System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
		Game.LocalizationDictionary = new PokemonUnity.Localization.XmlStringRes(null); //new Debugger());
		Game.LocalizationDictionary.Initialize(englishLocalization, (int)PokemonUnity.Languages.English);
	}

	public IPokeBattle_Scene initialize()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		battle = null;
		lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
		lastmove = new int[] { 0, 0, 0, 0 };
		pkmnwindows = new IWindow[] { null, null, null, null };
		sprites = new Dictionary<string, ISpriteWrapper>() {
			{ UI_MESSAGEBOX, messageBox as ISpriteWrapper },
			{ UI_FIGHTWINDOW, fightWindow as ISpriteWrapper },
			{ UI_COMMANDWINDOW, commandWindow as ISpriteWrapper },
			{ UI_MESSAGEWINDOW, messageWindow as ISpriteWrapper },
			{ "helpwindow", helpWindow as ISpriteWrapper },
			{ "battlebg", battlebg as ISpriteWrapper },
			{ "player", player as ISpriteWrapper },
			{ "playerB", playerB as ISpriteWrapper },
			{ "trainer", trainer as ISpriteWrapper },
			{ "trainer2", trainer2 as ISpriteWrapper },
			{ "playerbase", playerbase as ISpriteWrapper },
			{ "enemybase", enemybase as ISpriteWrapper },
			{ "partybarplayer", partybarplayer as ISpriteWrapper },
			{ "partybarfoe", partybarfoe as ISpriteWrapper },
			{ "capture", spriteBall as ISpriteWrapper },
			{ "battlebox0", battlebox0 as ISpriteWrapper },
			{ "battlebox1", battlebox1 as ISpriteWrapper },
			{ "battlebox2", battlebox2 as ISpriteWrapper },
			{ "battlebox3", battlebox3 as ISpriteWrapper },
			{ "pokemon0", pokemon0 as ISpriteWrapper },
			{ "pokemon1", pokemon1 as ISpriteWrapper },
			{ "pokemon2", pokemon2 as ISpriteWrapper },
			{ "pokemon3", pokemon3 as ISpriteWrapper },
			{ "shadow0", shadow0 as ISpriteWrapper },
			{ "shadow1", shadow1 as ISpriteWrapper },
			{ "shadow2", shadow2 as ISpriteWrapper },
			{ "shadow3", shadow3 as ISpriteWrapper },
		};
		battlestart = true;
		messagemode = false;
		abortable = true;
		aborted = false;
		return this;
	}

	public void pbUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		partyAnimationUpdate();
		//if (sprites["battlebg"] is GameObject g) g.pbUpdate();
	}

	public void pbGraphicsUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		partyAnimationUpdate();
		//if (sprites["battlebg"] is GameObject g) g.pbUpdate();
		//(Game.GameData as Game).Graphics.Update();
	}

	public void pbInputUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		PokemonUnity.Input.update();
		//ToDo: Use GameFramework Input manager instead of Unity
		if (UnityEngine.Input.GetButtonDown("Select") && abortable && !aborted)
		{
			aborted = true;
			battle.pbAbort();
		}
	}

	public void pbShowWindow(int windowtype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@sprites["messagebox"].visible = (windowtype == MESSAGEBOX ||
										  windowtype == COMMANDBOX ||
										  windowtype == FIGHTBOX ||
										  windowtype == BLANK);
		(@sprites["messagewindow"] as IWindow_AdvancedTextPokemon).visible = (windowtype == MESSAGEBOX);
		(@sprites["commandwindow"] as ICommandMenuDisplay).visible = (windowtype == COMMANDBOX);
		(@sprites["fightwindow"] as IFightMenuDisplay).visible = (windowtype == FIGHTBOX);
	}

	public void pbSetMessageMode(bool mode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@messagemode = mode;
		IWindow_AdvancedTextPokemon msgwindow = @sprites[UI_MESSAGEWINDOW] as IWindow_AdvancedTextPokemon;
		if (mode)       // Within Pokémon command
		{
			msgwindow.baseColor = PokeBattle_SceneConstants.MENUBASECOLOR;
			msgwindow.shadowColor = PokeBattle_SceneConstants.MENUSHADOWCOLOR;
			msgwindow.opacity = 255;
			msgwindow.x = 16;
			msgwindow.width = (Game.GameData as Game).Graphics.width;
			msgwindow.height = 96;
			msgwindow.y = (Game.GameData as Game).Graphics.height - msgwindow.height + 2;
		
		}
		else 
		{
			msgwindow.baseColor = PokeBattle_SceneConstants.MESSAGEBASECOLOR;
			msgwindow.shadowColor = PokeBattle_SceneConstants.MESSAGESHADOWCOLOR;
			msgwindow.opacity = 0;
			msgwindow.x = 16;
			msgwindow.width = (Game.GameData as Game).Graphics.width - 32;
			msgwindow.height = 96;
			msgwindow.y = (Game.GameData as Game).Graphics.height - msgwindow.height + 2;
		}*/
	}

	public void pbWaitMessage()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (@briefmessage)
		{
			pbShowWindow(MESSAGEBOX);
			IWindow_AdvancedTextPokemon cw = @sprites["messagewindow"] as IWindow_AdvancedTextPokemon;
			int i = 0;
			do
			{
				pbGraphicsUpdate();
				pbInputUpdate();
				pbFrameUpdate(cw);
				i++;
			} while (i < 60);
			cw.text = "";
			cw.visible = false;
			@briefmessage = false;
		}
	}

	public void pbDisplay(string msg, bool brief)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbDisplayMessage(msg, brief);
	}

	public void pbDisplay(string v)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbDisplay(v, false);
	}

	public void pbDisplayMessage(string msg, bool brief)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbWaitMessage();
		pbRefresh();
		pbShowWindow(MESSAGEBOX);
		IWindow_AdvancedTextPokemon cw = @sprites["messagewindow"] as IWindow_AdvancedTextPokemon;
		cw.text = msg;
		int i = 0;
		//All of this below should be a coroutine that returns the value selected in UI
		do //begin coroutine
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			if (i == 40)
			{
				//end dialog window, after 40 ticks.
				//MessageWindow.Text = "";
				//MessageWindow.IsActive(false);
				cw.text = "";
				cw.visible = false;
				return; //yield return null;
			}
			//ToDo: Use GameFramework Input manager instead of Unity
			if (UnityEngine.Input.GetButtonDown("Start") || abortable)
			{
				if (cw.pause)
				{
					if (!abortable) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					cw.resume();
				}
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

	public void pbDisplayPausedMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbWaitMessage();
		pbRefresh();
		pbShowWindow(MESSAGEBOX);
		if (@messagemode)
		{
			@switchscreen.pbDisplay(msg);
			return;
		}
		IWindow_AdvancedTextPokemon cw = @sprites["messagewindow"] as IWindow_AdvancedTextPokemon;
		cw.text = string.Format("{1:s}\\1", msg);
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) || @abortable)
			{
				if (cw.busy)
				{
					if (cw.pausing && !@abortable) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					cw.resume();
				}
				else if (!inPartyAnimation)
				{
					cw.text = "";
					(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					if (@messagemode) cw.visible = false;
					return;
				}
			}
			cw.update();
		} while (true);
	}

	public bool pbDisplayConfirmMessage(string msg)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		return pbShowCommands(msg, new string[] { Game._INTL("Yes"), Game._INTL("No") }, 1) == 0;
	}

	public bool pbShowCommands(string msg, string[] commands, bool defaultValue)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbWaitMessage();
		pbRefresh();
		pbShowWindow(MESSAGEBOX);
		IWindow_AdvancedTextPokemon dw = @sprites["messagewindow"] as IWindow_AdvancedTextPokemon;
		dw.text = msg;
		//IWindow_CommandPokemon cw = new Window_CommandPokemon(commands);
		IWindow_CommandPokemon cw = commandPokemon.initialize(commands);
		//cw.x = (Game.GameData as Game).Graphics.width - cw.width;
		//cw.y = (Game.GameData as Game).Graphics.height - cw.height - dw.height;
		cw.index = 0;
		cw.viewport = @viewport;
		pbRefresh();
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			cw.visible = !dw.busy;
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			dw.update();
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && defaultValue == false)
			{
				if (dw.busy)
				{
					if (dw.pausing) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					dw.resume();
				}
				else
				{
					cw.dispose();
					dw.text = "";
					return defaultValue;
				}
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))
			{
				if (dw.busy)
				{
					if (dw.pausing) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					dw.resume();
				}
				else
				{
					cw.dispose();
					dw.text = "";
					return cw.index == 0; //the position of "YES" out of the two option
				}
			}
		} while (true);
	}

	public int pbShowCommands(string msg, string[] commands, int defaultValue)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbWaitMessage();
		pbRefresh();
		pbShowWindow(MESSAGEBOX);
		IWindow_AdvancedTextPokemon dw = @sprites["messagewindow"] as IWindow_AdvancedTextPokemon;
		dw.text = msg;
		//IWindow_CommandPokemon cw = new Window_CommandPokemon(commands);
		IWindow_CommandPokemon cw = commandPokemon.initialize(commands);
		//cw.x = (Game.GameData as Game).Graphics.width - cw.width;
		//cw.y = (Game.GameData as Game).Graphics.height - cw.height - dw.height;
		cw.index = 0;
		cw.viewport = @viewport;
		pbRefresh();
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			cw.visible = !dw.busy;
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			dw.update();
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && defaultValue >= 0)
			{
				if (dw.busy)
				{
					if (dw.pausing) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					dw.resume();
				}
				else
				{
					cw.dispose();
					dw.text = "";
					return defaultValue;
				}
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))
			{
				if (dw.busy)
				{
					if (dw.pausing) (AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
					dw.resume();
				}
				else
				{
					cw.dispose();
					dw.text = "";
					return cw.index;
				}
			}
		} while (true);
	}

	public void pbFrameUpdate(IFightMenuDisplay cw)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (cw != null) cw.update();
		pbFrameUpdate();
	}

	public void pbFrameUpdate(ICommandMenuDisplay cw)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (cw != null) cw.update();
		pbFrameUpdate();
	}

	public void pbFrameUpdate(IWindow_CommandPokemon cw)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (cw != null) cw.update();
		pbFrameUpdate();
	}

	public void pbFrameUpdate(IWindow_AdvancedTextPokemon cw)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (cw != null) cw.update();
		pbFrameUpdate();
	}

	public void pbFrameUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//if (cw != null) cw.update();
		for (int i = 0; i < 4; i++)
		{
			if (@sprites.ContainsKey($"battlebox#{i}") && @sprites[$"battlebox#{i}"] != null)
			{
				(@sprites[$"battlebox#{i}"] as IPokemonDataBox).update();
			}
			if (@sprites.ContainsKey($"pokemon#{i}") && @sprites[$"pokemon#{i}"] != null)
			{
				(@sprites[$"pokemon#{i}"] as IPokemonBattlerSprite).update();
			}
		}
	}

	public void pbRefresh()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		for (int i = 0; i < 4; i++)
		{
			if (@sprites.ContainsKey($"battlebox#{i}") && @sprites[$"battlebox#{i}"] != null)
			{
				(@sprites[$"battlebox#{i}"] as IPokemonDataBox).refresh();
			}
		}
	}

	/// <summary>
	/// Instantiate Sprite GameObject, then adds to <see cref="sprites"/> by <paramref name="id"/> as lookup reference
	/// </summary>
	/// <param name="id"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="filename"></param>
	/// <param name="viewport"></param>
	public IIconSprite pbAddSprite(string id, float x, float y, string filename, IViewport viewport)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//IIconSprite sprite = new IconSprite(x, y, viewport);
		IIconSprite sprite = null; //UnityEngine.GameObject.Instantiate<IIconSprite>().initialize((float)x, (float)y, viewport);
		if (!string.IsNullOrEmpty(filename))
		{
			sprite.setBitmap(filename); //rescue null;
		}
		@sprites[id] = sprite;
		return sprite;
	}

	public void pbAddPlane(string id, string filename, IViewport viewport)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//IAnimatedPlane sprite = new AnimatedPlane(viewport);
		//if (!string.IsNullOrEmpty(filename))
		//{
		//	sprite.setBitmap(filename);
		//}
		//@sprites[id] = sprite;
		//return sprite;
	}

	public void pbDisposeSprites()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//pbDisposeSpriteHash(@sprites); //Clears list of GameObjects in scene?...
	}

	public void pbBeginCommandPhase()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//  Called whenever a new round begins.
		@battlestart = false;
	}

	/// <summary>
	/// Tween sprite on-screen over 20 frames
	/// </summary>
	/// <param name="index"></param>
	public void pbShowOpponent(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		string trainerfile = string.Empty;
		if (@battle.opponent != null)
		{
			if (@battle.opponent.Length > 1) //(@battle.opponent is Array)
			{
				trainerfile = pbTrainerSpriteFile(@battle.opponent[index].trainertype);
			}
			else
			{
				trainerfile = pbTrainerSpriteFile(@battle.opponent[0].trainertype);
			}
		}
		else
		{
			trainerfile = "Graphics/Characters/trfront";
		}
		pbAddSprite("trainer", (Game.GameData as Game).Graphics.width, PokeBattle_SceneConstants.FOETRAINER_Y,
		   trainerfile, @viewport);
		if (@sprites["trainer"].bitmap != null)
		{
			@sprites["trainer"].y -= @sprites["trainer"].bitmap.height;
			@sprites["trainer"].z = 8;
		}
		int i = 0;
		do //20.times ;
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			@sprites["trainer"].x -= 6;
			i++;
		} while (i < 20);*/
	}

	/// <summary>
	/// Tween sprite off-screen over 20 frames
	/// </summary>
	public void pbHideOpponent()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		int i = 0;
		do //20.times ;
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			@sprites["trainer"].x += 6;
			i++;
		} while (i < 20);
	}

	public void pbShowHelp(string text)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).resizeToFit(text, (Game.GameData as Game).Graphics.width);
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).y = 0;
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).x = 0;
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).text = text;
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).visible = true;
	}

	public void pbHideHelp()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).visible = false;
	}

	public void pbBackdrop()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		PokemonUnity.Overworld.Environments environ = @battle.environment;
		//  Choose backdrop
		string backdrop = "Field";
		if (environ == Environments.Cave)
		{
			backdrop = "Cave";
		}
		else if (environ == Environments.MovingWater || environ == Environments.StillWater)
		{
			backdrop = "Water";
		}
		else if (environ == Environments.Underwater)
		{
			backdrop = "Underwater";
		}
		else if (environ == Environments.Rock)
		{
			backdrop = "Mountain";
		}
		else
		{
			if (Game.GameData.GameMap == null || (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.pbGetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor)) //!pbGetMetadata(Game.GameData.GameMap.map_id, MetadataOutdoor))
			{
				backdrop = "IndoorA";
			}
		}
		if (Game.GameData.GameMap != null && Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc m1)
		{
			string back = m1.pbGetMetadata(Game.GameData.GameMap.map_id).Map.BattleBack;
			if (back != null && back != "")
			{
				backdrop = back;
			}
		}
		if (Game.GameData.Global != null && Game.GameData.Global.nextBattleBack != null)
		{
			backdrop = Game.GameData.Global.nextBattleBack.name;
		}
		//  Choose bases
		string _base = "";
		string trialname = "";
		if (environ == Environments.Grass || environ == Environments.TallGrass)
		{
			trialname = "Grass";
		}
		else if (environ == Environments.Sand)
		{
			trialname = "Sand";
		}
		else if (Game.GameData.Global.surfing)
		{
			trialname = "Water";
		}
		if (pbResolveBitmap(string.Format("Graphics/Battlebacks/playerbase" + backdrop + trialname)))
		{
			_base = trialname;
		}
		//  Choose time of day
		string time = "";
		if (Core.ENABLESHADING)
		{
			trialname = "";
			DateTime timenow = pbGetTimeNow();
			if (Game.PBDayNight.isNight(timenow))
			{
				trialname = "Night";
			}
			else if (Game.PBDayNight.isEvening(timenow))
			{
				trialname = "Eve";
			}
			if (pbResolveBitmap(string.Format("Graphics/Battlebacks/battlebg" + backdrop + trialname)))
			{
				time = trialname;
			}
		}
		//  Apply graphics
		string battlebg = "Graphics/Battlebacks/battlebg" + backdrop + time;
		string enemybase = "Graphics/Battlebacks/enemybase" + backdrop + _base + time;
		string playerbase = "Graphics/Battlebacks/playerbase" + backdrop + _base + time;
		pbAddPlane("battlebg", battlebg, @viewport);
		pbAddSprite("playerbase",
		   PokeBattle_SceneConstants.PLAYERBASEX,
		   PokeBattle_SceneConstants.PLAYERBASEY, playerbase, @viewport);
		if (@sprites["playerbase"].bitmap != null) @sprites["playerbase"].x -= @sprites["playerbase"].bitmap.width / 2;
		if (@sprites["playerbase"].bitmap != null) @sprites["playerbase"].y -= @sprites["playerbase"].bitmap.height;
		pbAddSprite("enemybase",
		   PokeBattle_SceneConstants.FOEBASEX,
		   PokeBattle_SceneConstants.FOEBASEY, enemybase, @viewport);
		if (@sprites["enemybase"].bitmap != null) @sprites["enemybase"].x -= @sprites["enemybase"].bitmap.width / 2;
		if (@sprites["enemybase"].bitmap != null) @sprites["enemybase"].y -= @sprites["enemybase"].bitmap.height / 2;
		@sprites["battlebg"].z = 0;
		@sprites["playerbase"].z = 1;
		@sprites["enemybase"].z = 1;*/
	}

	/// <summary>
	/// Shows the party line-ups appearing on-screen
	/// </summary>
	///TODO: Can rewrite this to use Coroutine and LeanTween to animate
	public void partyAnimationUpdate()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
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
			while (@xposplayer < (Game.GameData as Game).Graphics.width)
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
				pbAddSprite($"player#{i}", //Instantiate player GameObject, then adds to dictionary for tracking...
				   @xposplayer + i * ballmovedist * 6, PokeBattle_SceneConstants.PLAYERPARTYBALL1_Y,
				   ballgraphic, @viewport);
				@sprites[$"player#{i}"].z = 41;
				//  Choose the ball's graphic (opponent's side)
				ballgraphic = "Graphics/Pictures/ballempty";
				int enemyindex = i;
				if (@battle.doublebattle && i >= 3)
				{
					enemyindex = (i % 3) + @battle.pbSecondPartyBegin(1);
				}
				if (enemyindex < @battle.party2.Length && @battle.party2[enemyindex].IsNotNullOrNone())
				{
					if (@battle.party2[enemyindex].HP <= 0 || @battle.party2[enemyindex].isEgg)
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
				pbAddSprite($"enemy#{i}",
				   @xposenemy - i * ballmovedist * 6, PokeBattle_SceneConstants.FOEPARTYBALL1_Y,
				   ballgraphic, @viewport);
				@sprites[$"enemy#{i}"].z = 41;
			}
			@partyAnimPhase = 2;
		}
		//  Balls slide on
		if (@partyAnimPhase == 2)
		{
			for (int i = 0; i < 6; i++)
			{
				if (@sprites[$"enemy#{i}"].x < PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
				{
					@sprites[$"enemy#{i}"].x += ballmovedist;
					@sprites[$"player#{i}"].x -= ballmovedist;
					if (@sprites[$"enemy#{i}"].x >= PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
					{
						@sprites[$"enemy#{i}"].x = PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP;
						@sprites[$"player#{i}"].x = PokeBattle_SceneConstants.PLAYERPARTYBALL1_X + i * PokeBattle_SceneConstants.PLAYERPARTYBALL_GAP;
						if (i == 5)
						{
							@partyAnimPhase = 3;
						}
					}
				}
			}
		}*/
	}

	public void pbStartBattle(IBattle battle)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		//  Called whenever the battle begins
		this.battle = battle;
		@lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
		@lastmove = new int[] { 0, 0, 0, 0 };
		@showingplayer = true;
		@showingenemy = true;
		@sprites.Clear();
		//@viewport = new Viewport(0, (Game.GameData as Game).Graphics.height / 2, (Game.GameData as Game).Graphics.width, 0);
		@viewport.initialize(0, (Game.GameData as Game).Graphics.height / 2, (Game.GameData as Game).Graphics.width, 0);
		//@viewport.z = 99999;
		@traineryoffset = ((Game.GameData as Game).Graphics.height - 320); // Adjust player's side for screen size
		@foeyoffset = (float)Math.Floor(@traineryoffset * 3f / 4f);  // Adjust foe's side for screen size
		pbBackdrop();
		pbAddSprite("partybarfoe",
		   PokeBattle_SceneConstants.FOEPARTYBAR_X,
		   PokeBattle_SceneConstants.FOEPARTYBAR_Y,
		   "Graphics/Pictures/battleLineup", @viewport);
		pbAddSprite("partybarplayer",
		   PokeBattle_SceneConstants.PLAYERPARTYBAR_X,
		   PokeBattle_SceneConstants.PLAYERPARTYBAR_Y,
		   "Graphics/Pictures/battleLineup", @viewport);
		@sprites["partybarfoe"].x -= @sprites["partybarfoe"].bitmap.width;
		@sprites["partybarplayer"].mirror = true;
		@sprites["partybarfoe"].z = 40;
		@sprites["partybarplayer"].z = 40;
		@sprites["partybarfoe"].visible = false;
		@sprites["partybarplayer"].visible = false;
		string trainerfile = string.Empty;
		if (@battle.player.Length > 1) //(@battle.player is Array)
		{
			trainerfile = pbPlayerSpriteBackFile(@battle.player[0].trainertype);
			pbAddSprite("player",
				 PokeBattle_SceneConstants.PLAYERTRAINERD1_X,
				 PokeBattle_SceneConstants.PLAYERTRAINERD1_Y, trainerfile, @viewport);
			trainerfile = pbTrainerSpriteBackFile(@battle.player[1].trainertype);
			pbAddSprite("playerB",
				 PokeBattle_SceneConstants.PLAYERTRAINERD2_X,
				 PokeBattle_SceneConstants.PLAYERTRAINERD2_Y, trainerfile, @viewport);
			if (@sprites["player"].bitmap != null)
			{
				if (@sprites["player"].bitmap.width > @sprites["player"].bitmap.height)
				{
					@sprites["player"].src_rect.x = 0;
					@sprites["player"].src_rect.width = @sprites["player"].bitmap.width / 5;
				}
				@sprites["player"].x -= (@sprites["player"].src_rect.width / 2);
				@sprites["player"].y -= @sprites["player"].bitmap.height;
				@sprites["player"].z = 30;
			}
			if (@sprites["playerB"].bitmap != null)
			{
				if (@sprites["playerB"].bitmap.width > @sprites["playerB"].bitmap.height)
				{
					@sprites["playerB"].src_rect.x = 0;
					@sprites["playerB"].src_rect.width = @sprites["playerB"].bitmap.width / 5;
				}
				@sprites["playerB"].x -= (@sprites["playerB"].src_rect.width / 2);
				@sprites["playerB"].y -= @sprites["playerB"].bitmap.height;
				@sprites["playerB"].z = 31;
			}
		}
		else
		{
			trainerfile = pbPlayerSpriteBackFile(@battle.player[0].trainertype);
			pbAddSprite("player",
				 PokeBattle_SceneConstants.PLAYERTRAINER_X,
				 PokeBattle_SceneConstants.PLAYERTRAINER_Y, trainerfile, @viewport);
			if (@sprites["player"].bitmap != null)
			{
				if (@sprites["player"].bitmap.width > @sprites["player"].bitmap.height)
				{
					@sprites["player"].src_rect.x = 0;
					@sprites["player"].src_rect.width = @sprites["player"].bitmap.width / 5;
				}
				@sprites["player"].x -= (@sprites["player"].src_rect.width / 2);
				@sprites["player"].y -= @sprites["player"].bitmap.height;
				@sprites["player"].z = 30;
			}
		}
		if (@battle.opponent != null)
		{
			if (@battle.opponent.Length > 1) //(@battle.opponent is Array)
			{
				trainerfile = pbTrainerSpriteFile(@battle.opponent[1].trainertype);
				pbAddSprite("trainer2",
				   PokeBattle_SceneConstants.FOETRAINERD2_X,
				   PokeBattle_SceneConstants.FOETRAINERD2_Y, trainerfile, @viewport);
				trainerfile = pbTrainerSpriteFile(@battle.opponent[0].trainertype);
				pbAddSprite("trainer",
				   PokeBattle_SceneConstants.FOETRAINERD1_X,
				   PokeBattle_SceneConstants.FOETRAINERD1_Y, trainerfile, @viewport);
			}
			else
			{
				trainerfile = pbTrainerSpriteFile(@battle.opponent.trainertype);
				pbAddSprite("trainer",
				   PokeBattle_SceneConstants.FOETRAINER_X,
				   PokeBattle_SceneConstants.FOETRAINER_Y, trainerfile, @viewport);
			}
		}
		else
		{
			trainerfile = "Graphics/Characters/trfront";
			pbAddSprite("trainer",
				 PokeBattle_SceneConstants.FOETRAINER_X,
				 PokeBattle_SceneConstants.FOETRAINER_Y, trainerfile, @viewport);
		}
		if (@sprites["trainer"].bitmap != null)
		{
			@sprites["trainer"].x -= (@sprites["trainer"].bitmap.width / 2);
			@sprites["trainer"].y -= @sprites["trainer"].bitmap.height;
			@sprites["trainer"].z = 8;
		}
		if (@sprites["trainer2"] != null && @sprites["trainer2"].bitmap != null)
		{
			@sprites["trainer2"].x -= (@sprites["trainer2"].bitmap.width / 2);
			@sprites["trainer2"].y -= @sprites["trainer2"].bitmap.height;
			@sprites["trainer2"].z = 7;
		}
		//@sprites["shadow0"] = new IconSprite(0, 0, @viewport);
		(@sprites["shadow0"] as IIconSprite).initialize(0, 0, @viewport);
		(@sprites["shadow0"] as IIconSprite).z = 3;
		pbAddSprite("shadow1", 0, 0, "Graphics/Pictures/battleShadow", @viewport);
		(@sprites["shadow1"] as IIconSprite).z = 3;
		(@sprites["shadow1"] as IIconSprite).visible = false;
		//@sprites["pokemon0"] = new PokemonBattlerSprite(battle.doublebattle, 0, @viewport);
		(@sprites["pokemon0"] as IPokemonBattlerSprite).initialize(battle.doublebattle, 0, @viewport);
		(@sprites["pokemon0"] as IPokemonBattlerSprite).z = 21;
		//@sprites["pokemon1"] = new PokemonBattlerSprite(battle.doublebattle, 1, @viewport);
		(@sprites["pokemon1"] as IPokemonBattlerSprite).initialize(battle.doublebattle, 1, @viewport);
		(@sprites["pokemon1"] as IPokemonBattlerSprite).z = 16;
		if (battle.doublebattle)
		{
			//@sprites["shadow2"] = new IconSprite(0, 0, @viewport);
			(@sprites["shadow2"] as IIconSprite).initialize(0, 0, @viewport);
			(@sprites["shadow2"] as IIconSprite).z = 3;
			pbAddSprite("shadow3", 0, 0, "Graphics/Pictures/battleShadow", @viewport);
			(@sprites["shadow3"] as IIconSprite).z = 3;
			(@sprites["shadow3"] as IIconSprite).visible = false;
			//@sprites["pokemon2"] = new PokemonBattlerSprite(battle.doublebattle, 2, @viewport);
			(@sprites["pokemon2"] as IPokemonBattlerSprite).initialize(battle.doublebattle, 2, @viewport);
			(@sprites["pokemon2"] as IPokemonBattlerSprite).z = 26;
			//@sprites["pokemon3"] = new PokemonBattlerSprite(battle.doublebattle, 3, @viewport);
			(@sprites["pokemon3"] as IPokemonBattlerSprite).initialize(battle.doublebattle, 3, @viewport);
			(@sprites["pokemon3"] as IPokemonBattlerSprite).z = 11;
		}
		//@sprites["battlebox0"] = new PokemonDataBox(battle.battlers[0], battle.doublebattle, @viewport);
		//@sprites["battlebox1"] = new PokemonDataBox(battle.battlers[1], battle.doublebattle, @viewport);
		(@sprites["battlebox0"] as IPokemonDataBox).initialize(battle.battlers[0], battle.doublebattle, @viewport);
		(@sprites["battlebox1"] as IPokemonDataBox).initialize(battle.battlers[1], battle.doublebattle, @viewport);
		if (battle.doublebattle)
		{
			//@sprites["battlebox2"] = new PokemonDataBox(battle.battlers[2], battle.doublebattle, @viewport);
			//@sprites["battlebox3"] = new PokemonDataBox(battle.battlers[3], battle.doublebattle, @viewport);
			(@sprites["battlebox2"] as IPokemonDataBox).initialize(battle.battlers[2], battle.doublebattle, @viewport);
			(@sprites["battlebox3"] as IPokemonDataBox).initialize(battle.battlers[3], battle.doublebattle, @viewport);
		}
		pbAddSprite("messagebox", 0, (Game.GameData as Game).Graphics.height - 96, "Graphics/Pictures/battleMessage", @viewport);
		@sprites["messagebox"].z = 90;
		//@sprites["helpwindow"] = new Window_UnformattedTextPokemon().WithSize("", 0, 0, 32, 32, @viewport);
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).initialize().WithSize("", 0, 0, 32, 32, @viewport);
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).visible = false;
		(@sprites["helpwindow"] as IWindow_UnformattedTextPokemon).z = 90;
		//@sprites["messagewindow"] = new Window_AdvancedTextPokemon("");
		(@sprites["messagewindow"] as IWindow_AdvancedTextPokemon).initialize("");
		(@sprites["messagewindow"] as IWindow_AdvancedTextPokemon).letterbyletter = true;
		(@sprites["messagewindow"] as IWindow_AdvancedTextPokemon).viewport = @viewport;
		(@sprites["messagewindow"] as IWindow_AdvancedTextPokemon).z = 100;
		//@sprites["commandwindow"] = new CommandMenuDisplay(@viewport);
		(@sprites["commandwindow"] as ICommandMenuDisplay).initialize(@viewport);
		(@sprites["commandwindow"] as ICommandMenuDisplay).z = 100;
		//@sprites["fightwindow"] = new FightMenuDisplay(null, @viewport);
		(@sprites["fightwindow"] as IFightMenuDisplay).initialize(null, @viewport);
		(@sprites["fightwindow"] as IFightMenuDisplay).z = 100;
		pbShowWindow(MESSAGEBOX);
		pbSetMessageMode(false);
		IPokemonBattlerSprite trainersprite1 = @sprites["trainer"] as IPokemonBattlerSprite;
		IPokemonBattlerSprite trainersprite2 = @sprites["trainer2"] as IPokemonBattlerSprite;
		if (@battle.opponent == null)
		{
			@sprites["trainer"].visible = false;
			if (@battle.party2.Length >= 1)
			{
				Pokemons species = Pokemons.NONE;
				if (@battle.party2.Length == 1) //Single Battle
				{
					species = @battle.party2[0].Species;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).setPokemonBitmap(@battle.party2[0], false);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).tone = new Tone(-128, -128, -128, -128);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).x = PokeBattle_SceneConstants.FOEBATTLER_X;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).x -= (@sprites["pokemon1"] as IPokemonBattlerSprite).width / 2;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).y = PokeBattle_SceneConstants.FOEBATTLER_Y;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).y += adjustBattleSpriteY(@sprites["pokemon1"], species, 1);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).visible = true;
					(@sprites["shadow1"] as IIconSprite).x = PokeBattle_SceneConstants.FOEBATTLER_X;
					(@sprites["shadow1"] as IIconSprite).y = PokeBattle_SceneConstants.FOEBATTLER_Y;
					if ((@sprites["shadow1"] as IIconSprite).bitmap != null) (@sprites["shadow1"] as IIconSprite).x -= (@sprites["shadow1"] as IIconSprite).bitmap.width / 2;
					if ((@sprites["shadow1"] as IIconSprite).bitmap != null) (@sprites["shadow1"] as IIconSprite).y -= (@sprites["shadow1"] as IIconSprite).bitmap.height / 2;
					(@sprites["shadow1"] as IIconSprite).visible = showShadow(species);
					trainersprite1 = @sprites["pokemon1"] as IPokemonBattlerSprite;
				}
				else if (@battle.party2.Length == 2) //Double Battle 
				{
					species = @battle.party2[0].Species;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).setPokemonBitmap(@battle.party2[0], false);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).tone = new Tone(-128, -128, -128, -128);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).x = PokeBattle_SceneConstants.FOEBATTLERD1_X;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).x -= (@sprites["pokemon1"] as IPokemonBattlerSprite).width / 2;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).y = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
					(@sprites["pokemon1"] as IPokemonBattlerSprite).y += adjustBattleSpriteY(@sprites["pokemon1"] as IPokemonBattlerSprite, species, 1);
					(@sprites["pokemon1"] as IPokemonBattlerSprite).visible = true;
					(@sprites["shadow1"] as IIconSprite).x = PokeBattle_SceneConstants.FOEBATTLERD1_X;
					(@sprites["shadow1"] as IIconSprite).y = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
					if ((@sprites["shadow1"] as IIconSprite).bitmap != null) (@sprites["shadow1"] as IIconSprite).x -= (@sprites["shadow1"] as IIconSprite).bitmap.width / 2;
					if ((@sprites["shadow1"] as IIconSprite).bitmap != null) (@sprites["shadow1"] as IIconSprite).y -= (@sprites["shadow1"] as IIconSprite).bitmap.height / 2;
					(@sprites["shadow1"] as IIconSprite).visible = showShadow(species);
					trainersprite1 = @sprites["pokemon1"] as IPokemonBattlerSprite;
					species = @battle.party2[1].Species;
					(@sprites["pokemon3"] as IPokemonBattlerSprite).setPokemonBitmap(@battle.party2[1], false);
					(@sprites["pokemon3"] as IPokemonBattlerSprite).tone = new Tone(-128, -128, -128, -128);
					(@sprites["pokemon3"] as IPokemonBattlerSprite).x = PokeBattle_SceneConstants.FOEBATTLERD2_X;
					(@sprites["pokemon3"] as IPokemonBattlerSprite).x -= (@sprites["pokemon3"] as IPokemonBattlerSprite).width / 2;
					(@sprites["pokemon3"] as IPokemonBattlerSprite).y = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
					(@sprites["pokemon3"] as IPokemonBattlerSprite).y += adjustBattleSpriteY(@sprites["pokemon3"] as IPokemonBattlerSprite, species, 3);
					(@sprites["pokemon3"] as IPokemonBattlerSprite).visible = true;
					(@sprites["shadow3"] as IIconSprite).x = PokeBattle_SceneConstants.FOEBATTLERD2_X;
					(@sprites["shadow3"] as IIconSprite).y = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
					if ((@sprites["shadow3"] as IIconSprite).bitmap != null) (@sprites["shadow3"] as IIconSprite).x -= (@sprites["shadow3"] as IIconSprite).bitmap.width / 2;
					if ((@sprites["shadow3"] as IIconSprite).bitmap != null) (@sprites["shadow3"] as IIconSprite).y -= (@sprites["shadow3"] as IIconSprite).bitmap.height / 2;
					(@sprites["shadow3"] as IIconSprite).visible = showShadow(species);
					trainersprite2 = @sprites["pokemon3"] as IPokemonBattlerSprite;
				}
			}
		}
		// ################
		//  Move trainers/bases/etc. off-screen
		float[] oldx = new float[8];
		oldx[0] = @sprites["playerbase"].x; @sprites["playerbase"].x += (Game.GameData as Game).Graphics.width;
		oldx[1] = @sprites["player"].x; @sprites["player"].x += (Game.GameData as Game).Graphics.width;
		if (@sprites["playerB"] != null)
		{
			oldx[2] = @sprites["playerB"].x; @sprites["playerB"].x += (Game.GameData as Game).Graphics.width;
		}
		oldx[3] = @sprites["enemybase"].x; @sprites["enemybase"].x -= (Game.GameData as Game).Graphics.width;
		oldx[4] = trainersprite1.x; trainersprite1.x -= (Game.GameData as Game).Graphics.width;
		if (trainersprite2 != null)
		{
			oldx[5] = trainersprite2.x; trainersprite2.x -= (Game.GameData as Game).Graphics.width;
		}
		oldx[6] = (@sprites["shadow1"] as IIconSprite).x; (@sprites["shadow1"] as IIconSprite).x -= (Game.GameData as Game).Graphics.width;
		if (@sprites["shadow3"] != null)
		{
			oldx[7] = (@sprites["shadow3"] as IIconSprite).x; (@sprites["shadow3"] as IIconSprite).x -= (Game.GameData as Game).Graphics.width;
		}
		@sprites["partybarfoe"].x -= PokeBattle_SceneConstants.FOEPARTYBAR_X;
		@sprites["partybarplayer"].x += (Game.GameData as Game).Graphics.width - PokeBattle_SceneConstants.PLAYERPARTYBAR_X;
		// ################
		int appearspeed = 12;
		int n = 0;
		do {
			bool tobreak = true;

			if (@viewport.rect.y > 0)
			{
				@viewport.rect.y -= appearspeed / 2;
				if (@viewport.rect.y < 0) @viewport.rect.y = 0;
				@viewport.rect.height += appearspeed;
				if (@viewport.rect.height > (Game.GameData as Game).Graphics.height) @viewport.rect.height = (Game.GameData as Game).Graphics.height;
				tobreak = false;
			}
			if (!tobreak)
			{
				foreach (var i in @sprites)
				{
					//i[1].ox = @viewport.rect.x;
					//i[1].oy = @viewport.rect.y;
					i.Value.ox = @viewport.rect.x;
					i.Value.oy = @viewport.rect.y;
				}
			}
			if (@sprites["playerbase"].x > oldx[0])
			{
				@sprites["playerbase"].x -= appearspeed; tobreak = false;
				if (@sprites["playerbase"].x < oldx[0]) @sprites["playerbase"].x = oldx[0];
			}
			if (@sprites["player"].x > oldx[1])
			{
				@sprites["player"].x -= appearspeed; tobreak = false;
				if (@sprites["player"].x < oldx[1]) @sprites["player"].x = oldx[1];
			}
			if (@sprites["playerB"] != null && @sprites["playerB"].x > oldx[2])
			{
				@sprites["playerB"].x -= appearspeed; tobreak = false;
				if (@sprites["playerB"].x < oldx[2]) @sprites["playerB"].x = oldx[2];
			}
			if (@sprites["enemybase"].x < oldx[3])
			{
				@sprites["enemybase"].x += appearspeed; tobreak = false;
				if (@sprites["enemybase"].x > oldx[3]) @sprites["enemybase"].x = oldx[3];
			}
			if (trainersprite1.x < oldx[4])
			{
				trainersprite1.x += appearspeed; tobreak = false;
				if (trainersprite1.x > oldx[4]) trainersprite1.x = oldx[4];
			}
			if (trainersprite2 != null && trainersprite2.x < oldx[5])
			{
				trainersprite2.x += appearspeed; tobreak = false;
				if (trainersprite2.x > oldx[5]) trainersprite2.x = oldx[5];
			}
			if ((@sprites["shadow1"] as IIconSprite).x < oldx[6])
			{
				(@sprites["shadow1"] as IIconSprite).x += appearspeed; tobreak = false;
				if ((@sprites["shadow1"] as IIconSprite).x > oldx[6]) (@sprites["shadow1"] as IIconSprite).x = oldx[6];
			}
			if (@sprites["shadow3"] != null && (@sprites["shadow3"] as IIconSprite).x < oldx[7])
			{
				(@sprites["shadow3"] as IIconSprite).x += appearspeed; tobreak = false;
				if ((@sprites["shadow3"] as IIconSprite).x > oldx[7]) (@sprites["shadow3"] as IIconSprite).x = oldx[7];
			}
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (tobreak) break; n++;
		} while (n < (1 + (Game.GameData as Game).Graphics.width / appearspeed));
		// ################
		if (@battle.opponent != null) {
			@enablePartyAnim=true;
			@partyAnimPhase=0;
			@sprites["partybarfoe"].visible=true;
			@sprites["partybarplayer"].visible=true;
		} else
		{
			pbPlayCry(@battle.party2[0]);   // Play cry for wild Pokémon
			(@sprites["battlebox1"] as IPokemonDataBox).appear();
			if (@battle.party2.Length == 2) (@sprites["battlebox3"] as IPokemonDataBox).appear();
			bool appearing = true;
			do { //begin;
				pbGraphicsUpdate();
				pbInputUpdate();
				pbFrameUpdate();
				if ((@sprites["pokemon1"] as IPokemonBattlerSprite).tone.red < 0)		(@sprites["pokemon1"] as IPokemonBattlerSprite).tone.red += 8;
				if ((@sprites["pokemon1"] as IPokemonBattlerSprite).tone.blue < 0)		(@sprites["pokemon1"] as IPokemonBattlerSprite).tone.blue += 8;
				if ((@sprites["pokemon1"] as IPokemonBattlerSprite).tone.green < 0)		(@sprites["pokemon1"] as IPokemonBattlerSprite).tone.green += 8;
				if ((@sprites["pokemon1"] as IPokemonBattlerSprite).tone.gray < 0)		(@sprites["pokemon1"] as IPokemonBattlerSprite).tone.gray += 8;
				appearing = (@sprites["battlebox1"] as IPokemonDataBox).appearing;
				if (@battle.party2.Length == 2)
				{
					if ((@sprites["pokemon3"] as IPokemonBattlerSprite).tone.red < 0)		(@sprites["pokemon3"] as IPokemonBattlerSprite).tone.red += 8;
					if ((@sprites["pokemon3"] as IPokemonBattlerSprite).tone.blue < 0)		(@sprites["pokemon3"] as IPokemonBattlerSprite).tone.blue += 8;
					if ((@sprites["pokemon3"] as IPokemonBattlerSprite).tone.green < 0)		(@sprites["pokemon3"] as IPokemonBattlerSprite).tone.green += 8;
					if ((@sprites["pokemon3"] as IPokemonBattlerSprite).tone.gray < 0)		(@sprites["pokemon3"] as IPokemonBattlerSprite).tone.gray += 8;
					appearing = (appearing || (@sprites["battlebox3"] as IPokemonDataBox).appearing);
				}
			} while (appearing);
			//  Show shiny animation for wild Pokémon
			if (@battle.battlers[1].IsShiny && @battle.battlescene)
			{
				pbCommonAnimation("Shiny", @battle.battlers[1], null);
			}
			if (@battle.party2.Length == 2)
			{
				if (@battle.battlers[3].IsShiny && @battle.battlescene)
				{
					pbCommonAnimation("Shiny", @battle.battlers[3], null);
				}
			}
		}*/
	}

	public void pbEndBattle(BattleResults result)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@abortable = false;
		pbShowWindow(BLANK);
		//  Fade out all sprites
		(AudioHandler as IGameAudioPlay).pbBGMFade(1.0f);
		//pbFadeOutAndHide(@sprites);
		pbDisposeSprites();
	}

	/// <summary>
	/// Animates the pokemon leaving the battle and returning back to party/pokeball
	/// </summary>
	/// <param name="battlerindex"></param>
	public void pbRecall(int battlerindex)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@briefmessage = false;
		float origin = 0;
		if (@battle.pbIsOpposing(battlerindex))
		{
			origin = PokeBattle_SceneConstants.FOEBATTLER_Y;
			if (@battle.doublebattle)
			{
				if (battlerindex == 1) origin = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
				if (battlerindex == 3) origin = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
			}
			(@sprites[$"shadow#{battlerindex}"] as IIconSprite).visible = false;
		}
		else
		{
			origin = PokeBattle_SceneConstants.PLAYERBATTLER_Y;
			if (@battle.doublebattle)
			{
				if (battlerindex == 0) origin = PokeBattle_SceneConstants.PLAYERBATTLERD1_Y;
				if (battlerindex == 2) origin = PokeBattle_SceneConstants.PLAYERBATTLERD2_Y;
			}
		}
		IPokemonBattlerSprite spritePoke = @sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite;
		IPictureEx picturePoke = new PictureEx(spritePoke.z);
		float[] dims = new float[] { spritePoke.x, spritePoke.y };
		float[] center = getSpriteCenter(spritePoke);
		//  starting positions
		picturePoke.moveVisible(1, true);
		picturePoke.moveOrigin(1, PictureOrigin.Center);
		picturePoke.moveXY(0, 1, center[0], center[1]);
		//  directives
		//Tween animation types for values below...
		picturePoke.moveTone(10, 1, new Tone(248, 248, 248, 248));
		int delay = picturePoke.totalDuration;
		picturePoke.moveSE(delay, "Audio/SE/recall");
		picturePoke.moveZoom(15, delay, 0);
		picturePoke.moveXY(15, delay, center[0], origin);
		picturePoke.moveVisible(picturePoke.totalDuration, false);
		picturePoke.moveTone(0, picturePoke.totalDuration, new Tone(0, 0, 0, 0));
		picturePoke.moveOrigin(picturePoke.totalDuration, PictureOrigin.TopLeft);
		//begin coroutine of animation until finished
		do //;loop
		{
			picturePoke.update();
			setPictureSprite(spritePoke, picturePoke);
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (!picturePoke.running) break;
		} while (true);*/
	}

	public void pbTrainerSendOut(int battlerindex, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		IPokemon illusionpoke=@battle.battlers[battlerindex].effects.Illusion;
		@briefmessage=false;
		//ITrainerFadeAnimation fadeanim=null;
		while (inPartyAnimation) { } //i guess, delay action until trigger criteria is met
		if (@showingenemy) {
			//fadeanim=new TrainerFadeAnimation(@sprites);
			fadeanim.initialize(@sprites);
		}
		int frame=0;
		(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(pkmn,false);
		if (illusionpoke != null) {
			(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(illusionpoke,false);
		}
		//IPokeballSendOutAnimation sendout=new PokeballSendOutAnimation(@sprites[$"pokemon#{battlerindex}"],
		sendout.initialize(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite,
			@sprites,@battle.battlers[battlerindex],illusionpoke,@battle.doublebattle);
		do  //;loop
		{
			if (fadeanim != null) fadeanim.update();
			frame += 1;
			if (frame == 1) {
				(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).appear();
			}
			if (frame >= 10) {
				sendout.update();
			}
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if ((fadeanim == null || fadeanim.animdone) && sendout.animdone &&
				!(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).appearing) break;
		} while (true);
		if (@battle.battlers[battlerindex].IsShiny && @battle.battlescene) {
			pbCommonAnimation("Shiny",@battle.battlers[battlerindex],null);
		}
		sendout.dispose();
		if (@showingenemy) {
			@showingenemy=false;
			pbDisposeSprite(@sprites,"trainer");
			pbDisposeSprite(@sprites,"partybarfoe");
			for (int i = 0; i < 6; i++) {
				pbDisposeSprite(@sprites,$"enemy#{i}");
			}
		}
		pbRefresh();*/
	}

	public void pbTrainerSendOut(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//foreach trainer in battle
		//foreach(int i in battle.)
			pbTrainerSendOut(0, pkmn);
	}

	/// <summary>
	/// Player sending out Pokémon
	/// </summary>
	/// <param name="battlerindex"></param>
	/// <param name="pkmn"></param>
	public void pbSendOut(int battlerindex, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		while (inPartyAnimation){ }
		IPokemon illusionpoke=@battle.battlers[battlerindex].effects.Illusion;
		Items balltype=pkmn.ballUsed;
		if (illusionpoke != null) balltype=illusionpoke.ballUsed;
		string ballbitmap=string.Format("Graphics/Pictures/ball%02d",balltype);
		pictureBall=new PictureEx(32);
		int delay=1;
		pictureBall.moveVisible(delay,true);
		pictureBall.moveName(delay,ballbitmap);
		pictureBall.moveOrigin(delay,PictureOrigin.nenter);
		//  Setting the ball's movement path
		int[][] path=new int[][] { new int[] {0,   146}, new int[] {10,  134}, new int[] {21,  122}, new int[] {30,  112}, 
				new int[] {39,  104}, new int[] {46,   99}, new int[] {53,   95}, new int[] {61,   93}, 
				new int[] {68,   93}, new int[] {75,   96}, new int[] {82,  102}, new int[] {89,  111}, 
				new int[] {94,  121}, new int[] {100, 134}, new int[] {106, 150}, new int[] {111, 166}, 
				new int[] {116, 183}, new int[] {120, 199}, new int[] {124, 216}, new int[] {127, 238} };
		IIconSprite spriteBall=new IconSprite(0,0,@viewport);
		spriteBall.visible=false;
		float angle=0;
		decimal multiplier=1.0m;
		if (@battle.doublebattle) {
			multiplier=(battlerindex==0) ? 0.7m : 1.3m;
		}		
		//animate ball throw curve
		foreach (int[] coord in path) { //ToDo: replace with coroutine
			delay=pictureBall.totalDuration;
			pictureBall.moveAngle(0,delay,angle);
			pictureBall.moveXY(1,delay,coord[0]*multiplier,coord[1]);
			angle+=40;
			angle%=360;
		}
		pictureBall.adjustPosition(0,@traineryoffset);
		(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).visible=false;
		@briefmessage=false;
		//fadeanim=null;
		if (@showingplayer) {
			//fadeanim=new PlayerFadeAnimation(@sprites);
			fadeanim.initialize(@sprites);
		}
		int frame=0;
		(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(pkmn,true);
		if (illusionpoke != null) {
			(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(illusionpoke,true);
		}
		//sendout=new PokeballPlayerSendOutAnimation(@sprites[$"pokemon#{battlerindex}"],
		sendout.initialize(@sprites[$"pokemon#{battlerindex}"] as IPokemonBattlerSprite,
			@sprites,@battle.battlers[battlerindex],illusionpoke,@battle.doublebattle);
		do  //;loop
		{
			if (fadeanim != null) fadeanim.update();
			frame += 1;
			if (frame > 1 && !pictureBall.running && !(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).appearing) {
				(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).appear();
			}
			if (frame >= 3 && !pictureBall.running) {
				sendout.update();
			}
			if ((frame >= 10 || fadeanim == null) && pictureBall.running) {
				pictureBall.update();
				setPictureIconSprite(spriteBall, pictureBall);
			}
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if ((fadeanim == null || fadeanim.animdone) && sendout.animdone &&
				!(@sprites[$"battlebox#{battlerindex}"] as IPokemonDataBox).appearing) break;
		} while (true);
		spriteBall.dispose();
		sendout.dispose();
		if (@battle.battlers[battlerindex].IsShiny && @battle.battlescene) {
			pbCommonAnimation("Shiny",@battle.battlers[battlerindex],null);
		}
		if (@showingplayer) {
			@showingplayer=false;
			pbDisposeSprite(@sprites,"player");
			pbDisposeSprite(@sprites,"partybarplayer");
			for (int i = 0; i < 6; i++) {
				pbDisposeSprite(@sprites,$"player#{i}");
			}
		}
		pbRefresh();*/
	}

	/// <summary>
	/// Player sending out Pokémon
	/// </summary>
	/// <param name="battle"></param>
	/// <param name="pkmn"></param>
	public void pbSendOut(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//foreach trainer in battle
		//foreach(int i in battle.)
			pbSendOut(0, pkmn);

	}

	public void pbTrainerWithdraw(IBattle battle, IBattler pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbRefresh();
	}

	public void pbTrainerWithdraw(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbRefresh();
	}

	public void pbWithdraw(IBattle battle, IPokemon pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbRefresh();
	}

	public string pbMoveString(IBattleMove move)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		string ret = Game._INTL("{1}", move.Name);
		string typename = move.Type.ToString(TextScripts.Name);
		if (move.id > 0)
		{
			ret += Game._INTL(" ({1}) PP: {2}/{3}", typename, move.PP, move.TotalPP);
		}
		return ret;
	}

	public void pbBeginAttackPhase()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbSelectBattler(-1, 0); //pbSelectBattler(-1);
		pbGraphicsUpdate();
	}

	public void pbSafariStart()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@briefmessage = false;
		//@sprites["battlebox0"] = new SafariDataBox(@battle, @viewport);
		(@sprites["battlebox0"] as ISafariDataBox).initialize(@battle, @viewport);
		(@sprites["battlebox0"] as ISafariDataBox).appear();
		do //;loop
		{
			(@sprites["battlebox0"] as ISafariDataBox).update();
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (!(@sprites["battlebox0"] as ISafariDataBox).appearing) break;
		} while (true);
		pbRefresh();
	}

	public void pbResetCommandIndices()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
	}

	public void pbResetMoveIndex(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@lastmove[index] = 0;
	}

	public int pbSafariCommandMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		return pbCommandMenuEx(index,new string[] {
			Game._INTL("What will\n{1} throw?", @battle.pbPlayer().name),
			Game._INTL("Ball"),
			Game._INTL("Bait"),
			Game._INTL("Rock"),
			Game._INTL("Run")
		},2);
	}

	/// <summary>
	/// Use this method to display the list of commands.
	/// </summary>
	/// <param name="index"></param>
	/// <returns>Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
	public int pbCommandMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		bool shadowTrainer = (@battle.opponent != null); //hasConst(PBTypes,:SHADOW) && 
		int ret = pbCommandMenuEx(index,new string[] {
			Game._INTL("What will\n{1} do?", @battle.battlers[index].Name),
			Game._INTL("Fight"),
			Game._INTL("Bag"),
			Game._INTL("Pokémon"),
			shadowTrainer ? Game._INTL("Call") : Game._INTL("Run")
		},(shadowTrainer ? 1 : 0));
		if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
		return ret;
	}

	/// <summary>
	/// </summary>
	/// <param name="index"></param>
	/// <param name="texts"></param>
	/// <param name="mode">
	/// Mode: 0 - regular battle
	///       1 - Shadow Pokémon battle
	///       2 - Safari Zone
	///       3 - Bug Catching Contest
	/// </param>
	/// <returns></returns>
	public int pbCommandMenuEx(int index, string[] texts, int mode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		pbShowWindow(COMMANDBOX);
		ICommandMenuDisplay cw = @sprites["commandwindow"] as ICommandMenuDisplay;               
		cw.setTexts(texts);                         
		cw.index = (int)@lastcmd[index];
		cw.mode = mode;
		pbSelectBattler(index, mode); //pbSelectBattler(index);
		pbRefresh();
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			//  Update selected command
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
			{
				(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
				cw.index -= 1;
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
			{
				(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
				cw.index += 1;
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
			{
				(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
				cw.index -= 2;
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
			{
				(AudioHandler as IGameAudioPlay).pbPlayCursorSE();
				cw.index += 2;
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))	// Confirm choice
			{
				(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
				int ret = cw.index;
				@lastcmd[index] = (MenuCommands)ret;
				return ret;
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && index == 2 && @lastcmd[0] != (MenuCommands)2)	// Cancel
			{
				(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
				return -1;
			}
		} while (true);
	}

	/// <summary>
	/// Use this method to display the list of moves for a Pokémon
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public int pbFightMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pbShowWindow(FIGHTBOX);
		IFightMenuDisplay cw = @sprites["fightwindow"] as IFightMenuDisplay;
		IBattler battler = @battle.battlers[index];
		cw.battler = battler;
		int lastIndex = @lastmove[index];
		if (battler.moves[lastIndex].id != 0)
		{
			cw.setIndex(lastIndex);
		}
		else
		{
			cw.setIndex(0);
		}
		cw.megaButton = 0;
		if (@battle.pbCanMegaEvolve(index)) cw.megaButton = 1;
		pbSelectBattler(index, 0); //pbSelectBattler(index);
		pbRefresh();
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(cw);
			//  Update selected command
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
			{
				if (cw.setIndex(cw.index - 1)) (AudioHandler as IGameAudioPlay).pbPlayCursorSE();
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
			{
				if (cw.setIndex(cw.index + 1)) (AudioHandler as IGameAudioPlay).pbPlayCursorSE();
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
			{
				if (cw.setIndex(cw.index - 2)) (AudioHandler as IGameAudioPlay).pbPlayCursorSE();
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
			{
				if (cw.setIndex(cw.index + 2)) (AudioHandler as IGameAudioPlay).pbPlayCursorSE();
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))       // Confirm choice
			{
				int ret = cw.index;
				(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
				@lastmove[index] = ret;
				return ret;
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.CTRL))       // Use Mega Evolution
			{
				if (@battle.pbCanMegaEvolve(index))
				{
					@battle.pbRegisterMegaEvolution(index);
					cw.megaButton = 2;
					(AudioHandler as IGameAudioPlay).pbPlayDecisionSE();
				}
			}
			else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))       // Cancel fight menu
			{
				@lastmove[index] = cw.index;
				(AudioHandler as IGameAudioPlay).pbPlayCancelSE();
				return -1;
			}
		} while (true);
	}

	public Items pbItemMenu(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		Items ret = Items.NONE;
		/*int retindex = -1;
		int pkmnid = -1;
		bool endscene = true;
		oldsprites = pbFadeOutAndHide(@sprites);
		//IPokemonBagScene itemscene = new PokemonBag_Scene();
		IBagScene itemscene = Game.GameData.Scenes.Bag.initialize();
		itemscene.pbStartScene(Game.GameData.Bag);
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			Items item = itemscene.pbChooseItem();
			if (item == 0) break;
			int usetype =$ItemData[item][ITEMBATTLEUSE];
			int cmdUse = -1;
			IList<string> commands = new List<string>();
			if (usetype == 0)
			{
				//commands[commands.Length] = _INTL("Cancel");
				commands.Add(Game._INTL("Cancel"));
			}
			else
			{
				//commands[cmdUse = commands.Length] = _INTL("Use");
				//commands[commands.Length] = _INTL("Cancel");
				cmdUse = commands.Count;
				commands.Add(Game._INTL("Use"));
				commands.Add(Game._INTL("Cancel"));
			}
			string itemname = item.ToString(TextScripts.Name);
			int command = itemscene.pbShowCommands(Game._INTL("{1} is selected.", itemname), commands);
			if (cmdUse >= 0 && command == cmdUse)
			{
				if (usetype == 1 || usetype == 3)
				{
					IList<IPokemon> modparty = new List<IPokemon>();
					for (int i = 0; i < 6; i++)
					{
						modparty.Add(@battle.party1[@battle.party1order[i]]);
					}
					//pkmnlist = new PokemonScreen_Scene();
					//pkmnscreen = new PokemonScreen(pkmnlist, modparty);
					IPartyDisplayScene pkmnlist = Game.GameData.Scenes.Party.initialize();
					IPartyDisplayScreen pkmnscreen = Game.GameData.Screens.Party.initialize(pkmnlist, modparty);
					itemscene.pbEndScene();
					pkmnscreen.pbStartScene(Game._INTL("Use on which Pokémon?"), @battle.doublebattle);
					int activecmd = pkmnscreen.pbChoosePokemon();
					pkmnid = @battle.party1order[activecmd];
					if (activecmd >= 0 && pkmnid >= 0 && ItemHandlers.hasBattleUseOnPokemon(item))
					{
						pkmnlist.pbEndScene();
						ret = item;
						retindex = pkmnid;
						endscene = false;
						break;
					}
					pkmnlist.pbEndScene();
					itemscene.pbStartScene(Game.GameData.Bag);
				}
				else if (usetype == 2 || usetype == 4)
				{
					if (ItemHandlers.hasBattleUseOnBattler(item))
					{
						ret = item;
						retindex = index;
						break;
					}
				}
			}
		} while (true);

		if (ret > 0) pbConsumeItemInBattle(Game.GameData.Bag, ret);
		if (endscene) itemscene.pbEndScene();
		pbFadeInAndShow(@sprites, oldsprites);*/
		//return [ret, retindex];
		//return new KeyValuePair<Items,int> (ret, retindex);
		return ret;
	}

	public int pbForgetMove(IPokemon pokemon, Moves moveToLearn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		int ret = -1;
		if (Game.GameData is IGameSpriteWindow g) g.pbFadeOutIn(99999, block: () => {
			//IPokemonSummaryScene scene = new PokemonSummaryScene();
			//IPokemonSummaryScreen screen = new PokemonSummary(scene);
			IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
			IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
			ret = screen.pbStartForgetScreen(pokemon,0,moveToLearn);
		});
		return ret;
	}

	/// <summary>
	/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
	/// </summary>
	/// <param name="pokemon"></param>
	/// <param name="message"></param>
	/// <returns></returns>
	public int pbChooseMove(IPokemon pokemon, string message)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		int ret = -1;
		if (Game.GameData is IGameSpriteWindow g) g.pbFadeOutIn(99999, block: () => {
			//IPokemonSummaryScene scene = new PokemonSummaryScene();
			//IPokemonSummaryScreen screen = new PokemonSummary(scene);
			IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
			IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
			ret = screen.pbStartChooseMoveScreen(pokemon,0,message);
		});
		return ret;
	}

	public string pbNameEntry(string helptext, IPokemon pokemon)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//return pbEnterPokemonName(helptext, 0, 10, "", pokemon);
		return pokemon.Name;
	}

	public void pbSelectBattler(int index, int selectmode)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		int numwindows = @battle.doublebattle ? 4 : 2;
		for (int i = 0; i < numwindows; i++)
		{
			IPokemonDataBox sprite0 = @sprites[$"battlebox#{i}"] as IPokemonDataBox;
			sprite0.selected = (i == index) ? selectmode : 0;
			IPokemonBattlerSprite sprite1 = @sprites[$"pokemon#{i}"] as IPokemonBattlerSprite;
			sprite1.selected = (i == index) ? selectmode : 0;
		}
	}

	public int pbFirstTarget(int index, Targets targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		switch (targettype)
		{
			case Targets.SingleNonUser:
				//number of targets to select from is usually a max of 4 if double battle...
				for (int i = 0; i < 4; i++) 
				{
					if (i != index && !@battle.battlers[i].isFainted() &&
					   @battle.battlers[index].pbIsOpposing(i))
					{
						return i;
					}
				}
				break;
			case Targets.UserOrPartner:
				return index;
			default:
				break;
		}*/
		return -1;
	}

	public void pbFirstTarget(int index, int targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		throw new System.NotImplementedException();
	}

	public void pbUpdateSelected(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		int numwindows = @battle.doublebattle ? 4 : 2;
		for (int i = 0; i < numwindows; i++)
		{
			if (i == index)
			{
				(@sprites[$"battlebox#{i}"] as IPokemonDataBox).selected = 2;
				(@sprites[$"pokemon#{i}"] as IPokemonBattlerSprite).selected = 2;
			}
			else
			{
				(@sprites[$"battlebox#{i}"] as IPokemonDataBox).selected = 0;
				(@sprites[$"pokemon#{i}"] as IPokemonBattlerSprite).selected = 0;
			}
			(@sprites[$"battlebox#{i}"] as IPokemonDataBox).update();
			(@sprites[$"pokemon#{i}"] as IPokemonBattlerSprite).update();
		}
	}

	public int pbChooseTarget(int index, Targets targettype)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		
		pbShowWindow(FIGHTBOX);
		IFightMenuDisplay cw = @sprites["fightwindow"] as IFightMenuDisplay;
		IBattler battler = @battle.battlers[index];
		cw.battler = battler;
		int lastIndex = @lastmove[index];
		/*if (battler.moves[lastIndex].id != 0)
		{
			cw.setIndex(lastIndex);
		}
		else
		{
			cw.setIndex(0);
		}

		int curwindow = pbFirstTarget(index, targettype);
		int newcurwindow = -1;
		if (curwindow == -1)
		{
			//throw new RuntimeError(Game._INTL("No targets somehow..."));
			GameDebug.LogError(Game._INTL("No targets somehow..."));
		}
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			pbUpdateSelected(curwindow);
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
			{
				pbUpdateSelected(-1);
				return curwindow;
			}
			if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
			{
				pbUpdateSelected(-1);
				return -1;
			}
			if (curwindow >= 0)
			{
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) || PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN))
				{
					do //;loop
					{
						switch (targettype)
						{
							case Targets.SingleNonUser:
								switch (curwindow)
								{
									case 0:
										newcurwindow = 2;
										break;
									case 1:
										newcurwindow = 0;
										break;
									case 2:
										newcurwindow = 3;
										break;
									case 3:
										newcurwindow = 1;
										break;
								}
								break;
							case Targets.UserOrPartner:
								newcurwindow = (curwindow + 2) % 4;
								break;
						}
						curwindow = newcurwindow;
						if (targettype == Targets.SingleNonUser && curwindow == index) continue;
						if (!@battle.battlers[curwindow].isFainted()) break;
					} while (true);
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) || PokemonUnity.Input.trigger(PokemonUnity.Input.UP))
				{
					do //;loop
					{
						switch (targettype)
						{
							case Targets.SingleNonUser:
								switch (curwindow)
								{
									case 0:
										newcurwindow = 1;
										break;
									case 1:
										newcurwindow = 3;
										break;
									case 2:
										newcurwindow = 0;
										break;
									case 3:
										newcurwindow = 2;
										break;
								}
								break;
							case Targets.UserOrPartner:
								newcurwindow = (curwindow + 2) % 4;
								break;
						}
						curwindow = newcurwindow;
						if (targettype == Targets.SingleNonUser && curwindow == index) continue;
						if (!@battle.battlers[curwindow].isFainted()) break;
					} while (true);
				}
			}
		} while (true);*/
		return -1;
	}

	public int pbSwitch(int index, bool lax, bool cancancel)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		IPokemon[] party = @battle.pbParty(index);
		IList<int> partypos = @battle.party1order;
		int ret = -1;
		//  Fade out and hide all sprites
		/*visiblesprites = pbFadeOutAndHide(@sprites);
		pbShowWindow(BLANK);
		pbSetMessageMode(true);
		IList<IPokemon> modparty = new List<IPokemon>();
		for (int i = 0; i < 6; i++)
		{
			modparty.Add(party[partypos[i]]);
		}
		//IPartyDisplayScene scene = new PokemonScreen_Scene();
		//@switchscreen = new PokemonScreen(scene, modparty);
		IPartyDisplayScene scene = Game.GameData.Scenes.Party.initialize();
		@switchscreen = Game.GameData.Screens.Party.initialize(scene, modparty);
		@switchscreen.pbStartScene(Game._INTL("Choose a Pokémon."),
		   @battle.doublebattle && !@battle.fullparty1);
		//All of this below should be a coroutine that returns the value selected in UI
		do //;loop
		{
			scene.pbSetHelpText(Game._INTL("Choose a Pokémon."));
			int activecmd = @switchscreen.pbChoosePokemon();
			if (cancancel && activecmd == -1)
			{
				ret = -1;
				break;
			}
			if (activecmd >= 0)
			{
				IList<string> commands = new List<string>();
				int cmdShift = -1;
				int cmdSummary = -1;
				int pkmnindex = partypos[activecmd];
				if (!party[pkmnindex].isEgg) commands[cmdShift = commands.Count] = Game._INTL("Switch In");
				commands[cmdSummary = commands.Count] = Game._INTL("Summary");
				commands[commands.Count] = Game._INTL("Cancel");
				int command = scene.pbShowCommands(Game._INTL("Do what with {1}?", party[pkmnindex].Name), commands.ToArray());
				if (cmdShift >= 0 && command == cmdShift)
				{
					bool canswitch = lax ? @battle.pbCanSwitchLax(index, pkmnindex, true) :
					   @battle.pbCanSwitch(index, pkmnindex, true);
					if (canswitch)
					{
						ret = pkmnindex;
						break;
					}
				}
				else if (cmdSummary >= 0 && command == cmdSummary)
				{
					scene.pbSummary(activecmd);
				}
			}
		} while (true);
		@switchscreen.pbEndScene();
		@switchscreen = null;
		pbShowWindow(BLANK);
		pbSetMessageMode(false);
		//  back to main battle screen
		pbFadeInAndShow(@sprites, visiblesprites);*/
		return ret;
	}

	public void pbDamageAnimation(IBattler pkmn, TypeEffective effectiveness)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		IPokemonBattlerSprite pkmnsprite = @sprites[$"pokemon#{pkmn.Index}"] as IPokemonBattlerSprite;
		IIconSprite shadowsprite = @sprites[$"shadow#{pkmn.Index}"] as IIconSprite;
		IPokemonDataBox sprite = @sprites[$"battlebox#{pkmn.Index}"] as IPokemonDataBox;
		bool oldshadowvisible = shadowsprite.visible;
		bool oldvisible = sprite.visible;
		sprite.selected = 3;
		@briefmessage = false;
		//
		int i = 0; do //wait 6 frame ticks...
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
		} while (i < 6); //6.times 
		switch (effectiveness)
		{
			case TypeEffective.NormalEffective: //0:
				(AudioHandler as IGameAudioPlay).pbSEPlay("normaldamage");
				break;
			case TypeEffective.NotVeryEffective: //1:
				(AudioHandler as IGameAudioPlay).pbSEPlay("notverydamage");
				break;
			case TypeEffective.SuperEffective: //2:
				(AudioHandler as IGameAudioPlay).pbSEPlay("superdamage");
				break;
			//ToDo: Ineffective?
		}
		i = 0; do
		{
			pkmnsprite.visible = !pkmnsprite.visible;
			if (oldshadowvisible)
			{
				shadowsprite.visible = !shadowsprite.visible;
			}
			int j = 0; do
			{
				pbGraphicsUpdate();
				pbInputUpdate();
				pbFrameUpdate();
				sprite.update(); j++;
			} while (j < 4); i++;//4.times 
		} while (i < 8); //8.times 
		sprite.selected = 0;
		sprite.visible = oldvisible;
	}

	/// <summary>
	/// This method is called whenever a Pokémon's HP changes.
	/// Used to animate the HP bar.
	/// </summary>
	/// <param name="pkmn"></param>
	/// <param name="oldhp"></param>
	/// <param name="anim"></param>
	public void pbHPChanged(IBattler pkmn, int oldhp, bool anim)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		@briefmessage = false;
		int hpchange = pkmn.HP - oldhp;
		if (hpchange < 0)
		{
			hpchange = -hpchange;
			GameDebug.Log($"[HP change] #{pkmn.ToString()} lost #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
		}
		else
		{
			GameDebug.Log($"[HP change] #{pkmn.ToString()} gained #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
		}
		if (anim && @battle.battlescene)
		{
			if (pkmn.HP > oldhp)
			{
				pbCommonAnimation("HealthUp", pkmn, null);
			}
			else if (pkmn.HP < oldhp)
			{
				pbCommonAnimation("HealthDown", pkmn, null);
			}
		}
		IPokemonDataBox sprite = @sprites[$"battlebox#{pkmn.Index}"] as IPokemonDataBox;
		sprite.animateHP(oldhp, pkmn.HP);
		while (sprite.animatingHP)
		{
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			sprite.update();
		}
	}

	/// <summary>
	/// This method is called whenever a Pokémon faints.
	/// </summary>
	/// <param name="pkmn"></param>
	public void pbFainted(IBattler pkmn)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		int frames = pbCryFrameLength(pkmn.pokemon);
		pbPlayCry(pkmn.pokemon);
		int i = 0; do {
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate(); i++;
		} while (i < frames); //frames.times 
		@sprites[$"shadow#{pkmn.Index}"].visible=false;
		IPokemonBattlerSprite pkmnsprite = @sprites[$"pokemon#{pkmn.Index}"] as IPokemonBattlerSprite;
		int ycoord=0;
		if (@battle.doublebattle) {
			if (pkmn.Index==0) ycoord=PokeBattle_SceneConstants.PLAYERBATTLERD1_Y;
			if (pkmn.Index==1) ycoord=PokeBattle_SceneConstants.FOEBATTLERD1_Y;
			if (pkmn.Index==2) ycoord=PokeBattle_SceneConstants.PLAYERBATTLERD2_Y;
			if (pkmn.Index==3) ycoord=PokeBattle_SceneConstants.FOEBATTLERD2_Y;
		} else
		{
			if (@battle.pbIsOpposing(pkmn.Index))
			{
				ycoord = PokeBattle_SceneConstants.FOEBATTLER_Y;
			}
			else
			{
				ycoord = PokeBattle_SceneConstants.PLAYERBATTLER_Y;
			}
		}
		(AudioHandler as IGameAudioPlay).pbSEPlay("faint");
		do //;loop
		{
			pkmnsprite.y += 8;
			if (pkmnsprite.y - pkmnsprite.oy + pkmnsprite.src_rect.height >= ycoord)
			{
				pkmnsprite.src_rect.height = ycoord - pkmnsprite.y + pkmnsprite.oy;
			}
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (pkmnsprite.y >= ycoord) break;
		} while (true);

		pkmnsprite.visible = false;
		i = 0; do
		{
			(@sprites[$"battlebox#{pkmn.Index}"] as IPokemonDataBox).opacity -= 32;
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
		} while (i < 8); //8.times 
		(@sprites[$"battlebox#{pkmn.Index}"] as IPokemonDataBox).visible = false;
		pkmn.pbResetForm();*/
	}

	/// <summary>
	/// Use this method to choose a command for the enemy.
	/// </summary>
	/// <param name="index"></param>
	public void pbChooseEnemyCommand(int index)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if(@battle is IBattleAI b) b.pbDefaultChooseEnemyCommand(index);
	}

	/// <summary>
	/// Use this method to choose a new Pokémon for the enemy
	/// The enemy's party is guaranteed to have at least one choosable member.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="party"></param>
	/// <returns></returns>
	public int pbChooseNewEnemy(int index, IPokemon[] party)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (@battle is IBattleAI b) return b.pbDefaultChooseNewEnemy(index, party);
		else GameDebug.LogError("Need to Configure Battle AI to Select Next Pokemon for Computer...");
		return -1;
	}

	public void pbWildBattleSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//(AudioHandler as IGameAudioPlay).pbBGMPlay(pbGetWildVictoryME());
	}

	public void pbTrainerBattleSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//(AudioHandler as IGameAudioPlay).pbBGMPlay(pbGetTrainerVictoryME(@battle.opponent));
	}

	public void pbEXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		//if (battler != null)
		//{
		//	(@sprites[$"battlebox#{battler.Index}"] as IPokemonDataBox).refreshExpLevel();
		//	int exprange = (endexp - startexp);
		//	int startexplevel = 0;
		//	int endexplevel = 0;
		//	if (exprange != 0)
		//	{
		//		startexplevel = (tempexp1 - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / exprange;
		//		endexplevel = (tempexp2 - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / exprange;
		//	}
		//	(@sprites[$"battlebox#{battler.Index}"] as IPokemonDataBox).animateEXP(startexplevel, endexplevel);
		//	while ((@sprites[$"battlebox#{battler.Index}"] as IPokemonDataBox).animatingEXP)
		//	{
		//		pbGraphicsUpdate();
		//		pbInputUpdate();
		//		pbFrameUpdate();
		//		(@sprites[$"battlebox#{battler.Index}"] as IPokemonDataBox).update();
		//	}
		//}
	}

	public void pbShowPokedex(Pokemons species, int form)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (Game.GameData is IGameSpriteWindow g) g.pbFadeOutIn(99999, block: () => {
			//IPokemonPokedexScene scene = new PokemonPokedexScene();
			//IPokemonPokedexScreen screen = new PokemonPokedex(scene);
			IPokemonPokedexScene scene = Game.GameData.Scenes.PokedexScene.initialize();
			IPokemonPokedexScreen screen = Game.GameData.Screens.PokedexScreen.initialize(scene);
			screen.pbDexEntry(species);
		});
	}

	/*public void pbChangeSpecies(IBattler attacker, Pokemons species)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		pkmn = @sprites[$"pokemon#{attacker.Index}"];
		shadow = @sprites[$"shadow#{attacker.Index}"];
		back = !@battle.pbIsOpposing(attacker.Index);
		pkmn.setPokemonBitmapSpecies(attacker.pokemon, species, back);
		pkmn.x = -pkmn.bitmap.width / 2;
		pkmn.y = adjustBattleSpriteY(pkmn, species, attacker.Index);
		if (@battle.doublebattle)
		{
			switch (attacker.Index)
			{
				case 0:
					pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLERD1_X;
					pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLERD1_Y;
					break;
				case 1:
					pkmn.x += PokeBattle_SceneConstants.FOEBATTLERD1_X;
					pkmn.y += PokeBattle_SceneConstants.FOEBATTLERD1_Y;
					break;
				case 2:
					pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLERD2_X;
					pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLERD2_Y;
					break;
				case 3:
					pkmn.x += PokeBattle_SceneConstants.FOEBATTLERD2_X;
					pkmn.y += PokeBattle_SceneConstants.FOEBATTLERD2_Y;
					break;
			}
		}
		else
		{
			if (attacker.Index == 0) pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLER_X;
			if (attacker.Index == 0) pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLER_Y;
			if (attacker.Index == 1) pkmn.x += PokeBattle_SceneConstants.FOEBATTLER_X;
			if (attacker.Index == 1) pkmn.y += PokeBattle_SceneConstants.FOEBATTLER_Y;
		}
		if (shadow && !back)
		{
			shadow.visible = showShadow(species);
		}
	}*/

	public void pbChangePokemon(IBattler attacker, Forms pokemon) //ToDo: change Forms to IPokemon? All references points back to using IPokemon data
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		IPokemonBattlerSprite pkmn = @sprites[$"pokemon#{attacker.Index}"] as IPokemonBattlerSprite;
		IIconSprite shadow = @sprites[$"shadow#{attacker.Index}"] as IIconSprite;
		bool back = !@battle.pbIsOpposing(attacker.Index);
		pkmn.setPokemonBitmap(pokemon, back); //set pokemon battle sprite to current form 
		pkmn.x = -pkmn.bitmap.width / 2;
		pkmn.y = adjustBattleSpriteY(pkmn, pokemon.Species, attacker.Index); //grab sprite information for new form
		if (@battle.doublebattle)
		{
			switch (attacker.Index)
			{
				case 0:
					pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLERD1_X;
					pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLERD1_Y;
					break;
				case 1:
					pkmn.x += PokeBattle_SceneConstants.FOEBATTLERD1_X;
					pkmn.y += PokeBattle_SceneConstants.FOEBATTLERD1_Y;
					break;
				case 2:
					pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLERD2_X;
					pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLERD2_Y;
					break;
				case 3:
					pkmn.x += PokeBattle_SceneConstants.FOEBATTLERD2_X;
					pkmn.y += PokeBattle_SceneConstants.FOEBATTLERD2_Y;
					break;
			}
		}
		else
		{
			if (attacker.Index == 0) pkmn.x += PokeBattle_SceneConstants.PLAYERBATTLER_X;
			if (attacker.Index == 0) pkmn.y += PokeBattle_SceneConstants.PLAYERBATTLER_Y;
			if (attacker.Index == 1) pkmn.x += PokeBattle_SceneConstants.FOEBATTLER_X;
			if (attacker.Index == 1) pkmn.y += PokeBattle_SceneConstants.FOEBATTLER_Y;
		}
		if (shadow != null && !back)
		{
			shadow.visible = showShadow(pokemon.Species);
		}*/
	}

	public void ChangePokemon()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
	}

	//public void pbSaveShadows()
	public void pbSaveShadows(Action action = null)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		IList<bool> shadows = new List<bool>();
		IIconSprite s = null;
		for (int i = 0; i < 4; i++)
		{
			s = @sprites[$"shadow#{i}"] as IIconSprite;
			shadows[i] = s != null ? s.visible : false;
			if (s != null) s.visible = false;
		}
		if (action != null) action.Invoke(); //yield return null;
		for (int i = 0; i < 4; i++)
		{
			s = @sprites[$"shadow#{i}"] as IIconSprite;
			if (s != null) s.visible = shadows[i];
		}
	}

	public void pbFindAnimation(Moves moveid, int userIndex, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		try //begin;
		{
			move2anim = load_data("Data/move2anim.dat");
			bool noflip = false;
			if ((userIndex & 1) == 0)       // On player's side
			{
				anim = move2anim[0][moveid];
			}
			else                  // On opposing side
			{
				anim = move2anim[1][moveid];
				if (anim != null) noflip = true;
				if (anim == null) anim = move2anim[0][moveid];
			}
			if (anim != null) return [anim + hitnum, noflip];
			//  Actual animation not found, get the default animation for the move's type
			//IMove move = new PBMoveData(moveid);
			PokemonUnity.Attack.Data.MoveData move = Kernal.MoveData[moveid];
			Types type = move.Type;
			KeyValuePair<Types, Moves>[] typedefaultanim = new KeyValuePair<Types, Moves>[] {
						new KeyValuePair<Types, Moves> (Types.NORMAL,	Moves.TACKLE),
						new KeyValuePair<Types, Moves> (Types.FIGHTING,	Moves.COMET_PUNCH),
						new KeyValuePair<Types, Moves> (Types.FLYING,	Moves.GUST),
						new KeyValuePair<Types, Moves> (Types.POISON,	Moves.SLUDGE),
						new KeyValuePair<Types, Moves> (Types.GROUND,	Moves.MUD_SLAP),
						new KeyValuePair<Types, Moves> (Types.ROCK,		Moves.ROCK_THROW),
						new KeyValuePair<Types, Moves> (Types.BUG,		Moves.TWINEEDLE),
						new KeyValuePair<Types, Moves> (Types.GHOST,	Moves.NIGHT_SHADE),
						new KeyValuePair<Types, Moves> (Types.STEEL,	Moves.GYRO_BALL),
						new KeyValuePair<Types, Moves> (Types.FIRE,		Moves.EMBER),
						new KeyValuePair<Types, Moves> (Types.WATER,	Moves.WATER_GUN),
						new KeyValuePair<Types, Moves> (Types.GRASS,	Moves.RAZOR_LEAF),
						new KeyValuePair<Types, Moves> (Types.ELECTRIC,	Moves.THUNDER_SHOCK),
						new KeyValuePair<Types, Moves> (Types.PSYCHIC,	Moves.CONFUSION),
						new KeyValuePair<Types, Moves> (Types.ICE,		Moves.ICE_BALL),
						new KeyValuePair<Types, Moves> (Types.DRAGON,	Moves.DRAGON_RAGE),
						new KeyValuePair<Types, Moves> (Types.DARK,		Moves.PURSUIT),
						new KeyValuePair<Types, Moves> (Types.FAIRY,	Moves.FAIRY_WIND)
			};
			foreach (KeyValuePair<Types, Moves> i in typedefaultanim)
			{
				//if (isConst(type, PBTypes, i[0]) && hasConst(PBMoves, i[1]))
				//{
					noflip = false;
					if ((userIndex & 1) == 0)       // On player's side
					{
						//anim = move2anim[0][getConst(PBMoves, i[1])];
						anim = move2anim[0][i.Value];
					}
					else                  // On opposing side
					{
						//anim = move2anim[1][getConst(PBMoves, i[1])];
						anim = move2anim[1][i.Value];
						if (anim) noflip = true;
						//if (!anim) anim = move2anim[0][getConst(PBMoves, i[1])];
						if (!anim) anim = move2anim[0][i.Value];
					}
					if (anim != null) return new KeyValuePair<string,bool>(anim, noflip);
					break;
				//}
			}
			//  Default animation for the move's type not found, use Tackle's animation
			//if (hasConst(PBMoves,:TACKLE))
			//{
				anim = move2anim[0][Moves.TACKLE];
				if (anim != null) return new KeyValuePair<string,bool>(anim, false);
			//}
		}
		catch (Exception) //rescue;
		{
			return null;
		}
		return null;*/
	}

	public void pbCommonAnimation(string animname, IBattler user, IBattler target, int hitnum = 0)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		animations = load_data("Data/PkmnAnimations.rxdata");
		for (int i = 0; i < animations.Length; i++)
		{
			if (animations[i] && animations[i].name == "Common:" + animname)
			{
				pbAnimationCore(animations[i], user, (target != null) ? target : user);
				return;
			}
		}*/
	}

	public void pbCommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		/*animations = load_data("Data/PkmnAnimations.rxdata");
		for (int i = 0; i < animations.Length; i++)
		{
			if (animations[i] && animations[i].name == "Common:" + animname)
			{
				pbAnimationCore(animations[i], user, (target != null) ? target : user);
				return;
			}
		}*/
	}

	public void pbAnimation(Moves moveid, IBattler user, IBattler target, int hitnum)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		int animid = pbFindAnimation(moveid, user.Index, hitnum);
		if (!animid) return;
		anim = animid[0];
		animations = load_data("Data/PkmnAnimations.rxdata");
		pbSaveShadows(() =>
		{
			if (animid[1])       // On opposing side and using OppMove animation
			{
				pbAnimationCore(animations[anim], target, user, true);
			}
			else         // On player's side, and/or using Move animation
			{
				pbAnimationCore(animations[anim], user, target);
			}
		});
		if (new PBMoveData(moveid).function == 0x69 && user != null && target != null)      // Transform
		{ 
			//  Change form to transformed version
			pbChangePokemon(user, target.pokemon);
		}*/
	}

	public void pbAnimationCore(string animation, IBattler user, IBattler target, bool oppmove)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		if (!animation) return;
		@briefmessage = false;
		usersprite = (user) ? @sprites[$"pokemon#{user.Index}"] : null;
		targetsprite = (target) ? @sprites[$"pokemon#{target.Index}"] : null;
		olduserx = usersprite ? usersprite.x : 0;
		oldusery = usersprite ? usersprite.y : 0;
		oldtargetx = targetsprite ? targetsprite.x : 0;
		oldtargety = targetsprite ? targetsprite.y : 0;
		if (!targetsprite)
		{
			if (!target) target = user;
			animplayer = new PBAnimationPlayerX(animation, user, target, self, oppmove);
			userwidth = (!usersprite || !usersprite.bitmap || usersprite.bitmap.disposed ?) ? 128 : usersprite.bitmap.width;
			userheight = (!usersprite || !usersprite.bitmap || usersprite.bitmap.disposed ?) ? 128 : usersprite.bitmap.height;
			animplayer.setLineTransform(
			   PokeBattle_SceneConstants.sOCUSUSER_X, PokeBattle_SceneConstants.sOCUSUSER_Y,
			   PokeBattle_SceneConstants.sOCUSTARGET_X, PokeBattle_SceneConstants.sOCUSTARGET_Y,
			   olduserx + (userwidth / 2), oldusery + (userheight / 2),
			   olduserx + (userwidth / 2), oldusery + (userheight / 2));
		}
		else
		{
			animplayer = new PBAnimationPlayerX(animation, user, target, self, oppmove);
			userwidth = (!usersprite || !usersprite.bitmap || usersprite.bitmap.disposed ?) ? 128 : usersprite.bitmap.width;
			userheight = (!usersprite || !usersprite.bitmap || usersprite.bitmap.disposed ?) ? 128 : usersprite.bitmap.height;
			targetwidth = (!targetsprite.bitmap || targetsprite.bitmap.disposed ?) ? 128 : targetsprite.bitmap.width;
			targetheight = (!targetsprite.bitmap || targetsprite.bitmap.disposed ?) ? 128 : targetsprite.bitmap.height;
			animplayer.setLineTransform(
			   PokeBattle_SceneConstants.sOCUSUSER_X, PokeBattle_SceneConstants.sOCUSUSER_Y,
			   PokeBattle_SceneConstants.sOCUSTARGET_X, PokeBattle_SceneConstants.sOCUSTARGET_Y,
			   olduserx + (userwidth / 2), oldusery + (userheight / 2),
			   oldtargetx + (targetwidth / 2), oldtargety + (targetheight / 2));
		}
		animplayer.start();
		while (animplayer.playing)
		{
			animplayer.update();
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
		}
		if (usersprite) usersprite.ox = 0;
		if (usersprite) usersprite.oy = 0;
		if (usersprite) usersprite.x = olduserx;
		if (usersprite) usersprite.y = oldusery;
		if (targetsprite) targetsprite.ox = 0;
		if (targetsprite) targetsprite.oy = 0;
		if (targetsprite) targetsprite.x = oldtargetx;
		if (targetsprite) targetsprite.y = oldtargety;
		animplayer.dispose();*/
	}

	public void pbLevelUp(IPokemon pokemon, IBattler battler, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		pbTopRightWindow(Game._INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
		   pokemon.TotalHP - oldtotalhp,
		   pokemon.ATK - oldattack,
		   pokemon.DEF - olddefense,
		   pokemon.SPA - oldspatk,
		   pokemon.SPD - oldspdef,
		   pokemon.SPE - oldspeed));
		pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
		   pokemon.TotalHP, pokemon.ATK, pokemon.DEF, pokemon.SPA, pokemon.SPD, pokemon.SPE));*/
	}

	public void pbLevelUp(IBattler battler, IPokemon thispoke, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		pbTopRightWindow(Game._INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
		   thispoke.TotalHP - oldtotalhp,
		   thispoke.ATK - oldattack,
		   thispoke.DEF - olddefense,
		   thispoke.SPA - oldspatk,
		   thispoke.SPD - oldspdef,
		   thispoke.SPE - oldspeed));
		pbTopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
		   thispoke.TotalHP, thispoke.ATK, thispoke.DEF, thispoke.SPA, thispoke.SPD, thispoke.SPE));*/
	}

	public void pbThrowAndDeflect(Items ball, int targetBattler)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@briefmessage = false;
		balltype = pbGetBallType(ball);
		ball = string.Format("Graphics/Pictures/ball%02d", balltype);
		//  sprite
		IIconSprite spriteBall = new IconSprite(0, 0, @viewport);
		spriteBall.visible = false;
		//  picture
		pictureBall = new PictureEx((@sprites[$"pokemon#{targetBattler}"] as IPokemonBattlerSprite).z + 1);
		IPoint center = getSpriteCenter(@sprites[$"pokemon#{targetBattler}"]);
		//  starting positions
		pictureBall.moveVisible(1, true);
		pictureBall.moveName(1, ball);
		pictureBall.moveOrigin(1, PictureOrigin.nenter);
		pictureBall.moveXY(0, 1, 10, 180);
		//  directives
		pictureBall.moveSE(1, "Audio/SE/throw");
		pictureBall.moveCurve(30, 1, 150, 70, 30 + (Game.GameData as Game).Graphics.width / 2, 10, center[0], center[1]);
		pictureBall.moveAngle(30, 1, -1080);
		pictureBall.moveAngle(0, pictureBall.totalDuration, 0);
		delay = pictureBall.totalDuration;
		pictureBall.moveSE(delay, "Audio/SE/balldrop");
		pictureBall.moveXY(20, delay, 0, (Game.GameData as Game).Graphics.height);
		do //;loop
		{
			pictureBall.update();
			setPictureIconSprite(spriteBall, pictureBall);
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (!pictureBall.running) break;
		} while (true);

		spriteBall.dispose();*/
	}

	public void pbThrow(Items ball, int shakes, bool critical, int targetBattler, bool showplayer)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@briefmessage = false;
		int burst = -1;
		animations = load_data("Data/PkmnAnimations.rxdata");
		for (int i = 0; i < 2; i++)
		{
			int t = (i == 0) ? (int)ball : 0;
			for (int j = 0; j < animations.Length; j++)
			{
				if (animations[j])
				{
					if (animations[j].name == $"Common:BallBurst#{t}")
					{
						if (burst < 0) burst = t;
						break;
					}
				}
			}
			if (burst >= 0) break;
		}
		pokeballThrow(ball, shakes, critical, targetBattler, this, @battle.battlers[targetBattler], burst, showplayer);*/
	}

	public void pbThrowSuccess()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		
		//if (@battle.opponent == null) {
		//	@briefmessage=false;
		//	(AudioHandler as IGameAudioPlay).pbMEPlay("Jingle - HMTM");
		//	//All of this below should be a coroutine that returns the value selected in UI
		//	int frames=(int)(3.5*(Game.GameData as Game).Graphics.frame_rate);
		//	int i = 0; do {
		//		pbGraphicsUpdate();
		//		pbInputUpdate();
		//		pbFrameUpdate(); i++;
		//	} while (i < frames); //frames.times 
		//}
	}

	public void pbHideCaptureBall()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (@sprites["capture"] != null)
		{
			do //;loop
			{
				if (@sprites["capture"].opacity <= 0) break;
				@sprites["capture"].opacity -= 12;
				pbGraphicsUpdate();
				pbInputUpdate();
				pbFrameUpdate();
			} while (true);
		}
	}

	public void pbThrowBait()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@briefmessage = false;
		string ball = string.Format("Graphics/Pictures/battleBait");
		bool armanim = false;
		if (@sprites["player"].bitmap.width > @sprites["player"].bitmap.height)
		{
			armanim = true;
		}
		//  sprites
		spritePoke = @sprites["pokemon1"];
		spritePlayer = @sprites["player"];
		IIconSprite spriteBall = new IconSprite(0, 0, @viewport);
		spriteBall.visible = false;
		//  pictures
		pictureBall = new PictureEx(spritePoke.z + 1);
		picturePoke = new PictureEx(spritePoke.z);
		picturePlayer = new PictureEx(spritePoke.z + 2);
		IPoint dims =[spritePoke.x, spritePoke.y];
		IPoint pokecenter = getSpriteCenter(@sprites["pokemon1"]);
		IPoint playerpos = [@sprites["player"].x, @sprites["player"].y];
		float ballendy = PokeBattle_SceneConstants.FOEBATTLER_Y - 4;
		//  starting positions
		pictureBall.moveVisible(1, true);
		pictureBall.moveName(1, ball);
		pictureBall.moveOrigin(1, PictureOrigin.center);
		pictureBall.moveXY(0, 1, 64, 256);
		picturePoke.moveVisible(1, true);
		picturePoke.moveOrigin(1, PictureOrigin.center);
		picturePoke.moveXY(0, 1, pokecenter[0], pokecenter[1]);
		picturePlayer.moveVisible(1, true);
		picturePlayer.moveName(1, @sprites["player"].name);
		picturePlayer.moveOrigin(1, PictureOrigin.TopLeft);
		picturePlayer.moveXY(0, 1, playerpos[0], playerpos[1]);
		//  directives
		picturePoke.moveSE(1, "Audio/SE/throw");
		pictureBall.moveCurve(30, 1, 64, 256, (Game.GameData as Game).Graphics.width / 2, 48,
							  PokeBattle_SceneConstants.FOEBATTLER_X - 48,
							  PokeBattle_SceneConstants.FOEBATTLER_Y);
		pictureBall.moveAngle(30, 1, -720);
		pictureBall.moveAngle(0, pictureBall.totalDuration, 0);
		if (armanim)
		{
			picturePlayer.moveSrc(1, @sprites["player"].bitmap.height, 0);
			picturePlayer.moveXY(0, 1, playerpos[0] - 14, playerpos[1]);
			picturePlayer.moveSrc(4, @sprites["player"].bitmap.height * 2, 0);
			picturePlayer.moveXY(0, 4, playerpos[0] - 12, playerpos[1]);
			picturePlayer.moveSrc(8, @sprites["player"].bitmap.height * 3, 0);
			picturePlayer.moveXY(0, 8, playerpos[0] + 20, playerpos[1]);
			picturePlayer.moveSrc(16, @sprites["player"].bitmap.height * 4, 0);
			picturePlayer.moveXY(0, 16, playerpos[0] + 16, playerpos[1]);
			picturePlayer.moveSrc(40, 0, 0);
			picturePlayer.moveXY(0, 40, playerpos[0], playerpos[1]);
		}
		//  Show Pokémon jumping before eating the bait
		picturePoke.moveSE(50, "Audio/SE/jump");
		picturePoke.moveXY(8, 50, pokecenter[0], pokecenter[1] - 8);
		picturePoke.moveXY(8, 58, pokecenter[0], pokecenter[1]);
		pictureBall.moveVisible(66, false);
		picturePoke.moveSE(66, "Audio/SE/jump");
		picturePoke.moveXY(8, 66, pokecenter[0], pokecenter[1] - 8);
		picturePoke.moveXY(8, 74, pokecenter[0], pokecenter[1]);
		//  TODO: Show Pokémon eating the bait (pivots at the bottom right corner)
		picturePoke.moveOrigin(picturePoke.totalDuration, PictureOrigin.TopLeft);
		picturePoke.moveXY(0, picturePoke.totalDuration, dims[0], dims[1]);
		do //;loop
		{
			pictureBall.update();
			picturePoke.update();
			picturePlayer.update();
			setPictureIconSprite(spriteBall, pictureBall);
			setPictureSprite(spritePoke, picturePoke);
			setPictureIconSprite(spritePlayer, picturePlayer);
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (!pictureBall.running && !picturePoke.running && !picturePlayer.running) break;
		} while (true);

		spriteBall.dispose();*/
	}

	public void pbThrowRock()
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		/*
		@briefmessage=false;
		ball=string.Format("Graphics/Pictures/battleRock");
		anger=string.Format("Graphics/Pictures/battleAnger");
		armanim=false;
		if (@sprites["player"].bitmap.width>@sprites["player"].bitmap.height) {
			armanim=true;
		}
		//  sprites
		spritePoke=@sprites["pokemon1"];
		spritePlayer=@sprites["player"];
		spriteBall=new IconSprite(0,0,@viewport);
		spriteBall.visible=false;
		spriteAnger=new IconSprite(0,0,@viewport);
		spriteAnger.visible=false;
		//  pictures
		pictureBall=new PictureEx(spritePoke.z+1);
		picturePoke=new PictureEx(spritePoke.z);
		picturePlayer=new PictureEx(spritePoke.z+2);
		pictureAnger=new PictureEx(spritePoke.z+1);
		dims=[spritePoke.x,spritePoke.y];
		pokecenter=getSpriteCenter(@sprites["pokemon1"]);
		playerpos=[@sprites["player"].x,@sprites["player"].y];
		ballendy=PokeBattle_SceneConstants.FOEBATTLER_Y-4;
		//  starting positions
		pictureBall.moveVisible(1,true);
		pictureBall.moveName(1,ball);
		pictureBall.moveOrigin(1,PictureOrigin.nenter);
		pictureBall.moveXY(0,1,64,256);
		picturePoke.moveVisible(1,true);
		picturePoke.moveOrigin(1,PictureOrigin.nenter);
		picturePoke.moveXY(0,1,pokecenter[0],pokecenter[1]);
		picturePlayer.moveVisible(1,true);
		picturePlayer.moveName(1,@sprites["player"].name);
		picturePlayer.moveOrigin(1,PictureOrigin.nopLeft);
		picturePlayer.moveXY(0,1,playerpos[0],playerpos[1]);
		pictureAnger.moveVisible(1,false);
		pictureAnger.moveName(1,anger);
		pictureAnger.moveXY(0,1,pokecenter[0]-56,pokecenter[1]-48);
		pictureAnger.moveOrigin(1,PictureOrigin.nenter);
		pictureAnger.moveZoom(0,1,100);
		//  directives
		picturePoke.moveSE(1,"Audio/SE/throw");
		pictureBall.moveCurve(30,1,64,256,(Game.GameData as Game).Graphics.width/2,48,pokecenter[0],pokecenter[1]);
		pictureBall.moveAngle(30,1,-720);
		pictureBall.moveAngle(0,pictureBall.totalDuration,0);
		pictureBall.moveSE(30,"Audio/SE/notverydamage");
		if (armanim) {
			picturePlayer.moveSrc(1,@sprites["player"].bitmap.height,0);
			picturePlayer.moveXY(0,1,playerpos[0]-14,playerpos[1]);
			picturePlayer.moveSrc(4,@sprites["player"].bitmap.height*2,0);
			picturePlayer.moveXY(0,4,playerpos[0]-12,playerpos[1]);
			picturePlayer.moveSrc(8,@sprites["player"].bitmap.height*3,0);
			picturePlayer.moveXY(0,8,playerpos[0]+20,playerpos[1]);
			picturePlayer.moveSrc(16,@sprites["player"].bitmap.height*4,0);
			picturePlayer.moveXY(0,16,playerpos[0]+16,playerpos[1]);
			picturePlayer.moveSrc(40,0,0);
			picturePlayer.moveXY(0,40,playerpos[0],playerpos[1]);
		}
		pictureBall.moveVisible(40,false);
		//  Show Pokémon being angry
		pictureAnger.moveSE(48,"Audio/SE/jump");
		pictureAnger.moveVisible(48,true);
		pictureAnger.moveZoom(8,48,130);
		pictureAnger.moveZoom(8,56,100);
		pictureAnger.moveXY(0,64,pokecenter[0]+56,pokecenter[1]-64);
		pictureAnger.moveSE(64,"Audio/SE/jump");
		pictureAnger.moveZoom(8,64,130);
		pictureAnger.moveZoom(8,72,100);
		pictureAnger.moveVisible(80,false);
		picturePoke.moveOrigin(picturePoke.totalDuration,PictureOrigin.nopLeft);
		picturePoke.moveXY(0,picturePoke.totalDuration,dims[0],dims[1]);
		do  //;loop
		{
			pictureBall.update();
			picturePoke.update();
			picturePlayer.update();
			pictureAnger.update();
			setPictureIconSprite(spriteBall,pictureBall);
			setPictureSprite(spritePoke,picturePoke);
			setPictureIconSprite(spritePlayer,picturePlayer);
			setPictureIconSprite(spriteAnger,pictureAnger);
			pbGraphicsUpdate();
			pbInputUpdate();
			pbFrameUpdate();
			if (!pictureBall.running && !picturePoke.running &&
					!picturePlayer.running && !pictureAnger.running) break;
		} while (true);
		spriteBall.dispose();*/
	}

	public void pbChatter(IBattler attacker, IBattler opponent)
	{
		GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

		if (attacker.pokemon != null)
		{
			//Plays audio sound...
			if (Game.GameData is IGameUtility u) u.pbPlayCry(attacker.pokemon, 90, 100);
		}
		//All of this below should be a coroutine that returns the value selected in UI
		//int i = 0; do //Idle animation for 1 second...
		//{
		//	(Game.GameData as Game).Graphics.update();
		//	PokemonUnity.Input.update(); i++;
		//} while (i < (Game.GameData as Game).Graphics.frame_rate); //;(Game.GameData as Game).Graphics.frame_rate.times 
	}

	IPokeBattle_DebugSceneNoGraphics IPokeBattle_DebugSceneNoGraphics.initialize()
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

	int IPokeBattle_DebugSceneNoGraphics.pbBlitz(int keys)
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