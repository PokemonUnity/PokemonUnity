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
using PokemonUnity.Utility;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	/// This class should be static...
	//ToDo: Should all variables in Game be static?
	public partial class Game
	{
		public bool SandBoxMode { get; set; }

		#region Player and Overworld Data
		public static Player Player { get; set; }
		public static Regions Region { get; private set; }
		public static Locations Location { get; private set; }
		/// <summary>
		/// </summary>
		/// <see cref="Player.mapName"/>
		public static int Area { get; private set; }
		//ToDo: Unity implentation through funnel
		//public static PokemonUnity.Overworld.Level Level { get; set; }
		//public static PokemonUnity.Overworld.Camera Camera { get; set; }
		//public Game.TrainerPC PC { get { return new Game.TrainerPC(Player); } }
		//public int mapName { get; set; }
		//public int levelName;
		//ToDo: Move player position to platform engine or Game class..
		public static Vector PlayerPosition { get; set; }
		/// <summary>
		/// Rotation of player model in overworld scene
		/// </summary>
		/// Might be useful if the game is in 2d, but if in 3d... will need to use x,y... dont need 3rd axis
		/// scratch that... only need rotation on single quantization axis...
		/// not sure direction is even needed in save/load profile...
		/// Game should load player facing camera by default.
		public static float PlayerDirection { get; set; }
		/// <summary>
		/// Last town or Pokemon Center visited, that's used as Respawn Point upon a Player's Defeat
		/// </summary>
		public static Locations Checkpoint { get; set; }
		//public Locations respawnCenterId { get; set; }
		//public SeriV3 respawnScenePosition;
		//public int respawnSceneDirection;
		//ToDo: Missing Variables for RepelSteps, RepelType, Swarm
		//ToDo: Missing (Trainer)Player.Rival variable
		//public bool IsRunning() { return false; }
		//public bool startSurfing { get; set; }
		public static int RepelSteps { get; set; } // Should not stack (encourage users to deplete excessive money); reset count based on repel used.
		//public static int RepelType { get; set; } // Maybe instead of this, use Encounter.Rate or... Different repel only changes number of steps, not potency
		//public int SurfPokemon { get { int i = 0; foreach (Pokemon p in Party) if (p.knowsMove(Moves.SURF)) i++; return i; } }
		//public Pokemon GetWalkPokemon() { return null; }
		//public Forms GetWalkPokemon() { return Forms.NONE; } //ToDo: IsShiny?
		#endregion

		#region Private Records of Player Storage Data
		public bool IsCreator { get; private set; }
		private bool isCreator { get; set; }
		/// <summary>
		/// Name of PC Admin, for Pokemon Storage
		/// </summary>
		//public static string CreatorName { get { return isCreator ? "someone" : Player.Name; } }
		public string GetStorageCreator() { return string.Empty; }
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
		public static string PlayerItemData { get; set; }
		public static string PlayerDayCareData { get; set; } //KeyValuePair<Pokemon,steps>[]
		public static string PlayerBerryData { get; set; }
		public static string PlayerNPCData { get; set; }
		public static string PlayerApricornData { get; set; }
		public static Pokemon[,] PC_Poke { get; set; }
		public static string[] PC_boxNames { get; set; }
		public static int[] PC_boxTexture { get; set; }
		public static List<Items> PC_Items { get; set; } 
		public static List<Items> Bag_Items { get; set; }
		#endregion

		#region Constructor
		static Game()
		{
			//ToDo: Move to public Constructor
			#region Public Constructor
			//	Game boots up
			//		* Game engine calls public constructor to begin process
			//	Check files/database 
			//		* Issue update patch if files are outdated
			//		* Redownload if files are corrupt or broken
			//	Load data one by one, ensure validity
			//		* maybe even a loading bar
			//		* give warning to player if problems
			//	Scan for save files and previous game progress
			// Load Player/Character/Overworld THEN Encounter
			con.Open();
			InitNatures();
			InitPokemons();
			InitPokemonMoves();
			InitPokemonEvolutions();
			InitPokemonItems();
			InitMoves();
			InitItems();
			InitBerries();
			InitRegions();
			InitLocations();
			InitPlayerCharacter();
			#endregion
			//GameDebug.Init(null, "GameTestLog");
		}
		private static bool InitPlayerCharacter()
		{
			try
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
					//PC_boxTexture[i] = i; 
				}
				PC_Items = new List<Items>();
				Bag_Items = new List<Items>();

				//Overworld Experience
				Region = Regions.NOT_IN_OVERWORLD;
				Location = Locations.NOT_IN_OVERWORLD;
				Area = 0;
				//<X,Y,Z> Position
				//<X,Y,Z> Rotation
			}
			catch (Exception)
			{
				//throw;
				return false;
			}
			return true;
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
				#region Obsolete and Deprecated
				case "0.0.1":
				case "0.1.0":
					Game.Player.LoadTrainer(data); 
					Game.PC_Poke = data.PC.GetPokemonsFromSeri();
					Game.PC_boxNames = data.PC.BoxNames;
					Game.PC_boxTexture = data.PC.BoxTextures;
					Game.PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
					Game.Bag_Items = data.PlayerBag;
					break;
				case "0.1.1":
					//Game g = new Game();
					//g.PlayerItemData =
					//g.PlayerBerryData =
					//g.PlayerNPCData =
					//g.PlayerApriconData =
					//g.RepelSteps =
					//g.RepelType =
					//g.Rival =
					PlayerPosition = data.PlayerPosition;		//g.PlayerPosition = trainerSaveData.PlayerPosition;
					PlayerDirection = data.PlayerDirection;		//g.PlayerDirection = trainerSaveData.PlayerDirection;
					Checkpoint = (Locations)data.PokeCenterId;	//g.Checkpoint = (Locations)trainerSaveData.PokeCenterId;
					Game.Player.LoadTrainer(data);
					Game.Area = data.ActiveMapId;
					Game.PC_Poke = data.PC.GetPokemonsFromSeri();
					Game.PC_boxNames = data.PC.BoxNames;
					Game.PC_boxTexture = data.PC.BoxTextures;
					Game.PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
					Game.Bag_Items = data.PlayerBag;
					break;
				#endregion
				//Next one gets added to list, and default is copied above, and modified below...
				default:
					//ToDo: "0.2.0" => Load Game, Player, Bag, and PC data in dedicated classes
					//Game g = new Game();
					//g.PlayerItemData =
					//g.PlayerBerryData =
					//g.PlayerNPCData =
					//g.PlayerApriconData =
					//g.RepelSteps =
					//g.RepelType =
					//g.Rival =
					//PlayerPosition = data.PlayerPosition;		//g.PlayerPosition = trainerSaveData.PlayerPosition;
					//PlayerDirection = data.PlayerDirection;		//g.PlayerDirection = trainerSaveData.PlayerDirection;
					//Checkpoint = (Locations)data.PokeCenterId;	//g.Checkpoint = (Locations)trainerSaveData.PokeCenterId;
					//Game.Player.LoadTrainer(data);
					//Game.Area = data.ActiveMapId;
					//Game.PC_Poke = data.PC.GetPokemonsFromSeri();
					//Game.PC_boxNames = data.PC.BoxNames;
					//Game.PC_boxTexture = data.PC.BoxTextures;
					//Game.PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
					//Game.Bag_Items = data.PlayerBag;
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
		/*// <summary>
		/// Active Pokemon Battle the Player is currently involved in.
		/// Matches being spectated would be pass thru a non-static method
		/// </summary>
		/// ToDo: On Set, trigger UnityEngine EventHandler,
		/// Switch scenes, load rules, and animate pokemons
		/// Move to platform engine
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
		private static PokemonUnity.Battle.Battle _battle { get; set; }*/
		#endregion

		#region Audio 
		//public UnityEngine.Audio.AudioMixer audioMixer;
		public static int? nextBattleBGM { get; set; }
		public static int? nextBattleME { get; set; }
		public static int? nextBattleBack { get; set; }
		#endregion
	}
}