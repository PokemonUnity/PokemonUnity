﻿using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using PokemonUnity;
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

namespace PokemonUnity
{
	/// <summary>
	/// </summary>
	public partial class GameBase : MonoBehaviour//, IGame
	{
		#region Variables
		#endregion

		#region Unity Monobehavior
		void Awake()
		{
			Debug.Log("Game Events is Awake!");
			//UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject

			GameDebug.OnLog += GameDebug_OnLog;
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);
			try
			{
				//GameDebug.Log("0-" + System.IO.Path.GetFullPath("..\\veekun-pokedex.sqlite"));
				//GameDebug.Log("1-" + System.IO.Path.GetFullPath("..\\..\\veekun-pokedex.sqlite"));
				//GameDebug.Log("2-" + System.IO.Path.GetFullPath("..\\..\\..\\veekun-pokedex.sqlite"));
				//GameDebug.Log("3-" + System.IO.Path.GetFullPath("..\\..\\..\\..\\veekun-pokedex.sqlite"));
				Game.DatabasePath = "Data Source=..\\veekun-pokedex.sqlite";
				GameDebug.Log("ConnectionString Database Path: " + Game.DatabasePath);
				//Game.DatabasePath = "Data Source =" + UnityEngine.Application.dataPath + "/Data/veekun-pokedex.sqlite";
				Game.con = (System.Data.IDbConnection)new System.Data.SQLite.SQLiteConnection(Game.DatabasePath);
				//Game.con = new Mono.Data.Sqlite.SqliteConnection(Game.DatabasePath);
				Game.ResetSqlConnection(Game.DatabasePath);//@"Data\veekun-pokedex.sqlite"
				GameDebug.Log("Framework Connected to Database...");
				GameDebug.Log("Path to DB: " + ((System.Data.SQLite.SQLiteConnection)Game.con).FileName);
				//game = new Game();
				GameDebug.Log("New Game Entity Successfully Instantiated!~");
			}
			catch (InvalidOperationException) { GameDebug.LogError("Problem executing SQL with connected database"); }
			catch (Exception e) { GameDebug.LogError(e.ToString()); }
			finally
			{
				//Game.con.Open();

				GameDebug.Log("Is Pokemon DB Null? " + (Kernal.PokemonData == null).ToString());
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
						//Game.InitPokemonEvolutions();
						Game.InitPokemonItems();
						Game.InitMoves();
						Game.InitItems();
						Game.InitBerries();
						Game.InitTrainers();
						//Game.InitRegions();
						//Game.InitLocations();
					}
					catch (Exception) { GameDebug.LogError("there were some problems running sql..."); } //ignore...
				}
				GameDebug.Log(string.Format("Is Pokemon DB Greater than 0? {0} : {1}",
					(Kernal.PokemonData.Count > 0).ToString(), Kernal.PokemonData.Count));
				if (Kernal.PokemonData.Count == 0)
				{
					GameDebug.Log("Was Pokemon DB Successfully Created? " + Game.InitPokemons());
					GameDebug.Log(string.Format("Is Pokemon DB Greater than 0? {0} : {1}",
						(Kernal.PokemonData.Count > 0).ToString(), Kernal.PokemonData.Count));
				}
			}

			GameDebug.Log("Is Game Null? " + (Game.GameData == null).ToString());
			GameDebug.Log("Is Player Null? " + (Game.GameData.Player == null).ToString());
			//if (Game.GameData.Player == null)
			//{
			//	GameDebug.Log("Create Player Object");
			//	//IGamePlayer p = new Player();
			//	GameDebug.Log("Saving Player Object to Global Singleton");
			//	//Game.GameData.Player = p;
			//}
			GameDebug.Log("Is Trainer Null? " + (Game.GameData.Trainer == null).ToString());

			ConfigureScenes();
		}
		void Start()
		{
			GameDebug.Log("Run: {0}", System.Reflection.MethodBase.GetCurrentMethod().Name);

			string englishLocalization = "..\\..\\..\\LocalizationStrings.xml";
			//System.Console.WriteLine(System.IO.Directory.GetParent(englishLocalization).FullName);
			Game.LocalizationDictionary = new XmlStringRes(null); //new Debugger());
			Game.LocalizationDictionary.Initialize(englishLocalization, (int)Languages.English);

			//((object)game.Scenes.BattleScene as GameObject).SetActive(true); //Enable "OnStart" to trigger battle scene...
		}
		#endregion

		#region Methods
		private void ConfigureScenes()
		{
			//ToDo: Load all the different game scenes into an array, from unity inspector, and pass them as an input parameter below
			//(game as Game).SetScenes();
		}
		#endregion

		private static void GameDebug_OnLog(object sender, OnDebugEventArgs e)
		{
			if (e != null || e != System.EventArgs.Empty)
				if (e.Error == true)
					//System.Console.WriteLine("[ERR]: " + e.Message);
					UnityEngine.Debug.LogError("[ERR] " + UnityEngine.Time.frameCount + ": " + e.Message);
				else if (e.Error == false)
					//System.Console.WriteLine("[WARN]: " + e.Message);
					UnityEngine.Debug.LogWarning("[WARN] " + UnityEngine.Time.frameCount + ": " + e.Message);
				else
					//System.Console.WriteLine("[LOG]: " + e.Message);
					UnityEngine.Debug.Log("[LOG] " + UnityEngine.Time.frameCount + ": " + e.Message);
		}
	}
}