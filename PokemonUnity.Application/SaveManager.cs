using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using Newtonsoft.Json;

namespace PokemonUnity.Saving
{
	public static class SaveManager
	{
		#region Variables
		public const string BuildVersion = "0.2.0";

		/// <summary>
		/// If UseAppdata = true, Pokemon Unity will save the save files into %AppData%/Roaming/Pokemon Unity/Saves
		/// <para></para> 
		/// If UseAppdata = false, Pokemon Unity will save the save files into Assets/Saves
		/// </summary>
		/// <remarks>
		/// Instead of cluttering workspace by saving non-CompileTime content in directory,
		/// it should be left in bin, until the project is ready to be shipped out (i.e. production/release ready)
		/// At least then, it remains disposable; and doesnt get in the way of coding/git...
		/// Also, this bool is unnecessary, when the #pragma below does the job a lot better
		/// </remarks>
		private const bool UseAppdate = false;
#if DEBUG
		//Gets saved to a folder in `C:` drive, called `Saves`
		public static readonly string gameConfig = @"\Saves\ConfigFile.pku"; //TestProject\bin\Debug
		public static readonly string playerSave = @"\Saves\SaveFile.pku"; //TestProject\bin\Debug
		//private static string playerSave = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "/Saves/SaveFile.pku"; //TestProject\bin\Debug
		public static readonly string saveLocation = "\\Saves\\"; //TestProject\bin\Debug
		//private static string saveLocation = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Test.data"; 
		//private static string saveLocation = System.Environment.CurrentDirectory + @"\SaveDirectory\SaveFile.pku"; //@"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; 
		//private static string saveLocation = @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
#else
		private static readonly string gameConfig = UnityEngine.Application.persistentDataPath + "/ConfigFile.pku";
		private static readonly string playerSave = UnityEngine.Application.persistentDataPath + "/SaveFile.pku";
		//private static string saveLocation = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData) + @"\Pokemon Unity\Saves\";
		//private static string saveLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6) + "/Saves/";
		public static readonly string saveLocation = UnityEngine.Application.persistentDataPath + "/Saves/";
		//private static string saveLocation = UnityEngine.Application.dataPath + "/Saves/"; //Use for production
#endif
		#endregion
		
		#region 0.0.1 Original Save Mechanic
		//private static UnityEngine.GameObject Player;
		//private static List<SaveEvent> EventSaves = new List<SaveEvent>();

		//public static void RegisterPlayer(UnityEngine.GameObject player)
		//{
		//    Player = player;
		//    //UnityEngine.Debug.Log("Registered Player.");
		//	Game.DebugLog("Registered Player.");
		//}

		///// <summary>
		///// Registers an event into the EventSaves List<>
		///// </summary>
		///// <param name="customEvent">The event that needs to be registered</param>
		//public static void RegisterEvent(SaveEvent customEvent)
		//{
		//	EventSaves.Add(customEvent);
		//	EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
		//	//UnityEngine.Debug.Log(customEvent.ToString());
		//	Game.DebugLog(customEvent.ToString());
		//}
		//
		///// <summary>
		///// Get's the relevant CustomSaveEvents for the current Scene.
		///// </summary>
		///// <param name="sceneIndex">The scene index that the Player is currently on.</param>
		///// <returns></returns>
		//public static List<SaveEvent> GetRelaventSaveData(int sceneIndex)
		//{
		//	return EventSaves.Where(x => x.SceneIndex == sceneIndex).ToList();
		//}

		/*private static SaveData CreateSaveFile(string saveName, int activeScene)
		{
			Pokemon[] Party = Game.Player.Trainer.Party;
			Pokemon[,] PC = Game.PC_Poke;
			List<Items> PlayerBag = Game.Bag_Items;

			return new SaveData
				(
				saveName,
				activeScene,
				Game.Player.Trainer.Name,
				Game.Player.TrainerID, Game.Player.SecretID,
				Game.Player.isMale, //Game.Player.GymBadges,
				Game.Player.playerPokedex,
				Game.Player.PlayTime,
				Player.transform.position, Player.GetComponent<Player>().playerDirection,
				Player.transform.Find("Follower").transform.position, Player.GetComponent<FollowerMovement>().direction,
				Party, PC, PlayerBag,
				EventSaves
				);
		}*/

		///// <summary>
		///// Saves the game using the standard data.
		///// </summary>
		//public static void Save(string saveName, int activeScene)
		//{
		//	if (!UseAppdate)
		//	{
		//		saveLocation = UnityEngine.Application.dataPath + "/Saves/";
		//	}
		//
		//	//SaveData DataToSave = CreateSaveFile(saveName, activeScene);
		//	//SerializeAndCreateSaveFile(DataToSave);
		//}

		/// <summary>
		/// Saves the game using a given SaveData class.
		/// </summary>
		/// <param name="saveData">The SaveData containing the data that needs to be saved.</param>
		public static void Save(SaveData saveData)
		{
			SerializeAndCreateSaveFile(saveData);
		}

		public static void Overwrite(SaveData saveData, int saveID)
		{
			SerializeAndCreateSaveFile(saveData, saveID);
		}

		private static void SerializeAndCreateSaveFile(SaveData saveData, int saveID)
		{
			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				try
				{
					File.Delete(saveLocation + "Save" + saveID + ".pku");
				}
				catch (FileNotFoundException) {	}

				FileStream file = File.Open(saveLocation + "Save" + saveID + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
			catch (DirectoryNotFoundException)
			{
				Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));
				using (FileStream file = File.Open(saveLocation + "Save" + (Directory.GetFiles(saveLocation, "*.pku", SearchOption.TopDirectoryOnly).Length).ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write))
				{
					bf.Serialize(file, saveData);
				}
			}
		}

		private static void SerializeAndCreateSaveFile(SaveData saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				int saveAmount = Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length;

				if (saveAmount < 0)
					saveAmount = 0;

				FileStream file = File.Open(saveLocation + "Save" + saveAmount.ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
			catch (Exception)
			{
				Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));

				FileStream file = File.Open(saveLocation + "Save" + (Directory.GetFiles(saveLocation, "*pku", SearchOption.TopDirectoryOnly).Length).ToString() + ".pku", FileMode.OpenOrCreate, FileAccess.Write);
				bf.Serialize(file, saveData);
				file.Close();
			}
		}

		/// <summary>
		/// Loads the Save file that corresponeds with the saveInex.
		/// </summary>
		/// <param name="saveIndex">The index of the save (starting from 0, FE: "Save0.pku")</param>
		public static void Load(int saveIndex)
		{
			if (!UseAppdate)
			{
				//saveLocation = UnityEngine.Application.dataPath + "/Saves/";
				if (!Directory.Exists(saveLocation.Substring(0, saveLocation.Length - 1)))
				{
					Directory.CreateDirectory(saveLocation.Substring(0, saveLocation.Length - 1));
				}
			}

			BinaryFormatter bf = new BinaryFormatter();
			try
			{
				FileStream file = File.Open(saveLocation + "Save" + saveIndex.ToString() + ".pku", FileMode.Open, FileAccess.Read);
				SaveData DataToLoad = (SaveData)bf.Deserialize(file);

				if (null != DataToLoad)
				{
					//!Pass the data through to Game!

					/*
					//EventSaves contains all the Events that the Player has encountered
					EventSaves = DataToLoad.EventList;

					if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != DataToLoad.ActiveScene)
					{
						UnityEngine.SceneManagement.SceneManager.LoadScene(DataToLoad.ActiveScene);
					}
					else
					{
						//Loads the Trainer's Party into the Game
						for (int i = 0; i < DataToLoad.PlayerParty.Length; i++)
						{
							//The Trainer.Party is `private set;`, so we just loop through it and set it individually
							Game.Player.Trainer.Party[i] = DataToLoad.PlayerParty[i];
						}
						Game.PC_Poke = DataToLoad.PC;

						//Loads the Bag (containing the Items that the player owns) into the Game
						Game.Bag_Items = DataToLoad.PlayerBag;


						//Loading Player
						UnityEngine.GameObject Player = UnityEngine.GameObject.FindGameObjectWithTag("Player");
						Player.transform.position = DataToLoad.PlayerPosition;
						Player.GetComponent<PlayerMovement>().direction = DataToLoad.PlayerDirection;
						//Loading Follower
						UnityEngine.GameObject Follower = Player.transform.Find("Follower").gameObject;
						Follower.transform.position = DataToLoad.FollowerPosition;
						Follower.GetComponent<FollowerMovement>().direction = DataToLoad.FollowerDirection;

						EventSaves = EventSaves.OrderBy(x => x.EventTime).ToList();
					}*/
				}
				file.Dispose();
			}
			catch (FileNotFoundException)
			{
				//UnityEngine.Debug.Log("Couldn't find \"Save" + saveIndex + ".pku\".");
				//Game.DebugLog("Couldn't find \"Save" + saveIndex + ".pku\".");

			}
		}

		public static SaveData GetSave(int saveID)
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fileStream = new FileStream(saveLocation + "Save" + saveID + ".pku", FileMode.Open, FileAccess.Read))
			{
				try
				{
					SaveData newSave = (SaveData)bf.Deserialize(fileStream);
					return newSave;
				}
				catch (FileNotFoundException)
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Gets the amount of save files that's specified in Amount.
		/// </summary>
		/// <param name="Amount">The amount of save files that needs to be loaded (0 for all the save files).</param>
		/// <returns>A List containing the relevant amount of save files.</returns>
		public static List<SaveData> GetSaves(int Amount)
		{
			List<SaveData> saveFiles = new List<SaveData>();
			foreach (string file in Directory.GetFiles(saveLocation))
			{
				BinaryFormatter bf = new BinaryFormatter();
				try
				{
					if (Path.GetExtension(file) == "pku")
					{
						using (FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
						{
							try
							{
								SaveData saveData = (SaveData)bf.Deserialize(fileStream);
								if (saveData.BuildVersion == BuildVersion)
								{
									saveFiles.Add(saveData);
								}
								else
								{
									//Try to convert the created file into the current version
								}
							}
							catch (Exception e)
							{
								//UnityEngine.Debug.Log(e.ToString());
								//Game.DebugLog(e.ToString());
							}
						}
					}
				}
				catch (IOException e)
				{
					throw e;
				}
			}

			saveFiles = saveFiles.OrderBy(x => x.TimeCreated).ToList();
			if (Amount == 0 || saveFiles.Count < Amount)
			{
				return saveFiles;
			}
			else
			{
				return saveFiles.Take(Amount).ToList();
			}
		}

		//public static string GetBuildVersion()
		//{
		//	return BuildVersion;
		//}
		#endregion

		#region 0.1.0-0.1.1 Save Mechanic Rewrite
			[System.Obsolete]
		public static void CreateSaveFileAndSerialize(SaveData[] saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
			//if (System.IO.File.Exists(playerSave))
			//{//SaveData[] sd = GetSaves();
#if DEBUG
				//using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				//{
				//	using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8))
				//	{
				//		sw.Write(ob ((object)saveData).ToString());
				//		//sw.Flush(); sw.Close(); sw.Dispose();
				//	}
				//}
				File.WriteAllText(playerSave, JsonConvert.SerializeObject(saveData, Formatting.Indented/*, new JsonSerializerSettings() { Formatting = Formatting.Indented }*/));
#else
				using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				{
					bf.Serialize(fs, saveData);
				}
#endif
			//}
		}

			[System.Obsolete]
		public static SaveData[] GetSaves()
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(playerSave))
			{
#if DEBUG
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
					{
						//return (SaveData[])(object)sr.ReadToEnd();
						return JsonConvert.DeserializeObject<SaveData[]>(sr.ReadToEnd());
					};
				}
				//return JsonConvert.DeserializeObject<SaveData[]>(playerSave);
#else
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					return (SaveData[])bf.Deserialize(fs);
				}
#endif
			}
			else return null;
		}

			[System.Obsolete]
		public static void LoadGameState()
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(gameConfig))
			{
				using (FileStream fs = System.IO.File.Open(gameConfig, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					//ToDo: Create Custom class from anon
					var data = new {
						Language			= Game.UserLanguage,//(int)language;
						WindowBorder		= Game.WindowSkin,
						DialogBorder		= Game.DialogSkin,
						TextSpeed			= Game.textSpeed,
						mVol				= Game.mvol,
						sVol				= Game.svol,
						Fullscreen			= Game.fullscreen,
					};
#if DEBUG
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
					{
						data = sr.ReadToEnd().CastTo(data);
						//data = JsonConvert.DeserializeObject<data>(sr.ReadToEnd());
					};
#else
					data = bf.Deserialize(fs).CastTo(data);
#endif
					//Game.UserLanguage	= (Languages)data.Language;
					//Game.WindowSkin	= data.WindowBorder;
					//Game.DialogSkin	= data.DialogBorder;
					Game.textSpeed		= data.TextSpeed;
					//Game.mvol			= data.mVol;
					//Game.svol			= data.sVol;
					Game.fullscreen	= data.Fullscreen;
				}
			}
			else
				SaveGameState();
		}

			[System.Obsolete]
		public static void SaveGameState()
		{
			BinaryFormatter bf = new BinaryFormatter();
			using (FileStream fs = System.IO.File.Open(gameConfig, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
			{
#if DEBUG
				//using (StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.ASCII)) 
				//{
				//	sw.Write(
					File.WriteAllText(playerSave, JsonConvert.SerializeObject(new
					{
						Language			= Game.UserLanguage,//(int)language;
						WindowBorder		= Game.WindowSkin,
						DialogBorder		= Game.DialogSkin,
						TextSpeed			= Game.textSpeed,
						mVol				= Game.mvol,
						sVol				= Game.svol,
						Fullscreen			= Game.fullscreen,
					}, Formatting.Indented));
				//}
#else
				bf.Serialize(fs, new
				{
					Language			= GameVariables.UserLanguage,//(int)language;
					WindowBorder		= GameVariables.WindowSkin,
					DialogBorder		= GameVariables.DialogSkin,
					TextSpeed			= GameVariables.textSpeed,
					mVol				= GameVariables.mvol,
					sVol				= GameVariables.svol,
					Fullscreen			= GameVariables.fullscreen,
				});
#endif
			}
		}

		private static T CastTo<T>(this Object value, T target)
		{
			return (T)value;
		}
		#endregion

		#region 0.2.0 Save Mechanic Rewrite
			[System.Obsolete]
		public static void CreateSaveFileAndSerialize(SaveData saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
#if DEBUG
				File.WriteAllText(playerSave, JsonConvert.SerializeObject(saveData, Formatting.Indented/*, new JsonSerializerSettings() { Formatting = Formatting.Indented }*/));
#else
				using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				{
					bf.Serialize(fs, saveData);
				}
#endif
		}

			[System.Obsolete]
		public static SaveData GetSave()
		{
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(playerSave))
			{
#if DEBUG
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
					{
						return JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd(), new SaveDataConverter());
					};
				}
				//return JsonConvert.DeserializeObject<SaveData>(playerSave);
#else
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					return (SaveData[])bf.Deserialize(fs);
				}
#endif
			}
			else return null;
		}
		#endregion

		#region 0.3.0 SQL Save Mechanic
		public static void WriteSaveToDatabase(SaveData saveData)
		{
			BinaryFormatter bf = new BinaryFormatter();
#if DEBUG
				File.WriteAllText(playerSave, JsonConvert.SerializeObject(saveData, Formatting.Indented/*, new JsonSerializerSettings() { Formatting = Formatting.Indented }*/));
#else
				using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
				{
					bf.Serialize(fs, saveData);
				}
#endif
			//WriteSaveToDatabase(saveData.GameStates);
		}

		public static bool WriteSaveToDatabase(GameState gameState)
		{
			System.Data.SQLite.SQLiteTransaction trans = Game.con.BeginTransaction();
			try
			{
				//Step 3: Running a Command
				//System.Data.SQLite.SQLiteCommand stmt = Game.con.CreateCommand();
				System.Data.SQLite.SQLiteCommand stmt = trans.Connection.CreateCommand();

				#region Save Player State
				stmt.CommandText = @"INSERT OR REPLACE INTO game_player (id, ChallengeId, PlayerMoney, PlayerCoins, PlayerSavings, IsCreator, StorageOwnerName, ActiveStorageBox, TrainerName, TrainerIsMale, TrainerPublicId, TrainerSecretId, TimeCreated, PlayTime) VALUES (
					@id		--INTEGER NOT NULL,
					,@ChallengeId		--INTEGER NOT NULL,
					,@PlayerMoney		--INTEGER NOT NULL,
					,@PlayerCoins		--INTEGER NOT NULL,
					,@PlayerSavings		--INTEGER NOT NULL,
					,@IsCreator			--BIT NOT NULL,
					,@StorageOwnerName	--NVARCHAR(18) NOT NULL,
					,@ActiveStorageBox	--INTEGER NOT NULL,
					,@TrainerName		--NVARCHAR(18) NOT NULL,
					,@TrainerIsMale		--BIT NOT NULL,
					,@TrainerPublicId	--INTEGER NOT NULL,
					,@TrainerSecretId	--INTEGER NOT NULL,
					,@TimeCreated		--DATETIME NOT NULL,
					,@PlayTime)			--INTEGER NOT NULL"; 
				stmt.CommandType = System.Data.CommandType.Text;
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@id", value: (int)0));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ChallengeId", value: (int)gameState.Challenge));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PlayerMoney", value: (int)gameState.PlayerMoney));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PlayerCoins", value: (int)gameState.PlayerCoins));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PlayerSavings", value: (int)gameState.PlayerSavings));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IsCreator", value: (bool)gameState.IsMale));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@StorageOwnerName", System.Data.DbType.String, 18) { Value = gameState.PlayerName });
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ActiveStorageBox", value: (int)gameState.PlayerPC.ActiveBox));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerName", System.Data.DbType.String, 18) { Value = gameState.PlayerName });
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerIsMale", value: (bool)gameState.IsMale));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerPublicId", value: (int)gameState.TrainerID));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerSecretId", value: (int)gameState.SecretID));
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TimeCreated", System.Data.DbType.DateTime) { Value = gameState.TimeCreated });
				stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PlayTime", value: (int)gameState.PlayTime.Ticks));
				stmt.ExecuteNonQuery();
				#endregion

				for (int i = 0; i < gameState.Pokedex.Length; i++)
				{
					if (gameState.Pokedex[i][0] == 0)
						continue;
					#region Save Player State
					//stmt = Game.con.CreateCommand();
					stmt = stmt.Connection.CreateCommand();
					stmt.CommandText = @"INSERT OR REPLACE INTO game_player (game_id, pokemon_species_id, pokemon_form_id, HasCaptured) VALUES (
						@game_id				--INTEGER NOT NULL,
						,@pokemon_species_id    --INTEGER NOT NULL, Species
						,@pokemon_form_id		--INTEGER NOT NULL, Form
						,@HasCaptured			--BIT NOT NULL";
					stmt.CommandType = System.Data.CommandType.Text;
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@game_id", value: (int)gameState.Challenge));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_species_id", value: (int)i));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_form_id", value: (int)gameState.Pokedex[i][2]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@HasCaptured", value: (bool)(gameState.Pokedex[i][0] == 2)));
					stmt.ExecuteNonQuery();
					#endregion
				}

				for (int i = 0; i < gameState.PlayerParty.Length; i++)
				{
					if (gameState.PlayerParty[i].Species == 0)
						continue;
					#region Register Pokemons in Player's Party
					//stmt = Game.con.CreateCommand();
					stmt = stmt.Connection.CreateCommand();
					stmt.CommandText = @"INSERT OR REPLACE INTO game_player (id, NickName, pokemon_form_id, pokemon_species_id, ability_id, nature_id, IsShiny, Gender, PokerusStatus, PokerusDuration, ShadowLevel, HeartGuageSize, CurrentHP, held_item_id, IV_hp, IV_atk, IV_def, IV_spa, IV_spd, IV_spe, EV, ObtainedLevel, TotalExp, Happiness, Status, StatusCount, EggSteps, BallUsed_item_id, Mail, TrainerName, TrainerIsMale, TrainerPublicId, TrainerSecretId, ObtainedMethod, TimeReceived, TimeEggHatched, Markings1, Markings2, Markings3, Markings4, Markings5, Markings6) VALUES (
						,@id					--INTEGER NOT NULL, --PersonalID no duplicates
						,@NickName				--NVARCHAR(14) NULL, 
						,@pokemon_form_id		--INTEGER NOT NULL, --Form
						,@pokemon_species_id    --INTEGER NOT NULL, --Species
						,@ability_id			--INTEGER NOT NULL, --Ability
						,@nature_id				--INTEGER NOT NULL, --Nature
						,@IsShiny				--BIT NOT NULL,
						,@Gender				--BIT NULL,
						,@PokerusStatus			--INTEGER NOT NULL,
						,@PokerusDuration		--INTEGER NOT NULL,
						,@ShadowLevel			--INTEGER NULL,
						,@HeartGuageSize		--INTEGER NOT NULL,
						,@CurrentHP				--INTEGER NOT NULL,
						,@held_item_id			--INTEGER NOT NULL, --Item
						,@IV_hp					--INTEGER NOT NULL,
						,@IV_atk				--INTEGER NOT NULL,
						,@IV_def				--INTEGER NOT NULL,
						,@IV_spa				--INTEGER NOT NULL,
						,@IV_spd				--INTEGER NOT NULL,
						,@IV_spe				--INTEGER NOT NULL,
						,@EV					--NVARCHAR(8) NOT NULL, --Array[6]
						,@ObtainedLevel			--INTEGER NOT NULL,
						,@TotalExp				--INTEGER NOT NULL,
						,@Happiness				--INTEGER NOT NULL,
						,@Status				--INTEGER NOT NULL,
						,@StatusCount			--INTEGER NOT NULL,
						,@EggSteps				--INTEGER NOT NULL,
						,@BallUsed_item_id		--INTEGER NOT NULL, --BallUsed
						,@Mail					--NVARCHAR(100) NULL, --Message
						,@TrainerName			--NVARCHAR(18) NOT NULL,
						,@TrainerIsMale			--BIT NOT NULL,
						,@TrainerPublicId		--INTEGER NOT NULL,
						,@TrainerSecretId		--INTEGER NOT NULL,
						,@ObtainedMethod		--INTEGER NOT NULL,
						,@TimeReceived			--DATETIME NOT NULL,
						,@TimeEggHatched		--DATETIME NOT NULL,
						,@Markings1				--BIT NOT NULL,
						,@Markings2				--BIT NOT NULL,
						,@Markings3				--BIT NOT NULL,
						,@Markings4				--BIT NOT NULL,
						,@Markings5				--BIT NOT NULL,
						,@Markings6)			--BIT NOT NULL";
					stmt.CommandType = System.Data.CommandType.Text;
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@id", value: (int)gameState.PlayerParty[i].PersonalId));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@NickName", System.Data.DbType.String, 18) { Value = gameState.PlayerParty[i] });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_form_id", value: (int)gameState.PlayerParty[i].Form));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_species_id", value: (int)gameState.PlayerParty[i].Species));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ability_id", value: (int)gameState.PlayerParty[i].Ability));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@nature_id", value: (int)gameState.PlayerParty[i].Nature));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IsShiny", value: (bool)gameState.PlayerParty[i].IsShiny));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Gender", value: (bool)gameState.PlayerParty[i].Gender));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PokerusStatus", value: (int)gameState.PlayerParty[i].Pokerus[0]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PokerusDuration", value: (int)gameState.PlayerParty[i].Pokerus[1]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ShadowLevel", value: (int)gameState.PlayerParty[i].ShadowLevel));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@HeartGuageSize", value: (int)gameState.PlayerParty[i].HeartGuageSize));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CurrentHP", value: (int)gameState.PlayerParty[i].CurrentExp));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@held_item_id", value: (int)gameState.PlayerParty[i].Item));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_hp", value: (int)gameState.PlayerParty[i].IV[(int)Stats.HP]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_atk", value: (int)gameState.PlayerParty[i].IV[(int)Stats.ATTACK]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_def", value: (int)gameState.PlayerParty[i].IV[(int)Stats.DEFENSE]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spa", value: (int)gameState.PlayerParty[i].IV[(int)Stats.SPATK]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spd", value: (int)gameState.PlayerParty[i].IV[(int)Stats.SPDEF]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spe", value: (int)gameState.PlayerParty[i].IV[(int)Stats.SPEED]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@EV", System.Data.DbType.String, 18) { Value = gameState.PlayerParty[i].EV.ToString() });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ObtainedLevel", value: (int)gameState.PlayerParty[i].ObtainedLevel));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TotalExp", value: (int)gameState.PlayerParty[i].CurrentExp));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Happiness", value: (int)gameState.PlayerParty[i].Happiness));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Status", value: (int)gameState.PlayerParty[i].Status));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@StatusCount", value: (int)gameState.PlayerParty[i].StatusCount));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@EggSteps", value: (int)gameState.PlayerParty[i].EggSteps));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@BallUsed_item_id", value: (int)gameState.PlayerParty[i].BallUsed));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Mail", System.Data.DbType.String, 18) { Value = gameState.PlayerParty[i].Mail.Message });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerName", System.Data.DbType.String, 18) { Value = gameState.PlayerParty[i] });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerIsMale", value: (bool)gameState.PlayerParty[i].TrainerIsMale));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerPublicId", value: (int)gameState.PlayerParty[i].TrainerTrainerId));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerSecretId", value: (int)gameState.PlayerParty[i].TrainerSecretId));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ObtainedMethod", value: (int)gameState.PlayerParty[i].ObtainedMethod));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TimeReceived", System.Data.DbType.DateTime) { Value = gameState.PlayerParty[i].TimeReceived });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TimeEggHatched", System.Data.DbType.DateTime) { Value = gameState.PlayerParty[i].TimeEggHatched });
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings1", value: (bool)gameState.PlayerParty[i].Markings[0]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings2", value: (bool)gameState.PlayerParty[i].Markings[1]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings3", value: (bool)gameState.PlayerParty[i].Markings[2]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings4", value: (bool)gameState.PlayerParty[i].Markings[3]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings5", value: (bool)gameState.PlayerParty[i].Markings[4]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings6", value: (bool)gameState.PlayerParty[i].Markings[5]));
					stmt.ExecuteNonQuery();
					#endregion

					#region Assign Pokemons to Player's Party
					//stmt = Game.con.CreateCommand();
					stmt = stmt.Connection.CreateCommand();
					stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_location (pokemon_personal_id, pokemon_game_id, pokemon_location_id, pokemon_slot_position) VALUES (
						@pokemon_personal_id	--INTEGER NOT NULL UNIQUE ON CONFLICT REPLACE, --no duplicates
						,@pokemon_game_id		--INTEGER NOT NULL, --game state
						,@pokemon_location_id   --BIT NULL, --party / pc, or wild... make int and add `game modes` (stadium saved party)?
						,@pokemon_slot_position)--INTEGER NOT NULL --where"; 
					stmt.CommandType = System.Data.CommandType.Text;
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerParty[i].PersonalId));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_location_id", value: (bool)true));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_slot_position", value: (int)i));
					stmt.ExecuteNonQuery();
					#endregion

					for (int j = 0; j < gameState.PlayerParty[i].Archive.Length; j++)
					{
						#region Register Pokemon's Previously Learned Moves
						//stmt = Game.con.CreateCommand();
						stmt = stmt.Connection.CreateCommand();
						stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_move_archive (pokemon_game_id, pokemon_personal_id, move_id) VALUES (
						@pokemon_game_id		--INTEGER NOT NULL,
						,@pokemon_personal_id	--INTEGER NOT NULL,
						,@move_id)				--INTEGER NOT NULL";
						stmt.CommandType = System.Data.CommandType.Text;
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerParty[i].PersonalId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_id", value: (int)gameState.PlayerParty[i].Archive[j]));
						stmt.ExecuteNonQuery();
						#endregion
					}

					for (int j = 0; j < gameState.PlayerParty[i].Moves.Length; j++)
					{
						#region Register Pokemon's MoveSet
						//stmt = Game.con.CreateCommand();
						stmt = stmt.Connection.CreateCommand();
						stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_moves (pokemon_game_id, pokemon_personal_id, move_slot_position, move_id, PP, PPups) VALUES (
							@pokemon_game_id		--INTEGER NOT NULL,
							,@pokemon_personal_id	--INTEGER NOT NULL, 
							,@move_slot_position	--INTEGER NOT NULL,
							,@move_id				--INTEGER NOT NULL,
							,@PP					--INTEGER NOT NULL,
							,@PPups					--INTEGER NOT NULL,"; 
						stmt.CommandType = System.Data.CommandType.Text;
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerParty[i].PersonalId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_slot_position", value: (int)j));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_id", value: (int)gameState.PlayerParty[i].Moves[j].Move));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PP", value: (int)gameState.PlayerParty[i].Moves[j].PP));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PPups", value: (int)gameState.PlayerParty[i].Moves[j].PPups));
						stmt.ExecuteNonQuery();
						#endregion
					}

					for (int j = 0; j < gameState.PlayerParty[i].Ribbons.Length; j++)
					{
						#region Register Pokemon's Collected Ribbons
						//stmt = Game.con.CreateCommand();
						stmt = stmt.Connection.CreateCommand();
						stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_ribbon (pokemon_game_id, pokemon_personal_id, ribbon_id) VALUES (
						@pokemon_game_id		--INTEGER NOT NULL,
						,@pokemon_personal_id	--INTEGER NOT NULL,
						,@ribbon_id)			--INTEGER NOT NULL";
						stmt.CommandType = System.Data.CommandType.Text;
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerParty[i].PersonalId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ribbon_id", value: (int)gameState.PlayerParty[i].Ribbons[j]));
						stmt.ExecuteNonQuery();
						#endregion
					}
				}

				for (int i = 0; i < gameState.PlayerPC.Pokemons.Length; i++)
				{
					#region Register Storage Boxes in Player's PC
					//stmt = Game.con.CreateCommand();
					stmt = stmt.Connection.CreateCommand();
					stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_storage (game_id, slot_id, slot_background_id, slot_name, unlocked) VALUES (
						@game_id				--INTEGER NOT NULL,
						,@slot_id				--INTEGER NOT NULL, --box
						,@slot_background_id    --INTEGER NOT NULL, --theme / texture
						,@slot_name				--NVARCHAR(18) NOT NULL, --label
						,@unlocked)				--BIT NOT NULL"; 
					stmt.CommandType = System.Data.CommandType.Text;
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@game_id", value: (int)0));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@slot_id", value: (int)i));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@slot_background_id", value: (int)gameState.PlayerPC.BoxTextures[i]));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@slot_name", value: gameState.PlayerPC.BoxNames));
					stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@unlocked", value: (bool)true));
					stmt.ExecuteNonQuery();
					#endregion
					for (int j = 0; j < gameState.PlayerPC.Pokemons[i].Length; j++)
					{
						if (gameState.PlayerPC.Pokemons[i][j].Species == 0)
							continue;
						#region Register Pokemons in Player's PC
						//stmt = Game.con.CreateCommand();
						stmt = stmt.Connection.CreateCommand();
						stmt.CommandText = @"INSERT OR REPLACE INTO game_player (id, NickName, pokemon_form_id, pokemon_species_id, ability_id, nature_id, IsShiny, Gender, PokerusStatus, PokerusDuration, ShadowLevel, HeartGuageSize, CurrentHP, held_item_id, IV_hp, IV_atk, IV_def, IV_spa, IV_spd, IV_spe, EV, ObtainedLevel, TotalExp, Happiness, Status, StatusCount, EggSteps, BallUsed_item_id, Mail, TrainerName, TrainerIsMale, TrainerPublicId, TrainerSecretId, ObtainedMethod, TimeReceived, TimeEggHatched, Markings1, Markings2, Markings3, Markings4, Markings5, Markings6) VALUES (
							,@id					--INTEGER NOT NULL, --PersonalID no duplicates
							,@NickName				--NVARCHAR(14) NULL, 
							,@pokemon_form_id		--INTEGER NOT NULL, --Form
							,@pokemon_species_id    --INTEGER NOT NULL, --Species
							,@ability_id			--INTEGER NOT NULL, --Ability
							,@nature_id				--INTEGER NOT NULL, --Nature
							,@IsShiny				--BIT NOT NULL,
							,@Gender				--BIT NULL,
							,@PokerusStatus			--INTEGER NOT NULL,
							,@PokerusDuration		--INTEGER NOT NULL,
							,@ShadowLevel			--INTEGER NULL,
							,@HeartGuageSize		--INTEGER NOT NULL,
							,@CurrentHP				--INTEGER NOT NULL,
							,@held_item_id			--INTEGER NOT NULL, --Item
							,@IV_hp					--INTEGER NOT NULL,
							,@IV_atk				--INTEGER NOT NULL,
							,@IV_def				--INTEGER NOT NULL,
							,@IV_spa				--INTEGER NOT NULL,
							,@IV_spd				--INTEGER NOT NULL,
							,@IV_spe				--INTEGER NOT NULL,
							,@EV					--NVARCHAR(8) NOT NULL, --Array[6]
							,@ObtainedLevel			--INTEGER NOT NULL,
							,@TotalExp				--INTEGER NOT NULL,
							,@Happiness				--INTEGER NOT NULL,
							,@Status				--INTEGER NOT NULL,
							,@StatusCount			--INTEGER NOT NULL,
							,@EggSteps				--INTEGER NOT NULL,
							,@BallUsed_item_id		--INTEGER NOT NULL, --BallUsed
							,@Mail					--NVARCHAR(100) NULL, --Message
							,@TrainerName			--NVARCHAR(18) NOT NULL,
							,@TrainerIsMale			--BIT NOT NULL,
							,@TrainerPublicId		--INTEGER NOT NULL,
							,@TrainerSecretId		--INTEGER NOT NULL,
							,@ObtainedMethod		--INTEGER NOT NULL,
							,@TimeReceived			--DATETIME NOT NULL,
							,@TimeEggHatched		--DATETIME NOT NULL,
							,@Markings1				--BIT NOT NULL,
							,@Markings2				--BIT NOT NULL,
							,@Markings3				--BIT NOT NULL,
							,@Markings4				--BIT NOT NULL,
							,@Markings5				--BIT NOT NULL,
							,@Markings6)			--BIT NOT NULL";
						stmt.CommandType = System.Data.CommandType.Text;
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@id", value: (int)gameState.PlayerPC.Pokemons[i][j].PersonalId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@NickName", System.Data.DbType.String, 18) { Value = gameState.PlayerPC.Pokemons[i] });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_form_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Form));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_species_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Species));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ability_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Ability));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@nature_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Nature));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IsShiny", value: (bool)gameState.PlayerPC.Pokemons[i][j].IsShiny));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Gender", value: (bool)gameState.PlayerPC.Pokemons[i][j].Gender));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PokerusStatus", value: (int)gameState.PlayerPC.Pokemons[i][j].Pokerus[0]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PokerusDuration", value: (int)gameState.PlayerPC.Pokemons[i][j].Pokerus[1]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ShadowLevel", value: (int)gameState.PlayerPC.Pokemons[i][j].ShadowLevel));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@HeartGuageSize", value: (int)gameState.PlayerPC.Pokemons[i][j].HeartGuageSize));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@CurrentHP", value: (int)gameState.PlayerPC.Pokemons[i][j].CurrentExp));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@held_item_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Item));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_hp", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.HP]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_atk", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.ATTACK]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_def", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.DEFENSE]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spa", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.SPATK]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spd", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.SPDEF]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@IV_spe", value: (int)gameState.PlayerPC.Pokemons[i][j].IV[(int)Stats.SPEED]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@EV", System.Data.DbType.String, 18) { Value = gameState.PlayerPC.Pokemons[i][j].EV.ToString() });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ObtainedLevel", value: (int)gameState.PlayerPC.Pokemons[i][j].ObtainedLevel));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TotalExp", value: (int)gameState.PlayerPC.Pokemons[i][j].CurrentExp));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Happiness", value: (int)gameState.PlayerPC.Pokemons[i][j].Happiness));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Status", value: (int)gameState.PlayerPC.Pokemons[i][j].Status));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@StatusCount", value: (int)gameState.PlayerPC.Pokemons[i][j].StatusCount));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@EggSteps", value: (int)gameState.PlayerPC.Pokemons[i][j].EggSteps));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@BallUsed_item_id", value: (int)gameState.PlayerPC.Pokemons[i][j].BallUsed));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Mail", System.Data.DbType.String, 18) { Value = gameState.PlayerPC.Pokemons[i][j].Mail.Message });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerName", System.Data.DbType.String, 18) { Value = gameState.PlayerPC.Pokemons[i] });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerIsMale", value: (bool)gameState.PlayerPC.Pokemons[i][j].TrainerIsMale));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerPublicId", value: (int)gameState.PlayerPC.Pokemons[i][j].TrainerTrainerId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TrainerSecretId", value: (int)gameState.PlayerPC.Pokemons[i][j].TrainerSecretId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ObtainedMethod", value: (int)gameState.PlayerPC.Pokemons[i][j].ObtainedMethod));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TimeReceived", System.Data.DbType.DateTime) { Value = gameState.PlayerPC.Pokemons[i][j].TimeReceived });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@TimeEggHatched", System.Data.DbType.DateTime) { Value = gameState.PlayerPC.Pokemons[i][j].TimeEggHatched });
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings1", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[0]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings2", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[1]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings3", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[2]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings4", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[3]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings5", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[4]));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@Markings6", value: (bool)gameState.PlayerPC.Pokemons[i][j].Markings[5]));
						stmt.ExecuteNonQuery();
						#endregion

						#region Assign Pokemons to Player's PC
						//stmt = Game.con.CreateCommand();
						stmt = stmt.Connection.CreateCommand();
						stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_location (pokemon_personal_id, pokemon_game_id, pokemon_location_id, pokemon_slot_position) VALUES (
							@pokemon_personal_id	--INTEGER NOT NULL UNIQUE ON CONFLICT REPLACE, --no duplicates
							,@pokemon_game_id		--INTEGER NOT NULL, --game state
							,@pokemon_location_id   --BIT NULL, --party / pc, or wild... make int and add `game modes` (stadium saved party)?
							,@pokemon_slot_position)--INTEGER NOT NULL --where"; 
						stmt.CommandType = System.Data.CommandType.Text;
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerPC.Pokemons[i][j].PersonalId));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_location_id", value: (bool)true));
						stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_slot_position", value: (int)i));
						stmt.ExecuteNonQuery();
						#endregion

						for (int n = 0; n < gameState.PlayerPC.Pokemons[i][j].Archive.Length; n++)
						{
							#region Register Pokemon's Previously Learned Moves
							//stmt = Game.con.CreateCommand();
							stmt = stmt.Connection.CreateCommand();
							stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_move_archive (pokemon_game_id, pokemon_personal_id, move_id) VALUES (
							@pokemon_game_id		--INTEGER NOT NULL,
							,@pokemon_personal_id	--INTEGER NOT NULL,
							,@move_id)				--INTEGER NOT NULL";
							stmt.CommandType = System.Data.CommandType.Text;
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerPC.Pokemons[i][j].PersonalId));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Archive[n]));
							stmt.ExecuteNonQuery();
							#endregion
						}

						for (int n = 0; n < gameState.PlayerPC.Pokemons[i][j].Moves.Length; n++)
						{
							#region Register Pokemon's MoveSet
							//stmt = Game.con.CreateCommand();
							stmt = stmt.Connection.CreateCommand();
							stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_moves (pokemon_game_id, pokemon_personal_id, move_slot_position, move_id, PP, PPups) VALUES (
								@pokemon_game_id		--INTEGER NOT NULL,
								,@pokemon_personal_id	--INTEGER NOT NULL, 
								,@move_slot_position	--INTEGER NOT NULL,
								,@move_id				--INTEGER NOT NULL,
								,@PP					--INTEGER NOT NULL,
								,@PPups					--INTEGER NOT NULL,"; 
							stmt.CommandType = System.Data.CommandType.Text;
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerPC.Pokemons[i][j].PersonalId));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_slot_position", value: (int)n));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@move_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Moves[n].Move));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PP", value: (int)gameState.PlayerPC.Pokemons[i][j].Moves[n].PP));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@PPups", value: (int)gameState.PlayerPC.Pokemons[i][j].Moves[n].PPups));
							stmt.ExecuteNonQuery();
							#endregion
						}

						for (int n = 0; n < gameState.PlayerPC.Pokemons[i][j].Ribbons.Length; n++)
						{
							#region Register Pokemon's Collected Ribbons
							//stmt = Game.con.CreateCommand();
							stmt = stmt.Connection.CreateCommand();
							stmt.CommandText = @"INSERT OR REPLACE INTO game_player_pokemon_ribbon (pokemon_game_id, pokemon_personal_id, ribbon_id) VALUES (
							@pokemon_game_id		--INTEGER NOT NULL,
							,@pokemon_personal_id	--INTEGER NOT NULL,
							,@ribbon_id)			--INTEGER NOT NULL";
							stmt.CommandType = System.Data.CommandType.Text;
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_game_id", value: (int)0));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@pokemon_personal_id", value: (int)gameState.PlayerPC.Pokemons[i][j].PersonalId));
							stmt.Parameters.Add(new System.Data.SQLite.SQLiteParameter("@ribbon_id", value: (int)gameState.PlayerPC.Pokemons[i][j].Ribbons[n]));
							stmt.ExecuteNonQuery();
							#endregion
						}
					}
				}
				trans.Commit();
				return true;
			}
			catch (System.Data.SQLite.SQLiteException e)
			{
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				trans.Rollback();
				return false;
			}
		}

		/*public static GameState ReadSaveFromDatabase()
		{
			try
			{
				List<GameState> games = new List<GameState>();

				//Step 3: Running a Command
				System.Data.SQLite.SQLiteCommand stmt = Game.con.CreateCommand();

				#region DataReader
				stmt.CommandText = "select distinct move_id, pokemon_id, [level], pokemon_move_method_id from pokemon_moves --where version_group_id=18 order by pokemon_id ASC";
				//	@"select pokemon_moves.pokemon_id, pokemon_moves.move_id, pokemon_moves.level, pokemon_moves.pokemon_move_method_id, pokemon_moves."order", pokemon_moves.version_group_id,
				//pokemon_move_methods.identifier
				//from pokemon_moves
				//left join pokemon_move_methods on pokemon_move_methods.id=pokemon_moves.pokemon_move_method_id
				//where version_group_id=18
				//--order by pokemon_id ASC";
				System.Data.SQLite.SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using (reader)
				{
					GameState g = new GameState();
					while (reader.Read()) //if(reader.Read())
					{
						if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
							p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
								new List<Monster.Data.PokemonMoveset>());
						p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
							new PokemonUnity.Monster.Data.PokemonMoveset(
								moveId: (Moves)int.Parse((string)reader["move_id"].ToString())
								//pokemonId: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								//,generation: int.Parse((string)reader["version_group_id"].ToString())
								, method: (LearnMethod)int.Parse((string)reader["pokemon_move_method_id"].ToString())
								, level: int.Parse((string)reader["level"].ToString())
							//,order: int.Parse((string)reader["order"].ToString())
							)
						);
					}
					#endregion
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					foreach (var pkmn in p)
					{
						PokemonMovesData.Add(pkmn.Key, new Monster.Data.PokemonMoveTree(pkmn.Value.ToArray()));
					}
				}
				return true;
			}
			catch (System.Data.SQLite.SQLiteException e)
			{
				GameDebug.LogError("SQL Exception Message:" + e.Message);
				GameDebug.LogError("SQL Exception Code:" + e.ErrorCode.ToString());
				GameDebug.LogError("SQL Exception Help:" + e.HelpLink);
				return false;
			}
			BinaryFormatter bf = new BinaryFormatter();
			if (System.IO.File.Exists(playerSave))
			{
#if DEBUG
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8))
					{
						return JsonConvert.DeserializeObject<SaveData>(sr.ReadToEnd(), new SaveDataConverter());
					};
				}
				//return JsonConvert.DeserializeObject<SaveData>(playerSave);
#else
				using (FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.Open, System.IO.FileAccess.Read))
				{
					return (SaveData[])bf.Deserialize(fs);
				}
#endif
			}
			else return null;
		}*/
		#endregion

		#region Save/Load Data
		private static byte slotIndex { get; set; }
		//private int fileIndex { get; set; }
		/// <summary>
		/// Bool used to tell Start-Up screen whether or not to display "Continue" option
		/// </summary>
		public static bool SaveFileFound { get; private set; }
		//public System.DateTimeOffset fileCreationDate { get; set; }
		//public System.DateTimeOffset? lastSave { get; set; }
		//public System.DateTimeOffset startTime { get; set; }

		/// <summary
		/// Preload before any of the other scenes are loaded...
		/// </summary>
		///ToDo: Temp Save Profiles to be used and displayed on Start-Up screen
		public static void Load()
		{
			//Load player settings (language, full screen, vol...)
			//Load continue/new game/"choose load slots" options...
			//Load temp profile data (Party, pokedex seen/caught, hours played...)
		}
		/// <summary>
		/// Loads saved game data from memory slot
		/// </summary>
		/// <param name="i">Array int from binary stream</param>
		public static void Load(byte i)
		{
			slotIndex = i > 0 && i < 3 ? i : slotIndex;
			//Game.SaveLoad.Load();
			PokemonUnity.Saving.SaveData data = PokemonUnity.Saving.SaveManager.GetSave(i);
			//Game.Player = new Player();

			switch (data.BuildVersion)
			{
				#region Obsolete and Deprecated
				//ToDo: Sort out importing older formats...
				//case "0.0.1":
				//case "0.1.0":
				//	Player.LoadTrainer(data); 
				//	//PC_Poke = data.PC.GetPokemonsFromSeri();
				//	//PC_boxNames = data.PC.BoxNames;
				//	//PC_boxTexture = data.PC.BoxTextures;
				//	//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
				//	//Bag_Items = data.PlayerBag;
				//	PC = new Character.PC(
				//		pkmns: data.PC.GetPokemonsFromSeri().,
				//		items: data.PC.GetItemsFromSeri(),
				//		names: data.PC.BoxNames,
				//		textures: data.PC.BoxTextures
				//	);
				//	Bag = new Character.Bag(data.PlayerBag.ToArray());
				//	break;
				//case "0.1.1":
				//	PlayerPosition = data.PlayerPosition;		//g.PlayerPosition = trainerSaveData.PlayerPosition;
				//	PlayerDirection = data.PlayerDirection;		//g.PlayerDirection = trainerSaveData.PlayerDirection;
				//	Checkpoint = (Locations)data.PokeCenterId;	//g.Checkpoint = (Locations)trainerSaveData.PokeCenterId;
				//	Player.LoadTrainer(data);
				//	Area = data.ActiveMapId;
				//	PC_Poke = data.PC.GetPokemonsFromSeri();
				//	PC_boxNames = data.PC.BoxNames;
				//	PC_boxTexture = data.PC.BoxTextures;
				//	PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
				//	Bag_Items = data.PlayerBag;
				//	break;
				//case "0.2.0":
				//	Game g = InitLoadFile()[i];
				//	PlayerItemData = g.PlayerItemData;
				//	PlayerBerryData = g.PlayerBerryData;
				//	PlayerNPCData = g.PlayerNPCData;
				//	PlayerApricornData = g.PlayerApricornData;
				//	RepelSteps = g.RepelSteps;
				//	//RepelType = g.RepelType;
				//	Rival = g.Rival;
				//	PlayerPosition = g.PlayerPosition;// = trainerSaveData.PlayerPosition;
				//	//PlayerDirection = data.PlayerDirection;		//g.PlayerDirection = trainerSaveData.PlayerDirection;
				//	Checkpoint = (Locations)Checkpoint;// = (Locations)trainerSaveData.PokeCenterId;
				//	Player = g.Player;//.LoadTrainer(data);
				//	//Area = data.ActiveMapId;
				//	//PC_Poke = data.PC.GetPokemonsFromSeri();
				//	//PC_boxNames = data.PC.BoxNames;
				//	//PC_boxTexture = data.PC.BoxTextures;
				//	//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
				//	//Bag_Items = data.PlayerBag;
				//	PC = g.PC;
				//	Bag = g.Bag;
				//	GameData = g;
				//	break;
				#endregion
				//Next one gets added to list, and default is copied above, and modified below...
				default:
					//ToDo: "0.2.0" => Load Game, Player, Bag, and PC data in dedicated classes
					//Game g = InitLoadFile()[i];
					/*PlayerItemData = g.PlayerItemData;
					PlayerBerryData = g.PlayerBerryData;
					PlayerNPCData = g.PlayerNPCData;
					PlayerApricornData = g.PlayerApricornData;
					RepelSteps = g.RepelSteps;
					//RepelType = g.RepelType;
					Rival = g.Rival;
					PlayerPosition = g.PlayerPosition;// = trainerSaveData.PlayerPosition;
					//PlayerDirection = data.PlayerDirection;		//g.PlayerDirection = trainerSaveData.PlayerDirection;
					Checkpoint = (Locations)Checkpoint;// = (Locations)trainerSaveData.PokeCenterId;
					Player = g.Player;//.LoadTrainer(data);
					//Area = data.ActiveMapId;
					//PC_Poke = data.PC.GetPokemonsFromSeri();
					//PC_boxNames = data.PC.BoxNames;
					//PC_boxTexture = data.PC.BoxTextures;
					//PC_Items = new List<Items>(data.PC.GetItemsFromSeri());
					//Bag_Items = data.PlayerBag;
					PC = g.PC;
					Bag = g.Bag;*/
					//GameData = g;
					break;
			}
		}

		/// <summary>
		/// For Debug Use Purposes;
		/// Used Unit Tester...
		/// </summary>
		public static void Save(int slot, SaveData test = null)
		{
			//ToDo: LoadInitFiles()
			SaveData save = SaveManager.GetSave();
			//if (save == null)
			//	save = new SaveData[] { null, null, null };
			//save[slot] = test;
			if (test == null)
				SaveManager.CreateSaveFileAndSerialize(save);
			else
				SaveManager.CreateSaveFileAndSerialize(test);
		}
		#endregion

		//public static void Save()
		//{
		//	//using (System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Open(FILE_NAME,)))
		//	//Game.SaveLoad.Save();
		//
		//	//PokemonUnity.Saving.SaveManager.Overwrite(new PokemonUnity.Saving.SaveData(), slotIndex);
		//	SaveData[] save = SaveManager.GetSaves();
		//	if (save == null)
		//		save = new SaveData[] { null, null, null };
		//	save[slotIndex] = new SaveData();
		//	SaveManager.CreateSaveFileAndSerialize(save);
		//}

		/*private class SaveLoad {
			#region Variables
			//int DatabaseEntryStringWidth = 100;
			System.IO.FileStream fs;
			//BinaryWriter w; //= new BinaryWriter(fs);
			System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//Game data = new Game();
			Game[] gamesaves = new Game[3];
			Translator.Languages userpreflanguage = Translator.Languages.English;
	#if DEBUG
			private const string FILE_NAME = @"Test.pkud"; //TestProject\bin\Debug
			//private const string FILE_NAME = @"..\..\..\\Pokemon Unity\Assets\Scripts2\Test.data"; //TestProject\bin\Debug
			//string file = System.Environment.CurrentDirectory + @"\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //TestProject\bin\Debug
			//string file =  @"$(SolutionDir)\Assets\Resources\Database\Pokemon\Pokemon_" + fileLanguage + ".xml"; //Doesnt work
	#else
			private const string FILE_NAME = UnityEngine.Application.persistentDataPath + "/Test.pkud";
			//string filepath = UnityEngine.Application.dataPath + "/Scripts2/Translations/";//Resources/Database/Pokemon/Pokemon_" + fileLanguage + ".xml"; //Use for production
	#endif
			#endregion

			public static void Save(System.IO.BinaryWriter writer) { }
		
			/// <summary>
			/// When initially boots up, this will be all the application data stored
			/// on user's PersonalComputer (PC). If first time running game, naturally
			/// stored data would not exist, and the game will produce one by default.
			/// </summary>
			void OldMe()
			{
				//UnityEngine.Debug.Log("Checking to see if BinaryText exist...");
				if (System.IO.File.Exists(FILE_NAME))
				{
					//UnityEngine.Debug.Log(FILE_NAME + " already exists!");

					//UnityEngine.Debug.Log("Loading Old Info from BinaryText...");
					// Create the reader for data.
					//fs = new FileStream(FILE_NAME, FileMode.Open, FileAccess.Read);
					fs = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open, System.IO.FileAccess.Read);
					//UnityEngine.Debug.Log("Information Loaded.");
					//UnityEngine.Debug.Log("Deserializing Information...");
					//data = (Game)bf.Deserialize(fs);
					SaveLoad d = (SaveLoad)bf.Deserialize(fs);
					//UnityEngine.Debug.Log("Rewriting TextField/Variables...");

					//username = data.username;
					//rememberme = data.rememberme;
					Core.UserLanguage = d.userpreflanguage;

					//An Array[3] of Game representing GameSaves
					gamesaves = d.gamesaves;//(Game[])bf.Deserialize(fs);
					SaveFileFound = true;

					//UnityEngine.Debug.Log("Information Loaded and Updated.");
					//UnityEngine.Debug.Log("Closing BinaryText...");
					//UnityEngine.Debug.Log("Closing FileStream...");
					fs.Close();
					//UnityEngine.Debug.Log("BinaryText Closed.");
					return;
				}
				else
				{
					SaveFileFound = false;
					System.IO.File.Open(FILE_NAME, System.IO.FileMode.Create).Close();
				}
			}

			public static void Save()
			{
				//if (SaveDataOld.currentSave != null)
				//{
				//	if (SaveDataOld.currentSave.getFileIndex() >= 0 && SaveDataOld.currentSave.getFileIndex() < savedGames.Length)
				//	{
				//		SaveDataOld.currentSave.PlayTime += SaveDataOld.currentSave.startTime.Subtract(System.DateTime.UtcNow);
				//		SaveDataOld.currentSave.lastSave = System.DateTime.UtcNow;// new System.DateTime(,System.DateTimeKind.Utc);
				//		savedGames[SaveDataOld.currentSave.getFileIndex()] = SaveDataOld.currentSave;
				//		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				//		System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
				//		bf.Serialize(file, SaveLoad.savedGames);
				//		file.Close();
				//	}
				//}
			}

			public static bool Load()
			{
				//Debug.Log(Application.persistentDataPath);
				if (System.IO.File.Exists(FILE_NAME))
				{
					System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
					System.IO.FileStream file = System.IO.File.Open(FILE_NAME, System.IO.FileMode.Open);
					//SaveLoad.savedGames = (SaveDataOld[])bf.Deserialize(file);

					//Player = new Trainer().LoadTrainer(trainerSaveData: trainerData);

					file.Close();
					return true;
				}
				return false;
			}

			//public static int getSavedGamesCount()
			//{
			//	int count = 0;
			//	for (int i = 0; i < savedGames.Length; i++)
			//	{
			//		if (savedGames[i] != null)
			//		{
			//			count += 1;
			//		}
			//	}
			//	return count;
			//}

			//public static void resetSaveGame(int index)
			//{
			//	savedGames[index] = null;
			//
			//	if (index < 2)
			//	{
			//		for (int i = index; i < 2; i++)
			//		{
			//			SaveLoad.savedGames[i] = SaveLoad.savedGames[i + 1];
			//			SaveLoad.savedGames[i + 1] = null;
			//		}
			//	}
			//
			//	bool sGN1 = savedGames[0] == null;
			//	bool sGN2 = savedGames[1] == null;
			//	bool sGN3 = savedGames[2] == null;
			//
			//	//Debug.Log(sGN1.ToString() + ", " + sGN2.ToString() + ", " + sGN3.ToString());
			//
			//	System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
			//	System.IO.FileStream file = System.IO.File.Create(FILE_NAME);//Application.persistentDataPath + "/playerData.pkud"
			//	bf.Serialize(file, SaveLoad.savedGames);
			//	file.Close();
			//}
		}*/
	}
}