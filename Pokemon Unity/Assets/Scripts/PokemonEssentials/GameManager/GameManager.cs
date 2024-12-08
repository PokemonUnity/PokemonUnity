using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
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
using UnityEngine.UI;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// Manager for all the Pokemon game's functions and relationship with Unity Engine
	/// </summary>
	//[ExecuteInEditMode]
	//[RequireComponent(typeof(LevelLoader))]
	public partial class GameManager : MonoBehaviour
	{
		#region Variables
		public static GameManager current;
		public InputManager InputManager;
		public AudioManager AudioManager;
		public FileTest FileTest;
		public IGame game; //game scope used for temp actions, without affecting original copy?
		public event Action onUpdate;
		public event Action onLevelLoaded;
		//public event Action<int> onLoadLevel;
		public event Action<IScene> onLoadLevel;
		//public event Action<IOnLoadLevelEventArgs> onLoadLevel;
		/// <summary>
		/// <see cref="IGameScenesUI"/>
		/// </summary>
		[SerializeField] private LevelLoader sceneList;
		/// <summary>
		/// <see cref="IPokeBattle_SceneIE"/>
		/// </summary>
		//[SerializeField] private BattleScene battle;
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			Debug.Log("Game Manager is Awake!");
			//current = this;
			if (current == null)
			{
				current = this;
			}
			else if (current != this)
			{
				Destroy(gameObject);
			}
			//UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject
			LogManager.Logger.OnLog += (object sender, OnDebugEventArgs e) => {
				if (e != null || e != System.EventArgs.Empty)
					if (e.Error == true)
						//System.Console.WriteLine("[ERR]: " + e.Message);
						global::UnityEngine.Debug.LogError(string.Format("[ERR] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
					else if (e.Error == false)
						//System.Console.WriteLine("[WARN]: " + e.Message);
						global::UnityEngine.Debug.LogWarning(string.Format("[WARN] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
					else
						//System.Console.WriteLine("[LOG]: " + e.Message);
						global::UnityEngine.Debug.Log(string.Format("[LOG] " + global::UnityEngine.Time.frameCount + ": " + e.Message, e.MessageParameters));
			};

			//Debugger.Instance.Init("\\Logs", "GameLog"); //Path = "Logs\GameLog.txt"
			//Core.Logger?.Init("\\Logs", "GameLog"); //Path = "Logs\GameLog.txt"
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//Core.Logger?.Log("0-" + System.IO.Path.GetFullPath("..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("1-" + System.IO.Path.GetFullPath("..\\..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("2-" + System.IO.Path.GetFullPath("..\\..\\..\\veekun-pokedex.sqlite"));
				//Core.Logger?.Log("3-" + System.IO.Path.GetFullPath("..\\..\\..\\..\\veekun-pokedex.sqlite"));
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
				game = new Game();
				Core.Logger?.Log("New Game Entity Successfully Instantiated!~");
			}
			catch (InvalidOperationException) { Core.Logger?.LogError("Problem executing SQL with connected database"); }
			catch (Exception e) { Core.Logger?.LogError(e.ToString()); }
			finally
			{
				//Game.con.Open();

				Core.Logger?.Log("Is Pokemon DB Null? " + (Kernal.PokemonData == null).ToString());
				if (Kernal.PokemonData == null)
				{
					//Game.InitPokemons();
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
			}

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

			//ConfigureScenes();
		}
		void Start()
		{
			Core.Logger?.LogDebug(message: "Run: {0}.{1}", GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);

			string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
			//System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
			Game.LocalizationDictionary = new XmlStringRes(null); //new Debugger());
			Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);

			//Enable "OnStart" to trigger battle scene...
			//((object)game.Scenes?.BattleScene as GameObject)?.SetActive(true); //Scene is already active... Sort later.
		}
		#endregion

		#region Methods
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
		#endregion
	}
}