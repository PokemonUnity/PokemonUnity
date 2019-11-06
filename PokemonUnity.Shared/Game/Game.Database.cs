using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Attack;
using PokemonUnity.Saving;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
//using System.Data.SqlClient;
using System.IO;
using PokemonUnity.Inventory.Berry;
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
		const string FilePokemonXML = "";
		//ToDo: ReadOnly Immutable Dictionaries...
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonEvolution[]> PokemonEvolutionsData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; private set; }
		public static Dictionary<Natures,Nature> NatureData { get; private set; }
		//public static Dictionary<Moves,Move.MoveData> MoveData { get; private set; }
		//public static Dictionary<Items,Item> ItemData { get; private set; }
		//public static Dictionary<Berries,Item.Berry> BerryData { get; private set; }
		//ability
		//forms
		//held items
		//evolutions
		//locations
		//location encounters

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
		public static bool InitNatures(bool sql = true)
		{
			NatureData = new Dictionary<Natures, Nature>();
			if (sql) //using (con)
				return GetNaturesFromSQL(con);
			else return GetPokemonsFromXML();
		}

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
					//left join pokemon_species on pokemon_species.id = pokemon.id
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
	}
}