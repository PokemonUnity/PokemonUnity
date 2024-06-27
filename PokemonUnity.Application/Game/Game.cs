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
using PokemonUnity.Overworld;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// </summary>
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
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
			#endregion

			Global = new GlobalMetadata();
			//MapFactory = new MapFactory();
			//PokemonMap = new MapMetadata();
			//MapData = new MapData();
			PokemonSystem = new PokemonSystem();
			PokemonTemp = new PokemonTemp();
			PokemonEncounters = new PokemonUnity.Overworld.PokemonEncounter();
			//MapEncounterData = new MapEncounterData(); Duplicate
			PokemonStorage = new PokemonStorage();
			Bag = new PokemonBag();
			/*Scene = new SceneMap();
			GameTemp = new GameTemp();
			GamePlayer = new GamePlayer();
			//Player = new Player();	Duplicate
			//Trainer = new Trainer();
			DataSystem = new DataSystem();
			DataTilesets = new DataTilesets();
			DataCommonEvents = new DataCommonEvents();
			GameSwitches = new GameSwitches();
			GameSelfSwitches = new GameSelfSwitches();
			GameVariables = new GameVariables();*/
			GameSystem = new GameSystem();
			GameMap = new Game_Map();
			EncounterModifier = new EncounterModifier();
		}
		#endregion

		private void EnableEventListeners()
		{
			//Events_EncounterEnd;
			//Events_EncounterModifiers;
			//Events_OnEndBattle;
			//Events_OnMapChange;
			//Events_OnMapChangeRoaming;
			//Events_OnMapChanging;
			//Events_OnMapSceneChange;
			//Events_OnMapUpdate;
			//Events_OnSpritesetCreate;
			//Events_OnStartBattle;
			//Events_OnStepTaken;
			//Events_OnStepTakenFieldMovement;
			//Events_OnStepTakenPokemonShadow;
			//Events_OnStepTakenTransferPossible;
			//Events_OnWildBattleOverride;
		}
	}
}