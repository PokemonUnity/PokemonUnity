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
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonMoveTree> PokemonMovesData { get; private set; }
		public static Dictionary<Pokemons, PokemonUnity.Monster.Data.PokemonData> PokemonData { get; private set; }
		//public static Dictionary<Moves,Move.MoveData> MoveData { get; private set; }
		//public static Dictionary<Items,Item> ItemData { get; private set; }
		//public static Dictionary<Natures,Nature> NatureData { get; private set; }
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
						//--pokemon_species.base_happiness, pokemon_species.capture_rate, pokemon_species.gender_rate, pokemon_species.hatch_counter, pokemon_species.shape_id, pokemon_species.growth_rate_id,
						//pokemon_species.generation_id, pokemon_species.evolves_from_species_id, pokemon_species.evolution_chain_id, pokemon_species.color_id, pokemon_species.shape_id, pokemon_species.habitat_id, pokemon_species.gender_rate, pokemon_species.capture_rate, pokemon_species.base_happiness, pokemon_species.is_baby, pokemon_species.hatch_counter, pokemon_species.has_gender_differences, pokemon_species.growth_rate_id, pokemon_species.forms_switchable, pokemon_species.""order"",
						//pokemon_species_names.name,pokemon_species_names.genus,
						//pokemon_species_flavor_text.flavor_text
						//from pokemon
						//left join pokemon_abilities_view on pokemon.id = pokemon_abilities_view.pokemon_id
						//left join pokemon_egg_groups_view on pokemon_egg_groups_view.pokemon_id = pokemon.id
						//left join pokemon_stats_view on pokemon_stats_view.pokemon_id = pokemon.id
						//left join pokemon_types_view on pokemon_types_view.pokemon_id = pokemon.id
						//left join pokemon_species on pokemon_species.id = pokemon.id
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
	}
}