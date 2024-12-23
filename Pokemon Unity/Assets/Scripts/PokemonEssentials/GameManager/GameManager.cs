using System;
using System.IO;
using PokemonUnity;
using PokemonUnity.Interface;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Localization;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// GameManager acts as the main controller for the game.
	/// It handles dependencies, scene transitions, database initialization, and event handling.
	/// </summary>
	/// <remarks>
	/// Manager for all the Pokemon game's functions and relationship with Unity Engine
	/// </remarks>
	//[ExecuteInEditMode]
	//[RequireComponent(typeof(LevelLoader))]
	public partial class GameManager : MonoBehaviour
	{
		#region Variables
		/// <summary>
		/// Singleton instance of GameManager.
		/// </summary>
		public static GameManager current;

		/// <summary>
		/// Handles player input.
		/// </summary>
		public InputManager InputManager;

		/// <summary>
		/// Handles game audio.
		/// </summary>
		public AudioManager AudioManager;

		/// <summary>
		/// File manager for testing file operations.
		/// </summary>
		public FileTest FileTest;

		/// <summary>
		/// The core game instance.
		/// </summary>
		public IGame game; //game scope used for temp actions, without affecting original copy?

		/// <summary>
		/// Event fired on game update.
		/// </summary>
		public event Action onUpdate;

		/// <summary>
		/// Event fired after a level is loaded.
		/// </summary>
		public event Action onLevelLoaded;

		/// <summary>
		/// Event fired to load a new level.
		/// </summary>
		//public event Action<int> onLoadLevel;
		public event Action<IScene> onLoadLevel;
		//public event Action<IOnLoadLevelEventArgs> onLoadLevel;

		/// <summary>
		/// Reference to LevelLoader for managing scenes.
		/// <see cref="IGameScenesUI"/>
		/// </summary>
		[SerializeField] private LevelLoader sceneList;
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			//current = this;

			// Singleton pattern implementation.
			if (current == null)
			{
				current = this;
			}
			else if (current != this)
			{
				Destroy(gameObject); // Prevent duplicate instances.
				return;
			}

			// Subscribe to log events.
			ConfigureLogging();

			// Load localization data.
			LoadLocalization();

			// Initialize game data and database.
			InitializeDatabase();

			//ConfigureScenes();

			Debug.Log("GameManager is Awake!");
		}

		void Start()
		{
			//Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			Core.Logger?.LogDebug("GameManager Start method called.");

			// Validate LevelLoader assignment.
			if (sceneList == null)
			{
				Core.Logger?.LogWarning("LevelLoader is not assigned. Please assign it in the inspector.");
				//return;
			}

			// Perform checks to confirm the game has properly initialized
			Core.Logger?.Log("Is Game Null? " + (Game.GameData == null).ToString());
			Core.Logger?.Log("Is Player Null? " + (Game.GameData.Player == null).ToString());
			//if (Game.GameData.Player == null)
			//{
			//	Core.Logger?.Log("Create Player Object");
			//	//IGamePlayer p = new Player();
			//	Core.Logger?.Log("Saving Player Object to Global Singleton");
			//	//Game.GameData.Player = p;
			//}
			Core.Logger?.Log("Is Trainer Null? " + (Game.GameData.Trainer == null).ToString());

			//Enable "OnStart" to trigger battle scene...
			//((object)game.Scenes?.BattleScene as GameObject)?.SetActive(true); //Scene is already active... Sort later.
		}

		void OnDestroy()
		{
			// Unsubscribe from log events to prevent memory leaks.
			LogManager.Logger.OnLog -= HandleLogEvent;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Fires the event to load a new level.
		/// </summary>
		/// <param name="scene">The scene to load.</param>
		public void OnLoadLevel(IScene scene)
		{
			onLoadLevel?.Invoke(scene);
		}
		//public void OnLoadLevel(int id)
		//{
		//	if (onLoadLevel != null) onLoadLevel(id);
		//}
		//public void OnLoadLevel(IScene scene)
		//{
		//	if (onLoadLevel != null) onLoadLevel(scene);
		//}
		//
		//private void ConfigureScenes()
		//{
		//	Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
		//
		//	game.Scenes = gameObject.GetComponent<LevelLoader>() as IGameScenesUI;
		//	//ToDo: Load all the different game scenes into an array, from unity inspector, and pass them as an input parameter below
		//	//game.Scenes.initialize((IScene[])(object[])sceneList);
		//	//game.Scenes.initialize((IPokeBattle_Scene)battle.GetComponent<BattleScene>());
		//	//(game as Game).SetScenes((IPokeBattle_SceneIE)battle.GetComponent<BattleScene>());
		//}

		/// <summary>
		/// Configures logging by subscribing to LogManager events.
		/// </summary>
		private void ConfigureLogging()
		{
			//Debugger.Instance.Init("\\Logs", "GameLog"); //Path = "Logs\GameLog.txt"
			//Core.Logger?.Init("\\Logs", "GameLog"); //Path = "Logs\GameLog.txt"
			LogManager.Logger.OnLog += HandleLogEvent;
		}

		/// <summary>
		/// Initializes the game's database connection and loads data.
		/// </summary>
		private void InitializeDatabase()
		{
			try
			{
				// Test Unity/Framework's FilePath Matches Database Directory
				//Core.Logger?.Log("0-" + System.IO.Path.GetFullPath("..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("1-" + System.IO.Path.GetFullPath("..\\..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("2-" + System.IO.Path.GetFullPath("..\\..\\..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("3-" + System.IO.Path.GetFullPath("..\\..\\..\\..\\veekun-pokedex.sqlite"));

				// Set database path and establish connection.
				Game.DatabasePath = "Data Source=..\\veekun-pokedex.sqlite";
				//Game.DatabasePath = "Data Source =" + UnityEngine.Application.dataPath + "/Data/veekun-pokedex.sqlite";
				Core.Logger?.Log("ConnectionString Database Path: " + Game.DatabasePath);
				//Game.DatabasePath = "Data Source =" + UnityEngine.Application.dataPath + "/Data/veekun-pokedex.sqlite";
				//Game.con = (System.Data.IDbConnection)new System.Data.SQLite.SQLiteConnection(Game.DatabasePath);
				Game.con = (System.Data.IDbConnection)new Mono.Data.Sqlite.SqliteConnection(Game.DatabasePath);
				Game.ResetSqlConnection(Game.DatabasePath);//@"Data\veekun-pokedex.sqlite"
				Core.Logger?.Log("Framework Connected to Database...");
				//Core.Logger?.Log("Path to DB: " + ((System.Data.SQLite.SQLiteConnection)Game.con).FileName);
				Core.Logger?.Log("Path to DB: " + ((Mono.Data.Sqlite.SqliteConnection)Game.con).DataSource);

				// Instantiate the game instance.
				game = new Game();
				Core.Logger?.Log("New Game Entity Successfully Instantiated!~");
			}
			catch (InvalidOperationException ex) { Core.Logger?.LogError("Problem executing SQL with connected database"); Core.Logger?.LogError($"SQL Error: {ex.Message}"); }
			catch (Exception e) { Core.Logger?.LogError(e.ToString()); } //Core.Logger?.LogError($"Unexpected Error: {e}");
			finally
			{
				Core.Logger?.Log("Is Pokemon DB Null? " + (Kernal.PokemonData == null).ToString());
				if (Kernal.PokemonData == null)
				{
					try
					{
						Game.InitTypes();
						Game.InitNatures();
						Game.InitPokemons();
						Game.InitPokemonForms();
						Game.InitPokemonMoves();
						Game.InitPokemonEvolutions();
						Game.InitPokemonItems();
						Game.InitMoves();
						Game.InitItems();
						Game.InitBerries();
						Game.InitTrainers();
						//Game.InitRegions();
						//Game.InitLocations();
					}
					catch (Exception) { Core.Logger?.LogError("there were some problems running sql..."); } //ignore...
				}
				Core.Logger?.Log(string.Format("Is Pokemon DB Greater than 0? {0} : {1}",
					(Kernal.PokemonData.Count > 0).ToString(), Kernal.PokemonData.Count));
				if (Kernal.PokemonData.Count == 0)
				{
					Core.Logger?.Log("Was Pokemon DB Successfully Created? " + Game.InitPokemons());
					Core.Logger?.Log(string.Format("Is Pokemon DB Greater than 0? {0} : {1}",
						(Kernal.PokemonData.Count > 0).ToString(), Kernal.PokemonData.Count));
				}
				Debug.Log("Database successfully initialized.");

				// Ensure the connection is properly closed.
				//if (Game.con?.State == System.Data.ConnectionState.Open)
				//{
				//	Game.con.Close();
				//	Debug.Log("Database connection closed.");
				//}
				// SQlite Database belongs strictly to framework application, so connection doesnt need to close, nor be accessed by any other resource
			}
		}

		/// <summary>
		/// Loads localization data for the game.
		/// </summary>
		private void LoadLocalization()
		{
			Game.LocalizationDictionary = new XmlStringRes(null);
			string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
			//System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
			if (File.Exists(englishLocalization)) //Conditional check is unnecessary as it is handled by dictionary as well...
			{
				Core.Logger?.Log($"Localization file found at {englishLocalization}");
				Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);
			}
			//ToDo: Add other languages here...
			else
			{
				Core.Logger?.Log($"No Localization files found.");
				//return;
			}
		}

		/// <summary>
		/// Handles log events and outputs them to Unity's console.
		/// </summary>
		private void HandleLogEvent(object sender, OnDebugEventArgs e)
		{
			if (e == null || e == EventArgs.Empty) return;

			if (e.Error == true)
				//System.Console.WriteLine("[ERR]: " + e.Message);
				//Debug.LogError($"[ERR] {Time.frameCount}: {e.Message}");
				global::UnityEngine.Debug.LogError(string.Format("[ERR] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
			else if (e.Error == false)
				//System.Console.WriteLine("[WARN]: " + e.Message);
				//Debug.LogWarning($"[WARN] {Time.frameCount}: {e.Message}");
				global::UnityEngine.Debug.LogWarning(string.Format("[WARN] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
			else
				//System.Console.WriteLine("[LOG]: " + e.Message);
				//Debug.Log($"[LOG] {Time.frameCount}: {e.Message}");
				global::UnityEngine.Debug.Log(string.Format("[LOG] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
		}
		#endregion
	}
}