using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using PokemonUnity;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Overworld;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Attack.Data;
using PokemonUnity.Character;
using Newtonsoft.Json;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	//ToDo: Missing (Trainer)Player.Rival variable (KeyValuePair <Type,Name>)
	//ToDo: Add Write_DB_To_File Function
	public static class Kernal //: IDatabase
	{
		#region Variables
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; private set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]> PokemonItemsData { get; private set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.Form[]> PokemonFormsData { get; private set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; private set; }
		public static IDictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; private set; }
		public static IDictionary<Natures,PokemonUnity.Monster.Nature> NatureData { get; private set; }
		public static IDictionary<Types,PokemonUnity.Monster.Data.Type> TypeData { get; private set; }
		public static IDictionary<Moves,Attack.Data.MoveData> MoveData { get; private set; }
		public static IDictionary<Moves,Attack.Data.MetaData> MoveMetaData { get; private set; }
		public static IDictionary<Attack.Effects, PokemonEssentials.Interface.PokeBattle.IBattleMove> MoveEffectData { get; private set; } //ToDo: Replace with Func<Attack.Effects,Combat.Move> instead?
		public static IDictionary<Items,ItemData> ItemData { get; private set; }
		public static IDictionary<Items,BerryData> BerryData { get; private set; }
		public static IDictionary<Regions,Locations[]> RegionData { get; private set; }
		public static IDictionary<Regions,Pokedex> PokedexData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Locations"/> | Value: <seealso cref="IArea.Id"/>
		/// </summary>
		public static IDictionary<Locations,int[]> LocationData { get; private set; }
		/// <summary>
		/// List of <see cref="Player.Area"/> that triggers <seealso cref="Overworld.IMapEncounterMetadata"/>
		/// <para></para>
		/// Key: <seealso cref="Overworld.IArea.Id"/> | Value: <seealso cref="Player.Area"/>
		/// </summary>
		public static IDictionary<int,IArea> AreaData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Method"/> | Value: <seealso cref="IMapEncounterMetadata.Id"/>
		/// </summary>
		public static IDictionary<Method,int[]> MethodData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="IMapEncounterMetadata.Id"/> | Value: <seealso cref="Overworld.IMapEncounterMetadata"/>
		/// </summary>
		public static IDictionary<int,IMapEncounterMetadata> EncounterData { get; private set; }
		//public static IDictionary<Method,IMapEncounterMetadata> EncounterMethodData { get; private set; }
		//public static IDictionary<int,PokemonEssentials.Interface.Field.IEncounters> MapEncounterData { get; private set; }
		public static IDictionary<TrainerTypes,TrainerMetaData> TrainerMetaData { get; private set; }
		public static IDictionary<GymBadges,Character.BadgeData> BadgeData { get; private set; }
		public static IDictionary<HiddenMoves,HiddenMoveData> HiddenMoveData { get; private set; }
		public static IDictionary<int,MachineData> MachineData { get; private set; }
		//public static IDictionary<Items,KeyValuePair<Moves,int[]>[]> MachineMetaData { get; protected set; }
		//public static IDictionary<int,Machine> MapData { get; protected set; }
		//public static IDictionary<int,int> TextureData1 { get; protected set; }
		//public static IDictionary<int,int> TextureData2 { get; protected set; }
		//public static IDictionary<Location,Script[]> ScriptData { get; protected set; }
		//public static IDictionary<Location,Script[]> LevelScriptData { get; protected set; }
		//public static IDictionary<Location,Text[]> TextData { get; protected set; }
		//public static IDictionary<Location,EventData> EventData { get; protected set; }
		//public static IDictionary<Location,Event[]> EventData { get; protected set; }
		//public static IDictionary<Location,EventFlags[]> MapFlagData { get; protected set; }
		#endregion

		/// <summary>
		/// The names of each Dex list in the game, in order and with National Dex at
		/// the end. This is also the order that <see cref="PokemonEssentials.Interface.Field.IGlobalMetadata.pokedexUnlocked"/> is
		/// in, which records which Dexes have been unlocked (first is unlocked by
		/// default).
		/// </summary>
		/// <remarks>
		/// You can define which region a particular Dex list is linked to. This
		/// means the area map shown while viewing that Dex list will ALWAYS be that
		/// of the defined region, rather than whichever region the player is
		/// currently in. To define this, put the Dex name and the region number in
		/// an array, like the Kanto and Johto Dexes are. The National Dex isn't in
		/// an array with a region number, therefore its area map is whichever region
		/// the player is currently in.
		/// </remarks>
		/// <returns></returns>
		public static IEnumerable<string> DexNames()
		{
			//return new string[] {
			//	Game._INTL("Kanto Pokédex"),		//{ Game._INTL("Kanto Pokédex"),0 },
			//	Game._INTL("Johto Pokédex"),		//{ Game._INTL("Johto Pokédex"),1 },
			//	Game._INTL("National Pokédex")
			//};
			foreach (Pokedex dex in PokedexData.Values)
				yield return dex.Name;
		}

		/// <summary>
		/// A list of maps used by roaming Pokémon. Each map has an array of other maps it can lead to.
		/// </summary>
		public static KeyValuePair<int, int[]>[] RoamingAreas = new KeyValuePair<int, int[]>[] {
			new KeyValuePair<int, int[]>(5 , new int[] { 21,28,31,39,41,44,47,66,69 }),
			new KeyValuePair<int, int[]>(21, new int[] { 5 ,28,31,39,41,44,47,66,69 }),
			new KeyValuePair<int, int[]>(28, new int[] { 5 ,21,31,39,41,44,47,66,69 }),
			new KeyValuePair<int, int[]>(31, new int[] { 5 ,21,28,39,41,44,47,66,69 }),
			new KeyValuePair<int, int[]>(39, new int[] { 5 ,21,28,31,41,44,47,66,69 }),
			new KeyValuePair<int, int[]>(41, new int[] { 5 ,21,28,31,39,44,47,66,69 }),
			new KeyValuePair<int, int[]>(44, new int[] { 5 ,21,28,31,39,41,47,66,69 }),
			new KeyValuePair<int, int[]>(47, new int[] { 5 ,21,28,31,39,41,44,66,69 }),
			new KeyValuePair<int, int[]>(66, new int[] { 5 ,21,28,31,39,41,44,47,69 }),
			new KeyValuePair<int, int[]>(69, new int[] { 5 ,21,28,31,39,41,44,47,66 })
		};
		/// <summary>
		/// A set of arrays each containing the details of a roaming Pokémon.
		/// </summary>
		public static RoamingEncounterData[] RoamingSpecies = new RoamingEncounterData[] {
			new RoamingEncounterData (Pokemons.LATIAS,	30, 53, EncounterTypes.None,	"002-Battle02x"),
			new RoamingEncounterData (Pokemons.LATIOS,	30, 53, EncounterTypes.None,	"002-Battle02x"),
			new RoamingEncounterData (Pokemons.KYOGRE,	40, 54, EncounterTypes.Surfing,	null, areas: new Dictionary<int, int[]> {
				{ 2 , new int[] {21,31} },
				{ 21, new int[] {2,31,69} },
				{ 31, new int[] {2,21,69} },
				{ 69, new int[] {21,31} }
			}),
			new RoamingEncounterData (Pokemons.ENTEI,	40, 55, EncounterTypes.Walking,	null )
		};

		#region Initialize Data From Database
		static Kernal()
		{
			PokemonData = new Dictionary<Pokemons, Monster.Data.PokemonData>();
			PokemonMovesData = new Dictionary<Pokemons, Monster.Data.PokemonMoveTree>();
			PokemonEvolutionsData = new Dictionary<Pokemons, Monster.Data.PokemonEvolution[]>();
			PokemonItemsData = new Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]>();
			PokemonFormsData = new Dictionary<Pokemons, Monster.Data.Form[]>();
			NatureData = new Dictionary<Natures, Nature>();
			TypeData = new Dictionary<Types, PokemonUnity.Monster.Data.Type>();
			MoveData = new Dictionary<Moves, Attack.Data.MoveData>();
			MoveMetaData = new Dictionary<Moves, Attack.Data.MetaData>();
			//MoveEffectData = new Dictionary<Attack.Effects, Combat.Move>(); //Below
			MoveEffectData = new Dictionary<Attack.Effects, PokemonEssentials.Interface.PokeBattle.IBattleMove>();
			ItemData = new Dictionary<Items, ItemData>();
			BerryData = new Dictionary<Items, BerryData>();
			//Init in Region Method
			PokedexData = new Dictionary<Regions, Pokedex>();
			RegionData = new Dictionary<Regions, Locations[]>();
			LocationData = new Dictionary<Locations, int[]>();
			AreaData = new Dictionary<int, IArea>();
			MethodData = new Dictionary<Method, int[]>();
			EncounterData = new Dictionary<int, IMapEncounterMetadata>();
			//EncounterData = new Dictionary<int, Encounter>();
			//MapEncounterData = new Dictionary<int, PokemonEssentials.Interface.Field.IEncounters>();
			TrainerMetaData = new Dictionary<TrainerTypes, TrainerMetaData>();
			//TrainerData = new Dictionary<int, Encounter>();
			BadgeData = new Dictionary<GymBadges, Character.BadgeData>();
			HiddenMoveData = new Dictionary<HiddenMoves, HiddenMoveData>();
			//TrainerData = new Dictionary<int, Encounter>();
		}
		#endregion

		#region Write to and Read from Methods
		public static void load_data(string filepath = null) { }
		public static void load_data(out IDictionary<int, PokemonEssentials.Interface.Field.IEncounters> encdata, string filepath = null)
		{
			encdata = new Dictionary<int, PokemonEssentials.Interface.Field.IEncounters>();
			//if path is used, then the information will source from hard file
			if (!string.IsNullOrEmpty(filepath)) //and file exist...
			{
				UnicodeEncoding uniEncoding = new UnicodeEncoding();
				string tempString;

				using (FileStream fileStream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None))
				{
					byte[] readText = new byte[fileStream.Length];

					try
					{
						fileStream.Seek(0, SeekOrigin.Begin);
						fileStream.Read(
							readText, 0, (int)fileStream.Length);
						tempString = new String(
							uniEncoding.GetChars(
							readText, 0, readText.Length));
						//Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<int, PokemonEssentials.Interface.Field.IEncounters>>(fileStream.ReadToEnd());
						encdata = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<int, PokemonEssentials.Interface.Field.IEncounters>>(tempString);
					}
					// Catch the IOException generated if the
					// specified part of the file is locked.
					//catch (IOException e)
					//{
					//	Console.WriteLine("{0}: The read " +
					//		"operation could not be performed " +
					//		"because the specified part of the " +
					//		"file is locked.",
					//		e.GetType().Name);
					//}
					finally
					{
						//xmlString = tempString;
					}
					//return tempString;
				}
				return;
			}
			//otherwise, just use sql database...
			#region DataReader
			//Game.DatabasePath = "Data Source=..\\veekun-pokedex.sqlite";
			//Game.DatabasePath = "Data Source =" + UnityEngine.Application.dataPath + "/Data/veekun-pokedex.sqlite";
			//Game.con = (System.Data.IDbConnection)new System.Data.SQLite.SQLiteConnection(Game.DatabasePath);
			//Game.con = (System.Data.IDbConnection)new Mono.Data.Sqlite.SqliteConnection(Game.DatabasePath);

			//Step 3: Running a Command
			/*IDbCommand stmt = Game.con.CreateCommand();

			stmt.CommandText = string.Format(
				@"select --e.id, e.version_id, e.location_area_id, e.encounter_slot_id, e.pokemon_id, e.min_level, e.max_level,
				MIN(e.id) as id, e.location_area_id, group_concat(DISTINCT e.pokemon_id) as pokemon_group, MIN(e.min_level) as min_level, MAX(e.max_level) as max_level, group_concat(DISTINCT e.encounter_slot_id) as encounter_slot_group, group_concat(DISTINCT e.version_id) as version_group,
				s.encounter_method_id, s.slot, s.rarity, group_concat(DISTINCT s.version_group_id) as encounter_slot_version_group,
				--i.encounter_condition_value_group,
				group_concat(DISTINCT i.encounter_condition_value_id) as encounter_condition_value_group,
				a.location_id,
				l.region_id,
				g.generation_id
				--group_concat(DISTINCT g.generation_id) as generation_group
				--,n.name, n.subtitle
				from encounters as e
				--left join encounter_slots
				left join (
					select id, version_group_id, rarity, encounter_method_id,
					case when (slot is not null) AND version_group_id = 15 then slot + 1 else slot end as slot
					from encounter_slots
				)
				as s on s.id = e.encounter_slot_id
				--left join encounter_condition_value_map_view as i on e.id = i.encounter_id;
				--left join (
				--	select encounter_id, group_concat(DISTINCT encounter_condition_value_id) as encounter_condition_value_group
				--	from encounter_condition_value_map
				--	group by encounter_id
				--) as i on e.id = i.encounter_id
				left join encounter_condition_value_map as i on e.id = i.encounter_id
				--left join (
				--	select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
				--	from location_area_encounter_rates
				--	group by location_area_id, encounter_method_id, rate
				--) as r on e.location_area_id = r.location_area_id
				left join location_areas as a on a.id=e.location_area_id
				left join locations as l on l.id=a.location_id
				left join location_game_indices as g on l.id = g.location_id
				left join location_names as n on l.id = n.location_id AND n.local_language_id=9
				--where i.encounter_condition_value_group IS NOT NULL
				--where i.encounter_condition_value_id IS NOT NULL
				--where e.location_area_id = {0}
				group by e.location_area_id, s.encounter_method_id, s.slot, s.rarity; --e.pokemon_id;", Game.GameData.GamePlayer.map.map_id);

			//Step 4: Read the results
			using (IDataReader reader = stmt.ExecuteReader())
			{
				//Dictionary<Method, List<int>> e = new Dictionary<Method, List<int>>();
				Dictionary<Method, IList<PokemonEssentials.Interface.Field.IEncounterPokemon>> e = new Dictionary<Method, IList<PokemonEssentials.Interface.Field.IEncounterPokemon>>();
				while (reader.Read()) //if(reader.Read())
				{
					if (!e.ContainsKey((Method)int.Parse((string)reader["encounter_method_id"].ToString())))
						e.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), new List<PokemonEssentials.Interface.Field.IEncounterPokemon>());
					e[(Method)int.Parse((string)reader["encounter_method_id"].ToString())]
						.Add(new EncounterSlotData(
								//pokemon: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								pokemon: reader["pokemon_group"].ToString().Split(',').Select(x => (Pokemons)int.Parse(x)).ToArray()
								//, mapId: int.Parse((string)reader["location_area_id"].ToString())
								//, method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								//, slotId: int.Parse((string)reader["slot"].ToString())
								//, conditions: reader["encounter_condition_value_group"].ToString().Split(',').Select(x => (ConditionValue)int.Parse(x)).ToArray()
								//, generation: int.Parse((string)reader["generation_id"].ToString())
								, minLevel: int.Parse((string)reader["min_level"].ToString())
								, maxLevel: int.Parse((string)reader["max_level"].ToString())
								//, rarity: int.Parse((string)reader["rarity"].ToString())
								//, versions: reader["encounter_slot_version_group"].ToString().Split(',').Select(x => (Versions)int.Parse(x)).ToArray()
							));
					if (!encdata.ContainsKey(int.Parse((string)reader["location_area_id"].ToString())))
						encdata.Add(int.Parse((string)reader["location_area_id"].ToString()),
							new PokemonEncounter(
							//	//pokemon: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
							//	pokemon: reader["pokemon_group"].ToString().Split(',').Select(x => (Pokemons)int.Parse(x)).ToArray()
							//	id: int.Parse((string)reader["id"].ToString())
							//	, mapId: int.Parse((string)reader["location_area_id"].ToString())
							//	, method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
							//	//, slotId: int.Parse((string)reader["slot"].ToString())
							//	//, conditions: reader["encounter_condition_value_group"].ToString().Split(',').Select(x => (ConditionValue)int.Parse(x)).ToArray()
							//	//, generation: int.Parse((string)reader["generation_id"].ToString())
							//	, minLevel: int.Parse((string)reader["min_level"].ToString())
							//	, maxLevel: int.Parse((string)reader["max_level"].ToString())
							//	//, rarity: int.Parse((string)reader["rarity"].ToString())
							//	//, versions: reader["encounter_slot_version_group"].ToString().Split(',').Select(x => (Versions)int.Parse(x)).ToArray()
							)
						);
					//encdata[int.Parse((string)reader["location_area_id"].ToString())]
					//	.EnctypeChances.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), int.Parse((string)reader["rarity"].ToString()));
				}
				//Step 5: Closing up
				reader.Close();
				reader.Dispose();
				//foreach (var en in e)
				//{
				//	Kernal.MethodData.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), en.Value.ToArray());
				//}
			}*/
			#endregion
		}
		//public static void save_data(IDictionary<int, PokemonEssentials.Interface.Field.IEncounters> encdata, string filepath = null)
		public static void save_data(object encdata, string filepath = null)
		{
			if (encdata == null || encdata is null) return; //maybe save an empty file, or clear database?
			if (!string.IsNullOrEmpty(filepath))
			{
//				BinaryFormatter bf = new BinaryFormatter();
//#if (DEBUG || UNITY_EDITOR)
//				File.WriteAllText(playerSave, JsonConvert.SerializeObject(saveData, Formatting.Indented/*, new JsonSerializerSettings() { Formatting = Formatting.Indented }* /));
//#else
//				using(FileStream fs = System.IO.File.Open(playerSave, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write))
//				{
//					bf.Serialize(fs, saveData);
//				}
//#endif
				UnicodeEncoding uniEncoding = new UnicodeEncoding();
				string tempString;

				using (FileStream fileStream = new FileStream(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
				{
					tempString = Newtonsoft.Json.JsonConvert.SerializeObject(encdata); //<IDictionary<int, PokemonEssentials.Interface.Field.IEncounters>>
					byte[] readText = new byte[0]; //tempString.Length
					readText = uniEncoding.GetBytes(tempString);

					try
					{
						// Write the original file data.
						if (!string.IsNullOrEmpty(tempString)) //fileStream.Length == 0
						{
							//fileStream.Write(uniEncoding.GetBytes(tempString),
							//	0, uniEncoding.GetByteCount(tempString));
							fileStream.Write(readText,
								0, readText.Length);
						}
					}
					// Catch the IOException generated if the
					// specified part of the file is locked.
					//catch (IOException e)
					//{
					//	Console.WriteLine("{0}: The read " +
					//		"operation could not be performed " +
					//		"because the specified part of the " +
					//		"file is locked.",
					//		e.GetType().Name);
					//}
					finally
					{
						//return; //tempString;
					}
				}
				return;
			}
		}
		#endregion
	}
}