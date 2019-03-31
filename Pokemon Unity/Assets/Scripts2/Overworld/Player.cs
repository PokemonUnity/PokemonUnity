using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using PokemonUnity.Pokemon;
using PokemonUnity.Character;
using UnityEngine;
using PokemonUnity.Saving;
using PokemonUnity;

namespace P3D
{
	public class Player //: System.Security.HashSecureBase
	{
		#region 
		public string Name
		{
			get
			{
				//Assert("_name", _name);
				return _name;
			}
			set
			{
				//Assert("_name", _name, value);
				_name = value;
			}
		}

		public string RivalName
		{
			get
			{
				return _rivalName;
			}
			set
			{
				_rivalName = value;
			}
		}

		public bool Male
		{
			get
			{
				return _male;
			}
			set
			{
				_male = value;
			}
		}

		public int Money
		{
			get
			{
				return _money;
			}
			set
			{
				_money = value;
			}
		}

		public string OT
		{
			get
			{
				//Assert("_ot", _OT);
				return _OT;
			}
			set
			{
				//Assert("_ot", _OT, value);
				_OT = value;
			}
		}

		public int Points
		{
			get
			{
				return _points;
			}
			set
			{
				_points = value;
			}
		}

		public int BP
		{
			get
			{
				return _BP;
			}
			set
			{
				_BP = value;
			}
		}

		public int Coins
		{
			get
			{
				return _coins;
			}
			set
			{
				_coins = value;
			}
		}

		public bool HasPokedex
		{
			get
			{
				return _hasPokedex;
			}
			set
			{
				_hasPokedex = value;
			}
		}

		public bool HasPokegear
		{
			get
			{
				return _hasPokegear;
			}
			set
			{
				_hasPokegear = value;
			}
		}

		public string LastRestPlace
		{
			get
			{
				return _lastRestPlace;
			}
			set
			{
				_lastRestPlace = value;
			}
		}

		public string LastRestPlacePosition
		{
			get
			{
				return _lastRestPlacePosition;
			}
			set
			{
				_lastRestPlacePosition = value;
			}
		}

		public string LastSavePlace
		{
			get
			{
				return _lastSavePlace;
			}
			set
			{
				_lastSavePlace = value;
			}
		}

		public string LastSavePlacePosition
		{
			get
			{
				return _lastSavePlacePosition;
			}
			set
			{
				_lastSavePlacePosition = value;
			}
		}

		public int RepelSteps
		{
			get
			{
				return _repelSteps;
			}
			set
			{
				_repelSteps = value;
			}
		}

		public string SaveCreated
		{
			get
			{
				return _saveCreated;
			}
			set
			{
				_saveCreated = value;
			}
		}

		public int DaycareSteps
		{
			get
			{
				return _daycareSteps;
			}
			set
			{
				_daycareSteps = value;
			}
		}

		public string GameMode
		{
			get
			{
				return _gameMode;
			}
			set
			{
				_gameMode = value;
			}
		}

		public string Skin
		{
			get
			{
				return _skin;
			}
			set
			{
				_skin = value;
			}
		}

		public string VisitedMaps
		{
			get
			{
				return _visitedMaps;
			}
			set
			{
				_visitedMaps = value;
			}
		}

		public int GTSStars
		{
			get
			{
				return _GTSStars;
			}
			set
			{
				_GTSStars = value;
			}
		}

		public bool SandBoxMode
		{
			get
			{
				//Assert("_sandboxmode", _sandBoxMode);
				return _sandBoxMode;
			}
			set
			{
				//Assert("_sandboxmode", _sandBoxMode, value);
				_sandBoxMode = value;
			}
		}

		public string RegisterData
		{
			get
			{
				return _registerData;
			}
			set
			{
				_registerData = value;
			}
		}

		public string BerryData
		{
			get
			{
				return _berryData;
			}
			set
			{
				_berryData = value;
			}
		}

		public string PokedexData
		{
			get
			{
				return _pokedexData;
			}
			set
			{
				_pokedexData = value;
			}
		}

		public string ItemData
		{
			get
			{
				return _itemData;
			}
			set
			{
				_itemData = value;
			}
		}

		public string BoxData
		{
			get
			{
				return _boxData;
			}
			set
			{
				_boxData = value;
			}
		}

		public string NPCData
		{
			get
			{
				return _NPCData;
			}
			set
			{
				_NPCData = value;
			}
		}

		public string ApricornData
		{
			get
			{
				return _apricornData;
			}
			set
			{
				_apricornData = value;
			}
		}

		public string SecretBaseData
		{
			get
			{
				return _secretBaseData;
			}
			set
			{
				_secretBaseData = value;
			}
		}

		public string DaycareData
		{
			get
			{
				return _daycareData;
			}
			set
			{
				_daycareData = value;
			}
		}

		public string HallOfFameData
		{
			get
			{
				return _hallOfFameData;
			}
			set
			{
				_hallOfFameData = value;
			}
		}

		public string RoamingPokemonData
		{
			get
			{
				return _roamingPokemonData;
			}
			set
			{
				_roamingPokemonData = value;
			}
		}

		public string HistoryData
		{
			get
			{
				return _historyData;
			}
			set
			{
				_historyData = value;
			}
		}

		public bool IsGameJoltSave
		{
			get
			{
				//Assert("_isgamejoltsave", _isGamejoltSave);
				return _isGamejoltSave;
			}
			set
			{
				//Assert("_isgamejoltsave", _isGamejoltSave, value);
				_isGamejoltSave = value;
			}
		}

		public string EmblemBackground
		{
			get
			{
				//Assert("_emblembackground", _emblemBackground);
				return _emblemBackground;
			}
			set
			{
				//Assert("_emblembackground", _emblemBackground, value);
				_emblemBackground = value;
			}
		}
		#endregion
		
		#region Non-base datatypes:
		public List<Pokemon> Pokemons = new List<Pokemon>();
		public List<Pokedex> Pokedexes = new List<Pokedex>();
		//public PlayerInventory Inventory = new PlayerInventory();
		//public List<PokemonUnity.Item.Items> Inventory { get { return GameVariables.Bag_Items; } }
		public GameVariables.TrainerBag Inventory { get { return GameVariables.playerTrainer.Bag; } }
		public List<int> Badges = new List<int>();
		public TimeSpan PlayTime;
		public DateTime GameStart;
		public Vector3 LastPokemonPosition = new Vector3(999, 999, 999);
		public List<string> PokeFiles = new List<string>();
		public List<string> EarnedAchievements = new List<string>();
		public List<int> PokegearModules = new List<int>();
		public List<string> PhoneContacts = new List<string>();
		//public List<PokemonUnity.Item.Item.Mail.MailData> Mails = new List<PokemonUnity.Item.Item.Mail.MailData>();
		public List<int> Trophies = new List<int>();
		#endregion
		
		#region Non-secure fields:
		public int ShowBattleAnimations = 2;
		public int BoxAmount = 10;
		public bool DiagonalMovement = false;
		public int DifficultyMode = 0;
		public int BattleStyle = 1;
		public bool ShowModelsInBattle = true;
		public string TempSurfSkin = "Hilbert";
		public string TempRideSkin = "";
		public string Statistics = "";
		#endregion
		
		#region Secure fields:
		private string _name = "<playername>";
		private string _rivalName = "";
		private bool _male = true;
		private int _money = 0;
		private string _OT = "00000";
		private int _points = 0;
		private int _BP = 0;
		private int _coins = 0;
		private bool _hasPokedex = false;
		private bool _hasPokegear = false;
		private string _lastRestPlace = "yourroom.dat";
		private string _lastRestPlacePosition = "1,0.1f,3";
		private string _lastSavePlace = "yourroom.dat";
		private string _lastSavePlacePosition = "1,0.1f,3";
		private int _repelSteps = 0;
		private string _saveCreated = "Pre 0.21";
		private int _daycareSteps = 0;
		private string _gameMode = "Kolben";
		private string _skin = "Hilbert";
		private string _visitedMaps = "";
		private int _GTSStars = 8;
		private bool _sandBoxMode = false;

		private string _registerData = "";
		private string _berryData = "";
		private string _pokedexData = "";
		private string _itemData = "";
		private string _boxData = "";
		private string _NPCData = "";
		private string _apricornData = "";
		private string _secretBaseData = "";
		private string _daycareData = "";
		private string _hallOfFameData = "";
		private string _roamingPokemonData = "";
		private string _historyData = "";
		private bool _isGamejoltSave = false;
		private string _emblemBackground = "standard";
		#endregion

		public Vector3 startPosition = new Vector3(14, 0.1f, 10);
		public float startRotation = 0;
		public bool startFreeCameraMode = false;
		public string startMap = "barktown.dat";
		public float startFOV = 45.0f;
		public int startRotationSpeed = 12;
		public bool startThirdPerson = false;
		public bool startSurfing = false;
		public bool startRiding = false;

		public string filePrefix = "nilllzz";
		public string newFilePrefix = "";
		public bool AutosaveUsed = false;
		public bool loadedSave = false;

		//public PlayerTemp PlayerTemp = new PlayerTemp();

		public struct Temp
		{
			public static int PokemonScreenIndex = 0;
			public static int PokemonStatusPageIndex = 0;
			public static int BagIndex = 0;
			public static int[] BagPageIndex = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
			public static int[] BagItemIndex = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
			public static int BagSelectIndex = 0;
			public static int MenuIndex = 0;
			public static int PokedexIndex = 0;
			public static int PokemonSummaryPageIndex = 0;
			public static int PCBoxIndex = 0;
			public static Vector2 StorageSystemCursorPosition = new Vector2(1, 0);
			public static int OptionScreenIndex = 0;
			public static bool[] MapSwitch = new bool[4];
			public static Vector3 LastPosition;
			public static bool IsInBattle = false;
			public static Vector3 BeforeBattlePosition = new Vector3(0,0,0);
			public static string BeforeBattleLevelFile = "yourroom.dat";
			public static int BeforeBattleFacing = 0;
			public static int PokedexModeIndex = 0;
			public static int PokedexHabitatIndex = 0;
			public static int PokegearPage = 0;
			public static int LastCall = 32;
			public static int LastUsedRepel = -1;
			public static int MapSteps = 0;
			public static int HallOfFameIndex = 0;
			public static bool PCBoxChooseMode = false;
			//public static StorageSystemScreen.SelectionModes PCSelectionType = StorageSystemScreen.SelectionModes.SingleMove;
			public static decimal RadioStation = 0M;
			//public static GameJolt.PokegearScreen.MenuScreens LastPokegearPage = GameJolt.PokegearScreen.MenuScreens.Main;
		}

		private void ResetTemp()
		{
			Temp.PokemonScreenIndex = 0;
			Temp.PokemonStatusPageIndex = 0;
			Temp.BagIndex = 0;
			Temp.BagPageIndex = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
			Temp.BagItemIndex = new[] { 0, 0, 0, 0, 0, 0, 0, 0 };
			Temp.BagSelectIndex = 0;
			Temp.MenuIndex = 0;
			Temp.PokedexIndex = 0;
			Temp.PCBoxIndex = 0;
			Temp.OptionScreenIndex = 0;
			Temp.IsInBattle = false;
			for (var i = 0; i <= 3; i++)
				Temp.MapSwitch[i] = true;
			Temp.PokedexModeIndex = 0;
			Temp.PokedexHabitatIndex = 0;
			Temp.PokegearPage = 0;
			Temp.LastCall = 32;
			Temp.LastUsedRepel = -1;
			Temp.MapSteps = 0;
			Temp.HallOfFameIndex = 0;
			Temp.PCBoxChooseMode = false;
			Temp.StorageSystemCursorPosition = new Vector2(1, 0);
			//Temp.PCSelectionType = StorageSystemScreen.SelectionModes.SingleMove;
			Temp.RadioStation = 0M;
			//Temp.LastPokegearPage = GameJolt.PokegearScreen.MenuScreens.Main;
		}


		#region Load
		public void LoadGame(string filePrefix)
		{
			//foreach (string s in Core.GameOptions.ContentPackNames)
			//	ContentPackManager.Load(SaveManager.saveLocation + @"\ContentPacks\" + s + @"\exceptions.dat");

			//GameModeManager.CreateGameModesFolder();
			//GameModeManager.CreateKolbenMode();
			//
			//ScriptStorage.Clear();
			//ScriptBlock.TriggeredScriptBlock = false;
			//MysteryEventScreen.ClearActivatedEvents();
			//Pokedex.AutoDetect = true;
			//LevelLoader.ClearTempStructures();
			//BattleSystem.BattleScreen.ResetVars();
			PokemonUnity.Overworld.World.RegionWeatherSet = false;

			this.filePrefix = filePrefix;
			PokeFiles.Clear();
			GameMode = "Kolben";

			LoadPlayer();

			//if (!GameModeManager.GameModeExists(GameMode))
			//{
			//	GameMode = "Kolben";
			//	GameModeManager.SetGameModePointer("Kolben");
			//}
			//else
			//	GameModeManager.SetGameModePointer(GameMode);

			//BattleSystem.GameModeAttackLoader.Load();

			if (IsGameJoltSave)
				SandBoxMode = false;

			//Localization.ReloadGameModeTokens();

			//SmashRock.Load();
			//Badge.Load();
			//Pokedex.Load();
			//PokemonInteractions.Load();
			//PokemonForms.Initialize();

			LoadPokedex();
			LoadParty();
			LoadItems();
			LoadBerries();
			LoadApricorns();
			LoadDaycare();
			LoadOptions();
			LoadRegister();
			LoadItemData();
			LoadBoxData();
			LoadNPCData();
			LoadHallOfFameData();
			LoadSecretBaseData();
			LoadRoamingPokemonData();
			LoadStatistics();

			//PlayerTemp.Reset();
			//ResetTemp();
			//Chat.ClearChat();

			if (AutosaveUsed)
			{
				System.IO.Directory.Delete(SaveManager.saveLocation + @"\Save\" + this.filePrefix, true);

				this.filePrefix = newFilePrefix;
				AutosaveUsed = false;

				string outputString = newFilePrefix;

				//Core.GameMessage.ShowMessage(Localization.GetString("game_message_continue_autosave") + " \"" + outputString + "\"", 12, FontManager.MainFont, Color.White);

				newFilePrefix = "";
			}

			//if (IsGameJoltSave)
			//{
			//	lastLevel = GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points);
			//	OT = GameJoltSave.GameJoltID;
			//}
			//
			//Entity.MakeShake = Name.ToLower() == "drunknilllzz";

			/// Indev 0.54 Removal List
			/// 1. All Mega Stones. [ID: 507 - 553]
			/// 2. Shiny Candy [ID: 501]
			//if (!ActionScript.IsRegistered("PokemonIndev054Update"))
			//{
			//	// Check Inventory.
			//	Inventory.RemoveItem((PokemonUnity.Item.Items)501);
			//	//Inventory.Remove((PokemonUnity.Item.Items)501);
			//	for (int i = 507; i >= 553; i += +1)
			//		Inventory.RemoveItem((PokemonUnity.Item.Items)i);
			//		//Inventory.Remove((PokemonUnity.Item.Items)i);
			//
			//	// Check Party Pokemon.
			//	foreach (Pokemon Pokemon in Pokemons)
			//	{
			//		if (Pokemon.Item != PokemonUnity.Item.Items.NONE && (Pokemon.Item >= (PokemonUnity.Item.Items)501 || (Pokemon.Item >= (PokemonUnity.Item.Items)507 && Pokemon.Item <= (PokemonUnity.Item.Items)553)))
			//			//Pokemon.Item = PokemonUnity.Item.Items.NONE;
			//			Pokemon.SwapItem(PokemonUnity.Item.Items.NONE);
			//	}
			//
			//	// Check PC Boxes.
			//	if (!string.IsNullOrEmpty(BoxData))
			//	{
			//		List<string> TempBoxData = new List<string>();
			//		TempBoxData.AddRange(BoxData.SplitAtNewline());
			//
			//		foreach (string item in TempBoxData)
			//		{
			//			if (!string.IsNullOrEmpty(item) && !item.StartsWith("BOX"))
			//			{
			//				string TempString = item.Remove(item.IndexOf("{"));
			//				//Pokemon TempPokemon = Pokemon.GetPokemonByData((Pokemons)System.Convert.ToInt32(item.Remove(0, item.IndexOf("{"))));
			//				Pokemon TempPokemon = new Pokemon((Pokemons)System.Convert.ToInt32(item.Remove(0, item.IndexOf("{"))));
			//
			//				if (TempPokemon.Item != PokemonUnity.Item.Items.NONE && (TempPokemon.Item >= (PokemonUnity.Item.Items)501 || (TempPokemon.Item >= (PokemonUnity.Item.Items)507 && TempPokemon.Item <= (PokemonUnity.Item.Items)553)))
			//					//TempPokemon.Item = PokemonUnity.Item.Items.NONE;
			//					TempPokemon.SwapItem(PokemonUnity.Item.Items.NONE);
			//
			//				//item = TempString + TempPokemon.ToString();
			//			}
			//		}
			//
			//		BoxData = string.Join(Environment.NewLine, TempBoxData.ToArray());
			//	}
			//
			//	// Check Day Care.
			//	if (!string.IsNullOrEmpty(DaycareData))
			//	{
			//		List<string> TempDaycareData = new List<string>();
			//		TempDaycareData.AddRange(DaycareData.SplitAtNewline());
			//
			//		foreach (string item in TempDaycareData)
			//		{
			//			if (!string.IsNullOrEmpty(item) && item.Contains("{"))
			//			{
			//				string TempString = ItemData.Remove(item.IndexOf("{"));
			//				//Pokemon TempPokemon = Pokemon.GetPokemonByData(item.Remove(0, item.IndexOf("{")));
			//				Pokemon TempPokemon = new Pokemon((Pokemons)System.Convert.ToInt32(item.Remove(0, item.IndexOf("{"))));
			//
			//				if (TempPokemon.Item != PokemonUnity.Item.Items.NONE && (TempPokemon.Item >= (PokemonUnity.Item.Items)501 || (TempPokemon.Item >= (PokemonUnity.Item.Items)507 && TempPokemon.Item <= (PokemonUnity.Item.Items)553)))
			//					//TempPokemon.Item = PokemonUnity.Item.Items.NONE;
			//					TempPokemon.SwapItem(PokemonUnity.Item.Items.NONE);
			//
			//				//item = TempString + TempPokemon.ToString();
			//			}
			//		}
			//
			//		DaycareData = string.Join(Environment.NewLine, TempDaycareData.ToArray());
			//	}
			//
			//	ActionScript.RegisterID("PokemonIndev054Update");
			//}

			/// Indev 0.54.2 OT Fix List.
			//if (!ActionScript.IsRegistered("PokemonIndev0542Update"))
			//{
			//	// Check Party Pokemon.
			//	foreach (Pokemon Pokemon in Pokemons)
			//	{
			//		if (string.Equals(Pokemon.CatchTrainerName, GameVariables.playerTrainer.PlayerName, StringComparison.OrdinalIgnoreCase) && Pokemon.OT != GameJoltSave.GameJoltID)
			//			Pokemon.OT = GameJoltSave.GameJoltID;
			//	}
			//
			//	// Check PC Boxes.
			//	if (!string.IsNullOrEmpty(BoxData))
			//	{
			//		List<string> TempBoxData = new List<string>();
			//		TempBoxData.AddRange(BoxData.SplitAtNewline());
			//
			//		foreach (string item in TempBoxData)
			//		{
			//			if (!string.IsNullOrEmpty(item) && !item.StartsWith("BOX"))
			//			{
			//				string TempString = item.Remove(item.IndexOf("{"));
			//				Pokemon TempPokemon = Pokemon.GetPokemonByData(item.Remove(0, item.IndexOf("{")));
			//
			//				if (string.Equals(TempPokemon.CatchTrainerName, GameVariables.playerTrainer.PlayerName, StringComparison.OrdinalIgnoreCase) && TempPokemon.OT != GameJoltSave.GameJoltID)
			//					TempPokemon.OT = GameJoltSave.GameJoltID;
			//
			//				item = TempString + TempPokemon.ToString();
			//			}
			//		}
			//
			//		BoxData = string.Join(Environment.NewLine, TempBoxData);
			//	}
			//
			//	// Check Day Care.
			//	if (!string.IsNullOrEmpty(DaycareData))
			//	{
			//		List<string> TempDaycareData = new List<string>();
			//		TempDaycareData.AddRange(DaycareData.SplitAtNewline());
			//
			//		foreach (string item in TempDaycareData)
			//		{
			//			if (!string.IsNullOrEmpty(item) && item.Contains("{"))
			//			{
			//				string TempString = ItemData.Remove(item.IndexOf("{"));
			//				Pokemon TempPokemon = Pokemon.GetPokemonByData(item.Remove(0, item.IndexOf("{")));
			//
			//				if (string.Equals(TempPokemon.CatchTrainerName, GameVariables.playerTrainer.PlayerName, StringComparison.OrdinalIgnoreCase) && TempPokemon.OT != GameJoltSave.GameJoltID)
			//					TempPokemon.OT = GameJoltSave.GameJoltID;
			//
			//				item = TempString + TempPokemon.ToString();
			//			}
			//		}
			//
			//		DaycareData = string.Join(Environment.NewLine, TempDaycareData);
			//	}
			//
			//	// Remove Duplicate data.
			//	GameVariables.playerTrainer.PokeFiles = GameVariables.playerTrainer.PokeFiles.Distinct().ToList();
			//
			//	ActionScript.RegisterID("PokemonIndev0542Update");
			//}

			loadedSave = true;
		}

		private void LoadParty()
		{
			Pokemons.Clear();

			string[] PokeData;
			//if (IsGameJoltSave)
			//	PokeData = GameJoltSave.Party.SplitAtNewline();
			//else
				PokeData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Party.dat").SplitAtNewline();

			if (PokeData.Count() > 0 && PokeData[0] != "")
			{
				foreach (string Line in PokeData)
				{
					if (Line.StartsWith("{") & Line.EndsWith("}"))
					{
						Pokemon p = new Pokemon();//Pokemon.GetPokemonByData(Line);

						if (!p.isEgg)
						{
							if (p.IsShiny)
								//PokedexData = Pokedex.ChangeEntry(PokedexData, p.Number, 3);
								GameVariables.playerTrainer.PlayerPokedex[(int)p.Species, 1] = 2;
							else
								//PokedexData = Pokedex.ChangeEntry(PokedexData, p.Number, 2);
								GameVariables.playerTrainer.PlayerPokedex[(int)p.Species, 1] = 1;
						}

						Pokemons.Add(p);
					}
				}
			}
		}

		private void LoadPlayer()
		{
			string[] Data;
			//if (IsGameJoltSave)
			//	Data = GameJoltSave.Player.SplitAtNewline();
			//else
				Data = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Player.dat").SplitAtNewline();

			if (GameVariables.Level != null)
				GameVariables.Level.Riding = false;

			foreach (string Line in Data)
			{
				if (Line != "" & Line.Contains("|"))
				{
					string ID = Line.Remove(Line.IndexOf("|"));
					string Value = Line.Remove(0, Line.IndexOf("|") + 1);
					switch (ID.ToLower())
					{
						case "name":
							{
								Name = Value;

								//if (IsGameJoltSave)
								//{
								//	if (Name.ToLower() != GameJolt.API.username.ToLower())
								//		Name = GameJolt.API.username;
								//}

								break;
							}
						case "position":
							{
								string[] v = Value.Split(System.Convert.ToChar(","));
								startPosition.x = System.Convert.ToSingle(v[0].Replace(".", StringHelper.DecSeparator));
								startPosition.y = System.Convert.ToSingle(v[1].Replace(".", StringHelper.DecSeparator));
								startPosition.z = System.Convert.ToSingle(v[2].Replace(".", StringHelper.DecSeparator));
								break;
							}
						case "lastpokemonposition":
							{
								string[] v = Value.Split(System.Convert.ToChar(","));
								LastPokemonPosition.x = System.Convert.ToSingle(v[0].Replace(".", StringHelper.DecSeparator));
								LastPokemonPosition.y = System.Convert.ToSingle(v[1].Replace(".", StringHelper.DecSeparator));
								LastPokemonPosition.z = System.Convert.ToSingle(v[2].Replace(".", StringHelper.DecSeparator));
								break;
							}
						case "mapfile":
							{
								startMap = Value;
								break;
							}
						case "rivalname":
							{
								RivalName = Value;
								break;
							}
						case "money":
							{
								Money = System.Convert.ToInt32(Value);
								break;
							}
						case "badges":
							{
								Badges.Clear();

								if (Value == "0")
									Badges = new List<int>();
								else if (!Value.Contains(","))
									Badges = new int[]	
									{
										System.Convert.ToInt32(Value)

									}.ToList();
								else
								{
									List<string> l = Value.Split(System.Convert.ToChar(",")).ToList();

									for (var i = 0; i <= l.Count - 1; i++)
										Badges.Add(System.Convert.ToInt32(l[i]));
								}

								break;
							}
						case "rotation":
							{
								startRotation = System.Convert.ToSingle(Value.Replace(".", StringHelper.DecSeparator));
								break;
							}
						case "Gender":
							{
								if (Value == "Male")
									Male = true;
								else
									Male = false;
								break;
							}
						case "playtime":
							{
								string[] dd = Value.Split(System.Convert.ToChar(","));
								if (dd.Count() >= 4)
									PlayTime = new TimeSpan(System.Convert.ToInt32(dd[3]), System.Convert.ToInt32(dd[0]), System.Convert.ToInt32(dd[1]), System.Convert.ToInt32(dd[2]));
								else
									PlayTime = new TimeSpan(System.Convert.ToInt32(dd[0]), System.Convert.ToInt32(dd[1]), System.Convert.ToInt32(dd[2]));
								break;
							}
						case "ot":
							{
								OT = System.Convert.ToString(System.Convert.ToInt32(Value).Clamp(0, 99999));
								break;
							}
						case "points":
							{
								Points = System.Convert.ToInt32(Value);
								break;
							}
						case "haspokedex":
							{
								HasPokedex = System.Convert.ToBoolean(Value);
								break;
							}
						case "haspokegear":
							{
								HasPokegear = System.Convert.ToBoolean(Value);
								break;
							}
						case "freecamera":
							{
								startFreeCameraMode = System.Convert.ToBoolean(Value);
								break;
							}
						case "thirdperson":
							{
								startThirdPerson = System.Convert.ToBoolean(Value);
								break;
							}
						case "skin":
							{
								Skin = Value;
								break;
							}
						case "battleanimations":
							{
								ShowBattleAnimations = System.Convert.ToInt32(Value);
								break;
							}
						case "boxamount":
							{
								BoxAmount = System.Convert.ToInt32(Value);
								break;
							}
						case "lastrestplace":
							{
								LastRestPlace = Value;
								break;
							}
						case "lastrestplaceposition":
							{
								LastRestPlacePosition = Value;
								break;
							}
						case "diagonalmovement":
							{
								if (GameVariables.IS_DEBUG_ACTIVE)
									DiagonalMovement = System.Convert.ToBoolean(Value);
								else
									DiagonalMovement = false;
								break;
							}
						case "repelsteps":
							{
								RepelSteps = System.Convert.ToInt32(Value);
								break;
							}
						case "lastsaveplace":
							{
								LastSavePlace = Value;
								break;
							}
						case "lastsaveplaceposition":
							{
								LastSavePlacePosition = Value;
								break;
							}
						case "difficulty":
							{
								DifficultyMode = System.Convert.ToInt32(Value);
								break;
							}
						case "battlestyle":
							{
								BattleStyle = System.Convert.ToInt32(Value);
								break;
							}
						case "savecreated":
							{
								SaveCreated = Value;
								break;
							}
						case "autosave":
							{
								if (!IsGameJoltSave)
								{
									newFilePrefix = Value;
									AutosaveUsed = true;
								}

								break;
							}
						case "daycaresteps":
							{
								DaycareSteps = System.Convert.ToInt32(Value);
								break;
							}

						case "gamemode":
							{
								GameMode = Value;
								break;
							}

						case "pokefiles":
							{
								if (Value != "")
								{
									if (Value.Contains(","))
										PokeFiles.AddRange(Value.Split(System.Convert.ToChar(",")));
									else
										PokeFiles.Add(Value);
								}

								break;
							}

						case "visitedmaps":
							{
								VisitedMaps = Value;
								break;
							}

						case "tempsurfskin":
							{
								TempSurfSkin = Value;
								break;
							}

						case "surfing":
							{
								startSurfing = System.Convert.ToBoolean(Value);
								break;
							}

						case "bp":
							{
								BP = System.Convert.ToInt32(Value);
								break;
							}

						case "gtsstars":
							{
								GTSStars = System.Convert.ToInt32(Value);
								break;
							}

						case "showmodels":
							{
								ShowModelsInBattle = System.Convert.ToBoolean(Value);
								break;
							}

						case "sandboxmode":
							{
								SandBoxMode = System.Convert.ToBoolean(Value);
								break;
							}

						case "earnedachievements":
							{
								if (Value != "")
									EarnedAchievements = Value.Split(System.Convert.ToChar(",")).ToList();
								break;
							}
					}
				}
				else
					GameVariables.DebugLog("Player.vb: The line \"" + Line + "\" is either empty or does not conform the player.dat file rules.", false);
			}

			if (IsGameJoltSave & !startSurfing)
			{
				//Skin = GameJolt.Emblem.GetPlayerSpriteFile(GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points), GameJoltSave.GameJoltID, GameJoltSave.Gender);
				//switch (GameJoltSave.Gender)
				//{
				//	case "0":
				//		{
				//			Male = true;
				//			break;
				//		}
				//	case "1":
				//		{
				//			Male = false;
				//			break;
				//		}
				//	default:
				//		{
				//			Male = true;
				//			break;
				//		}
				//}
			}

			GameStart = DateTime.Now;
		}

		private void LoadOptions()
		{
			string[] Data;
			//if (IsGameJoltSave)
			//	Data = GameJoltSave.Options.SplitAtNewline();
			//else
				Data = System.IO.File.ReadAllLines(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Options.dat");

			foreach (string Line in Data)
			{
				if (Line.Contains("|"))
				{
					string ID = Line.Remove(Line.IndexOf("|"));
					string Value = Line.Remove(0, Line.IndexOf("|") + 1);
					switch (ID.ToLower())
					{
						case "fov":
							{
								startFOV = System.Convert.ToSingle(Value.Replace(".", StringHelper.DecSeparator)).Clamp(1, 179);
								break;
							}
						case "textspeed":
							{
								GameVariables.textSpeed = System.Convert.ToByte(Value);
								break;
							}
						case "mousespeed":
							{
								startRotationSpeed = System.Convert.ToInt32(Value);
								break;
							}
					}
				}
			}
		}

		private void LoadItems()
		{
			//Inventory.Clear();
			GameVariables.Bag_Items.Clear();
			//Mails.Clear();

			string Data;
			//if (IsGameJoltSave)
			//	Data = GameJoltSave.Items;
			//else
				Data = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Items.dat");

			if (Data != "")
			{
				string[] ItemData = Data.SplitAtNewline();

				foreach (string ItemDat in ItemData)
				{
					if (ItemDat != "" & ItemDat.StartsWith("{") & ItemDat.EndsWith("}") & ItemDat.Contains("|"))
					{
						string ItemID = ItemDat.Remove(0, ItemDat.IndexOf("{") + 1);
						ItemID = ItemID.Remove(ItemID.IndexOf("}"));

						int amount = System.Convert.ToInt32(ItemID.Remove(0, ItemID.IndexOf("|") + 1));
						ItemID = ItemID.Remove(ItemID.IndexOf("|"));

						Inventory.AddItem((PokemonUnity.Item.Items)System.Convert.ToInt32(ItemID), amount);
					}
					else if (ItemDat != "" & ItemDat.StartsWith("Mail|"))
					{
						string mailData = ItemDat.Remove(0, 5);
						//Mails.Add(Items.MailItem.GetMailDataFromString(mailData));
					}
				}
			}
		}
		#endregion

		#region Loading Misc Data
		private void LoadBerries()
		{
			//if (IsGameJoltSave)
			//	GameVariables.playerTrainer.BerryData = GameJoltSave.Berries;
			//else
			//	GameVariables.playerTrainer.BerryData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Berries.dat");
		}

		private void LoadApricorns()
		{
			//if (IsGameJoltSave)
			//	GameVariables.playerTrainer.ApricornData = GameJoltSave.Apricorns;
			//else
			//	GameVariables.playerTrainer.ApricornData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Apricorns.dat");
		}

		private void LoadDaycare()
		{
			//GameVariables.playerTrainer.DaycareData = "";
			//if (IsGameJoltSave)
			//	GameVariables.playerTrainer.DaycareData = GameJoltSave.Daycare;
			//else if (System.IO.File.Exists(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Daycare.dat"))
			//	GameVariables.playerTrainer.DaycareData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Daycare.dat");
		}

		private void LoadPokedex()
		{
			//if (IsGameJoltSave)
			//	PokedexData = GameJoltSave.Pokedex;
			//else
			//	PokedexData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Pokedex.dat");
			//
			//if (PokedexData == "")
			//	PokedexData = Pokedex.NewPokedex();
		}

		private void LoadRegister()
		{
			//if (IsGameJoltSave)
			//	RegisterData = GameJoltSave.Register;
			//else
			//	RegisterData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Register.dat");
		}

		private void LoadItemData()
		{
			//if (IsGameJoltSave)
			//	ItemData = GameJoltSave.ItemData;
			//else
			//	ItemData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\ItemData.dat");
		}

		private void LoadBoxData()
		{
			//if (IsGameJoltSave)
			//	BoxData = GameJoltSave.Box;
			//else
			//	BoxData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Box.dat");
		}

		private void LoadNPCData()
		{
			//if (IsGameJoltSave)
			//	NPCData = GameJoltSave.NPC;
			//else
			//	NPCData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\NPC.dat");
		}

		private void LoadHallOfFameData()
		{
			//if (IsGameJoltSave)
			//	HallOfFameData = GameJoltSave.HallOfFame;
			//else if (System.IO.File.Exists(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\HallOfFame.dat"))
			//	HallOfFameData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\HallOfFame.dat");
			//else
			//	HallOfFameData = "";
		}

		private void LoadSecretBaseData()
		{
			//if (IsGameJoltSave)
			//	SecretBaseData = GameJoltSave.SecretBase;
			//else if (System.IO.File.Exists(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\SecretBase.dat"))
			//	SecretBaseData = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\SecretBase.dat");
			//else
			//	SecretBaseData = "";
		}

		private void LoadRoamingPokemonData()
		{
			//RoamingPokemonData = "";
			//if (IsGameJoltSave)
			//	RoamingPokemonData = GameJoltSave.RoamingPokemon;
			//else if (System.IO.File.Exists(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\RoamingPokemon.dat"))
			//{
			//	foreach (string line in System.IO.File.ReadAllLines(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\RoamingPokemon.dat"))
			//	{
			//		if (RoamingPokemonData != "")
			//			RoamingPokemonData += Environment.NewLine;
			//		if (line.CountSeperators("|") < 5)
			//		{
			//			// Convert potential old data:
			//			string[] data = line.Split(System.Convert.ToChar("|"));
			//			Pokemon newP = new Pokemon((Pokemons)System.Convert.ToInt32(data[0]));
			//			newP.Generate(System.Convert.ToInt32(data[1]), true);
			//
			//			RoamingPokemonData += ((int)newP.Species).ToString() + "|" + newP.Level.ToString() + "|" + data[2] + "|" + data[3] + "||" + newP.GetSaveData();
			//		}
			//		else
			//			RoamingPokemonData += line;
			//	}
			//}
		}

		private void LoadStatistics()
		{
			//if (IsGameJoltSave)
			//	Statistics = GameJoltSave.Statistics;
			//else if (System.IO.File.Exists(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Statistics.dat"))
			//	Statistics = System.IO.File.ReadAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Statistics.dat");
			//else
			//	Statistics = "";
			//PlayerStatistics.Load(Statistics);
		}
		#endregion

		private Dictionary<string, string> GameJoltTempStoreString = new Dictionary<string, string>();

		public void SaveGame(bool IsAutosave)
		{
			//SaveGameHelpers.ResetSaveCounter();

			if (IsAutosave)
			{
				newFilePrefix = filePrefix;
				filePrefix = "autosave";

				//if (!System.IO.Directory.Exists(SaveManager.saveLocation + @"\Save\autosave"))
				//	System.IO.Directory.CreateDirectory(SaveManager.saveLocation + @"\Save\autosave");
			}
			else
				newFilePrefix = filePrefix;

			GameJoltTempStoreString.Clear();

			if (!IsGameJoltSave)
			{
				//if (System.IO.Directory.Exists(SaveManager.saveLocation + @"\Save\" + !filePrefix))
				//	System.IO.Directory.CreateDirectory(SaveManager.saveLocation + @"\Save\" + filePrefix);
			}

			SavePlayer(IsAutosave);
			SaveParty();
			SaveItems();
			SaveBerries();
			SaveApricorns();
			SaveDaycare();
			SaveOptions();
			SavePokedex();
			SaveRegister();
			SaveItemData();
			SaveBoxData();
			SaveNPCData();
			SaveHallOfFameData();
			SaveSecretBaseData();
			SaveRoamingPokemonData();
			SaveStatistics();

			filePrefix = newFilePrefix;

			if (IsGameJoltSave)
			{
				//if (Core.GameOptions.Extras.Contains("Backup Save Feature"))
				//{
				//	if (!Directory.Exists(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID.ToString() + "/Encrypted"))
				//		Directory.CreateDirectory(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID.ToString() + "/Encrypted");
				//
				//	string OriginalHASH = Encryption.EncryptString(GetApricornsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetBerriesData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetBoxData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetDaycareData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetHallOfFameData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetItemDataData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetItemsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetNPCDataData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetOptionsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetPartyData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetPlayerData(false), StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetPokedexData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetRegisterData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetRoamingPokemonData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetSecretBaseData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)) + "|" + Encryption.EncryptString(GetStatisticsData, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID));
				//
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID.ToString() + "/Encrypted/Encrypted.dat", OriginalHASH + "|" + Encryption.EncryptString(OriginalHASH, StringObfuscation.Obfuscate(GameJoltSave.GameJoltID)));
				//
				//	string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss");
				//
				//	if (!Directory.Exists(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp))
				//		Directory.CreateDirectory(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp);
				//
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Apricorns.dat", GetApricornsData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Berries.dat", GetBerriesData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Box.dat", GetBoxData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Daycare.dat", GetDaycareData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/HallOfFame.dat", GetHallOfFameData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/ItemData.dat", GetItemDataData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Items.dat", GetItemsData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/NPC.dat", GetNPCDataData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Options.dat", GetOptionsData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Party.dat", GetPartyData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Player.dat", GetPlayerData(false));
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Pokedex.dat", GetPokedexData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Register.dat", GetRegisterData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/RoamingPokemon.dat", GetRoamingPokemonData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/SecretBase.dat", GetSecretBaseData);
				//	File.WriteAllText(SaveManager.saveLocation + "/Backup Save/" + GameJoltSave.GameJoltID + "/" + timestamp + "/Statistics.dat", GetStatisticsData);
				//}

				//GameJolt.APICall APICallSave = new GameJolt.APICall(SaveGameHelpers.CompleteGameJoltSave);
				//APICallSave.CallFails += Exception ex =>
				//{
				//	SaveGameHelpers.CompleteGameJoltSave("false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false" + Environment.NewLine + "false");
				//};

				List<string> keys = new List<string>();
				List<string> dataItems = new List<string>();
				List<bool> useUsername = new List<bool>();

				//for (var i = 0; i <= GameJoltTempStoreString.Count - 1; i++)
				//{
				//	keys.Add(GameJoltTempStoreString.Keys(i));
				//	dataItems.Add(GameJoltTempStoreString.Values(i));
				//	useUsername.Add(true);
				//}
				//APICallSave.SetStorageData(keys.ToArray(), dataItems.ToArray(), useUsername.ToArray());

				//SavePublicVars();

				//GameJoltSave.UpdatePlayerScore();
			}
		}

		//private void SavePublicVars()
		//{
		//	if (!GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID))
		//	{
		//		GameJolt.APICall APICallPoints = new GameJolt.APICall(SaveGameHelpers.AddGameJoltSaveCounter);
		//		APICallPoints.SetStorageData("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|points", GameJoltSave.Points.ToString(), false);
		//
		//		GameJolt.APICall APICallEmblem = new GameJolt.APICall(SaveGameHelpers.AddGameJoltSaveCounter);
		//		APICallEmblem.SetStorageData("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|emblem", GameJoltSave.Emblem, false);
		//
		//		GameJolt.APICall APICallGender = new GameJolt.APICall(SaveGameHelpers.AddGameJoltSaveCounter);
		//		APICallGender.SetStorageData("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|gender", GameJoltSave.Gender, false);
		//	}
		//}

		#region Get
		public string GetPartyData()
		{
			string Data = "";
			for (var i = 0; i <= Pokemons.Count - 1; i++)
			{
				if (Data != "")
					Data += Environment.NewLine;
				//Data += Pokemons[i].GetSaveData();
			}
			return Data;
		}

		public string GetPlayerData(bool IsAutosave)
		{
			string GenderString = "";
			if (Male)
				GenderString = "Male";
			else
				GenderString = "Female";

			string badgeString = "";
			if (Badges.Count > 0)
			{
				for (var i = 0; i <= Badges.Count - 1; i++)
				{
					if (i != 0)
						badgeString += ",";
					badgeString += Badges[i].ToString();
				}
			}
			else
				badgeString = "0";

			string hasPokedexString = HasPokedex.ToNumberString();

			//OverworldCamera c = GetOverworldCamera();
			//string freeCameraString = c.FreeCameraMode.ToNumberString();

			TimeSpan diff = (DateTime.Now - GameStart);
			TimeSpan p = PlayTime + diff;
			string PlayTimeString = p.Hours + "," + p.Minutes + "," + p.Seconds + "," + p.Days;

			string lastPokemonPosition = "999,999,999";
			if (GameVariables.Level.OverworldPokemon.Visible)
				lastPokemonPosition = (GameVariables.Level.OverworldPokemon.Position.x.ToString().Replace(StringHelper.DecSeparator, ".") + "," + GameVariables.Level.OverworldPokemon.Position.y.ToString().Replace(StringHelper.DecSeparator, ".") + "," + GameVariables.Level.OverworldPokemon.Position.z.ToString().Replace(StringHelper.DecSeparator, "."));

			string PokeFilesString = "";
			if (PokeFiles.Count > 0)
			{
				foreach (string pokefile in PokeFiles)
				{
					if (PokeFilesString != "")
						PokeFilesString += ",";

					PokeFilesString += pokefile;
				}
			}

			string EarnedAchievementsString = "";
			if (EarnedAchievements.Count > 0)
			{
				foreach (string ea in EarnedAchievements)
				{
					if (EarnedAchievementsString != "")
						EarnedAchievementsString += ",";

					EarnedAchievementsString += ea;
				}
			}

			string skin = GameVariables.Level.OwnPlayer.SkinName;
			if (GameVariables.Level.Riding)
				skin = TempRideSkin;

			string Data = "";//"Name|" + Name + Environment.NewLine + "Position|" + c.Position.X.ToString().Replace(StringHelper.DecSeparator, ".") + "," + c.Position.Y.ToString.Replace(StringHelper.DecSeparator, ".") + "," + c.Position.Z.ToString().Replace(StringHelper.DecSeparator, ".") + Environment.NewLine + "MapFile|" + GameVariables.Level.LevelFile + Environment.NewLine + "Rotation|" + c.Yaw.ToString.Replace(StringHelper.DecSeparator, ".") + Environment.NewLine + "RivalName|" + RivalName + Environment.NewLine + "Money|" + Money + Environment.NewLine + "Badges|" + badgeString + Environment.NewLine + "Gender|" + GenderString + Environment.NewLine + "PlayTime|" + PlayTimeString + Environment.NewLine + "OT|" + OT + Environment.NewLine + "Points|" + Points.ToString() + Environment.NewLine + "hasPokedex|" + hasPokedexString + Environment.NewLine + "hasPokegear|" + HasPokegear.ToNumberString() + Environment.NewLine + "freeCamera|" + freeCameraString + Environment.NewLine + "thirdPerson|" + c.ThirdPerson.ToNumberString() + Environment.NewLine + "skin|" + skin + Environment.NewLine + "location|" + GameVariables.Level.MapName + Environment.NewLine + "battleAnimations|" + ShowBattleAnimations.ToString() + Environment.NewLine + "BoxAmount|" + BoxAmount.ToString() + Environment.NewLine + "LastRestPlace|" + LastRestPlace + Environment.NewLine + "LastRestPlacePosition|" + LastRestPlacePosition + Environment.NewLine + "DiagonalMovement|" + DiagonalMovement.ToNumberString() + Environment.NewLine + "RepelSteps|" + RepelSteps.ToString() + Environment.NewLine + "LastSavePlace|" + LastSavePlace + Environment.NewLine + "LastSavePlacePosition|" + LastSavePlacePosition + Environment.NewLine + "Difficulty|" + DifficultyMode.ToString() + Environment.NewLine + "BattleStyle|" + BattleStyle.ToString() + Environment.NewLine + "saveCreated|" + SaveCreated + Environment.NewLine + "LastPokemonPosition|" + lastPokemonPosition + Environment.NewLine + "DaycareSteps|" + DaycareSteps.ToString() + Environment.NewLine + "GameMode|" + GameMode + Environment.NewLine + "PokeFiles|" + PokeFilesString + Environment.NewLine + "VisitedMaps|" + VisitedMaps + Environment.NewLine + "TempSurfSkin|" + TempSurfSkin + Environment.NewLine + "Surfing|" + GameVariables.Level.Surfing.ToNumberString() + Environment.NewLine + "BP|" + BP + Environment.NewLine + "ShowModels|" + ShowModelsInBattle.ToNumberString() + Environment.NewLine + "GTSStars|" + GTSStars + Environment.NewLine + "SandBoxMode|" + SandBoxMode.ToNumberString() + Environment.NewLine + "EarnedAchievements|" + EarnedAchievementsString;

			if (IsAutosave)
				Data += Environment.NewLine + "AutoSave|" + newFilePrefix;

			return Data;
		}

		public string GetOptionsData()
		{
			//OverworldCamera c = GetOverworldCamera();
			//
			//string FOVstring = c.FOV.ToString.Replace(",", ".");
			//string MouseSpeedString = System.Convert.ToString(c.RotationSpeed * 10000);
			//string TextSpeedString = System.Convert.ToString(GameVariables.textSpeed);
			//
			//string Data = "FOV|" + FOVstring + Environment.NewLine + "TextSpeed|" + TextSpeedString + Environment.NewLine + "MouseSpeed|" + MouseSpeedString;

			return "";//Data;
		}

		public string GetItemsData()
		{
			string Data = "";

			foreach (var c in Inventory.Sort(GameVariables.TrainerBag.Order.Category))
			{
				if (Data != "")
					Data += Environment.NewLine;

				Data += "{" + c.Key.ItemId + "|" + c.Value + "}";
			}

			//foreach (Item.Mail.MailData mail in Mails)
			//{
			//	if (Data != "")
			//		Data += Environment.NewLine;
			//	Data += "Mail|" + Item.Mail.GetStringFromMail(mail);
			//}

			return Data;
		}

		public string GetBerriesData()
		{
			return BerryData;
		}

		public string GetApricornsData()
		{
			return ApricornData;
		}

		public string GetDaycareData()
		{
			return DaycareData;
		}

		public string GetPokedexData()
		{
			return PokedexData;
		}

		public string GetRegisterData()
		{
			return RegisterData;
		}

		public string GetItemDataData()
		{
			return ItemData;
		}

		public string GetBoxData()
		{
			return BoxData;
		}

		public string GetNPCDataData()
		{
			return NPCData;
		}

		public string GetHallOfFameData()
		{
			return HallOfFameData;
		}

		public string GetSecretBaseData()
		{
			return SecretBaseData;
		}

		public string GetRoamingPokemonData()
		{
			return RoamingPokemonData;
		}

		public string GetStatisticsData()
		{
			return Statistics;
		}

		//private OverworldCamera GetOverworldCamera()
		//{
		//	Screen baseScreen = CurrentScreen;
		//	while (!baseScreen.PreScreen == null)
		//		baseScreen = baseScreen.PreScreen;
		//
		//	if (baseScreen.Identification == Screen.Identifications.BattleScreen)
		//		return (OverworldCamera)(BattleSystem.BattleScreen)baseScreen.SavedOverworld.Camera;
		//	else if (baseScreen.Identification == Screen.Identifications.CreditsScreen)
		//		return (OverworldCamera)(CreditsScreen)baseScreen.SavedOverworld.Camera;
		//	else if (baseScreen.Identification == Screen.Identifications.HallofFameScreen)
		//		return (OverworldCamera)(HallOfFameScreen)baseScreen.SavedOverworld.Camera;
		//
		//	return (OverworldCamera)Screen.Camera;
		//}
		#endregion

		#region Save
		private void SaveParty()
		{
			string Data = GetPartyData();

			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|party", Data);
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Party.dat", Data);
		}

		private void SavePlayer(bool IsAutosave)
		{
			string Data = GetPlayerData(IsAutosave);

			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|player", Data);
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Player.dat", Data);
		}

		private void SaveOptions()
		{
			string Data = GetOptionsData();

			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|options", Data);
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Options.dat", Data);
		}

		private void SaveItems()
		{
			Inventory.RemoveItem((PokemonUnity.Item.Items)177); // Removing Sport Balls if player has those.

			string Data = GetItemsData();

			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|items", Data);
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Items.dat", Data);
		}

		private void SaveBerries()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|berries", GetBerriesData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Berries.dat", GetBerriesData());
		}

		private void SaveApricorns()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|apricorns", GetApricornsData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Apricorns.dat", GetApricornsData());
		}

		private void SaveDaycare()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|daycare", GetDaycareData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Daycare.dat", GetDaycareData());
		}

		private void SavePokedex()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|pokedex", GetPokedexData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Pokedex.dat", GetPokedexData());
		}

		private void SaveRegister()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|register", GetRegisterData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Register.dat", GetRegisterData());
		}

		private void SaveItemData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|itemdata", GetItemDataData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\ItemData.dat", GetItemDataData());
		}

		private void SaveBoxData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|box", GetBoxData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Box.dat", GetBoxData());
		}

		private void SaveNPCData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|npc", GetNPCDataData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\NPC.dat", GetNPCDataData());
		}

		private void SaveHallOfFameData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|halloffame", GetHallOfFameData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\HallOfFame.dat", GetHallOfFameData());
		}

		private void SaveSecretBaseData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|secretbase", GetSecretBaseData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\SecretBase.dat", GetSecretBaseData());
		}

		private void SaveRoamingPokemonData()
		{
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|roamingpokemon", GetRoamingPokemonData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\RoamingPokemon.dat", GetRoamingPokemonData());
		}

		private void SaveStatistics()
		{
			//Statistics = PlayerStatistics.GetData();
			//if (IsGameJoltSave)
			//	GameJoltTempStoreString.Add("saveStorageV" + SaveManager.BuildVersion + "|" + GameJoltSave.GameJoltID + "|statistics", GetStatisticsData());
			//else
			//	System.IO.File.WriteAllText(SaveManager.saveLocation + @"\Save\" + filePrefix + @"\Statistics.dat", GetStatisticsData());
		}
		#endregion
		
		public void HealParty()
		{
			for (var i = 0; i <= Pokemons.Count - 1; i++)
				Pokemons[i].Heal();
		}

		public void HealParty(int[] Members)
		{
			foreach (int member in Members)
			{
				if (Pokemons.Count - 1 >= member)
					Pokemons[member].Heal();
			}
		}
		
		public int CountFightablePokemon
		{
			get
			{
				int i = 0;

				foreach (Pokemon Pokemon in Pokemons)
				{
					if (Pokemon.Status != Status.FAINT & Pokemon.EggSteps == 0 & Pokemon.HP > 0)
						i += 1;
				}
				return i;
			}
		}

		public bool CanCatchPokémon
		{
			get
			{
				string[] data = BoxData.ToArray("§");
				if (data.Count() >= BoxAmount * 30)
					return false;
				return true;
			}
		}

		public int SurfPokemon
		{
			get
			{
				for (var i = 0; i <= Pokemons.Count - 1; i++)
				{
					Pokemon p = Pokemons[i];

					if (!p.isEgg)
					{
						foreach (PokemonUnity.Attack.Move a in p.moves)
						{
							if (a.MoveId == PokemonUnity.Moves.SURF)
								return i;
						}
					}
				}
				if (GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
					return 0;
				else
					return -1;
			}
		}

		public Pokemon GetWalkPokemon()
		{
			if (Pokemons.Count == 0)
				return null/* TODO Change to default(_) if this is not a reference type */;

			for (var i = 0; i <= Pokemons.Count - 1; i++)
			{
				if (Pokemons[i].Status != Status.FAINT & !Pokemons[i].isEgg)
					return Pokemons[i];
			}
			return null/* TODO Change to default(_) if this is not a reference type */;
		}

		public int GetValidPokemonCount()
		{
			int c = 0;
			foreach (Pokemon p in GameVariables.playerTrainer.Party)
			{
				if (p.Status != Status.FAINT & p.EggSteps == 0)
					c += 1;
			}
			return c;
		}



		public bool IsFlying = false;

		// ===STEP EVENT INFORMATION===
		// Events when taking a step	| Priority	| Event Type    | Resolution if Not fired
		// ---------------------------	|-----------|---------------|--------------------------------------------------------------------------------
		// ScriptBlock trigger			| 0		    | ScriptBlock	| Always fire!
		// Trainer Is in sight			| 1		    | Script		| Ignore, will be activated when walked by on a different tile. Design failure.
		// Egg hatches					| 2		    | Screen change	| Will happen On Next Step automatically.
		// Repel wears out				| 3		    | Script		| Add one Step To the repel counter, so the Event happens On the Next Step.
		// Wild Pokémon appears			| 4		    | WildPokemon	| Just ignore, random Event
		// Pokegear call			    | 5		    | Script		| Just ignore, Not too important
		// ----------------------------------------------------------------------------------------------------------------------------------------
		// All Script Events need a special check condition set.
		// Script Blocks are handled externally.
		// 
		// Additional things to do that always fire:
		// - Set the player's LastPosition
		// - Add to the daycare cycle, if it finishes, do daycare events, add to the friendship value of Pokémon, add points and check or following pokemon pickup.
		// - Apply shaders to following pokemon and player, and make following pokemon visible
		// - make wild Pokémon noises
		// - add to the Temp map step count
		// - track the statistic for walked steps.

		private bool _stepEventStartedTrainer = false;
		private bool _stepEventRepelMessage = false;
		private bool _stepEventEggHatched = false;

		public void TakeStep(int stepAmount)
		{
			_stepEventEggHatched = false;
			_stepEventRepelMessage = false;
			_stepEventStartedTrainer = false;

			if (!IsFlying)
			{
				// Set the last position:
				//Temp.LastPosition = Screen.Camera.Position;

				// Increment step counters:
				GameVariables.Level.WalkedSteps += 1;
				Temp.MapSteps += 1;
				DaycareSteps += stepAmount;
				//PlayerStatistics.Track("Steps taken", stepAmount);

				// Daycare cycle:
				//PlayerTemp.DayCareCycle -= stepAmount;
				//if (PlayerTemp.DayCareCycle <= 0)
				//{
				//	Daycare.EggCircle();
				//
				//	// Every 256 steps, add friendship to the Pokémon in the player's team.
				//	foreach (Pokemon p in Pokemons)
				//	{
				//		if (p.Status != Status.FAINT & !p.isEgg)
				//			p.ChangeFriendShip(Pokemon.FriendShipCauses.Walking);
				//	}
				//
				//	AddPoints(1, "Completed an Egg Circle.");
				//
				//	//PokemonInteractions.CheckForRandomPickup();
				//}

				// Apply shaders and set following pokemon:
				GameVariables.Level.OwnPlayer.ApplyShaders();
				GameVariables.Level.OverworldPokemon.ApplyShaders();

				GameVariables.Level.OverworldPokemon.ChangeRotation();
				GameVariables.Level.OverworldPokemon.MakeVisible();

				// Make wild pokemon noises:
				MakeWildPokemonNoise();

				StepEventCheckTrainers();
				StepEventCheckEggHatching(stepAmount);
				StepEventCheckRepel(stepAmount);
				StepEventWildPokemon();
				StepEventPokegearCall();
			}
			else
				IsFlying = false;
		}

		private void StepEventCheckTrainers()
		{
			if (CanFireStepEvent())
			{
				GameVariables.Level.CheckTrainerSights();
				//if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
				//{
				//	if (!(OverworldScreen)CurrentScreen.ActionScript.IsReady)
				//		_stepEventStartedTrainer = true;
				//}
			}
		}

		private void StepEventCheckEggHatching(int stepAmount)
		{
			if (CanFireStepEvent())
			{
				int addEggSteps = stepAmount;
				foreach (Pokemon p in Pokemons)
				{
					if (p.Ability != Abilities.NONE)
					{
						if (p.Ability == Abilities.MAGMA_ARMOR || p.Ability == Abilities.FLAME_BODY)
						{
							addEggSteps *= Settings.Rand.Next(1, 4);
							break;
						}
					}
				}

				List<Pokemon> eggsReady = new List<Pokemon>();
				foreach (Pokemon p in Pokemons)
				{
					if (p.EggSteps > 0)
					{
						//p.EggSteps += addEggSteps;
						p.AddSteps((byte)addEggSteps);
						if (p.EggSteps >= Pokemon.PokemonData.GetPokemon(p.Species).HatchTime)
							eggsReady.Add(p);
					}
				}

				if (eggsReady.Count > 0)
				{
					foreach (Pokemon p in eggsReady)
						Pokemons.Remove(p);

					//SetScreen(new TransitionScreen(CurrentScreen, new HatchEggScreen(CurrentScreen, eggsReady), Color.White, false));

					_stepEventEggHatched = true;
				}
			}
		}

		private void StepEventCheckRepel(int stepAmount)
		{
			if (RepelSteps > 0)
			{
				RepelSteps -= stepAmount;

				if (RepelSteps <= 0)
				{
					//if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
					//{
					//	if (CanFireStepEvent())
					//	{
					//		GameVariables.Level.WalkedSteps = 0;
					//
					//		string s = "version=2" + Environment.NewLine + "@Text.Show(Your repel effect wore off.)" + Environment.NewLine + ":end";
					//
					//		if (Temp.LastUsedRepel > -1)
					//		{
					//			bool haveItemLeft = Inventory.GetItemAmount(Temp.LastUsedRepel) > 0;
					//
					//			if (haveItemLeft)
					//				s = "version=2" + Environment.NewLine + "@Text.Show(Your repel effect wore off.*Do you want to use~another <inventory.name(" + Temp.LastUsedRepel + ")>?)" + Environment.NewLine + "@Options.Show(Yes,No)" + Environment.NewLine + ":when:Yes" + Environment.NewLine + "@sound.play(repel_use)" + Environment.NewLine + "@Text.Show(<player.name> used~a <inventory.name(" + Temp.LastUsedRepel + ")>.)" + Environment.NewLine + "@item.repel(" + Temp.LastUsedRepel + ")" + Environment.NewLine + "@item.remove(" + Temp.LastUsedRepel + ",1,0)" + Environment.NewLine + ":endwhen" + Environment.NewLine + ":end";
					//		}
					//
					//		((OverworldScreen)CurrentScreen).ActionScript.StartScript(s, 2, false);
					//		_stepEventRepelMessage = true;
					//	}
					//	else
					//		_repelSteps = 1;
					//}
				}
			}
		}

		private void StepEventWildPokemon()
		{
			if (CanFireStepEvent())
			{
				//if (GameVariables.Level.WildPokemonFloor & !GameVariables.Level.Surfing)
				//	GameVariables.Level.PokemonEncounter.TryEncounterWildPokemon(GameVariables.Camera.Position, EncounterTypes.Land, "");
			}
		}

		private void StepEventPokegearCall()
		{
			if (CanFireStepEvent())
			{
				if (Temp.MapSteps > 0)
				{
					if (Temp.LastCall < 256)
						Temp.LastCall += 1;
					else if (Settings.Rand.Next(0, 700) == 0)
					{
						//GameJolt.PokegearScreen.RandomCall();
						Temp.LastCall = 0;
					}
				}
			}
		}

		private bool CanFireStepEvent()
		{
			//if (!ScriptBlock.TriggeredScriptBlock)
			//{
			//	if (!_stepEventStartedTrainer)
			//	{
			//		if (!_stepEventEggHatched)
			//		{
			//			if (!_stepEventRepelMessage)
			//			{
			//				if (!GameVariables.Level.PokemonEncounterData.EncounteredPokemon)
			//					return true;
			//			}
			//		}
			//	}
			//}
			return false;
		}

		private void MakeWildPokemonNoise()
		{
			//if (GameVariables.Level.WildPokemonGrass)
			//{
			//	if (Settings.Rand.Next(0, 193) == 0)
			//	{
			//		Pokemon p = Spawner.GetPokemon(GameVariables.Level.LevelFile, EncounterTypes.Land, false, "");
			//
			//		if (p != null || p.Species != PokemonUnity.Pokemons.NONE)
			//			PlayWildPokemonNoise((int)p.Species);
			//	}
			//}
			//if (GameVariables.Level.WildPokemonFloor)
			//{
			//	if (Settings.Rand.Next(0, 193) == 0)
			//	{
			//		Pokemon p = Spawner.GetPokemon(GameVariables.Level.LevelFile, EncounterTypes.Land, false, "");
			//
			//		if (p != null)
			//		{
			//			PlayWildPokemonNoise((int)p.Species);
			//			return;
			//		}
			//	}
			//}
			//if (GameVariables.Level.WildPokemonWater)
			//{
			//	if (Settings.Rand.Next(0, 193) == 0)
			//	{
			//		Pokemon p = Spawner.GetPokemon(GameVariables.Level.LevelFile, EncounterTypes.Surfing, false, "");
			//
			//		if (p != null)
			//		{
			//			PlayWildPokemonNoise((int)p.Species);
			//			return;
			//		}
			//	}
			//}
		}

		private void PlayWildPokemonNoise(int number)
		{
			SoundManager.PlayPokemonCry(number, Settings.Rand.Next(0, 6) / (float)10.0f, Settings.Rand.Next(0, 20) / (float)10.0f - 1, GameVariables.sVol * 0.35f);
		}


		public void AddVisitedMap(string mapFile)
		{
			List<string> maps = VisitedMaps.Split(System.Convert.ToChar(",")).ToList();

			if (!maps.Contains(mapFile))
			{
				maps.Add(mapFile);
				VisitedMaps = "";
				foreach (string map in maps)
				{
					if (VisitedMaps != "")
						VisitedMaps += ",";
					VisitedMaps += map;
				}
			}
		}

		public void AddPoints(int amount, string reason)
		{
			int addPoints = amount;

			//foreach (MysteryEventScreen.MysteryEvent mysteryEvent in MysteryEventScreen.ActivatedMysteryEvents)
			//{
			//	if (mysteryEvent.EventType == MysteryEventScreen.EventTypes.PointsMultiplier)
			//		addPoints = System.Convert.ToInt32(addPoints * System.Convert.ToSingle(mysteryEvent.Value.Replace(".", StringHelper.DecSeparator)));
			//}
			//
			//if (IsGameJoltSave)
			//{
			//	if (!GameJolt.LogInScreen.UserBanned(GameJoltSave.GameJoltID))
			//		GameJoltSave.Points += addPoints;
			//}
			//else
			//	Points += addPoints;
			//
			//HistoryScreen.HistoryHandler.AddHistoryItem("Obtained game points", "Amount: " + addPoints.ToString() + "; Reason: " + reason, false, false);
		}

		public void ResetNewLevel()
		{
			lastLevel = 0;
			displayEmblemDelay = 0.0f;
			//emblemPositionX = windowSize.Width;
		}

		private int lastLevel = 0;
		private float displayEmblemDelay = 0.0f;
		//private int emblemPositionX = windowSize.Width;

		public void DrawLevelUp()
		{
			if (IsGameJoltSave)
			{
				//if (lastLevel != GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points) & lastLevel != 0)
				//{
				//	lastLevel = GameJolt.Emblem.GetPlayerLevel(GameJoltSave.Points);
				//	displayEmblemDelay = 35.0f;
				//	Skin = GameJolt.Emblem.GetPlayerSpriteFile(lastLevel, GameJoltSave.GameJoltID, GameJoltSave.Gender);
				//}

				//if (displayEmblemDelay > 0.0f)
				//{
				//	displayEmblemDelay -= 0.1f;
				//	if (displayEmblemDelay <= 6.4f)
				//	{
				//		if (emblemPositionX < windowSize.Width)
				//			emblemPositionX += 8;
				//	}
				//	else if (emblemPositionX > windowSize.Width - 512)
				//		emblemPositionX -= 8;
				//
				//	//GameJolt.Emblem.Draw(GameJolt.API.username, GameJoltSave.GameJoltID, GameJoltSave.Points, GameJoltSave.Gender, GameJoltSave.Emblem, new Vector2(emblemPositionX, 0), 4, GameJoltSave.DownloadedSprite);
				//
				//	if (displayEmblemDelay <= 0.0f)
				//	{
				//		displayEmblemDelay = 0.0f;
				//		emblemPositionX = windowSize.Width;
				//	}
				//}
			}
		}

		public static bool IsSaveGameFolder(string folder)
		{
			if (System.IO.Directory.Exists(folder))
			{
				string[] files = new[] { "Apricorns", "Berries", "Box", "Daycare", "HallOfFame", "ItemData", "Items", "NPC", "Options", "Party", "Player", "Pokedex", "Register", "RoamingPokemon", "SecretBase" };
				foreach (string file in files)
				{
					if (!System.IO.File.Exists(folder + @"\" + file + ".dat"))
						return false;
				}
				return true;
			}
			return false;
		}

		public bool IsRunning()
		{
			//if (KeyBoardHandler.KeyDown(Keys.LeftShift) | ControllerHandler.ButtonDown(Buttons.B))
			if (Input.GetKeyDown(KeyCode.LeftShift) | Input.GetButtonDown(""))
			{
				if (!GameVariables.Level.Riding & !GameVariables.Level.Surfing & Inventory.HasRunningShoes())
					return true;
			}

			return false;
		}

		public void Unload()
		{
			// This function clears all data from the loaded player and restores the default values.

			if (loadedSave)
			{
				// Clearning lists:
				Pokemons.Clear();
				Pokedexes.Clear();
				//Inventory.Clear();
				GameVariables.Bag_Items.Clear();
				Badges.Clear();
				PokeFiles.Clear();
				EarnedAchievements.Clear();
				PokegearModules.Clear();
				PhoneContacts.Clear();
				//Mails.Clear();
				Trophies.Clear();

				// Restore default values:
				Name = "<playername>";
				RivalName = "";
				Male = true;
				Money = 0;
				PlayTime = TimeSpan.Zero;
				GameStart = DateTime.Now;
				OT = "00000";
				Points = 0;
				BP = 0;
				Coins = 0;
				HasPokedex = false;
				HasPokegear = false;
				ShowBattleAnimations = 2;
				BoxAmount = 10;
				LastRestPlace = "yourroom.dat";
				LastRestPlacePosition = "1,0.1f,3";
				LastSavePlace = "yourroom.dat";
				LastSavePlacePosition = "1,0.1f,3";
				DiagonalMovement = false;
				RepelSteps = 0;
				DifficultyMode = 0;
				BattleStyle = 1;
				ShowModelsInBattle = true;
				SaveCreated = "Pre 0.21";
				LastPokemonPosition = new Vector3(999,999,999);
				DaycareSteps = 0;
				GameMode = "Kolben";
				VisitedMaps = "";
				TempSurfSkin = "Hilbert";
				TempRideSkin = "";
				GTSStars = 8;
				SandBoxMode = false;
				Statistics = "";
				startPosition = new Vector3(14, 0.1f, 10);
				startRotation = 0;
				startFreeCameraMode = false;
				startMap = "barktown.dat";
				startFOV = 45.0f;
				startRotationSpeed = 12;
				startThirdPerson = false;
				startSurfing = false;
				startRiding = false;
				Skin = "Hilbert";

				// Clear temp save data:
				RegisterData = "";
				BerryData = "";
				PokedexData = "";
				ItemData = "";
				BoxData = "";
				NPCData = "";
				ApricornData = "";
				SecretBaseData = "";
				DaycareData = "";
				HallOfFameData = "";
				RoamingPokemonData = "";

				filePrefix = "nilllzz";
				newFilePrefix = "";
				AutosaveUsed = false;
				loadedSave = false;

				IsGameJoltSave = false;
				EmblemBackground = "standard";

				ResetNewLevel();
			}
		}
	}
}