using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
//using PokemonUnity.Inventory.ItemData;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using PokemonUnity.Inventory.Plants;
using PokemonUnity.Attack.Data;
using PokemonUnity.Character;
//using System.Data.SqlClient;
//using System.Security.Cryptography.MD5;

namespace PokemonUnity
{
	/// <summary>
	/// Variables that are stored when game is saved, and other temp values used for gameplay.
	/// This class should be called once, when the game boots-up.
	/// During boot-up, game will check directory for save files and load data.
	/// Game class will overwrite all the other class default values when player triggers a load state.
	/// </summary>
	public partial class Game 
	{
		#region Variables
		const string FilePokemonXML = "";
		//ToDo: ReadOnly Immutable Dictionaries...
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]> PokemonItemsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.Form[]> PokemonFormsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; private set; }
		public static Dictionary<Natures,Nature> NatureData { get; private set; }
		public static Dictionary<Moves,Attack.Data.MoveData> MoveData { get; private set; }
		public static Dictionary<Moves,Attack.Data.MetaData> MoveMetaData { get; private set; }
		public static Dictionary<Items,ItemData> ItemData { get; private set; }
		public static Dictionary<Items,Berry> BerryData { get; private set; }
		public static Dictionary<Regions,Locations[]> RegionData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Locations"/> | Value: <seealso cref="Area.Id"/>
		/// </summary>
		public static Dictionary<Locations,int[]> LocationData { get; private set; }
		/// <summary>
		/// List of <see cref="Area"/> that triggers <seealso cref="Encounter"/>
		/// <para></para>
		/// Key: <seealso cref="Area.Id"/> | Value: <seealso cref="Area"/>
		/// </summary>
		public static Dictionary<int,Area> AreaData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Method"/> | Value: <seealso cref="Encounter.Id"/>
		/// </summary>
		public static Dictionary<Method,int[]> MethodData { get; private set; }
		/// <summary>
		/// Key: <seealso cref="Encounter.Id"/> | Value: <seealso cref="Encounter"/>
		/// </summary>
		public static Dictionary<int,Encounter> EncounterData { get; private set; }
		public static Dictionary<TrainerTypes,TrainerData> TrainerMetaData { get; private set; }
		//public static Dictionary<Items,KeyValuePair<Moves,int[]>[]> MachineMetaData { get; private set; }
		//public static Dictionary<int,Machine> MachineData { get; private set; }
		//public static Dictionary<int,Machine> MapData { get; private set; }
		//public static Dictionary<int,int> TextureData1 { get; private set; }
		//public static Dictionary<int,int> TextureData2 { get; private set; }
		//public static Dictionary<Location,Script[]> ScriptData { get; private set; }
		//public static Dictionary<Location,Script[]> LevelScriptData { get; private set; }
		//public static Dictionary<Location,Text[]> TextData { get; private set; }
		//public static Dictionary<Location,EventData> EventData { get; private set; }
		//public static Dictionary<Location,Event[]> EventData { get; private set; }
		//public static Dictionary<Location,EventFlags[]> MapFlagData { get; private set; }
		#endregion

		#region Initialize Datab From Database
		public static bool InitPokemons(bool sql = true)
		{
			PokemonData = new Dictionary<Pokemons, Monster.Data.PokemonData>();
			if (sql) //using (con)
				return GetPokemonsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitPokemonMoves(bool sql = true)
		{
			PokemonMovesData = new Dictionary<Pokemons, Monster.Data.PokemonMoveTree>();
			if (sql) //using (con)
				return GetPokemonMovesFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitPokemonEvolutions(bool sql = true)
		{
			PokemonEvolutionsData = new Dictionary<Pokemons, Monster.Data.PokemonEvolution[]>();
			if (sql) //using (con)
				return GetPokemonEvolutionsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitPokemonItems(bool sql = true)
		{
			PokemonItemsData = new Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonWildItems[]>();
			if (sql) //using (con)
				return GetPokemonItemsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitPokemonForms(bool sql = true)
		{
			PokemonFormsData = new Dictionary<Pokemons, Monster.Data.Form[]>();
			if (sql) //using (con)
				return GetPokemonFormsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitNatures(bool sql = true)
		{
			NatureData = new Dictionary<Natures, Nature>();
			if (sql) //using (con)
				return GetNaturesFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitMoves(bool sql = true)
		{
			MoveData = new Dictionary<Moves, Attack.Data.MoveData>();
			MoveMetaData = new Dictionary<Moves, Attack.Data.MetaData>();
			if (sql) //using (con)
				return GetMovesFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitItems(bool sql = true)
		{
			ItemData = new Dictionary<Items, ItemData>();
			if (sql) //using (con)
				return GetItemsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitBerries(bool sql = true)
		{
			BerryData = new Dictionary<Items, Berry>();
			if (sql) //using (con)
				return GetBerriesFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitRegions(bool sql = true)
		{
			RegionData = new Dictionary<Regions, Locations[]>();
			if (sql) //using (con)
				return  GetRegionsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitLocations(bool sql = true)
		{
			LocationData = new Dictionary<Locations, int[]>();
			AreaData = new Dictionary<int, Area>();
			if (sql) //using (con)
				return  GetLocationsFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitEncounters(bool sql = true)
		{
			MethodData = new Dictionary<Method, int[]>();
			EncounterData = new Dictionary<int, Encounter>();
			if (sql) //using (con)
				return GetEncountersFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitMachines(bool sql = true)
		{
			//MethodData = new Dictionary<Method, int[]>();
			//EncounterData = new Dictionary<int, Encounter>();
			if (sql) //using (con)
				return GetMachinesFromSQL(con);
			else return GetPokemonsFromXML();
		}
		public static bool InitTrainers(bool sql = true)
		{
			TrainerMetaData = new Dictionary<TrainerTypes, TrainerData>();
			//TrainerData = new Dictionary<int, Encounter>();
			if (sql) //using (con)
				return GetTrainersFromSQL(con);
			else return GetPokemonsFromXML();
		}
		#endregion

		#region From XML
		static bool GetPokemonsFromXML()
		{
			PokemonData = new Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData>();

			var xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(LockFileStream(FilePokemonXML));

			var pokemons = xmlDocument.SelectNodes("/Pokemons");
			if (pokemons == null || pokemons.Count <= 0)
			{
				//throw new Exception("Xml must include "Pokemons" as root node.");
				return false;
			}

			var dublicateNames = new List<string>();

			var pokemonNodes = xmlDocument.SelectSingleNode("/Pokemons").ChildNodes;// /text
			if (pokemonNodes != null)
			{
				//foreach value in xml
				foreach (XmlNode node in pokemonNodes)
				{
					//XmlAttribute sngl = node.GetAttributeValueOrNull("SingleGender");
					PokemonData.Add((Pokemons)int.Parse(node.GetAttributeValueOrNull("Id")),
						new PokemonUnity.Monster.Data.PokemonData(
							Id: (Pokemons)int.Parse(node.GetAttributeValueOrNull("Id"))
							,regionalDex: new int[] { int.Parse(node.GetAttributeValueOrNull("ReDex")) }
							,type1: (Types)int.Parse(node.GetAttributeValueOrNull("Type1"))
							,type2: (Types)int.Parse(node.GetAttributeValueOrNull("Type2"))
							,ability1: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability1"))
							,ability2: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability2"))
							,hiddenAbility: (Abilities)int.Parse(node.GetAttributeValueOrNull("Ability3"))
							,genderRatio: (PokemonUnity.Monster.GenderRatio)int.Parse(node.GetAttributeValueOrNull("Gender"))
							//,maleRatio: float.Parse(node.GetAttributeValueOrNull("Gender"))
							,catchRate: int.Parse(node.GetAttributeValueOrNull("Catch"))
							,eggGroup1: (PokemonUnity.Monster.EggGroups)int.Parse(node.GetAttributeValueOrNull("Egg1"))
							,eggGroup2: (PokemonUnity.Monster.EggGroups)int.Parse(node.GetAttributeValueOrNull("Egg2"))
							,hatchTime: int.Parse(node.GetAttributeValueOrNull("Hatch"))
							,height: float.Parse(node.GetAttributeValueOrNull("Height"))
							,weight: float.Parse(node.GetAttributeValueOrNull("Weight"))
							,baseExpYield: int.Parse(node.GetAttributeValueOrNull("Exp"))
							,levelingRate: (PokemonUnity.Monster.LevelingRate)int.Parse(node.GetAttributeValueOrNull("Growth"))
							,evHP:  int.Parse(node.GetAttributeValueOrNull("eHP") )
							,evATK: int.Parse(node.GetAttributeValueOrNull("eATK"))
							,evDEF: int.Parse(node.GetAttributeValueOrNull("eDEF"))
							,evSPA: int.Parse(node.GetAttributeValueOrNull("eSPA"))
							,evSPD: int.Parse(node.GetAttributeValueOrNull("eSPD"))
							,evSPE: int.Parse(node.GetAttributeValueOrNull("eSPE"))
							,pokedexColor: (Color)int.Parse(node.GetAttributeValueOrNull("Color"))
							,baseFriendship: int.Parse(node.GetAttributeValueOrNull("Friend"))
							,baseStatsHP:  int.Parse(node.GetAttributeValueOrNull("HP" ))
							,baseStatsATK: int.Parse(node.GetAttributeValueOrNull("ATK"))
							,baseStatsDEF: int.Parse(node.GetAttributeValueOrNull("DEF"))
							,baseStatsSPA: int.Parse(node.GetAttributeValueOrNull("SPA"))
							,baseStatsSPD: int.Parse(node.GetAttributeValueOrNull("SPD"))
							,baseStatsSPE: int.Parse(node.GetAttributeValueOrNull("SPE"))
							,rarity: (Rarity)int.Parse(node.GetAttributeValueOrNull("Rarity"))
							//,luminance: Data.luminance
							//,movesetmoves: Data.movesetmoves
							//,movesetLevels: Data.movesetLevels
							//,movesetMoves: Data.movesetMoves
							//,tmList: Data.tmList
							//,evolution: Data.evolution
							//,evolutionID: Data.EvolutionTO
							//,evolutionLevel: Data.evolutionLevel
							//,evolutionMethod: Data.evolutionMethod						
							//,evolutionFROM: Data.EvolutionFROM
							//,evolutionTO: Data.EvolutionTO
							,evoChainId: int.Parse(node.GetAttributeValueOrNull("EvoChainId"))
							,generationId: (byte)int.Parse(node.GetAttributeValueOrNull("GenerationId"))
							,isDefault:		node.GetAttributeValueOrNull("IsBaseForm") == "1"
							,isBaby:			node.GetAttributeValueOrNull("IsBaby") == "1"
							,formSwitchable:	node.GetAttributeValueOrNull("SwitchableForm") == "1"
							,hasGenderDiff:	node.GetAttributeValueOrNull("GenderDiff") == "1"
							,habitatId: (PokemonUnity.Monster.Habitat)int.Parse(node.GetAttributeValueOrNull("HabitatId"))
							,shapeId: (PokemonUnity.Monster.Shape)int.Parse(node.GetAttributeValueOrNull("ShapeId"))
							//,baseForm: Data.baseForm
							//,heldItem: Data.heldItem
						)
					);
				}
			}

			//dictionaries cant store duplicate values...
			//if (dublicateNames.Count > 0)
			//{
			//	//throw new Exception("A dictionary can not contain same key twice. There are some duplicated names: " + dublicateNames.JoinAsString(", "));//string.Join(", ",dublicateNames.ToArray())
			//}

			return true; //dictionary;
		}
		#endregion
		#region From SQL
		static bool GetPokemonsFromSQL(SQLiteConnection con)
		{
			try
			{
				//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				//{ 
					//Step 3: Running a Command
					SQLiteCommand stmt = con.CreateCommand();

					#region DataReader
					stmt.CommandText = "select * from pokemon_views --order by id ASC";
					//	@"select pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon.""order""
					//pokemon_abilities_view.ability1, pokemon_abilities_view.ability2, pokemon_abilities_view.ability3, 
					//pokemon_egg_groups_view.egg_group1, pokemon_egg_groups_view.egg_group2,
					//pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
					//pokemon_types_view.type1, pokemon_types_view.type2,
					//pokemon_color_names.name as color,
					//pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species.""order"",
					//evolution_chains.baby_trigger_item_id as incense,
					//pokemon_species_names.name,pokemon_species_names.genus,
					//pokemon_species_flavor_text.flavor_text
					//from pokemon
					//left join pokemon_abilities_view on pokemon.id = pokemon_abilities_view.pokemon_id
					//left join pokemon_egg_groups_view on pokemon_egg_groups_view.pokemon_id = pokemon.id
					//left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id
					//left join pokemon_types_view on pokemon_types_view.pokemon_id = pokemon.id
					//left join pokemon_species on pokemon_species.id = pokemon.species_id
					//left join evolution_chains on evolution_chains.id = pokemon_species.evolution_chain_id
					//left join pokemon_colors on pokemon_colors.id = pokemon_species.color_id
					//left join pokemon_color_names on pokemon_color_names.pokemon_color_id = pokemon_colors.id AND pokemon_color_names.local_language_id = 9
					//left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.id AND pokemon_species_names.local_language_id = 9
					//left join pokemon_species_flavor_text on pokemon_species_flavor_text.species_id = pokemon.id AND pokemon_species_flavor_text.version_id = 26 AND pokemon_species_flavor_text.language_id = 9
					//order by pokemon.id ASC;";
					SQLiteDataReader reader = stmt.ExecuteReader();

					//Step 4: Read the results
					using(reader)
					{
						PokemonData.Add(Pokemons.NONE, new Monster.Data.PokemonData(Id:Pokemons.NONE, hatchTime: 1000));
						while(reader.Read()) //if(reader.Read())
						{
							PokemonData.Add((Pokemons)int.Parse((string)reader["id"].ToString()),
								new PokemonUnity.Monster.Data.PokemonData(
									Id: (Pokemons)int.Parse((string)reader["id"].ToString())
									,baseForm: (Pokemons)int.Parse((string)reader["species_id"].ToString())
									//,regionalDex: new int[].ToString() { int.Parse((string)reader["ReDex"].ToString()) }
									,type1: (Types)int.Parse(string.IsNullOrEmpty((string)reader["type1"].ToString()) ? "0" : (string)reader["type1"].ToString())
									,type2: (Types)int.Parse(string.IsNullOrEmpty((string)reader["type2"].ToString()) ? "0" : (string)reader["type2"].ToString())
									,ability1: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability1"].ToString()) ? "0" : (string)reader["ability1"].ToString())
									,ability2: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability2"].ToString()) ? "0" : (string)reader["ability2"].ToString())
									,hiddenAbility: (Abilities)int.Parse(string.IsNullOrEmpty((string)reader["ability3"].ToString()) ? "0" : (string)reader["ability3"].ToString())
									,genderRatio: (PokemonUnity.Monster.GenderRatio)int.Parse(string.IsNullOrEmpty((string)reader["gender_rate"].ToString()) ? "0" : (string)reader["gender_rate"].ToString())
									//,maleRatio: float.Parse((string)reader["Gender"].ToString())
									,catchRate: int.Parse(string.IsNullOrEmpty((string)reader["capture_rate"].ToString()) ? "0" : (string)reader["capture_rate"].ToString())
									,eggGroup1: (PokemonUnity.Monster.EggGroups)int.Parse(string.IsNullOrEmpty((string)reader["egg_group1"].ToString()) ? "0" : (string)reader["egg_group1"].ToString())
									,eggGroup2: (PokemonUnity.Monster.EggGroups)int.Parse(string.IsNullOrEmpty((string)reader["egg_group2"].ToString()) ? "0" : (string)reader["egg_group2"].ToString())
									,hatchTime: int.Parse(string.IsNullOrEmpty((string)reader["hatch_counter"].ToString()) ? "0" : (string)reader["hatch_counter"].ToString())
									,height: float.Parse((string)reader["height"].ToString())
									,weight: float.Parse((string)reader["weight"].ToString())
									,baseExpYield: int.Parse(string.IsNullOrEmpty((string)reader["base_experience"].ToString()) ? "0" : (string)reader["base_experience"].ToString())
									,levelingRate: (PokemonUnity.Monster.LevelingRate)int.Parse(string.IsNullOrEmpty((string)reader["growth_rate_id"].ToString()) ? "0" : (string)reader["growth_rate_id"].ToString())
									,evHP:  int.Parse((string)reader["ehp"].ToString())
									,evATK: int.Parse((string)reader["eatk"].ToString())
									,evDEF: int.Parse((string)reader["edef"].ToString())
									,evSPA: int.Parse((string)reader["espa"].ToString())
									,evSPD: int.Parse((string)reader["espd"].ToString())
									,evSPE: int.Parse((string)reader["espe"].ToString())
									,pokedexColor: (Color)int.Parse(string.IsNullOrEmpty((string)reader["color_id"].ToString()) ? "0" : (string)reader["color_id"].ToString())
									,baseFriendship: int.Parse(string.IsNullOrEmpty((string)reader["base_happiness"].ToString()) ? "0" : (string)reader["base_happiness"].ToString())
									,baseStatsHP:  int.Parse((string)reader["bhp"].ToString())
									,baseStatsATK: int.Parse((string)reader["batk"].ToString())
									,baseStatsDEF: int.Parse((string)reader["bdef"].ToString())
									,baseStatsSPA: int.Parse((string)reader["bspa"].ToString())
									,baseStatsSPD: int.Parse((string)reader["bspd"].ToString())
									,baseStatsSPE: int.Parse((string)reader["bspe"].ToString())
									//,rarity: (Rarity)int.Parse((string)reader["Rarity"].ToString())
									//,luminance: Data.luminance
									//,movesetmoves: Data.movesetmoves
									//,movesetLevels: Data.movesetLevels
									//,movesetMoves: Data.movesetMoves
									//,tmList: Data.tmList
									//,evolution: Data.evolution
									//,evolutionID: Data.EvolutionTO
									//,evolutionLevel: Data.evolutionLevel
									//,evolutionMethod: Data.evolutionMethod						
									//,evolutionFROM: Data.EvolutionFROM
									//,evolutionTO: Data.EvolutionTO
									,incense: (Items)int.Parse(string.IsNullOrEmpty((string)reader["incense"].ToString()) ? "0" : (string)reader["incense"].ToString())
									,evoChainId: int.Parse(string.IsNullOrEmpty((string)reader["evolution_chain_id"].ToString()) ? "0" : (string)reader["evolution_chain_id"].ToString())
									,generationId: (byte)int.Parse(string.IsNullOrEmpty((string)reader["generation_id"].ToString()) ? "0" : (string)reader["generation_id"].ToString())
									//,isDefault: (string)reader["IsBaseForm"].ToString() == "1"
									,isBaby: (string)reader["is_baby"].ToString() == "1"
									,formSwitchable: (string)reader["forms_switchable"].ToString() == "1"
									,hasGenderDiff: (string)reader["has_gender_differences"].ToString() == "1"
									,habitatId: (PokemonUnity.Monster.Habitat)int.Parse(string.IsNullOrEmpty((string)reader["habitat_id"].ToString()) ? "0" : (string)reader["habitat_id"].ToString())
									,shapeId: (PokemonUnity.Monster.Shape)int.Parse(string.IsNullOrEmpty((string)reader["shape_id"].ToString()) ? "0" : (string)reader["shape_id"].ToString())
									//,heldItem: Data.heldItem
								)
							);
						}
					//}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonMovesFromSQL(SQLiteConnection con)
		{
			try
			{
				//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				//{ 
					//Step 3: Running a Command
					SQLiteCommand stmt = con.CreateCommand();

					#region DataReader
					stmt.CommandText = "select distinct move_id, pokemon_id, [level], pokemon_move_method_id from pokemon_moves --where version_group_id=18 order by pokemon_id ASC";
					//	@"select pokemon_moves.pokemon_id, pokemon_moves.move_id, pokemon_moves.level, pokemon_moves.pokemon_move_method_id, pokemon_moves."order", pokemon_moves.version_group_id,
					//pokemon_move_methods.identifier 
					//from pokemon_moves 
					//left join pokemon_move_methods on pokemon_move_methods.id=pokemon_moves.pokemon_move_method_id 
					//where version_group_id=18 
					//--order by pokemon_id ASC";
					SQLiteDataReader reader = stmt.ExecuteReader();

					//Step 4: Read the results
					using(reader)
					{
						Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>> p = new Dictionary<Pokemons, List<Monster.Data.PokemonMoveset>>();
						while(reader.Read()) //if(reader.Read())
						{
							if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
								p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
									new List<Monster.Data.PokemonMoveset>());
							p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
								new PokemonUnity.Monster.Data.PokemonMoveset(
									moveId: (Moves)int.Parse((string)reader["move_id"].ToString())
									//pokemonId: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
									//,generation: int.Parse((string)reader["version_group_id"].ToString())
									,method: (LearnMethod)int.Parse((string)reader["pokemon_move_method_id"].ToString())
									,level: int.Parse((string)reader["level"].ToString())
									//,order: int.Parse((string)reader["order"].ToString())
								)
							);
						}
					//}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					PokemonMovesData.Add(Pokemons.NONE, new Monster.Data.PokemonMoveTree(new Monster.Data.PokemonMoveset[] { }));
					foreach (var pkmn in p)
					{
						PokemonMovesData.Add(pkmn.Key, new Monster.Data.PokemonMoveTree(pkmn.Value.ToArray()));
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonFormsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.Form>> p = new Dictionary<Pokemons, List<Monster.Data.Form>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= PokemonData.Keys.Length; n++)
				{
					p.Add(x, new List<Monster.Data.Form>());
				}
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = @"select pokemon_forms.id, pokemon_forms.form_identifier, pokemon_forms.is_default, pokemon_forms.is_battle_only, pokemon_forms.is_mega, pokemon_forms.form_order, pokemon_forms.""order"",
				pokemon.id as pokemon_id, pokemon.species_id as species_id--, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon.""order""
				--pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
				--pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species.""order""
				from pokemon
				left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
				left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id 
				left join pokemon_species on pokemon_species.id = pokemon.species_id
				--where --pokemon_species.id != pokemon.id AND
				--pokemon_forms.identifier != pokemon.identifier OR
				--pokemon_forms.id != pokemon.id
				--pokemon_forms.id != pokemon_species.id
				order by pokemon.species_id, pokemon_forms.form_order ASC;";
				//	@"select pokemon_forms.id, pokemon_forms.identifier,
				//pokemon.id, pokemon.species_id, pokemon.identifier, pokemon.height, pokemon.weight, pokemon.base_experience, --pokemon."order"
				//pokemon_stats_view.bhp, pokemon_stats_view.batk, pokemon_stats_view.bdef, pokemon_stats_view.bspa, pokemon_stats_view.bspd, pokemon_stats_view.bspe, pokemon_stats_view.ehp, pokemon_stats_view.eatk, pokemon_stats_view.edef, pokemon_stats_view.espa, pokemon_stats_view.espd, pokemon_stats_view.espe,
				//pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species."order"
				//from pokemon
				//left join pokemon_forms on pokemon_forms.pokemon_id = pokemon.id
				//left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id 
				//left join pokemon_species on pokemon_species.id = pokemon.species_id
				//left join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.species_id AND pokemon_species_names.local_language_id=9
				//where --pokemon_species.id != pokemon.id AND
				//pokemon_forms.identifier != pokemon.identifier OR
				//--pokemon_forms.id != pokemon.id
				//pokemon_forms.id != pokemon_species.id
				//order by pokemon.species_id, pokemon_forms.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						//if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
						//	p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
						//		new List<Monster.Data.Form>());
						p[(Pokemons)int.Parse((string)reader["species_id"].ToString())].Add(
							new PokemonUnity.Monster.Data.Form(
								id: (Forms)int.Parse((string)reader["id"].ToString())
								,pkmn: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								,species: (Pokemons)int.Parse((string)reader["species_id"].ToString())
								,identifier: (string)reader["form_identifier"].ToString()
								,isMega: (string)reader["is_mega"].ToString() == "1"
								,isDefault: (string)reader["is_default"].ToString() == "1"
								,isBattleOnly: (string)reader["is_battle_only"].ToString() == "1"
								,formOrder: (byte)int.Parse((string)reader["form_order"].ToString())
								,order: int.Parse((string)reader["order"].ToString())
							)
						);
						if (int.Parse((string)reader["pokemon_id"].ToString()) != int.Parse((string)reader["species_id"].ToString()))
							p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
								new PokemonUnity.Monster.Data.Form(
									id: (Forms)int.Parse((string)reader["id"].ToString())
									,pkmn: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
									,species: (Pokemons)int.Parse((string)reader["species_id"].ToString())
									,identifier: (string)reader["form_identifier"].ToString()
									,isMega: (string)reader["is_mega"].ToString() == "1"
									,isDefault: (string)reader["is_default"].ToString() == "1"
									,isBattleOnly: (string)reader["is_battle_only"].ToString() == "1"
									,formOrder: (byte)int.Parse((string)reader["form_order"].ToString())
									,order: int.Parse((string)reader["order"].ToString())
								)
							);
					}
				}
				//Step 5: Closing up
				reader.Close();
				reader.Dispose();
				#endregion
				foreach (var pkmn in p)
				{
					PokemonFormsData.Add(pkmn.Key, pkmn.Value
						//.OrderBy(x => x.FormOrder)
						.ToArray());
					//if (PokemonFormsData[pkmn.Value[0].Pokemon].Length == 0)
					if (pkmn.Key != Pokemons.NONE && pkmn.Value[0].Pokemon != pkmn.Value[0].Base)
						PokemonFormsData[pkmn.Key] = PokemonFormsData[pkmn.Value[0].Base];
				}
				PokemonFormsData[Pokemons.NONE] = new Monster.Data.Form[] { new Monster.Data.Form(Forms.NONE, Pokemons.NONE, Pokemons.NONE) };
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonEvolutionsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.EvolutionTrigger>> p = new Dictionary<Pokemons, List<Monster.Data.EvolutionTrigger>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= Enum.GetValues(typeof(Pokemons)).Length; n++)
				{
					//p.Add(x, new List<Monster.Data.EvolutionTrigger>());
					PokemonEvolutionsData[x] = new Monster.Data.PokemonEvolution[0];
				} 
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from pokemon_evolution_view";
					@"select pokemon_species.id, pokemon_species.identifier, pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.""order"",
				pokemon_evolution.evolution_trigger_id, pokemon_evolution.trigger_item_id, pokemon_evolution.minimum_level, pokemon_evolution.gender_id, pokemon_evolution.location_id, pokemon_evolution.held_item_id, pokemon_evolution.time_of_day, pokemon_evolution.known_move_id, pokemon_evolution.known_move_type_id, pokemon_evolution.minimum_happiness, pokemon_evolution.minimum_beauty, pokemon_evolution.minimum_affection, pokemon_evolution.relative_physical_stats, pokemon_evolution.party_species_id, pokemon_evolution.party_type_id, pokemon_evolution.trade_species_id, pokemon_evolution.needs_overworld_rain, pokemon_evolution.turn_upside_down
				from pokemon_evolution
				left join pokemon_species on pokemon_evolution.evolved_species_id = pokemon_species.id
				--group by pokemon_species.""order"" --pokemon_species.id --pokemon_species.evolution_chain_id
				order by pokemon_species.evolution_chain_id, pokemon_species.id;
				--pokemon_species.evolves_from_species_id, pokemon_species.""order"";";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						Pokemons id = (Pokemons)int.Parse((string)reader["evolves_from_species_id"].ToString());
						if (!p.ContainsKey(id))
							p.Add(id, new List<Monster.Data.EvolutionTrigger>());
						//if list.conains evolve id && evolve by level && (location id == null || gender id == null || )
						p[id].Add(
							new PokemonUnity.Monster.Data.EvolutionTrigger(
								species: (Pokemons)int.Parse((string)reader["id"].ToString())
								,evo: (Monster.Data.EvoTrigger)int.Parse((string)reader["evolution_trigger_id"].ToString())
								,trigger: string.IsNullOrEmpty((string)reader["trigger_item_id"].ToString())		? Items.NONE : (Items)int.Parse((string)reader["trigger_item_id"].ToString())
								,minLevel:string.IsNullOrEmpty((string)reader["minimum_level"].ToString())			? (int?)null : int.Parse((string)reader["minimum_level"].ToString())
								,gender: string.IsNullOrEmpty((string)reader["gender_id"].ToString())				? (bool?)null : (bool)((string)reader["gender_id"].ToString() == "1")
								,location:string.IsNullOrEmpty((string)reader["location_id"].ToString())			? (int?)null : int.Parse((string)reader["location_id"].ToString())
								,held: string.IsNullOrEmpty((string)reader["held_item_id"].ToString())				? Items.NONE : (Items)int.Parse((string)reader["held_item_id"].ToString())
								,time:string.IsNullOrEmpty((string)reader["time_of_day"].ToString())				? (int?)null : ((string)reader["time_of_day"].ToString() == "day" ? 1 : 0) //int.Parse((string)reader["time_of_day"].ToString())
								,knownMove: string.IsNullOrEmpty((string)reader["known_move_id"].ToString())		? Moves.NONE: (Moves)int.Parse((string)reader["known_move_id"].ToString())
								,knownType: string.IsNullOrEmpty((string)reader["known_move_type_id"].ToString())	? Types.NONE : (Types)int.Parse((string)reader["known_move_type_id"].ToString())
								,happiness:string.IsNullOrEmpty((string)reader["minimum_happiness"].ToString())		? (int?)null : int.Parse((string)reader["minimum_happiness"].ToString())
								,beauty:string.IsNullOrEmpty((string)reader["minimum_beauty"].ToString())			? (int?)null : int.Parse((string)reader["minimum_beauty"].ToString())
								,affection:string.IsNullOrEmpty((string)reader["minimum_affection"].ToString())		? (int?)null : int.Parse((string)reader["minimum_affection"].ToString())
								,physicalStat:string.IsNullOrEmpty((string)reader["relative_physical_stats"].ToString()) ? (int?)null : int.Parse((string)reader["relative_physical_stats"].ToString())
								,partySpecies: string.IsNullOrEmpty((string)reader["party_species_id"].ToString())	? Pokemons.NONE : (Pokemons)int.Parse((string)reader["party_species_id"].ToString())
								,partyType: string.IsNullOrEmpty((string)reader["party_type_id"].ToString())		? Types.NONE : (Types)int.Parse((string)reader["party_type_id"].ToString())
								,tradeSpecies: string.IsNullOrEmpty((string)reader["trade_species_id"].ToString())	? Pokemons.NONE : (Pokemons)int.Parse((string)reader["trade_species_id"].ToString())
								,overworldRain: (string)reader["needs_overworld_rain"].ToString() == "1"
								,turnUpsideDown: (string)reader["turn_upside_down"].ToString() == "1"
							)
						);
					}
				}
				//Step 5: Closing up
				reader.Close();
				reader.Dispose();
				#endregion
				//PokemonEvolutionsData.Add(Pokemons.NONE, new Monster.Data.PokemonEvolution[] { });
				foreach (var pkmn in p)
				{
					//ToDo: Fix bad duplicates
					//if (pkmn.Key == Pokemons.ROCKRUFF)
					//{
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count+1];
					//	PokemonEvolutionsData[pkmn.Key][2] = new Monster.Data.PokemonEvolution(pkmn.Value[2].Species, EvolutionMethod.Lycanroc, pkmn.Value[2].MinLevel);
					//}
					//else if (pkmn.Key == Pokemons.MAGNETON || pkmn.Key == Pokemons.NOSEPASS)// || pkmn.Key == Pokemons.ESPURR
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count-1];
					//else if (pkmn.Key == Pokemons.EEVEE)
					//	PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count-2];
					//else
						PokemonEvolutionsData[pkmn.Key] = new Monster.Data.PokemonEvolution[pkmn.Value.Count];
					for(int i = 0; i < pkmn.Value.Count; i++)
					{
						//if (!pkmn.Value[i].Location.HasValue && (pkmn.Key == Pokemons.MAGNETON || pkmn.Key == Pokemons.NOSEPASS || pkmn.Value[i].Species == Pokemons.LEAFEON || pkmn.Value[i].Species == Pokemons.GLACEON))// || pkmn.Key == Pokemons.ESPURR
						//	continue;
						#region Happiness Evolution
						if (pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 0 //"night" 
							&& pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HappinessNight, pkmn.Value[i].Happiness.Value);
							continue;
						}
						else if (pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 1 //"day" 
								&& pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HappinessDay, pkmn.Value[i].Happiness.Value);
							continue;
						}
						else if (pkmn.Value[i].Happiness.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Happiness, pkmn.Value[i].Happiness.Value);
							continue;
						}
						#endregion

						#region Item Evolution
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == true //"1" 
							&& pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.ItemFemale, pkmn.Value[i].Trigger);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == false //"2" 
							&& pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.ItemMale, pkmn.Value[i].Trigger);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.use_item && pkmn.Value[i].Trigger != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Item, pkmn.Value[i].Trigger);
							continue;
						}
						#endregion

						#region Location Evolution
						if (pkmn.Value[i].Location.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Location, pkmn.Value[i].Location.Value);
							continue;
						}
						#endregion

						#region Trade Evolution
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].TradeSpecies != Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.TradeSpecies, pkmn.Value[i].TradeSpecies);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].Held != Items.NONE && pkmn.Value[i].TradeSpecies == Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.TradeItem, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.trade && pkmn.Value[i].Held == Items.NONE && pkmn.Value[i].TradeSpecies == Pokemons.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Trade, null);
							continue;
						}
						#endregion

						#region Hold Item
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 1 //"day" 
								&& pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItemDay, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Time.HasValue && pkmn.Value[i].Time.Value == 0 //"night" 
							&& pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItemNight, pkmn.Value[i].Held);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Held != Items.NONE)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.HoldItem, pkmn.Value[i].Held);
							continue;
						}
						#endregion

						#region Beauty/Affection
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Beauty.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Beauty, pkmn.Value[i].Beauty.Value);
							continue;
						}
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Affection.HasValue)
						{
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Affection, pkmn.Value[i].KnownType);
							continue;
						}
						#endregion

						#region Move
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].KnownMove != Moves.NONE && pkmn.Value[i].PartySpecies == Pokemons.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Moves>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Moves.{tempMove.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Move, pkmn.Value[i].KnownMove);
							continue;
						}
						#endregion

						#region Party
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PartySpecies != Pokemons.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Pokemons>(Pokemons.{name.ToUpper()}, EvolutionMethod.Party, Pokemons.{pkmn.Value[i].Species.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Party, pkmn.Value[i].PartySpecies);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].KnownType != Types.NONE)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<Types>(Pokemons.{name.ToUpper()}, EvolutionMethod.Move, Types.{tempType.ToUpper()}),";
							//break;
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Type, pkmn.Value[i].PartyType);
							continue;
						}
						#endregion

						#region Attack > Defense > Equal =
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == 1)           //Attack Greater Than Defense (Attack > Defense)    1
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AttackGreater),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.AttackGreater, pkmn.Value[i].PartyType);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == -1)    //Defense Greater Than Attack (Attack < Defense)    -1
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.DefenseGreater),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.DefenseGreater, pkmn.Value[i].PartyType);
							continue;
						}
						else if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].PhysicalStat.HasValue && pkmn.Value[i].PhysicalStat.Value == 0)    //Attack Equal To Attack (Attack = Defense)         0
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.AtkDefEqual),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.AtkDefEqual, pkmn.Value[i].PartyType);
							continue;
						}
						#endregion

						#region Silcoon
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.SILCOON)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Silcoon),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Silcoon);
							continue;
						}
						#endregion

						#region Cascoon
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.CASCOON)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Cascoon),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Cascoon);
							continue;
						}
						#endregion

						#region Ninjask
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.NINJASK)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Ninjask),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Ninjask);
							continue;
						}
						#endregion

						#region Shedinja
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Species == Pokemons.SHEDINJA)
						{
							//MethodCode = MethodCode + $"\n\tnew PokemonEvolution(Pokemons.{name.ToUpper()}, EvolutionMethod.Shedinja),";
							PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Shedinja);
							continue;
						}
						#endregion

						#region Level
						if (pkmn.Value[i].Evo == Monster.Data.EvoTrigger.level_up && pkmn.Value[i].Trigger == Items.NONE && pkmn.Value[i].MinLevel.HasValue && !pkmn.Value[i].Happiness.HasValue && !pkmn.Value[i].TurnUpsideDown)
						{
							if (pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == true) //"1" 
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelFemale, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.LevelFemale, pkmn.Value[i].MinLevel);
								continue;
							}
							else if (pkmn.Value[i].Gender.HasValue && pkmn.Value[i].Gender.Value == false) //"2"
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.LevelMale, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.LevelMale, pkmn.Value[i].MinLevel);
								continue;
							}
							else if (pkmn.Value[i].Time.HasValue) //"Lycanroc"
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Lycanroc, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Lycanroc, pkmn.Value[i].MinLevel);
								continue;
							}
							else
							{
								//MethodCode = MethodCode + $"\n\tnew PokemonEvolution<int>(Pokemons.{name.ToUpper()}, EvolutionMethod.Level, {csv.Context.Record[4]}),";
								PokemonEvolutionsData[pkmn.Key][i] = new Monster.Data.PokemonEvolution(pkmn.Value[i].Species, EvolutionMethod.Level, pkmn.Value[i].MinLevel);
								continue;
							}
						}
						#endregion
						//continue;
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetPokemonItemsFromSQL(SQLiteConnection con)
		{
			try
			{
				Dictionary<Pokemons, List<Monster.Data.PokemonWildItems>> p = new Dictionary<Pokemons, List<Monster.Data.PokemonWildItems>>();
				foreach (Pokemons x in PokemonData.Keys)//for(int n = 1; n <= PokemonData.Keys.Length; n++)
				{
					p.Add(x, new List<Monster.Data.PokemonWildItems>());
				} 
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = "select * from pokemon_items group by pokemon_id, item_id";
				//	@"select * 
				//from pokemon_items
				//group by pokemon_id, item_id";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					while(reader.Read()) //if(reader.Read())
					{
						//if (!p.ContainsKey((Pokemons)int.Parse((string)reader["pokemon_id"].ToString())))
						//	p.Add((Pokemons)int.Parse((string)reader["pokemon_id"].ToString()),
						//		new List<Monster.Data.PokemonWildItems>());
						p[(Pokemons)int.Parse((string)reader["pokemon_id"].ToString())].Add(
							new PokemonUnity.Monster.Data.PokemonWildItems(
								itemId: (Items)int.Parse((string)reader["item_id"].ToString())
								,generation: int.Parse((string)reader["version_id"].ToString())
								,rarity: int.Parse((string)reader["rarity"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//PokemonItemsData.Add(Pokemons.NONE, new Monster.Data.PokemonWildItems[] { });
					foreach (var pkmn in p)
					{
						PokemonItemsData.Add(pkmn.Key, pkmn.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetNaturesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = "select * from natures order by game_index ASC";
				//	@"";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//NatureData.Add(Natures.UNSET, new Nature());
					while(reader.Read()) //if(reader.Read())
					{
						NatureData.Add((Natures)(int.Parse((string)reader["id"].ToString())-1),
							new Nature(
								nature: (Natures)(int.Parse((string)reader["id"].ToString())-1)
								,increase: (Stats)(int.Parse((string)reader["increased_stat_id"].ToString())-1)
								,decrease: (Stats)(int.Parse((string)reader["decreased_stat_id"].ToString())-1)
								,like: (Flavours)int.Parse((string)reader["likes_flavor_id"].ToString())
								,dislike: (Flavours)int.Parse((string)reader["hates_flavor_id"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetMovesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from moves_view";
					@"select moves.id, moves.identifier, moves.generation_id, moves.type_id, moves.power, moves.pp, moves.accuracy, moves.priority, moves.target_id, moves.damage_class_id, moves.effect_id, moves.effect_chance, moves.contest_type_id, moves.contest_effect_id, moves.super_contest_effect_id,
				contest_effects.appeal, contest_effects.jam, super_contest_effects.appeal as super_appeal,
				move_meta.meta_category_id, move_meta.meta_ailment_id, move_meta.min_hits, move_meta.max_hits, move_meta.min_turns, move_meta.max_turns, move_meta.drain, move_meta.healing, move_meta.crit_rate, move_meta.ailment_chance, move_meta.flinch_chance, move_meta.stat_chance,
				--move_flags.identifier as move_flag,
				move_flag_map_view.move_flag_group,
				move_effect_prose.short_effect, move_effect_prose.effect,
				--move_flag_prose.name as flag_name, move_flag_prose.description as flag_description,
				move_targets.identifier as target_identifier,
				move_target_prose.name as target_name, move_target_prose.description as target_description,
				move_names.name,
				move_flavor_text.flavor_text
				from moves
				left join (
					select move_id, group_concat(DISTINCT move_flag_id) as move_flag_group
					from move_flag_map 
					group by move_id
				) as move_flag_map_view on move_flag_map_view.move_id = moves.id 
				--left join move_flags on move_flags.id = moves.id 
				left join move_meta on move_meta.move_id = moves.id 
				left join contest_effects on moves.contest_effect_id = contest_effects.id
				left join super_contest_effects on moves.super_contest_effect_id = super_contest_effects.id
				left join move_targets on move_targets.id = moves.target_id
				left join move_target_prose on move_target_prose.move_target_id=move_targets.id AND move_target_prose.local_language_id=9
				left join move_effect_prose on move_effect_prose.move_effect_id = moves.effect_id AND move_effect_prose.local_language_id=9
				left join move_names on move_names.move_id = moves.id AND move_names.local_language_id=9
				left join move_flavor_text on move_flavor_text.move_id = moves.id AND move_flavor_text.version_group_id=18 AND move_flavor_text.language_id=9
				order by moves.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//MoveData.Add(Moves.NONE, new Attack.Data.MoveData());
					while(reader.Read()) //if(reader.Read())
					{
						string[] f = ((string)reader["move_flag_group"].ToString()).Split(',');
						MoveData.Add((Moves)int.Parse((string)reader["id"].ToString()),
							new MoveData(
								id: (Moves)int.Parse((string)reader["id"].ToString())
								,category: (Category)int.Parse((string)reader["damage_class_id"].ToString())
								,generationId: int.Parse((string)reader["generation_id"].ToString())
								,accuracy: string.IsNullOrEmpty((string)reader["accuracy"].ToString()) ? (int?)null : int.Parse((string)reader["accuracy"].ToString())
								,baseDamage: string.IsNullOrEmpty((string)reader["power"].ToString()) ? (int?)null : int.Parse((string)reader["power"].ToString())
								,pp: string.IsNullOrEmpty((string)reader["pp"].ToString()) ? (byte)0 : (byte)int.Parse((string)reader["pp"].ToString())
								,priority: int.Parse((string)reader["priority"].ToString())
								,flags: (Flag)new Flag(
									authentic: f.Contains("14")
									,bite: f.Contains("16")
									,bullet: f.Contains("18")
									,charge: f.Contains("2")
									,contact: f.Contains("1")
									//,crit: f.Contains("1")
									,dance: f.Contains("21")
									,defrost: f.Contains("11")
									,distance: f.Contains("12")
									//,flinch: f.Contains("1")
									,gravity: f.Contains("10")
									,heal: f.Contains("13")
									,mirror: f.Contains("7")
									,mental: f.Contains("19")
									,nonsky: f.Contains("20")
									,powder: f.Contains("15")
									,protect: f.Contains("4")
									,pulse: f.Contains("17")
									,punch: f.Contains("8")
									,recharge: f.Contains("3")
									,reflectable: f.Contains("5")
									,snatch: f.Contains("6")
									,sound: f.Contains("9"))
								,target: (Targets)int.Parse((string)reader["target_id"].ToString())
								,type: (Types)int.Parse((string)reader["type_id"].ToString())
								,contestType: string.IsNullOrEmpty((string)reader["contest_type_id"].ToString()) ? (Contests?)null : (Contests)int.Parse((string)reader["contest_type_id"].ToString())
								//,function: int.Parse((string)reader["decreased_stat_id"].ToString())
								//,functionAsString: int.Parse((string)reader["decreased_stat_id"].ToString())
								,effects: (Effects)int.Parse((string)reader["effect_id"].ToString())
								,chance: string.IsNullOrEmpty((string)reader["effect_chance"].ToString()) ? (int?)null : int.Parse((string)reader["effect_chance"].ToString())
								//,description: int.Parse((string)reader["decreased_stat_id"].ToString())
								//ToDo:,contestEffects: string.IsNullOrEmpty((string)reader["contest_effect_id"].ToString()) ? (int?)null : (ContestEffects)int.Parse((string)reader["contest_effect_id"].ToString())
								//ToDo:,superContestEffects: string.IsNullOrEmpty((string)reader["super_contest_effect_id"].ToString()) ? (int?)null : (SuperContestEffects)int.Parse((string)reader["super_contest_effect_id"].ToString())
								,appeal: string.IsNullOrEmpty((string)reader["appeal"].ToString()) ? (int?)null : int.Parse((string)reader["appeal"].ToString())
								,superAppeal: string.IsNullOrEmpty((string)reader["super_appeal"].ToString()) ? (int?)null : int.Parse((string)reader["super_appeal"].ToString())
								,jamming: string.IsNullOrEmpty((string)reader["jam"].ToString()) ? (int?)null : int.Parse((string)reader["jam"].ToString())
							)
						);
						if(!string.IsNullOrEmpty((string)reader["meta_category_id"].ToString()))
							MoveMetaData.Add((Moves)int.Parse((string)reader["id"].ToString()),
								new MetaData(
									move: (Moves)int.Parse((string)reader["id"].ToString())
									,category: (MetaCategory)int.Parse((string)reader["meta_category_id"].ToString())
									,ailment: (MoveMetaAilments)int.Parse((string)reader["meta_ailment_id"].ToString())
									,minHits:  string.IsNullOrEmpty((string)reader["min_hits"].ToString()) ? (int?)null : int.Parse((string)reader["min_hits"].ToString())
									,maxHits:  string.IsNullOrEmpty((string)reader["max_hits"].ToString()) ? (int?)null : int.Parse((string)reader["max_hits"].ToString())
									,minTurns: string.IsNullOrEmpty((string)reader["min_turns"].ToString()) ? (int?)null : int.Parse((string)reader["min_turns"].ToString())
									,maxTurns: string.IsNullOrEmpty((string)reader["max_turns"].ToString()) ? (int?)null : int.Parse((string)reader["max_turns"].ToString())
									,drain: int.Parse((string)reader["drain"].ToString())
									,healing: int.Parse((string)reader["healing"].ToString())
									,critRate: int.Parse((string)reader["crit_rate"].ToString())
									,ailmentChance: int.Parse((string)reader["ailment_chance"].ToString())
									,flinchChance: int.Parse((string)reader["flinch_chance"].ToString())
								)
							);
						//else
						//	MoveMetaData.Add((Moves)int.Parse((string)reader["id"].ToString()),
						//		new MetaData(
						//			move: (Moves)int.Parse((string)reader["id"].ToString())
						//			//,category: MetaCategory.DAMAGE
						//			//,ailment: MoveMetaAilments.NONE
						//			//,minHits:  (int?)null
						//			//,maxHits:  (int?)null
						//			//,minTurns: (int?)null
						//			//,maxTurns: (int?)null
						//			//,drain: 0
						//			//,healing: 0
						//			//,critRate: 0
						//			//,ailmentChance: 0
						//			//,flinchChance: 0
						//		)
						//	);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetItemsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from item_views";
					@"select items.id, items.identifier, items.category_id, items.cost, items.fling_power, items.fling_effect_id,
				item_flag_map_view.item_flag_group,
				item_game_indices_view.generation_group, item_game_indices_view.game_index_group,
				item_prose.short_effect, item_prose.effect,
				item_categories.pocket_id, 
				item_pockets.identifier as item_pocket_identifier,
				item_names.name,
				item_flavor_text.flavor_text
				from items
				left join(
					select item_id, group_concat(DISTINCT item_flag_id) as item_flag_group
					from item_flag_map
					group by item_id
					) as item_flag_map_view on item_flag_map_view.item_id = items.id
				left join(
					select item_id, group_concat(DISTINCT generation_id) as generation_group, group_concat(DISTINCT game_index) as game_index_group
					from item_game_indices
					group by item_id
					) as item_game_indices_view on item_game_indices_view.item_id = items.id
				left join item_categories on item_categories.id = items.category_id
				left join item_pockets on item_pockets.id = item_categories.pocket_id
				left join item_prose on item_prose.item_id = items.id AND item_prose.local_language_id = 9
				left join item_fling_effect_prose on item_fling_effect_prose.item_fling_effect_id = items.fling_effect_id AND item_fling_effect_prose.local_language_id = 9
				left join item_names on item_names.item_id = items.id AND item_names.local_language_id = 9
				left join item_flavor_text on item_flavor_text.item_id = items.id AND item_flavor_text.version_group_id = 18 AND item_flavor_text.language_id = 9
				order by items.id ASC;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//ItemData.Add(Items.NONE, new ItemData());
					while(reader.Read()) //if(reader.Read())
					{
						string[] f = ((string)reader["item_flag_group"].ToString()).Split(',');
						ItemData.Add((Items)int.Parse((string)reader["id"].ToString()),
							new ItemData(
								itemId: (Items)int.Parse((string)reader["id"].ToString())
								,price: int.Parse((string)reader["cost"].ToString())
								,itemCategory: (ItemCategory)int.Parse((string)reader["category_id"].ToString())
								,itemFlag: (ItemFlag)new ItemFlag(
									countable: f.Contains("1")
									,consumable: f.Contains("2")
									,useableOverworld: f.Contains("3")
									,useableBattle: f.Contains("4")
									,holdable: f.Contains("5")
									,holdablePassive: f.Contains("6")
									,holdableActive: f.Contains("7")
									,underground: f.Contains("8"))
								,power: string.IsNullOrEmpty((string)reader["fling_power"].ToString()) ? (int?)null : int.Parse((string)reader["fling_power"].ToString())
								,itemFlingEffect: string.IsNullOrEmpty((string)reader["fling_effect_id"].ToString()) ? ItemFlingEffect.NONE : (ItemFlingEffect)int.Parse((string)reader["fling_effect_id"].ToString())
								//,pocket: (Flavours)int.Parse((string)reader["pocket_id"].ToString())
								,gens: reader["generation_group"].ToString().Split(',').Select(x => int.Parse(x)).ToArray()
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetBerriesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from berry_views order by game_index ASC";
					@"select b.id, b.item_id, b.firmness_id, b.natural_gift_power, b.natural_gift_type_id, b.size, b.max_harvest, b.growth_time, b.soil_dryness, b.smoothness
				,i.cool, i.beauty, i.cute, i.smart, i.tough
				from berries as b
				left join (select 
					b.id as berry_id
					,CAST(AVG(CASE WHEN i.contest_type_id = 1 THEN i.flavor
					END) as int) as cool
					,CAST(AVG(CASE WHEN i.contest_type_id = 2 THEN i.flavor
					END) as int) as beauty
					,CAST(AVG(CASE WHEN i.contest_type_id = 3 THEN i.flavor
					END) as int) as cute
					,CAST(AVG(CASE WHEN i.contest_type_id = 4 THEN i.flavor
					END) as int) as smart
					,CAST(AVG(CASE WHEN i.contest_type_id = 5 THEN i.flavor
					END) as int) as tough
					from berries as b
					left join berry_flavors as i on b.id = i.berry_id
					group by b.id
				) as i on b.id = i.berry_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					//NatureData.Add(Natures.UNSET, new Nature());
					while(reader.Read()) //if(reader.Read())
					{
						BerryData.Add((Items)(int.Parse((string)reader["item_id"].ToString())-1),
							new Berry(
								berry: (Items)int.Parse((string)reader["item_id"].ToString())
								,firmness: (FirmnessLevel)int.Parse((string)reader["firmness_id"].ToString())
								,power: int.Parse((string)reader["natural_gift_power"].ToString())
								,type: (Types)int.Parse(string.IsNullOrEmpty((string)reader["natural_gift_type_id"].ToString()) ? "0" : (string)reader["natural_gift_type_id"].ToString())
								,size: int.Parse((string)reader["size"].ToString())
								,max: int.Parse((string)reader["max_harvest"].ToString())
								,growth: int.Parse((string)reader["growth_time"].ToString())
								,soil: int.Parse((string)reader["soil_dryness"].ToString())
								,smooth: int.Parse((string)reader["smoothness"].ToString())
								,cool: int.Parse((string)reader["cool"].ToString())
								,beauty: int.Parse((string)reader["beauty"].ToString())
								,cute: int.Parse((string)reader["cute"].ToString())
								,smart: int.Parse((string)reader["smart"].ToString())
								,tough: int.Parse((string)reader["tough"].ToString())
							)
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetRegionsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select l.id, l.region_id, l.identifier,
				group_concat(DISTINCT g.generation_id) as generation_group, --g.generation_id, 
				n.name, n.subtitle,
				group_concat(DISTINCT v.version_group_id) as version_group
				from locations as l
				left join location_game_indices as g on l.id = g.location_id
				left join location_names as n on l.id = n.location_id AND n.local_language_id = 9
				left join version_group_regions as v on l.region_id = v.region_id
				group by l.id; --having count(*) > 1;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Regions, List<Locations>> r = new Dictionary<Regions, List<Locations>>();//ToDo: List<Locations,int[]>(Locations.Id,version_group.ToArray())
					r.Add(Regions.NOT_IN_OVERWORLD, new List<Locations>());
					r[Regions.NOT_IN_OVERWORLD].Add(Locations.NOT_IN_OVERWORLD);
					while (reader.Read()) //if(reader.Read())
					{
						if (!string.IsNullOrEmpty(reader["region_id"].ToString()) && !r.ContainsKey((Regions)int.Parse((string)reader["region_id"].ToString())))
							r.Add((Regions)int.Parse((string)reader["region_id"].ToString()), new List<Locations>());
						r[string.IsNullOrEmpty(reader["region_id"].ToString()) ? Regions.NOT_IN_OVERWORLD : (Regions)int.Parse((string)reader["region_id"].ToString())]
							.Add((Locations)int.Parse((string)reader["id"].ToString()));
						//RegionData.Add((Regions)int.Parse((string)reader["region_id"].ToString()),
						//	r.ToArray()
						//	//new Location(
						//	//	location: (Locations)int.Parse((string)reader["id"].ToString())
						//	//	//,region: (Regions)int.Parse((string)reader["region_id"].ToString())
						//	//	//,gen: int.Parse((string)reader["generation_id"].ToString())
						//	//)
						//);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//RegionData.Add(Regions.NOT_IN_OVERWORLD, new Locations[] { Locations.NOT_IN_OVERWORLD });
					foreach (var reg in r)
					{
						//RegionData.Add((Regions)int.Parse((string)reader["region_id"].ToString()), reg.Value.ToArray());
						RegionData.Add((Regions)reg.Key, reg.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetLocationsFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select l.id, l.location_id, l.identifier,
				location_area_encounter_rates_view.location_area_id, location_area_encounter_rates_view.version_group, location_area_encounter_rates_view.encounter_method_id, location_area_encounter_rates_view.rate
				from location_areas as l
				left join(
					select location_area_id, group_concat(DISTINCT version_id) as version_group, encounter_method_id, rate
					from location_area_encounter_rates
					group by location_area_id, encounter_method_id, rate
				) location_area_encounter_rates_view on l.id = location_area_encounter_rates_view.location_area_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Locations, List<int>> l = new Dictionary<Locations, List<int>>();
					Dictionary<int, List<KeyValuePair<Method, int>>> a = new Dictionary<int, List<KeyValuePair<Method, int>>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!l.ContainsKey((Locations)int.Parse((string)reader["location_id"].ToString())))
							l.Add((Locations)int.Parse((string)reader["location_id"].ToString()), new List<int>());
						if (!l[(Locations)int.Parse((string)reader["location_id"].ToString())]
							.Contains(int.Parse((string)reader["id"].ToString())))
							l[(Locations)int.Parse((string)reader["location_id"].ToString())]
								.Add(int.Parse((string)reader["id"].ToString()));
						//AreaData.Add(int.Parse((string)reader["location_area_id"].ToString()),
						//	new Area(
						//		id: int.Parse((string)reader["location_area_id"].ToString())
						//		,method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
						//		,rate: int.Parse((string)reader["rate"].ToString())
						//	)
						//);
						if (!string.IsNullOrEmpty(reader["location_area_id"].ToString()))
						{
							if (!a.ContainsKey(int.Parse((string)reader["location_area_id"].ToString())))
								a.Add(int.Parse((string)reader["location_area_id"].ToString()), new List<KeyValuePair<Method, int>>());
							a[int.Parse((string)reader["location_area_id"].ToString())]
								.Add(new KeyValuePair<Method, int>(//ToDo: int[] { rate, version_group }
								//id: int.Parse((string)reader["location_area_id"].ToString())
								key: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								,value: int.Parse((string)reader["rate"].ToString())
							));
						}
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					LocationData.Add(Locations.NOT_IN_OVERWORLD, new List<int>() { }.ToArray());
					foreach (var loc in l)
					{
						//LocationData.Add((Locations)int.Parse((string)reader["location_id"].ToString()), loc.Value.ToArray());
						LocationData.Add((Locations)loc.Key, loc.Value.ToArray());
					}
					foreach (var area in a)
					{
						AreaData.Add(area.Key,//int.Parse((string)reader["location_area_id"].ToString()),
							new Area(
								id: area.Key
								//,rate: a.Where(x => x.Key == area.Key).Select<KeyValuePair<Method, int>>(n => n.Value).ToArray()
								,rate: a.Single(x => x.Key == area.Key).Value.ToArray()
							)
						); ;
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetEncountersFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = string.Format(//"select * from natures order by game_index ASC";
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
				left join encounter_slots as s on s.id = e.encounter_slot_id
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
				where e.location_area_id = {0}
				group by e.location_area_id, s.encounter_method_id, s.slot, s.rarity; --e.pokemon_id;", Area);
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Method, List<int>> e = new Dictionary<Method, List<int>>();
					while (reader.Read()) //if(reader.Read())
					{
						if (!e.ContainsKey((Method)int.Parse((string)reader["encounter_method_id"].ToString())))
							e.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), new List<int>());
						e[(Method)int.Parse((string)reader["encounter_method_id"].ToString())]
							.Add(int.Parse((string)reader["id"].ToString())					
						);
						EncounterData.Add(int.Parse((string)reader["id"].ToString()),
							new Encounter(
								id: int.Parse((string)reader["id"].ToString())
								,area: int.Parse((string)reader["location_area_id"].ToString())
								,method: (Method)int.Parse((string)reader["encounter_method_id"].ToString())
								,slotId: int.Parse((string)reader["slot"].ToString())
								//,pokemon: (Pokemons)int.Parse((string)reader["pokemon_id"].ToString())
								,pokemon: reader["pokemon_group"].ToString().Split(',').Select(x => (Pokemons)int.Parse(x)).ToArray()
								, conditions: reader["encounter_condition_value_group"].ToString().Split(',').Select(x => (ConditionValue)int.Parse(x)).ToArray()
								,generation: int.Parse((string)reader["generation_id"].ToString())
								,minLevel: int.Parse((string)reader["min_level"].ToString())
								,maxLevel: int.Parse((string)reader["max_level"].ToString())
								,rarity: int.Parse((string)reader["rarity"].ToString())
								,versions: reader["encounter_slot_version_group"].ToString().Split(',').Select(x => (Versions)int.Parse(x)).ToArray()
							)						
						);
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
					//EncounterData.Add(Method.NONE, new List<Encounter>() { }.ToArray());
					foreach (var en in e)
					{
						MethodData.Add((Method)int.Parse((string)reader["encounter_method_id"].ToString()), en.Value.ToArray());
					}
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetMachinesFromSQL(SQLiteConnection con)
		{
			try
			{
				//Step 3: Running a Command
				SQLiteCommand stmt = con.CreateCommand();

				#region DataReader
				stmt.CommandText = //"select * from natures order by game_index ASC";
					@"select row_number() over (order by m.machine_number) as id, m.machine_number as tm_num, m.item_id, group_concat(DISTINCT m.move_id) as move_group, group_concat(DISTINCT m.version_group_id) as version_group,
				g.identifier, g.type_id
				--n.name, n.subtitle
				from machines as m
				left join moves as g on g.id = m.move_id
				--left join location_names as n on l.id = n.location_id AND n.local_language_id = 9; 
				group by m.item_id, m.move_id;";
				SQLiteDataReader reader = stmt.ExecuteReader();

				//Step 4: Read the results
				using(reader)
				{
					Dictionary<Method, List<int>> e = new Dictionary<Method, List<int>>();
					while (reader.Read()) //if(reader.Read())
					{
					}
					//Step 5: Closing up
					reader.Close();
					reader.Dispose();
					#endregion
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		static bool GetTrainersFromSQL(SQLiteConnection con)
		{
			try
			{
				foreach (TrainerTypes type in Enum.GetValues(typeof(TrainerTypes)))
				{
					int BaseMoney = 30;
					int SkillLevel = 1;
					bool? gender = null;
					bool doubel = false;
					#region Gender & IsDouble
					switch (type)
					{
						case TrainerTypes.POKEMONTRAINER_Red:
						case TrainerTypes.POKEMONTRAINER_Leaf:
						case TrainerTypes.POKEMONTRAINER_Brendan:
						case TrainerTypes.RIVAL1:
						case TrainerTypes.RIVAL2:
						case TrainerTypes.BIKER:
						case TrainerTypes.BIRDKEEPER:
						case TrainerTypes.BUGCATCHER:
						case TrainerTypes.BURGLAR:
						case TrainerTypes.CHANELLER:
						case TrainerTypes.CUEBALL:
						case TrainerTypes.ENGINEER:
						case TrainerTypes.FISHERMAN:
						case TrainerTypes.GAMBLER:
						case TrainerTypes.GENTLEMAN:
						case TrainerTypes.HIKER:
						case TrainerTypes.JUGGLER:
						case TrainerTypes.PAINTER:
						case TrainerTypes.POKEMANIAC:
						case TrainerTypes.POKEMONBREEDER:
						case TrainerTypes.PROFESSOR:
						case TrainerTypes.ROCKER:
						case TrainerTypes.RUINMANIAC:
						case TrainerTypes.SAILOR:
						case TrainerTypes.SCIENTIST:
						case TrainerTypes.SUPERNERD:
						case TrainerTypes.TAMER:
						case TrainerTypes.BLACKBELT:
						case TrainerTypes.CAMPER:
						case TrainerTypes.PICNICKER:
						case TrainerTypes.COOLTRAINER_M:
						case TrainerTypes.YOUNGSTER:
						case TrainerTypes.POKEMONRANGER_M:
						case TrainerTypes.PSYCHIC_M:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.CRUSHKIN:
						case TrainerTypes.TEAMROCKET_M:
						case TrainerTypes.ROCKETBOSS:
						case TrainerTypes.LEADER_Brock:
						case TrainerTypes.LEADER_Surge:
						case TrainerTypes.LEADER_Koga:
						case TrainerTypes.LEADER_Blaine:
						case TrainerTypes.LEADER_Giovanni:
						case TrainerTypes.ELITEFOUR_Bruno:
						case TrainerTypes.ELITEFOUR_Lance:
						case TrainerTypes.CHAMPION:
							gender = true;
							break;
						case TrainerTypes.POKEMONTRAINER_May:
						case TrainerTypes.AROMALADY:
						case TrainerTypes.BEAUTY:
						case TrainerTypes.LADY:
						case TrainerTypes.CRUSHGIRL:
						case TrainerTypes.COOLTRAINER_F:
						case TrainerTypes.LASS:
						case TrainerTypes.POKEMONRANGER_F:
						case TrainerTypes.PSYCHIC_F:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_F:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
						case TrainerTypes.TEAMROCKET_F:
						case TrainerTypes.LEADER_Misty:
						case TrainerTypes.LEADER_Erika:
						case TrainerTypes.LEADER_Sabrina:
						case TrainerTypes.ELITEFOUR_Lorelei:
						case TrainerTypes.ELITEFOUR_Agatha:
							gender = false;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.TWINS:
						case TrainerTypes.YOUNGCOUPLE:
							doubel = true;
							break;
						case TrainerTypes.WildPokemon:
						case TrainerTypes.PLAYER:
						default:
							break;
					}
					switch (type)
					{
						case TrainerTypes.BURGLAR:
						case TrainerTypes.GAMBLER:
							SkillLevel = 32;
							break;
						case TrainerTypes.LADY:
							SkillLevel = 72;
							break;
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER2_M:
							SkillLevel = 32;
							break;
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_F:
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.YOUNGCOUPLE:
							break;
						default:
							break;
					}
					#endregion
					#region Base Money
					switch (type)
					{
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_F:
							BaseMoney = 4;
							break;
						case TrainerTypes.RIVAL1:
						case TrainerTypes.BUGCATCHER:
						case TrainerTypes.PAINTER:
						case TrainerTypes.CAMPER:
						case TrainerTypes.LASS:
						case TrainerTypes.PICNICKER:
						case TrainerTypes.SISANDBRO:
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.SWIMMER2_F:
						case TrainerTypes.YOUNGSTER:
							BaseMoney = 16;
							break;
						case TrainerTypes.CUEBALL:
						case TrainerTypes.CRUSHGIRL:
						case TrainerTypes.ROCKER:
						case TrainerTypes.TWINS:
							BaseMoney = 24;
							break;
						case TrainerTypes.AROMALADY:
						case TrainerTypes.BIKER:
						case TrainerTypes.BIRDKEEPER:
						case TrainerTypes.BLACKBELT:
						case TrainerTypes.CHANELLER:
						case TrainerTypes.FISHERMAN:
						case TrainerTypes.HIKER:
						case TrainerTypes.JUGGLER:
						case TrainerTypes.PSYCHIC_M:
						case TrainerTypes.PSYCHIC_F:
						case TrainerTypes.SAILOR:
						case TrainerTypes.TEAMROCKET_M:
						case TrainerTypes.TEAMROCKET_F:
							BaseMoney = 32;
							break;
						case TrainerTypes.RIVAL2:
							BaseMoney = 36;
							break;
						case TrainerTypes.CRUSHKIN:
						case TrainerTypes.TAMER:
							BaseMoney = 48;
							break;
						case TrainerTypes.ENGINEER:
						case TrainerTypes.POKEMONBREEDER:
						case TrainerTypes.RUINMANIAC:
						case TrainerTypes.SCIENTIST:
						case TrainerTypes.SUPERNERD:
							BaseMoney = 48;
							break;
						case TrainerTypes.BEAUTY:
							BaseMoney = 56;
							break;
						case TrainerTypes.PLAYER:
						case TrainerTypes.POKEMONTRAINER_Red:
						case TrainerTypes.POKEMONTRAINER_Brendan:
						case TrainerTypes.POKEMONTRAINER_Leaf:
						case TrainerTypes.POKEMONTRAINER_May:
						case TrainerTypes.COOLTRAINER_M:
						case TrainerTypes.COOLTRAINER_F:
						case TrainerTypes.POKEMONRANGER_M:
						case TrainerTypes.POKEMONRANGER_F:
						case TrainerTypes.YOUNGCOUPLE:
							BaseMoney = 60;
							break;
						case TrainerTypes.POKEMANIAC:
							BaseMoney = 64;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.GAMBLER:
						case TrainerTypes.GENTLEMAN:
							BaseMoney = 72;
							break;
						case TrainerTypes.BURGLAR:
							BaseMoney = 88;
							break;
						case TrainerTypes.CHAMPION:
						case TrainerTypes.LEADER_Brock:
						case TrainerTypes.LEADER_Surge:
						case TrainerTypes.LEADER_Koga:
						case TrainerTypes.LEADER_Blaine:
						case TrainerTypes.LEADER_Giovanni:
						case TrainerTypes.LEADER_Misty:
						case TrainerTypes.LEADER_Erika:
						case TrainerTypes.LEADER_Sabrina:
						case TrainerTypes.ELITEFOUR_Lorelei:
						case TrainerTypes.ELITEFOUR_Agatha:
						case TrainerTypes.ELITEFOUR_Bruno:
						case TrainerTypes.ELITEFOUR_Lance:
						case TrainerTypes.ROCKETBOSS:
						case TrainerTypes.PROFESSOR:
							BaseMoney = 100;
							break;
						case TrainerTypes.LADY:
							BaseMoney = 160;
							break;
						default:
							BaseMoney = 30;
							break;
					}
					#endregion
					#region Skill Level
					switch (type)
					{
						case TrainerTypes.BURGLAR:
						case TrainerTypes.GAMBLER:
							SkillLevel = 32;
							break;
						case TrainerTypes.LADY:
							SkillLevel = 72;
							break;
						case TrainerTypes.SWIMMER_M:
						case TrainerTypes.SWIMMER_F:
						case TrainerTypes.SWIMMER2_M:
						case TrainerTypes.SWIMMER2_F:
							SkillLevel = 32;
							break;
						case TrainerTypes.TUBER_M:
						case TrainerTypes.TUBER_F:
						case TrainerTypes.TUBER2_M:
						case TrainerTypes.TUBER2_F:
							SkillLevel = 16;
							break;
						case TrainerTypes.COOLCOUPLE:
						case TrainerTypes.SISANDBRO:
							SkillLevel = 48;
							break;
						case TrainerTypes.YOUNGCOUPLE:
							SkillLevel = 32;
							break;
						default:
							SkillLevel = BaseMoney;
							break;
					}
					#endregion
					TrainerMetaData.Add(type, new TrainerData());
				}
				return true;
			} catch (SQLiteException e) {
				//Debug.Log("SQL Exception Message:" + e.Message);
				//Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
				//Debug.Log("SQL Exception Help:" + e.HelpLink);
				return false;
			}
		}
		#endregion
	}
}