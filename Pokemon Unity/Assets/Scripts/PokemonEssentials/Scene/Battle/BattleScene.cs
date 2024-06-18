using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Attack.Data;
using PokemonUnity.Combat;
using PokemonUnity.Inventory;
using PokemonUnity.Interface;
using PokemonUnity.Interface.UnityEngine;
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
using UnityEngine;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Serialization;

namespace PokemonUnity.Interface.UnityEngine
{
	public partial class BattleScene : global::UnityEngine.MonoBehaviour, IPokeBattle_SceneIE//, IScene
	{
		#region Variable Property
		const int BLANK = 0;
		const int MESSAGEBOX = 1;
		const int COMMANDBOX = 2;
		const int FIGHTBOX = 3;
		//const string UI_MESSAGEBOX = "messagebox";
		//const string UI_MESSAGEWINDOW = "messagewindow";
		//const string UI_COMMANDWINDOW = "commandwindow";
		//const string UI_FIGHTWINDOW = "fightwindow";

		/// <summary>
		/// Scene Id; Match against unity's scene loader management, and use this value as input parameter
		/// </summary>
		public int								Id { get { return (int)Scenes.Battle; } }
		//public IGameAudioPlay					AudioHandler {  get { return AudioManager.AudioHandler; } }
		public IAudio							AudioHandler {  get { return AudioManager.AudioHandler; } }
		public bool								inPartyAnimation { get { return @enablePartyAnim && @partyAnimPhase < 3; } }
		public MenuCommands[]					lastcmd	{ get { return _lastcmd; } set { _lastcmd = value; } }
		//public IList<int>						lastcmd	{ get { return _lastcmd; } set { _lastcmd = value; } }
		public int[]							lastmove	{ get { return _lastmove; } set { _lastmove = value; } }
		//public PokemonEssentials.Interface.PokeBattle.IBattle			battle	{ get { return _battle; } set { _battle = value; } }
		//public PokemonUnity.Combat.Battle			battle	{ get { return _battle; } set { _battle = value; } }
		public PokemonUnity.Interface.UnityEngine.IBattleIE			battle	{ get { return _battle; } set { _battle = value; } }
		public ITrainerFadeAnimation				fadeanim		{ get { return _fadeanim; } set { _fadeanim = value; } }
		public IPokeballSendOutAnimation			sendout			{ get { return _sendout; } set { _sendout = value; } }
		public IWindow_CommandPokemon				commandpokemon	{ get { return _commandPokemon; } set { _commandPokemon = value as CommandWindowText; } }
		/// <summary>
		/// Select turn action; Fight, Bag, Item, Run...
		/// </summary>
		public ICommandMenuDisplay					commandwindow	{ get { return _commandWindow; } set { _commandWindow = value as CommandMenuDisplay; } }
		/// <summary>
		/// Select Pokemon's attack move for given turn
		/// </summary>
		public IFightMenuDisplay					fightwindow		{ get { return _fightWindow; } set { _fightWindow = value as FightMenuDisplay; } }
		public IWindow_UnformattedTextPokemon		helpwindow		{ get { return _helpWindow; } set { _helpWindow = value as WindowText; } }
		/// <summary>
		/// Display text and system responses to player
		/// </summary>
		public IWindow_AdvancedTextPokemon			messagewindow	{ get { return _messageWindow; } set { _messageWindow = value as WindowTextAdvance; } }
		/// <summary>
		/// The container canvas frame with custom UI border
		/// </summary>
		public IWindow_AdvancedTextPokemon			messagebox		{ get { return _messageBox; } set { _messageBox = value as WindowTextAdvance; } }
		public IPartyDisplayScreen					@switchscreen	{ get { return _switchscreen; } set { _switchscreen = value; } }
		public IPokemonBattlerSprite				player			{ get { return _player; } set { _player = value; } }
		public IPokemonBattlerSprite				playerB			{ get { return _playerB; } set { _playerB = value; } }
		public IPokemonBattlerSprite				trainer			{ get { return _trainer; } set { _trainer = value; } }
		public IPokemonBattlerSprite				trainer2		{ get { return _trainer2; } set { _trainer2 = value; } }
		public ISpriteWrapper						battlebg		{ get { return _battlebg; } set { _battlebg = value as SpriteWrapper; } }
		public ISpriteWrapper						enemybase		{ get { return _enemybase; } set { _enemybase = value as SpriteWrapper; } }
		public ISpriteWrapper						playerbase		{ get { return _playerbase; } set { _playerbase = value as SpriteWrapper; } }
		public ISpriteWrapper						partybarfoe		{ get { return _partybarfoe; } set { _partybarfoe = value as SpriteWrapper; } }
		public ISpriteWrapper						partybarplayer	{ get { return _partybarplayer; } set { _partybarplayer = value as SpriteWrapper; } }
		/// <summary>
		/// HP, Exp, and other status of battling pokemon; Player side slot 1
		/// </summary>
		public IPokemonDataBox						battlebox0	{ get { return _battlebox0; } set { _battlebox0 = value as PokemonDataBox; } }
		/// <summary>
		/// HP, Exp, and other status of battling pokemon; Enemy side slot 1
		/// </summary>
		public IPokemonDataBox						battlebox1	{ get { return _battlebox1; } set { _battlebox1 = value as PokemonDataBox; } }
		/// <summary>
		/// HP, Exp, and other status of battling pokemon; Player side slot 2
		/// </summary>
		public IPokemonDataBox						battlebox2	{ get { return _battlebox2; } set { _battlebox2 = value as PokemonDataBox; } }
		/// <summary>
		/// HP, Exp, and other status of battling pokemon; Enemy side slot 2
		/// </summary>
		public IPokemonDataBox						battlebox3	{ get { return _battlebox3; } set { _battlebox3 = value as PokemonDataBox; } }
		/// <summary>
		/// Your side primary battle pokemon; slot one
		/// </summary>
		public IPokemonBattlerSprite				pokemon0	{ get { return _pokemon0; } set { _pokemon0 = value; } }
		/// <summary>
		/// Your side ally battle pokemon; slot two (only appears in double battles)
		/// </summary>
		public IPokemonBattlerSprite				pokemon2	{ get { return _pokemon2; } set { _pokemon2 = value; } }
		/// <summary>
		/// Enemy side primary battle pokemon; slot one
		/// </summary>
		public IPokemonBattlerSprite				pokemon1	{ get { return _pokemon1; } set { _pokemon1 = value; } }
		/// <summary>
		/// Enemy side ally battle pokemon; slot two (only appears in double battles)
		/// </summary>
		public IPokemonBattlerSprite				pokemon3	{ get { return _pokemon3; } set { _pokemon3 = value; } }
		public IIconSprite							shadow0	{ get { return _shadow0; } set { _shadow0 = value; } }
		public IIconSprite							shadow1	{ get { return _shadow1; } set { _shadow1 = value; } }
		public IIconSprite							shadow2	{ get { return _shadow2; } set { _shadow2 = value; } }
		public IIconSprite							shadow3	{ get { return _shadow3; } set { _shadow3 = value; } }
		/// <summary>
		/// pokeball thrown at pokemons for capture animation
		/// </summary>
		public IIconSprite							capture	{ get { return _spriteBall; } set { _spriteBall = value; } }
		public IList<IWindow>						pkmnwindows	{ get { return _pkmnwindows; } set { _pkmnwindows = value; } }
		public IDictionary<string, object>			sprites	{ get { return _sprites; } set { _sprites = value; } }
		//public Dictionary<string, UnityEngine.GameObject>			sprites	{ get { return _sprites; } set { _sprites = value; } }
		public IViewport							viewport	{ get { return _viewport; } set { _viewport = value; } }
		public bool									aborted	{ get { return _aborted; } set { _aborted = value; } }
		public bool									abortable	{ get { return _abortable; } set { _abortable = value; } }
		public bool									battlestart	{ get { return _battlestart; } set { _battlestart = value; } }
		public bool									messagemode	{ get { return _messagemode; } set { _messagemode = value; } }
		public bool									briefmessage	{ get { return _briefmessage; } set { _briefmessage = value; } }
		public bool									showingplayer	{ get { return _showingplayer; } set { _showingplayer = value; } }
		public bool									showingenemy	{ get { return _showingenemy; } set { _showingenemy = value; } }
		public bool									enablePartyAnim	{ get { return _enablePartyAnim; } set { _enablePartyAnim = value; } }
		public int									partyAnimPhase	{ get { return _partyAnimPhase; } set { _partyAnimPhase = value; } }
		public float								xposplayer	{ get { return _xposplayer; } set { _xposplayer = value; } }
		public float								xposenemy	{ get { return _xposenemy; } set { _xposenemy = value; } }
		public float								foeyoffset	{ get { return _foeyoffset; } set { _foeyoffset = value; } }
		public float								traineryoffset	{ get { return _traineryoffset; } set { _traineryoffset = value; } }
		#endregion

		#region Unity's MonoBehavior Inspector Properties
		[SerializeField] private IDictionary<string, object>		_sprites;
		[Header("Scene Canvas Panel Layers")]
		[SerializeField] private IViewport							_viewport;
		[SerializeField] private CommandWindowText					_commandPokemon;
		[SerializeField] private CommandMenuDisplay					_commandWindow;
		[SerializeField] private FightMenuDisplay					_fightWindow;
		[SerializeField] private WindowText							_helpWindow;
		[SerializeField] private WindowTextAdvance					_messageWindow;
		[SerializeField] private WindowTextAdvance					_messageBox;
		[SerializeField] private IPartyDisplayScreen				_switchscreen;
		[SerializeField] private IList<IWindow>						_pkmnwindows;
		[Space()]
		[Header("Scene Battle GameObjects")]
		[Space()]
		[Header("Trainer Avatars")]
		[SerializeField] private IPokemonBattlerSprite				_player;
		[SerializeField] private IPokemonBattlerSprite				_playerB;
		[SerializeField] private IPokemonBattlerSprite				_trainer;
		[SerializeField] private IPokemonBattlerSprite				_trainer2;
		[SerializeField] private SpriteWrapper						_battlebg;
		[SerializeField] private SpriteWrapper						_enemybase;
		[SerializeField] private SpriteWrapper						_playerbase;
		[SerializeField] private SpriteWrapper						_partybarfoe;
		[SerializeField] private SpriteWrapper						_partybarplayer;
		[SerializeField] private IIconSprite						_spriteBall;
		[SerializeField] private ITrainerFadeAnimation				_fadeanim;
		[SerializeField] private IPokeballSendOutAnimation			_sendout;
		[Header("HP and Experience Meter")]
		[SerializeField] private PokemonDataBox						_battlebox0;
		[SerializeField] private PokemonDataBox						_battlebox1;
		[SerializeField] private PokemonDataBox						_battlebox2;
		[SerializeField] private PokemonDataBox						_battlebox3;
		[Header("Pokemon Avatars with Shadow")]
		[SerializeField] private IPokemonBattlerSprite				_pokemon0;
		[SerializeField] private IPokemonBattlerSprite				_pokemon2;
		[SerializeField] private IPokemonBattlerSprite				_pokemon1;
		[SerializeField] private IPokemonBattlerSprite				_pokemon3;
		[SerializeField] private IIconSprite						_shadow0;
		[SerializeField] private IIconSprite						_shadow1;
		[SerializeField] private IIconSprite						_shadow2;
		[SerializeField] private IIconSprite						_shadow3;
		[Header("Debugging Battle Data")]
		[SerializeField] private IBattleIE							_battle;
		[SerializeField] private MenuCommands[]						_lastcmd;
		[SerializeField] private int[]								_lastmove;
		[SerializeField] private bool								_aborted;
		[SerializeField] private bool								_abortable;
		[SerializeField] private bool								_battlestart;
		[SerializeField] private bool								_messagemode;
		[SerializeField] private bool								_briefmessage;
		[SerializeField] private bool								_showingplayer;
		[SerializeField] private bool								_showingenemy;
		[SerializeField] private bool								_enablePartyAnim;
		[SerializeField] private int								_partyAnimPhase;
		[SerializeField] private float								_xposplayer;
		[SerializeField] private float								_xposenemy;
		[SerializeField] private float								_foeyoffset;
		[SerializeField] private float								_traineryoffset;
		#endregion

		#region Unity MonoBehavior Functions
		private void Awake()
		{
			if (commandwindow == null) commandwindow = GetComponentInChildren<ICommandMenuDisplay>();
			if (fightwindow == null) fightwindow = GetComponentInChildren<IFightMenuDisplay>();

			//messageBox = _messageBox.GetComponent<>() as IGameObject;
			//fightWindow = _fightWindow.GetComponent<FightMenuDisplay>() as IGameObject;
			//commandWindow = _commandWindow.GetComponent<>() as IGameObject;
			//messageWindow = _messageWindow.GetComponent<>() as IGameObject;
			//helpWindow = _helpwindow.GetComponent<>() as IGameObject;
			//battlebg = _battlebg.GetComponent<>() as IGameObject;
			//player = _player.GetComponent<>() as IGameObject;
			//playerB = _playerB.GetComponent<>() as IGameObject;
			//trainer = _trainer.GetComponent<>() as IGameObject;
			//trainer2 = _trainer2.GetComponent<>() as IGameObject;
			//playerbase = _playerbase.GetComponent<>() as IGameObject;
			//enemybase = _enemybase.GetComponent<>() as IGameObject;
			//partybarplayer = _partybarplayer.GetComponent<>() as IGameObject;
			//partybarfoe = _partybarfoe.GetComponent<>() as IGameObject;
			//spriteBall = _capture.GetComponent<>() as IGameObject;
			//battlebox0 = _battlebox0.GetComponent<>() as IGameObject;
			//battlebox1 = _battlebox1.GetComponent<>() as IGameObject;
			//battlebox2 = _battlebox2.GetComponent<>() as IGameObject;
			//battlebox3 = _battlebox3.GetComponent<>() as IGameObject;
			//pokemon0 = _pokemon0.GetComponent<>() as IGameObject;
			//pokemon1 = _pokemon1.GetComponent<>() as IGameObject;
			//pokemon2 = _pokemon2.GetComponent<>() as IGameObject;
			//pokemon3 = _pokemon3.GetComponent<>() as IGameObject;
			//shadow0 = _shadow0.GetComponent<>() as IGameObject;
			//shadow1 = _shadow1.GetComponent<>() as IGameObject;
			//shadow2 = _shadow2.GetComponent<>() as IGameObject;
			//shadow3 = _shadow3.GetComponent<>() as IGameObject;

			//ToDo: Remove scene assign from here, and move to a consolidated monobehaviour game object
			//GameManager.game.Scenes.BattleScene = this;
		}

		private void Start()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			Core.Logger.Log("######################################");
			Core.Logger.Log("# Hello - Welcome to Unity Battle! #");
			Core.Logger.Log("######################################");

			//IPokeBattle_DebugSceneNoGraphics pokeBattle = new PokeBattleScene();
			(this as IPokeBattle_SceneIE).initialize(); //pokeBattle.initialize();


			IPokemon[] p1 = new IPokemon[] { new PokemonUnity.Monster.Pokemon(Pokemons.ABRA), new PokemonUnity.Monster.Pokemon(Pokemons.EEVEE) };
			IPokemon[] p2 = new IPokemon[] { new PokemonUnity.Monster.Pokemon(Pokemons.MONFERNO) }; //, new PokemonUnity.Monster.Pokemon(Pokemons.SEEDOT) };

			p1[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			p1[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			p2[0].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);
			//p2[1].moves[0] = new PokemonUnity.Attack.Move(Moves.POUND);

			//PokemonUnity.Character.TrainerData trainerData = new PokemonUnity.Character.TrainerData("FlakTester", true, 120, 002);
			//Game.GameData.Player = new PokemonUnity.Character.Player(trainerData, p1);
			//Game.GameData.Trainer = new Trainer("FlakTester", true, 120, 002);

			(p1[0] as PokemonUnity.Monster.Pokemon).SetNickname("Test1");
			(p1[1] as PokemonUnity.Monster.Pokemon).SetNickname("Test2");

			(p2[0] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest1");
			//(p2[1] as PokemonUnity.Monster.Pokemon).SetNickname("OppTest2");

			//ITrainer player = new Trainer(Game.GameData.Trainer.name, TrainerTypes.PLAYER);
			ITrainer player = new PokemonUnity.Trainer("FlakTester", TrainerTypes.CHAMPION);
			//ITrainer pokemon = new Trainer("Wild Pokemon", TrainerTypes.WildPokemon);
			Game.GameData.Trainer = player;
			Game.GameData.Trainer.party = p1;

			//IBattle battle = new Battle(pokeBattle, Game.GameData.Trainer.party, p2, Game.GameData.Trainer, null, 2);
			battle = new UnityBattleTest(this, p1, p2, Game.GameData.Trainer, null);

			battle.rules.Add(BattleRule.SUDDENDEATH, false);
			battle.rules.Add("drawclause", false);
			battle.rules.Add(BattleRule.MODIFIEDSELFDESTRUCTCLAUSE, false);

			battle.weather = PokemonUnity.Combat.Weather.SUNNYDAY;

			battle.StartBattle(true);
		}
		#endregion

		#region Interface Methods
		public IPokeBattle_SceneIE initialize()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			battle = null;
			lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			lastmove = new int[] { 0, 0, 0, 0 };
			pkmnwindows = new IWindow[] { null, null, null, null };
			sprites = new Dictionary<string, object>() {
				{ "messagebox",		messagebox			as IGameObject },
				{ "fightwindow",	fightwindow			as IGameObject },
				{ "commandwindow",	commandwindow		as IGameObject },
				{ "messagewindow",	messagewindow		as IGameObject },
				{ "helpwindow",		helpwindow			as IGameObject },
				{ "battlebg",		battlebg			as IGameObject },
				{ "player",			player				as IGameObject },
				{ "playerB",		playerB				as IGameObject },
				{ "trainer",		trainer				as IGameObject },
				{ "trainer2",		trainer2			as IGameObject },
				{ "playerbase",		playerbase			as IGameObject },
				{ "enemybase",		enemybase			as IGameObject },
				{ "partybarplayer",	partybarplayer		as IGameObject },
				{ "partybarfoe",	partybarfoe			as IGameObject },
				//{ "capture",		spriteBall			as IGameObject },
				{ "capture",		capture				as IGameObject },
				{ "battlebox0",		battlebox0			as IGameObject },
				{ "battlebox1",		battlebox1			as IGameObject },
				{ "battlebox2",		battlebox2			as IGameObject },
				{ "battlebox3",		battlebox3			as IGameObject },
				{ "pokemon0",		pokemon0			as IGameObject },
				{ "pokemon1",		pokemon1			as IGameObject },
				{ "pokemon2",		pokemon2			as IGameObject },
				{ "pokemon3",		pokemon3			as IGameObject },
				{ "shadow0",		shadow0				as IGameObject },
				{ "shadow1",		shadow1				as IGameObject },
				{ "shadow2",		shadow2				as IGameObject },
				{ "shadow3",		shadow3				as IGameObject }
			};
			battlestart = true;
			messagemode = false;
			abortable = true;
			aborted = false;
			return this;
		}

		IPokeBattle_Scene IPokeBattle_Scene.initialize()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			return (IPokeBattle_Scene)initialize();
		}

		public void Update()
		{
			//LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			partyAnimationUpdate();
			//if (sprites["battlebg"] is GameObject g) g.Update();
		}

		public void GraphicsUpdate()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			partyAnimationUpdate();
			//if (sprites["battlebg"] is GameObject g) g.Update();
			//(Game.GameData as Game).Graphics.Update();
		}

		public void InputUpdate()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			PokemonUnity.Input.update();
			if (global::UnityEngine.Input.GetButtonDown("Select") && abortable && !aborted)
			{
				aborted = true;
				battle.Abort();
			}
		}

		/// <summary>
		/// Sets the UI component to visible, in scene
		/// </summary>
		/// <param name="windowtype"></param>
		public void ShowWindow(int windowtype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//messagebox.visible = (windowtype == MESSAGEBOX ||
			//						windowtype == COMMANDBOX ||
			//						windowtype == FIGHTBOX ||
			//						windowtype == BLANK);
			//(messagewindow).visible = (windowtype == MESSAGEBOX);
			(commandwindow).visible = (windowtype == COMMANDBOX);
			(fightwindow).visible = (windowtype == FIGHTBOX);
		}

		public void SetMessageMode(bool mode)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			@messagemode = mode;
			if (messagewindow == null) return;
			IWindow_AdvancedTextPokemon msgwindow = messagewindow;
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
			}
		}

		public IEnumerator WaitMessage()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (@briefmessage)
			{
				ShowWindow(MESSAGEBOX);
				IWindow_AdvancedTextPokemon cw = messagewindow;
				int i = 0;
				do
				{
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate(cw as IGameObject);
					i++;
					yield return null;
				} while (i < 60);
				cw.text = "";
				cw.visible = false;
				@briefmessage = false;
			}
		}

		void IPokeBattle_Scene.WaitMessage() { }

		public IEnumerator Display(string msg, bool brief)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			return DisplayMessage(msg, brief);
		}

		void IPokeBattle_Scene.Display(string msg, bool brief)
		{
		}

		public IEnumerator Display(string v)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			return Display(v, false);
		}

		void IHasDisplayMessage.Display(string v)
		{
			Display(v);
		}

		public IEnumerator DisplayMessage(string msg, bool brief)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log(message: "Display Message: `{0}`", msg);
			WaitMessage();
			Refresh();
			ShowWindow(MESSAGEBOX);
			//IWindow_AdvancedTextPokemon cw = messagewindow;
			//cw.text = msg;
			//int i = 0;
			////All of this below should be a coroutine that returns the value selected in UI
			//do //begin coroutine
			//{
			//	GraphicsUpdate();
			//	InputUpdate();
			//	FrameUpdate(cw as IGameObject);
			//	if (i == 40)
			//	{
			//		//end dialog window, after 40 ticks.
			//		//messagewindow.text = "";
			//		//messagewindow.visible = false;
			//		cw.text = "";
			//		cw.visible = false;
			//		yield break;
			//	}
			//	//if (global::UnityEngine.Input.GetButtonDown("Start") || abortable)
			//	if (PokemonUnity.Input.trigger(PokemonUnity.Input.CTRL) || abortable)
			//	{
			//		if (cw.pause)
			//		{
			//			if (!abortable && AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
			//			cw.resume();
			//		}
			//	}
			//	if (!cw.busy)
			//	{
			//		if (brief)
			//		{
			//			briefmessage = true;
			//			yield break;
			//		}
			//		i++;
			//	}
			//} while (true);
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.DisplayMessage(string msg, bool brief)
		{
			DisplayMessage(msg, brief);
		}

		public IEnumerator DisplayPausedMessage(string msg)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log(message: "Display Message: \"{0}\"", msg);
			WaitMessage();
			Refresh();
			ShowWindow(MESSAGEBOX);
			if (@messagemode)
			{
				@switchscreen.Display(msg);
				yield break;
			}
			//IWindow_AdvancedTextPokemon cw = messagewindow;
			//cw.text = string.Format("{1:s}\\1", msg);
			////All of this below should be a coroutine that returns the value selected in UI
			//do //;loop
			//{
			//	GraphicsUpdate();
			//	InputUpdate();
			//	FrameUpdate(cw as IGameObject);
			//	if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) || @abortable)
			//	{
			//		if (cw.busy)
			//		{
			//			if (cw.pausing && !@abortable && AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
			//			cw.resume();
			//		}
			//		else if (!inPartyAnimation)
			//		{
			//			cw.text = "";
			//			if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
			//			if (@messagemode) cw.visible = false;
			//			yield break;
			//		}
			//	}
			//	cw.update();
			//} while (true);
		}

		void IPokeBattle_DebugSceneNoGraphics.DisplayPausedMessage(string msg)
		{
			DisplayPausedMessage(msg);
		}

		public IEnumerator DisplayConfirmMessage(string msg, System.Action<bool> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int value = 0;
			yield return ShowCommands(msg, new string[] { Game._INTL("Yes"), Game._INTL("No") }, 1, result: r => value = r);
			result?.Invoke(value == 0);
			//yield return ShowCommands(msg, new string[] { Game._INTL("Yes"), Game._INTL("No") }, 1) == 0;
		}

		bool IPokeBattle_DebugSceneNoGraphics.DisplayConfirmMessage(string msg)
		{
			bool r = false;
			DisplayConfirmMessage(msg, result: value => r = value);
			return r;
		}

		IEnumerator IHasDisplayMessageIE.DisplayConfirm(string v, Action<bool> result)
		{
			yield return DisplayConfirmMessage(v, result);
		}

		bool IPokeBattle_DebugSceneNoGraphics.ShowCommands(string msg, string[] commands, bool defaultValue)
		{
			throw new NotImplementedException();
		}

		int IPokeBattle_DebugSceneNoGraphics.ShowCommands(string msg, string[] commands, int defaultValue)
		{
			throw new NotImplementedException();
		}

		public IEnumerator ShowCommands(string msg, string[] commands, int defaultValue, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Core.Logger.Log(message: "Display Message: \"{0}\"", msg);
			WaitMessage();
			Refresh();
			ShowWindow(MESSAGEBOX);
			//IWindow_AdvancedTextPokemon dw = messagewindow;
			//dw.text = msg;
			////IWindow_CommandPokemon cw = new Window_CommandPokemon(commands);
			//IWindow_CommandPokemon cw = commandpokemon.initialize(commands);
			////cw.x = (Game.GameData as Game).Graphics.width - cw.width;
			////cw.y = (Game.GameData as Game).Graphics.height - cw.height - dw.height;
			//cw.index = 0;
			//cw.viewport = @viewport;
			//Refresh();
			////All of this below should be a coroutine that returns the value selected in UI
			//do //;loop
			//{
			//	cw.visible = !dw.busy;
			//	GraphicsUpdate();
			//	InputUpdate();
			//	FrameUpdate(cw as IGameObject);
			//	dw.update();
			//	if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && defaultValue >= 0)
			//	{
			//		if (dw.busy)
			//		{
			//			if (dw.pausing && AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
			//			dw.resume();
			//		}
			//		else
			//		{
			//			cw.Dispose();
			//			dw.text = "";
			//			result?.Invoke(defaultValue);
			//			yield break;
			//		}
			//	}
			//	if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))
			//	{
			//		if (dw.busy)
			//		{
			//			if (dw.pausing && AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
			//			dw.resume();
			//		}
			//		else
			//		{
			//			cw.Dispose();
			//			dw.text = "";
			//			result?.Invoke(cw.index);
			//			yield break;
			//		}
			//	}
			//	yield return null;
			//} while (true);
			yield break;//return 0;
		}

		//public void FrameUpdate(IFightMenuDisplay cw)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	if (cw != null) cw.update();
		//	FrameUpdate();
		//}
		//
		//public void FrameUpdate(ICommandMenuDisplay cw)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	if (cw != null) cw.update();
		//	FrameUpdate();
		//}
		//
		//public void FrameUpdate(IWindow_CommandPokemon cw)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	if (cw != null) cw.update();
		//	FrameUpdate();
		//}
		//
		//public void FrameUpdate(IWindow_AdvancedTextPokemon cw)
		//{
		//	Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	if (cw != null) cw.update();
		//	FrameUpdate();
		//}

		public void FrameUpdate(IGameObject cw)
		{
			//Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (cw != null) cw.update();
			FrameUpdate();
		}

		void IPokeBattle_DebugScene.FrameUpdate(IViewport cw)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (cw != null) cw.update();
			FrameUpdate();
		}

		private void FrameUpdate()
		{
			//Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if (cw != null) cw.update();
			for (int i = 0; i < 4; i++)
			{
				if (@sprites.ContainsKey($"battlebox{i}") && @sprites[$"battlebox{i}"] != null)
				{
					(@sprites[$"battlebox{i}"] as IPokemonDataBox).update();
				}
				if (@sprites.ContainsKey($"pokemon{i}") && @sprites[$"pokemon{i}"] != null)
				{
					(@sprites[$"pokemon{i}"] as IPokemonBattlerSprite).update();
				}
			}
		}

		public void Refresh()
		{
			//Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			for (int i = 0; i < 4; i++)
			{
				if (@sprites.ContainsKey($"battlebox{i}") && @sprites[$"battlebox{i}"] != null)
				{
					(@sprites[$"battlebox{i}"] as IPokemonDataBox).refresh();
				}
			}
			global::UnityEngine.Canvas.ForceUpdateCanvases();
			StartCoroutine(ForceFrameRefresh());
		}

		/// <summary>
		/// Instantiate Sprite GameObject, then adds to <see cref="sprites"/> by <paramref name="id"/> as lookup reference
		/// </summary>
		/// <param name="id"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="filename"></param>
		/// <param name="viewport"></param>
		public IIconSprite AddSprite(string id, float x, float y, string filename, IViewport viewport)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//IIconSprite sprite = new IconSprite(x, y, viewport);
			IIconSprite sprite = null; //UnityEngine.GameObject.Instantiate<IIconSprite>().initialize((float)x, (float)y, viewport);
			if (!string.IsNullOrEmpty(filename))
			{
				sprite.setBitmap(filename); //rescue null;
			}
			@sprites[id] = sprite;
			return sprite;
		}

		public void AddPlane(string id, string filename, IViewport viewport)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//IAnimatedPlane sprite = new AnimatedPlane(viewport);
			//if (!string.IsNullOrEmpty(filename))
			//{
			//	sprite.setBitmap(filename);
			//}
			//@sprites[id] = sprite;
			//return sprite;
		}

		public void DisposeSprites()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//DisposeSpriteHash(@sprites); //Clears list of GameObjects in scene?...
		}

		public void BeginCommandPhase()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//  Called whenever a new round begins.
			@battlestart = false;
		}

		/// <summary>
		/// Tween sprite on-screen over 20 frames
		/// </summary>
		/// <param name="index"></param>
		public IEnumerator ShowOpponent(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			string trainerfile = string.Empty;
			if (@battle.opponent != null)
			{
				if (@battle.opponent.Length > 1) //(@battle.opponent is Array)
				{
					trainerfile = FileTest.TrainerSpriteFile(@battle.opponent[index].trainertype);
				}
				else
				{
					trainerfile = FileTest.TrainerSpriteFile(@battle.opponent[0].trainertype);
				}
			}
			else
			{
				trainerfile = "Graphics/Characters/trfront";
			}
			//AddSprite("trainer", (Game.GameData as Game).Graphics.width, PokeBattle_SceneConstants.FOETRAINER_Y,
			//   trainerfile, @viewport); //Set item for visible in scene
			if (trainer.bitmap != null)
			{
				trainer.y -= trainer.bitmap.height;
				trainer.z = 8;
			}
			int i = 0;
			do { //20.times ;
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				(trainer).x -= 6;
				i++;
				yield return null;
			} while (i < 20);
		}

		void IPokeBattle_DebugSceneNoGraphics.ShowOpponent(int opp) { }

		/// <summary>
		/// Tween sprite off-screen over 20 frames
		/// </summary>
		public IEnumerator HideOpponent()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int i = 0;
			do { //20.times ;3
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				(trainer).x += 6;
				i++;
				yield return null;
			} while (i < 20);
		}

		void IPokeBattle_DebugSceneNoGraphics.HideOpponent() { }

		public void ShowHelp(string text)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//(helpwindow).resizeToFit(text, (Game.GameData as Game).Graphics.width);
			(helpwindow).y = 0;
			(helpwindow).x = 0;
			(helpwindow).text = text;
			(helpwindow).visible = true;
		}

		public void HideHelp()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			(helpwindow).visible = false;
		}

		public void Backdrop()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

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
				if (Game.GameData.GameMap == null || (Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc e && !e.GetMetadata(Game.GameData.GameMap.map_id).Map.Outdoor)) //!GetMetadata(Game.GameData.GameMap.map_id, MetadataOutdoor))
				{
					backdrop = "IndoorA";
				}
			}
			if (Game.GameData.GameMap != null && Game.GameData is PokemonEssentials.Interface.Field.IGameMetadataMisc m1)
			{
				string back = m1.GetMetadata(Game.GameData.GameMap.map_id).Map.BattleBack;
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
			if (FileTest.ResolveBitmap(string.Format("Graphics/Battlebacks/playerbase" + backdrop + trialname)))
			{
				_base = trialname;
			}
			//  Choose time of day
			string time = "";
			if (Core.ENABLESHADING)
			{
				trialname = "";
				DateTime timenow = Game.GetTimeNow;
				if (Game.DayNight.isNight(timenow))
				{
					trialname = "Night";
				}
				else if (Game.DayNight.isEvening(timenow))
				{
					trialname = "Eve";
				}
				if (FileTest.ResolveBitmap(string.Format("Graphics/Battlebacks/battlebg" + backdrop + trialname)))
				{
					time = trialname;
				}
			}
			//  Apply graphics
			//string battlebg = "Graphics/Battlebacks/battlebg" + backdrop + time;
			//string enemybase = "Graphics/Battlebacks/enemybase" + backdrop + _base + time;
			//string playerbase = "Graphics/Battlebacks/playerbase" + backdrop + _base + time;
			/*AddPlane("battlebg", battlebg, @viewport);
			AddSprite("playerbase",
			   PokeBattle_SceneConstants.PLAYERBASEX,
			   PokeBattle_SceneConstants.PLAYERBASEY, playerbase, @viewport);
			if (playerbase.bitmap != null) playerbase.x -= playerbase.bitmap.width / 2;
			if (playerbase.bitmap != null) playerbase.y -= playerbase.bitmap.height;
			AddSprite("enemybase",
			   PokeBattle_SceneConstants.FOEBASEX,
			   PokeBattle_SceneConstants.FOEBASEY, enemybase, @viewport);
			if (enemybase.bitmap != null) enemybase.x -= enemybase.bitmap.width / 2;
			if (enemybase.bitmap != null) enemybase.y -= enemybase.bitmap.height / 2;*/
			battlebg.z = 0;
			playerbase.z = 1;
			enemybase.z = 1;
		}

		/// <summary>
		/// Shows the party line-ups appearing on-screen
		/// </summary>
		///TODO: Can rewrite this to use Coroutine and LeanTween to animate
		///Can also be a void: while(true) { if frame == index, then move sprite to x/y position }?
		public IEnumerator partyAnimationUpdate()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (!inPartyAnimation) yield break;
			/*int ballmovedist = 16; // How far a ball moves each frame
			//  Bar slides on
			if (@partyAnimPhase == 0)
			{
				partybarfoe.x += 16;
				partybarplayer.x -= 16;
				if (partybarfoe.x + partybarfoe.bitmap.width >= PokeBattle_SceneConstants.FOEPARTYBAR_X)
				{
					partybarfoe.x = PokeBattle_SceneConstants.FOEPARTYBAR_X - partybarfoe.bitmap.width;
					partybarplayer.x = PokeBattle_SceneConstants.PLAYERPARTYBAR_X;
					@partyAnimPhase = 1;
				}
				yield break;
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
					yield return null;
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
					AddSprite($"player{i}", //Instantiate player GameObject, then adds to dictionary for tracking...
					   @xposplayer + i * ballmovedist * 6, PokeBattle_SceneConstants.PLAYERPARTYBALL1_Y,
					   ballgraphic, @viewport);
					@sprites[$"player{i}"].z = 41;
					//  Choose the ball's graphic (opponent's side)
					ballgraphic = "Graphics/Pictures/ballempty";
					int enemyindex = i;
					if (@battle.doublebattle && i >= 3)
					{
						enemyindex = (i % 3) + @battle.SecondPartyBegin(1);
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
					AddSprite($"enemy{i}",
					   @xposenemy - i * ballmovedist * 6, PokeBattle_SceneConstants.FOEPARTYBALL1_Y,
					   ballgraphic, @viewport);
					@sprites[$"enemy{i}"].z = 41;
					yield return null;
				}
				@partyAnimPhase = 2;
			}
			//  Balls slide on
			if (@partyAnimPhase == 2)
			{
				for (int i = 0; i < 6; i++)
				{
					if (@sprites[$"enemy{i}"].x < PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
					{
						@sprites[$"enemy{i}"].x += ballmovedist;
						@sprites[$"player{i}"].x -= ballmovedist;
						if (@sprites[$"enemy{i}"].x >= PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP)
						{
							@sprites[$"enemy{i}"].x = PokeBattle_SceneConstants.FOEPARTYBALL1_X - i * PokeBattle_SceneConstants.FOEPARTYBALL_GAP;
							@sprites[$"player{i}"].x = PokeBattle_SceneConstants.PLAYERPARTYBALL1_X + i * PokeBattle_SceneConstants.PLAYERPARTYBALL_GAP;
							if (i == 5)
							{
								@partyAnimPhase = 3;
							}
						}
					}
					yield return null;
				}
			}*/
			yield break;
		}

		void IPokeBattle_Scene.partyAnimationUpdate() { }

		public IEnumerator StartBattle(IBattleIE battle)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			Core.Logger.Log("Start of the battle");
			Core.Logger.Log("Play animation of pokemons coming to scene before player can control actions");

			//  Called whenever the battle begins
			this.battle = battle; //as Battle;
			@lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
			@lastmove = new int[] { 0, 0, 0, 0 };
			@showingplayer = true;
			@showingenemy = true;
			//@sprites.Clear(); //ToDo: make a reset without deleting the entire dictionary?...
			//@viewport = new Viewport(0, (Game.GameData as Game).Graphics.height / 2, (Game.GameData as Game).Graphics.width, 0);
			/*@viewport.initialize(0, (Game.GameData as Game).Graphics.height / 2, (Game.GameData as Game).Graphics.width, 0);
			//@viewport.z = 99999;
			// ToDo: Create coroutine animation for the below sprite changes
			@traineryoffset = ((Game.GameData as Game).Graphics.height - 320); // Adjust player's side for screen size
			@foeyoffset = (float)Math.Floor(@traineryoffset * 3f / 4f);  // Adjust foe's side for screen size
			Backdrop();
			AddSprite("partybarfoe",
				PokeBattle_SceneConstants.FOEPARTYBAR_X,
				PokeBattle_SceneConstants.FOEPARTYBAR_Y,
				"Graphics/Pictures/battleLineup", @viewport);
			AddSprite("partybarplayer",
				PokeBattle_SceneConstants.PLAYERPARTYBAR_X,
				PokeBattle_SceneConstants.PLAYERPARTYBAR_Y,
				"Graphics/Pictures/battleLineup", @viewport);
			partybarfoe.x -= partybarfoe.bitmap.width;
			partybarplayer.mirror = true;
			partybarfoe.z = 40;
			partybarplayer.z = 40;
			partybarfoe.visible = false;
			partybarplayer.visible = false;
			string trainerfile = string.Empty;
			if (@battle.player.Length > 1) //(@battle.player is Array)
			{
				trainerfile = FileTest.PlayerSpriteBackFile(@battle.player[0].trainertype);
				AddSprite("player",
					 PokeBattle_SceneConstants.PLAYERTRAINERD1_X,
					 PokeBattle_SceneConstants.PLAYERTRAINERD1_Y, trainerfile, @viewport);
				trainerfile = FileTest.TrainerSpriteBackFile(@battle.player[1].trainertype);
				AddSprite("playerB",
					 PokeBattle_SceneConstants.PLAYERTRAINERD2_X,
					 PokeBattle_SceneConstants.PLAYERTRAINERD2_Y, trainerfile, @viewport);
				if (player.bitmap != null)
				{
					if (player.bitmap.width > player.bitmap.height)
					{
						player.src_rect.x = 0;
						player.src_rect.width = player.bitmap.width / 5;
					}
					player.x -= (player.src_rect.width / 2);
					player.y -= player.bitmap.height;
					player.z = 30;
				}
				if (playerB.bitmap != null)
				{
					if (playerB.bitmap.width > playerB.bitmap.height)
					{
						playerB.src_rect.x = 0;
						playerB.src_rect.width = playerB.bitmap.width / 5;
					}
					playerB.x -= (playerB.src_rect.width / 2);
					playerB.y -= playerB.bitmap.height;
					playerB.z = 31;
				}
			}
			else
			{
				trainerfile = FileTest.PlayerSpriteBackFile(@battle.player[0].trainertype);
				AddSprite("player",
					 PokeBattle_SceneConstants.PLAYERTRAINER_X,
					 PokeBattle_SceneConstants.PLAYERTRAINER_Y, trainerfile, @viewport);
				if (player.bitmap != null)
				{
					if (player.bitmap.width > player.bitmap.height)
					{
						player.src_rect.x = 0;
						player.src_rect.width = player.bitmap.width / 5;
					}
					player.x -= (player.src_rect.width / 2);
					player.y -= player.bitmap.height;
					player.z = 30;
				}
			}
			if (@battle.opponent != null)
			{
				if (@battle.opponent.Length > 1) //(@battle.opponent is Array)
				{
					trainerfile = FileTest.TrainerSpriteFile(@battle.opponent[1].trainertype);
					AddSprite("trainer2",
						PokeBattle_SceneConstants.FOETRAINERD2_X,
						PokeBattle_SceneConstants.FOETRAINERD2_Y, trainerfile, @viewport);
					trainerfile = FileTest.TrainerSpriteFile(@battle.opponent[0].trainertype);
					AddSprite("trainer",
						PokeBattle_SceneConstants.FOETRAINERD1_X,
						PokeBattle_SceneConstants.FOETRAINERD1_Y, trainerfile, @viewport);
				}
				else
				{
					trainerfile = FileTest.TrainerSpriteFile(@battle.opponent[0].trainertype);
					AddSprite("trainer",
						PokeBattle_SceneConstants.FOETRAINER_X,
						PokeBattle_SceneConstants.FOETRAINER_Y, trainerfile, @viewport);
				}
			}
			else
			{
				trainerfile = "Graphics/Characters/trfront";
				AddSprite("trainer",
					PokeBattle_SceneConstants.FOETRAINER_X,
					PokeBattle_SceneConstants.FOETRAINER_Y, trainerfile, @viewport);
			}
			if (trainer.bitmap != null)
			{
				trainer.x -= (trainer.bitmap.width / 2);
				trainer.y -= trainer.bitmap.height;
				trainer.z = 8;
			}
			if (trainer2 != null && trainer2.bitmap != null)
			{
				trainer2.x -= (trainer2.bitmap.width / 2);
				trainer2.y -= trainer2.bitmap.height;
				trainer2.z = 7;
			}
			//shadow0 = new IconSprite(0, 0, @viewport);
			(shadow0).initialize(0, 0, @viewport);
			(shadow0).z = 3;
			AddSprite("shadow1", 0, 0, "Graphics/Pictures/battleShadow", @viewport);
			(shadow1).z = 3;
			(shadow1).visible = false;
			//pokemon0 = new PokemonBattlerSprite(battle.doublebattle, 0, @viewport);
			(pokemon0).initialize(battle.doublebattle, 0, @viewport);
			(pokemon0).z = 21;
			//pokemon1 = new PokemonBattlerSprite(battle.doublebattle, 1, @viewport);
			(pokemon1).initialize(battle.doublebattle, 1, @viewport);
			(pokemon1).z = 16;
			if (battle.doublebattle)
			{
				//shadow2 = new IconSprite(0, 0, @viewport);
				(shadow2).initialize(0, 0, @viewport);
				(shadow2).z = 3;
				AddSprite("shadow3", 0, 0, "Graphics/Pictures/battleShadow", @viewport);
				(shadow3).z = 3;
				(shadow3).visible = false;
				//pokemon2 = new PokemonBattlerSprite(battle.doublebattle, 2, @viewport);
				(pokemon2).initialize(battle.doublebattle, 2, @viewport);
				(pokemon2).z = 26;
				//pokemon3 = new PokemonBattlerSprite(battle.doublebattle, 3, @viewport);
				(pokemon3).initialize(battle.doublebattle, 3, @viewport);
				(pokemon3).z = 11;
			}*/
			//battlebox0 = new PokemonDataBox(battle.battlers[0], battle.doublebattle, @viewport);
			//battlebox1 = new PokemonDataBox(battle.battlers[1], battle.doublebattle, @viewport);
			(battlebox0).initialize(battle.battlers[0], battle.doublebattle, @viewport);
			(battlebox1).initialize(battle.battlers[1], battle.doublebattle, @viewport);
			if (battle.doublebattle)
			{
				//battlebox2 = new PokemonDataBox(battle.battlers[2], battle.doublebattle, @viewport);
				//battlebox3 = new PokemonDataBox(battle.battlers[3], battle.doublebattle, @viewport);
				(battlebox2).initialize(battle.battlers[2], battle.doublebattle, @viewport);
				(battlebox3).initialize(battle.battlers[3], battle.doublebattle, @viewport);
			}
			/*AddSprite("messagebox", 0, (Game.GameData as Game).Graphics.height - 96, "Graphics/Pictures/battleMessage", @viewport);
			messagebox.z = 90;
			//helpwindow = new Window_UnformattedTextPokemon().WithSize("", 0, 0, 32, 32, @viewport);
			(helpwindow).initialize().WithSize("", 0, 0, 32, 32, @viewport);
			(helpwindow).visible = false;
			(helpwindow).z = 90;
			//messagewindow = new Window_AdvancedTextPokemon("");
			(messagewindow).initialize("");
			(messagewindow).letterbyletter = true;
			(messagewindow).viewport = @viewport;
			(messagewindow).z = 100;
			//commandwindow = new CommandMenuDisplay(@viewport);
			(commandwindow).initialize(@viewport);
			(commandwindow).z = 100;
			//fightwindow = new FightMenuDisplay(null, @viewport);
			(fightwindow).initialize(null, @viewport);
			(fightwindow).z = 100;
			ShowWindow(MESSAGEBOX);
			SetMessageMode(false);
			IPokemonBattlerSprite trainersprite1 = trainer;
			IPokemonBattlerSprite trainersprite2 = trainer2;
			if (@battle.opponent == null)
			{
				trainer.visible = false;
				if (@battle.party2.Length >= 1 && Game.GameData is IGameSprite gs)
				{
					Pokemons species = Pokemons.NONE;
					if (@battle.party2.Length == 1) //Single Battle
					{
						species = @battle.party2[0].Species;
						(pokemon1).setPokemonBitmap(@battle.party2[0], false);
						//(pokemon1).tone = new Tone(-128, -128, -128, -128);
						(pokemon1).x = PokeBattle_SceneConstants.FOEBATTLER_X;
						(pokemon1).x -= (pokemon1).width / 2;
						(pokemon1).y = PokeBattle_SceneConstants.FOEBATTLER_Y;
						(pokemon1).y += gs.adjustBattleSpriteY(pokemon1, species, 1);
						(pokemon1).visible = true;
						(shadow1).x = PokeBattle_SceneConstants.FOEBATTLER_X;
						(shadow1).y = PokeBattle_SceneConstants.FOEBATTLER_Y;
						if ((shadow1).bitmap != null) (shadow1).x -= (shadow1).bitmap.width / 2;
						if ((shadow1).bitmap != null) (shadow1).y -= (shadow1).bitmap.height / 2;
						(shadow1).visible = gs.showShadow(species);
						trainersprite1 = pokemon1;
					}
					else if (@battle.party2.Length == 2) //Double Battle
					{
						species = @battle.party2[0].Species;
						(pokemon1).setPokemonBitmap(@battle.party2[0], false);
						//(pokemon1).tone = new Tone(-128, -128, -128, -128);
						(pokemon1).x = PokeBattle_SceneConstants.FOEBATTLERD1_X;
						(pokemon1).x -= (pokemon1).width / 2;
						(pokemon1).y = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
						(pokemon1).y += gs.adjustBattleSpriteY(pokemon1, species, 1);
						(pokemon1).visible = true;
						(shadow1).x = PokeBattle_SceneConstants.FOEBATTLERD1_X;
						(shadow1).y = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
						if ((shadow1).bitmap != null) (shadow1).x -= (shadow1).bitmap.width / 2;
						if ((shadow1).bitmap != null) (shadow1).y -= (shadow1).bitmap.height / 2;
						(shadow1).visible = gs.showShadow(species);
						trainersprite1 = pokemon1;
						species = @battle.party2[1].Species;
						(pokemon3).setPokemonBitmap(@battle.party2[1], false);
						//(pokemon3).tone = new Tone(-128, -128, -128, -128);
						(pokemon3).x = PokeBattle_SceneConstants.FOEBATTLERD2_X;
						(pokemon3).x -= (pokemon3).width / 2;
						(pokemon3).y = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
						(pokemon3).y += gs.adjustBattleSpriteY(pokemon3, species, 3);
						(pokemon3).visible = true;
						(shadow3).x = PokeBattle_SceneConstants.FOEBATTLERD2_X;
						(shadow3).y = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
						if ((shadow3).bitmap != null) (shadow3).x -= (shadow3).bitmap.width / 2;
						if ((shadow3).bitmap != null) (shadow3).y -= (shadow3).bitmap.height / 2;
						(shadow3).visible = gs.showShadow(species);
						trainersprite2 = pokemon3;
					}
				}
			}
			// ################
			//  Move trainers/bases/etc. off-screen
			float[] oldx = new float[8];
			oldx[0] = playerbase.x; playerbase.x += (Game.GameData as Game).Graphics.width;
			oldx[1] = player.x; player.x += (Game.GameData as Game).Graphics.width;
			if (playerB != null)
			{
				oldx[2] = playerB.x; playerB.x += (Game.GameData as Game).Graphics.width;
			}
			oldx[3] = enemybase.x; enemybase.x -= (Game.GameData as Game).Graphics.width;
			oldx[4] = trainersprite1.x; trainersprite1.x -= (Game.GameData as Game).Graphics.width;
			if (trainersprite2 != null)
			{
				oldx[5] = trainersprite2.x; trainersprite2.x -= (Game.GameData as Game).Graphics.width;
			}
			oldx[6] = (shadow1).x; (shadow1).x -= (Game.GameData as Game).Graphics.width;
			if (shadow3 != null)
			{
				oldx[7] = (shadow3).x; (shadow3).x -= (Game.GameData as Game).Graphics.width;
			}
			partybarfoe.x -= PokeBattle_SceneConstants.FOEPARTYBAR_X;
			partybarplayer.x += (Game.GameData as Game).Graphics.width - PokeBattle_SceneConstants.PLAYERPARTYBAR_X;
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
					//foreach (var i in @sprites)
					//{
					//	//i[1].ox = @viewport.rect.x;
					//	//i[1].oy = @viewport.rect.y;
					//	i.Value.ox = @viewport.rect.x;
					//	i.Value.oy = @viewport.rect.y;
					//}
				}
				if (playerbase.x > oldx[0])
				{
					playerbase.x -= appearspeed; tobreak = false;
					if (playerbase.x < oldx[0]) playerbase.x = oldx[0];
				}
				if (player.x > oldx[1])
				{
					player.x -= appearspeed; tobreak = false;
					if (player.x < oldx[1]) player.x = oldx[1];
				}
				if (playerB != null && playerB.x > oldx[2])
				{
					playerB.x -= appearspeed; tobreak = false;
					if (playerB.x < oldx[2]) playerB.x = oldx[2];
				}
				if (enemybase.x < oldx[3])
				{
					enemybase.x += appearspeed; tobreak = false;
					if (enemybase.x > oldx[3]) enemybase.x = oldx[3];
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
				if ((shadow1).x < oldx[6])
				{
					(shadow1).x += appearspeed; tobreak = false;
					if ((shadow1).x > oldx[6]) (shadow1).x = oldx[6];
				}
				if (shadow3 != null && (shadow3).x < oldx[7])
				{
					(shadow3).x += appearspeed; tobreak = false;
					if ((shadow3).x > oldx[7]) (shadow3).x = oldx[7];
				}
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (tobreak) break; n++;
				yield return null;
			} while (n < (1 + (Game.GameData as Game).Graphics.width / appearspeed));
			// ################
			if (@battle.opponent != null) {
				@enablePartyAnim=true;
				@partyAnimPhase=0;
				partybarfoe.visible=true;
				partybarplayer.visible=true;
			} else {
				if (Game.GameData is IGameUtility gu) gu.PlayCry(@battle.party2[0]);   // Play cry for wild Pokémon
				(battlebox1).appear();
				if (@battle.party2.Length == 2) (battlebox3).appear();
				bool appearing = true;
				do { //begin;
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate();
					if ((pokemon1).tone.red < 0)		(pokemon1).tone.red += 8;
					if ((pokemon1).tone.blue < 0)		(pokemon1).tone.blue += 8;
					if ((pokemon1).tone.green < 0)		(pokemon1).tone.green += 8;
					if ((pokemon1).tone.gray < 0)		(pokemon1).tone.gray += 8;
					appearing = (battlebox1).appearing;
					if (@battle.party2.Length == 2)
					{
						if ((pokemon3).tone.red < 0)		(pokemon3).tone.red += 8;
						if ((pokemon3).tone.blue < 0)		(pokemon3).tone.blue += 8;
						if ((pokemon3).tone.green < 0)		(pokemon3).tone.green += 8;
						if ((pokemon3).tone.gray < 0)		(pokemon3).tone.gray += 8;
						appearing = (appearing || (battlebox3).appearing);
					}
					yield return null;
				} while (appearing);
				//  Show shiny animation for wild Pokémon
				if (@battle.battlers[1].IsShiny && @battle.battlescene)
				{
					CommonAnimation("Shiny", @battle.battlers[1], null);
				}
				if (@battle.party2.Length == 2)
				{
					if (@battle.battlers[3].IsShiny && @battle.battlescene)
					{
						CommonAnimation("Shiny", @battle.battlers[3], null);
					}
				}
			}*/
			yield break; //return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.StartBattle(IBattle battle) { }

		public IEnumerator EndBattle(BattleResults result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			Core.Logger.Log($"End of the battle. Result: {result}");

			@abortable = false;
			ShowWindow(BLANK);
			//  Fade out all sprites
			if (AudioHandler is IGameAudioPlay gap) gap.BGMFade(1.0f);
			//FadeOutAndHide(@sprites);
			DisposeSprites();
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.EndBattle(BattleResults result) { }

		/// <summary>
		/// Animates the pokemon leaving the battle and returning back to party/pokeball
		/// </summary>
		/// <param name="battlerindex"></param>
		public IEnumerator Recall(int battlerindex)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			@briefmessage = false;
			float origin = 0;
			if (@battle.IsOpposing(battlerindex))
			{
				origin = PokeBattle_SceneConstants.FOEBATTLER_Y;
				if (@battle.doublebattle)
				{
					if (battlerindex == 1) origin = PokeBattle_SceneConstants.FOEBATTLERD1_Y;
					if (battlerindex == 3) origin = PokeBattle_SceneConstants.FOEBATTLERD2_Y;
				}
				(@sprites[$"shadow{battlerindex}"] as IIconSprite).visible = false;
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
			IPokemonBattlerSprite spritePoke = @sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite;
			/*IPictureEx picturePoke = new PictureEx(spritePoke.z);
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
			picturePoke.moveSE(delay, "AudioManager/SE/recall");
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
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (!picturePoke.running) break;
				yield return null;
			} while (true);*/
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.Recall(int battlerindex) { }

		public IEnumerator TrainerSendOut(int battlerindex, IPokemon pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			//TODO show opponent's pokemon
			IPokemon illusionpoke=@battle.battlers[battlerindex].effects.Illusion;
			@briefmessage=false;
			//ITrainerFadeAnimation fadeanim=null;
			while (inPartyAnimation) { yield return null; } //i guess, delay action until trigger criteria is met
			if (@showingenemy) {
				//fadeanim=new TrainerFadeAnimation(@sprites);
				fadeanim.initialize(@sprites);
			}
			int frame=0;
			(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(pkmn,false);
			if (illusionpoke != null) {
				(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(illusionpoke,false);
			}
			//IPokeballSendOutAnimation sendout=new PokeballSendOutAnimation(@sprites[$"pokemon{battlerindex}"],
			sendout.initialize(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite,
				@sprites,@battle.battlers[battlerindex],illusionpoke,@battle.doublebattle);
			do  //;loop
			{
				if (fadeanim != null) fadeanim.update();
				frame += 1;
				if (frame == 1) {
					(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).appear();
				}
				if (frame >= 10) {
					sendout.update();
				}
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if ((fadeanim == null || fadeanim.animdone) && sendout.animdone &&
					!(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).appearing) break;
				yield return null;
			} while (true);
			if (@battle.battlers[battlerindex].IsShiny && @battle.battlescene) {
				CommonAnimation("Shiny",@battle.battlers[battlerindex],null);
			}
			sendout.Dispose();
			if (@showingenemy) {
				@showingenemy=false;
				trainer.visible=false;			//DisposeSprite(@sprites,"trainer");
				this.partybarfoe.visible=false;	//DisposeSprite(@sprites,"partybarfoe");
				for (int i = 0; i < 6; i++) {
					//DisposeSprite(@sprites,$"enemy{i}");
					(@sprites[$"enemy{i}"] as IGameObject).visible=false;
				}
			}
			Refresh();
		}

		void IPokeBattle_Scene.TrainerSendOut(int battlerindex, IPokemon pkmn)
		{
		}

		/// <summary>
		/// Player sending out Pokémon
		/// </summary>
		/// <param name="battlerindex"></param>
		/// <param name="pkmn"></param>
		public IEnumerator SendOut(int battlerindex, IPokemon pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//TODO show player's pokemon
			/*while (inPartyAnimation){ yield return null; } //ToDo: Uncomment, and ensure that logic has an exit
			IPokemon illusionpoke=@battle.battlers[battlerindex].effects.Illusion;
			Items balltype=pkmn.ballUsed;
			if (illusionpoke != null) balltype=illusionpoke.ballUsed;
			string ballbitmap=string.Format("Graphics/Pictures/ball%02d",balltype);
			//pictureBall=new PictureEx(32);
			IPictureEx pictureBall=new PictureEx(32);
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
			//IIconSprite spriteBall=new IconSprite(0,0,@viewport);
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
			(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).visible=false;
			@briefmessage=false;
			//fadeanim=null;
			if (@showingplayer) {
				//fadeanim=new PlayerFadeAnimation(@sprites);
				fadeanim.initialize(@sprites);
			}
			int frame=0;
			(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(pkmn,true);
			if (illusionpoke != null) {
				(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite).setPokemonBitmap(illusionpoke,true);
			}
			//sendout=new PokeballPlayerSendOutAnimation(@sprites[$"pokemon{battlerindex}"],
			sendout.initialize(@sprites[$"pokemon{battlerindex}"] as IPokemonBattlerSprite,
				@sprites,@battle.battlers[battlerindex],illusionpoke,@battle.doublebattle);
			do  //;loop
			{
				if (fadeanim != null) fadeanim.update();
				frame += 1;
				if (frame > 1 && !pictureBall.running && !(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).appearing) {
					(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).appear();
				}
				if (frame >= 3 && !pictureBall.running) {
					sendout.update();
				}
				if ((frame >= 10 || fadeanim == null) && pictureBall.running) {
					pictureBall.update();
					setPictureIconSprite(spriteBall, pictureBall);
				}
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if ((fadeanim == null || fadeanim.animdone) && sendout.animdone &&
					!(@sprites[$"battlebox{battlerindex}"] as IPokemonDataBox).appearing) break;
				yield return null;
			} while (true);
			spriteBall.Dispose();
			sendout.Dispose();
			if (@battle.battlers[battlerindex].IsShiny && @battle.battlescene) {
				CommonAnimation("Shiny",@battle.battlers[battlerindex],null);
			}
			if (@showingplayer) {
				@showingplayer=false;
				player.visible=false;				//DisposeSprite(@sprites,"player");
				this.partybarplayer.visible=false;	//DisposeSprite(@sprites,"partybarplayer");
				for (int i = 0; i < 6; i++) {
					//DisposeSprite(@sprites,$"player{i}");
					(@sprites[$"player{i}"] as IGameObject).visible = false;
				}
			}*/
			Refresh();
			yield break;
		}

		void IPokeBattle_Scene.SendOut(int battlerindex, IPokemon pkmn)
		{
		}

		public IEnumerator TrainerWithdraw(IBattle battle, IBattlerIE pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//TODO hide opponent's pokemon
			Refresh();
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.TrainerWithdraw(IBattle battle, IBattler pkmn)
		{
		}

		public IEnumerator Withdraw(IBattle battle, IBattlerIE pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//TODO hide player's pokemon
			Refresh();
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.Withdraw(IBattle battle, IBattler pkmn)
		{
		}

		public string MoveString(IBattleMove move)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			string ret = Game._INTL("{1}", move.Name);
			string typename = move.Type.ToString(TextScripts.Name);
			if (move.id > 0)
			{
				ret += Game._INTL(" ({1}) PP: {2}/{3}", typename, move.PP, move.TotalPP);
			}
			return ret;
		}

		public IEnumerator BeginAttackPhase()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			SelectBattler(-1, 0); //SelectBattler(-1);
			GraphicsUpdate();
			yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.BeginAttackPhase() { }

		public IEnumerator SafariStart()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			@briefmessage = false;
			//battlebox0 = new SafariDataBox(@battle, @viewport);
			(battlebox0 as ISafariDataBox).initialize(@battle, @viewport);
			(battlebox0).appear();
			do //;loop
			{
				(battlebox0).update();
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (!(battlebox0).appearing) break;
				yield return null;
			} while (true);
			Refresh();
		}

		void IPokeBattle_Scene.SafariStart() { }

		public void ResetCommandIndices()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//reset the frontend command indices to 0;
			@lastcmd = new MenuCommands[] { 0, 0, 0, 0 };
		}

		public void ResetMoveIndex(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//reset the frontend move index to 0;
			@lastmove[index] = 0;
		}

		public IEnumerator SafariCommandMenu(int index, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			yield return CommandMenuEx(index,new string[] {
				Game._INTL("What will\n{1} throw?", @battle.Player().name),
				Game._INTL("Ball"),
				Game._INTL("Bait"),
				Game._INTL("Rock"),
				Game._INTL("Run")
			},2,result); //result: value => result?.Invoke(value)
			Core.Logger.Log($"Action Selected: {result}");
		}

		int IPokeBattle_Scene.SafariCommandMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//return CommandMenuEx(index,new string[] {
			//	Game._INTL("What will\n{1} throw?", @battle.Player().name),
			//	Game._INTL("Ball"),
			//	Game._INTL("Bait"),
			//	Game._INTL("Rock"),
			//	Game._INTL("Run")
			//},2);
			int ret = -1;
			StartCoroutine(SafariCommandMenu(index,result:value=>ret=value));
			return ret;
		}

		/// <summary>
		/// Use this method to display the list of commands.
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Return values: 0=Fight, 1=Bag, 2=Pokémon, 3=Run, 4=Call</returns>
		public IEnumerator CommandMenu(int index, System.Action<MenuCommands> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = (@battle.opponent != null); //hasConst(Types,:SHADOW) &&
			int ret = -1;
			Core.Logger.Log("Choose Action: [FIGHT=0] [BAG=1] [POKEMON=2] [RUN=3] [CALL=4]");
			yield return CommandMenuEx(index,new string[] {
				Game._INTL("What will\n{1} do?", @battle.battlers[index].Name),
				Game._INTL("Fight"),//MenuCommands.FIGHT=0
				Game._INTL("Bag"),//MenuCommands.BAG=1
				Game._INTL("Pokémon"),//MenuCommands.POKEMON=2
				shadowTrainer ? Game._INTL("Call") : Game._INTL("Run") //MenuCommands.CALL=4|MenuCommands.RUN=3
			},shadowTrainer ? 1 : 0, result: value => ret = value);
			if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			Core.Logger.Log($"Action Selected: {ret}");
			result?.Invoke((MenuCommands)ret);
		}

		int IPokeBattle_SceneNonInteractive.CommandMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool shadowTrainer = (@battle.opponent != null); //hasConst(Types,:SHADOW) &&
			Core.Logger.Log("Choose Action: [FIGHT=0] [BAG=1] [POKEMON=2] [RUN=3] [CALL=4]");
			int ret = CommandMenuEx(index,new string[] {
				Game._INTL("What will\n{1} do?", @battle.battlers[index].Name),
				Game._INTL("Fight"),
				Game._INTL("Bag"),
				Game._INTL("Pokémon"),
				shadowTrainer ? Game._INTL("Call") : Game._INTL("Run")
			},shadowTrainer ? 1 : 0);
			if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			Core.Logger.Log($"Action Selected: {ret}");
			return ret;
		}

		int IPokeBattle_DebugSceneNoGraphics.CommandMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//bool shadowTrainer = (@battle.opponent != null); //hasConst(Types,:SHADOW) &&
			//int ret = CommandMenuEx(index,new string[] {
			//	Game._INTL("What will\n{1} do?", @battle.battlers[index].Name),
			//	Game._INTL("Fight"),
			//	Game._INTL("Bag"),
			//	Game._INTL("Pokémon"),
			//	shadowTrainer ? Game._INTL("Call") : Game._INTL("Run")
			//},shadowTrainer ? 1 : 0);
			//if (ret == 3 && shadowTrainer) ret = 4; // Convert "Run" to "Call"
			//Core.Logger.Log($"Action Selected: {ret}");
			int ret = -1;
			StartCoroutine(CommandMenu(index,result:value=>ret=(int)value));
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
		public IEnumerator CommandMenuEx(int index, string[] texts, int mode, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			ShowWindow(COMMANDBOX);
			ICommandMenuDisplay cw = commandwindow;
			cw.setTexts(texts);
			cw.index = (int)@lastcmd[index];
			cw.mode = mode;
			SelectBattler(index, mode); //SelectBattler(index);
			Refresh();
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate(cw);
				//  Update selected command
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index -= 1;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index += 1;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index -= 2;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index += 2;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))	// Confirm choice
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					int ret = cw.index;
					@lastcmd[index] = (MenuCommands)ret;
					return ret;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && index == 2 && @lastcmd[0] != (MenuCommands)2)	// Cancel
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					return -1;
				}
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("CommandMenuIE");
			yield return StartCoroutine(CommandMenuIE(index, texts, mode, cw, r => _coroutineValue = r));
			result?.Invoke(_coroutineValue);
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
		private int CommandMenuEx(int index, string[] texts, int mode)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			ShowWindow(COMMANDBOX);
			ICommandMenuDisplay cw = commandwindow;
			cw.setTexts(texts);
			cw.index = (int)@lastcmd[index];
			cw.mode = mode;
			SelectBattler(index, mode); //SelectBattler(index);
			Refresh();
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate(cw);
				//  Update selected command
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index -= 1;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index += 1;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index -= 2;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index += 2;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))	// Confirm choice
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					int ret = cw.index;
					@lastcmd[index] = (MenuCommands)ret;
					return ret;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && index == 2 && @lastcmd[0] != (MenuCommands)2)	// Cancel
				{
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					return -1;
				}
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("CommandMenuIE");
			StartCoroutine(CommandMenuIE(index, texts, mode, cw, r => _coroutineValue = r));
			return (int)_coroutineValue;
		}

		private IEnumerator CommandMenuIE(int index, string[] texts, int mode, ICommandMenuDisplay cw, System.Action<int> result)
		{
			do //;loop
			{
				//GraphicsUpdate();
				//InputUpdate();
				FrameUpdate(cw as IGameObject);
				//  Update selected command
				//if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				//if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.LeftArrow) && (cw.index & 1) == 1)
				//{
				//	Core.Logger.Log("[CommandMenu] Going Left !");
				//	if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				//	cw.index -= 1;
				//}
				////else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.RightArrow) && (cw.index & 1) == 0)
				//{
				//	Core.Logger.Log("[CommandMenu] Going Right !");
				//	if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				//	cw.index += 1;
				//}
				////else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.UpArrow) && (cw.index & 2) == 2)
				if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.UpArrow) && cw.index > 0)
				{
					Core.Logger.Log("[CommandMenu] Going Up !");
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index--; //-= 2;
				}
				//else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.DownArrow) && (cw.index & 2) == 0)
				else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.DownArrow) && cw.index < 3)
				{
					Core.Logger.Log("[CommandMenu] Going Down !");
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
					cw.index++; //+= 2;
				}
				//if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))   // Confirm choice
				if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Space))   // Confirm choice
				{
					Core.Logger.Log("[CommandMenu] Select !");
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					int ret = cw.index;
					@lastcmd[index] = (MenuCommands)ret;
					//return ret;
					result(ret); break;
				}
				//else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B) && index == 2 && @lastcmd[0] != (MenuCommands)2)  // Cancel
				else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Escape) && index == 2 && @lastcmd[0] != (MenuCommands)2)  // Cancel
				{
					Core.Logger.Log("[CommandMenu] Cancel !");
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					//return -1;
					result(-1); break;
				}
				yield return null;
			} while (true);
		}

		/// <summary>
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public IEnumerator FightMenu(int index, Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			ShowWindow(FIGHTBOX);
			IFightMenuDisplay cw = fightwindow;
			IBattlerIE battler = @battle.battlers[index];
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
			if (@battle.CanMegaEvolve(index)) cw.megaButton = 1;
			SelectBattler(index, 0); //SelectBattler(index);
			Refresh();
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate(cw);
				//  Update selected command
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				{
					if (cw.setIndex(cw.index - 1)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				{
					if (cw.setIndex(cw.index + 1)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				{
					if (cw.setIndex(cw.index - 2)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				{
					if (cw.setIndex(cw.index + 2)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))       // Confirm choice
				{
					int ret = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					@lastmove[index] = ret;
					return ret;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.CTRL))       // Use Mega Evolution
				{
					if (@battle.CanMegaEvolve(index))
					{
						@battle.RegisterMegaEvolution(index);
						cw.megaButton = 2;
						if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					}
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))       // Cancel fight menu
				{
					@lastmove[index] = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCancelSE();
					return -1;
				}
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("FightMenuIE");
			yield return StartCoroutine(FightMenuIE(index, cw, r => _coroutineValue = r));
			result?.Invoke(_coroutineValue);
		}

		public int FightMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			ShowWindow(FIGHTBOX);
			IFightMenuDisplay cw = fightwindow;
			IBattlerIE battler = @battle.battlers[index];
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
			if (@battle.CanMegaEvolve(index)) cw.megaButton = 1;
			SelectBattler(index, 0); //SelectBattler(index);
			Refresh();
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate(cw);
				//  Update selected command
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				{
					if (cw.setIndex(cw.index - 1)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				{
					if (cw.setIndex(cw.index + 1)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				{
					if (cw.setIndex(cw.index - 2)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				{
					if (cw.setIndex(cw.index + 2)) if (AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))       // Confirm choice
				{
					int ret = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					@lastmove[index] = ret;
					return ret;
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.CTRL))       // Use Mega Evolution
				{
					if (@battle.CanMegaEvolve(index))
					{
						@battle.RegisterMegaEvolution(index);
						cw.megaButton = 2;
						if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					}
				}
				else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))       // Cancel fight menu
				{
					@lastmove[index] = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCancelSE();
					return -1;
				}
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("FightMenuIE");
			StartCoroutine(FightMenuIE(index, cw, r => _coroutineValue = r));
			return (int)_coroutineValue;
		}

		/// <summary>
		/// Use this method to display the list of moves for a Pokémon
		/// </summary>
		/// <param name="index"></param>
		/// <returns>Callbacks an int</returns>
		private IEnumerator FightMenuIE(int index, IFightMenuDisplay cw, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//All of this below should be a coroutine that returns the value selected in UI
			do //;loop
			{
				//GraphicsUpdate();
				//InputUpdate();
				FrameUpdate(cw as IGameObject);
				//  Update selected command
				//if (PokemonUnity.Input.trigger(PokemonUnity.Input.LEFT) && (cw.index & 1) == 1)
				//if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.LeftArrow) && (cw.index & 1) == 1)
				//{
				//	Core.Logger.Log("[CommandMenu] Going Left !");
				//	cw.setIndex(cw.index - 1);
				//	if (cw.setIndex(cw.index - 1) && AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				//}
				////else if (PokemonUnity.Input.trigger(PokemonUnity.Input.RIGHT) && (cw.index & 1) == 0)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.RightArrow) && (cw.index & 1) == 0)
				//{
				//	Core.Logger.Log("[CommandMenu] Going Right !");
				//	cw.setIndex(cw.index + 1);
				//	if (cw.setIndex(cw.index + 1) && AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				//}
				////else if (PokemonUnity.Input.trigger(PokemonUnity.Input.UP) && (cw.index & 2) == 2)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.UpArrow) && (cw.index & 2) == 2)
				if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.UpArrow) && cw.index > 0)
				{
					Core.Logger.Log("[CommandMenu] Going Up !");
					cw.setIndex(cw.index - 1); //- 2
					if (cw.setIndex(cw.index - 1) &&  //- 2
						AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				//else if (PokemonUnity.Input.trigger(PokemonUnity.Input.DOWN) && (cw.index & 2) == 0)
				//else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.DownArrow) && (cw.index & 2) == 0)
				else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.DownArrow) && cw.index < 3)
				{
					Core.Logger.Log("[CommandMenu] Going Down !");
					cw.setIndex(cw.index + 1); //+ 2
					if (cw.setIndex(cw.index + 1) && //+ 2
						AudioHandler is IGameAudioPlay gap) gap.PlayCursorSE();
				}
				//if (PokemonUnity.Input.trigger(PokemonUnity.Input.A))				// Confirm choice
				if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Space))		// Confirm choice
				{
					Core.Logger.Log("[CommandMenu] Select !");
					int ret = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					@lastmove[index] = ret;
					result(ret);
					break;
				}
				//else if (PokemonUnity.Input.trigger(PokemonUnity.Input.CTRL))		// Use Mega Evolution
				else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.RightControl))
				{
					Core.Logger.Log("[CommandMenu] Mega !");
					if (@battle.CanMegaEvolve(index))
					{
						@battle.RegisterMegaEvolution(index);
						cw.megaButton = 2;
						if (AudioHandler is IGameAudioPlay gap) gap.PlayDecisionSE();
					}
				}
				//else if (PokemonUnity.Input.trigger(PokemonUnity.Input.B))			// Cancel fight menu
				else if (global::UnityEngine.Input.GetKeyDown(global::UnityEngine.KeyCode.Escape))
				{
					Core.Logger.Log("[CommandMenu] Cancel !");
					@lastmove[index] = cw.index;
					if (AudioHandler is IGameAudioPlay gap) gap.PlayCancelSE();
					result(-1);
					break;
				}

				yield return null;
			} while (true);
		}

		public IEnumerator ItemMenu(int index, System.Action<Items> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Items ret = Items.NONE;
			//int retindex = -1;
			//int pkmnid = -1;
			bool endscene = true;
			//oldsprites = FadeOutAndHide(@sprites); //Disable current Scene from view after fading to black
			//IPokemonBagScene itemscene = new PokemonBag_Scene();
			IBagScene itemscene = Game.GameData.Scenes.Bag.initialize();
			itemscene.StartScene(Game.GameData.Bag);
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				Items item = itemscene.ChooseItem();
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
				int command = itemscene.ShowCommands(Game._INTL("{1} is selected.", itemname), commands);
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
						itemscene.EndScene();
						pkmnscreen.StartScene(Game._INTL("Use on which Pokémon?"), @battle.doublebattle);
						int activecmd = pkmnscreen.ChoosePokemon();
						pkmnid = @battle.party1order[activecmd];
						if (activecmd >= 0 && pkmnid >= 0 && ItemHandlers.hasBattleUseOnPokemon(item))
						{
							pkmnlist.EndScene();
							ret = item;
							retindex = pkmnid;
							endscene = false;
							break;
						}
						pkmnlist.EndScene();
						itemscene.StartScene(Game.GameData.Bag);
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
				yield return null;
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("ItemMenuIE");
			StartCoroutine(ItemMenuIE(index, itemscene, (item, pokemon, sceneEnd) => { _coroutineValue = (int)item; endscene = sceneEnd; }));
			ret = _coroutineValue > 0 ? (Items)_coroutineValue : ret;

			if (ret > 0) (Game.GameData as Game).ConsumeItemInBattle(Game.GameData.Bag, ret);
			if (endscene) itemscene.EndScene();
			//FadeInAndShow(@sprites, oldsprites); //Return previous scene, with all of the old data after fading from black
			//return [ret, retindex];
			//return new KeyValuePair<Items,int> (ret, retindex);
			result?.Invoke(ret);
			yield break;
		}

		public Items ItemMenu(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Items ret = Items.NONE;
			//int retindex = -1;
			//int pkmnid = -1;
			bool endscene = true;
			//oldsprites = FadeOutAndHide(@sprites); //Disable current Scene from view after fading to black
			//IPokemonBagScene itemscene = new PokemonBag_Scene();
			IBagScene itemscene = Game.GameData.Scenes.Bag.initialize();
			itemscene.StartScene(Game.GameData.Bag);
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				Items item = itemscene.ChooseItem();
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
				int command = itemscene.ShowCommands(Game._INTL("{1} is selected.", itemname), commands);
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
						itemscene.EndScene();
						pkmnscreen.StartScene(Game._INTL("Use on which Pokémon?"), @battle.doublebattle);
						int activecmd = pkmnscreen.ChoosePokemon();
						pkmnid = @battle.party1order[activecmd];
						if (activecmd >= 0 && pkmnid >= 0 && ItemHandlers.hasBattleUseOnPokemon(item))
						{
							pkmnlist.EndScene();
							ret = item;
							retindex = pkmnid;
							endscene = false;
							break;
						}
						pkmnlist.EndScene();
						itemscene.StartScene(Game.GameData.Bag);
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
			} while (true);*/
			int _coroutineValue = -1;
			StopCoroutine("ItemMenuIE");
			StartCoroutine(ItemMenuIE(index, itemscene, (item, pokemon, sceneEnd) => { _coroutineValue = (int)item; endscene = sceneEnd; }));
			ret = _coroutineValue > 0 ? (Items)_coroutineValue : ret;

			if (ret > 0) (Game.GameData as Game).ConsumeItemInBattle(Game.GameData.Bag, ret);
			if (endscene) itemscene.EndScene();
			//FadeInAndShow(@sprites, oldsprites); //Return previous scene, with all of the old data after fading from black
			//return [ret, retindex];
			//return new KeyValuePair<Items,int> (ret, retindex);
			return ret;
		}

		private IEnumerator ItemMenuIE(int index, IBagScene itemscene, System.Action<Items,int,bool> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			Items ret = Items.NONE;
			int retindex = -1;
			int pkmnid = -1;
			bool endscene = true;
			//All of this below should be a coroutine that returns the value selected in UI
			do //;loop
			{
				Items item = itemscene.ChooseItem();
				if (item == 0) break;
				//int usetype =$ItemData[item][ITEMBATTLEUSE];
				int cmdUse = -1;
				IList<string> commands = new List<string>();
				if (!Kernal.ItemData[item].BattleUse) //(usetype == 0)
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
				int command = itemscene.ShowCommands(Game._INTL("{1} is selected.", itemname), commands);
				if (cmdUse >= 0 && command == cmdUse)
				{
					if (Kernal.ItemData[item].Flags.Consumable || Kernal.ItemData[item].Flags.Useable_In_Battle) //(usetype == 1 || usetype == 3)
					{
						IList<IPokemon> modparty = new List<IPokemon>();
						for (int i = 0; i < 6; i++)
						{
							modparty.Add(@battle.party1[@battle.party1order[i]]);
						}
						//pkmnlist = new PokemonScreen_Scene();
						//pkmnscreen = new PokemonScreen(pkmnlist, modparty);
						IPartyDisplayScene pkmnlist = Game.GameData.Scenes.Party;//.initialize();
						IPartyDisplayScreen pkmnscreen = Game.GameData.Screens.Party.initialize(pkmnlist, modparty);
						itemscene.EndScene();
						pkmnscreen.StartScene(Game._INTL("Use on which Pokémon?"), @battle.doublebattle);
						int activecmd = pkmnscreen.ChoosePokemon();
						pkmnid = @battle.party1order[activecmd];
						if (activecmd >= 0 && pkmnid >= 0 && ItemHandlers.hasBattleUseOnPokemon(item))
						{
							pkmnlist.EndScene();
							ret = item;
							retindex = pkmnid;
							endscene = false;
							result(ret, retindex, endscene); break;
						}
						pkmnlist.EndScene();
						itemscene.StartScene(Game.GameData.Bag);
					}
					else if (Kernal.ItemData[item].Flags.Useable_Overworld || !Kernal.ItemData[item].Flags.Countable) //(usetype == 2 || usetype == 4)
					{
						if (ItemHandlers.hasBattleUseOnBattler(item))
						{
							ret = item;
							retindex = index;
							result(ret, retindex, endscene); break;
						}
					}
				}
				yield return null;
			} while (true);
		}

		public IEnumerator ForgetMove(IPokemon pokemon, Moves moveToLearn, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int ret = -1;
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				//IPokemonSummaryScene scene = new PokemonSummaryScene();
				//IPokemonSummaryScreen screen = new PokemonSummary(scene);
				IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
				IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
				ret = screen.StartForgetScreen(pokemon,0,moveToLearn);
			});
			result?.Invoke(ret);
			yield break;
		}

		public int ForgetMove(IPokemon pokemon, Moves moveToLearn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int ret = -1;
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				//IPokemonSummaryScene scene = new PokemonSummaryScene();
				//IPokemonSummaryScreen screen = new PokemonSummary(scene);
				IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
				IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
				ret = screen.StartForgetScreen(pokemon,0,moveToLearn);
			});
			return ret;
		}

		/// <summary>
		/// Called whenever a Pokémon needs one of its moves chosen. Used for Ether.
		/// </summary>
		/// <param name="pokemon"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public IEnumerator ChooseMove(IPokemon pokemon, string message, Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int ret = -1;
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				//IPokemonSummaryScene scene = new PokemonSummaryScene();
				//IPokemonSummaryScreen screen = new PokemonSummary(scene);
				IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
				IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
				ret = screen.StartChooseMoveScreen(pokemon,0,message);
			});
			result?.Invoke(ret);
			yield break;
		}

		public int ChooseMove(IPokemon pokemon, string message)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int ret = -1;
			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				//IPokemonSummaryScene scene = new PokemonSummaryScene();
				//IPokemonSummaryScreen screen = new PokemonSummary(scene);
				IPokemonSummaryScene scene = Game.GameData.Scenes.Summary.initialize();
				IPokemonSummaryScreen screen = Game.GameData.Screens.Summary.initialize(scene);
				ret = screen.StartChooseMoveScreen(pokemon,0,message);
			});
			return ret;
		}

		public IEnumerator NameEntry(string helptext, IPokemon pokemon, System.Action<string> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//yield return EnterPokemonName(helptext, 0, 10, "", pokemon);
			result?.Invoke(pokemon.Name);
			yield break;
		}

		public string NameEntry(string helptext, IPokemon pokemon)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//return EnterPokemonName(helptext, 0, 10, "", pokemon);
			return pokemon.Name;
		}

		public void SelectBattler(int index, int selectmode)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int numwindows = 1; //@battle.doublebattle ? 4 : 2;
			for (int i = 0; i < numwindows; i++)
			{
				IPokemonDataBox sprite0 = @sprites[$"battlebox{i}"] as IPokemonDataBox;
				sprite0.selected = (i == index) ? selectmode : 0;
				//IPokemonBattlerSprite sprite1 = @sprites[$"pokemon{i}"] as IPokemonBattlerSprite;
				//sprite1.selected = (i == index) ? selectmode : 0;
			}
		}

		public int FirstTarget(int index, Targets targettype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			switch (targettype)
			{
				//case Targets.UserOrPartner:
				case Targets.USER:
				case Targets.USER_OR_ALLY:
					return index;
				case Targets.ALLY:
					//number of targets to select from is usually a max of 4 if double battle...
					for (int i = 0; i < 4; i++)
					{
						if (i != index && !@battle.battlers[i].isFainted() &&
						   !@battle.battlers[index].IsOpposing(i)) //returns index of first ally pokemon
						{
							return i;
						}
					}
					break;
				//case Targets.SingleNonUser:
				case Targets.RANDOM_OPPONENT:
				case Targets.SELECTED_POKEMON_ME_FIRST:
				case Targets.SELECTED_POKEMON: //Use ui to select from scene?
					//number of targets to select from is usually a max of 4 if double battle...
					for (int i = 0; i < 4; i++)
					{
						if (i != index && !@battle.battlers[i].isFainted() &&
						   @battle.battlers[index].IsOpposing(i))
						{
							return i;
						}
					}
					break;
				case Targets.SPECIFIC_MOVE: //if the pokemon possess a specific move in moveset?
				//	//number of targets to select from is usually a max of 4 if double battle...
				//	for (int i = 0; i < 4; i++)
				//	{
				//		if (i != index && !@battle.battlers[i].isFainted() &&
				//		   @battle.battlers[index].IsOpposing(i))
				//		{
				//			return i;
				//		}
				//	}
				//	break;
				case Targets.USERS_FIELD:
				case Targets.USER_AND_ALLIES:
				case Targets.OPPONENTS_FIELD:
				case Targets.ALL_OTHER_POKEMON:
				case Targets.ALL_OPPONENTS:
				case Targets.ALL_POKEMON:
				case Targets.ENTIRE_FIELD:
				default:
					break;
			}
			return -1;
		}

		public void UpdateSelected(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int numwindows = @battle.doublebattle ? 4 : 2;
			for (int i = 0; i < numwindows; i++)
			{
				if (i == index)
				{
					(@sprites[$"battlebox{i}"] as IPokemonDataBox).selected = 2;
					(@sprites[$"pokemon{i}"] as IPokemonBattlerSprite).selected = 2;
				}
				else
				{
					(@sprites[$"battlebox{i}"] as IPokemonDataBox).selected = 0;
					(@sprites[$"pokemon{i}"] as IPokemonBattlerSprite).selected = 0;
				}
				(@sprites[$"battlebox{i}"] as IPokemonDataBox).update();
				(@sprites[$"pokemon{i}"] as IPokemonBattlerSprite).update();
			}
		}

		public IEnumerator ChooseTarget(int index, Targets targettype, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			ShowWindow(FIGHTBOX);
			IFightMenuDisplay cw = fightwindow;
			IBattlerIE battler = @battle.battlers[index];
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

			int curwindow = FirstTarget(index, targettype);
			//int newcurwindow = -1;
			if (curwindow == -1)
			{
				//throw new RuntimeError(Game._INTL("No targets somehow..."));
				Core.Logger.LogError(Game._INTL("No targets somehow..."));
			}
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				UpdateSelected(curwindow);
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
					return curwindow;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
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
			int _coroutineValue = -1;
			StopCoroutine("ChooseTargetIE");
			yield return StartCoroutine(ChooseTargetIE(index, targettype, curwindow, r => _coroutineValue = r));
			result?.Invoke(_coroutineValue);
		}

		int IPokeBattle_SceneNonInteractive.ChooseTarget(int index, Targets targettype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			ShowWindow(FIGHTBOX);
			IFightMenuDisplay cw = fightwindow;
			IBattlerIE battler = @battle.battlers[index];
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

			int curwindow = FirstTarget(index, targettype);
			//int newcurwindow = -1;
			if (curwindow == -1)
			{
				//throw new RuntimeError(Game._INTL("No targets somehow..."));
				Core.Logger.LogError(Game._INTL("No targets somehow..."));
			}
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				UpdateSelected(curwindow);
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
					return curwindow;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
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
			int _coroutineValue = -1;
			StopCoroutine("ChooseTargetIE");
			StartCoroutine(ChooseTargetIE(index, targettype, curwindow, r => _coroutineValue = r));
			return (int)_coroutineValue;
		}

		int IPokeBattle_DebugSceneNoGraphics.ChooseTarget(int index, Targets targettype)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			ShowWindow(FIGHTBOX);
			IFightMenuDisplay cw = fightwindow;
			IBattlerIE battler = @battle.battlers[index];
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

			int curwindow = FirstTarget(index, targettype);
			//int newcurwindow = -1;
			if (curwindow == -1)
			{
				//throw new RuntimeError(Game._INTL("No targets somehow..."));
				Core.Logger.LogError(Game._INTL("No targets somehow..."));
			}
			//All of this below should be a coroutine that returns the value selected in UI
			/*do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				UpdateSelected(curwindow);
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
					return curwindow;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
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
			int _coroutineValue = -1;
			StopCoroutine("ChooseTargetIE");
			StartCoroutine(ChooseTargetIE(index, targettype, curwindow, r => _coroutineValue = r));
			return (int)_coroutineValue;
		}

		private IEnumerator ChooseTargetIE(int index, Targets targettype, int curwindow, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*int newcurwindow = -1;
			//All of this below should be a coroutine that returns the value selected in UI
			do //;loop
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				UpdateSelected(curwindow);
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
					//return curwindow;
					result(curwindow); break;
				}
				if (PokemonUnity.Input.trigger(PokemonUnity.Input.t))
				{
					UpdateSelected(-1);
					//return -1;
					result(-1); break;
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

			yield return null;
		}

		public int Switch(int index, bool lax, bool cancancel)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int _coroutineValue = -1;
			StopCoroutine("Switch");
			StartCoroutine(Switch(index, lax, cancancel, r => _coroutineValue = r));
			return (int)_coroutineValue;
		}

		public IEnumerator Switch(int index, bool lax, bool cancancel, System.Action<int> result)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			int ret = -1;
			IPokemon[] party = @battle.Party(index);
			IList<int> partypos = @battle.party1order;
			//  Fade out and hide all sprites
			/*visiblesprites = FadeOutAndHide(@sprites);
			ShowWindow(BLANK);
			SetMessageMode(true);
			IList<IPokemon> modparty = new List<IPokemon>();
			for (int i = 0; i < 6; i++)
			{
				modparty.Add(party[partypos[i]]);
			}
			//IPartyDisplayScene scene = new PokemonScreen_Scene();
			//@switchscreen = new PokemonScreen(scene, modparty);
			IPartyDisplayScene scene = Game.GameData.Scenes.Party.initialize();
			@switchscreen = Game.GameData.Screens.Party.initialize(scene, modparty);
			yield return @switchscreen.StartScene(Game._INTL("Choose a Pokémon."),
			   @battle.doublebattle && !@battle.fullparty1);
			//All of this below should be a coroutine that returns the value selected in UI
			do //;loop
			{
				scene.SetHelpText(Game._INTL("Choose a Pokémon."));
				int activecmd = @switchscreen.ChoosePokemon();
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
					int command = yield return scene.ShowCommands(Game._INTL("Do what with {1}?", party[pkmnindex].Name), commands.ToArray());
					if (cmdShift >= 0 && command == cmdShift)
					{
						bool canswitch = lax ? @battle.CanSwitchLax(index, pkmnindex, true) :
						   @battle.CanSwitch(index, pkmnindex, true);
						if (canswitch)
						{
							ret = pkmnindex;
							break;
						}
					}
					else if (cmdSummary >= 0 && command == cmdSummary)
					{
						yield return scene.Summary(activecmd);
					}
				}
				yield return null;
			} while (true);
			@switchscreen.EndScene();
			@switchscreen = null;
			ShowWindow(BLANK);
			SetMessageMode(false);
			//  back to main battle screen
			FadeInAndShow(@sprites, visiblesprites);*/
			result(ret);
			yield return null;
		}

		public IEnumerator DamageAnimation(IBattlerIE pkmn, TypeEffective effectiveness)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			IPokemonBattlerSprite pkmnsprite = @sprites[$"pokemon{pkmn.Index}"] as IPokemonBattlerSprite;
			IIconSprite shadowsprite = @sprites[$"shadow{pkmn.Index}"] as IIconSprite;
			IPokemonDataBox sprite = @sprites[$"battlebox{pkmn.Index}"] as IPokemonDataBox;
			bool oldshadowvisible = shadowsprite.visible;
			bool oldvisible = sprite.visible;
			sprite.selected = 3;
			@briefmessage = false;
			int i = 0; do //wait 6 frame ticks...
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				yield return null;
			} while (i < 6); //6.times
			switch (effectiveness)
			{
				case TypeEffective.NormalEffective: //0:
					if (AudioHandler is IGameAudioPlay gap1) gap1.SEPlay("normaldamage");
					break;
				case TypeEffective.NotVeryEffective: //1:
					if (AudioHandler is IGameAudioPlay gap2) gap2.SEPlay("notverydamage");
					break;
				case TypeEffective.SuperEffective: //2:
					if (AudioHandler is IGameAudioPlay gap3) gap3.SEPlay("superdamage");
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
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate();
					sprite.update(); j++;
					yield return null;
				} while (j < 4); i++;//4.times
			} while (i < 8); //8.times
			sprite.selected = 0;
			sprite.visible = oldvisible;
		}

		void IPokeBattle_DebugSceneNoGraphics.DamageAnimation(IBattler pkmn, TypeEffective effectiveness) { }

		/// <summary>
		/// This method is called whenever a Pokémon's HP changes.
		/// Used to animate the HP bar.
		/// </summary>
		/// <param name="pkmn"></param>
		/// <param name="oldhp"></param>
		/// <param name="anim"></param>
		public IEnumerator HPChanged(IBattlerIE pkmn, int oldhp, bool anim)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			@briefmessage = false;
			int hpchange = pkmn.HP - oldhp;
			if (hpchange < 0)
			{
				hpchange = -hpchange;
				Core.Logger.Log($"[HP change] #{pkmn.ToString()} lost #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			else
			{
				Core.Logger.Log($"[HP change] #{pkmn.ToString()} gained #{hpchange} HP (#{oldhp}=>#{pkmn.HP})");
			}
			if (anim && @battle.battlescene)
			{
				if (pkmn.HP > oldhp)
				{
					yield return CommonAnimation("HealthUp", pkmn, null);
				}
				else if (pkmn.HP < oldhp)
				{
					yield return CommonAnimation("HealthDown", pkmn, null);
				}
			}
			IPokemonDataBox sprite = @sprites[$"battlebox{pkmn.Index}"] as IPokemonDataBox;
			sprite.animateHP(oldhp, pkmn.HP);
			while (sprite.animatingHP)
			{
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				sprite.update();
				yield return null;
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.HPChanged(IBattler pkmn, int oldhp, bool anim) { }

		/// <summary>
		/// This method is called whenever a Pokémon faints.
		/// </summary>
		/// <param name="pkmn"></param>
		public IEnumerator Fainted(IBattlerIE pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*int frames = CryFrameLength(pkmn.pokemon);
			PlayCry(pkmn.pokemon);
			int i = 0; do {
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate(); i++;
			} while (i < frames); //frames.times
			@sprites[$"shadow{pkmn.Index}"].visible=false;
			IPokemonBattlerSprite pkmnsprite = @sprites[$"pokemon{pkmn.Index}"] as IPokemonBattlerSprite;
			int ycoord=0;
			if (@battle.doublebattle) {
				if (pkmn.Index==0) ycoord=PokeBattle_SceneConstants.PLAYERBATTLERD1_Y;
				if (pkmn.Index==1) ycoord=PokeBattle_SceneConstants.FOEBATTLERD1_Y;
				if (pkmn.Index==2) ycoord=PokeBattle_SceneConstants.PLAYERBATTLERD2_Y;
				if (pkmn.Index==3) ycoord=PokeBattle_SceneConstants.FOEBATTLERD2_Y;
			} else
			{
				if (@battle.IsOpposing(pkmn.Index))
				{
					ycoord = PokeBattle_SceneConstants.FOEBATTLER_Y;
				}
				else
				{
					ycoord = PokeBattle_SceneConstants.PLAYERBATTLER_Y;
				}
			}
			if (AudioHandler is IGameAudioPlay gap) gap.SEPlay("faint");
			do //;loop
			{
				pkmnsprite.y += 8;
				if (pkmnsprite.y - pkmnsprite.oy + pkmnsprite.src_rect.height >= ycoord)
				{
					pkmnsprite.src_rect.height = ycoord - pkmnsprite.y + pkmnsprite.oy;
				}
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (pkmnsprite.y >= ycoord) break;
			} while (true);

			pkmnsprite.visible = false;
			i = 0; do
			{
				(@sprites[$"battlebox{pkmn.Index}"] as IPokemonDataBox).opacity -= 32;
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
			} while (i < 8); //8.times
			(@sprites[$"battlebox{pkmn.Index}"] as IPokemonDataBox).visible = false;
			pkmn.ResetForm();*/
			yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.Fainted(IBattler pkmn)
		{
		}

		/// <summary>
		/// Use this method to choose a command for the enemy.
		/// </summary>
		/// <param name="index"></param>
		public void ChooseEnemyCommand(int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if(@battle is IBattleAI b) b.DefaultChooseEnemyCommand(index);
		}

		/// <summary>
		/// Use this method to choose a new Pokémon for the enemy
		/// The enemy's party is guaranteed to have at least one choosable member.
		/// </summary>
		/// <param name="index"></param>
		/// <param name="party"></param>
		/// <returns></returns>
		public int ChooseNewEnemy(int index, IPokemon[] party)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (@battle is IBattleAI b) return b.DefaultChooseNewEnemy(index, party);
			else Core.Logger.LogError("Need to Configure Battle AI to Select Next Pokemon for Computer...");
			return -1;
		}

		public IEnumerator WildBattleSuccess()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//yield return if (AudioHandler is IGameAudioPlay gap) gap.BGMPlay(GetWildVictoryME());
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.WildBattleSuccess() { }

		public IEnumerator TrainerBattleSuccess()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//yield retun if (AudioHandler is IGameAudioPlay gap) gap.BGMPlay(GetTrainerVictoryME(@battle.opponent));
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.TrainerBattleSuccess() { }

		public IEnumerator EXPBar(IBattlerIE battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (battler != null)
			{
				(@sprites[$"battlebox{battler.Index}"] as IPokemonDataBox).refreshExpLevel();
				//int exprange = (endexp - startexp);
				//int startexplevel = 0;
				//int endexplevel = 0;
				//if (exprange != 0)
				//{
				//	startexplevel = (tempexp1 - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / exprange;
				//	endexplevel = (tempexp2 - startexp) * PokeBattle_SceneConstants.EXPGAUGESIZE / exprange;
				//}
				//(@sprites[$"battlebox{battler.Index}"] as IPokemonDataBox).animateEXP(startexplevel, endexplevel);
				(@sprites[$"battlebox{battler.Index}"] as IPokemonDataBox).animateEXP(startexp, endexp);
				while ((@sprites[$"battlebox{battler.Index}"] as IPokemonDataBox).animatingEXP)
				{
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate();
					(@sprites[$"battlebox{battler.Index}"] as IPokemonDataBox).update();
					yield return null;
				}
			}
		}

		void IPokeBattle_DebugSceneNoGraphics.EXPBar(IBattler battler, IPokemon thispoke, int startexp, int endexp, int tempexp1, int tempexp2) { }

		public IEnumerator ShowPokedex(Pokemons species, int form)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (Game.GameData is IGameSpriteWindow g) g.FadeOutIn(99999, block: () => {
				//IPokemonPokedexScene scene = new PokemonPokedexScene();
				//IPokemonPokedexScreen screen = new PokemonPokedex(scene);
				IPokemonPokedexScene scene = Game.GameData.Scenes.PokedexScene.initialize();
				IPokemonPokedexScreen screen = Game.GameData.Screens.PokedexScreen.initialize(scene);
				screen.DexEntry(species);
			});
			yield break;
		}

		void IPokeBattle_Scene.ShowPokedex(Pokemons species, int form) { }

		/*public void ChangeSpecies(IBattlerIE attacker, Pokemons species)
		{
			Core.Logger.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			pkmn = @sprites[$"pokemon{attacker.Index}"];
			shadow = @sprites[$"shadow{attacker.Index}"];
			back = !@battle.IsOpposing(attacker.Index);
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

		public void ChangePokemon(IBattler attacker, IPokemon pokemon)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*IPokemonBattlerSprite pkmn = @sprites[$"pokemon{attacker.Index}"] as IPokemonBattlerSprite;
			IIconSprite shadow = @sprites[$"shadow{attacker.Index}"] as IIconSprite;
			bool back = !@battle.IsOpposing(attacker.Index);
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
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		}

		//public void SaveShadows()
		public void SaveShadows(Action action = null)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			IList<bool> shadows = new List<bool>();
			IIconSprite s = null;
			for (int i = 0; i < 4; i++)
			{
				s = @sprites[$"shadow{i}"] as IIconSprite;
				shadows[i] = s != null ? s.visible : false;
				if (s != null) s.visible = false;
			}
			if (action != null) action.Invoke(); //yield return null;
			for (int i = 0; i < 4; i++)
			{
				s = @sprites[$"shadow{i}"] as IIconSprite;
				if (s != null) s.visible = shadows[i];
			}
		}

		public KeyValuePair<string, bool>? FindAnimation(Moves moveid, int userIndex, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
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
				//IMove move = new MoveData(moveid);
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
					//if (isConst(type, Types, i[0]) && hasConst(Moves, i[1]))
					//{
						noflip = false;
						if ((userIndex & 1) == 0)       // On player's side
						{
							//anim = move2anim[0][getConst(Moves, i[1])];
							anim = move2anim[0][i.Value];
						}
						else                  // On opposing side
						{
							//anim = move2anim[1][getConst(Moves, i[1])];
							anim = move2anim[1][i.Value];
							if (anim) noflip = true;
							//if (!anim) anim = move2anim[0][getConst(Moves, i[1])];
							if (!anim) anim = move2anim[0][i.Value];
						}
						if (anim != null) return new KeyValuePair<string,bool>(anim, noflip);
						break;
					//}
				}
				//  Default animation for the move's type not found, use Tackle's animation
				//if (hasConst(Moves,:TACKLE))
				//{
					anim = move2anim[0][Moves.TACKLE];
					if (anim != null) return new KeyValuePair<string,bool>(anim, false);
				//}
			}
			catch (Exception) //rescue;
			{
				return null;
			}*/
			return null;
		}

		public IEnumerator CommonAnimation(string animname, IBattlerIE user, IBattlerIE target, int hitnum = 0)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*animations = load_data("Data/PkmnAnimations.rxdata");
			for (int i = 0; i < animations.Length; i++)
			{
				if (animations[i] && animations[i].name == "Common:" + animname)
				{
					yield return AnimationCore(animations[i], user, (target != null) ? target : user);
					yield break;
				}
			}*/
			yield break;
		}

		public void CommonAnimation(string animname, IBattler user, IBattler target, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*animations = load_data("Data/PkmnAnimations.rxdata");
			for (int i = 0; i < animations.Length; i++)
			{
				if (animations[i] && animations[i].name == "Common:" + animname)
				{
					AnimationCore(animations[i], user, (target != null) ? target : user);
					return;
				}
			}*/
		}

		void IPokeBattle_DebugSceneNoGraphics.CommonAnimation(Moves moveid, IBattler attacker, IBattler opponent, int hitnum) { }

		public IEnumerator Animation(Moves moveid, IBattlerIE user, IBattlerIE target, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*KeyValuePair<string,bool>? animid = FindAnimation(moveid, user.Index, hitnum);
			if (animid==null) yield break;
			string anim = animid.Value.Key;
			IDictionary<string,string> animations = load_data("Data/PkmnAnimations.rxdata");
			SaveShadows(() =>
			{
				if (animid.Value.Value)       // On opposing side and using OppMove animation
				{
					yield return AnimationCore(animations[anim], target, user, true);
				}
				else         // On player's side, and/or using Move animation
				{
					yield return AnimationCore(animations[anim], user, target);
				}
			});
			if (new MoveData(moveid).function == 0x69 && user != null && target != null)      // Transform
			{
				//  Change form to transformed version
				ChangePokemon(user, target.pokemon);
			}*/
			yield break;
		}

		void IPokeBattle_DebugSceneNoGraphics.Animation(Moves moveid, IBattler user, IBattler target, int hitnum)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			/*
			int animid = FindAnimation(moveid, user.Index, hitnum);
			if (!animid) return;
			anim = animid[0];
			animations = load_data("Data/PkmnAnimations.rxdata");
			SaveShadows(() =>
			{
				if (animid[1])       // On opposing side and using OppMove animation
				{
					AnimationCore(animations[anim], target, user, true);
				}
				else         // On player's side, and/or using Move animation
				{
					AnimationCore(animations[anim], user, target);
				}
			});
			if (new MoveData(moveid).function == 0x69 && user != null && target != null)      // Transform
			{
				//  Change form to transformed version
				ChangePokemon(user, target.pokemon);
			}*/
		}

		public IEnumerator AnimationCore(string animation, IBattlerIE user, IBattlerIE target, bool oppmove)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*if (!animation) yield break;
			@briefmessage = false;
			usersprite = (user) ? @sprites[$"pokemon{user.Index}"] : null;
			targetsprite = (target) ? @sprites[$"pokemon{target.Index}"] : null;
			olduserx = usersprite ? usersprite.x : 0;
			oldusery = usersprite ? usersprite.y : 0;
			oldtargetx = targetsprite ? targetsprite.x : 0;
			oldtargety = targetsprite ? targetsprite.y : 0;
			if (!targetsprite)
			{
				if (!target) target = user;
				animplayer = new AnimationPlayerX(animation, user, target, self, oppmove);
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
				animplayer = new AnimationPlayerX(animation, user, target, self, oppmove);
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
			animplayer.start(); //yield return StartCoroutine?...
			while (animplayer.playing)
			{
				animplayer.update();
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				yield return null;
			}
			if (usersprite) usersprite.ox = 0;
			if (usersprite) usersprite.oy = 0;
			if (usersprite) usersprite.x = olduserx;
			if (usersprite) usersprite.y = oldusery;
			if (targetsprite) targetsprite.ox = 0;
			if (targetsprite) targetsprite.oy = 0;
			if (targetsprite) targetsprite.x = oldtargetx;
			if (targetsprite) targetsprite.y = oldtargety;
			animplayer.Dispose();*/
			yield break;
		}

		void IPokeBattle_Scene.AnimationCore(string animation, IBattler user, IBattler target, bool oppmove) { }

		public IEnumerator LevelUp(IBattlerIE battler, IPokemon pokemon, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*TopRightWindow(Game._INTL("Max. HP<r>+{1}\r\nAttack<r>+{2}\r\nDefense<r>+{3}\r\nSp. Atk<r>+{4}\r\nSp. Def<r>+{5}\r\nSpeed<r>+{6}",
			   pokemon.TotalHP - oldtotalhp,
			   pokemon.ATK - oldattack,
			   pokemon.DEF - olddefense,
			   pokemon.SPA - oldspatk,
			   pokemon.SPD - oldspdef,
			   pokemon.SPE - oldspeed));
			TopRightWindow(Game._INTL("Max. HP<r>{1}\r\nAttack<r>{2}\r\nDefense<r>{3}\r\nSp. Atk<r>{4}\r\nSp. Def<r>{5}\r\nSpeed<r>{6}",
			   pokemon.TotalHP, pokemon.ATK, pokemon.DEF, pokemon.SPA, pokemon.SPD, pokemon.SPE));*/
			yield return null;
		}

		void IPokeBattle_DebugSceneNoGraphics.LevelUp(IBattler battler, IPokemon pokemon, int oldtotalhp, int oldattack, int olddefense, int oldspeed, int oldspatk, int oldspdef) { }

		public IEnumerator ThrowAndDeflect(Items ball, int targetBattler)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*@briefmessage = false;
			balltype = GetBallType(ball);
			ball = string.Format("Graphics/Pictures/ball%02d", balltype);
			//  sprite
			IIconSprite spriteBall = new IconSprite(0, 0, @viewport);
			spriteBall.visible = false;
			//  picture
			pictureBall = new PictureEx((@sprites[$"pokemon{targetBattler}"] as IPokemonBattlerSprite).z + 1);
			IPoint center = getSpriteCenter(@sprites[$"pokemon{targetBattler}"]);
			//  starting positions
			pictureBall.moveVisible(1, true);
			pictureBall.moveName(1, ball);
			pictureBall.moveOrigin(1, PictureOrigin.nenter);
			pictureBall.moveXY(0, 1, 10, 180);
			//  directives
			pictureBall.moveSE(1, "AudioManager/SE/throw");
			pictureBall.moveCurve(30, 1, 150, 70, 30 + (Game.GameData as Game).Graphics.width / 2, 10, center[0], center[1]);
			pictureBall.moveAngle(30, 1, -1080);
			pictureBall.moveAngle(0, pictureBall.totalDuration, 0);
			delay = pictureBall.totalDuration;
			pictureBall.moveSE(delay, "AudioManager/SE/balldrop");
			pictureBall.moveXY(20, delay, 0, (Game.GameData as Game).Graphics.height);
			do //;loop
			{
				pictureBall.update();
				setPictureIconSprite(spriteBall, pictureBall);
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (!pictureBall.running) break;
				yield return null;
			} while (true);

			spriteBall.Dispose();*/
			yield break;
		}

		void IPokeBattle_Scene.ThrowAndDeflect(Items ball, int targetBattler) { }

		public IEnumerator Throw(Items ball, int shakes, bool critical, int targetBattler, bool showplayer)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*@briefmessage = false;
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
			yield return pokeballThrow(ball, shakes, critical, targetBattler, this, @battle.battlers[targetBattler], burst, showplayer);*/
			yield break;
		}

		void IPokeBattle_Scene.Throw(Items ball, int shakes, bool critical, int targetBattler, bool showplayer) { StartCoroutine(this.Throw(ball, shakes, critical, targetBattler, showplayer)); }

		public IEnumerator ThrowSuccess()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			//if wild pokemon...
			if (@battle.opponent == null) {
				@briefmessage=false;
				if (AudioHandler is IGameAudioPlay gap) gap.MEPlay("Jingle - HMTM");
				//All of this below should be a coroutine that returns the value selected in UI
				int frames=(int)(3.5*(Game.GameData as Game).Graphics.frame_rate);
				int i = 0; do {
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate(); i++;
					yield return null;
				} while (i < frames); //frames.times
			}
			yield break;
		}

		void IPokeBattle_Scene.ThrowSuccess() { StartCoroutine(this.ThrowSuccess()); }

		public IEnumerator HideCaptureBall()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (capture != null)
			{
				do //;loop
				{
					if ((capture).opacity <= 0) break;
					(capture).opacity -= 12;
					GraphicsUpdate();
					InputUpdate();
					FrameUpdate();
					yield return null;
				} while (true); //Should end after opacity is reached... maybe destroy after?
			}
		}

		void IPokeBattle_Scene.HideCaptureBall() { StartCoroutine(HideCaptureBall()); }

		public IEnumerator ThrowBait()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*@briefmessage = false;
			string ball = string.Format("Graphics/Pictures/battleBait");
			bool armanim = false;
			if (player.bitmap.width > player.bitmap.height)
			{
				armanim = true;
			}
			//  sprites
			spritePoke = pokemon1;
			spritePlayer = player;
			IIconSprite spriteBall = new IconSprite(0, 0, @viewport);
			spriteBall.visible = false;
			//  pictures
			pictureBall = new PictureEx(spritePoke.z + 1);
			picturePoke = new PictureEx(spritePoke.z);
			picturePlayer = new PictureEx(spritePoke.z + 2);
			IPoint dims =[spritePoke.x, spritePoke.y];
			IPoint pokecenter = getSpriteCenter(pokemon1);
			IPoint playerpos = [player.x, player.y];
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
			picturePlayer.moveName(1, player.name);
			picturePlayer.moveOrigin(1, PictureOrigin.TopLeft);
			picturePlayer.moveXY(0, 1, playerpos[0], playerpos[1]);
			//  directives
			picturePoke.moveSE(1, "AudioManager/SE/throw");
			pictureBall.moveCurve(30, 1, 64, 256, (Game.GameData as Game).Graphics.width / 2, 48,
								  PokeBattle_SceneConstants.FOEBATTLER_X - 48,
								  PokeBattle_SceneConstants.FOEBATTLER_Y);
			pictureBall.moveAngle(30, 1, -720);
			pictureBall.moveAngle(0, pictureBall.totalDuration, 0);
			if (armanim)
			{
				picturePlayer.moveSrc(1, player.bitmap.height, 0);
				picturePlayer.moveXY(0, 1, playerpos[0] - 14, playerpos[1]);
				picturePlayer.moveSrc(4, player.bitmap.height * 2, 0);
				picturePlayer.moveXY(0, 4, playerpos[0] - 12, playerpos[1]);
				picturePlayer.moveSrc(8, player.bitmap.height * 3, 0);
				picturePlayer.moveXY(0, 8, playerpos[0] + 20, playerpos[1]);
				picturePlayer.moveSrc(16, player.bitmap.height * 4, 0);
				picturePlayer.moveXY(0, 16, playerpos[0] + 16, playerpos[1]);
				picturePlayer.moveSrc(40, 0, 0);
				picturePlayer.moveXY(0, 40, playerpos[0], playerpos[1]);
			}
			//  Show Pokémon jumping before eating the bait
			picturePoke.moveSE(50, "AudioManager/SE/jump");
			picturePoke.moveXY(8, 50, pokecenter[0], pokecenter[1] - 8);
			picturePoke.moveXY(8, 58, pokecenter[0], pokecenter[1]);
			pictureBall.moveVisible(66, false);
			picturePoke.moveSE(66, "AudioManager/SE/jump");
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
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (!pictureBall.running && !picturePoke.running && !picturePlayer.running) break;
				yield return null;
			} while (true);

			spriteBall.Dispose();*/
			yield break;
		}

		void IPokeBattle_Scene.ThrowBait() { }

		public IEnumerator ThrowRock()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			/*@briefmessage=false;
			ball=string.Format("Graphics/Pictures/battleRock");
			anger=string.Format("Graphics/Pictures/battleAnger");
			armanim=false;
			if (player.bitmap.width>player.bitmap.height) {
				armanim=true;
			}
			//  sprites
			spritePoke=pokemon1;
			spritePlayer=player;
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
			pokecenter=getSpriteCenter(pokemon1);
			playerpos=[player.x,player.y];
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
			picturePlayer.moveName(1,player.name);
			picturePlayer.moveOrigin(1,PictureOrigin.nopLeft);
			picturePlayer.moveXY(0,1,playerpos[0],playerpos[1]);
			pictureAnger.moveVisible(1,false);
			pictureAnger.moveName(1,anger);
			pictureAnger.moveXY(0,1,pokecenter[0]-56,pokecenter[1]-48);
			pictureAnger.moveOrigin(1,PictureOrigin.nenter);
			pictureAnger.moveZoom(0,1,100);
			//  directives
			picturePoke.moveSE(1,"AudioManager/SE/throw");
			pictureBall.moveCurve(30,1,64,256,(Game.GameData as Game).Graphics.width/2,48,pokecenter[0],pokecenter[1]);
			pictureBall.moveAngle(30,1,-720);
			pictureBall.moveAngle(0,pictureBall.totalDuration,0);
			pictureBall.moveSE(30,"AudioManager/SE/notverydamage");
			if (armanim) {
				picturePlayer.moveSrc(1,player.bitmap.height,0);
				picturePlayer.moveXY(0,1,playerpos[0]-14,playerpos[1]);
				picturePlayer.moveSrc(4,player.bitmap.height*2,0);
				picturePlayer.moveXY(0,4,playerpos[0]-12,playerpos[1]);
				picturePlayer.moveSrc(8,player.bitmap.height*3,0);
				picturePlayer.moveXY(0,8,playerpos[0]+20,playerpos[1]);
				picturePlayer.moveSrc(16,player.bitmap.height*4,0);
				picturePlayer.moveXY(0,16,playerpos[0]+16,playerpos[1]);
				picturePlayer.moveSrc(40,0,0);
				picturePlayer.moveXY(0,40,playerpos[0],playerpos[1]);
			}
			pictureBall.moveVisible(40,false);
			//  Show Pokémon being angry
			pictureAnger.moveSE(48,"AudioManager/SE/jump");
			pictureAnger.moveVisible(48,true);
			pictureAnger.moveZoom(8,48,130);
			pictureAnger.moveZoom(8,56,100);
			pictureAnger.moveXY(0,64,pokecenter[0]+56,pokecenter[1]-64);
			pictureAnger.moveSE(64,"AudioManager/SE/jump");
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
				GraphicsUpdate();
				InputUpdate();
				FrameUpdate();
				if (!pictureBall.running && !picturePoke.running &&
						!picturePlayer.running && !pictureAnger.running) break;
				yield return null;
			} while (true);
			spriteBall.Dispose();*/
			yield break;
		}

		void IPokeBattle_Scene.ThrowRock() { }

		public void Chatter(IBattler attacker, IBattler opponent) //ToDo: Make IEnumerator...
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			if (attacker.pokemon != null)
			{
				//Plays audio sound...
				if (Game.GameData is IGameUtility u) u.PlayCry(attacker.pokemon, 90, 100);
			}
			//All of this below should be a coroutine that returns the value selected in UI
			//int i = 0; do //Idle animation for 1 second...
			//{
			//	(Game.GameData as Game).Graphics.update();
			//	PokemonUnity.Input.update(); i++;
			//	yield return null;
			//} while (i < (Game.GameData as Game).Graphics.frame_rate); //Graphics.frame_rate.times
		}

		private IEnumerator ForceFrameRefresh()
		{
			//UnityEngine.Canvas.ForceUpdateCanvases();
			yield return null; //new UnityEngine.WaitForEndOfFrame();
		}
		#endregion

		#region Interface Implicit Implementation
		IPokeBattle_DebugSceneNoGraphics IPokeBattle_DebugSceneNoGraphics.initialize()
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		void IPokeBattle_DebugSceneNoGraphics.BattleArenaBattlers(IBattle b1, IBattle b2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		void IPokeBattle_DebugSceneNoGraphics.BattleArenaJudgment(IBattle b1, IBattle b2, int[] r1, int[] r2)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		int IPokeBattle_DebugSceneNoGraphics.Blitz(int keys)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		public IEnumerator NextTarget(int cur, int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		void IPokeBattle_DebugScene.NextTarget(int cur, int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		void IPokeBattle_DebugScene.PokemonString(IPokemon pkmn)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		public IEnumerator PrevTarget(int cur, int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		void IPokeBattle_DebugScene.PrevTarget(int cur, int index)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			throw new System.NotImplementedException();
		}

		bool IHasDisplayMessage.DisplayConfirm(string v)
		{
			LogManager.Logger.LogDebug(this, message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			bool value = false;
			//return DisplayConfirmMessage(v);
			DisplayConfirmMessage(v, result: r => value = r);
			return value;
		}
		#endregion
	}
}