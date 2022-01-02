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
		#region Constructor
		static Game()
		{
			//UserLanguage = Languages.English;
			GameData = (PokemonEssentials.Interface.IGame)new Game();

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
			//ResetSqlConnection();
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

		public Game(PokemonEssentials.Interface.IGamePlayer player, Feature? features = null, Challenges challenge = Challenges.Classic, Locations checkpoint = Locations.PALLET_TOWN, int area = 0, int repelSteps = 0, string[] rival = null, 
			string playerItemData = null, string playerDayCareData = null, string playerBerryData = null, string playerNPCData = null, string playerApricornData = null)
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
		#endregion

		/*private bool InitPlayerCharacter()
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
			//UserLanguage  = Languages.English;
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
		}*/

		public Game SetScenes(params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			Scenes = new UX.Scene(scenes);
			return this;
		}

		public Game SetScreens(params PokemonEssentials.Interface.Screen.IScreen[] screens)
		{
			Screens = new UX.Screen(screens);
			return this;
		}

		//public static implicit operator Game(GameState state)
		//{
		//	Game game = new Game()
		//	{
		//		Player				= state.GetPlayer()
		//		,RepelSteps			= state.RepelSteps
		//		//,Rival				= state.Rival
		//		//,PlayerPosition		= state.PlayerPosition
		//		,Checkpoint			= (Locations)state.PokeCenterId
		//		//,Area				= data.ActiveMapId;
		//		//,PC					= state.PlayerPC
		//		//,Bag				= state.PlayerBag
		//		//PlayerItemData		= state.PlayerItemData
		//		//,PlayerBerryData	= state.PlayerBerryData
		//		//,PlayerNPCData		= state.PlayerNPCData
		//		//,PlayerApricornData	= state.PlayerApricornData
		//		//PC_Poke = data.PC.GetPokemonsFromSeri();
		//		//PC_boxNames = data.PC.BoxNames;
		//		//PC_boxTexture = data.PC.BoxTextures;
		//		//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
		//		//Bag_Items = data.PlayerBag;
		//		,Challenge = state.Challenge
		//		,Features = state.Features
		//	};
		//	return game;
		//}
	}
}