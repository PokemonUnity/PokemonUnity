using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
//using Localization;
//using Localization.Dictionaries;
//using Localization.Dictionaries.Xml;
//using System.IO;
using System.Xml;
using System.Collections;
//using System.Collections.Immutable;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;

/// <summary>
/// Variables that are stored when game is saved, and other temp values used for gameplay.
/// This class should be called once, when the game boots-up.
/// During boot-up, game will check directory for save files and load data.
/// Game class will overwrite all the other class default values when player triggers a load state.
/// </summary>
/// This class should be static...
public partial class Game : UnityUtilityIntegration//: UnityEngine.MonoBehaviour//, UnityEngine.EventSystems.
{
	#region Player and Overworld Data
	//ToDo: Missing Variables for RepelSteps, RepelType, Swarm
	public static Player Player { get; set; }
	public static PokemonUnity.Overworld.Level Level { get; set; }
	public static PokemonUnity.Overworld.Camera Camera { get; set; }
	//public Game.TrainerPC PC { get { return new Game.TrainerPC(Player); } }
	/*public static PokemonUnity.Overworld.Season Season 
	{ 
		get 
		{
			int month = System.DateTime.Today.Month;
			
			if (Jan, May, Sept)
			{
				return Season.Spring;
			}
			if (Feb, Jun, Oct)
			{
				return Season.Summer;
			}
			if (Mar,Jul, Nov)
			{
				return Season.Autumn;
			}
			if (Apr, Aug, Dec)
			{
				return Season.Spring
			}
		}	
	}*/
	#endregion

	#region Private Records of Player Storage Data
	public static bool IsCreator { get; private set; }
	private static bool isCreator { get; set; }
	/// <summary>
	/// Name of PC Admin, for Pokemon Storage
	/// </summary>
	//public static string CreatorName { get { return isCreator ? "someone" : Player.Name; } }
	public string GetStorageCreator() { return ""; }
	//ToDo: Berry Field Data (0x18 per tree, 36 trees)
	//ToDo: Honey Tree, smearing honey on tree will spawn pokemon in 6hrs, for 24hrs (21 trees)
	//Honey tree timer is done in minutes (1440, spawns at 1080), only goes down while playing...
	//ToDo: Missing Variable for DayCare, maybe `Pokemon[,]` for multipe locations?
	//Daycare Data
	//(Slot 1) Occupied Flag 
	//(Slot 1) Steps Taken Since Depositing 
	//(Slot 1) Box EK6 1 
	//(Slot 2) Occupied Flag 
	//(Slot 2) Steps Taken Since Depositing2 
	//(Slot 2) Box EK6 2 
	//Flag (egg available) 
	//RNG Seed
	//ToDo: a bool variable for PC background (if texture is unlocked) `bool[]`
	public static Pokemon[,] PC_Poke { get; set; }
	public static string[] PC_boxNames { get; set; }
	public static int[] PC_boxTexture { get; set; }
	public static List<Item> PC_Items { get; set; } //ToDo: Why not ItemS?
	public static List<Items> Bag_Items { get; set; }
	#endregion

	#region Custom Game Mode
	//Nuzlocke Challenge => Pokemon Centers cost money, every pokemon must be named, when defeated/fainted pokemon is gone, only allowed to capture first pokemon encountered when entering new map
	/// <summary>
	/// Basically, you use the Dexnav to find pokemon in the area, they appear as shadows in the grass, and you need to sneak up on them
	/// these pokemon can have egg moves, or even their HiddenAbility
	/// </summary>
	/// Apparently you can use the Sneaking feature to helps with this. 
	/// ToDo: OnlyAllowEggMovesWhenUsingDexNav or DexNavAllowsEggMoves
	public static bool CatchPokemonsWithEggMoves { get; private set; }
    #endregion

	#region Constructor
	static Game()
	{
		GameDebug.Init(null, "GameTestLog");
		if(LoadInitFile())
		{		
			PC_Poke = new Pokemon[Core.STORAGEBOXES, 30];
			PC_boxNames = new string[Core.STORAGEBOXES];
			PC_boxTexture = new int[Core.STORAGEBOXES];
			for (int i = 0; i < Core.STORAGEBOXES; i++)
			{
				//Initialize the PC storage so pokemons arent null (in value)
				for (int j = 0; j < PC_Poke.GetLength(1); j++)
				{
					//All default values must be `NONE`
					PC_Poke[i, j] = new Pokemon(Pokemons.NONE);//pokemons[i, j];
				}
				//ToDo: Using string from translator here
				PC_boxNames[i] = string.Format("Box {0}", (i + 1).ToString());
				//ToDo: Make sure there's enough texture in library for array size
				PC_boxTexture[i] = i; 
			}
			PC_Items = new List<Item>();
			Bag_Items = new List<Items>();
		}
	}
	private static bool LoadInitFile()
	{
		//Load User Saved Preferences and then apply it to start-up variable below
		//Or create new User Preference file
		UserLanguage  = Languages.English;
		//Load Localization
		//Then Import Databases
		//if (SaveFileFound)
		//LoadPokemonDatabase();
		return true;
	}
	#endregion

	#region Unity Canvas UI
	//public static Translator.Languages UserLanguage = Translator.Languages.English;
	public static Languages UserLanguage { get; private set; }// = Languages.English;
    //public GlobalVariables.Language playerLanguage = GlobalVariables.Language.English;
	/// <summary>
	/// Frame Style for all System Prompts and Text Displays
	/// </summary>
	public static byte WindowSkin { get; private set; }
	/// <summary>
	/// Frame Style for all player and non-playable characters Speech bubbles
	/// </summary>
	public static byte DialogSkin { get; private set; }
	public static byte mvol { get; private set; }
	/// <summary>
	/// Music Volume
	/// </summary>
	public static float mVol {
		set
		{
			if (value > 20f) mvol = (byte)20;
			if (value < 0f) mvol = (byte)0;
			if (value < 20f && value > 0f) mvol = (byte)value;
		}
		get { return (mvol / 20f) * (mvol / 20f); }
	}
	public static byte svol { get; private set; }
    /// <summary>
    /// SFX (Sound Effects) Volume 
    /// </summary>
    public static float sVol {
		set
		{
			if (value > 20f) svol = (byte)20;
			if (value < 0f)  svol = (byte)0;
			if (value < 20f && value > 0f) svol = (byte)value;
		}
		get { return (svol / 20f) * (svol / 20f); }
	}
    public static bool battleScene = true;
    public static bool fullscreen;
    public static byte textSpeed = 2;
	#endregion

	#region Unity Scene Manager
	public static CanvasUIHandler CanvasManager { get; private set; }
	public static DialogHandler TextBox { get; private set; }
	public static StartupSceneHandler StartScene { get; private set; }
	public static BattlePokemonHandler BattleScene { get; private set; }
	//public static ItemHandler ItemScene { get; private set; }
	//public static SummaryHandler SummaryScene { get; private set; }
	//public static SettingsHandler SettingsScene { get; private set; }
	#region Scene Manager Methods
	public static void SetCanvasManager(CanvasUIHandler canvas) { CanvasManager = canvas; }
	public static void SetStartScene(StartupSceneHandler start) { StartScene = start; }
	#endregion
	#endregion

	#region Save/Load Data
	private static byte slotIndex { get; set; }
	//private int fileIndex { get; set; }
	/// <summary>
	/// Bool used to tell Start-Up screen whether or not to display "Continue" option
	/// </summary>
    public static bool SaveFileFound { get; private set; }
    //public System.DateTimeOffset fileCreationDate { get; set; }
	//public System.DateTimeOffset? lastSave { get; set; }
	//public System.DateTimeOffset startTime { get; set; }

	/// <summary
	/// Preload before any of the other scenes are loaded...
	/// </summary>
	///ToDo: Temp Save Profiles to be used and displayed on Start-Up screen
	public static void Load()
	{
		//Load player settings (language, full screen, vol...)
		//Load continue/new game/"choose load slots" options...
		//Load temp profile data (Party, pokedex seen/caught, hours played...)
	}
	/// <summary>
	/// Loads saved game data from memory slot
	/// </summary>
	/// <param name="i">Array int from binary stream</param>
	public static void Load(byte i)
    {
		slotIndex = i > 0 && i < 3 ? i : slotIndex;
        //Game.SaveLoad.Load();
		PokemonUnity.Saving.SaveData data = PokemonUnity.Saving.SaveManager.GetSave(i);
		Game.Player = new Player();

		switch (data.BuildVersion)
		{
			case "0.0.1":
			//Next one gets added to list, and default is copied above, and modified below...
			default:
				Game.Player.LoadTrainer(data); 
				Game.PC_Poke = data.PC.GetPokemonsFromSeri();
				Game.PC_boxNames = data.PC.BoxNames;
				Game.PC_boxTexture = data.PC.BoxTextures;
				Game.PC_Items = new List<Item>(data.PC.GetItemsFromSeri());
				Game.Bag_Items = data.PlayerBag;
				break;
		}
	}
    public static void Save()
    {
		//using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(FILE_NAME,)))
		//Game.SaveLoad.Save();

		//PokemonUnity.Saving.SaveManager.Overwrite(new PokemonUnity.Saving.SaveData(), slotIndex);
		SaveData[] save = SaveManager.GetSaves();
		if (save == null)
			save = new SaveData[] { null, null, null };
		save[slotIndex] = new SaveData();
		SaveManager.CreateSaveFileAndSerialize(save);
    }
	/// <summary>
	/// For Debug Use Purposes;
	/// Used Unit Tester...
	/// </summary>
    public static void Save(SaveData test, int slot)
    {
		SaveData[] save = SaveManager.GetSaves();
		if (save == null)
			save = new SaveData[] { null, null, null };
		save[slot] = test;
		SaveManager.CreateSaveFileAndSerialize(save);
    }

    /*private class SaveLoad {
        #region Variables
        //int DatabaseEntryStringWidth = 100;
        System.IO.FileStream fs;
        //BinaryWriter w; //= new BinaryWriter(fs);
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        //Game data = new Game();
        Game[] gamesaves = new Game[3];
        Translator.Languages userpreflanguage = Translator.Languages.English;
#if DEBUG
		private const string FILE_NAME = @"Test.pkud"; //TestProject\bin\Debug
		//private const string FILE_NAME = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Test.data"; //TestProject\bin\Debug
		//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
		//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
		private const string FILE_NAME = UnityEngine.Application.persistentDataPath + "/Test.pkud";
		//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
#endif
        #endregion

        public static void Save(System.IO.BinaryWriter writer) { }
        
        /// <summary>
        /// When initially boots up, this will be all the application data stored
        /// on user's PersonalComputer (PC). If first time running game, naturally
        /// stored data would not exist, and the game will produce one by default.
        /// </summary>
        void OldMe()
        {
            //UnityEngine.Debug.Log("Checking to see if BinaryText exist...");
            if (System.IO.File.Exists(FILE_NAME))
            {
                //UnityEngine.Debug.Log(FILE_NAME + " already exists!");

                //UnityEngine.Debug.Log("Loading Old Info from BinaryText...");
                // Create the reader for data.
                //fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
                fs = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                //UnityEngine.Debug.Log("Information Loaded.");
                //UnityEngine.Debug.Log("Deserializing Information...");
                //data = (Game)bf.Deserialize(fs);
                SaveLoad d = (SaveLoad)bf.Deserialize(fs);
                //UnityEngine.Debug.Log("Rewriting TextField/Variables...");

                //username = data.username;
                //rememberme = data.rememberme;
                Core.UserLanguage = d.userpreflanguage;

                //An Array[3] of Game representing GameSaves
                gamesaves = d.gamesaves;//(Game[])bf.Deserialize(fs);
                SaveFileFound = true;

                //UnityEngine.Debug.Log("Information Loaded and Updated.");
                //UnityEngine.Debug.Log("Closing BinaryText...");
                //UnityEngine.Debug.Log("Closing FileStream...");
                fs.Close();
                //UnityEngine.Debug.Log("BinaryText Closed.");
                return;
            }
            else
            {
                SaveFileFound = false;
                System.IO.File.Open(FILE_NAME, System.IO.FileMode.Create).Close();
            }
		}

		//public static SaveDataOld[] savedGames = new SaveDataOld[]
		//{
		//	null, null, null
		//};

		public static void Save()
		{
			//if (SaveDataOld.currentSave != null)
			//{
			//	if (SaveDataOld.currentSave.getFileIndex() >= 0 && SaveDataOld.currentSave.getFileIndex() < savedGames.Length)
			//	{
			//		SaveDataOld.currentSave.PlayTime += SaveDataOld.currentSave.startTime.Subtract(System.DateTime.UtcNow);
			//		SaveDataOld.currentSave.lastSave = System.DateTime.UtcNow;// new System.DateTime(,System.DateTimeKind.Utc);
			//		savedGames[SaveDataOld.currentSave.getFileIndex()] = SaveDataOld.currentSave;
			//		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//		System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
			//		bf.Serialize(file, SaveLoad.savedGames);
			//		file.Close();
			//	}
			//}
		}

		public static bool Load()
		{
			//Debug.Log(Application.persistentDataPath);
			if (System.IO.File.Exists(FILE_NAME))
			{
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				System.IO.FileStream file = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open);
				//SaveLoad.savedGames = (SaveDataOld[])bf.Deserialize(file);

				//Player = new Trainer().LoadTrainer(trainerSaveData: trainerData);

				file.Close();
				return true;
			}
			return false;
		}

		//public static int getSavedGamesCount()
		//{
		//	int count = 0;
		//	for (int i = 0; i < savedGames.Length; i++)
		//	{
		//		if (savedGames[i] != null)
		//		{
		//			count += 1;
		//		}
		//	}
		//	return count;
		//}

		//public static void resetSaveGame(int index)
		//{
		//	savedGames[index] = null;
		//
		//	if (index < 2)
		//	{
		//		for (int i = index; i < 2; i++)
		//		{
		//			SaveLoad.savedGames[i] = SaveLoad.savedGames[i + 1];
		//			SaveLoad.savedGames[i + 1] = null;
		//		}
		//	}
		//
		//	bool sGN1 = savedGames[0] == null;
		//	bool sGN2 = savedGames[1] == null;
		//	bool sGN3 = savedGames[2] == null;
		//
		//	//Debug.Log(sGN1.ToString() + ", " + sGN2.ToString() + ", " + sGN3.ToString());
		//
		//	System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		//	System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
		//	bf.Serialize(file, SaveLoad.savedGames);
		//	file.Close();
		//}
	}*/
	#endregion

	#region Active Battle and Misc Battle related Data
	/// <summary>
	/// Active Pokemon Battle the Player is currently involved in.
	/// Matches being spectated would be pass thru a non-static method
	/// </summary>
	/// ToDo: On Set, trigger UnityEngine EventHandler,
	/// Switch scenes, load rules, and animate pokemons
	//public static Battle Battle { get; set; }
	public static PokemonUnity.Battle.Battle battle
	{
		get
		{
			return _battle;
		} 
		set
		{
#if !DEBUG
			UnityEngine.Debug.Log(value);
#endif
			_battle = value;
		}
	}
	private static PokemonUnity.Battle.Battle _battle { get; set; }
	#endregion

	#region Audio 
	public UnityEngine.Audio.AudioMixer audioMixer;
	public static int? nextBattleBGM { get; set; }
	public static int? nextBattleME { get; set; }
	public static int? nextBattleBack { get; set; }
	#endregion
}

/// <summary>
/// This class will be inherited by all other classes, 
/// and offer a direct link to Unity's Engine
/// for ease of use utility and integration.
/// </summary>
public class UnityUtilityIntegration
#if !DEBUG //|| UNITY_EDITOR 
	//Not sure if this is something i want inheriting monobehavior...
	: UnityEngine.MonoBehaviour
#endif
{
	#region Unity Monobehaviour Stuff
#if !DEBUG //|| UNITY_EDITOR 
	public static GameVariables game;
    //public LevelManager levelManager;
	public void Awake()
	{
		//Log bool results in debugger...
		//GameDebug.Assert(game == null);
		DontDestroyOnLoad(this.gameObject);
		game = this;

		var commandLineArgs = new List<string>(System.Environment.GetCommandLineArgs());

		// If -logfile was passed, we try to put our own logs next to the engine's logfile
		var engineLogFileLocation = ".";
		var logfileArgIdx = commandLineArgs.IndexOf("-logfile");
		if (logfileArgIdx >= 0 && commandLineArgs.Count >= logfileArgIdx)
		{
			engineLogFileLocation = System.IO.Path.GetDirectoryName(commandLineArgs[logfileArgIdx + 1]);
		}

		//Headless means if it's offline...
		var logName = true ? "game_" + DateTime.UtcNow.ToString("yyyyMMdd_HHmmss_fff") : "game";
		GameDebug.Init(engineLogFileLocation, logName);

#if UNITY_EDITOR
        GameDebug.Log("Build type: editor");
#elif DEVELOPMENT_BUILD
        GameDebug.Log("Build type: development");
#else
        GameDebug.Log("Build type: release");
#endif
		//ToDo: Load asssembly build version into logger
        GameDebug.Log("BuildID: " + buildId);

        //levelManager = new LevelManager();
        //levelManager.Init();
        //GameDebug.Log("LevelManager initialized");

        inputSystem = new InputSystem();
        GameDebug.Log("InputSystem initialized");
	}
	
    void OnDestroy()
    {
        GameDebug.Shutdown();
        Console.Shutdown();
        //if (m_DebugOverlay != null)
        //    m_DebugOverlay.Shutdown();
    }

    void OnApplicationQuit()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        GameDebug.Log("Farewell, cruel world...");
        System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
        ShutdownGameLoops();
    }

	public void LoadLevel(string levelname)
	{
		if (!Game.game.levelManager.CanLoadLevel(levelname))
		{
			GameDebug.Log("ERROR : Cannot load level : " + levelname);
			return;
		}

		Game.game.levelManager.LoadLevel(levelname);
	}
#endif
	#endregion

	#region Debug Functions and Features
	public static bool IS_DEBUG_ACTIVE { get; set; }
	public static void DebugLog(string text, bool? error = null)
	{
		if (!error.HasValue)
			//Debug = text;
			GameDebug.Log(text);
		else
		{
			if (error.Value)
				//DebugError = text;
				GameDebug.LogError(text);
			else
				//DebugWarning = text;
				GameDebug.LogWarning(text);
		}
	}
	#endregion

	#region MyRegion
	public static class Input
	{
		[Flags]
		public enum Blocker
		{
			None = 0,
			Console = 1,
			Chat = 2,
			Debug = 4,
		}
		static Blocker blocks;

		public static void SetBlock(Blocker b, bool value)
		{
			if (value)
				blocks |= b;
			else
				blocks &= ~b;
		}

		internal static float GetAxisRaw(string axis)
		{
			return blocks != Blocker.None ? 0.0f : UnityEngine.Input.GetAxisRaw(axis);
		}

		internal static bool GetKey(UnityEngine.KeyCode key)
		{
			return blocks != Blocker.None ? false : UnityEngine.Input.GetKey(key);
		}

		internal static bool GetKeyDown(UnityEngine.KeyCode key)
		{
			return blocks != Blocker.None ? false : UnityEngine.Input.GetKeyDown(key);
		}

		internal static bool GetMouseButton(int button)
		{
			return blocks != Blocker.None ? false : UnityEngine.Input.GetMouseButton(button);
		}

		internal static bool GetKeyUp(UnityEngine.KeyCode key)
		{
			return blocks != Blocker.None ? false : UnityEngine.Input.GetKeyUp(key);
		}
	}
	#endregion

	#region Unity Canvas UI
	#region Resources
	public static UnityEngine.Sprite[] LoadAllWindowSkinSprites()
	{
		return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\WindowSkin");
	}

	public static UnityEngine.Sprite[] LoadAllDialogSkinSprites()
	{
		return UnityEngine.Resources.LoadAll<UnityEngine.Sprite>(@"\Sprites\GUI\Frame\DialogSkin");
	}
	#endregion
	//Game UI
	//public UnityEngine.Texture2D DialogWindowSkin;
	//private UnityEngine.UI.Image DialogWindowSkin;
	/// <summary>
	/// Frame Style for all System Prompts and Text Displays
	/// </summary>
	public static UnityEngine.Sprite WindowSkinSprite { get { return LoadAllWindowSkinSprites()[Game.WindowSkin]; } }
	/// <summary>
	/// Frame Style for all player and non-playable characters Speech bubbles
	/// </summary>
	public static UnityEngine.Sprite DialogSkinSprite { get { return LoadAllDialogSkinSprites()[Game.DialogSkin]; } }
	/// <summary>
	/// In-game UI dialog window to prompt message to user
	/// </summary>
	/// ToDo: Allow game logic to pass npc scripts thru this
	/// ToDo: Option for dialog prompts, i.e. "Yes/No, Continue.."
	/// <param name="text"></param>
	/// <param name="error">Maybe something about interupting coroutine</param>
	public static void Dialog(string text, bool? error = null, params string[] promptOptions)
	{
		//ToDo: Pass values directly to DialogEventHandler
		//ToDo: Make a struct for each non-class (enum, etc) type and add a ToString(bool) override that output unity richtext color tags
		//Consider adding a Queue to dialog text... so messages arent replaced but appended
		//if (!error.HasValue)
		//	Debug = text;
		//else
		//{
		//	if (error.Value)
		//		DebugError = text;
		//	else
		//		DebugWarning = text;
		//}
		DebugLog(text, error);
	}
	protected static void Display(string text)
	{

	}
	protected static void DisplayBrief(string text)
	{

	}
	protected static void DisplayPause(string text)
	{

	}
	protected static void DisplayConfirm(string text)
	{

	}
	protected static string L(Text text, string textid, params string[] vs)
	{
		return LanguageExtension.Translate(text, textid, vs).Value;
	}
	#endregion
}

namespace PokemonUnity
{
	/*// <summary>
	/// Extension methods for <see cref="MenuItemDefinition"/>.
	/// </summary>
	public static class MenuItemDefinitionExtensions
	{
		/// <summary>
		/// Moves a menu item to top in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToTop(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(0, menuItem);
		}

		/// <summary>
		/// Moves a menu item to bottom in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		public static void MoveMenuItemToBottom(this IList<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.Count, menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list after another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move before it)</param>
		public static void MoveMenuItemBefore(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem), menuItem);
		}

		/// <summary>
		/// Moves a menu item in the list before another menu item in the list.
		/// </summary>
		/// <param name="menuItems">List of menu items</param>
		/// <param name="menuItemName">Name of the menu item to move</param>
		/// <param name="targetMenuItemName">Target menu item (to move after it)</param>
		public static void MoveMenuItemAfter(this IList<MenuItemDefinition> menuItems, string menuItemName, string targetMenuItemName)
		{
			var menuItem = GetMenuItem(menuItems, menuItemName);
			var targetMenuItem = GetMenuItem(menuItems, targetMenuItemName);
			menuItems.Remove(menuItem);
			menuItems.Insert(menuItems.IndexOf(targetMenuItem) + 1, menuItem);
		}

		private static MenuItemDefinition GetMenuItem(IEnumerable<MenuItemDefinition> menuItems, string menuItemName)
		{
			var menuItem = menuItems.FirstOrDefault(i => i.Name == menuItemName);
			if (menuItem == null)
			{
				throw new Exception("Can not find menu item: " + menuItemName);
			}

			return menuItem;
		}
	}*/
}