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
		}

		//public Game(PokemonEssentials.Interface.IGamePlayer player, Feature? features = null, Challenges challenge = Challenges.Classic, //string[] rival = null,
		//	string playerItemData = null, string playerDayCareData = null, string playerBerryData = null, string playerNPCData = null, string playerApricornData = null)
		//{
		//	Features = features ?? new Feature();
		//	Challenge = challenge;
		//	//Player = player ?? new Player();
		//	//Trainer = trainer ?? new Combat.Trainer();
		//	//PC = pC ?? throw new ArgumentNullException(nameof(pC));
		//	//Bag = bag ?? throw new ArgumentNullException(nameof(bag));
		//}
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
	}
}