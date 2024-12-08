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
using UnityEngine;

namespace PokemonUnity.Interface.UnityEngine
{
    /// <summary>
    /// GameManager acts as the main controller for the game.
    /// It handles dependencies, scene transitions, database initialization, and event handling.
    /// </summary>
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
        public InputManager InputManager { get; private set; }

        /// <summary>
        /// Handles game audio.
        /// </summary>
        public AudioManager AudioManager { get; private set; }

        /// <summary>
        /// File manager for testing file operations.
        /// </summary>
        public FileTest FileTest { get; private set; }

        /// <summary>
        /// The core game instance.
        /// </summary>
        public IGame game;

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
        public event Action<IScene> onLoadLevel;

        /// <summary>
        /// Reference to LevelLoader for managing scenes.
        /// </summary>
        [SerializeField]
        private LevelLoader sceneList;
        #endregion

        #region Unity Lifecycle Methods
        private void Awake()
        {
            Debug.Log("GameManager is Awake!");

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

            // Initialize game data and database.
            InitializeDatabase();
        }

        private void Start()
        {
            Debug.Log("GameManager Start method called.");

            // Validate LevelLoader assignment.
            if (sceneList == null)
            {
                Debug.LogError("LevelLoader is not assigned. Please assign it in the inspector.");
                return;
            }

            // Load localization data.
            LoadLocalization();
        }

        private void OnDestroy()
        {
            // Unsubscribe from log events to prevent memory leaks.
            LogManager.Logger.OnLog -= HandleLogEvent;
        }
        #endregion

        #region Initialization Methods
        /// <summary>
        /// Configures logging by subscribing to LogManager events.
        /// </summary>
        private void ConfigureLogging()
        {
            LogManager.Logger.OnLog += HandleLogEvent;
        }

        /// <summary>
        /// Handles log events and outputs them to Unity's console.
        /// </summary>
        private void HandleLogEvent(object sender, OnDebugEventArgs e)
        {
            if (e == null || e == EventArgs.Empty) return;

            if (e.Error == true)
                Debug.LogError($"[ERR] {Time.frameCount}: {e.Message}");
            else if (e.Error == false)
                Debug.LogWarning($"[WARN] {Time.frameCount}: {e.Message}");
            else
                Debug.Log($"[LOG] {Time.frameCount}: {e.Message}");
        }

        /// <summary>
        /// Initializes the game's database connection and loads data.
        /// </summary>
        private void InitializeDatabase()
        {
            try
            {
                // Set database path and establish connection.
                Game.DatabasePath = "Data Source=..\\veekun-pokedex.sqlite";
                Game.con = new Mono.Data.Sqlite.SqliteConnection(Game.DatabasePath);
                Game.ResetSqlConnection(Game.DatabasePath);

                // Instantiate the game instance.
                game = new Game();
                Debug.Log("Database successfully initialized.");
            }
            catch (InvalidOperationException ex)
            {
                Debug.LogError($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected Error: {ex}");
            }
            finally
            {
                // Ensure the connection is properly closed.
                if (Game.con?.State == System.Data.ConnectionState.Open)
                {
                    Game.con.Close();
                    Debug.Log("Database connection closed.");
                }
            }
        }

        /// <summary>
        /// Loads localization data for the game.
        /// </summary>
        private void LoadLocalization()
        {
            string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
            if (!File.Exists(englishLocalization))
            {
                Debug.LogError($"Localization file not found at {englishLocalization}");
                return;
            }

            Game.LocalizationDictionary = new XmlStringRes(null);
            Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Fires the event to load a new level.
        /// </summary>
        /// <param name="scene">The scene to load.</param>
        public void OnLoadLevel(IScene scene)
        {
            onLoadLevel?.Invoke(scene);
        }
        #endregion
    }
}
