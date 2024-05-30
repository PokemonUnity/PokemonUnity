using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		//private readonly static Lazy<Game> _instance = new Lazy<Game>(() => new Game());

		#region Constructor
		static Game()
		{
			//UserLanguage = Languages.English;
			GameData = new Game();

			//GameDebug.Init(null, "GameTestLog");
		}

		public Game()
		{
			//Rival = new string[0];
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

		public Game(PokemonEssentials.Interface.IGamePlayer player, Feature? features = null, Challenges challenge = Challenges.Classic, //string[] rival = null,
			string playerItemData = null, string playerDayCareData = null, string playerBerryData = null, string playerNPCData = null, string playerApricornData = null)
		{
			Features = features ?? new Feature();
			Challenge = challenge;
			//Player = player ?? new Player();
			//Trainer = trainer ?? new Combat.Trainer();
			//Rival = rival							?? new string[0];//throw new ArgumentNullException(nameof(rival));
			PlayerItemData = playerItemData			?? string.Empty;//throw new ArgumentNullException(nameof(playerItemData));
			PlayerDayCareData = playerDayCareData	?? string.Empty;//throw new ArgumentNullException(nameof(playerDayCareData));
			PlayerBerryData = playerBerryData		?? string.Empty;//throw new ArgumentNullException(nameof(playerBerryData));
			PlayerNPCData = playerNPCData			?? string.Empty;//throw new ArgumentNullException(nameof(playerNPCData));
			PlayerApricornData = playerApricornData	?? string.Empty;//throw new ArgumentNullException(nameof(playerApricornData));
			//PC = pC ?? throw new ArgumentNullException(nameof(pC));
			//Bag = bag ?? throw new ArgumentNullException(nameof(bag));
		}
		#endregion

		public Game SetScenes(params PokemonEssentials.Interface.Screen.IScene[] scenes)
		{
			//Scenes = new UX.Scene(scenes);
			//if (Scenes == null)
			Scenes.initialize(scenes);
			return this;
		}

		public Game SetScreens(params PokemonEssentials.Interface.Screen.IScreen[] screens)
		{
			Screens = new PokemonUnity.Interface.Screen(screens);
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