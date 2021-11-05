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
using PokemonUnity.Application;
using PokemonUnity.Character;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// </summary>
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	//ToDo: Should all variables in Game be static?
	public partial class Game
	{
		/// <summary>
		/// Singleton Instance of Game class to store current/active play state.
		/// </summary>
		public static Game GameData { get; private set; }
		public Feature Features { get; private set; }
		public Challenges Challenge { get; private set; }
		//public GameModes Mode { get; private set; }
		#region Player and Overworld Data
		[System.Obsolete("Use `GameData.Trainer` because Player class is being phased out")]
		public Character.Player Player { get; set; }
		//public Regions Region { get; private set; }
		//public Locations Location { get; private set; }
		/// <summary>
		/// Last town or Pokemon Center visited, that's used as Respawn Point upon a Player's Defeat
		/// </summary>
		///public SeriV3 respawnScenePosition;
		///public Locations respawnCenterId { get; set; }
		public Locations Checkpoint { get; set; }
		/// <summary>
		/// </summary>
		// <see cref="Character.Player.mapName"/>
		public int Area { get; private set; }
		//ToDo: Missing Variables for RepelType, Swarm
		public int RepelSteps { get; set; } // Should not stack (encourage users to deplete excessive money); reset count based on repel used.
		//public static int RepelType { get; set; } // Maybe instead of this, use Encounter.Rate or... Different repel only changes number of steps, not potency
		public string[] Rival { get; set; }
		private byte slotIndex { get; set; }
		#endregion

		#region Private Records of Player Storage Data
		///// <summary>
		///// Name of PC Admin, for Pokemon Storage
		///// </summary>
		///// <remarks>
		///// Maybe check region accessing pc from, and associate region prof with creator
		///// ...until maybe the player encounter's the prof and exchanges pokedex (or however it works)
		///// </remarks>
		//ToDo: Move CreatorName() to PC npc...
		//public static string CreatorName { get { return Player.IsCreator ? "someone" : Player.Name; } }
		//public string GetStorageCreator() { return string.Empty; }

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
		public string PlayerItemData { get; set; }
		public string PlayerDayCareData { get; set; } //KeyValuePair<Pokemon,steps>[]
		public string PlayerBerryData { get; set; }
		public string PlayerNPCData { get; set; }
		public string PlayerApricornData { get; set; }
		//public Pokemon[,] PC_Poke { get; set; }
		//public string[] PC_boxNames { get; set; }
		//public int[] PC_boxTexture { get; set; }
		//public List<Items> PC_Items { get; set; } 
		//public List<Items> Bag_Items { get; set; }
		//public Character.PC PC { get; private set; }
		//public Character.Bag Bag { get; private set; }
		#endregion

		#region Constructor
		static Game()
		{
			UserLanguage = Languages.English;
			GameData = new Game();

			//GameDebug.Init(null, "GameTestLog");
		}

		public Game()
		{
			Rival = new string[0];
			Features = new Feature();
			#region Public Constructor
			//	Game boots up
			//		* Game engine calls private constructor to begin process
			//		* private constructor creates public constructor as default/placeholder 
			//	Check files/database 
			//		* Issue update patch if files are outdated
			//		* Redownload if files are corrupt or broken
			//	Load data one by one, ensure validity
			//		* maybe even a loading bar
			//		* give warning to player if problems
			//	Scan for save files and previous game progress
			// Load Player/Character/Overworld THEN Encounter
			ResetSqlConnection();
			//con.Open();
			//InitTypes();
			//InitNatures();
			//InitPokemons();
			//InitPokemonForms();
			//InitPokemonMoves();
			//InitPokemonEvolutions();
			//InitPokemonItems();
			//InitMoves();
			//InitItems();
			//InitBerries();
			//InitTrainers();
			//InitRegions();
			//InitLocations();
			//InitPlayerCharacter();
			//InitLoadFile();
			#endregion
		}

		public Game(Player player, Feature? features = null, Challenges challenge = Challenges.Classic, Locations checkpoint = Locations.PALLET_TOWN, int area = 0, int repelSteps = 0, string[] rival = null, 
			string playerItemData = null, string playerDayCareData = null, string playerBerryData = null, string playerNPCData = null, string playerApricornData = null)
			//: this ()
		{
			Features = features ?? new Feature();
			Challenge = challenge;
			//Player = player ?? new Player();
			//Trainer = trainer ?? new Combat.Trainer();
			Checkpoint = checkpoint;
			Area = area;
			RepelSteps = repelSteps;
			Rival = rival							?? new string[0];//throw new ArgumentNullException(nameof(rival));
			PlayerItemData = playerItemData			?? string.Empty;//throw new ArgumentNullException(nameof(playerItemData));
			PlayerDayCareData = playerDayCareData	?? string.Empty;//throw new ArgumentNullException(nameof(playerDayCareData));
			PlayerBerryData = playerBerryData		?? string.Empty;//throw new ArgumentNullException(nameof(playerBerryData));
			PlayerNPCData = playerNPCData			?? string.Empty;//throw new ArgumentNullException(nameof(playerNPCData));
			PlayerApricornData = playerApricornData	?? string.Empty;//throw new ArgumentNullException(nameof(playerApricornData));
			//PC = pC ?? throw new ArgumentNullException(nameof(pC));
			//Bag = bag ?? throw new ArgumentNullException(nameof(bag));
		}

		private bool InitPlayerCharacter()
		{
			try
			{
				//PC_Poke = new Pokemon[Core.STORAGEBOXES, 30];
				//PC_boxNames = new string[Core.STORAGEBOXES];
				//PC_boxTexture = new int[Core.STORAGEBOXES];
				//for (int i = 0; i < Core.STORAGEBOXES; i++)
				//{
				//	//Initialize the PC storage so pokemons arent null (in value)
				//	for (int j = 0; j < PC_Poke.GetLength(1); j++)
				//	{
				//		//All default values must be `NONE`
				//		PC_Poke[i, j] = new Pokemon(Pokemons.NONE);//pokemons[i, j];
				//	}
				//	//ToDo: Using string from translator here
				//	PC_boxNames[i] = string.Format("Box {0}", (i + 1).ToString());
				//	//ToDo: Make sure there's enough texture in library for array size
				//	//PC_boxTexture[i] = i; 
				//}
				//PC_Items = new List<Items>();
				//Bag_Items = new List<Items>();
				//PC = new Character.PC();
				//Bag = new Character.Bag();

				//Overworld Experience
				//Region = Regions.NOT_IN_OVERWORLD;
				//Location = Locations.NOT_IN_OVERWORLD;
				//Area = 0;
				//<X,Y,Z> Position
				//<X,Y,Z> Rotation
				Player = new Player();
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
		/// <summary>
		/// </summary>
		/// <param name="sql"></param>
		/// <returns></returns>
		/// <remarks>
		/// Call this method each time, to read from file, and load saved data
		/// </remarks>
		//public static bool InitLoadFile(bool sql = false)
		public static GameState[] InitLoadFile(bool sql = false)
		{
			//Initialize DEFAULT global/game variables (User Settings)
			//Load SaveData variable from File (and/or CloudStorage)
			if (sql) //using (con)
			{
				//return false;
				return new GameState[0];
			}
			else //return GetPokemonsFromXML();
			{
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				if (System.IO.File.Exists(Saving.SaveManager.playerSave))
				{
#if DEBUG
					using (System.IO.FileStream fs = System.IO.File.Open(Saving.SaveManager.playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
						using (System.IO.StreamReader sr = new System.IO.StreamReader(fs, System.Text.Encoding.UTF8))
						{
							SaveData data = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd());
							UserLanguage	= 	(Languages)data.Language;
							WindowSkin		= 	data.WindowBorder;
							DialogSkin		= 	data.DialogBorder;
							textSpeed		= 	data.TextSpeed	 ;
							mVol			= 	data.mVol		 ;
							sVol			= 	data.sVol		 ;
							//return true;
							return data.GameStates;
						}
#else
					using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
					{
						return (SaveData)bf.Deserialize(fs);
					}
#endif
				}
				//else return false;
				else return null;
			}
		}
		#endregion

		#region Unity Canvas UI
		public static bool DebugButtonPressed { get; private set; }
		//public static Translator.Languages UserLanguage = Translator.Languages.English;
		public static Languages UserLanguage { get; set; }// = Languages.English;
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
		public static bool fullscreen;
		public static byte textSpeed = 2;

		public static Languages pbGetLanguage() { return UserLanguage; }
		public static void pbMessage(string msg) {  }
		public static int pbMessage(string msg, string[] commands, int selectedChoice) { return selectedChoice; }
		#endregion

		public static implicit operator Game(GameState state)
		{
			Game game = new Game()
			{
				Player				= state.GetPlayer()
				,RepelSteps			= state.RepelSteps
				//,Rival				= state.Rival
				//,PlayerPosition		= state.PlayerPosition
				,Checkpoint			= (Locations)state.PokeCenterId
				//,Area				= data.ActiveMapId;
				//,PC					= state.PlayerPC
				//,Bag				= state.PlayerBag
				//PlayerItemData		= state.PlayerItemData
				//,PlayerBerryData	= state.PlayerBerryData
				//,PlayerNPCData		= state.PlayerNPCData
				//,PlayerApricornData	= state.PlayerApricornData
				//PC_Poke = data.PC.GetPokemonsFromSeri();
				//PC_boxNames = data.PC.BoxNames;
				//PC_boxTexture = data.PC.BoxTextures;
				//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
				//Bag_Items = data.PlayerBag;
				,Challenge = state.Challenge
				,Features = state.Features
			};
			return game;
		}
	}
}