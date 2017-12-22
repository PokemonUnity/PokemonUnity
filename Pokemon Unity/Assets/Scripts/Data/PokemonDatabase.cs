//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using Assets.Scripts.ImportedScripts;//.ColorExtensions;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using Mono.Data.Sqlite;
//using System.Data.SQLite;
using System.IO;
using System;

/// <summary>
/// all of the values are hard coded
/// first index value is null
/// returns [0,1,2...] as order in pokedex
/// </summary>
public static class PokemonDatabase
{
	/// <summary>
	/// Supports multiple languages
	/// </summary>
	private static PokemonData[] pokedex(int language = 9) {//Default language is English
		Debug.Log("Fetch Pokedex");
		List<PokemonData> pokedexArray = new List<PokemonData>();
		pokedexArray.Add(null); //pokedex index 0 is null value

<<<<<<< HEAD
		//might need to make a new enum in PokemonData, type = x.Color...
		Dictionary<string,Color> StringToColor = new Dictionary<string,Color>() {
			{"Black",Color.black },//dark
			{"Blue",Color.blue },//water
			{"Clear",Color.clear },
			{"Cyan",Color.cyan },
			{"Gray",Color.gray },
			{"Green",Color.green },//grass
			{"Grey",Color.grey },
			{"Magenta",Color.magenta },
			{"Red",Color.red },//fire
			{"White",Color.white },//normals
			{"Yellow",Color.yellow },//electric
			{"Purple", new Color() },//ghost
			{"Brown", new Color() },//fighting
			{"Pink", new Color() }//,//fairy
			//{"", new Color() },//fly, drag, steel, psychic, ice, shadow, unknown, bug, ground, poison?
			//{"", new Color() },
			//{"", new Color() },
			//{"", new Color() },
			//{"", new Color() },
			//{"", new Color() }
		};

		try
		{
			//Step 1: Get Provider
			//DbProviderFactory f = SQLiteFactory.Instance;
			//Step 2: Create a connection
			Debug.Log("Creating Connection to Pokemon DB");
			Debug.Log(Application.dataPath);
			//IDbConnection con;// = f.CreateConnection();
			//SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
			//builder.FailIfMissing = true;
			//builder.Version = 3;
			//builder.LegacyFormat = true;
			//builder.Pooling = true;
			string ConnectionString = "URI=file:" //"Data Source="//"Driver=SQLite3 ODBC Driver; Database="
			//builder.Uri = "file:"
			//builder.DataSource = "Data Source="
			//builder.ConnectionString = "Driver=SQLite3 ODBC Driver; Database="
				//+ System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase),"veekun-pokedex.sqlite");
				//+ @"C:\Users\Username\Documents\Pokemon\Pokemon Unity\"//Assets\Resources\Databases"
				+ Application.dataPath + Path.DirectorySeparatorChar
				//+ "Assets" + Path.DirectorySeparatorChar//Standard
				//+ "Scripts" + Path.DirectorySeparatorChar
				+ "Resources" + Path.DirectorySeparatorChar
				+ "Databases" + Path.DirectorySeparatorChar
				+ "veekun-pokedex.sqlite;Version=3;";// "Data Source=C:\\folder\
				//+ "new_pokedex.db;Version=3;";// "Data Source=C:\\folder\\HelloWorld.sqlite;Version=3;New=False;Compress=True";
			SqliteConnection con = new SqliteConnection(ConnectionString);//ConnectionString
			Debug.Log("Opening Pokemon DB");
			con.Open();
			Debug.Log("Opened Pokemon DB");

			//int n = 1;
			//int pokedexCount = 1000;
			//bool isNull = false;
			string sqlQuery;
			using(con)
			{
				#region Variables
				int ID = 0;
				string NAME = "";
				PokemonData.Type TYPE1 = PokemonData.Type.NONE;
				PokemonData.Type TYPE2 = PokemonData.Type.NONE;
				int? Ability1 = null;
				int? Ability2 = null;
				int? HiddenAbility = null;
				float maleRatio = 0;
				int catchRate = 0;
				PokemonData.EggGroup eggGroup1 = PokemonData.EggGroup.NONE;
				PokemonData.EggGroup eggGroup2 = PokemonData.EggGroup.NONE;
				int hatchTime = 0;
				float height = 0;
				float weight = 0;
				int baseExpYield = 0;
				PokemonData.LevelingRate levelingRate = 0;
				int evYieldHP = 0;
				int evYieldATK = 0;
				int evYieldDEF = 0;
				int evYieldSPA = 0;
				int evYieldSPD = 0;
				int evYieldSPE = 0;
				PokemonData.PokedexColor pokedexColor = PokemonData.PokedexColor.WHITE;
				int baseFriendship = 0;

				int species = 0;
				string pokedexEntry = "";

				int baseStatsHP = 0;
				int baseStatsATK = 0;
				int baseStatsDEF = 0;
				int baseStatsSPA = 0;
				int baseStatsSPD = 0;
				int baseStatsSPE = 0;

				float luminance = 0;
				Color lightColor = Color.clear; 
				int[][] heldItem;
				int[] movesetLevels;
				int[] movesetMoves;
				int[] tmList;

				int[] evolutionID;
				string[] evolutionRequirements;
				//				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
				//				Species, PokedexEntry (choose your favourite)
				//				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
				//				new int[]{ level, level, level, etc...}
				//				new string[]{ "move", "move", "move", etc...} ),
				//				new int[]{pokemonID}, new string[]{"Method,Parameter"}),
				#endregion
				for(int n=1; n<721;n++)//couldnt figure out a way to automate the loop, so i did it manually ("end at 721"... the db has 800...)
				{ 
				//Step 3: Running a Command
				IDbCommand stmt = con.CreateCommand();

					#region DataReader
					sqlQuery = string.Format("select * from pokemonDB where id={0} order by id ASC",n);//[order] = {0} order by [order] ASC",n);//for the sake of debugging
					stmt.CommandText = sqlQuery;//"select * --pokemon.*,abilities_view.ability1,abilities_view.ability2,abilities_view.hidden, egg_group_view.egg_group1, egg_group_view.egg_group2" +
					//	"from pokemon" +
					//	"join abilities_view on pokemon.id = abilities_view.pokemon_id"+
					//	"join egg_group_view on egg_group_view.species_id = pokemon.id"+
					//	"join pokemon_species on pokemon_species.id = pokemon.id"+
					//	"join pokemon_species_names on pokemon_species_names.pokemon_species_id = pokemon.id AND pokemon_species_names.local_language_id = 9"+
					//	"join pokemon_species_flavor_text on pokemon_species_flavor_text.species_id = pokemon.id AND pokemon_species_flavor_text.version_id = 26 AND pokemon_species_flavor_text.language_id = 9 ";
					//"SELECT * FROM StoredProcedure/View ORDER BY ID ASC";
					Debug.Log("Establishing Reader for Pokemon DB");
					IDataReader reader = stmt.ExecuteReader();
					//if(!((System.Data.Common.DbDataReader)reader).HasRows) { Debug.Log("DB is Empty"); }//break for-loop

					//Step 4: Read the results
					using(reader)
					{
						Debug.Log("Reading Pokemon DB");
						while(reader.Read())
						{
							Debug.Log("Pokemon DB Test Value:" + reader["name"] as string);
							ID = System.Convert.ToInt16(reader["id"]);// is int ? (int)reader["id"] : -1;// as System.UInt16;
							//pokemonOrder = (System.UInt16)reader["order"];// numerical order of pokemon evolution
							NAME = reader["name"] as string;
							TYPE1 = reader["type1"] is DBNull ? PokemonData.Type.NONE : (PokemonData.Type)Convert.ToInt16(reader["type1"]);//if correct this should convert int to enum string 
							TYPE2 = reader["type2"] is DBNull ? PokemonData.Type.NONE : (PokemonData.Type)Convert.ToInt16(reader["type2"]);
							Ability1 = reader["ability1"] is DBNull ? (int?)null : (System.UInt16)Convert.ToInt16(reader["ability1"]);
							Ability2 = reader["ability2"] is DBNull ? (int?)null : (System.Int16)Convert.ToInt16(reader["ability2"]);//if null, needs to be a number... possibly -1 or 0
							HiddenAbility = reader["hidden"] is DBNull ? (int?)null : (System.Int16)Convert.ToInt16(reader["hidden"]);//if null, needs to be a number... possibly -1 or 0
							maleRatio = (float)Convert.ToInt16(reader["gender_rate"]);
							catchRate = (System.UInt16)Convert.ToInt16(reader["capture_rate"]);
							eggGroup1 = reader["egg_group1"] is DBNull ? PokemonData.EggGroup.NONE : (PokemonData.EggGroup)Convert.ToInt16(reader["egg_group1"]);
							eggGroup2 = reader["egg_group1"] is DBNull ? PokemonData.EggGroup.NONE : (PokemonData.EggGroup)Convert.ToInt16(reader["egg_group2"]);
							hatchTime = (System.UInt16)Convert.ToInt16(reader["hatch_counter"]);
							height = (System.Int32)Convert.ToInt32(reader["height"]);
							weight = (System.Int32)Convert.ToInt32(reader["weight"]);
							baseExpYield = (System.UInt16)Convert.ToInt16(reader["base_experience"]);
							levelingRate = (PokemonData.LevelingRate)Convert.ToInt16(reader["growth_rate_id"]);
							//evYieldHP = (System.UInt16)reader["evhp"];
							//evYieldATK = (System.UInt16)reader["evatk"];
							//evYieldDEF = (System.UInt16)reader["evdef"];
							//evYieldSPA = (System.UInt16)reader["evspa"];
							//evYieldSPD = (System.UInt16)reader["evspd"];
							//evYieldSPE = (System.UInt16)reader["evspd"];
							species = (System.UInt16)Convert.ToInt16(reader["species_id"]);//pokedex-id
							pokedexEntry = reader["flavor_text"] as string;
							baseStatsHP = (System.UInt16)Convert.ToInt16(reader["bhp"]);
							baseStatsATK = (System.UInt16)Convert.ToInt16(reader["batk"]);
							baseStatsDEF = (System.UInt16)Convert.ToInt16(reader["bdef"]);
							baseStatsSPA = (System.UInt16)Convert.ToInt16(reader["bspa"]);
							baseStatsSPD = (System.UInt16)Convert.ToInt16(reader["bspd"]);
							baseStatsSPE = (System.UInt16)Convert.ToInt16(reader["bspe"]);
							luminance = 0;//(float)reader["lumi"]; //i dontknow what this is...
							lightColor = !string.IsNullOrEmpty((string)reader["color"]) ? (Color)StringToColor[Convert.ToString(reader["color"])] : Color.clear;
							//pokedexColor = (PokemonData.PokedexColor)Convert.ToInt16(reader["color"]); //lightColor; no db on pokedex-colers per pokemon...
							baseFriendship = (System.UInt16)Convert.ToInt16(reader["base_happiness"]);
							//shape id
						}
					}
					//Step 5: Closing up
					reader.Close();
                    reader.Dispose();
					#endregion

					#region DataReader
					//Step 3: Running a Command
					sqlQuery = string.Format(@"SELECT distinct a.move_id, a.pokemon_id as idA, a.pokemon_move_method_id as methodA, c.pLv FROM pokemon_moves as a 
						join(SELECT distinct[level] as pLv,move_id,pokemon_id as id,pokemon_move_method_id as methodB FROM pokemon_moves as b WHERE ID = {0} AND methodB = 1 ORDER BY[move_id] ASC, [level] ASC) as c 
						on a.move_id = c.move_id AND idA = c.id AND c.id = {1} AND c.methodB = 1 AND methodA = methodB 
						group by a.move_id 
						--ORDER BY c.[pLv] ASC,a.[move_id] ASC",n,n);//(System.UInt16)Convert.ToInt16(reader["id"]));//select distinct move_id, pokemon_id, [level] from pokemon_moves where pokemon_id={0} ORDER BY [move_id] ASC, [level] DESC
					stmt.CommandText = sqlQuery;
					//select * from openquery(veekun, 'select distinct move_id, pokemon_id, [level] from pokemon_moves where pokemon_id=1 ORDER BY pokemon_id ASC, [level] ASC--, [order] ASC')
					Debug.Log("Opening Pokemon MoveArray DB");
					IDataReader reader2 = stmt.ExecuteReader();
					Debug.Log("Opened Pokemon MoveArray DB");

					List<int> lvArray = new List<int>();
					List<int> moveArray = new List<int>();

					//Step 4: Read the results
					using(reader2)
					{
						Debug.Log("Reading Pokemon MoveArrary DB");
						while(reader2.Read())
						{
							Debug.Log("Pokemon Level:"+reader2["pLv"] as string);
							lvArray.Add((System.UInt16)Convert.ToInt16(reader2["pLv"])); //level where pokemon learns move
							Debug.Log("Pokemon Move ID:" + reader2["move_id"] as string);
							moveArray.Add((System.UInt16)Convert.ToInt16(reader2["move_id"])); //move_id
							//strictly added moves that can be learned thru leveling-up.
							//methodArray.Add((System.UInt16)reader["methodA"]); //method on how pokemon learns move (tm, tutor, lvl-up...)
						}
						//reader2.NextResult();
					}
					//Step 5: Closing up
					reader2.Close();
					#endregion

					#region DataReader
					//Step 3: Running a Command
					sqlQuery = string.Format("select t1.*, t1.id as pokemon, t2.id as ParamID from pokemon_species as t1 inner Join pokemon_evolution as t2 on t1.id=t2.evolved_species_id where t1.evolves_from_species_id = {0}",n);//(System.UInt16)Convert.ToInt16(reader["id"]));
					stmt.CommandText = sqlQuery;//"SELECT evolutionID, evoRequirements FROM Table WHERE ID = @0 ORDER BY evolutionID ASC";
											    //select * from openquery(veekun, 'select t1.*, t1.id as pokemon, t2.id as ParamID from pokemon_species as t1 inner Join pokemon_evolution as t2 on t1.id=t2.evolved_species_id where t1.evolves_from_species_id = @')
					Debug.Log("Opening Pokemon EvolutionArray DB");
					IDataReader reader3 = stmt.ExecuteReader();
					Debug.Log("Opened Pokemon EvolutionArray DB");

					List<int> evoIDarray = new List<int>();
					List<string> evoReqArray = new List<string>();

					//Step 4: Read the results
					using(reader3)
					{
						Debug.Log("Reading Pokemon EvArray DB");
						while(reader3.Read())
						{//not all pokemons evolve... wasnt sure how to null a int[], but we'll see on debug
							if(!reader3.IsDBNull(1/*colum index*/))
							{ //if not null, add value, else skip it..
								//null;
								//else { 
								evoIDarray.Add((System.UInt16)Convert.ToInt16(reader3["id"])); //pokemon id
								evoReqArray.Add(reader3["paramid"] as string); //requirements id
							}
						}
					}
					//Step 5: Closing up
					reader3.Close();
					#endregion

					#region DataReader
					//Step 3: Running a Command
					sqlQuery = string.Format("select distinct pokemon_id, item_id, rarity from pokemon_items where pokemon_id = {0}",n);// (System.UInt16)reader["id"]);
					stmt.CommandText = sqlQuery;//
					Debug.Log("Opening Pokemon HeldItemArray DB");
					IDataReader reader5 = stmt.ExecuteReader();
					Debug.Log("Opened Pokemon HeldItemArray DB");

					//List<List<int>> holdArray = new List<List<int>>();
					List<int[]> holdArray = new List<int[]>();// works better with unknown size
					//holdArray.Add(new List<int>());

					//Step 4: Read the results
					using(reader5)
					{
						Debug.Log("Reading Pokemon Item DB");
						while(reader5.Read())
						{//not all pokemons evolve... wasnt sure how to null a int[], but we'll see on debug
							List<int> itemList = new List<int>();
							if(reader5.IsDBNull(1/*colum index*/))
							{ //if not null, add value, else skip it..
								holdArray.Add(new int[2] { -1,100 });//null held item, with value of 100%;;
								//holdArray[0].Add(100);//value of 100%;
							} else
							{
								holdArray.Add(new int[2] { (System.UInt16)Convert.ToInt16(reader5["item_id"]),(System.UInt16)Convert.ToInt16(reader5["rarity"]) }); //pokemon id
																																									//rarityArray.Add((System.UInt16)Convert.ToInt16(reader["rarity"])); //requirements id
							}
						}
					}
					//Step 5: Closing up
					reader5.Close();
					#endregion

					#region DataReader
					//Step 3: Running a Command
					sqlQuery = string.Format("select distinct tm_views.move_id, tm_views.itemNo, pokemon_moves.pokemon_id, pokemon_moves.pokemon_move_method_id " +
						"from tm_views " +
						"join pokemon_moves on tm_views.move_id = pokemon_moves.move_id " +
						"where pokemon_moves.pokemon_move_method_id = 4 AND pokemon_moves.pokemon_id = {0} " +
						"order by pokemon_moves.pokemon_id ASC,tm_views.move_id Asc",n);// (System.UInt16)Convert.ToInt16(reader["id"]));
					stmt.CommandText = sqlQuery;//"SELECT tmID FROM Table WHERE ID = @0 ORDER BY tmID ASC";
					Debug.Log("Opening Pokemon Technical Machine DB");
					IDataReader reader4 = stmt.ExecuteReader();
					Debug.Log("Opened Pokemon TM DB");

					List<int> tmArray = new List<int>();

					//Step 4: Read the results
					using(reader4)
					{
						Debug.Log("Reading Pokemon Machine DB");
						while(reader4.Read())
						{
							tmArray.Add((System.UInt16)Convert.ToInt16(reader4["itemNo"]));
						}
					}
					//Step 5: Closing up
					reader4.Close();
					#endregion

					movesetLevels = new int[lvArray.Count];
					movesetMoves = new int[moveArray.Count];
					evolutionID = new int[evoIDarray.Count];
					evolutionRequirements = new string[evoReqArray.Count];
					tmList = new int[tmArray.Count];
					heldItem = new int[holdArray.Count][];
					lvArray.CopyTo(movesetLevels);
					moveArray.CopyTo(movesetMoves);
					evoIDarray.CopyTo(evolutionID);
					evoReqArray.CopyTo(evolutionRequirements);
					tmArray.CopyTo(tmList);
					holdArray.CopyTo(heldItem);

					pokedexArray.Add(new PokemonData(ID,NAME,TYPE1,TYPE2,Ability1,Ability2,HiddenAbility,
						maleRatio,catchRate,eggGroup1,eggGroup2,hatchTime,height,weight,
						baseExpYield,levelingRate,evYieldHP,evYieldATK,evYieldDEF,evYieldSPA,evYieldSPD,evYieldSPE,pokedexColor,baseFriendship,
						species,pokedexEntry,baseStatsHP,baseStatsATK,baseStatsDEF,baseStatsSPA,baseStatsSPD,baseStatsSPE,luminance,lightColor,
						movesetLevels,movesetMoves,tmList,evolutionID,evolutionRequirements,-1, /*int[] =  null = -1 */heldItem));
					Debug.Log("For Variable: "+n.ToString());
				}
				}
				con.Close();
				Debug.Log("Closed all Pokemon DB Connections");

				PokemonData[] pokedex = new PokemonData[pokedexArray.Count];
				pokedexArray.CopyTo(pokedex);
				//{//null, first index is null, because pokedex starts at 0 not 1
				//  PokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE1, Ability1, Ability2, HiddenAbility,
				//				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
				//				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
				//				Species, PokedexEntry (choose your favourite)
				//				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
				//				new int[]{ level, level, level, etc...}
				//				new string[]{ "move", "move", "move", etc...} ),
				//				new int[]{pokemonID}, new string[]{"Method,Parameter"}),
				//};
				Debug.Log("Returning Pokedex");
				return pokedex;
		} catch (SqliteException e) {
			Debug.Log("SQL Exception Message:" + e.Message);
			Debug.Log("SQL Exception Code:" + e.ErrorCode.ToString());
			Debug.Log("SQL Exception Help:" + e.HelpLink);
			return null;
		}
	}

	#region Debug
	/// <summary>
	/// The Database is incomplete. Many Pokémon have not been programmed in. If you wish to add a Pokémon's data
	/// to the database, ensure that it is placed in the correct position for it's ID.
	/// E.G. Pikachu (025) would go after Arbok (024) or the closest ID below if Arbok isn't there (i.e. Fearow (022))
	/// and before Raichu (026) or the closest ID above if Raichu isn't there (i.e. Nidoran♂ (032))
	/// </summary>
	private static PokemonData[] debug_pokedex = new PokemonData[] {
		null,
		//  PokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE1, Ability1, Ability2, HiddenAbility,
		//				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
		//				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
		//				Species, PokedexEntry (choose your favourite)
		//				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
		//				new int[]{ level, level, level, etc...}
		//				new string[]{ "move", "move", "move", etc...} ),
		//				new int[]{pokemonID}, new string[]{"Method,Parameter"}),
	new PokemonData(1, "Bulbasaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.7f, 6.9f,
		                64, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Seed", "For some time after its birth, it grows by gaining nourishment from the seed on its back.",
		                45,49,49,65,65,45, 0, Color.clear,
		                new int[]{   1,       3,      7,           9,          13,             13,            15,         19,          21,           25,      27,           31,          33,         37},
						new string[]{"Tackle","Growl","Leech Seed","Vine Whip","Poison Powder","Sleep Powder","Take Down","Razor Leaf","Sweet Scent","Growth","Double-Edge","Worry Seed","Synthesis","Seed Bomb"},
						new string[]{},
						new int[]{2}, new string[]{"Level,16"}),
		new PokemonData(2,"Ivysaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1, 13,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,1,0, PokemonData.PokedexColor.GREEN, 70,
		                "Seed", "There is a bud on this Pokémon’s back. To support its weight, Ivysaur’s legs and trunk grow thick and strong. If it starts spending more time lying in the sunlight, it’s a sign that the bud will bloom into a large flower soon.",
		                60,62,63,80,80,60, 0, Color.clear,
		                new int[]{   1,       1,      1,           3,      7,           9,          13,             13,            15,         20,          23,           28,      31,           36,          39,         44},
						new string[]{"Tackle","Growl","Leech Seed","Growl","Leech Seed","Vine Whip","Poison Powder","Sleep Powder","Take Down","Razor Leaf","Sweet Scent","Growth","Double-Edge","Worry Seed","Synthesis","Solar Beam"},
						new string[]{},
						new int[]{3}, new string[]{"Level,32"}),
		new PokemonData(3, "Venusaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5335, 2, 100, 
		                236, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,1,0, PokemonData.PokedexColor.GREEN, 70,
		                "Seed", "There is a large flower on Venusaur’s back. The flower is said to take on vivid colors if it gets plenty of nutrition and sunlight. The flower’s aroma soothes the emotions of people.",
		                80,82,83,100,100,80, 0, Color.clear,
		                new int[]{   1,       1,      1,           1,          3,      7,          9,           13,             13,            15,         20,          23,           28,      31,           32,           39,          45,         50,              53},
						new string[]{"Tackle","Growl","Leech Seed","Vine Whip","Growl","Vine Whip","Leech Seed","Poison Powder","Sleep Powder","Take Down","Razor Leaf","Sweet Scent","Growth","Double-Edge","Petal Dance","Worry Seed","Synthesis","Petal Blizzard","Solar Beam"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(4, "Charmander", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Solar Power",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.6f, 8.5f,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.RED, 70,
		                "Lizard", "The flame on its tail shows the strength of its life force. If it is weak, the flame also burns weakly.",
		                39,52,43,60,50,65, 0, Color.clear,
		                new int[]{   1,        1,      7,      10,           16,           19,          25,         28,           34,     37,            43,         46},
						new string[]{"Scratch","Growl","Ember","Smokescreen","Dragon Rage","Scary Face","Fire Fang","Flame Burst","Slash","Flamethrower","Fire Spin","Inferno"},
						new string[]{},
						new int[]{5}, new string[]{"Level,16"}),
		new PokemonData(5, "Charmeleon", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Solar Power",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.1f, 19,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,1, PokemonData.PokedexColor.RED, 70,
		                "Flame", "Charmeleon mercilessly destroys its foes using its sharp claws. If it encounters a strong foe, it turns aggressive. In this excited state, the flame at the tip of its tail flares with a bluish white color.",
		                58,64,58,80,65,80, 0, Color.clear,
		                new int[]{   1,        1,      1,      7,      10,           17,           21,          28,         32,           39,     43,            50,         54},
						new string[]{"Scratch","Growl","Ember","Ember","Smokescreen","Dragon Rage","Scary Face","Fire Fang","Flame Burst","Slash","Flamethrower","Fire Spin","Inferno"},
						new string[]{},
						new int[]{6}, new string[]{"Level,36"}),
		new PokemonData(6, "Charizard", PokemonData.Type.FIRE, PokemonData.Type.FLYING, "Blaze", null, "Solar Power", 
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.7f, 90.5f, 
		                240, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.RED, 70, 
		                "Flame", "When expelling a blast of superhot fire, the red flame at the tip of its tail burns more intensely.", 
		                78,84,78,109,85,100, 0.4f, new Color(1,0.5f,0,1), 
		                new int[]{   1,            1,          1,            1,          1,            1,        1,      1,      1,            7,      10,           17,           21,          28,         32,           36,           41,     47,            56,         62,       71,         77}, 
						new string[]{"Flare Blitz","Heat Wave","Dragon Claw","Air Slash","Shadow Claw","Scratch","Ember","Growl","SmokeScreen","Ember","Smokescreen","Dragon Rage","Scary Face","Fire Fang","Flame Burst","Wing Attack","Slash","Flamethrower","Fire Spin","Inferno","Heat Wave","Flare Blitz"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(7, "Squirtle",PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.5f, 9,
		                63,PokemonData.LevelingRate.MEDIUMSLOW, 0,0,1,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Tiny Turtle", "Squirtle's shell is not merely used for protection. The shell's rounded shape and the grooves on its surface help minimize resistance in water, enabling this Pokémon to swim at high speeds",
		                44,48,65,50,64,43, 0, Color.clear,
		                new int[]{   1,       4,          7,          10,        13,      16,    19,          22,       25,           28,         31,          34,            37,          40},
						new string[]{"Tackle","Tail Whip","Water Gun","Withdraw","Bubble","Bite","Rapid Spin","Protect","Water Pulse","Aqua Tail","Skull Bash","Iron Defence","Rain Dance","Hydro Pump"},
						new string[]{},
						new int[]{8}, new string[]{"Level,16"}),
		new PokemonData(8, "Wartortle",PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1, 22.5f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,1,0,1,0, PokemonData.PokedexColor.BLUE,70,
		                "Turtle","Its tail is large and covered with a rich, thick fur. The tail becomes increasingly deeper in color as Wartortle ages. The scratches on its shell are evidence of this Pokémon’s toughness as a battler.",
		                59,63,80,65,80,58, 0, Color.clear,
		                new int[]{   1,       1,          1,          4,          7,          10,        13,      16,    20,          24,       28,           32,         36,          40,            44,          48},
						new string[]{"Tackle","Tail Whip","Water Gun","Tail Whip","Water Gun","Withdraw","Bubble","Bite","Rapid Spin","Protect","Water Pulse","Aqua Tail","Skull Bash","Iron Defence","Rain Dance","Hydro Pump"},
						new string[]{},
						new int[]{9}, new string[]{"Level,36"}),
		new PokemonData(9, "Blastoise", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.6f, 85.5f,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,3,0, PokemonData.PokedexColor.BLUE, 70,
		                "Shellfish","The jets of water it spouts from the rocket cannons on its shell can punch through thick steel",
		                79,83,100,85,105,78, 0, Color.clear,
		                new int[]{   1,             1,       1,          1,          1,         4,          7,          10,        13,      16,    20,          24,       28,           32,         39,          46,            53,          60},
						new string[]{"Flash Cannon","Tackle","Tail Whip","Water Gun","Withdraw","Tail Whip","Water Gun","Withdraw","Bubble","Bite","Rapid Spin","Protect","Water Pulse","Aqua Tail","Skull Bash","Iron Defence","Rain Dance","Hydro Pump"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(10, "Caterpie", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shield Dust", null, "Run Away",
		                50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.3f, 2.9f, 
		                39, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Worm", "Caterpie has a voracious appetite. It can devour leaves bigger than its body right before your eyes. From its antenna, this Pokémon releases a terrifically strong odor.",
		                45,30,35,20,20,45, 0, Color.clear, 
		                new int[]{   1,       1,            15},
						new string[]{"Tackle","String Shot","Bug Bite"},
						new string[]{},
						new int[]{11}, new string[]{"Level,7"}),
		new PokemonData(11, "Metapod", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
		                50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.7f, 9.9f, 
		                72, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Cocoon", "The shell covering this Pokémon’s body is as hard as an iron slab. Metapod does not move very much. It stays still because it is preparing its soft innards for evolution inside the hard shell.",
		                50,20,55,25,25,30, 0, Color.clear,
		                new int[]{   1,       7},
						new string[]{"Harden","Harden"},
						new string[]{},
						new int[]{12}, new string[]{"Level,10"}),
		new PokemonData(12, "Butterfree", PokemonData.Type.BUG, PokemonData.Type.FLYING, "Compound Eyes", null, "Tinted Lens",
		                50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 1.1f, 32, 
		                173, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,2,1,0, PokemonData.PokedexColor.WHITE, 70,
		                "Butterfly", "Its wings, covered with poisonous powders, repel water. This allows it to fly in the rain.",
		                60,45,50,90,80,70, 0, Color.clear,
		                new int[]{   1,          10,         12,             12,          12,            16,    18,          22,         24,       28,           30,        34,           36,          40,         42,        46},
						new string[]{"Confusion","Confusion","Poison Powder","Stun Spore","Sleep Powder","Gust","Supersonic","Whirlwind","Psybeam","Silver Wind","Tailwind","Rage Powder","Safe Guard","Captivate","Bug Buzz","Quiver Dance"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(13, "Weedle", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shield Dust", null, "Run Away",
		                50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.3f, 3.2f, 
		                39, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Hairy Bug", "Its poison stinger is very powerful. Its bright-colored body is intended to warn off its enemies.",
		                40,35,30,20,20,50, 0, Color.clear, 
		                new int[]{   1,             1,            15},
						new string[]{"Poison Sting","String Shot","Bug Bite"},
						new string[]{},
						new int[]{14}, new string[]{"Level,7"}),
		new PokemonData(14, "Kakuna", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shed Skin", null, null,
		                50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.6f, 10, 
		                72, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Cocoon", "Able to move only slightly. When endangered, it may stick out its stinger and poison its enemy.",
		                45,25,50,25,25,35, 0, Color.clear, 
		                new int[]{   1,       7},
						new string[]{"Harden","Harden"},
						new string[]{},
						new int[]{15}, new string[]{"Level,10"}),
		new PokemonData(15, "Beedrill", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", null, "Sniper",
		                50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 1, 29.5f,
		                173, PokemonData.LevelingRate.MEDIUMFAST, 0,2,0,0,1,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Poison Bee", "Beedrill is extremely territorial. No one should ever approach its nest—this is for their own safety. If angered, they will attack in a furious swarm.",
		                65,90,40,45,80,75, 0, Color.clear,
		                new int[]{   1,            10,           13,            16,         19,    22,       25,            28,           31,       34,         37,          40,        45},
						new string[]{"Fury Attack","Fury Attack","Focus Energy","Twineedle","Rage","Pursuit","Toxic Spikes","Pin Missile","Agility","Assurance","Poison Jab","Endeavor","Fell Stinger"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(16, "Pidgey", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet", "Big Pecks",
		                50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 1.8f, 
		                50, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Ting Bird", "Very docile. If attacked, it will often kick up sand to protect itself rather than fight back.",
		                40,45,40,35,35,56, 0, Color.clear,
		                new int[]{   1,       5,            9,     13,            17,         21,       25,             29,       33,           37,     41,        45,           49,         53},
						new string[]{"Tackle","Sand Attack","Gust","Quick Attack","Whirlwind","Twister","Feather Dance","Agility","Wing Attack","Roost","Tailwind","Mirror Move","Air Slash","Hurricane"},
						new string[]{},
						new int[]{17}, new string[]{"Level,18"}),
		new PokemonData(17, "Pidgeotto", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet", "Big Pecks",
		                50, 120, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.1f, 30,
		                122, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,2, PokemonData.PokedexColor.BROWN, 70,
		                "Bird", "Pidgeotto claims a large area as its own territory. This Pokémon flies around, patrolling its living space. If its territory is violated, it shows no mercy in thoroughly punishing the foe with its sharp claws.",
		                63,60,55,50,50,71, 0, Color.clear,
		                new int[]{   1,       1,            1,     5,            9,     13,            17,         22,       27,             32,       37,           42,     47,        52,           57,         62},
						new string[]{"Tackle","Sand Attack","Gust","Sand Attack","Gust","Quick Attack","Whirlwind","Twister","Feather Dance","Agility","Wing Attack","Roost","Tailwind","Mirror Move","Air Slash","Hurricane"},
						new string[]{},
						new int[]{18}, new string[]{"Level,36"}),
		new PokemonData(18, "Pidgeot", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet", "Big Pecks",
		                50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.5f, 39.5f,
		                211, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,3, PokemonData.PokedexColor.BROWN, 70, 
		                "Bird", "This Pokémon has a dazzling plumage of beautifully glossy feathers. Many Trainers are captivated by the striking beauty of the feathers on its head, compelling them to choose Pidgeot as their Pokémon.",
		                83,80,75,70,70,101, 0, Color.clear,
		                new int[]{   1,          1,       1,            1,     1,             5,            9,     13,            17,         22,       27,             32,       38,           44,     50,        56,           62,         68},
						new string[]{"Hurricane","Tackle","Sand Attack","Gust","Quick Attack","Sand Attack","Gust","Quick Attack","Whirlwind","Twister","Feather Dance","Agility","Wing Attack","Roost","Tailwind","Mirror Move","Air Slash","Hurricane"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(21, "Spearow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", null, "Sniper",
		                50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 2, 
		                52, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Tiny Bird", "Very protective of its territory, it flaps its short wings busily to dart around at high speed.",
		                40,60,30,31,31,70, 0, Color.clear,
		                new int[]{   1,     1,      5,     9,            13,       17,          21,           25,       29,         33,     37},
						new string[]{"Peck","Growl","Leer","Fury Attack","Pursuit","Aerial Ace","Mirror Move","Agility","Assurance","Roost","Drill Peck"},
						new string[]{},
						new int[]{22}, new string[]{"Level,20"}),
		new PokemonData(22, "Fearow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", null, "Sniper",
		                50, 90, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.2f, 38, 
		                155, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,2, PokemonData.PokedexColor.BROWN, 70,
		                "Beak", "Fearow is recognized by its long neck and elongated beak. They are conveniently shaped for catching prey in soil or water. It deftly moves its long and skinny beak to pluck prey.",
		                65,90,65,61,61,100, 0, Color.clear,
		                new int[]{   1,          1,      1,     1,      1,     1,            5,     9,            13,       17,          23,           29,       35,         41,     47,          53},
						new string[]{"Drill Run","Pluck","Peck","Growl","Leer","Fury Attack","Leer","Fury Attack","Pursuit","Aerial Ace","Mirror Move","Agility","Assurance","Roost","Drill Peck","Drill Run"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(32, "Nidoran♂", PokemonData.Type.POISON, PokemonData.Type.NONE, "Poison Point", "Rivalry", "Hustle",
		                100, 235, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 0.5f, 9,
		                55, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.PURPLE, 70,
		                "Poison Pin", "The male Nidoran has developed muscles for moving its ears. Thanks to them, the ears can be freely moved in any direction. Even the slightest sound does not escape this Pokémon's notice.",
		                46,57,40,40,40,50, 0, Color.clear,
		                new int[]{1,1,7,9,13,19,21,25,31,33,37,43,45},
						new string[]{"Leer","Peck","Focus Energy","Double Kick","Poison Sting","Fury Attack","Horn Attack","Helping Hand","Toxic Spikes","Flatter","Poison Jab","Captivate","Horn Drill"},
						new string[]{},
						new int[]{33}, new string[]{"Level,16"}),
		new PokemonData(33, "Nidorino", PokemonData.Type.POISON, PokemonData.Type.NONE, "Poison Point", "Rivalry", "Hustle",
		                100, 120, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 0.9f, 19.5f,
		                128, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.PURPLE, 70,
		                "Poison Pin", "ts horn is harder than diamond. If it punctures an enemy,it pumps powerful venom into the wound.",
		                61,72,57,55,55,65, 0, Color.clear,
		                new int[]{1,1,7,9,13,20,23,28,35,38,43,50,58},
						new string[]{"Leer","Peck","Focus Energy","Double Kick","Poison Sting","Fury Attack","Horn Attack","Helping Hand","Toxic Spikes","Flatter","Poison Jab","Captivate","Horn Drill"},
						new string[]{},
						new int[]{34}, new string[]{"Stone,Moon Stone"}),
		new PokemonData(34, "Nidoking", PokemonData.Type.POISON, PokemonData.Type.GROUND, "Poison Point", "Rivalry", "Sheer Force",
		                100, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 1.4f, 62,
		                223, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.PURPLE, 70,
		                "Drill", "Its tail is thick and powerful. If it binds an enemy, it can snap the victim's spine quite easily.",
		                81,102,77,85,75,85, 0, Color.clear,
		                new int[]{   1,         1,     1,             1,            1,             23,         35,      43,           58},
						new string[]{"Megahorn","Peck","Focus Energy","Double Kick","Poison Sting","Chip Away","Thrash","Earth Power","Megahorn"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(56, "Mankey", PokemonData.Type.FIGHTING,PokemonData.Type.NONE, "Vital Spirit", "Anger Point", "Defiant",
		                50, 190, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 28, 
		                61, PokemonData.LevelingRate.MEDIUMFAST, 0,1,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Pig Monkey", "It lives in groups in the treetops. If it loses sight of its group, it becomes infuriated by its loneliness.",
		                40,80,35,35,45,70, 0, Color.clear, 
		                new int[]{   1,      1,        1,         1,     1,             9,            13,           17,            21,       25,         33,       37,          41,      45,          49,            53},
						new string[]{"Covet","Scratch","Low Kick","Leer","Focus Energy","Fury Swipes","Karate Chop","Seismic Toss","Screech","Assurance","Swagger","Cross Chop","Thrash","Punishment","Close Combat","Final Gambit"},
						new string[]{},
						new int[]{57}, new string[]{"Level,28"}),
		new PokemonData(57, "Primeape", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Vital Spirit", "Anger Point", "Defiant",
		                50, 75, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 32,
		                159, PokemonData.LevelingRate.MEDIUMFAST, 0,2,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Pig Monkey", "It grows angry if you see its eyes and gets angrier if you run. If you beat it, it gets even madder.",
		                65,105,60,60,70,95, 0, Color.clear,
		                new int[]{   1,             1,      1,        1,         1,     1,             9,            13,           17,            21,       25,         28,    35,       41,          47,      53,          59,            63},
						new string[]{"Final Gambit","Fling","Scratch","Low Kick","Leer","Focus Energy","Fury Swipes","Karate Chop","Seismic Toss","Screech","Assurance","Rage","Swagger","Cross Chop","Thrash","Punishment","Close Combat","Final Gambit"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(66, "Machop", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
		                75, 180, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 5355, 0.8f, 19.5f, 
		                61, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Superpower", "Machop exercises by hefting around a Graveler as if it were a barbell. There are some Machop that travel the world in a quest to master all kinds of martial arts.",
		                70,80,50,35,35,35, 0, Color.clear,
		                new int[]{1,1,3,7,9,13,15,19,21,25,27,31,33,37,39,43,45},
						new string[]{"Low Kick","Leer","Focus Energy","Karate Chop","Foresight","Low Sweep","Seismic Toss","Revenge","Knock Off","Vital Throw","Wake-Up Slap","Dual Chop","Submission","Bulk Up","Cross Chop","Scary Face","Dynamic Punch"},
						new string[]{},
						new int[]{67}, new string[]{"Level,28"}),
		new PokemonData(67, "Machoke", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
		                75, 90, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 5355, 1.5f, 70.5f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Superpower", "Machoke's boundless power is very dangerous, so it wears a belt that suppresses its energy.",
		                80,100,70,50,60,45, 0, Color.clear, 
		                new int[]{   1,         1,     1,             1,            3,             7,            9,          13,         15,            19,       21,         25,           27,            33,         37,          43,       47,          53,          57},
						new string[]{"Low Kick","Leer","Focus Energy","Karate Chop","Focus Energy","Karate Chop","Foresight","Low Sweep","Seismic Toss","Revenge","Knock Off","Vital Throw","Wake-Up Slap","Dual Chop","Submission","Bulk Up","Cross Chop","Scary Face","Dynamic Punch"},
						new string[]{},
						new int[]{68}, new string[]{"Trade"}),
		new PokemonData(68, "Machamp", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
		                75, 45, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE,5355, 1.6f, 130,
		                227, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0,PokemonData.PokedexColor.GRAY, 70,
		                "Superpower", "Machamp has the power to hurl anything aside. However, trying to do any work requiring care and dexterity causes its arms to get tangled. This Pokémon tends to leap into action before it thinks.",
		                90,130,80,65,85,55, 0, Color.clear,
		                new int[]{   1,           1,         1,     1,             1,            3,             7,            9,          13,         15,            19,       21,         25,           27,            33,         37,          43,       47,          53,          57},
						new string[]{"Wide Guard","Low Kick","Leer","Focus Energy","Karate Chop","Focus Energy","Karate Chop","Foresight","Low Sweep","Seismic Toss","Revenge","Knock Off","Vital Throw","Wake-Up Slap","Dual Chop","Submission","Bulk Up","Cross Chop","Scary Face","Dynamic Punch"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(126, "Magmar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null, "Vital Spirit",
		                75, 45, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 6630, 1.3f, 44.5f,
		                173, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,2,0,0, PokemonData.PokedexColor.RED, 70,
		                "Spitfire", "The scorching fire exhaled by Magmar forms heat waves around its body, making it hard to see the Pokémon clearly.",
		                65,95,57,100,85,93, 0, Color.clear,
		                new int[]{   1,     1,     1,      5,      8,            12,            15,         19,          22,           26,           29,          36,          42,         49,            55},
						new string[]{"Smog","Leer","Ember","Ember","Smokescreen","Feint Attack","Fire Spin","Clear Smog","Flame Burst","Confuse Ray","Fire Punch","Lava Plume","Sunny Day","Flamethrower","Fire Blast"},
						new string[]{},
						new int[]{}, new string[]{"Trade\\Item,Magmarizer"}),
		new PokemonData(129, "Magikarp", PokemonData.Type.WATER, PokemonData.Type.NONE, "Swift Swim", null, "Rattled",
		                50, 255, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.DRAGON, 1530, 0.9f, 10, 
		                40, PokemonData.LevelingRate.SLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.RED, 70,
		                "Fish", "Magikarp is a pathetic excuse for a Pokémon that is only capable of flopping and splashing. This behavior prompted scientists to undertake research into it.",
		                20,10,55,15,20,80, 0,Color.clear,
		                new int[]{   1,       15,      30},
						new string[]{"Splash","Tackle","Flail"},
						new string[]{},
						new int[]{130}, new string[]{"Level,20"}),
		new PokemonData(130, "Gyarados", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Intimidate", null, "Moxie",
		                50, 45, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.DRAGON, 1530, 6.5f, 235, 
		                189, PokemonData.LevelingRate.SLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Atrocious", "Once Gyarados goes on a rampage, its ferociously violent blood doesn't calm until it has burned everything down. There are records of this Pokémon's rampages lasting a whole month.",
		                95,125,79,60,100,81, 0, Color.clear,
		                new int[]{1,20,23,26,29,32,35,38,41,44,47,50},
						new string[]{"Thrash","Bite","Dragon Rage","Leer","Twister","Ice Fang","Aqua Tail","Rain Dance","Crunch","Hydro Pump","Dragon Dance","Hyper Beam"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(152, "Chikorita" , PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.9f, 6.4f,
		                64, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,1,0, PokemonData.PokedexColor.GREEN, 70,
		                "Leaf", "In battle, Chikorita waves its leaf around to keep the foe at bay. However, a sweet fragrance also wafts from the leaf, becalming the battling Pokémon and creating a cozy, friendly atmosphere all around.",
		                45,49,65,49,65,45, 0, Color.clear,
		                new int[]{   1,       1,      6,           9,              12,         17,       20,            23,            28,           31,            34,         39,         42,            45},
						new string[]{"Tackle","Growl","Razor Leaf","Poison Powder","Synthesis","Reflect","Magical Leaf","Natural Gift","Sweet Scent","Light Screen","Body Slam","Safeguard","Aromatherapy","Solar Beam"},
						new string[]{},
						new int[]{153}, new string[]{"Level,16"}),
		new PokemonData(153, "Bayleef", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.2f, 15.8f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,1,0,1,0, PokemonData.PokedexColor.GREEN, 70,
		                "Leaf", "Bayleef's neck is ringed by curled-up leaves. Inside each tubular leaf is a small shoot of a tree. The fragrance of this shoot makes people peppy.",
		                60,62,80,63,80,60, 0, Color.clear,
		                new int[]{   1,       1,      1,           1,              6,           9,              12,         18,       22,            26,            32,           36,            40,         46,         50,            54},
						new string[]{"Tackle","Growl","Razor Leaf","Poison Powder","Razor Leaf","Poison Powder","Synthesis","Reflect","Magical Leaf","Natural Gift","Sweet Scent","Light Screen","Body Slam","Safeguard","Aromatherapy","Solar Beam"},
						new string[]{},
						new int[]{154}, new string[]{"Level,32"}),
		new PokemonData(154, "Meganium", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.8f, 100.5f,
		                236, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,1,0,2,0, PokemonData.PokedexColor.GREEN, 70,
		                "Herb", "Meganium's breath has the power to revive dead grass and plants. It can make them healthy again.",
		                80,82,100,83,100,80, 0, Color.clear,
		                new int[]{   1,               1,       1,      1,           1,              6,           9,              12,         18,       22,            26,            32,           34,           40,            46,         54,          60,            66,          70},
						new string[]{"Petal Blizzard","Tackle","Growl","Razor Leaf","Poison Powder","Razor Leaf","Poison Powder","Synthesis","Reflect","Magical Leaf","Natural Gift","Petal Dance","Sweet Scent","Light Screen","Body Slam","Safe Guard","Aromatherapy","Solar Beam","Petal Blizzard"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(155, "Cyndaquil", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 7.9f,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.YELLOW, 70,
		                "Fire Mouse", "It is timid, and always curls itself up in a ball. If attacked, it flares up its back for protection.",
		                39,52,43,60,50,65, 0, Color.clear,
		                new int[]{   1,       1,     6,            10,     13,            19,           22,            28,            31,     37,          40,            46,       49,       55,           58},
						new string[]{"Tackle","Leer","Smokescreen","Ember","Quick Attack","Flame Wheel","Defense Curl","Flame Charge","Swift","Lava Plume","Flamethrower","Inferno","Rollout","Double-Edge","Eruption"},
						new string[]{},
						new int[]{156}, new string[]{"Level,14"}),
		new PokemonData(156, "Quilava", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.9f, 19,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,1, PokemonData.PokedexColor.YELLOW, 70,
		                "Volcano", "Quilava keeps its foes at bay with the intensity of its flames and gusts of superheated air. This Pokémon applies its outstanding nimbleness to dodge attacks even while scorching the foes with flames.",
		                58,64,58,80,65,80, 0, Color.clear,
		                new int[]{   1,       1,     1,            6,            10,     13,            20,           24,            31,     35,            42,          46,            53,       57,       64,           68},
						new string[]{"Tackle","Leer","Smokescreen","Smokescreen","Ember","Quick Attack","Flame Wheel","Defense Curl","Swift","Flame Charge","Lava Plume","Flamethrower","Inferno","Rollout","Double-Edge","Eruption"},
						new string[]{},
						new int[]{157}, new string[]{"Level,36"}),
		new PokemonData(157, "Typhlosion", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.7f, 79.5f,
		                240, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Volcano", "Typhlosion obscures itself behind a shimmering heat haze that it creates using its intensely hot flames. This Pokémon creates blazing explosive blasts that burn everything to cinders.",
		                78,84,78,109,85,100, 0.5f, new Color(1,0.5f,0,1),
		                new int[]{   1,         1,            1,          1,       1,     1,            1,      6,            10,     13,            20,           24,            31,     35,            43,          48,            56,       61,       69,           74},
						new string[]{"Eruption","Double-Edge","Gyro ball","Tackle","Leer","Smokescreen","Ember","Smokescreen","Ember","Quick Attack","Flame Wheel","Defense Curl","Swift","Flame Charge","Lava Plume","Flamethrower","Inferno","Rollout","Double-Edge","Eruption"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(158, "Totodile", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.6f, 9.5f,
		                63, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Big Jaw", "Despite the smallness of its body, Totodile's jaws are very powerful. While the Pokémon may think it is just playfully nipping, its bite has enough power to cause serious injury.",
		                50,65,64,44,48,43, 0, Color.clear,
		                new int[]{   1,        1,     6,          8,     13,    15,          20,        22,     27,      29,         34,     36,       41,      43,         48,           50},
						new string[]{"Scratch","Leer","Water Gun","Rage","Bite","Scary Face","Ice Fang","Flail","Crunch","Chip Away","Slash","Screech","Thrash","Aqua Tail","Superpower","Hydro Pump"},
						new string[]{},
						new int[]{159}, new string[]{"Level,18"}),
		new PokemonData(159, "Croconaw", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.1f, 25,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,1,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Big Jaw", "Once Croconaw has clamped its jaws on its foe, it will absolutely not let go. Because the tips of its fangs are forked back like barbed fishhooks, they become impossible to remove when the have sunk in.",
		                65,80,80,59,63,58, 0, Color.clear,
		                new int[]{   1,        1,     1,          6,          8,     13,    15,          21,        24,     30,      33,         39,     42,       48,      51,         57,          60},
						new string[]{"Scratch","Leer","Water Gun","Water Gun","Rage","Bite","Scary Face","Ice Fang","Flail","Crunch","Chip Away","Slash","Screech","Thrash","Aqua Tail","Superpower","Hydro Pump"},
						new string[]{},
						new int[]{160}, new string[]{"Level,30"}),
		new PokemonData(160, "Feraligatr", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 2.3f, 88.8f,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,1,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Big Jaw", "Feraligatr intimidates its foes by opening its huge mouth. In battle, it will kick the ground hard with its thick and powerful hind legs to charge at the foe at an incredible speed.",
		                85,105,100,79,83,78, 0, Color.clear,
		                new int[]{   1,        1,     1,          1,     6,          8,     13,    15,          21,        24,     30,       32,      37,         45,     50,       58,      63,         71,          76},
						new string[]{"Scratch","Leer","Water Gun","Rage","Water Gun","Rage","Bite","Scary Face","Ice Fang","Flail","Agility","Crunch","Chip Away","Slash","Screech","Thrash","Aqua Tail","Superpower","Hydro Pump"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(161, "Sentret", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Keen Eye", "Frisk",
		                50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.8f, 6, 
		                43, PokemonData.LevelingRate.MEDIUMFAST, 0,1,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Scout", "When Sentret sleeps, it does so while another stands guard. The sentry wakes the others at the first sign of danger. When this Pokémon becomes separated from its pack, it becomes incapable of sleep due to fear.",
		                35,46,34,35,45,20, 0, Color.clear,
		                new int[]{   1,        1,          4,             7,             13,           16,            19,         25,    28,    31,            36,       39,          42,        47},
						new string[]{"Scratch","Foresight","Defense Curl","Quick Attack","Fury Swipes","Helping Hand","Follow Me","Slam","Rest","Sucker Punch","Amnesia","Baton Pass","Me First","Hyper Voice"},
						new string[]{},
						new int[]{162}, new string[]{"Level,15"}),
		new PokemonData(162, "Furret", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Keen Eye", "Frisk",
		                50, 90, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 1.8f, 32.5f, 
		                145, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,2, PokemonData.PokedexColor.BROWN, 70,
		                "Long Body", "It lives in narrow burrows that fit its slim body. The deeper the nests go, the more maze-like they become.",
		                85,76,64,45,55,90, 0, Color.clear,
		                new int[]{   1,        1,          1,             1,             4,             7,             13,           17,            21,         28,    32,    36,            42,       46,          50,        56},
						new string[]{"Scratch","Foresight","Defense Curl","Quick Attack","Defense Curl","Quick Attack","Fury Swipes","Helping Hand","Follow Me","Slam","Rest","Sucker Punch","Amnesia","Baton Pass","Me First","Hyper Voice"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(163, "Hoothoot", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Insomnia", "Keen Eye", "Tinted Lens",
		                50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.7f, 21.2f, 
		                52, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Owl", "It always stand on one foot. It changes feet so fast, the movement can rarely be seen.",
		                60,30,30,36,56,50, 0, Color.clear,
		                new int[]{   1,       1,      1,          5,         9,     13,      17,       21,         25,            29,         33,         37,            41,            45,            49,            53,     57},
						new string[]{"Tackle","Growl","Foresight","Hypnosis","Peck","Uproar","Reflect","Confusion","Echoed Voice","Take Down","Air Slash","Zen Headbutt","Synchronoise","Extrasensory","Psycho Shift","Roost","Dream Eater"},
						new string[]{},
						new int[]{164}, new string[]{"Level,20"}),
		new PokemonData(164, "Noctowl", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Insomnia", "Keen Eye", "Tinted Lens",
		                50, 90, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.6f, 40.8f,
		                155, PokemonData.LevelingRate.MEDIUMFAST, 2,0,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Owl", "Noctowl never fails at catching prey in darkness. This Pokémon owes its success to its superior vision that allows it to see in minimal light, and to its soft, supple wings that make no sound in flight.",
		                100,50,50,76,96,70, 0, Color.clear,
		                new int[]{   1,            1,           1,       1,      1,          1,         5,         9,     13,      17,       22,         27,            32,         37,         42,            47,            52,            57,            62,     67},
						new string[]{"Dream Eater","Sky Attack","Tackle","Growl","Foresight","Hypnosis","Hypnosis","Peck","Uproar","Reflect","Confusion","Echoed Voice","Take Down","Air Slash","Zen Headbutt","Synchronoise","Extrasensory","Psycho Shift","Roost","Dream Eater"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(167, "Spinarak", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", "Insomnia", "Sniper",
		                50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.5f, 8.5f, 
		                50, PokemonData.LevelingRate.FAST, 0,1,0,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "String Spit", "The web spun by Spinarak can be considered its second nervous system. It is said that this Pokémon can determine what kind of prey is touching its web just by the tiny vibrations it feels through the web's strands.",
		                40,60,40,40,40,30, 0, Color.clear, 
		                new int[]{   1,             1,            5,           8,          12,          15,           19,            22,           26,            29,          33,       36,           40,       43,          47,            50},
						new string[]{"Poison Sting","String Shot","Scary Face","Constrict","Leech Life","Night Shade","Shadow Sneak","Fury Swipes","Sucker Punch","Spider Web","Agility","Pin Missile","Psychic","Poison Jab","Cross Poison","Sticky Web"},
						new string[]{},
						new int[]{168}, new string[]{"Level,22"}),
		new PokemonData(168, "Ariados", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", "Insomnia", "Sniper",
		                50, 90, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1.1f, 33.5f, 
		                137, PokemonData.LevelingRate.FAST, 0,2,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Long Leg", "It attaches silk to its prey and sets it free. Later, it tracks the silk to the prey and its friends.",
		                70,90,70,60,60,40, 0, Color.clear,
		                new int[]{   1,             1,             1,         1,             1,            1,           1,          5,           8,          12,          15,           19,            23,           28,            32,          37,       41,           46,       50,          55,            58},
						new string[]{"Venom Drench","Fell Stinger","Bug Bite","Poison Sting","String Shot","Scary Face","Constrict","Scary Face","Constrict","Leech Life","Night Shade","Shadow Sneak","Fury Swipes","Sucker Punch","Spider Web","Agility","Pin Missile","Psychic","Poison Jab","Cross Poison","Sticky Web"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(183, "Marill", PokemonData.Type.WATER, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power", "Sap Sipper",
		                50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FAIRY, 2805, 0.4f, 8.5f,
		                88, PokemonData.LevelingRate.FAST, 2,0,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Aqua Mouse", "Marill's oil-filled tail acts much like a life preserver. If you see just its tail bobbing on the water's surface, it's a sure indication that this Pokémon is diving beneath the water to feed on aquatic plants.",
		                70,20,50,20,50,40, 0, Color.clear,
		                new int[]{   1,       1,          2,          5,            7,       10,            10,       13,           16,            20,         23,          28,         31,          37,           40,          47},
						new string[]{"Tackle","Water Gun","Tail Whip","Water Sport","Bubble","Defense Curl","Rollout","Bubble Beam","Helping Hand","Aqua Tail","Play Rough","Aqua Ring","Rain Dance","Double-Edge","Superpower","Hydro Pump"},
						new string[]{},
						new int[]{184}, new string[]{"Level,18"}),
		new PokemonData(184, "Azumarill", PokemonData.Type.WATER, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power", "Sap Sipper",
		                50, 75, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FAIRY, 2805, 0.8f, 28.5f,
		                185, PokemonData.LevelingRate.FAST, 3,0,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Aqua Rabbit", "Azumarill's long ears are indispensable sensors. By focusing its hearing, this Pokémon can identify what kinds of prey are around, even in rough and fast-running rivers.",
		                100,50,80,60,80,50, 0, Color.clear,
		                new int[]{   1,       1,          1,            1,          2,          5,            7,       10,            10,       13,           16,            21,         25,          31,         35,          42,           46,          55},
						new string[]{"Tackle","Tail Whip","Water Sport","Water Gun","Tail Whip","Water Sport","Bubble","Defense Curl","Rollout","Bubble Beam","Helping Hand","Aqua Tail","Play Rough","Aqua Ring","Rain Dance","Double-Edge","Superpower","Hydro Pump"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(196, "Espeon", PokemonData.Type.PSYCHIC, PokemonData.Type.NONE, "Synchronize", null, "Magic Bounce",
		                87.5f, 45, PokemonData.EggGroup.FIELD,PokemonData.EggGroup.NONE, 9180, 0.9f, 26.5f,
		                184, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,2,0,0, PokemonData.PokedexColor.PURPLE, 70,
		                "Sun", "Espeon is extremely loyal to any Trainer it considers to be worthy. It is said that this Pokémon developed its precognitive powers to protect its Trainer from harm.",
		                65,65,60,130,90,110, 0, Color.clear, 
		                new int[]{1,1,1,5,9,13,17,20,25,29,33,37,41,45},
						new string[]{"Helping Hand","Tackle","Tail Whip","Sand Attack","Confusion","Quick Attack","Swift","Psybeam","Future Sight","Psych Up","Morning Sun","Psychic","Last Resort","Power Swap"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(197, "Umbreon", PokemonData.Type.DARK, PokemonData.Type.NONE, "Synchronize", null, "Inner Focus",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 9180, 1, 27, 
		                184, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,2,0, PokemonData.PokedexColor.BLACK, 35, 
		                "Moonlight", "Umbreon evolved as a result of exposure to the moon's waves. It hides silently in darkness and waits for its foes to make a move. The rings on its body glow when it leaps to attack.",
		                95,65,110,60,130,65, 0.1f, new Color(1,1,0,1),
		                new int[]{   1,              1,        1,           5,             9,         13,             17,            20,             25,          29,        33,          37,          41,            45} ,
						new string[]{"Helping Hand", "Tackle", "Tail Whip", "Sand Attack", "Pursuit", "Quick Attack", "Confuse Ray", "Feint Attack", "Assurance", "Screech", "Moonlight", "Mean Look", "Last Resort", "Guard Swap"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(218, "Slugma", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Magma Armor", "Flame Body", "Weak Armor",
		                50, 190, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.7f, 35,
		                50, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,1,0,0, PokemonData.PokedexColor.RED, 70,
		                "Lava", "It is a species of Pokémon that lives in volcanic areas. If its body cools, its skin hardens and immobilizes it. To avoid that, it sleeps near magma",
		                40,40,40,70,40,20, 0, Color.clear,
		                new int[]{   1,     1,     6,      8,           13,      15,          20,          22,             27,           29,          34,          36,       41,         43,       48,            50},
						new string[]{"Yawn","Smog","Ember","Rock Throw","Harden","Incinerate","Clear Smog","Ancient Power","Flame Burst","Rock Slide","Lava Plume","Amnesia","Body Slam","Recover","Flamethrower","Earth Power"},
						new string[]{},
						new int[]{219}, new string[]{"Level,38"}),
		new PokemonData(219, "Slugma", PokemonData.Type.FIRE, PokemonData.Type.ROCK, "Magma Armor", "Flame Body", "Weak Armor",
		                50, 75, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.8f, 55, 
		                144, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Lava", "Magcargo's shell is actually its skin that hardened as a result of cooling. Its shell is very brittle and fragile - just touching it causes it to crumble apart. This Pokémon returns to its original size by dipping itself in magma.",
		                50,50,120,80,80,30, 0, Color.clear,
		                new int[]{   1,            1,     1,     1,      1,           6,      8,           13,      15,          20,          22,             27,           29,          34,          36,       38,           43,         47,       54,            58},
						new string[]{"Earth Power","Yawn","Smog","Ember","Rock Throw","Ember","Rock Throw","Harden","Incinerate","Clear Smog","Ancient Power","Flame Burst","Rock Slide","Lava Plume","Amnesia","Shell Smash","Body Slam","Recover","Flamethrower","Earth Power"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(240, "Magby", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null, "Vital Spirit",
		                75, 45, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 6630, 0.7f, 21.4f, 
		                73, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.RED, 70,
		                "Live Coal", "Magby's state of health is determined by observing the fire it breathes. If the Pokémon is spouting yellow flames from its mouth, it is in good health. When it is fatigued, black smoke will be mixed in with the flames.",
		                45,75,37,70,55,83, 0, Color.clear,
		                new int[]{   1,     1,     5,      8,            12,            15,         19,          22,           26,           29,          33,          36,         40,            43},
						new string[]{"Smog","Leer","Ember","Smokescreen","Feint Attack","Fire Spin","Clear Smog","Flame Burst","Confuse Ray","Fire Punch","Lava Plume","Sunny Day","Flamethrower","Fire Blast"},
						new string[]{},
						new int[]{126}, new string[]{"Level,30"}),
		new PokemonData(246, "Larvitar", PokemonData.Type.ROCK, PokemonData.Type.GROUND, "Guts", null, "Sand Veil",
		                50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 0.6f, 72,
		                60, PokemonData.LevelingRate.SLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.GREEN, 35,
		                "Rock Skin", "Born deep underground, it comes aboveground and becomes a pupa once it has finished eating the surrounding soil.", 
		                50,64,50,45,50,41, 0, Color.clear,
		                new int[]{   1,     1,     5,          10,       14,         19,          23,          28,      32,          37,       41,      46,          50,          55},
						new string[]{"Bite","Leer","Sandstorm","Screech","Chip Away","Rock Slide","Scary Face","Thrash","Dark Pulse","Payback","Crunch","Earthquake","Stone Edge","Hyper Beam"},
						new string[]{},
						new int[]{247}, new string[]{"Level,30"}),
		new PokemonData(247, "Pupitar", PokemonData.Type.ROCK, PokemonData.Type.GROUND, "Shed Skin", null, null, 
		                50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 1.2f, 152,
		                144, PokemonData.LevelingRate.SLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.GRAY, 35,
		                "Hard Shell", "Pupitar creates a gas inside its body that it compresses and forcefully ejects to propel itself like a jet. The body is very durable--it avoids damage even if it hits solid steel.",
		                70,84,70,65,70,51, 0, Color.clear,
		                new int[]{   1,     1,     1,          1,        5,          10,       14,         19,          23,          28,      34,          41,       47,      54,          60,          67},
						new string[]{"Bite","Leer","Sandstorm","Screech","Sandstorm","Screech","Chip Away","Rock Slide","Scary Face","Thrash","Dark Pulse","Payback","Crunch","Earthquake","Stone Edge","Hyper Beam"},
						new string[]{},
						new int[]{248}, new string[]{"Level,55"}),
		new PokemonData(248, "Tyranitar", PokemonData.Type.ROCK, PokemonData.Type.DARK, "Sand Stream", null, "Unnerve",
		                50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 2.5f, 202,
		                270, PokemonData.LevelingRate.SLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.GREEN, 35,
		                "Armor", "Tyranitar is so overwhelmingly powerful, it can bring down a whole mountain to make its nest. This Pokémon wanders about in mountains seeking new opponents to fight.",
		                100,134,110,95,100,61, 0, Color.clear,
		                new int[]{   1,             1,         1,          1,     1,     1,          1,        5,          10,       14,         19,          23,          28,      34,          41,       47,      54,          63,          73,          82},
						new string[]{"Thunder Fang","Ice Fang","Fire Fang","Bite","Leer","Sandstorm","Screech","Sandstorm","Screech","Chip Away","Rock Slide","Scary Face","Thrash","Dark Pulse","Payback","Crunch","Earthquake","Stone Edge","Hyper Beam","Giga Impact"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(252, "Treecko", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.5f, 5,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.GREEN, 70,
		                "Wood Gecko", "Treecko is cool, calm, and collected—it never panics under any situation. If a bigger foe were to glare at this Pokémon, it would glare right back without conceding an inch of ground.",
		                40,45,35,65,55,70, 0, Color.clear,
		                new int[]{   1,      1,     5,       9,             13,          17,       21,          25,       29,    33,      37,           41,           45,        49},
						new string[]{"Pound","Leer","Absorb","Quick Attack","Mega Drain","Pursuit","Giga Drain","Agility","Slam","Detect","Energy Ball","Quick Guard","Endeavor","Screech"},
						new string[]{},
						new int[]{253}, new string[]{"Level,16"}),
		new PokemonData(253, "Grovyle", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.9f, 21.6f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,2, PokemonData.PokedexColor.GREEN, 70,
		                "Wood Gecko", "This Pokémon adeptly flies from branch to branch in trees. In a forest, no Pokémon can ever hope to catch a fleeing Grovyle however fast they may be.",
		                50,65,45,85,65,95, 0, Color.clear,
		                new int[]{   1,      1,     1,       1,             5,       9,             13,          16,           18,       23,          28,       33,    38,      43,         48,           53,           58,          63},
						new string[]{"Pound","Leer","Absorb","Quick Attack","Absorb","Quick Attack","Mega Drain","Fury Cutter","Pursuit","Leaf Blade","Agility","Slam","Detect","X-Scissor","False Swipe","Quick Guard","Leaf Storm","Screech"},
						new string[]{},
						new int[]{254}, new string[]{"Level,36"}),
		new PokemonData(254, "Sceptile", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.7f,52.2f,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,3, PokemonData.PokedexColor.GREEN, 70,
		                "Forest", "The leaves that grow on its arms can slice down thick trees. It is without peer in jungle combat.",
		                70,85,65,105,85,120, 0, Color.clear,
		                new int[]{   1,           1,            1,      1,     1,       1,             5,       9,             13,          16,           18,       23,          28,       33,    36,         39,      45,         51,           57,           63,          69},
						new string[]{"Leaf Storm","Night Slash","Pound","Leer","Absorb","Quick Attack","Absorb","Quick Attack","Mega Drain","Fury Cutter","Pursuit","Leaf Blade","Agility","Slam","Dual Chop","Detect","X-Scissor","False Swipe","Quick Guard","Leaf Storm","Screech"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(255, "Torchic", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Speed Boost",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 2.5f,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.RED, 70,
		                "Chick", "Inside its body is a place where it keeps a small flame. Hug it! It will be as warm as a hot-water bottle.",
		                45,60,40,70,50,45, 0, Color.clear,
		                new int[]{   1,        1,      5,      14,           10,    19,         23,            28,           32,            37,     41,           46},
						new string[]{"Scratch","Growl","Ember","Sand Attack","Peck","Fire Spin","Quick Attack","Flame Burst","Focus Energy","Slash","Mirror Move","Flamethrower"},
						new string[]{},
						new int[]{256}, new string[]{"Level,16"}),
		new PokemonData(256, "Combusken", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Speed Boost",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.9f, 19.5f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,1,0,0, PokemonData.PokedexColor.RED, 70,
		                "Young Fowl", "Its kicking mastery lets it loose 10 kicks per second. It emits sharp cries to intimidate foes.",
		                60,85,60,85,60,55, 0, Color.clear,
		                new int[]{   1,        1,      1,      1,            5,      10,           14,    16,           20,            25,            31,       36,            42,     47,           53,            58},
						new string[]{"Scratch","Growl","Ember","Sand Attack","Ember","Sand Attack","Peck","Double Kick","Flame Charge","Quick Attack","Bulk Up","Focus Energy","Slash","Mirror Move","Sky Uppercut","Flare Blitz"},
						new string[]{},
						new int[]{257}, new string[]{"Level,36"}),
		new PokemonData(257, "Blaziken", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Speed Boost",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.9f, 52,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Blaze", "In battle, Blaziken blows out intense flames from its wrists and attacks foes courageously. The stronger the foe, the more intensely this Pokémon’s wrists burn.",
		                80,120,70,110,70,80, 0, Color.clear,
		                new int[]{   1,            1,           1,               1,        1,      1,      1,            5,      10,           14,    16,           20,            25,            31,       36,          37,            44,     50,          57,            63},
						new string[]{"Flare Blitz","Fire Punch","High Jump Kick","Scratch","Growl","Ember","Sand Attack","Ember","Sand Attack","Peck","Double Kick","Flame Charge","Quick Attack","Bulk Up","Blaze Kick","Focus Energy","Slash","Brave Bird","Sky Uppercut","Flare Blitz"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(258, "Mudkip", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Damp",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.4f, 7.6f,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Mud Fish", "The fin on Mudkip’s head acts as highly sensitive radar. Using this fin to sense movements of water and air, this Pokémon can determine what is taking place around it without using its eyes.",
		                50,70,50,50,50,40, 0, Color.clear,
		                new int[]{   1,       1,      4,          9,         12,         17,    20,         25,          28,       33,         36,         41,          44},
						new string[]{"Tackle","Growl","Water Gun","Mud-Slap","Foresight","Bide","Mud Sport","Rock Throw","Protect","Whirlpool","Take Down","Hydro Pump","Endeavor"},
						new string[]{},
						new int[]{259}, new string[]{"Level,16"}),
		new PokemonData(259, "Marshtomp", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Torrent", null, "Damp",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.7f, 28,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Mud Fish", "Marshtomp is much faster at traveling through mud than it is at swimming. This Pokémon’s hindquarters exhibit obvious development, giving it the ability to walk on just its hind legs.",
		                70,85,70,60,70,50, 0, Color.clear,
		                new int[]{   1,       1,      1,          1,         4,          9,         12,         16,        18,    22,        28,          32,       38,           42,         48,          52},
						new string[]{"Tackle","Growl","Water Gun","Mud-Slap","Water Gun","Mud Slap","Foresight","Mud Shot","Bide","Mud Bomb","Rock Slide","Protect","Muddy Water","Take Down","Earthquake","Endeavor" },
						new string[]{},
						new int[]{260}, new string[]{"Level,36"}),
		new PokemonData(260, "Swampert", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Torrent", null, "Damp",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.5f, 81.9f,
		                241, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Mud Fish", "Swampert predicts storms by sensing subtle differences in the sounds of waves and tidal winds with its fins. If a storm is approaching, it piles up boulders to protect itself.",
		                100,110,90,85,90,60, 0, Color.clear,
		                new int[]{   1,           1,       1,      1,          1,         4,          9,         12,         16,        18,    22,        28,          32,       39,           44,         51,          56,        63},
						new string[]{"Hammer Arm","Tackle","Growl","Water Gun","Mud-Slap","Water Gun","Mud-Slap","Foresight","Mud Shot","Bide","Mud Bomb","Rock Slide","Protect","Muddy Water","Take Down","Earthquake","Endeavor","Hammer Arm"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(263, "Zigzagoon", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Pickup", "Gluttony", "Quick Feet",
		                50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.4f, 17.5f,
		                48, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Tiny Raccoon", "Rubbing its nose against the ground, it always wanders about back and forth in search of something. It is distinguished by the zigzag footprints it leaves.",
		                38,30,41,30,41,60, 0, Color.clear,
		                new int[]{   1,       1,      5,          7,            11,        12,              13,           17,         19,           23,     25,      29,     31,         35,    37,          41}, 
						new string[]{"Tackle","Growl","Tail Whip","Sand Attack","Headbutt","Baby-Doll Eyes","Odor Sleuth","Mud Sport","Pin Missile","Covet","Bestow","Flail","Take Down","Rest","Belly Drum","Fling"},
						new string[]{},
						new int[]{264}, new string[]{"Level,20"}),
		new PokemonData(264, "Linoone", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Pickup", "Gluttony", "Quick Feet",
		                50, 90, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.5f, 32.5f,
		                147, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,2, PokemonData.PokedexColor.WHITE, 70,
		                "Rushing", "Linoone always runs full speed and only in straight lines. If facing an obstacle, it makes a right-angle turn to evade it. This Pokémon is very challenged by gently curving roads.",
		                78,70,61,50,61,100, 0, Color.clear,
		                new int[]{   1,           1,           1,           1,       1,      1,          1,            5,          7,            11,        13,           17,         19,           24,     27,      32,     35,           40,    43,          48},
						new string[]{"Play Rough","Rototiller","Switcheroo","Tackle","Growl","Tail Whip","Sand Attack","Tail Whip","Sand Attack","Headbutt","Odor Sleuth","Mud Sport","Fury Swipes","Covet","Bestow","Slash","Double-Edge","Rest","Belly Drum","Fling"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(265, "Wurmple", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shield Dust", null, "Run Away",
		                50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.3f, 3.6f, 
		                39, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Worm", "Using the spikes on its rear end, Wurmple peels the bark off trees and feeds on the sap that oozes out. This Pokémon's feet are tipped with suction pads that allow it to cling to glass without slipping.",
		                45,45,35,20,30,20, 0, Color.clear,
		                new int[]{   1,       1,            5,             15},
						new string[]{"Tackle","String Shot","Poison Sting","Bug Bite"},
						new string[]{},
						new int[]{266,268}, new string[]{"if pokemon's shinyValue divided by 2's remainder is equal to 0","if pokemon's shinyValue divided by 2's remainder is equal to 1"}),
		new PokemonData(266, "Silcoon", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
		                50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.6f, 10,
		                72, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.WHITE, 70,
		                "Cocoon", "Silcoon was thought to endure hunger and not consume anything before its evolution. However, it is now thought that this Pokémon slakes its thirst by drinking rainwater that collects on its silk.",
		                50,35,55,25,25,15, 0, Color.clear,
		                new int[]{   1,       7},
						new string[]{"Harden","Harden"},
						new string[]{},
						new int[]{267}, new string[]{"Level,10"}),
		new PokemonData(267, "Beautifly", PokemonData.Type.BUG, PokemonData.Type.FLYING, "Swarm", null, "Rivalry",
		                50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1, 28.4f, 
		                173, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,3,0,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Butterfly", "It has an aggressive nature. It stabs prey with its long, narrow mouth to drain the prey's fluids.",
		                60,70,50,90,50,65, 0, Color.clear,
		                new int[]{   1,     10,    12,      15,          17,           20,          22,          25,           27,       30,         32,          35,        37,    40},
						new string[]{"Gust","Gust","Absorb","Stun Spore","Morning Sun","Air Cutter","Mega Drain","Silver Wind","Attract","Whirlwind","Giga Drain","Bug Buzz","Rage","Quiver Dance"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(268, "Cascoon", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
		                50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.7f, 11.5f, 
		                41, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.PURPLE, 70,
		                "Cocoon", "It never forgets any attack it endured while in the cocoon. After evolution, it seeks payback.",
		                50,35,55,25,25,15, 0, Color.clear, 
		                new int[]{   1,       7},
						new string[]{"Harden","Harden"},
						new string[]{},
						new int[]{269}, new string[]{"Level,10"}),
		new PokemonData(269, "Dustox", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shield Dust", null, "Compound Eyes",
		                50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1.2f, 31.6f,
		                135, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,3,0, PokemonData.PokedexColor.GREEN, 70,
		                "Poison Moth", "Dustox is instinctively drawn to light. Swarms of this Pokémon are attracted by the bright lights of cities, where they wreak havoc by stripping the leaves off roadside trees for food.",
		                60,50,70,50,90,65, 0, Color.clear,
		                new int[]{   1,     10,    12,         15,             17,         20,         22,       25,           27,            30,         32,     35,        37,       40},
						new string[]{"Gust","Gust","Confusion","Poison Powder","Moonlight","Venoshock","Psybeam","Silver Wind","Light Screen","Whirlwind","Toxic","Bug Buzz","Protect","Quiver Dance"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(276, "Taillow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Guts", null, "Scrappy",
		                50, 200, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 2.3f,
		                54, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.BLUE, 70,
		                "Tiny Swallow", "Taillow courageously stands its ground against foes, however strong they may be. This gutsy Pokémon will remain defiant even after a loss. On the other hand, it cries loudly if it becomes hungry.",
		                40,55,30,30,30,85, 0, Color.clear,
		                new int[]{   1,     1,      5,             9,             13,           17,           21,          25,           29,       33,         37,        41},
						new string[]{"Peck","Growl","Focus Energy","Quick Attack","Wing Attack","Double Team","Aerial Ace","Quick Guard","Agility","Air Slash","Endeavor","Brave Bird"},
						new string[]{},
						new int[]{277}, new string[]{"Level,22"}),
		new PokemonData(277, "Swellow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Guts", null, "Scrappy",
		                50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.7f, 19.8f, 
		                151, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,2, PokemonData.PokedexColor.BLUE, 70,
		                "Swallow", "Swellow is very conscientious about the upkeep of its glossy wings. Once two Swellow are gathered, they diligently take care of cleaning each other’s wings",
		                60,85,60,50,50,125, 0, Color.clear, 
		                new int[]{1,1,1,1,1,1,1,5,9,13,17,21,27,33,39,45,51},
						new string[]{"Brave Bird","Air Slash","Pluck","Peck","Growl","Focus Energy","Quick Attack","Focus Energy","Quick Attack","Wing Attack","Double Team","Aerial Ace","Quick Guard","Agility","Endeavor","Air Slash","Brave Bird"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(278, "Wingull", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", null, "Rain Dish",
		                50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 0.6f, 9.5f, 
		                54, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.WHITE, 70,
		                "Seagull", "Wingull has the habit of carrying prey and valuables in its beak and hiding them in all sorts of locations. This Pokémon rides the winds and flies as if it were skating across the sky.",
		                40,30,30,55,30,85, 0, Color.clear,
		                new int[]{   1,      1,          5,           8,            12,    15,           19,            22,          26,       29,          33,     36,       40,         43},
						new string[]{"Growl","Water Gun","Supersonic","Wing Attack","Mist","Water Pulse","Quick Attack","Air Cutter","Pursuit","Aerial Ace","Roost","Agility","Air Slash","Hurricane"},
						new string[]{},
						new int[]{279}, new string[]{"Level,25"}), 
		new PokemonData(279, "Pelipper", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", null, "Rain Dish",
		                50, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 1.2f, 28, 
		                151, PokemonData.LevelingRate.MEDIUMFAST, 0,0,2,0,0,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Water Bird", "Pelipper is a flying transporter that carries small Pokémon and eggs inside its massive bill. This Pokémon builds its nest on steep cliffs facing the sea.",
		                60,50,100,85,70,65, 0, Color.clear,
		                new int[]{   1,          1,           1,         1,     1,      1,          1,            1,            5,           8,            12,    15,           19,       22,     25,       28,     33,         33,       33,       39,     44,        50,          55},
						new string[]{"Hurricane","Hydro Pump","Tailwind","Soak","Growl","Water Gun","Water Sport","Wing Attack","Supersonic","Wing Attack","Mist","Water Pulse","Payback","Roost","Protect","Brine","Stockpile","Swallow","Spit Up","Fling","Tailwind","Hydro Pump","Hurricane"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(283, "Surskit", PokemonData.Type.BUG, PokemonData.Type.WATER, "Swift Swim", null, "Rain Dish",
		                50, 200, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.BUG, 4080, 0.5f, 1.7f, 
		                54, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.BLUE, 70,
		                "Pond Skater", "If Surskit senses danger, it secretes a thick, sugary syrup from the tip of its head. There are some Pokémon that love eating this syrup.",
		                40,30,32,50,52,65, 0, Color.clear,
		                new int[]{1,6,9,14,17,22,25,30,35,38},
						new string[]{"Bubble","Quick Attack","Sweet Scent","Water Sport","Bubble Beam","Agility","Mist","Haze","Aqua Jet","Baton Pass","Sticky Web"},
						new string[]{},
						new int[]{284}, new string[]{"Level,22"}),
		new PokemonData(284, "Masquerain",PokemonData.Type.BUG, PokemonData.Type.FLYING, "Intimidate", null, "Unnerve",
		                50, 75, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.BUG, 4080, 0.8f, 3.6f, 
		                145, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,1,1,0, PokemonData.PokedexColor.BLUE, 70,
		                "Eyeball", "Masquerain intimidates enemies with the eyelike patterns on its antennas. This Pokémon flaps its four wings to freely fly in any direction—even sideways and backwards—as if it were a helicopter.",
		                70,60,62,80,82,60, 0, Color.clear, 
		                new int[]{   1,             1,         1,          1,             1,       1,             1,            1,            6,             9,            14,           17,    22,          22,          26,          32,           38,         42,        48,         52},
						new string[]{"Quiver Dance","Bug Buzz","Whirlwind","Ominous Wind","Bubble","Quick Attack","Sweet Scent","Water Sport","Quick Attack","Sweet Scent","Water Sport","Gust","Scary Face","Air Cutter","Stun Spore","Silver Wind","Air Slash","Bug Buzz","Whirlwind","Quiver Dance"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(298, "Azurill", PokemonData.Type.NORMAL, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power", "Sap Sipper",
		                25, 150, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 2805, 0.2f, 2,
		                38, PokemonData.LevelingRate.FAST, 1,0,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Polka Dot", "Azurill spins its tail as if it were a lasso, then hurls it far. The momentum of the throw sends its body flying, too. Using this unique action, one of these Pokémon managed to hurl itself a record 10 meters.",
		                50,20,40,20,40,20, 0, Color.clear,
		                new int[]{1,1,2,5,7,10,13,16,20,23},
						new string[]{"Splash","Water Gun","Tail Whip","Water Sport","Bubble","Charm","Bubble Beam","Helping Hand","Slam","Bounce"},
						new string[]{},
						new int[]{183}, new string[]{"Friendship"}),
		new PokemonData(300, "Skitty", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Normalize", "Wonder Skin",
		                25, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.FAIRY, 4080, 0.6f, 11, 
		                52, PokemonData.LevelingRate.FAST, 0,0,0,0,0,1, PokemonData.PokedexColor.PINK, 70,
		                "Kitten", "Skitty has the habit of becoming fascinated by moving objects and chasing them around. This Pokémon is known to chase after its own tail and become dizzy.",
		                50,45,45,35,35,50, 0, Color.clear,
		                new int[]{   1,         1,      1,          1,       4,          7,        10,    13,               16,           19,       22,            25,     28,            31,      34,     37,         40,           43,         46},
						new string[]{"Fake Out","Growl","Tail Whip","Tackle","Foresight","Attract","Sing","Disarming Voice","Double Slap","Copycat","Feint Attack","Charm","Wake-Up Slap","Assist","Covet","Heal Bell","Double-Edge","Captivate","Play Rough"},
						new string[]{},
						new int[]{301}, new string[]{"Stone,Moon Stone"}),
		new PokemonData(301, "Delcatty", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Normalize", "Wonder Skin",
		                25, 60, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.FAIRY, 4080, 1.1f, 32.6f, 
		                133, PokemonData.LevelingRate.FAST, 1,0,0,0,0,1, PokemonData.PokedexColor.PURPLE, 70,
		                "Prim", "Delcatty prefers to live an unfettered existence in which it can do as it pleases at its own pace. Because this Pokémon eats and sleeps whenever it decides, its daily routines are completely random.",
		                70,65,65,55,55,70, 0, Color.clear,
		                new int[]{1,1,1,1},
						new string[]{"Fake Out","Attract","Sing","Double Slap"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(315, "Roselia", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point", "Leaf Guard",
		                50, 150, PokemonData.EggGroup.FAIRY, PokemonData.EggGroup.GRASS, 5355, 0.3f, 2,
		                140, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Thorn", "Roselia shoots sharp thorns as projectiles at any opponent that tries to steal the flowers on its arms. The aroma of this Pokémon brings serenity to living things.",
		                50,60,45,100,80,65, 0, Color.clear,
		                new int[]{   1,       4,       7,             10,          13,          16,          19,            22,             25,          28,            31,           34,       37,              40,     43,            46,         50}, 
						new string[]{"Absorb","Growth","Poison Sting","Stun Spore","Mega Drain","Leech Seed","Magical Leaf","Grass Whistle","Giga Drain","Toxic Spikes","Sweet Scent","Ingrain","Petal Blizzard","Toxic","Aromatherapy","Synthesis","Petal Dance"},
						new string[]{},
						new int[]{407}, new string[]{"Stone,Shiny Stone"}), 
		new PokemonData(322, "Numel", PokemonData.Type.FIRE, PokemonData.Type.GROUND, "Oblivious", "Simple", "Own Tempo",
		                50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.7f, 24,
		                61, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,1,0,0, PokemonData.PokedexColor.YELLOW, 70,
		                "Numb", "Numel is extremely dull witted—it doesn’t notice being hit. However, it can’t stand hunger for even a second. This Pokémon’s body is a seething cauldron of boiling magma.",
		                60,60,40,65,45,35, 0, Color.clear,
		                new int[]{   1,      1,       5,      8,             12,         15,           19,       22,          26,           29,     31,         36,    40,          43,            47},
						new string[]{"Growl","Tackle","Ember","Focus Energy","Magnitude","Flame Burst","Amnesia","Lava Plume","Earth Power","Curse","Take Down","Yawn","Earthquake","Flamethrower","Doube-Edge"},
						new string[]{},
						new int[]{323}, new string[]{"Level,33"}),
		new PokemonData(323, "Camerupt", PokemonData.Type.FIRE, PokemonData.Type.GROUND, "Magma Armor", "Solid Rock", "Anger Point",
		                50, 150, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.9f, 220,
		                161, PokemonData.LevelingRate.MEDIUMFAST, 0,1,0,1,0,0, PokemonData.PokedexColor.RED, 70,
		                "Eruption", "The humps on Camerupt’s back are formed by a transformation of its bones. They sometimes blast out molten magma. This Pokémon apparently erupts often when it is enraged.",
		                70,100,70,105,75,40, 0, Color.clear,
		                new int[]{   1,        1,         1,      1,       1,      1,             5,      8,             12,         15,           19,       22,          26,           29,     31,         33,          39,    46,          52,        59},
						new string[]{"Fissure","Eruption","Growl","Tackle","Ember","Focus Energy","Ember","Focus Energy","Magnitude","Flame Burst","Amnesia","Lava Plume","Earth Power","Curse","Take Down","Rock Slide","Yawn","Earthquake","Eruption","Fissure"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(339, "Barboach", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Oblivious", "Anticipation", "Hydration",
		                50, 190, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.NONE, 5355, 0.4f, 1.9f,
		                58, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Whiskers", "Barboach’s sensitive whiskers serve as a superb radar system. This Pokémon hides in mud, leaving only its two whiskers exposed while it waits for prey to come along.",
		                50,48,43,46,41,60, 0, Color.clear,
		                new int[]{   1,         6,          6,            9,          13,        15,       17,           20,         25,    25,     28,         32,          35,           39,            44},
						new string[]{"Mud-Slap","Mud Sport","Water Sport","Water Gun","Mud Bomb","Amnesia","Water Pulse","Magnitude","Rest","Snore","Aqua Tail","Earthquake","Muddy Water","Future Sight","Fissure"},
						new string[]{},
						new int[]{340}, new string[]{"Level,30"}),
		new PokemonData(340, "Whiscash", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Oblivious", "Anticipation", "Hydration",
		                50, 75, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.NONE, 5355, 0.9f, 23.6f, 
		                164, PokemonData.LevelingRate.MEDIUMFAST, 2,0,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Whiskers", "Whiscash is extremely territorial. Just one of these Pokémon will claim a large pond as its exclusive territory. If a foe approaches it, it thrashes about and triggers a massive earthquake.",
		                110,78,73,76,71,60, 0, Color.clear,
		                new int[]{   1,       1,         1,          1,            1,          6,          6,            9,          13,        15,       17,           20,         25,    25,     28,         30,            34,          39,           45,            52},
						new string[]{"Tickle","Mud-Slap","Mud Sport","Water Sport","Water Gun","Mud Sport","Water Sport","Water Gun","Mud Bomb","Amnesia","Water Pulse","Magnitude","Rest","Snore","Aqua Tail","Zen Headbutt","Earthquake","Muddy Water","Future Sight","Fissure"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(387, "Turtwig", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.4f, 10.2f,
		                64, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Tiny Leaf", "Photosynthesis occurs across its body under the sun. The shell on its back is actually hardened soil.",
		                55,68,64,45,55,31, 0, Color.clear,
		                new int[]{   1,       5,         9,       13,          17,     21,    25,          29,          33,         37,      41,          45},
						new string[]{"Tackle","Withdraw","Absorb","Razor Leaf","Curse","Bite","Mega Drain","Leech Seed","Synthesis","Crunch","Giga Drain","Leaf Storm"},
						new string[]{},
						new int[]{388}, new string[]{"Level,18"}),
		new PokemonData(388, "Grotle", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.1f, 97,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,1,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Grove", "It knows where pure water wells up. It carries fellow Pokémon there on its back.",
		                75,89,85,55,65,36, 0, Color.clear,
		                new int[]{   1,       1,         5,         9,       13,          17,     22,    27,          32,          37,         42,      47,          52},
						new string[]{"Tackle","Withdraw","Withdraw","Absorb","Razor Leaf","Curse","Bite","Mega Drain","Leech Seed","Synthesis","Crunch","Giga Drain","Leaf Storm"},
						new string[]{},
						new int[]{389}, new string[]{"Level,32"}),
		new PokemonData(389, "Torterra", PokemonData.Type.GRASS, PokemonData.Type.GROUND, "Overgrow", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 2.2f, 310,
		                236, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,1,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Continent", "Small Pokémon occasionally gather on its unmoving back to begin building their nests.",
		                95,109,105,75,85,56, 0, Color.clear,
		                new int[]{   1,            1,       1,         1,       1,           5,         9,       13,          17,     22,    27,          32,          33,          39,         45,      51,          57},
						new string[]{"Wood Hammer","Tackle","Withdraw","Absorb","Razor Leaf","Withdraw","Absorb","Razor Leaf","Curse","Bite","Mega Drain","Earthquake","Leech Seed","Synthesis","Crunch","Giga Drain","Leaf Storm"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(390, "Chimchar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Iron Fist",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.5f, 6.2f,
		                62, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Chimp","The gas made in its belly burns from its rear end. The fire burns weakly when it feels sick",
		                44,58,44,58,44,61, 0, Color.clear,
		                new int[]{   1,        1,     7,      9,      15,           17,           23,          25,       31,      33,         39,          41,         47},
						new string[]{"Scratch","Leer","Ember","Taunt","Fury Swipes","Flame Wheel","Nasty Plot","Torment","Facade","Fire Spin","Acrobatics","Slack Off","Flamethrower"},
						new string[]{},
						new int[]{391}, new string[]{"Level,14"}),
		new PokemonData(391, "Monferno", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Iron Fist",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.9f, 22,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Playful", "A bigger fire on its tail and a brighter blue pattern on its face means its rank in its pack is higher.",
		                64,78,52,78,52,81, 0,Color.clear,
		                new int[]{   1,        1,     1,      7,      9,      14,          16,           19,           26,     29,       36,            39,         46,          49,         56},
						new string[]{"Scratch","Leer","Ember","Ember","Taunt","Mach Punch","Fury Swipes","Flame Wheel","Feint","Torment","Close Combat","Fire Spin","Acrobatics","Slack Off","Flare Blitz"},
						new string[]{},
						new int[]{392}, new string[]{"Level,36"}),
		new PokemonData(392, "Infernape", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Iron Fist",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 1.2f, 55,
		                240, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,1,0,1, PokemonData.PokedexColor.BROWN, 70,
		                "Flame", "It tosses its enemies around with agility. It uses all its limbs to fight in its own unique style.",
		                76,104,71,104,71,108, 0, Color.clear,
		                new int[]{   1,            1,           1,        1,     1,      1,      7,      9,      14,          16,           19,           26,     29,          36,            42,         52,          58,         68},
						new string[]{"Flare Blitz","Punishment","Scratch","Leer","Ember","Taunt","Ember","Taunt","Mach Punch","Fury Swipes","Flame Wheel","Feint","Punishment","Close Combat","Fire Spin","Acrobatics","Calm Mind","Flare Blitz"},
						new string[]{},
						new int[]{393}, new string[]{"Level,36"}),
		new PokemonData(393, "Piplup", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Defiant", 
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 0.4f, 5.2f, 
		                63, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.BLUE, 70, 
		                "Penguin", "A poor walker, it often falls down. However, its strong pride makes it puff up its chest without a care.",
		                53,51,53,61,56,40, 0, Color.clear,
		                new int[]{   1,      4,      8,       11,           15,    18,           22,    25,           29,     32,         36,    39,          43},
						new string[]{"Pound","Growl","Bubble","Water Sport","Peck","Bubble Beam","Bide","Fury Attack","Brine","Whirlpool","Mist","Drill Peck","Hydro Pump"},
						new string[]{},
						new int[]{394}, new string[]{"Level,16"}),
		new PokemonData(394, "Prinplup", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Defiant",
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 0.8f, 23,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Penguin", "Because every Prinplup considers itself to be the most important, they can never form a group.",
		                64,66,68,81,76,50, 0, Color.clear,
		                new int[]{   1,       1,      4,      8,       11,           15,    16,          19,           24,    28,           33,     37,         42,    46,          50},
						new string[]{"Tackle","Growl","Growl","Bubble","Water Sport","Peck","Metal Claw","Bubble Beam","Bide","Fury Attack","Brine","Whirlpool","Mist","Drill Peck","Hydro Pump"},
						new string[]{},
						new int[]{395}, new string[]{"Level,36"}),						
		new PokemonData(395, "Empoleon", PokemonData.Type.WATER, PokemonData.Type.STEEL, "Torrent", null, "Defiant",
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 1.7f, 84.5f,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Emperor", "It avoids unnecessary disputes, but it will decimate anything that threatens its pride.",
		                84,86,88,111,101,60, 0, Color.clear,
		                new int[]{   1,       1,      1,       4,      8,       15,    16,          19,           28,           33,     36,        39,         46,    52,          59},
						new string[]{"Tackle","Growl","Bubble","Growl","Bubble","Peck","Metal Claw","Bubble Beam","Fury Attack","Brine","Aqua Jet","Whirlpool","Mist","Drill Peck","Hydro Pump"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(399, "Bidoof", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Simple", "Unaware", "Moody",
		                50, 255, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 4080, 0.5f, 20,
		                50, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Plump Mouse", "It lives in groups by the water. It chews up boulders and trees around its nest with its incisors.",
		                59,45,40,35,40,31, 0, Color.clear,
		                new int[]{   1,       5,      9,             13,       17,        21,          25,    29,       33,         37,          41,           45},
						new string[]{"Tackle","Growl","Defense Curl","Rollout","Headbutt","Hyper Fang","Yawn","Amnesia","Take Down","Super Fang","Super Power","Curse"},
						new string[]{},
						new int[]{400}, new string[]{"Level,15"}),
		new PokemonData(400, "Bibarel", PokemonData.Type.NORMAL, PokemonData.Type.WATER, "Simple", "Unaware", "Moody",
		                50, 127, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 4080, 1, 31.5f,
		                144, PokemonData.LevelingRate.MEDIUMFAST, 0,2,0,0,0,0, PokemonData.PokedexColor.BROWN, 70,
		                "Beaver", "A river dammed by Bibarel will never overflow its banks, which is appreciated by people nearby.",
		                79,85,60,55,60,71, 0, Color.clear,
		                new int[]{   1,           1,       1,      5,      9,             13,       15,         18,        23,          28,    33,       38,         43,          48,          53},
						new string[]{"Rototiller","Tackle","Growl","Growl","Defense Curl","Rollout","Water Gun","Headbutt","Hyper Fang","Yawn","Amnesia","Take Down","Super Fang","Superpower","Curse"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(406, "Budew", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point", "Leaf Guard",
		                50, 255, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 5355, 0.2f, 1.2f,
		                56, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Bud", "It lives alongside clear ponds. It scatters pollen that induces harsh sneezing and runny noses.",
		                40,30,35,50,70,55, 0, Color.clear,
		                new int[]{   1,       4,       7,            10,          13,         16},
						new string[]{"Absorb","Growth","Water Sport","Stun Spore","Mega Dran","Worry Seed"},
						new string[]{},
						new int[]{315}, new string[]{"Friendship\\Time,Day"}),
		new PokemonData(407, "Roserade", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point", "Technician",
		                50, 75, PokemonData.EggGroup.FAIRY, PokemonData.EggGroup.GRASS, 5355, 0.9f, 14.5f,
		                227, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Bouquet", "Luring prey with a sweet scent, it uses poison whips on its arms to poison, bind, and finish off the prey.",
		                60,70,65,125,105,90, 0, Color.clear,
		                new int[]{   1,             1,               1,             1,             1,           1,             1},
						new string[]{"Venom Drench","Grassy Terrain","Weather Ball","Poison Sting","Mega Drain","Magical Leaf","Sweet Scent"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(408, "Cranidos", PokemonData.Type.ROCK, PokemonData.Type.NONE, "Mold Breaker", null, "Sheer Force",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 0.9f, 31.5f,
		                70, PokemonData.LevelingRate.ERRATIC, 0,1,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Head Butt", "A lifelong jungle dweller from 100 million years ago, it would snap obstructing trees with headbutts.",
		                67,125,40,30,30,58, 0, Color.clear,
		                new int[]{   1,         1,     6,             10,       15,         19,          24,         28,         33,             37,            42,       46},
						new string[]{"Headbutt","Leer","Focus Energy","Pursuit","Take Down","Scary Face","Assurance","Chip Away","Ancient Power","Zen Headbutt","Screech","Head Smash"},
						new string[]{},
						new int[]{409}, new string[]{"Level,30"}),
		new PokemonData(409, "Rampardos", PokemonData.Type.ROCK, PokemonData.Type.NONE, "Mold Breaker", null, "Sheer Force",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 1.6f, 102.5f,
		                173, PokemonData.LevelingRate.ERRATIC, 0,2,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Head Butt", "Its skull is as hard as iron, it is a brute that tears down jungle trees while catching prey.",
		                97,165,60,65,50,58, 0, Color.clear,
		                new int[]{   1,         1,     6,             10,       15,         19,          24,         28,         30,        36,             43,            51,       58},
						new string[]{"Headbutt","Leer","Focus Energy","Pursuit","Take Down","Scary Face","Assurance","Chip Away","Endeavor","Ancient Power","Zen Headbutt","Screech","Head Smash"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(410, "Shieldon", PokemonData.Type.ROCK, PokemonData.Type.STEEL, "Sturdy", null, "Soundproof",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 0.5f, 57,
		                70, PokemonData.LevelingRate.ERRATIC, 0,0,1,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Shield","It habitually polishes its face by rubbing it against tree trunks. It is weak to attacks from behind.",
		                30,42,118,42,88,30, 0, Color.clear,
		                new int[]{   1,       1,        6,      10,           15,         19,            24,       28,             33,      37,           42,         46},
						new string[]{"Tackle","Protect","Taunt","Metal Sound","Take Down","Iron Defense","Swagger","Ancient Power","Endure","Metal Burst","Iron Head","Heavy Slam"},
						new string[]{},
						new int[]{411}, new string[]{"Level,30"}),
		new PokemonData(411, "Bastiodon", PokemonData.Type.ROCK,  PokemonData.Type.STEEL, "Sturdy", null, "Soundproof",
		                87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 1.3f, 149.5f, 
		                173, PokemonData.LevelingRate.ERRATIC, 0,0,2,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Shield", "When attacked, they form a wall with their shield like faces to protect their young.",
		                60,52,168,57,138,30, 0, Color.clear,
		                new int[]{   1,       1,        1,      1,            6,      10,           15,         19,            24,       28,             30,     36,      43,           51,         58},
						new string[]{"Tackle","Protect","Taunt","Metal Sound","Taunt","Metal Sound","Take Down","Iron Defense","Swagger","Ancient Power","Block","Endure","Metal Burst","Iron Head","Heavy Slam"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(427, "Buneary", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Klutz", "Limber", 50, 190,
		                PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.4f, 5.5f,
		                70, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,1, PokemonData.PokedexColor.BROWN, 0,
		                "Rabbit", "By extending its rolled-up ears and striking the ground, it can bound so high it surprises itself.",
		                55,66,44,44,56,85, 0, Color.clear,
		                new int[]{   1,             1,       1,      1,          6,       10,              13,           16,            23,         26,          33,       36,           43,         46,     50,           56,      63},
						new string[]{"Defense Curl","Splash","Pound","Foresight","Endure","Baby-Doll Eyes","Frustration","Quick Attack","Jump Kick","Baton Pass","Agility","Dizzy Punch","After You","Charm","Entrainment","Bounce","Healing Wish"},
						new string[]{},
						new int[]{428}, new string[]{"Friendship"}),
		new PokemonData(428, "Lopunny", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Klutz", "Limber",
		                50, 60, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 1.2f, 33.3f,
		                168, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,2, PokemonData.PokedexColor.BROWN, 140,
		                "Rabbit", "The ears appear to be delicate. If they are touched roughly, it kicks with its graceful legs.",
		                65,76,84,54,96,105, 0, Color.clear,
		                new int[]{   1,             1,       1,           1,           1,           1,             1,       1,      1,          6,       13,      16,            23,         26,          33,       36,           43,         46,     53,            56,      63,            66},
						new string[]{"Healing Wish","Bounce","Rototiller","Mirro Coat","Magic Coat","Defense Curl","Splash","Pound","Foresight","Endure","Return","Quick Attack","Jump Kick","Baton Pass","Agility","Dizzy Punch","After You","Charm","Entraintment","Bounce","Healing Wish","High Jump Kick"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(447, "Riolu", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Steadfast", "Inner Focus", "Prankster",
		                87.5f, 75, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 6630, 0.7f, 20.2f, 
		                57, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Emanation", "It uses the shapes of auras, which change according to emotion, to communicate with others.",
		                40,70,40,35,40,60, 0, Color.clear,
		                new int[]{1,1,1,6,11,15,19,24,29,47,50},
						new string[]{"Foresight","Quick Attack","Endure","Counter","Feint","Force Palm","Copycat","Screech","Reversal","Nasty Plot","Final Gambit"},
						new string[]{},
						new int[]{448}, new string[]{"Friendship"}),
		new PokemonData(448, "Lucario", PokemonData.Type.FIGHTING, PokemonData.Type.STEEL, "Steadfast", "Inner Focus", "Justified",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE,6630, 1.2f, 54,
		                184, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,1,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Aura", "By catching the Aura emanating from others, it can read their thoughts and movements.", 
		                70,110,70,115,70,90, 0, Color.clear,
		                new int[]{ 	 1,              1,             1,             1,            1,          1,             1,       1,           6,        11,     15,              19,            24,           29,         33,           37,        42,           47,         51,          55,            60,            65},
						new string[]{"Extreme Speed","Dragon Pulse","Close Combat","Aura Sphere","Foresight","Quick Attack","Detect","Metal Claw","Counter","Feint","Power-Up Punch","Swords Dance","Metal Sound","Bone Rush","Quick Guard","Me First","Aura Sphere","Calm Mind","Heal Pulse","Close Combat","Dragon Pulse","Extreme Speed"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(467, "Magmortar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null, "Vital Spirt",
		                70, 30, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 6630, 1.6f, 68,
		                243, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,3,0,0, PokemonData.PokedexColor.RED, 70,
		                "Blast", "It blasts fireballs of over 2,000 degrees Celsius from the ends of its arms. It lives in volcanic craters.",
		                75,95,67,125,95,83, 0, Color.clear,
		                new int[]{   1,              1,     1,     1,      1,            5,      8,            12,            15,         19,          22,           26,           29,          36,          42,         49,            55,          62},
						new string[]{"Thunder Punch","Smog","Leer","Ember","Smokescreen","Ember","Smokescreen","Feint Attack","Fire Spin","Clear Smog","Flame Burst","Confuse Ray","Fire Punch","Lava Plume","Sunny Day","Flamethrower","Fire Blast","Hyper Beam"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(495, "Snivy", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 0.6f, 8.1f,
		                28, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.GREEN, 70,
		                "Grass Snake", "They photosynthesize by bathing their tails in sunlight. When they are not feeling well, their tails droop.",
		                45,45,55,45,55,63, 0, Color.clear,
		                new int[]{   1,       4,     7,          10,    13,      16,            19,          22,          25,    28,          31,    34,          37,         40,           43},
						new string[]{"Tackle","Leer","Vine Whip","Wrap","Growth","Leaf Tornado","Leech Seed","Mega Drain","Slam","Leaf Blade","Coil","Giga Drain","Wring Out","Gastro Acid","Leaf Storm"},
						new string[]{},
						new int[]{496}, new string[]{"Level,17"}),
		new PokemonData(496, "Servine", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 0.8f, 16,
		                145, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1,PokemonData.PokedexColor.GREEN, 70,
		                "Grass Snake", "When it gets dirty, its leaves can't be used in photosynthesis, so it always keeps itself clean.",
		                60,60,75,60,75,83, 0, Color.clear,
		                new int[]{   1,       1,     1,          4,     7,          10,    13,      16,            20,          24,          28,    32,          36,    40,          44,         48,           52},
						new string[]{"Tackle","Leer","Vine Whip","Leer","Vine Whip","Wrap","Growth","Leaf Tornado","Leech Seed","Mega Drain","Slam","Leaf Blade","Coil","Giga Drain","Wring Out","Gastro Acid","Leaf Storm"},
						new string[]{},
						new int[]{497}, new string[]{"Level,36"}),
		new PokemonData(497, "Serperior", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 3.3f, 63,
		                238, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,3, PokemonData.PokedexColor.GREEN, 70,
		                "Regal", "It only gives its all against strong opponents who are not fazed by the glare from Serperior's noble eyes.",
		                75,75,95,75,95,113, 0, Color.clear,
		                new int[]{   1,       1,     1,          4,     7,          10,    13,      16,            20,          24,          28,    32,          38,    44,          50,         56,           62},
						new string[]{"Tackle","Leer","Vine Whip","Leer","Vine Whip","Wrap","Growth","Leaf Tornado","Leech Seed","Mega Drain","Slam","Leaf Blade","Coil","Giga Drain","Wring Out","Gastro Acid","Leaf Storm"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(498, "Tepig", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Thick Fat",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 9.9f,
		                28,PokemonData.LevelingRate.MEDIUMSLOW, 1,0,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Fire Pig", "It loves to eat roasted berries, but sometimes it gets too excited and burns them to a crisp.",
		                65,63,45,45,45,45, 0, Color.clear,
		                new int[]{   1,       3,          7,      9,            13,            15,            19,    21,       25,         27,          31,         33,            37,          39,    43},
						new string[]{"Tackle","Tail Whip","Ember","Odor Sleuth","Defense Crul","Flame Charge","Smog","Rollout","Take Down","Heat Crash","Assurance","Flamethrower","Head Smash","Roar","Flare Blitz"},
						new string[]{},
						new int[]{499}, new string[]{"Level,17"}),
		new PokemonData(499, "Pignite", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Thick Fat",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 55.5f, 
		                146, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Fire Pig", "The more it eats, the more fuel it has to make the fire in its stomach stronger. This fills it with even more power.",
		                90,93,55,70,55,55, 0, Color.clear,
		                new int[]{   1,       1,      1,          1,            3,          7,      9,            13,            15,            17,          20,    23,       28,         31,          36,         39,            44,          47,    52},
						new string[]{"Tackle","Ember","Tail Whip","Odor Sleuth","Tail Whip","Ember","Odor Sleuth","Defense Curl","Flame Charge","Arm Thrust","Smog","Rollout","Take Down","Heat Crash","Assurance","Flamethrower","Head Smash","Roar","Flare Blitz"},
						new string[]{},
						new int[]{500}, new string[]{"Level,36"}),
		new PokemonData(500, "Emboar", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Reckless",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.6f, 150,
		                238, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.RED, 70,
		                "Mega Fire Pig", "A flaring beard of fire is proof that it is fired up. It is adept at using many different moves.",
		                110,123,65,100,65,65, 0, Color.clear,
		                new int[]{   1,           1,       1,          1,      1,            3,          7,      9,            12,            15,            17,          20,    23,       28,         31,          38,         43,            50,          55,    62},
						new string[]{"Hammer Arm","Tackle","Tail Whip","Ember","Odor Sleuth","Tail Whip","Ember","Odor Sleuth","Defense Curl","Flame Charge","Arm Thrust","Smog","Rollout","Take Down","Heat Crash","Assurance","Flamethrower","Head Smash","Roar","Flare Blitz"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(501, "Oshawott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 5.9f,
		                28, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Sea Otter", "The scalchop on its stomach is made from the same element as claws. It detaches the scalchop for use as a blade.",
		                55,55,45,63,45,45, 0, Color.clear,
		                new int[]{   1,       5,          7,          11,           13,            17,           19,           23,           25,       29,        31,      35,         37,         41,            43},
						new string[]{"Tackle","Tail Whip","Water Gun","Water Sport","Focus Energy","Razor Shell","Fury Cutter","Water Pulse","Revenge","Aqua Jet","Encore","Aqua Tail","Retaliate","Swords Dance","Hydro Pump"},
						new string[]{},
						new int[]{502}, new string[]{"Level,17"}),
		new PokemonData(502, "Dewott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.8f, 24.5f,
		                145, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Discipline", "As a result of strict training, each Dewott learns different forms for using the scalchops.",
		                75,75,60,83,60,60, 0, Color.clear,
		                new int[]{   1,       1,          1,          1,            5,          7,          11,           13,            17,           20,           25,           28,       33,        38,      45,         50,         57,            62},
						new string[]{"Tackle","Tail Whip","Water Gun","Water Sport","Tail Whip","Water Gun","Water Sport","Focus Energy","Razor Shell","Fury Cutter","Water Pulse","Revenge","Aqua Jet","Encore","Aqua Tail","Retaliate","Swords Dance","Hydro Pump"},
						new string[]{},
						new int[]{503}, new string[]{"Level,17"}),
		new PokemonData(503, "Samurott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.5f, 94.6f,
		                238, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Formidable", "One swing of the sword incorporated in its armor can fell an opponent. A simple glare from one of them quiets everybody.",
		                95,100,85,108,70,70, 0, Color.clear,
		                new int[]{1,1,1,1,1,5,7,11,13,17,20,25,28,33,36,38,45,50,57,62},
						new string[]{"Megahorn","Tackle","Tail Whip","Water Gun","Water Sport","Focus Energy","Razor Shell","Fury Cutter","Water Pulse","Revenge","Aqua Jet","Slash","Encore","Aqua Tail","Retaliate","Swords Dance","Hydro Pump"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(519, "Pidove", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck", "Rivalry",
		                50, 255, PokemonData.EggGroup.FLYING,PokemonData.EggGroup.NONE, 4080, 0.3f, 2.1f, 
		                53, PokemonData.LevelingRate.MEDIUMSLOW, 0,1,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Tiny Pigeon", "This very forgetful Pokémon will wait for a new order from its Trainer even though it already has one.",
		                50,55,50,36,30,43, 0, Color.clear,
		                new int[]{   1,     4,      8,     11,            15,          18,     22,      25,     29,         32,          36,             39,       43,      46,        50},
						new string[]{"Gust","Growl","Leer","Quick Attack","Air Cutter","Roost","Detect","Taunt","Air Slash","Razor Wind","Feather Dance","Swagger","Facade","Tailwind","Sky Attack"},
						new string[]{},
						new int[]{520}, new string[]{"Level,21"}),
		new PokemonData(520, "Tranquill", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck", "Rivalry",
		                50, 120, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.6f, 15,
		                125, PokemonData.LevelingRate.MEDIUMSLOW, 0,2,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Wild Pigeon", "No matter where in the world it goes, it knows where its nest is, so it never gets separated from its Trainer.",
		                62,77,62,50,42,65, 0, Color.clear,
		                new int[]{   1,     1,      1,     1,             4,      8,     11,            15,          18,     23,      27,     32,         36,          41,             45,       50,      54,        59},
						new string[]{"Gust","Growl","Leer","Quick Attack","Growl","Leer","Quick Attack","Air Cutter","Roost","Detect","Taunt","Air Slash","Razor Wind","Feather Dance","Swagger","Facade","Tailwind","Sky Attack"},
						new string[]{},
						new int[]{521}, new string[]{"Level,32"}),
		new PokemonData(521, "Unfezant", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck", "Rivalry",
		                50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.2f, 29, 
		                215, PokemonData.LevelingRate.MEDIUMSLOW, 0,3,0,0,0,0, PokemonData.PokedexColor.GRAY, 70,
		                "Proud", "Males swing their head plumage to threaten opponents. The females' flying abilities surpass those of the males.",
		                80,115,80,65,55,93, 0, Color.clear,
		                new int[]{   1,     1,      1,     1,             4,      8,     11,            15,          18,     23,      27,     33,         38,          44,             49,       55,      60,        66},
						new string[]{"Gust","Growl","Leer","Quick Attack","Growl","Leer","Quick Attack","Air Cutter","Roost","Detect","Taunt","Air Slash","Razor Wind","Feather Dance","Swagger","Facade","Tailwind","Sky Attack"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(580, "Ducklett", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", "Big Pecks", "Hydration",
		                50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 0.5f, 5.5f, 
		                61, PokemonData.LevelingRate.MEDIUMFAST, 1,0,0,0,0,0, PokemonData.PokedexColor.BLUE, 70,
		                "Water Bird", "They are better at swimming than flying, and they happily eat their favorite food, peat moss, as they dive underwater",
		                62,44,50,44,50,55, 0, Color.clear,
		                new int[]{   1,          3,            6,      9,            13,           15,          19,           21,             24,         27,         30,     34,          37,        41,          46},
						new string[]{"Water Gun","Water Sport","Defog","Wing Attack","Water Pulse","Aerial Ace","Bubble Beam","Feather Dance","Aqua Ring","Air Slash","Roost","Rain Dance","Tailwind","Brave Bird","Hurricane"},
						new string[]{},
						new int[]{581}, new string[]{"Level,35"}),
		new PokemonData(581, "Swanna", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", "Big Pecks", "Hydration",
		                50, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 1.3f, 24.2f,
		                166, PokemonData.LevelingRate.MEDIUMFAST, 0,0,0,0,0,2, PokemonData.PokedexColor.WHITE, 70,
		                "White Bird", "Swanna start to dance at dusk. The one dancing in the middle is the leader of the flock.",
		                75,87,63,87,63,98, 0, Color.clear,
		                new int[]{   1,          1,            1,      1,            3,            6,      9,            13,           15,          19,           21,             24,         27,         30,     34,          40,        47,          55},
						new string[]{"Water Gun","Water Sport","Defog","Wing Attack","Water Sport","Defog","Wing Attack","Water Pulse","Aerial Ace","Bubble Beam","Feather Dance","Aqua Ring","Air Slash","Roost","Rain Dance","Tailwind","Brave Bird","Hurricane"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(607, "Litwick", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body", "Infiltrator",
		                50, 190, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.3f, 3.1f,
		                55, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.WHITE, 70,
		                "Candle", "Its flame is usually out, but it starts shining when it absorbs life force from people or Pokémon.",
		                50,30,55,65,55,20, 0.3f, new Color(0.62f,0.37f,1,1),
		                new int[]{   1,      1,         3,         5,     7,          10,           13,           16,           20,           24,         28,   33,       38,       43,     49,           55,          61},
						new string[]{"Ember","Astonish","Minimize","Smog","Fire Spin","Confuse Ray","Night Shade","Will-O-Wisp","Flame Burst","Imprision","Hex","Memento","Inferno","Curse","Shadow Ball","Pain Split","Overheat"},
						new string[]{},
						new int[]{608}, new string[]{"Level,41"}),
		new PokemonData(608, "Lampent", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body", "Infiltrator",
		                50, 90, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.6f, 13, 
		                130, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,0,0, PokemonData.PokedexColor.BLACK, 70,
		                "Lamp", "The spirits it absorbs fuel its baleful fire. It hangs around hospitals waiting for people to pass on.",
		                60,40,60,95,60,55, 0.4f, new Color(0.62f,0.37f,1,1),
                        new int[]{   1,      1,         1,         1,     3,         5,     7,          10,           13,           16,           20,           24,        28,   33,       38,       45,     53,           61,          69},
						new string[]{"Ember","Astonish","Minimize","Smog","Minimize","Smog","Fire Spin","Confuse Ray","Night Shade","Will-O-Wisp","Flame Burst","Imprison","Hex","Memento","Inferno","Curse","Shadow Ball","Pain Split","Overheat"},
						new string[]{},
						new int[]{609}, new string[]{"Stone,Dusk Stone"}),
		new PokemonData(609, "Chandelure", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body", "Infiltrator",
		                50, 45, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 1, 34.3f,
		                234, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.BLACK, 70,
		                "Luring", "Being consumed in Chandelure's flame burns up the spirit, leaving the body behind.",
		                60,55,90,145,90,80, 0.55f, new Color(0.62f,0.37f,1,1),
                        new int[]{   1,           1,     1,            1,            1},
						new string[]{"Pain Split","Smog","Confuse Ray","Flame Burst","Hex"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(650, "Chespin", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Bulletproof",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 9,
		                63, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,1,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Spiny Nut", "Such a thick shell of wood covers its head and back that even a direct hit from a truck wouldn't faze it.",
		                56,61,65,48,45,38, 0, Color.clear,
		                new int[]{   1,       1,      5,          8,        11,    15,          18,           27,         32,         35,        39,       42,         45,          48},
						new string[]{"Tackle","Growl","Vine Whip","Rollout","Bite","Leech Seed","Pin Missile","Take Down","Seed Bomb","Mud Shot","Bulk Up","Body Slam","Pain Split","Wood Hammer"},
						new string[]{},
						new int[]{651}, new string[]{"Level,16"}),
		new PokemonData(651, "Quilladin", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Bulletproof",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.7f, 29,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,2,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Spiny Armor", "They strengthen their lower bodies by running into one another. They are very kind and won't start fights.",
		                61,78,95,56,58,57, 0, Color.clear,
		                new int[]{   1,       1,      5,          8,        11,    15,          20,           26,          30,         35,         39,        44,       48,         52,          55},
						new string[]{"Tackle","Growl","Vine Whip","Rollout","Bite","Leech Seed","Pin Missile","Needle Arm","Take Down","Seed Bomb","Mud Shot","Bulk Up","Body Slam","Pain Split","Wood Hammer"},
						new string[]{},
						new int[]{652}, new string[]{"Level,36"}),
		new PokemonData(652, "Chesnaught", PokemonData.Type.GRASS, PokemonData.Type.FIGHTING, "Overgrow", null, "Bulletproof",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.6f, 90,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,3,0,0,0, PokemonData.PokedexColor.GREEN, 70,
		                "Spiny Armor", "When it takes a defensive posture with its fists guarding its face, it could withstand a bomb blast.",
		                88,107,122,74,75,64, 0, Color.clear,
		                new int[]{   1,      1,           1,           1,       1,      5,          8,        11,    15,          20,           26,          30,         35,         36,            41,        44,       48,         52,          55,           60,          70,           75},
						new string[]{"Feint","Hammer Arm","Belly Drum","Tackle","Growl","Vine Whip","Rollout","Bite","Leech Seed","Pin Missile","Needle Arm","Take Down","Seed Bomb","Spiky Shield","Mud Shot","Bulk Up","Body Slam","Pain Split","Wood Hammer","Hammer Arm","Giga Impact","Spiky Shield"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(653, "Fennekin", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Magician",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 9.4f,
		                61, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,1,0,0, PokemonData.PokedexColor.RED, 70,
		                "Fox", "As it walks, it munches on a twig in place of a snack. It intimidates opponents by puffing hot air out of its ears.",
		                40,45,40,62,60,60, 0, Color.clear,
		                new int[]{   1,        1,          5,      11,    14,            17,       20,         25,           27,            31,        35,            38,           41,       43,         46,          48},
						new string[]{"Scratch","Tail Whip","Ember","Howl","Flame Charge","Psybeam","Fire Spin","Lucky Chant","Light Screen","Psyshock","Flamethrower","Will-O-Wisp","Psychic","Sunny Day","Magic Room","Fire Blast"},
						new string[]{},
						new int[]{654}, new string[]{"Level,16"}),
		new PokemonData(654, "Braixen", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Magician",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 14.5f,
		                143, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,2,0,0, PokemonData.PokedexColor.RED, 70,
		                "Fox", "It has a twig stuck in its tail. With friction from its tail fur, it sets the twig on fire and launches into battle.",
		                59,59,58,90,70,73, 0, Color.clear,
		                new int[]{   1,        1,          5,      11,    14,            18,       22,         27,           30,            34,        41,            45,           48,       51,         53,          55},
						new string[]{"Scratch","Tail Whip","Ember","Howl","Flame Charge","Psybeam","Fire Spin","Lucky Chant","Light Screen","Psyshock","Flamethrower","Will-O-Wisp","Psychic","Sunny Day","Magic Room","Fire Blast"},
						new string[]{},
						new int[]{655}, new string[]{"Level,36"}),
		new PokemonData(655, "Delphox", PokemonData.Type.FIRE, PokemonData.Type.PSYCHIC, "Blaze", null, "Magician",
		                87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.5f, 39,
		                240, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,3,0,0, PokemonData.PokedexColor.RED, 70,
		                "Fox", "It gazes into the flame at the tip of its branch to achieve a focused state, which allows it to see into the future.",
		                75,69,72,114,100,104, 0, Color.clear,
		                new int[]{   1,             1,          1,           1,            1,        1,         5,      11,    14,            18,       22,         27,           30,            34,        36,             42,            47,           51,       55,         58,          61,          69,            75},
						new string[]{"Future Sight","Role Play","Switcheroo","Shadow Ball","Scratch","Tailwhip","Ember","Howl","Flame Charge","Psybeam","Fire Spin","Lucky Chant","Light Screen","Psyshock","Mystical Fire","Flamethrower","Will-O-Wisp","Psychic","Sunny Day","Magic Room","Fire Blast","Future Sight","Mystical Fire"},
						new string[]{},
						new int[]{}, new string[]{}),
		new PokemonData(656, "Froakie", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Protean",
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 0.3f, 7,
		                63, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,1, PokemonData.PokedexColor.BLUE, 70,
		                "Bubble Frog", "It secretes flexible bubbles from its chest and back. The bubbles reduce the damage it would otherwise take when attacked.",
		                41,56,40,62,44,71, 0, Color.clear,
		                new int[]{   1,      1,      5,       8,             10,    14,           18,           21,     25,     29,          35,         39,      43,           48},
						new string[]{"Pound","Growl","Bubble","Quick Attack","Lick","Water Pulse","Smokescreen","Round","Fling","Smack Down","Substitue","Bounce","Double Team","Hydro Pump"},
						new string[]{},
						new int[]{657}, new string[]{"Level,16"}),
		new PokemonData(657, "Frogadier", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Protean",
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 0.6f, 10.9f,
		                142, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,2, PokemonData.PokedexColor.BLUE, 70,
		                "Bubble Frog", "It can throw bubble-covered pebbles with precise control, hitting empty cans up to a hundred feet away.",
		                54,63,52,83,56,97, 0, Color.clear,
		                new int[]{   1,      1,      5,       8,             10,    14,           20,           23,     28,     33,          38,          44,      48,           55},
						new string[]{"Pound","Growl","Bubble","Quick Attack","Lick","Water Pulse","Smokescreen","Round","Fling","Smack Down","Substitute","Bounce","Double Team","Hydro Pump"},
						new string[]{},
						new int[]{658}, new string[]{"Level,36"}),
		new PokemonData(658, "Greninja", PokemonData.Type.WATER, PokemonData.Type.DARK, "Torrent", null, "Protean",
		                87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 1.5f, 40,
		                239, PokemonData.LevelingRate.MEDIUMSLOW, 0,0,0,0,0,3, PokemonData.PokedexColor.BLUE, 70,
		                "Ninja","It appears and vanishes with a ninja’s grace. It toys with its enemies using swift movements, while slicing them with throwing stars of sharpest water.",
		                72,95,67,103,71,122, 0, Color.clear,
		                new int[]{   1,            1,          1,          1,      1,      5,       8,             10,    14,           20,           23,            28,      33,            36,              43,          49,            52,           56,    60,          70,           75},
						new string[]{"Night Slash","Role Play","Mat Block","Pound","Growl","Bubble","Quick Attack","Lick","Water Pulse","Smokescreen","Shadow Sneak","Spikes","Feint Attack","Water Shuriken","Substitute","Extrasensory","Double Team","Haze","Hydro Pump","Night Slash","Water Shuriken"},
						new string[]{},
						new int[]{}, new string[]{})

	};
	#endregion

	#region expTable
	private static int[] expTableErratic = new int[]{
		0,15,52,122,237,406,637,942,1326,1800,
		2369,3041,3822,4719,5737,6881,8155,9564,11111,12800,
		14632,16610,18737,21012,23437,26012,28737,31610,34632,37800,
		41111,44564,48155,51881,55737,59719,63822,68041,72369,76800,
		81326,85942,90637,95406,100237,105122,110052,115105,120001,125000,
		131324,137795,144410,151165,158056,165079,172229,179503,186894,194400,
		202013,209728,217540,225443,233431,241496,249633,257834,267406,276458,
		286328,296358,305767,316074,326531,336255,346965,357812,567807,378880,
		390077,400293,411686,423190,433572,445239,457001,467489,479378,491346,
		501878,513934,526049,536557,548720,560922,571333,583539,591882,600000};
=======
public static class PokemonDatabase
{
    private static PokemonData[] pokedex = new PokemonData[]
    {
        null,
        //  PokemonData(ID, NAME, PokemonData.Type.TYPE1, PokemonData.Type.TYPE1, Ability1, Ability2, HiddenAbility,
        //				MaleRatio, CatchRate, PokemonData.EggGroup.EGGGROUP1, PokemonData.EggGroup.EGGGROUP2, HatchTime, Height, Weight,
        //				EXPYield, PokemonData.LevelingRate.LEVELINGRATE, evYieldHP,ATK,DEF,SPA,SPD,SPE, PokemonData.PokedexColor.COLOR, BaseFriendship,
        //				Species, PokedexEntry (choose your favourite)
        //				baseStatsHP,ATK,DEF,SPA,SPD,SPE, Luminance (0 if unknown), LightColor (Color.clear if unknown)
        //				new int[]{ level, level, level, etc...}
        //				new string[]{ "move", "move", "move", etc...} ),
        //				new int[]{pokemonID}, new string[]{"Method,Parameter"}),
        new PokemonData(1, "Bulbasaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.7f, 6.9f,
            64, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Seed", "For some time after its birth, it grows by gaining nourishment from the seed on its back.",
            45, 49, 49, 65, 65, 45, 0, Color.clear,
            new int[] {1, 3, 7, 9, 13, 13, 15, 19, 21, 25, 27, 31, 33, 37},
            new string[]
            {
                "Tackle", "Growl", "Leech Seed", "Vine Whip", "Poison Powder", "Sleep Powder", "Take Down",
                "Razor Leaf", "Sweet Scent", "Growth", "Double-Edge", "Worry Seed", "Synthesis", "Seed Bomb"
            },
            new string[] {},
            new int[] {2}, new string[] {"Level,16"}),
        new PokemonData(2, "Ivysaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1, 13,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 1, 0, PokemonData.PokedexColor.GREEN, 70,
            "Seed",
            "There is a bud on this Pokémon’s back. To support its weight, Ivysaur’s legs and trunk grow thick and strong. If it starts spending more time lying in the sunlight, it’s a sign that the bud will bloom into a large flower soon.",
            60, 62, 63, 80, 80, 60, 0, Color.clear,
            new int[] {1, 1, 1, 3, 7, 9, 13, 13, 15, 20, 23, 28, 31, 36, 39, 44},
            new string[]
            {
                "Tackle", "Growl", "Leech Seed", "Growl", "Leech Seed", "Vine Whip", "Poison Powder", "Sleep Powder",
                "Take Down", "Razor Leaf", "Sweet Scent", "Growth", "Double-Edge", "Worry Seed", "Synthesis",
                "Solar Beam"
            },
            new string[] {},
            new int[] {3}, new string[] {"Level,32"}),
        new PokemonData(3, "Venusaur", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Overgrow", null, "Chlorophyll",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5335, 2, 100,
            236, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 1, 0, PokemonData.PokedexColor.GREEN, 70,
            "Seed",
            "There is a large flower on Venusaur’s back. The flower is said to take on vivid colors if it gets plenty of nutrition and sunlight. The flower’s aroma soothes the emotions of people.",
            80, 82, 83, 100, 100, 80, 0, Color.clear,
            new int[] {1, 1, 1, 1, 3, 7, 9, 13, 13, 15, 20, 23, 28, 31, 32, 39, 45, 50, 53},
            new string[]
            {
                "Tackle", "Growl", "Leech Seed", "Vine Whip", "Growl", "Vine Whip", "Leech Seed", "Poison Powder",
                "Sleep Powder", "Take Down", "Razor Leaf", "Sweet Scent", "Growth", "Double-Edge", "Petal Dance",
                "Worry Seed", "Synthesis", "Petal Blizzard", "Solar Beam"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(4, "Charmander", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Solar Power",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.6f, 8.5f,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.RED, 70,
            "Lizard",
            "The flame on its tail shows the strength of its life force. If it is weak, the flame also burns weakly.",
            39, 52, 43, 60, 50, 65, 0, Color.clear,
            new int[] {1, 1, 7, 10, 16, 19, 25, 28, 34, 37, 43, 46},
            new string[]
            {
                "Scratch", "Growl", "Ember", "Smokescreen", "Dragon Rage", "Scary Face", "Fire Fang", "Flame Burst",
                "Slash", "Flamethrower", "Fire Spin", "Inferno"
            },
            new string[] {},
            new int[] {5}, new string[] {"Level,16"}),
        new PokemonData(5, "Charmeleon", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Solar Power",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.1f, 19,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 1, PokemonData.PokedexColor.RED, 70,
            "Flame",
            "Charmeleon mercilessly destroys its foes using its sharp claws. If it encounters a strong foe, it turns aggressive. In this excited state, the flame at the tip of its tail flares with a bluish white color.",
            58, 64, 58, 80, 65, 80, 0, Color.clear,
            new int[] {1, 1, 1, 7, 10, 17, 21, 28, 32, 39, 43, 50, 54},
            new string[]
            {
                "Scratch", "Growl", "Ember", "Ember", "Smokescreen", "Dragon Rage", "Scary Face", "Fire Fang",
                "Flame Burst", "Slash", "Flamethrower", "Fire Spin", "Inferno"
            },
            new string[] {},
            new int[] {6}, new string[] {"Level,36"}),
        new PokemonData(6, "Charizard", PokemonData.Type.FIRE, PokemonData.Type.FLYING, "Blaze", null, "Solar Power",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.7f, 90.5f,
            240, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Flame",
            "When expelling a blast of superhot fire, the red flame at the tip of its tail burns more intensely.",
            78, 84, 78, 109, 85, 100, 0.4f, new Color(1, 0.5f, 0, 1),
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 7, 10, 17, 21, 28, 32, 36, 41, 47, 56, 62, 71, 77},
            new string[]
            {
                "Flare Blitz", "Heat Wave", "Dragon Claw", "Air Slash", "Shadow Claw", "Scratch", "Ember", "Growl",
                "SmokeScreen", "Ember", "Smokescreen", "Dragon Rage", "Scary Face", "Fire Fang", "Flame Burst",
                "Wing Attack", "Slash", "Flamethrower", "Fire Spin", "Inferno", "Heat Wave", "Flare Blitz"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(7, "Squirtle", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.5f, 9,
            63, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 1, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Tiny Turtle",
            "Squirtle's shell is not merely used for protection. The shell's rounded shape and the grooves on its surface help minimize resistance in water, enabling this Pokémon to swim at high speeds",
            44, 48, 65, 50, 64, 43, 0, Color.clear,
            new int[] {1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40},
            new string[]
            {
                "Tackle", "Tail Whip", "Water Gun", "Withdraw", "Bubble", "Bite", "Rapid Spin", "Protect",
                "Water Pulse", "Aqua Tail", "Skull Bash", "Iron Defence", "Rain Dance", "Hydro Pump"
            },
            new string[] {},
            new int[] {8}, new string[] {"Level,16"}),
        new PokemonData(8, "Wartortle", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1, 22.5f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 1, 0, 1, 0, PokemonData.PokedexColor.BLUE, 70,
            "Turtle",
            "Its tail is large and covered with a rich, thick fur. The tail becomes increasingly deeper in color as Wartortle ages. The scratches on its shell are evidence of this Pokémon’s toughness as a battler.",
            59, 63, 80, 65, 80, 58, 0, Color.clear,
            new int[] {1, 1, 1, 4, 7, 10, 13, 16, 20, 24, 28, 32, 36, 40, 44, 48},
            new string[]
            {
                "Tackle", "Tail Whip", "Water Gun", "Tail Whip", "Water Gun", "Withdraw", "Bubble", "Bite",
                "Rapid Spin", "Protect", "Water Pulse", "Aqua Tail", "Skull Bash", "Iron Defence", "Rain Dance",
                "Hydro Pump"
            },
            new string[] {},
            new int[] {9}, new string[] {"Level,36"}),
        new PokemonData(9, "Blastoise", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Rain Dish",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.6f, 85.5f,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 3, 0, PokemonData.PokedexColor.BLUE, 70,
            "Shellfish",
            "The jets of water it spouts from the rocket cannons on its shell can punch through thick steel",
            79, 83, 100, 85, 105, 78, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 4, 7, 10, 13, 16, 20, 24, 28, 32, 39, 46, 53, 60},
            new string[]
            {
                "Flash Cannon", "Tackle", "Tail Whip", "Water Gun", "Withdraw", "Tail Whip", "Water Gun", "Withdraw",
                "Bubble", "Bite", "Rapid Spin", "Protect", "Water Pulse", "Aqua Tail", "Skull Bash", "Iron Defence",
                "Rain Dance", "Hydro Pump"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(10, "Caterpie", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shield Dust", null, "Run Away",
            50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.3f, 2.9f,
            39, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Worm",
            "Caterpie has a voracious appetite. It can devour leaves bigger than its body right before your eyes. From its antenna, this Pokémon releases a terrifically strong odor.",
            45, 30, 35, 20, 20, 45, 0, Color.clear,
            new int[] {1, 1, 15},
            new string[] {"Tackle", "String Shot", "Bug Bite"},
            new string[] {},
            new int[] {11}, new string[] {"Level,7"}),
        new PokemonData(11, "Metapod", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
            50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.7f, 9.9f,
            72, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Cocoon",
            "The shell covering this Pokémon’s body is as hard as an iron slab. Metapod does not move very much. It stays still because it is preparing its soft innards for evolution inside the hard shell.",
            50, 20, 55, 25, 25, 30, 0, Color.clear,
            new int[] {1, 7},
            new string[] {"Harden", "Harden"},
            new string[] {},
            new int[] {12}, new string[] {"Level,10"}),
        new PokemonData(12, "Butterfree", PokemonData.Type.BUG, PokemonData.Type.FLYING, "Compound Eyes", null,
            "Tinted Lens",
            50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 1.1f, 32,
            173, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 2, 1, 0, PokemonData.PokedexColor.WHITE, 70,
            "Butterfly", "Its wings, covered with poisonous powders, repel water. This allows it to fly in the rain.",
            60, 45, 50, 90, 80, 70, 0, Color.clear,
            new int[] {1, 10, 12, 12, 12, 16, 18, 22, 24, 28, 30, 34, 36, 40, 42, 46},
            new string[]
            {
                "Confusion", "Confusion", "Poison Powder", "Stun Spore", "Sleep Powder", "Gust", "Supersonic",
                "Whirlwind", "Psybeam", "Silver Wind", "Tailwind", "Rage Powder", "Safe Guard", "Captivate", "Bug Buzz",
                "Quiver Dance"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(13, "Weedle", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shield Dust", null, "Run Away",
            50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.3f, 3.2f,
            39, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Hairy Bug",
            "Its poison stinger is very powerful. Its bright-colored body is intended to warn off its enemies.",
            40, 35, 30, 20, 20, 50, 0, Color.clear,
            new int[] {1, 1, 15},
            new string[] {"Poison Sting", "String Shot", "Bug Bite"},
            new string[] {},
            new int[] {14}, new string[] {"Level,7"}),
        new PokemonData(14, "Kakuna", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shed Skin", null, null,
            50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 0.6f, 10,
            72, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Cocoon", "Able to move only slightly. When endangered, it may stick out its stinger and poison its enemy.",
            45, 25, 50, 25, 25, 35, 0, Color.clear,
            new int[] {1, 7},
            new string[] {"Harden", "Harden"},
            new string[] {},
            new int[] {15}, new string[] {"Level,10"}),
        new PokemonData(15, "Beedrill", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", null, "Sniper",
            50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 2805, 1, 29.5f,
            173, PokemonData.LevelingRate.MEDIUMFAST, 0, 2, 0, 0, 1, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Poison Bee",
            "Beedrill is extremely territorial. No one should ever approach its nest—this is for their own safety. If angered, they will attack in a furious swarm.",
            65, 90, 40, 45, 80, 75, 0, Color.clear,
            new int[] {1, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 45},
            new string[]
            {
                "Fury Attack", "Fury Attack", "Focus Energy", "Twineedle", "Rage", "Pursuit", "Toxic Spikes",
                "Pin Missile", "Agility", "Assurance", "Poison Jab", "Endeavor", "Fell Stinger"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(16, "Pidgey", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet",
            "Big Pecks",
            50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 1.8f,
            50, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Ting Bird",
            "Very docile. If attacked, it will often kick up sand to protect itself rather than fight back.",
            40, 45, 40, 35, 35, 56, 0, Color.clear,
            new int[] {1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45, 49, 53},
            new string[]
            {
                "Tackle", "Sand Attack", "Gust", "Quick Attack", "Whirlwind", "Twister", "Feather Dance", "Agility",
                "Wing Attack", "Roost", "Tailwind", "Mirror Move", "Air Slash", "Hurricane"
            },
            new string[] {},
            new int[] {17}, new string[] {"Level,18"}),
        new PokemonData(17, "Pidgeotto", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet",
            "Big Pecks",
            50, 120, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.1f, 30,
            122, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BROWN, 70,
            "Bird",
            "Pidgeotto claims a large area as its own territory. This Pokémon flies around, patrolling its living space. If its territory is violated, it shows no mercy in thoroughly punishing the foe with its sharp claws.",
            63, 60, 55, 50, 50, 71, 0, Color.clear,
            new int[] {1, 1, 1, 5, 9, 13, 17, 22, 27, 32, 37, 42, 47, 52, 57, 62},
            new string[]
            {
                "Tackle", "Sand Attack", "Gust", "Sand Attack", "Gust", "Quick Attack", "Whirlwind", "Twister",
                "Feather Dance", "Agility", "Wing Attack", "Roost", "Tailwind", "Mirror Move", "Air Slash", "Hurricane"
            },
            new string[] {},
            new int[] {18}, new string[] {"Level,36"}),
        new PokemonData(18, "Pidgeot", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", "Tangled Feet",
            "Big Pecks",
            50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.5f, 39.5f,
            211, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 3, PokemonData.PokedexColor.BROWN, 70,
            "Bird",
            "This Pokémon has a dazzling plumage of beautifully glossy feathers. Many Trainers are captivated by the striking beauty of the feathers on its head, compelling them to choose Pidgeot as their Pokémon.",
            83, 80, 75, 70, 70, 101, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 9, 13, 17, 22, 27, 32, 38, 44, 50, 56, 62, 68},
            new string[]
            {
                "Hurricane", "Tackle", "Sand Attack", "Gust", "Quick Attack", "Sand Attack", "Gust", "Quick Attack",
                "Whirlwind", "Twister", "Feather Dance", "Agility", "Wing Attack", "Roost", "Tailwind", "Mirror Move",
                "Air Slash", "Hurricane"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(21, "Spearow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", null, "Sniper",
            50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 2,
            52, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Tiny Bird",
            "Very protective of its territory, it flaps its short wings busily to dart around at high speed.",
            40, 60, 30, 31, 31, 70, 0, Color.clear,
            new int[] {1, 1, 5, 9, 13, 17, 21, 25, 29, 33, 37},
            new string[]
            {
                "Peck", "Growl", "Leer", "Fury Attack", "Pursuit", "Aerial Ace", "Mirror Move", "Agility", "Assurance",
                "Roost", "Drill Peck"
            },
            new string[] {},
            new int[] {22}, new string[] {"Level,20"}),
        new PokemonData(22, "Fearow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Keen Eye", null, "Sniper",
            50, 90, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.2f, 38,
            155, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BROWN, 70,
            "Beak",
            "Fearow is recognized by its long neck and elongated beak. They are conveniently shaped for catching prey in soil or water. It deftly moves its long and skinny beak to pluck prey.",
            65, 90, 65, 61, 61, 100, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 5, 9, 13, 17, 23, 29, 35, 41, 47, 53},
            new string[]
            {
                "Drill Run", "Pluck", "Peck", "Growl", "Leer", "Fury Attack", "Leer", "Fury Attack", "Pursuit",
                "Aerial Ace", "Mirror Move", "Agility", "Assurance", "Roost", "Drill Peck", "Drill Run"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(32, "Nidoran♂", PokemonData.Type.POISON, PokemonData.Type.NONE, "Poison Point", "Rivalry",
            "Hustle",
            100, 235, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 0.5f, 9,
            55, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.PURPLE, 70,
            "Poison Pin",
            "The male Nidoran has developed muscles for moving its ears. Thanks to them, the ears can be freely moved in any direction. Even the slightest sound does not escape this Pokémon's notice.",
            46, 57, 40, 40, 40, 50, 0, Color.clear,
            new int[] {1, 1, 7, 9, 13, 19, 21, 25, 31, 33, 37, 43, 45},
            new string[]
            {
                "Leer", "Peck", "Focus Energy", "Double Kick", "Poison Sting", "Fury Attack", "Horn Attack",
                "Helping Hand", "Toxic Spikes", "Flatter", "Poison Jab", "Captivate", "Horn Drill"
            },
            new string[] {},
            new int[] {33}, new string[] {"Level,16"}),
        new PokemonData(33, "Nidorino", PokemonData.Type.POISON, PokemonData.Type.NONE, "Poison Point", "Rivalry",
            "Hustle",
            100, 120, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 0.9f, 19.5f,
            128, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.PURPLE, 70,
            "Poison Pin",
            "ts horn is harder than diamond. If it punctures an enemy,it pumps powerful venom into the wound.",
            61, 72, 57, 55, 55, 65, 0, Color.clear,
            new int[] {1, 1, 7, 9, 13, 20, 23, 28, 35, 38, 43, 50, 58},
            new string[]
            {
                "Leer", "Peck", "Focus Energy", "Double Kick", "Poison Sting", "Fury Attack", "Horn Attack",
                "Helping Hand", "Toxic Spikes", "Flatter", "Poison Jab", "Captivate", "Horn Drill"
            },
            new string[] {},
            new int[] {34}, new string[] {"Stone,Moon Stone"}),
        new PokemonData(34, "Nidoking", PokemonData.Type.POISON, PokemonData.Type.GROUND, "Poison Point", "Rivalry",
            "Sheer Force",
            100, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.FIELD, 5355, 1.4f, 62,
            223, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.PURPLE, 70,
            "Drill",
            "Its tail is thick and powerful. If it binds an enemy, it can snap the victim's spine quite easily.",
            81, 102, 77, 85, 75, 85, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 23, 35, 43, 58},
            new string[]
            {
                "Megahorn", "Peck", "Focus Energy", "Double Kick", "Poison Sting", "Chip Away", "Thrash", "Earth Power",
                "Megahorn"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(56, "Mankey", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Vital Spirit", "Anger Point",
            "Defiant",
            50, 190, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 28,
            61, PokemonData.LevelingRate.MEDIUMFAST, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Pig Monkey",
            "It lives in groups in the treetops. If it loses sight of its group, it becomes infuriated by its loneliness.",
            40, 80, 35, 35, 45, 70, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 9, 13, 17, 21, 25, 33, 37, 41, 45, 49, 53},
            new string[]
            {
                "Covet", "Scratch", "Low Kick", "Leer", "Focus Energy", "Fury Swipes", "Karate Chop", "Seismic Toss",
                "Screech", "Assurance", "Swagger", "Cross Chop", "Thrash", "Punishment", "Close Combat", "Final Gambit"
            },
            new string[] {},
            new int[] {57}, new string[] {"Level,28"}),
        new PokemonData(57, "Primeape", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Vital Spirit", "Anger Point",
            "Defiant",
            50, 75, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 32,
            159, PokemonData.LevelingRate.MEDIUMFAST, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Pig Monkey",
            "It grows angry if you see its eyes and gets angrier if you run. If you beat it, it gets even madder.",
            65, 105, 60, 60, 70, 95, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 9, 13, 17, 21, 25, 28, 35, 41, 47, 53, 59, 63},
            new string[]
            {
                "Final Gambit", "Fling", "Scratch", "Low Kick", "Leer", "Focus Energy", "Fury Swipes", "Karate Chop",
                "Seismic Toss", "Screech", "Assurance", "Rage", "Swagger", "Cross Chop", "Thrash", "Punishment",
                "Close Combat", "Final Gambit"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(66, "Machop", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
            75, 180, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 5355, 0.8f, 19.5f,
            61, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Superpower",
            "Machop exercises by hefting around a Graveler as if it were a barbell. There are some Machop that travel the world in a quest to master all kinds of martial arts.",
            70, 80, 50, 35, 35, 35, 0, Color.clear,
            new int[] {1, 1, 3, 7, 9, 13, 15, 19, 21, 25, 27, 31, 33, 37, 39, 43, 45},
            new string[]
            {
                "Low Kick", "Leer", "Focus Energy", "Karate Chop", "Foresight", "Low Sweep", "Seismic Toss", "Revenge",
                "Knock Off", "Vital Throw", "Wake-Up Slap", "Dual Chop", "Submission", "Bulk Up", "Cross Chop",
                "Scary Face", "Dynamic Punch"
            },
            new string[] {},
            new int[] {67}, new string[] {"Level,28"}),
        new PokemonData(67, "Machoke", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
            75, 90, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 5355, 1.5f, 70.5f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Superpower", "Machoke's boundless power is very dangerous, so it wears a belt that suppresses its energy.",
            80, 100, 70, 50, 60, 45, 0, Color.clear,
            new int[] {1, 1, 1, 1, 3, 7, 9, 13, 15, 19, 21, 25, 27, 33, 37, 43, 47, 53, 57},
            new string[]
            {
                "Low Kick", "Leer", "Focus Energy", "Karate Chop", "Focus Energy", "Karate Chop", "Foresight",
                "Low Sweep", "Seismic Toss", "Revenge", "Knock Off", "Vital Throw", "Wake-Up Slap", "Dual Chop",
                "Submission", "Bulk Up", "Cross Chop", "Scary Face", "Dynamic Punch"
            },
            new string[] {},
            new int[] {68}, new string[] {"Trade"}),
        new PokemonData(68, "Machamp", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Guts", "No Guard", "Steadfast",
            75, 45, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 5355, 1.6f, 130,
            227, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Superpower",
            "Machamp has the power to hurl anything aside. However, trying to do any work requiring care and dexterity causes its arms to get tangled. This Pokémon tends to leap into action before it thinks.",
            90, 130, 80, 65, 85, 55, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 3, 7, 9, 13, 15, 19, 21, 25, 27, 33, 37, 43, 47, 53, 57},
            new string[]
            {
                "Wide Guard", "Low Kick", "Leer", "Focus Energy", "Karate Chop", "Focus Energy", "Karate Chop",
                "Foresight", "Low Sweep", "Seismic Toss", "Revenge", "Knock Off", "Vital Throw", "Wake-Up Slap",
                "Dual Chop", "Submission", "Bulk Up", "Cross Chop", "Scary Face", "Dynamic Punch"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(126, "Magmar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null, "Vital Spirit",
            75, 45, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 6630, 1.3f, 44.5f,
            173, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Spitfire",
            "The scorching fire exhaled by Magmar forms heat waves around its body, making it hard to see the Pokémon clearly.",
            65, 95, 57, 100, 85, 93, 0, Color.clear,
            new int[] {1, 1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 36, 42, 49, 55},
            new string[]
            {
                "Smog", "Leer", "Ember", "Ember", "Smokescreen", "Feint Attack", "Fire Spin", "Clear Smog",
                "Flame Burst", "Confuse Ray", "Fire Punch", "Lava Plume", "Sunny Day", "Flamethrower", "Fire Blast"
            },
            new string[] {},
            new int[] {}, new string[] {"Trade\\Item,Magmarizer"}),
        new PokemonData(129, "Magikarp", PokemonData.Type.WATER, PokemonData.Type.NONE, "Swift Swim", null, "Rattled",
            50, 255, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.DRAGON, 1530, 0.9f, 10,
            40, PokemonData.LevelingRate.SLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.RED, 70,
            "Fish",
            "Magikarp is a pathetic excuse for a Pokémon that is only capable of flopping and splashing. This behavior prompted scientists to undertake research into it.",
            20, 10, 55, 15, 20, 80, 0, Color.clear,
            new int[] {1, 15, 30},
            new string[] {"Splash", "Tackle", "Flail"},
            new string[] {},
            new int[] {130}, new string[] {"Level,20"}),
        new PokemonData(130, "Gyarados", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Intimidate", null, "Moxie",
            50, 45, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.DRAGON, 1530, 6.5f, 235,
            189, PokemonData.LevelingRate.SLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Atrocious",
            "Once Gyarados goes on a rampage, its ferociously violent blood doesn't calm until it has burned everything down. There are records of this Pokémon's rampages lasting a whole month.",
            95, 125, 79, 60, 100, 81, 0, Color.clear,
            new int[] {1, 20, 23, 26, 29, 32, 35, 38, 41, 44, 47, 50},
            new string[]
            {
                "Thrash", "Bite", "Dragon Rage", "Leer", "Twister", "Ice Fang", "Aqua Tail", "Rain Dance", "Crunch",
                "Hydro Pump", "Dragon Dance", "Hyper Beam"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(152, "Chikorita", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.9f, 6.4f,
            64, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 1, 0, PokemonData.PokedexColor.GREEN, 70,
            "Leaf",
            "In battle, Chikorita waves its leaf around to keep the foe at bay. However, a sweet fragrance also wafts from the leaf, becalming the battling Pokémon and creating a cozy, friendly atmosphere all around.",
            45, 49, 65, 49, 65, 45, 0, Color.clear,
            new int[] {1, 1, 6, 9, 12, 17, 20, 23, 28, 31, 34, 39, 42, 45},
            new string[]
            {
                "Tackle", "Growl", "Razor Leaf", "Poison Powder", "Synthesis", "Reflect", "Magical Leaf",
                "Natural Gift", "Sweet Scent", "Light Screen", "Body Slam", "Safeguard", "Aromatherapy", "Solar Beam"
            },
            new string[] {},
            new int[] {153}, new string[] {"Level,16"}),
        new PokemonData(153, "Bayleef", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.2f, 15.8f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 1, 0, 1, 0, PokemonData.PokedexColor.GREEN, 70,
            "Leaf",
            "Bayleef's neck is ringed by curled-up leaves. Inside each tubular leaf is a small shoot of a tree. The fragrance of this shoot makes people peppy.",
            60, 62, 80, 63, 80, 60, 0, Color.clear,
            new int[] {1, 1, 1, 1, 6, 9, 12, 18, 22, 26, 32, 36, 40, 46, 50, 54},
            new string[]
            {
                "Tackle", "Growl", "Razor Leaf", "Poison Powder", "Razor Leaf", "Poison Powder", "Synthesis", "Reflect",
                "Magical Leaf", "Natural Gift", "Sweet Scent", "Light Screen", "Body Slam", "Safeguard", "Aromatherapy",
                "Solar Beam"
            },
            new string[] {},
            new int[] {154}, new string[] {"Level,32"}),
        new PokemonData(154, "Meganium", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Leaf Guard",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.8f, 100.5f,
            236, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 1, 0, 2, 0, PokemonData.PokedexColor.GREEN, 70,
            "Herb", "Meganium's breath has the power to revive dead grass and plants. It can make them healthy again.",
            80, 82, 100, 83, 100, 80, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 6, 9, 12, 18, 22, 26, 32, 34, 40, 46, 54, 60, 66, 70},
            new string[]
            {
                "Petal Blizzard", "Tackle", "Growl", "Razor Leaf", "Poison Powder", "Razor Leaf", "Poison Powder",
                "Synthesis", "Reflect", "Magical Leaf", "Natural Gift", "Petal Dance", "Sweet Scent", "Light Screen",
                "Body Slam", "Safe Guard", "Aromatherapy", "Solar Beam", "Petal Blizzard"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(155, "Cyndaquil", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 7.9f,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.YELLOW, 70,
            "Fire Mouse",
            "It is timid, and always curls itself up in a ball. If attacked, it flares up its back for protection.",
            39, 52, 43, 60, 50, 65, 0, Color.clear,
            new int[] {1, 1, 6, 10, 13, 19, 22, 28, 31, 37, 40, 46, 49, 55, 58},
            new string[]
            {
                "Tackle", "Leer", "Smokescreen", "Ember", "Quick Attack", "Flame Wheel", "Defense Curl", "Flame Charge",
                "Swift", "Lava Plume", "Flamethrower", "Inferno", "Rollout", "Double-Edge", "Eruption"
            },
            new string[] {},
            new int[] {156}, new string[] {"Level,14"}),
        new PokemonData(156, "Quilava", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.9f, 19,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 1, PokemonData.PokedexColor.YELLOW, 70,
            "Volcano",
            "Quilava keeps its foes at bay with the intensity of its flames and gusts of superheated air. This Pokémon applies its outstanding nimbleness to dodge attacks even while scorching the foes with flames.",
            58, 64, 58, 80, 65, 80, 0, Color.clear,
            new int[] {1, 1, 1, 6, 10, 13, 20, 24, 31, 35, 42, 46, 53, 57, 64, 68},
            new string[]
            {
                "Tackle", "Leer", "Smokescreen", "Smokescreen", "Ember", "Quick Attack", "Flame Wheel", "Defense Curl",
                "Swift", "Flame Charge", "Lava Plume", "Flamethrower", "Inferno", "Rollout", "Double-Edge", "Eruption"
            },
            new string[] {},
            new int[] {157}, new string[] {"Level,36"}),
        new PokemonData(157, "Typhlosion", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Flash Fire",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.7f, 79.5f,
            240, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Volcano",
            "Typhlosion obscures itself behind a shimmering heat haze that it creates using its intensely hot flames. This Pokémon creates blazing explosive blasts that burn everything to cinders.",
            78, 84, 78, 109, 85, 100, 0.5f, new Color(1, 0.5f, 0, 1),
            new int[] {1, 1, 1, 1, 1, 1, 1, 6, 10, 13, 20, 24, 31, 35, 43, 48, 56, 61, 69, 74},
            new string[]
            {
                "Eruption", "Double-Edge", "Gyro ball", "Tackle", "Leer", "Smokescreen", "Ember", "Smokescreen",
                "Ember", "Quick Attack", "Flame Wheel", "Defense Curl", "Swift", "Flame Charge", "Lava Plume",
                "Flamethrower", "Inferno", "Rollout", "Double-Edge", "Eruption"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(158, "Totodile", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.6f, 9.5f,
            63, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Big Jaw",
            "Despite the smallness of its body, Totodile's jaws are very powerful. While the Pokémon may think it is just playfully nipping, its bite has enough power to cause serious injury.",
            50, 65, 64, 44, 48, 43, 0, Color.clear,
            new int[] {1, 1, 6, 8, 13, 15, 20, 22, 27, 29, 34, 36, 41, 43, 48, 50},
            new string[]
            {
                "Scratch", "Leer", "Water Gun", "Rage", "Bite", "Scary Face", "Ice Fang", "Flail", "Crunch",
                "Chip Away", "Slash", "Screech", "Thrash", "Aqua Tail", "Superpower", "Hydro Pump"
            },
            new string[] {},
            new int[] {159}, new string[] {"Level,18"}),
        new PokemonData(159, "Croconaw", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.1f, 25,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 1, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Big Jaw",
            "Once Croconaw has clamped its jaws on its foe, it will absolutely not let go. Because the tips of its fangs are forked back like barbed fishhooks, they become impossible to remove when the have sunk in.",
            65, 80, 80, 59, 63, 58, 0, Color.clear,
            new int[] {1, 1, 1, 6, 8, 13, 15, 21, 24, 30, 33, 39, 42, 48, 51, 57, 60},
            new string[]
            {
                "Scratch", "Leer", "Water Gun", "Water Gun", "Rage", "Bite", "Scary Face", "Ice Fang", "Flail",
                "Crunch", "Chip Away", "Slash", "Screech", "Thrash", "Aqua Tail", "Superpower", "Hydro Pump"
            },
            new string[] {},
            new int[] {160}, new string[] {"Level,30"}),
        new PokemonData(160, "Feraligatr", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Sheer Force",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 2.3f, 88.8f,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 1, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Big Jaw",
            "Feraligatr intimidates its foes by opening its huge mouth. In battle, it will kick the ground hard with its thick and powerful hind legs to charge at the foe at an incredible speed.",
            85, 105, 100, 79, 83, 78, 0, Color.clear,
            new int[] {1, 1, 1, 1, 6, 8, 13, 15, 21, 24, 30, 32, 37, 45, 50, 58, 63, 71, 76},
            new string[]
            {
                "Scratch", "Leer", "Water Gun", "Rage", "Water Gun", "Rage", "Bite", "Scary Face", "Ice Fang", "Flail",
                "Agility", "Crunch", "Chip Away", "Slash", "Screech", "Thrash", "Aqua Tail", "Superpower", "Hydro Pump"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(161, "Sentret", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Keen Eye", "Frisk",
            50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.8f, 6,
            43, PokemonData.LevelingRate.MEDIUMFAST, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Scout",
            "When Sentret sleeps, it does so while another stands guard. The sentry wakes the others at the first sign of danger. When this Pokémon becomes separated from its pack, it becomes incapable of sleep due to fear.",
            35, 46, 34, 35, 45, 20, 0, Color.clear,
            new int[] {1, 1, 4, 7, 13, 16, 19, 25, 28, 31, 36, 39, 42, 47},
            new string[]
            {
                "Scratch", "Foresight", "Defense Curl", "Quick Attack", "Fury Swipes", "Helping Hand", "Follow Me",
                "Slam", "Rest", "Sucker Punch", "Amnesia", "Baton Pass", "Me First", "Hyper Voice"
            },
            new string[] {},
            new int[] {162}, new string[] {"Level,15"}),
        new PokemonData(162, "Furret", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Keen Eye", "Frisk",
            50, 90, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 1.8f, 32.5f,
            145, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BROWN, 70,
            "Long Body",
            "It lives in narrow burrows that fit its slim body. The deeper the nests go, the more maze-like they become.",
            85, 76, 64, 45, 55, 90, 0, Color.clear,
            new int[] {1, 1, 1, 1, 4, 7, 13, 17, 21, 28, 32, 36, 42, 46, 50, 56},
            new string[]
            {
                "Scratch", "Foresight", "Defense Curl", "Quick Attack", "Defense Curl", "Quick Attack", "Fury Swipes",
                "Helping Hand", "Follow Me", "Slam", "Rest", "Sucker Punch", "Amnesia", "Baton Pass", "Me First",
                "Hyper Voice"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(163, "Hoothoot", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Insomnia", "Keen Eye",
            "Tinted Lens",
            50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.7f, 21.2f,
            52, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Owl", "It always stand on one foot. It changes feet so fast, the movement can rarely be seen.",
            60, 30, 30, 36, 56, 50, 0, Color.clear,
            new int[] {1, 1, 1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45, 49, 53, 57},
            new string[]
            {
                "Tackle", "Growl", "Foresight", "Hypnosis", "Peck", "Uproar", "Reflect", "Confusion", "Echoed Voice",
                "Take Down", "Air Slash", "Zen Headbutt", "Synchronoise", "Extrasensory", "Psycho Shift", "Roost",
                "Dream Eater"
            },
            new string[] {},
            new int[] {164}, new string[] {"Level,20"}),
        new PokemonData(164, "Noctowl", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Insomnia", "Keen Eye",
            "Tinted Lens",
            50, 90, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.6f, 40.8f,
            155, PokemonData.LevelingRate.MEDIUMFAST, 2, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Owl",
            "Noctowl never fails at catching prey in darkness. This Pokémon owes its success to its superior vision that allows it to see in minimal light, and to its soft, supple wings that make no sound in flight.",
            100, 50, 50, 76, 96, 70, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 5, 9, 13, 17, 22, 27, 32, 37, 42, 47, 52, 57, 62, 67},
            new string[]
            {
                "Dream Eater", "Sky Attack", "Tackle", "Growl", "Foresight", "Hypnosis", "Hypnosis", "Peck", "Uproar",
                "Reflect", "Confusion", "Echoed Voice", "Take Down", "Air Slash", "Zen Headbutt", "Synchronoise",
                "Extrasensory", "Psycho Shift", "Roost", "Dream Eater"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(167, "Spinarak", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", "Insomnia", "Sniper",
            50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.5f, 8.5f,
            50, PokemonData.LevelingRate.FAST, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "String Spit",
            "The web spun by Spinarak can be considered its second nervous system. It is said that this Pokémon can determine what kind of prey is touching its web just by the tiny vibrations it feels through the web's strands.",
            40, 60, 40, 40, 40, 30, 0, Color.clear,
            new int[] {1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 33, 36, 40, 43, 47, 50},
            new string[]
            {
                "Poison Sting", "String Shot", "Scary Face", "Constrict", "Leech Life", "Night Shade", "Shadow Sneak",
                "Fury Swipes", "Sucker Punch", "Spider Web", "Agility", "Pin Missile", "Psychic", "Poison Jab",
                "Cross Poison", "Sticky Web"
            },
            new string[] {},
            new int[] {168}, new string[] {"Level,22"}),
        new PokemonData(168, "Ariados", PokemonData.Type.BUG, PokemonData.Type.POISON, "Swarm", "Insomnia", "Sniper",
            50, 90, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1.1f, 33.5f,
            137, PokemonData.LevelingRate.FAST, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Long Leg",
            "It attaches silk to its prey and sets it free. Later, it tracks the silk to the prey and its friends.",
            70, 90, 70, 60, 60, 40, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 5, 8, 12, 15, 19, 23, 28, 32, 37, 41, 46, 50, 55, 58},
            new string[]
            {
                "Venom Drench", "Fell Stinger", "Bug Bite", "Poison Sting", "String Shot", "Scary Face", "Constrict",
                "Scary Face", "Constrict", "Leech Life", "Night Shade", "Shadow Sneak", "Fury Swipes", "Sucker Punch",
                "Spider Web", "Agility", "Pin Missile", "Psychic", "Poison Jab", "Cross Poison", "Sticky Web"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(183, "Marill", PokemonData.Type.WATER, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power",
            "Sap Sipper",
            50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FAIRY, 2805, 0.4f, 8.5f,
            88, PokemonData.LevelingRate.FAST, 2, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Aqua Mouse",
            "Marill's oil-filled tail acts much like a life preserver. If you see just its tail bobbing on the water's surface, it's a sure indication that this Pokémon is diving beneath the water to feed on aquatic plants.",
            70, 20, 50, 20, 50, 40, 0, Color.clear,
            new int[] {1, 1, 2, 5, 7, 10, 10, 13, 16, 20, 23, 28, 31, 37, 40, 47},
            new string[]
            {
                "Tackle", "Water Gun", "Tail Whip", "Water Sport", "Bubble", "Defense Curl", "Rollout", "Bubble Beam",
                "Helping Hand", "Aqua Tail", "Play Rough", "Aqua Ring", "Rain Dance", "Double-Edge", "Superpower",
                "Hydro Pump"
            },
            new string[] {},
            new int[] {184}, new string[] {"Level,18"}),
        new PokemonData(184, "Azumarill", PokemonData.Type.WATER, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power",
            "Sap Sipper",
            50, 75, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FAIRY, 2805, 0.8f, 28.5f,
            185, PokemonData.LevelingRate.FAST, 3, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Aqua Rabbit",
            "Azumarill's long ears are indispensable sensors. By focusing its hearing, this Pokémon can identify what kinds of prey are around, even in rough and fast-running rivers.",
            100, 50, 80, 60, 80, 50, 0, Color.clear,
            new int[] {1, 1, 1, 1, 2, 5, 7, 10, 10, 13, 16, 21, 25, 31, 35, 42, 46, 55},
            new string[]
            {
                "Tackle", "Tail Whip", "Water Sport", "Water Gun", "Tail Whip", "Water Sport", "Bubble", "Defense Curl",
                "Rollout", "Bubble Beam", "Helping Hand", "Aqua Tail", "Play Rough", "Aqua Ring", "Rain Dance",
                "Double-Edge", "Superpower", "Hydro Pump"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(196, "Espeon", PokemonData.Type.PSYCHIC, PokemonData.Type.NONE, "Synchronize", null,
            "Magic Bounce",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 9180, 0.9f, 26.5f,
            184, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.PURPLE, 70,
            "Sun",
            "Espeon is extremely loyal to any Trainer it considers to be worthy. It is said that this Pokémon developed its precognitive powers to protect its Trainer from harm.",
            65, 65, 60, 130, 90, 110, 0, Color.clear,
            new int[] {1, 1, 1, 5, 9, 13, 17, 20, 25, 29, 33, 37, 41, 45},
            new string[]
            {
                "Helping Hand", "Tackle", "Tail Whip", "Sand Attack", "Confusion", "Quick Attack", "Swift", "Psybeam",
                "Future Sight", "Psych Up", "Morning Sun", "Psychic", "Last Resort", "Power Swap"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(197, "Umbreon", PokemonData.Type.DARK, PokemonData.Type.NONE, "Synchronize", null, "Inner Focus",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 9180, 1, 27,
            184, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 2, 0, PokemonData.PokedexColor.BLACK, 35,
            "Moonlight",
            "Umbreon evolved as a result of exposure to the moon's waves. It hides silently in darkness and waits for its foes to make a move. The rings on its body glow when it leaps to attack.",
            95, 65, 110, 60, 130, 65, 0.1f, new Color(1, 1, 0, 1),
            new int[] {1, 1, 1, 5, 9, 13, 17, 20, 25, 29, 33, 37, 41, 45},
            new string[]
            {
                "Helping Hand", "Tackle", "Tail Whip", "Sand Attack", "Pursuit", "Quick Attack", "Confuse Ray",
                "Feint Attack", "Assurance", "Screech", "Moonlight", "Mean Look", "Last Resort", "Guard Swap"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(218, "Slugma", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Magma Armor", "Flame Body",
            "Weak Armor",
            50, 190, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.7f, 35,
            50, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Lava",
            "It is a species of Pokémon that lives in volcanic areas. If its body cools, its skin hardens and immobilizes it. To avoid that, it sleeps near magma",
            40, 40, 40, 70, 40, 20, 0, Color.clear,
            new int[] {1, 1, 6, 8, 13, 15, 20, 22, 27, 29, 34, 36, 41, 43, 48, 50},
            new string[]
            {
                "Yawn", "Smog", "Ember", "Rock Throw", "Harden", "Incinerate", "Clear Smog", "Ancient Power",
                "Flame Burst", "Rock Slide", "Lava Plume", "Amnesia", "Body Slam", "Recover", "Flamethrower",
                "Earth Power"
            },
            new string[] {},
            new int[] {219}, new string[] {"Level,38"}),
        new PokemonData(219, "Slugma", PokemonData.Type.FIRE, PokemonData.Type.ROCK, "Magma Armor", "Flame Body",
            "Weak Armor",
            50, 75, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.8f, 55,
            144, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Lava",
            "Magcargo's shell is actually its skin that hardened as a result of cooling. Its shell is very brittle and fragile - just touching it causes it to crumble apart. This Pokémon returns to its original size by dipping itself in magma.",
            50, 50, 120, 80, 80, 30, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 6, 8, 13, 15, 20, 22, 27, 29, 34, 36, 38, 43, 47, 54, 58},
            new string[]
            {
                "Earth Power", "Yawn", "Smog", "Ember", "Rock Throw", "Ember", "Rock Throw", "Harden", "Incinerate",
                "Clear Smog", "Ancient Power", "Flame Burst", "Rock Slide", "Lava Plume", "Amnesia", "Shell Smash",
                "Body Slam", "Recover", "Flamethrower", "Earth Power"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(240, "Magby", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null, "Vital Spirit",
            75, 45, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 6630, 0.7f, 21.4f,
            73, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.RED, 70,
            "Live Coal",
            "Magby's state of health is determined by observing the fire it breathes. If the Pokémon is spouting yellow flames from its mouth, it is in good health. When it is fatigued, black smoke will be mixed in with the flames.",
            45, 75, 37, 70, 55, 83, 0, Color.clear,
            new int[] {1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 33, 36, 40, 43},
            new string[]
            {
                "Smog", "Leer", "Ember", "Smokescreen", "Feint Attack", "Fire Spin", "Clear Smog", "Flame Burst",
                "Confuse Ray", "Fire Punch", "Lava Plume", "Sunny Day", "Flamethrower", "Fire Blast"
            },
            new string[] {},
            new int[] {126}, new string[] {"Level,30"}),
        new PokemonData(246, "Larvitar", PokemonData.Type.ROCK, PokemonData.Type.GROUND, "Guts", null, "Sand Veil",
            50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 0.6f, 72,
            60, PokemonData.LevelingRate.SLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.GREEN, 35,
            "Rock Skin",
            "Born deep underground, it comes aboveground and becomes a pupa once it has finished eating the surrounding soil.",
            50, 64, 50, 45, 50, 41, 0, Color.clear,
            new int[] {1, 1, 5, 10, 14, 19, 23, 28, 32, 37, 41, 46, 50, 55},
            new string[]
            {
                "Bite", "Leer", "Sandstorm", "Screech", "Chip Away", "Rock Slide", "Scary Face", "Thrash", "Dark Pulse",
                "Payback", "Crunch", "Earthquake", "Stone Edge", "Hyper Beam"
            },
            new string[] {},
            new int[] {247}, new string[] {"Level,30"}),
        new PokemonData(247, "Pupitar", PokemonData.Type.ROCK, PokemonData.Type.GROUND, "Shed Skin", null, null,
            50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 1.2f, 152,
            144, PokemonData.LevelingRate.SLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 35,
            "Hard Shell",
            "Pupitar creates a gas inside its body that it compresses and forcefully ejects to propel itself like a jet. The body is very durable--it avoids damage even if it hits solid steel.",
            70, 84, 70, 65, 70, 51, 0, Color.clear,
            new int[] {1, 1, 1, 1, 5, 10, 14, 19, 23, 28, 34, 41, 47, 54, 60, 67},
            new string[]
            {
                "Bite", "Leer", "Sandstorm", "Screech", "Sandstorm", "Screech", "Chip Away", "Rock Slide", "Scary Face",
                "Thrash", "Dark Pulse", "Payback", "Crunch", "Earthquake", "Stone Edge", "Hyper Beam"
            },
            new string[] {},
            new int[] {248}, new string[] {"Level,55"}),
        new PokemonData(248, "Tyranitar", PokemonData.Type.ROCK, PokemonData.Type.DARK, "Sand Stream", null, "Unnerve",
            50, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 10455, 2.5f, 202,
            270, PokemonData.LevelingRate.SLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.GREEN, 35,
            "Armor",
            "Tyranitar is so overwhelmingly powerful, it can bring down a whole mountain to make its nest. This Pokémon wanders about in mountains seeking new opponents to fight.",
            100, 134, 110, 95, 100, 61, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 5, 10, 14, 19, 23, 28, 34, 41, 47, 54, 63, 73, 82},
            new string[]
            {
                "Thunder Fang", "Ice Fang", "Fire Fang", "Bite", "Leer", "Sandstorm", "Screech", "Sandstorm", "Screech",
                "Chip Away", "Rock Slide", "Scary Face", "Thrash", "Dark Pulse", "Payback", "Crunch", "Earthquake",
                "Stone Edge", "Hyper Beam", "Giga Impact"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(252, "Treecko", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.5f, 5,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.GREEN, 70,
            "Wood Gecko",
            "Treecko is cool, calm, and collected—it never panics under any situation. If a bigger foe were to glare at this Pokémon, it would glare right back without conceding an inch of ground.",
            40, 45, 35, 65, 55, 70, 0, Color.clear,
            new int[] {1, 1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45, 49},
            new string[]
            {
                "Pound", "Leer", "Absorb", "Quick Attack", "Mega Drain", "Pursuit", "Giga Drain", "Agility", "Slam",
                "Detect", "Energy Ball", "Quick Guard", "Endeavor", "Screech"
            },
            new string[] {},
            new int[] {253}, new string[] {"Level,16"}),
        new PokemonData(253, "Grovyle", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 0.9f, 21.6f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.GREEN, 70,
            "Wood Gecko",
            "This Pokémon adeptly flies from branch to branch in trees. In a forest, no Pokémon can ever hope to catch a fleeing Grovyle however fast they may be.",
            50, 65, 45, 85, 65, 95, 0, Color.clear,
            new int[] {1, 1, 1, 1, 5, 9, 13, 16, 18, 23, 28, 33, 38, 43, 48, 53, 58, 63},
            new string[]
            {
                "Pound", "Leer", "Absorb", "Quick Attack", "Absorb", "Quick Attack", "Mega Drain", "Fury Cutter",
                "Pursuit", "Leaf Blade", "Agility", "Slam", "Detect", "X-Scissor", "False Swipe", "Quick Guard",
                "Leaf Storm", "Screech"
            },
            new string[] {},
            new int[] {254}, new string[] {"Level,36"}),
        new PokemonData(254, "Sceptile", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Unburden",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.DRAGON, 5355, 1.7f, 52.2f,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 3, PokemonData.PokedexColor.GREEN, 70,
            "Forest",
            "The leaves that grow on its arms can slice down thick trees. It is without peer in jungle combat.",
            70, 85, 65, 105, 85, 120, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 5, 9, 13, 16, 18, 23, 28, 33, 36, 39, 45, 51, 57, 63, 69},
            new string[]
            {
                "Leaf Storm", "Night Slash", "Pound", "Leer", "Absorb", "Quick Attack", "Absorb", "Quick Attack",
                "Mega Drain", "Fury Cutter", "Pursuit", "Leaf Blade", "Agility", "Slam", "Dual Chop", "Detect",
                "X-Scissor", "False Swipe", "Quick Guard", "Leaf Storm", "Screech"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(255, "Torchic", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Speed Boost",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 2.5f,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Chick",
            "Inside its body is a place where it keeps a small flame. Hug it! It will be as warm as a hot-water bottle.",
            45, 60, 40, 70, 50, 45, 0, Color.clear,
            new int[] {1, 1, 5, 14, 10, 19, 23, 28, 32, 37, 41, 46},
            new string[]
            {
                "Scratch", "Growl", "Ember", "Sand Attack", "Peck", "Fire Spin", "Quick Attack", "Flame Burst",
                "Focus Energy", "Slash", "Mirror Move", "Flamethrower"
            },
            new string[] {},
            new int[] {256}, new string[] {"Level,16"}),
        new PokemonData(256, "Combusken", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Speed Boost",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.9f, 19.5f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 1, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Young Fowl",
            "Its kicking mastery lets it loose 10 kicks per second. It emits sharp cries to intimidate foes.",
            60, 85, 60, 85, 60, 55, 0, Color.clear,
            new int[] {1, 1, 1, 1, 5, 10, 14, 16, 20, 25, 31, 36, 42, 47, 53, 58},
            new string[]
            {
                "Scratch", "Growl", "Ember", "Sand Attack", "Ember", "Sand Attack", "Peck", "Double Kick",
                "Flame Charge", "Quick Attack", "Bulk Up", "Focus Energy", "Slash", "Mirror Move", "Sky Uppercut",
                "Flare Blitz"
            },
            new string[] {},
            new int[] {257}, new string[] {"Level,36"}),
        new PokemonData(257, "Blaziken", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Speed Boost",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.9f, 52,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Blaze",
            "In battle, Blaziken blows out intense flames from its wrists and attacks foes courageously. The stronger the foe, the more intensely this Pokémon’s wrists burn.",
            80, 120, 70, 110, 70, 80, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 5, 10, 14, 16, 20, 25, 31, 36, 37, 44, 50, 57, 63},
            new string[]
            {
                "Flare Blitz", "Fire Punch", "High Jump Kick", "Scratch", "Growl", "Ember", "Sand Attack", "Ember",
                "Sand Attack", "Peck", "Double Kick", "Flame Charge", "Quick Attack", "Bulk Up", "Blaze Kick",
                "Focus Energy", "Slash", "Brave Bird", "Sky Uppercut", "Flare Blitz"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(258, "Mudkip", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Damp",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.4f, 7.6f,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Mud Fish",
            "The fin on Mudkip’s head acts as highly sensitive radar. Using this fin to sense movements of water and air, this Pokémon can determine what is taking place around it without using its eyes.",
            50, 70, 50, 50, 50, 40, 0, Color.clear,
            new int[] {1, 1, 4, 9, 12, 17, 20, 25, 28, 33, 36, 41, 44},
            new string[]
            {
                "Tackle", "Growl", "Water Gun", "Mud-Slap", "Foresight", "Bide", "Mud Sport", "Rock Throw", "Protect",
                "Whirlpool", "Take Down", "Hydro Pump", "Endeavor"
            },
            new string[] {},
            new int[] {259}, new string[] {"Level,16"}),
        new PokemonData(259, "Marshtomp", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Torrent", null, "Damp",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 0.7f, 28,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Mud Fish",
            "Marshtomp is much faster at traveling through mud than it is at swimming. This Pokémon’s hindquarters exhibit obvious development, giving it the ability to walk on just its hind legs.",
            70, 85, 70, 60, 70, 50, 0, Color.clear,
            new int[] {1, 1, 1, 1, 4, 9, 12, 16, 18, 22, 28, 32, 38, 42, 48, 52},
            new string[]
            {
                "Tackle", "Growl", "Water Gun", "Mud-Slap", "Water Gun", "Mud Slap", "Foresight", "Mud Shot", "Bide",
                "Mud Bomb", "Rock Slide", "Protect", "Muddy Water", "Take Down", "Earthquake", "Endeavor"
            },
            new string[] {},
            new int[] {260}, new string[] {"Level,36"}),
        new PokemonData(260, "Swampert", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Torrent", null, "Damp",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.WATER1, 5355, 1.5f, 81.9f,
            241, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Mud Fish",
            "Swampert predicts storms by sensing subtle differences in the sounds of waves and tidal winds with its fins. If a storm is approaching, it piles up boulders to protect itself.",
            100, 110, 90, 85, 90, 60, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 4, 9, 12, 16, 18, 22, 28, 32, 39, 44, 51, 56, 63},
            new string[]
            {
                "Hammer Arm", "Tackle", "Growl", "Water Gun", "Mud-Slap", "Water Gun", "Mud-Slap", "Foresight",
                "Mud Shot", "Bide", "Mud Bomb", "Rock Slide", "Protect", "Muddy Water", "Take Down", "Earthquake",
                "Endeavor", "Hammer Arm"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(263, "Zigzagoon", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Pickup", "Gluttony",
            "Quick Feet",
            50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.4f, 17.5f,
            48, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Tiny Raccoon",
            "Rubbing its nose against the ground, it always wanders about back and forth in search of something. It is distinguished by the zigzag footprints it leaves.",
            38, 30, 41, 30, 41, 60, 0, Color.clear,
            new int[] {1, 1, 5, 7, 11, 12, 13, 17, 19, 23, 25, 29, 31, 35, 37, 41},
            new string[]
            {
                "Tackle", "Growl", "Tail Whip", "Sand Attack", "Headbutt", "Baby-Doll Eyes", "Odor Sleuth", "Mud Sport",
                "Pin Missile", "Covet", "Bestow", "Flail", "Take Down", "Rest", "Belly Drum", "Fling"
            },
            new string[] {},
            new int[] {264}, new string[] {"Level,20"}),
        new PokemonData(264, "Linoone", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Pickup", "Gluttony",
            "Quick Feet",
            50, 90, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 4080, 0.5f, 32.5f,
            147, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.WHITE, 70,
            "Rushing",
            "Linoone always runs full speed and only in straight lines. If facing an obstacle, it makes a right-angle turn to evade it. This Pokémon is very challenged by gently curving roads.",
            78, 70, 61, 50, 61, 100, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 5, 7, 11, 13, 17, 19, 24, 27, 32, 35, 40, 43, 48},
            new string[]
            {
                "Play Rough", "Rototiller", "Switcheroo", "Tackle", "Growl", "Tail Whip", "Sand Attack", "Tail Whip",
                "Sand Attack", "Headbutt", "Odor Sleuth", "Mud Sport", "Fury Swipes", "Covet", "Bestow", "Slash",
                "Double-Edge", "Rest", "Belly Drum", "Fling"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(265, "Wurmple", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shield Dust", null, "Run Away",
            50, 255, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.3f, 3.6f,
            39, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Worm",
            "Using the spikes on its rear end, Wurmple peels the bark off trees and feeds on the sap that oozes out. This Pokémon's feet are tipped with suction pads that allow it to cling to glass without slipping.",
            45, 45, 35, 20, 30, 20, 0, Color.clear,
            new int[] {1, 1, 5, 15},
            new string[] {"Tackle", "String Shot", "Poison Sting", "Bug Bite"},
            new string[] {},
            new int[] {266, 268},
            new string[]
            {
                "if pokemon's shinyValue divided by 2's remainder is equal to 0",
                "if pokemon's shinyValue divided by 2's remainder is equal to 1"
            }),
        new PokemonData(266, "Silcoon", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
            50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.6f, 10,
            72, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.WHITE, 70,
            "Cocoon",
            "Silcoon was thought to endure hunger and not consume anything before its evolution. However, it is now thought that this Pokémon slakes its thirst by drinking rainwater that collects on its silk.",
            50, 35, 55, 25, 25, 15, 0, Color.clear,
            new int[] {1, 7},
            new string[] {"Harden", "Harden"},
            new string[] {},
            new int[] {267}, new string[] {"Level,10"}),
        new PokemonData(267, "Beautifly", PokemonData.Type.BUG, PokemonData.Type.FLYING, "Swarm", null, "Rivalry",
            50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1, 28.4f,
            173, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Butterfly",
            "It has an aggressive nature. It stabs prey with its long, narrow mouth to drain the prey's fluids.",
            60, 70, 50, 90, 50, 65, 0, Color.clear,
            new int[] {1, 10, 12, 15, 17, 20, 22, 25, 27, 30, 32, 35, 37, 40},
            new string[]
            {
                "Gust", "Gust", "Absorb", "Stun Spore", "Morning Sun", "Air Cutter", "Mega Drain", "Silver Wind",
                "Attract", "Whirlwind", "Giga Drain", "Bug Buzz", "Rage", "Quiver Dance"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(268, "Cascoon", PokemonData.Type.BUG, PokemonData.Type.NONE, "Shed Skin", null, null,
            50, 120, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 0.7f, 11.5f,
            41, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.PURPLE, 70,
            "Cocoon", "It never forgets any attack it endured while in the cocoon. After evolution, it seeks payback.",
            50, 35, 55, 25, 25, 15, 0, Color.clear,
            new int[] {1, 7},
            new string[] {"Harden", "Harden"},
            new string[] {},
            new int[] {269}, new string[] {"Level,10"}),
        new PokemonData(269, "Dustox", PokemonData.Type.BUG, PokemonData.Type.POISON, "Shield Dust", null,
            "Compound Eyes",
            50, 45, PokemonData.EggGroup.BUG, PokemonData.EggGroup.NONE, 4080, 1.2f, 31.6f,
            135, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 3, 0, PokemonData.PokedexColor.GREEN, 70,
            "Poison Moth",
            "Dustox is instinctively drawn to light. Swarms of this Pokémon are attracted by the bright lights of cities, where they wreak havoc by stripping the leaves off roadside trees for food.",
            60, 50, 70, 50, 90, 65, 0, Color.clear,
            new int[] {1, 10, 12, 15, 17, 20, 22, 25, 27, 30, 32, 35, 37, 40},
            new string[]
            {
                "Gust", "Gust", "Confusion", "Poison Powder", "Moonlight", "Venoshock", "Psybeam", "Silver Wind",
                "Light Screen", "Whirlwind", "Toxic", "Bug Buzz", "Protect", "Quiver Dance"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(276, "Taillow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Guts", null, "Scrappy",
            50, 200, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 2.3f,
            54, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BLUE, 70,
            "Tiny Swallow",
            "Taillow courageously stands its ground against foes, however strong they may be. This gutsy Pokémon will remain defiant even after a loss. On the other hand, it cries loudly if it becomes hungry.",
            40, 55, 30, 30, 30, 85, 0, Color.clear,
            new int[] {1, 1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41},
            new string[]
            {
                "Peck", "Growl", "Focus Energy", "Quick Attack", "Wing Attack", "Double Team", "Aerial Ace",
                "Quick Guard", "Agility", "Air Slash", "Endeavor", "Brave Bird"
            },
            new string[] {},
            new int[] {277}, new string[] {"Level,22"}),
        new PokemonData(277, "Swellow", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Guts", null, "Scrappy",
            50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.7f, 19.8f,
            151, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BLUE, 70,
            "Swallow",
            "Swellow is very conscientious about the upkeep of its glossy wings. Once two Swellow are gathered, they diligently take care of cleaning each other’s wings",
            60, 85, 60, 50, 50, 125, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 5, 9, 13, 17, 21, 27, 33, 39, 45, 51},
            new string[]
            {
                "Brave Bird", "Air Slash", "Pluck", "Peck", "Growl", "Focus Energy", "Quick Attack", "Focus Energy",
                "Quick Attack", "Wing Attack", "Double Team", "Aerial Ace", "Quick Guard", "Agility", "Endeavor",
                "Air Slash", "Brave Bird"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(278, "Wingull", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", null, "Rain Dish",
            50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 0.6f, 9.5f,
            54, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.WHITE, 70,
            "Seagull",
            "Wingull has the habit of carrying prey and valuables in its beak and hiding them in all sorts of locations. This Pokémon rides the winds and flies as if it were skating across the sky.",
            40, 30, 30, 55, 30, 85, 0, Color.clear,
            new int[] {1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 33, 36, 40, 43},
            new string[]
            {
                "Growl", "Water Gun", "Supersonic", "Wing Attack", "Mist", "Water Pulse", "Quick Attack", "Air Cutter",
                "Pursuit", "Aerial Ace", "Roost", "Agility", "Air Slash", "Hurricane"
            },
            new string[] {},
            new int[] {279}, new string[] {"Level,25"}),
        new PokemonData(279, "Pelipper", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", null, "Rain Dish",
            50, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 1.2f, 28,
            151, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Water Bird",
            "Pelipper is a flying transporter that carries small Pokémon and eggs inside its massive bill. This Pokémon builds its nest on steep cliffs facing the sea.",
            60, 50, 100, 85, 70, 65, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 5, 8, 12, 15, 19, 22, 25, 28, 33, 33, 33, 39, 44, 50, 55},
            new string[]
            {
                "Hurricane", "Hydro Pump", "Tailwind", "Soak", "Growl", "Water Gun", "Water Sport", "Wing Attack",
                "Supersonic", "Wing Attack", "Mist", "Water Pulse", "Payback", "Roost", "Protect", "Brine", "Stockpile",
                "Swallow", "Spit Up", "Fling", "Tailwind", "Hydro Pump", "Hurricane"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(283, "Surskit", PokemonData.Type.BUG, PokemonData.Type.WATER, "Swift Swim", null, "Rain Dish",
            50, 200, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.BUG, 4080, 0.5f, 1.7f,
            54, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BLUE, 70,
            "Pond Skater",
            "If Surskit senses danger, it secretes a thick, sugary syrup from the tip of its head. There are some Pokémon that love eating this syrup.",
            40, 30, 32, 50, 52, 65, 0, Color.clear,
            new int[] {1, 6, 9, 14, 17, 22, 25, 30, 35, 38},
            new string[]
            {
                "Bubble", "Quick Attack", "Sweet Scent", "Water Sport", "Bubble Beam", "Agility", "Mist", "Haze",
                "Aqua Jet", "Baton Pass", "Sticky Web"
            },
            new string[] {},
            new int[] {284}, new string[] {"Level,22"}),
        new PokemonData(284, "Masquerain", PokemonData.Type.BUG, PokemonData.Type.FLYING, "Intimidate", null, "Unnerve",
            50, 75, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.BUG, 4080, 0.8f, 3.6f,
            145, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 1, 1, 0, PokemonData.PokedexColor.BLUE, 70,
            "Eyeball",
            "Masquerain intimidates enemies with the eyelike patterns on its antennas. This Pokémon flaps its four wings to freely fly in any direction—even sideways and backwards—as if it were a helicopter.",
            70, 60, 62, 80, 82, 60, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 6, 9, 14, 17, 22, 22, 26, 32, 38, 42, 48, 52},
            new string[]
            {
                "Quiver Dance", "Bug Buzz", "Whirlwind", "Ominous Wind", "Bubble", "Quick Attack", "Sweet Scent",
                "Water Sport", "Quick Attack", "Sweet Scent", "Water Sport", "Gust", "Scary Face", "Air Cutter",
                "Stun Spore", "Silver Wind", "Air Slash", "Bug Buzz", "Whirlwind", "Quiver Dance"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(298, "Azurill", PokemonData.Type.NORMAL, PokemonData.Type.FAIRY, "Thick Fat", "Huge Power",
            "Sap Sipper",
            25, 150, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 2805, 0.2f, 2,
            38, PokemonData.LevelingRate.FAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Polka Dot",
            "Azurill spins its tail as if it were a lasso, then hurls it far. The momentum of the throw sends its body flying, too. Using this unique action, one of these Pokémon managed to hurl itself a record 10 meters.",
            50, 20, 40, 20, 40, 20, 0, Color.clear,
            new int[] {1, 1, 2, 5, 7, 10, 13, 16, 20, 23},
            new string[]
            {
                "Splash", "Water Gun", "Tail Whip", "Water Sport", "Bubble", "Charm", "Bubble Beam", "Helping Hand",
                "Slam", "Bounce"
            },
            new string[] {},
            new int[] {183}, new string[] {"Friendship"}),
        new PokemonData(300, "Skitty", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Normalize",
            "Wonder Skin",
            25, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.FAIRY, 4080, 0.6f, 11,
            52, PokemonData.LevelingRate.FAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.PINK, 70,
            "Kitten",
            "Skitty has the habit of becoming fascinated by moving objects and chasing them around. This Pokémon is known to chase after its own tail and become dizzy.",
            50, 45, 45, 35, 35, 50, 0, Color.clear,
            new int[] {1, 1, 1, 1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43, 46},
            new string[]
            {
                "Fake Out", "Growl", "Tail Whip", "Tackle", "Foresight", "Attract", "Sing", "Disarming Voice",
                "Double Slap", "Copycat", "Feint Attack", "Charm", "Wake-Up Slap", "Assist", "Covet", "Heal Bell",
                "Double-Edge", "Captivate", "Play Rough"
            },
            new string[] {},
            new int[] {301}, new string[] {"Stone,Moon Stone"}),
        new PokemonData(301, "Delcatty", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Normalize",
            "Wonder Skin",
            25, 60, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.FAIRY, 4080, 1.1f, 32.6f,
            133, PokemonData.LevelingRate.FAST, 1, 0, 0, 0, 0, 1, PokemonData.PokedexColor.PURPLE, 70,
            "Prim",
            "Delcatty prefers to live an unfettered existence in which it can do as it pleases at its own pace. Because this Pokémon eats and sleeps whenever it decides, its daily routines are completely random.",
            70, 65, 65, 55, 55, 70, 0, Color.clear,
            new int[] {1, 1, 1, 1},
            new string[] {"Fake Out", "Attract", "Sing", "Double Slap"},
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(315, "Roselia", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point",
            "Leaf Guard",
            50, 150, PokemonData.EggGroup.FAIRY, PokemonData.EggGroup.GRASS, 5355, 0.3f, 2,
            140, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Thorn",
            "Roselia shoots sharp thorns as projectiles at any opponent that tries to steal the flowers on its arms. The aroma of this Pokémon brings serenity to living things.",
            50, 60, 45, 100, 80, 65, 0, Color.clear,
            new int[] {1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43, 46, 50},
            new string[]
            {
                "Absorb", "Growth", "Poison Sting", "Stun Spore", "Mega Drain", "Leech Seed", "Magical Leaf",
                "Grass Whistle", "Giga Drain", "Toxic Spikes", "Sweet Scent", "Ingrain", "Petal Blizzard", "Toxic",
                "Aromatherapy", "Synthesis", "Petal Dance"
            },
            new string[] {},
            new int[] {407}, new string[] {"Stone,Shiny Stone"}),
        new PokemonData(322, "Numel", PokemonData.Type.FIRE, PokemonData.Type.GROUND, "Oblivious", "Simple", "Own Tempo",
            50, 255, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.7f, 24,
            61, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.YELLOW, 70,
            "Numb",
            "Numel is extremely dull witted—it doesn’t notice being hit. However, it can’t stand hunger for even a second. This Pokémon’s body is a seething cauldron of boiling magma.",
            60, 60, 40, 65, 45, 35, 0, Color.clear,
            new int[] {1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 31, 36, 40, 43, 47},
            new string[]
            {
                "Growl", "Tackle", "Ember", "Focus Energy", "Magnitude", "Flame Burst", "Amnesia", "Lava Plume",
                "Earth Power", "Curse", "Take Down", "Yawn", "Earthquake", "Flamethrower", "Doube-Edge"
            },
            new string[] {},
            new int[] {323}, new string[] {"Level,33"}),
        new PokemonData(323, "Camerupt", PokemonData.Type.FIRE, PokemonData.Type.GROUND, "Magma Armor", "Solid Rock",
            "Anger Point",
            50, 150, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.9f, 220,
            161, PokemonData.LevelingRate.MEDIUMFAST, 0, 1, 0, 1, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Eruption",
            "The humps on Camerupt’s back are formed by a transformation of its bones. They sometimes blast out molten magma. This Pokémon apparently erupts often when it is enraged.",
            70, 100, 70, 105, 75, 40, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 31, 33, 39, 46, 52, 59},
            new string[]
            {
                "Fissure", "Eruption", "Growl", "Tackle", "Ember", "Focus Energy", "Ember", "Focus Energy", "Magnitude",
                "Flame Burst", "Amnesia", "Lava Plume", "Earth Power", "Curse", "Take Down", "Rock Slide", "Yawn",
                "Earthquake", "Eruption", "Fissure"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(339, "Barboach", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Oblivious", "Anticipation",
            "Hydration",
            50, 190, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.NONE, 5355, 0.4f, 1.9f,
            58, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Whiskers",
            "Barboach’s sensitive whiskers serve as a superb radar system. This Pokémon hides in mud, leaving only its two whiskers exposed while it waits for prey to come along.",
            50, 48, 43, 46, 41, 60, 0, Color.clear,
            new int[] {1, 6, 6, 9, 13, 15, 17, 20, 25, 25, 28, 32, 35, 39, 44},
            new string[]
            {
                "Mud-Slap", "Mud Sport", "Water Sport", "Water Gun", "Mud Bomb", "Amnesia", "Water Pulse", "Magnitude",
                "Rest", "Snore", "Aqua Tail", "Earthquake", "Muddy Water", "Future Sight", "Fissure"
            },
            new string[] {},
            new int[] {340}, new string[] {"Level,30"}),
        new PokemonData(340, "Whiscash", PokemonData.Type.WATER, PokemonData.Type.GROUND, "Oblivious", "Anticipation",
            "Hydration",
            50, 75, PokemonData.EggGroup.WATER2, PokemonData.EggGroup.NONE, 5355, 0.9f, 23.6f,
            164, PokemonData.LevelingRate.MEDIUMFAST, 2, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Whiskers",
            "Whiscash is extremely territorial. Just one of these Pokémon will claim a large pond as its exclusive territory. If a foe approaches it, it thrashes about and triggers a massive earthquake.",
            110, 78, 73, 76, 71, 60, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 6, 6, 9, 13, 15, 17, 20, 25, 25, 28, 30, 34, 39, 45, 52},
            new string[]
            {
                "Tickle", "Mud-Slap", "Mud Sport", "Water Sport", "Water Gun", "Mud Sport", "Water Sport", "Water Gun",
                "Mud Bomb", "Amnesia", "Water Pulse", "Magnitude", "Rest", "Snore", "Aqua Tail", "Zen Headbutt",
                "Earthquake", "Muddy Water", "Future Sight", "Fissure"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(387, "Turtwig", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 0.4f, 10.2f,
            64, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Tiny Leaf",
            "Photosynthesis occurs across its body under the sun. The shell on its back is actually hardened soil.",
            55, 68, 64, 45, 55, 31, 0, Color.clear,
            new int[] {1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45},
            new string[]
            {
                "Tackle", "Withdraw", "Absorb", "Razor Leaf", "Curse", "Bite", "Mega Drain", "Leech Seed", "Synthesis",
                "Crunch", "Giga Drain", "Leaf Storm"
            },
            new string[] {},
            new int[] {388}, new string[] {"Level,18"}),
        new PokemonData(388, "Grotle", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 1.1f, 97,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 1, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Grove", "It knows where pure water wells up. It carries fellow Pokémon there on its back.",
            75, 89, 85, 55, 65, 36, 0, Color.clear,
            new int[] {1, 1, 5, 9, 13, 17, 22, 27, 32, 37, 42, 47, 52},
            new string[]
            {
                "Tackle", "Withdraw", "Withdraw", "Absorb", "Razor Leaf", "Curse", "Bite", "Mega Drain", "Leech Seed",
                "Synthesis", "Crunch", "Giga Drain", "Leaf Storm"
            },
            new string[] {},
            new int[] {389}, new string[] {"Level,32"}),
        new PokemonData(389, "Torterra", PokemonData.Type.GRASS, PokemonData.Type.GROUND, "Overgrow", null,
            "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.GRASS, 5355, 2.2f, 310,
            236, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 1, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Continent", "Small Pokémon occasionally gather on its unmoving back to begin building their nests.",
            95, 109, 105, 75, 85, 56, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 9, 13, 17, 22, 27, 32, 33, 39, 45, 51, 57},
            new string[]
            {
                "Wood Hammer", "Tackle", "Withdraw", "Absorb", "Razor Leaf", "Withdraw", "Absorb", "Razor Leaf",
                "Curse", "Bite", "Mega Drain", "Earthquake", "Leech Seed", "Synthesis", "Crunch", "Giga Drain",
                "Leaf Storm"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(390, "Chimchar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Iron Fist",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.5f, 6.2f,
            62, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Chimp", "The gas made in its belly burns from its rear end. The fire burns weakly when it feels sick",
            44, 58, 44, 58, 44, 61, 0, Color.clear,
            new int[] {1, 1, 7, 9, 15, 17, 23, 25, 31, 33, 39, 41, 47},
            new string[]
            {
                "Scratch", "Leer", "Ember", "Taunt", "Fury Swipes", "Flame Wheel", "Nasty Plot", "Torment", "Facade",
                "Fire Spin", "Acrobatics", "Slack Off", "Flamethrower"
            },
            new string[] {},
            new int[] {391}, new string[] {"Level,14"}),
        new PokemonData(391, "Monferno", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Iron Fist",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.9f, 22,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Playful",
            "A bigger fire on its tail and a brighter blue pattern on its face means its rank in its pack is higher.",
            64, 78, 52, 78, 52, 81, 0, Color.clear,
            new int[] {1, 1, 1, 7, 9, 14, 16, 19, 26, 29, 36, 39, 46, 49, 56},
            new string[]
            {
                "Scratch", "Leer", "Ember", "Ember", "Taunt", "Mach Punch", "Fury Swipes", "Flame Wheel", "Feint",
                "Torment", "Close Combat", "Fire Spin", "Acrobatics", "Slack Off", "Flare Blitz"
            },
            new string[] {},
            new int[] {392}, new string[] {"Level,36"}),
        new PokemonData(392, "Infernape", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Iron Fist",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 1.2f, 55,
            240, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 1, 0, 1, PokemonData.PokedexColor.BROWN, 70,
            "Flame",
            "It tosses its enemies around with agility. It uses all its limbs to fight in its own unique style.",
            76, 104, 71, 104, 71, 108, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 7, 9, 14, 16, 19, 26, 29, 36, 42, 52, 58, 68},
            new string[]
            {
                "Flare Blitz", "Punishment", "Scratch", "Leer", "Ember", "Taunt", "Ember", "Taunt", "Mach Punch",
                "Fury Swipes", "Flame Wheel", "Feint", "Punishment", "Close Combat", "Fire Spin", "Acrobatics",
                "Calm Mind", "Flare Blitz"
            },
            new string[] {},
            new int[] {393}, new string[] {"Level,36"}),
        new PokemonData(393, "Piplup", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Defiant",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 0.4f, 5.2f,
            63, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Penguin",
            "A poor walker, it often falls down. However, its strong pride makes it puff up its chest without a care.",
            53, 51, 53, 61, 56, 40, 0, Color.clear,
            new int[] {1, 4, 8, 11, 15, 18, 22, 25, 29, 32, 36, 39, 43},
            new string[]
            {
                "Pound", "Growl", "Bubble", "Water Sport", "Peck", "Bubble Beam", "Bide", "Fury Attack", "Brine",
                "Whirlpool", "Mist", "Drill Peck", "Hydro Pump"
            },
            new string[] {},
            new int[] {394}, new string[] {"Level,16"}),
        new PokemonData(394, "Prinplup", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Defiant",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 0.8f, 23,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Penguin", "Because every Prinplup considers itself to be the most important, they can never form a group.",
            64, 66, 68, 81, 76, 50, 0, Color.clear,
            new int[] {1, 1, 4, 8, 11, 15, 16, 19, 24, 28, 33, 37, 42, 46, 50},
            new string[]
            {
                "Tackle", "Growl", "Growl", "Bubble", "Water Sport", "Peck", "Metal Claw", "Bubble Beam", "Bide",
                "Fury Attack", "Brine", "Whirlpool", "Mist", "Drill Peck", "Hydro Pump"
            },
            new string[] {},
            new int[] {395}, new string[] {"Level,36"}),
        new PokemonData(395, "Empoleon", PokemonData.Type.WATER, PokemonData.Type.STEEL, "Torrent", null, "Defiant",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 5355, 1.7f, 84.5f,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Emperor", "It avoids unnecessary disputes, but it will decimate anything that threatens its pride.",
            84, 86, 88, 111, 101, 60, 0, Color.clear,
            new int[] {1, 1, 1, 4, 8, 15, 16, 19, 28, 33, 36, 39, 46, 52, 59},
            new string[]
            {
                "Tackle", "Growl", "Bubble", "Growl", "Bubble", "Peck", "Metal Claw", "Bubble Beam", "Fury Attack",
                "Brine", "Aqua Jet", "Whirlpool", "Mist", "Drill Peck", "Hydro Pump"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(399, "Bidoof", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Simple", "Unaware", "Moody",
            50, 255, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 4080, 0.5f, 20,
            50, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Plump Mouse",
            "It lives in groups by the water. It chews up boulders and trees around its nest with its incisors.",
            59, 45, 40, 35, 40, 31, 0, Color.clear,
            new int[] {1, 5, 9, 13, 17, 21, 25, 29, 33, 37, 41, 45},
            new string[]
            {
                "Tackle", "Growl", "Defense Curl", "Rollout", "Headbutt", "Hyper Fang", "Yawn", "Amnesia", "Take Down",
                "Super Fang", "Super Power", "Curse"
            },
            new string[] {},
            new int[] {400}, new string[] {"Level,15"}),
        new PokemonData(400, "Bibarel", PokemonData.Type.NORMAL, PokemonData.Type.WATER, "Simple", "Unaware", "Moody",
            50, 127, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FIELD, 4080, 1, 31.5f,
            144, PokemonData.LevelingRate.MEDIUMFAST, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.BROWN, 70,
            "Beaver", "A river dammed by Bibarel will never overflow its banks, which is appreciated by people nearby.",
            79, 85, 60, 55, 60, 71, 0, Color.clear,
            new int[] {1, 1, 1, 5, 9, 13, 15, 18, 23, 28, 33, 38, 43, 48, 53},
            new string[]
            {
                "Rototiller", "Tackle", "Growl", "Growl", "Defense Curl", "Rollout", "Water Gun", "Headbutt",
                "Hyper Fang", "Yawn", "Amnesia", "Take Down", "Super Fang", "Superpower", "Curse"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(406, "Budew", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point",
            "Leaf Guard",
            50, 255, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 5355, 0.2f, 1.2f,
            56, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Bud", "It lives alongside clear ponds. It scatters pollen that induces harsh sneezing and runny noses.",
            40, 30, 35, 50, 70, 55, 0, Color.clear,
            new int[] {1, 4, 7, 10, 13, 16},
            new string[] {"Absorb", "Growth", "Water Sport", "Stun Spore", "Mega Dran", "Worry Seed"},
            new string[] {},
            new int[] {315}, new string[] {"Friendship\\Time,Day"}),
        new PokemonData(407, "Roserade", PokemonData.Type.GRASS, PokemonData.Type.POISON, "Natural Cure", "Poison Point",
            "Technician",
            50, 75, PokemonData.EggGroup.FAIRY, PokemonData.EggGroup.GRASS, 5355, 0.9f, 14.5f,
            227, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Bouquet",
            "Luring prey with a sweet scent, it uses poison whips on its arms to poison, bind, and finish off the prey.",
            60, 70, 65, 125, 105, 90, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1},
            new string[]
            {
                "Venom Drench", "Grassy Terrain", "Weather Ball", "Poison Sting", "Mega Drain", "Magical Leaf",
                "Sweet Scent"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(408, "Cranidos", PokemonData.Type.ROCK, PokemonData.Type.NONE, "Mold Breaker", null,
            "Sheer Force",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 0.9f, 31.5f,
            70, PokemonData.LevelingRate.ERRATIC, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Head Butt",
            "A lifelong jungle dweller from 100 million years ago, it would snap obstructing trees with headbutts.",
            67, 125, 40, 30, 30, 58, 0, Color.clear,
            new int[] {1, 1, 6, 10, 15, 19, 24, 28, 33, 37, 42, 46},
            new string[]
            {
                "Headbutt", "Leer", "Focus Energy", "Pursuit", "Take Down", "Scary Face", "Assurance", "Chip Away",
                "Ancient Power", "Zen Headbutt", "Screech", "Head Smash"
            },
            new string[] {},
            new int[] {409}, new string[] {"Level,30"}),
        new PokemonData(409, "Rampardos", PokemonData.Type.ROCK, PokemonData.Type.NONE, "Mold Breaker", null,
            "Sheer Force",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 1.6f, 102.5f,
            173, PokemonData.LevelingRate.ERRATIC, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Head Butt", "Its skull is as hard as iron, it is a brute that tears down jungle trees while catching prey.",
            97, 165, 60, 65, 50, 58, 0, Color.clear,
            new int[] {1, 1, 6, 10, 15, 19, 24, 28, 30, 36, 43, 51, 58},
            new string[]
            {
                "Headbutt", "Leer", "Focus Energy", "Pursuit", "Take Down", "Scary Face", "Assurance", "Chip Away",
                "Endeavor", "Ancient Power", "Zen Headbutt", "Screech", "Head Smash"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(410, "Shieldon", PokemonData.Type.ROCK, PokemonData.Type.STEEL, "Sturdy", null, "Soundproof",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 0.5f, 57,
            70, PokemonData.LevelingRate.ERRATIC, 0, 0, 1, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Shield",
            "It habitually polishes its face by rubbing it against tree trunks. It is weak to attacks from behind.",
            30, 42, 118, 42, 88, 30, 0, Color.clear,
            new int[] {1, 1, 6, 10, 15, 19, 24, 28, 33, 37, 42, 46},
            new string[]
            {
                "Tackle", "Protect", "Taunt", "Metal Sound", "Take Down", "Iron Defense", "Swagger", "Ancient Power",
                "Endure", "Metal Burst", "Iron Head", "Heavy Slam"
            },
            new string[] {},
            new int[] {411}, new string[] {"Level,30"}),
        new PokemonData(411, "Bastiodon", PokemonData.Type.ROCK, PokemonData.Type.STEEL, "Sturdy", null, "Soundproof",
            87.5f, 45, PokemonData.EggGroup.MONSTER, PokemonData.EggGroup.NONE, 7905, 1.3f, 149.5f,
            173, PokemonData.LevelingRate.ERRATIC, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Shield", "When attacked, they form a wall with their shield like faces to protect their young.",
            60, 52, 168, 57, 138, 30, 0, Color.clear,
            new int[] {1, 1, 1, 1, 6, 10, 15, 19, 24, 28, 30, 36, 43, 51, 58},
            new string[]
            {
                "Tackle", "Protect", "Taunt", "Metal Sound", "Taunt", "Metal Sound", "Take Down", "Iron Defense",
                "Swagger", "Ancient Power", "Block", "Endure", "Metal Burst", "Iron Head", "Heavy Slam"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(427, "Buneary", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Run Away", "Klutz", "Limber",
            50, 190,
            PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 0.4f, 5.5f,
            70, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BROWN, 0,
            "Rabbit",
            "By extending its rolled-up ears and striking the ground, it can bound so high it surprises itself.",
            55, 66, 44, 44, 56, 85, 0, Color.clear,
            new int[] {1, 1, 1, 1, 6, 10, 13, 16, 23, 26, 33, 36, 43, 46, 50, 56, 63},
            new string[]
            {
                "Defense Curl", "Splash", "Pound", "Foresight", "Endure", "Baby-Doll Eyes", "Frustration",
                "Quick Attack", "Jump Kick", "Baton Pass", "Agility", "Dizzy Punch", "After You", "Charm", "Entrainment",
                "Bounce", "Healing Wish"
            },
            new string[] {},
            new int[] {428}, new string[] {"Friendship"}),
        new PokemonData(428, "Lopunny", PokemonData.Type.NORMAL, PokemonData.Type.NONE, "Cute Charm", "Klutz", "Limber",
            50, 60, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 5355, 1.2f, 33.3f,
            168, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BROWN, 140,
            "Rabbit", "The ears appear to be delicate. If they are touched roughly, it kicks with its graceful legs.",
            65, 76, 84, 54, 96, 105, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 1, 6, 13, 16, 23, 26, 33, 36, 43, 46, 53, 56, 63, 66},
            new string[]
            {
                "Healing Wish", "Bounce", "Rototiller", "Mirro Coat", "Magic Coat", "Defense Curl", "Splash", "Pound",
                "Foresight", "Endure", "Return", "Quick Attack", "Jump Kick", "Baton Pass", "Agility", "Dizzy Punch",
                "After You", "Charm", "Entraintment", "Bounce", "Healing Wish", "High Jump Kick"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(447, "Riolu", PokemonData.Type.FIGHTING, PokemonData.Type.NONE, "Steadfast", "Inner Focus",
            "Prankster",
            87.5f, 75, PokemonData.EggGroup.UNDISCOVERED, PokemonData.EggGroup.NONE, 6630, 0.7f, 20.2f,
            57, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Emanation", "It uses the shapes of auras, which change according to emotion, to communicate with others.",
            40, 70, 40, 35, 40, 60, 0, Color.clear,
            new int[] {1, 1, 1, 6, 11, 15, 19, 24, 29, 47, 50},
            new string[]
            {
                "Foresight", "Quick Attack", "Endure", "Counter", "Feint", "Force Palm", "Copycat", "Screech",
                "Reversal", "Nasty Plot", "Final Gambit"
            },
            new string[] {},
            new int[] {448}, new string[] {"Friendship"}),
        new PokemonData(448, "Lucario", PokemonData.Type.FIGHTING, PokemonData.Type.STEEL, "Steadfast", "Inner Focus",
            "Justified",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.HUMANLIKE, 6630, 1.2f, 54,
            184, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 1, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Aura", "By catching the Aura emanating from others, it can read their thoughts and movements.",
            70, 110, 70, 115, 70, 90, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 1, 1, 6, 11, 15, 19, 24, 29, 33, 37, 42, 47, 51, 55, 60, 65},
            new string[]
            {
                "Extreme Speed", "Dragon Pulse", "Close Combat", "Aura Sphere", "Foresight", "Quick Attack", "Detect",
                "Metal Claw", "Counter", "Feint", "Power-Up Punch", "Swords Dance", "Metal Sound", "Bone Rush",
                "Quick Guard", "Me First", "Aura Sphere", "Calm Mind", "Heal Pulse", "Close Combat", "Dragon Pulse",
                "Extreme Speed"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(467, "Magmortar", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Flame Body", null,
            "Vital Spirt",
            70, 30, PokemonData.EggGroup.HUMANLIKE, PokemonData.EggGroup.NONE, 6630, 1.6f, 68,
            243, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Blast",
            "It blasts fireballs of over 2,000 degrees Celsius from the ends of its arms. It lives in volcanic craters.",
            75, 95, 67, 125, 95, 83, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 8, 12, 15, 19, 22, 26, 29, 36, 42, 49, 55, 62},
            new string[]
            {
                "Thunder Punch", "Smog", "Leer", "Ember", "Smokescreen", "Ember", "Smokescreen", "Feint Attack",
                "Fire Spin", "Clear Smog", "Flame Burst", "Confuse Ray", "Fire Punch", "Lava Plume", "Sunny Day",
                "Flamethrower", "Fire Blast", "Hyper Beam"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(495, "Snivy", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 0.6f, 8.1f,
            28, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.GREEN, 70,
            "Grass Snake",
            "They photosynthesize by bathing their tails in sunlight. When they are not feeling well, their tails droop.",
            45, 45, 55, 45, 55, 63, 0, Color.clear,
            new int[] {1, 4, 7, 10, 13, 16, 19, 22, 25, 28, 31, 34, 37, 40, 43},
            new string[]
            {
                "Tackle", "Leer", "Vine Whip", "Wrap", "Growth", "Leaf Tornado", "Leech Seed", "Mega Drain", "Slam",
                "Leaf Blade", "Coil", "Giga Drain", "Wring Out", "Gastro Acid", "Leaf Storm"
            },
            new string[] {},
            new int[] {496}, new string[] {"Level,17"}),
        new PokemonData(496, "Servine", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 0.8f, 16,
            145, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.GREEN, 70,
            "Grass Snake",
            "When it gets dirty, its leaves can't be used in photosynthesis, so it always keeps itself clean.",
            60, 60, 75, 60, 75, 83, 0, Color.clear,
            new int[] {1, 1, 1, 4, 7, 10, 13, 16, 20, 24, 28, 32, 36, 40, 44, 48, 52},
            new string[]
            {
                "Tackle", "Leer", "Vine Whip", "Leer", "Vine Whip", "Wrap", "Growth", "Leaf Tornado", "Leech Seed",
                "Mega Drain", "Slam", "Leaf Blade", "Coil", "Giga Drain", "Wring Out", "Gastro Acid", "Leaf Storm"
            },
            new string[] {},
            new int[] {497}, new string[] {"Level,36"}),
        new PokemonData(497, "Serperior", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Contrary",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.GRASS, 5355, 3.3f, 63,
            238, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 3, PokemonData.PokedexColor.GREEN, 70,
            "Regal",
            "It only gives its all against strong opponents who are not fazed by the glare from Serperior's noble eyes.",
            75, 75, 95, 75, 95, 113, 0, Color.clear,
            new int[] {1, 1, 1, 4, 7, 10, 13, 16, 20, 24, 28, 32, 38, 44, 50, 56, 62},
            new string[]
            {
                "Tackle", "Leer", "Vine Whip", "Leer", "Vine Whip", "Wrap", "Growth", "Leaf Tornado", "Leech Seed",
                "Mega Drain", "Slam", "Leaf Blade", "Coil", "Giga Drain", "Wring Out", "Gastro Acid", "Leaf Storm"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(498, "Tepig", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Thick Fat",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 9.9f,
            28, PokemonData.LevelingRate.MEDIUMSLOW, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Fire Pig", "It loves to eat roasted berries, but sometimes it gets too excited and burns them to a crisp.",
            65, 63, 45, 45, 45, 45, 0, Color.clear,
            new int[] {1, 3, 7, 9, 13, 15, 19, 21, 25, 27, 31, 33, 37, 39, 43},
            new string[]
            {
                "Tackle", "Tail Whip", "Ember", "Odor Sleuth", "Defense Crul", "Flame Charge", "Smog", "Rollout",
                "Take Down", "Heat Crash", "Assurance", "Flamethrower", "Head Smash", "Roar", "Flare Blitz"
            },
            new string[] {},
            new int[] {499}, new string[] {"Level,17"}),
        new PokemonData(499, "Pignite", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Thick Fat",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 55.5f,
            146, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Fire Pig",
            "The more it eats, the more fuel it has to make the fire in its stomach stronger. This fills it with even more power.",
            90, 93, 55, 70, 55, 55, 0, Color.clear,
            new int[] {1, 1, 1, 1, 3, 7, 9, 13, 15, 17, 20, 23, 28, 31, 36, 39, 44, 47, 52},
            new string[]
            {
                "Tackle", "Ember", "Tail Whip", "Odor Sleuth", "Tail Whip", "Ember", "Odor Sleuth", "Defense Curl",
                "Flame Charge", "Arm Thrust", "Smog", "Rollout", "Take Down", "Heat Crash", "Assurance", "Flamethrower",
                "Head Smash", "Roar", "Flare Blitz"
            },
            new string[] {},
            new int[] {500}, new string[] {"Level,36"}),
        new PokemonData(500, "Emboar", PokemonData.Type.FIRE, PokemonData.Type.FIGHTING, "Blaze", null, "Reckless",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.6f, 150,
            238, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Mega Fire Pig",
            "A flaring beard of fire is proof that it is fired up. It is adept at using many different moves.",
            110, 123, 65, 100, 65, 65, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 3, 7, 9, 12, 15, 17, 20, 23, 28, 31, 38, 43, 50, 55, 62},
            new string[]
            {
                "Hammer Arm", "Tackle", "Tail Whip", "Ember", "Odor Sleuth", "Tail Whip", "Ember", "Odor Sleuth",
                "Defense Curl", "Flame Charge", "Arm Thrust", "Smog", "Rollout", "Take Down", "Heat Crash", "Assurance",
                "Flamethrower", "Head Smash", "Roar", "Flare Blitz"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(501, "Oshawott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.5f, 5.9f,
            28, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Sea Otter",
            "The scalchop on its stomach is made from the same element as claws. It detaches the scalchop for use as a blade.",
            55, 55, 45, 63, 45, 45, 0, Color.clear,
            new int[] {1, 5, 7, 11, 13, 17, 19, 23, 25, 29, 31, 35, 37, 41, 43},
            new string[]
            {
                "Tackle", "Tail Whip", "Water Gun", "Water Sport", "Focus Energy", "Razor Shell", "Fury Cutter",
                "Water Pulse", "Revenge", "Aqua Jet", "Encore", "Aqua Tail", "Retaliate", "Swords Dance", "Hydro Pump"
            },
            new string[] {},
            new int[] {502}, new string[] {"Level,17"}),
        new PokemonData(502, "Dewott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.8f, 24.5f,
            145, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Discipline", "As a result of strict training, each Dewott learns different forms for using the scalchops.",
            75, 75, 60, 83, 60, 60, 0, Color.clear,
            new int[] {1, 1, 1, 1, 5, 7, 11, 13, 17, 20, 25, 28, 33, 38, 45, 50, 57, 62},
            new string[]
            {
                "Tackle", "Tail Whip", "Water Gun", "Water Sport", "Tail Whip", "Water Gun", "Water Sport",
                "Focus Energy", "Razor Shell", "Fury Cutter", "Water Pulse", "Revenge", "Aqua Jet", "Encore",
                "Aqua Tail", "Retaliate", "Swords Dance", "Hydro Pump"
            },
            new string[] {},
            new int[] {503}, new string[] {"Level,17"}),
        new PokemonData(503, "Samurott", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Shell Armor",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.5f, 94.6f,
            238, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Formidable",
            "One swing of the sword incorporated in its armor can fell an opponent. A simple glare from one of them quiets everybody.",
            95, 100, 85, 108, 70, 70, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 7, 11, 13, 17, 20, 25, 28, 33, 36, 38, 45, 50, 57, 62},
            new string[]
            {
                "Megahorn", "Tackle", "Tail Whip", "Water Gun", "Water Sport", "Focus Energy", "Razor Shell",
                "Fury Cutter", "Water Pulse", "Revenge", "Aqua Jet", "Slash", "Encore", "Aqua Tail", "Retaliate",
                "Swords Dance", "Hydro Pump"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(519, "Pidove", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck",
            "Rivalry",
            50, 255, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.3f, 2.1f,
            53, PokemonData.LevelingRate.MEDIUMSLOW, 0, 1, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Tiny Pigeon",
            "This very forgetful Pokémon will wait for a new order from its Trainer even though it already has one.",
            50, 55, 50, 36, 30, 43, 0, Color.clear,
            new int[] {1, 4, 8, 11, 15, 18, 22, 25, 29, 32, 36, 39, 43, 46, 50},
            new string[]
            {
                "Gust", "Growl", "Leer", "Quick Attack", "Air Cutter", "Roost", "Detect", "Taunt", "Air Slash",
                "Razor Wind", "Feather Dance", "Swagger", "Facade", "Tailwind", "Sky Attack"
            },
            new string[] {},
            new int[] {520}, new string[] {"Level,21"}),
        new PokemonData(520, "Tranquill", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck",
            "Rivalry",
            50, 120, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 0.6f, 15,
            125, PokemonData.LevelingRate.MEDIUMSLOW, 0, 2, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Wild Pigeon",
            "No matter where in the world it goes, it knows where its nest is, so it never gets separated from its Trainer.",
            62, 77, 62, 50, 42, 65, 0, Color.clear,
            new int[] {1, 1, 1, 1, 4, 8, 11, 15, 18, 23, 27, 32, 36, 41, 45, 50, 54, 59},
            new string[]
            {
                "Gust", "Growl", "Leer", "Quick Attack", "Growl", "Leer", "Quick Attack", "Air Cutter", "Roost",
                "Detect", "Taunt", "Air Slash", "Razor Wind", "Feather Dance", "Swagger", "Facade", "Tailwind",
                "Sky Attack"
            },
            new string[] {},
            new int[] {521}, new string[] {"Level,32"}),
        new PokemonData(521, "Unfezant", PokemonData.Type.NORMAL, PokemonData.Type.FLYING, "Big Pecks", "Super Luck",
            "Rivalry",
            50, 45, PokemonData.EggGroup.FLYING, PokemonData.EggGroup.NONE, 4080, 1.2f, 29,
            215, PokemonData.LevelingRate.MEDIUMSLOW, 0, 3, 0, 0, 0, 0, PokemonData.PokedexColor.GRAY, 70,
            "Proud",
            "Males swing their head plumage to threaten opponents. The females' flying abilities surpass those of the males.",
            80, 115, 80, 65, 55, 93, 0, Color.clear,
            new int[] {1, 1, 1, 1, 4, 8, 11, 15, 18, 23, 27, 33, 38, 44, 49, 55, 60, 66},
            new string[]
            {
                "Gust", "Growl", "Leer", "Quick Attack", "Growl", "Leer", "Quick Attack", "Air Cutter", "Roost",
                "Detect", "Taunt", "Air Slash", "Razor Wind", "Feather Dance", "Swagger", "Facade", "Tailwind",
                "Sky Attack"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(580, "Ducklett", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", "Big Pecks",
            "Hydration",
            50, 190, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 0.5f, 5.5f,
            61, PokemonData.LevelingRate.MEDIUMFAST, 1, 0, 0, 0, 0, 0, PokemonData.PokedexColor.BLUE, 70,
            "Water Bird",
            "They are better at swimming than flying, and they happily eat their favorite food, peat moss, as they dive underwater",
            62, 44, 50, 44, 50, 55, 0, Color.clear,
            new int[] {1, 3, 6, 9, 13, 15, 19, 21, 24, 27, 30, 34, 37, 41, 46},
            new string[]
            {
                "Water Gun", "Water Sport", "Defog", "Wing Attack", "Water Pulse", "Aerial Ace", "Bubble Beam",
                "Feather Dance", "Aqua Ring", "Air Slash", "Roost", "Rain Dance", "Tailwind", "Brave Bird", "Hurricane"
            },
            new string[] {},
            new int[] {581}, new string[] {"Level,35"}),
        new PokemonData(581, "Swanna", PokemonData.Type.WATER, PokemonData.Type.FLYING, "Keen Eye", "Big Pecks",
            "Hydration",
            50, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.FLYING, 5355, 1.3f, 24.2f,
            166, PokemonData.LevelingRate.MEDIUMFAST, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.WHITE, 70,
            "White Bird", "Swanna start to dance at dusk. The one dancing in the middle is the leader of the flock.",
            75, 87, 63, 87, 63, 98, 0, Color.clear,
            new int[] {1, 1, 1, 1, 3, 6, 9, 13, 15, 19, 21, 24, 27, 30, 34, 40, 47, 55},
            new string[]
            {
                "Water Gun", "Water Sport", "Defog", "Wing Attack", "Water Sport", "Defog", "Wing Attack",
                "Water Pulse", "Aerial Ace", "Bubble Beam", "Feather Dance", "Aqua Ring", "Air Slash", "Roost",
                "Rain Dance", "Tailwind", "Brave Bird", "Hurricane"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(607, "Litwick", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body",
            "Infiltrator",
            50, 190, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.3f, 3.1f,
            55, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.WHITE, 70,
            "Candle",
            "Its flame is usually out, but it starts shining when it absorbs life force from people or Pokémon.",
            50, 30, 55, 65, 55, 20, 0.3f, new Color(0.62f, 0.37f, 1, 1),
            new int[] {1, 1, 3, 5, 7, 10, 13, 16, 20, 24, 28, 33, 38, 43, 49, 55, 61},
            new string[]
            {
                "Ember", "Astonish", "Minimize", "Smog", "Fire Spin", "Confuse Ray", "Night Shade", "Will-O-Wisp",
                "Flame Burst", "Imprision", "Hex", "Memento", "Inferno", "Curse", "Shadow Ball", "Pain Split",
                "Overheat"
            },
            new string[] {},
            new int[] {608}, new string[] {"Level,41"}),
        new PokemonData(608, "Lampent", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body",
            "Infiltrator",
            50, 90, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 0.6f, 13,
            130, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.BLACK, 70,
            "Lamp",
            "The spirits it absorbs fuel its baleful fire. It hangs around hospitals waiting for people to pass on.",
            60, 40, 60, 95, 60, 55, 0.4f, new Color(0.62f, 0.37f, 1, 1),
            new int[] {1, 1, 1, 1, 3, 5, 7, 10, 13, 16, 20, 24, 28, 33, 38, 45, 53, 61, 69},
            new string[]
            {
                "Ember", "Astonish", "Minimize", "Smog", "Minimize", "Smog", "Fire Spin", "Confuse Ray", "Night Shade",
                "Will-O-Wisp", "Flame Burst", "Imprison", "Hex", "Memento", "Inferno", "Curse", "Shadow Ball",
                "Pain Split", "Overheat"
            },
            new string[] {},
            new int[] {609}, new string[] {"Stone,Dusk Stone"}),
        new PokemonData(609, "Chandelure", PokemonData.Type.GHOST, PokemonData.Type.FIRE, "Flash Fire", "Flame Body",
            "Infiltrator",
            50, 45, PokemonData.EggGroup.AMORPHOUS, PokemonData.EggGroup.NONE, 5355, 1, 34.3f,
            234, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.BLACK, 70,
            "Luring", "Being consumed in Chandelure's flame burns up the spirit, leaving the body behind.",
            60, 55, 90, 145, 90, 80, 0.55f, new Color(0.62f, 0.37f, 1, 1),
            new int[] {1, 1, 1, 1, 1},
            new string[] {"Pain Split", "Smog", "Confuse Ray", "Flame Burst", "Hex"},
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(650, "Chespin", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Bulletproof",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 9,
            63, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 1, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Spiny Nut",
            "Such a thick shell of wood covers its head and back that even a direct hit from a truck wouldn't faze it.",
            56, 61, 65, 48, 45, 38, 0, Color.clear,
            new int[] {1, 1, 5, 8, 11, 15, 18, 27, 32, 35, 39, 42, 45, 48},
            new string[]
            {
                "Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", "Pin Missile", "Take Down",
                "Seed Bomb", "Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"
            },
            new string[] {},
            new int[] {651}, new string[] {"Level,16"}),
        new PokemonData(651, "Quilladin", PokemonData.Type.GRASS, PokemonData.Type.NONE, "Overgrow", null, "Bulletproof",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.7f, 29,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 2, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Spiny Armor",
            "They strengthen their lower bodies by running into one another. They are very kind and won't start fights.",
            61, 78, 95, 56, 58, 57, 0, Color.clear,
            new int[] {1, 1, 5, 8, 11, 15, 20, 26, 30, 35, 39, 44, 48, 52, 55},
            new string[]
            {
                "Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed", "Pin Missile", "Needle Arm",
                "Take Down", "Seed Bomb", "Mud Shot", "Bulk Up", "Body Slam", "Pain Split", "Wood Hammer"
            },
            new string[] {},
            new int[] {652}, new string[] {"Level,36"}),
        new PokemonData(652, "Chesnaught", PokemonData.Type.GRASS, PokemonData.Type.FIGHTING, "Overgrow", null,
            "Bulletproof",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.6f, 90,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 3, 0, 0, 0, PokemonData.PokedexColor.GREEN, 70,
            "Spiny Armor",
            "When it takes a defensive posture with its fists guarding its face, it could withstand a bomb blast.",
            88, 107, 122, 74, 75, 64, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 8, 11, 15, 20, 26, 30, 35, 36, 41, 44, 48, 52, 55, 60, 70, 75},
            new string[]
            {
                "Feint", "Hammer Arm", "Belly Drum", "Tackle", "Growl", "Vine Whip", "Rollout", "Bite", "Leech Seed",
                "Pin Missile", "Needle Arm", "Take Down", "Seed Bomb", "Spiky Shield", "Mud Shot", "Bulk Up",
                "Body Slam", "Pain Split", "Wood Hammer", "Hammer Arm", "Giga Impact", "Spiky Shield"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(653, "Fennekin", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Magician",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 0.4f, 9.4f,
            61, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 1, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Fox",
            "As it walks, it munches on a twig in place of a snack. It intimidates opponents by puffing hot air out of its ears.",
            40, 45, 40, 62, 60, 60, 0, Color.clear,
            new int[] {1, 1, 5, 11, 14, 17, 20, 25, 27, 31, 35, 38, 41, 43, 46, 48},
            new string[]
            {
                "Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant",
                "Light Screen", "Psyshock", "Flamethrower", "Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room",
                "Fire Blast"
            },
            new string[] {},
            new int[] {654}, new string[] {"Level,16"}),
        new PokemonData(654, "Braixen", PokemonData.Type.FIRE, PokemonData.Type.NONE, "Blaze", null, "Magician",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1, 14.5f,
            143, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 2, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Fox",
            "It has a twig stuck in its tail. With friction from its tail fur, it sets the twig on fire and launches into battle.",
            59, 59, 58, 90, 70, 73, 0, Color.clear,
            new int[] {1, 1, 5, 11, 14, 18, 22, 27, 30, 34, 41, 45, 48, 51, 53, 55},
            new string[]
            {
                "Scratch", "Tail Whip", "Ember", "Howl", "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant",
                "Light Screen", "Psyshock", "Flamethrower", "Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room",
                "Fire Blast"
            },
            new string[] {},
            new int[] {655}, new string[] {"Level,36"}),
        new PokemonData(655, "Delphox", PokemonData.Type.FIRE, PokemonData.Type.PSYCHIC, "Blaze", null, "Magician",
            87.5f, 45, PokemonData.EggGroup.FIELD, PokemonData.EggGroup.NONE, 5355, 1.5f, 39,
            240, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 3, 0, 0, PokemonData.PokedexColor.RED, 70,
            "Fox",
            "It gazes into the flame at the tip of its branch to achieve a focused state, which allows it to see into the future.",
            75, 69, 72, 114, 100, 104, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 1, 5, 11, 14, 18, 22, 27, 30, 34, 36, 42, 47, 51, 55, 58, 61, 69, 75},
            new string[]
            {
                "Future Sight", "Role Play", "Switcheroo", "Shadow Ball", "Scratch", "Tailwhip", "Ember", "Howl",
                "Flame Charge", "Psybeam", "Fire Spin", "Lucky Chant", "Light Screen", "Psyshock", "Mystical Fire",
                "Flamethrower", "Will-O-Wisp", "Psychic", "Sunny Day", "Magic Room", "Fire Blast", "Future Sight",
                "Mystical Fire"
            },
            new string[] {},
            new int[] {}, new string[] {}),
        new PokemonData(656, "Froakie", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Protean",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 0.3f, 7,
            63, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 1, PokemonData.PokedexColor.BLUE, 70,
            "Bubble Frog",
            "It secretes flexible bubbles from its chest and back. The bubbles reduce the damage it would otherwise take when attacked.",
            41, 56, 40, 62, 44, 71, 0, Color.clear,
            new int[] {1, 1, 5, 8, 10, 14, 18, 21, 25, 29, 35, 39, 43, 48},
            new string[]
            {
                "Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling",
                "Smack Down", "Substitue", "Bounce", "Double Team", "Hydro Pump"
            },
            new string[] {},
            new int[] {657}, new string[] {"Level,16"}),
        new PokemonData(657, "Frogadier", PokemonData.Type.WATER, PokemonData.Type.NONE, "Torrent", null, "Protean",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 0.6f, 10.9f,
            142, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 2, PokemonData.PokedexColor.BLUE, 70,
            "Bubble Frog",
            "It can throw bubble-covered pebbles with precise control, hitting empty cans up to a hundred feet away.",
            54, 63, 52, 83, 56, 97, 0, Color.clear,
            new int[] {1, 1, 5, 8, 10, 14, 20, 23, 28, 33, 38, 44, 48, 55},
            new string[]
            {
                "Pound", "Growl", "Bubble", "Quick Attack", "Lick", "Water Pulse", "Smokescreen", "Round", "Fling",
                "Smack Down", "Substitute", "Bounce", "Double Team", "Hydro Pump"
            },
            new string[] {},
            new int[] {658}, new string[] {"Level,36"}),
        new PokemonData(658, "Greninja", PokemonData.Type.WATER, PokemonData.Type.DARK, "Torrent", null, "Protean",
            87.5f, 45, PokemonData.EggGroup.WATER1, PokemonData.EggGroup.NONE, 5355, 1.5f, 40,
            239, PokemonData.LevelingRate.MEDIUMSLOW, 0, 0, 0, 0, 0, 3, PokemonData.PokedexColor.BLUE, 70,
            "Ninja",
            "It appears and vanishes with a ninja’s grace. It toys with its enemies using swift movements, while slicing them with throwing stars of sharpest water.",
            72, 95, 67, 103, 71, 122, 0, Color.clear,
            new int[] {1, 1, 1, 1, 1, 5, 8, 10, 14, 20, 23, 28, 33, 36, 43, 49, 52, 56, 60, 70, 75},
            new string[]
            {
                "Night Slash", "Role Play", "Mat Block", "Pound", "Growl", "Bubble", "Quick Attack", "Lick",
                "Water Pulse", "Smokescreen", "Shadow Sneak", "Spikes", "Feint Attack", "Water Shuriken", "Substitute",
                "Extrasensory", "Double Team", "Haze", "Hydro Pump", "Night Slash", "Water Shuriken"
            },
            new string[] {},
            new int[] {}, new string[] {})
    };


    private static int[] expTableErratic = new int[]
    {
        0, 15, 52, 122, 237, 406, 637, 942, 1326, 1800,
        2369, 3041, 3822, 4719, 5737, 6881, 8155, 9564, 11111, 12800,
        14632, 16610, 18737, 21012, 23437, 26012, 28737, 31610, 34632, 37800,
        41111, 44564, 48155, 51881, 55737, 59719, 63822, 68041, 72369, 76800,
        81326, 85942, 90637, 95406, 100237, 105122, 110052, 115105, 120001, 125000,
        131324, 137795, 144410, 151165, 158056, 165079, 172229, 179503, 186894, 194400,
        202013, 209728, 217540, 225443, 233431, 241496, 249633, 257834, 267406, 276458,
        286328, 296358, 305767, 316074, 326531, 336255, 346965, 357812, 567807, 378880,
        390077, 400293, 411686, 423190, 433572, 445239, 457001, 467489, 479378, 491346,
        501878, 513934, 526049, 536557, 548720, 560922, 571333, 583539, 591882, 600000
    };

    private static int[] expTableFast = new int[]
    {
        0, 6, 21, 51, 100, 172, 274, 409, 583, 800,
        1064, 1382, 1757, 2195, 2700, 3276, 3930, 4665, 5487, 6400,
        7408, 8518, 9733, 11059, 12500, 14060, 15746, 17561, 19511, 21600,
        23832, 26214, 28749, 31443, 34300, 37324, 40522, 43897, 47455, 51200,
        55136, 59270, 63605, 68147, 72900, 77868, 83058, 88473, 94119, 100000,
        106120, 112486, 119101, 125971, 133100, 140492, 148154, 156089, 164303, 172800,
        181584, 190662, 200037, 209715, 219700, 229996, 240610, 251545, 262807, 274400,
        286328, 298598, 311213, 324179, 337500, 351180, 365226, 379641, 394431, 409600,
        425152, 441094, 457429, 474163, 491300, 508844, 526802, 545177, 563975, 583200,
        602856, 622950, 643485, 664467, 685900, 707788, 730138, 752953, 776239, 800000
    };
>>>>>>> refs/remotes/origin/master

    private static int[] expTableMediumFast = new int[]
    {
        0, 8, 27, 64, 125, 216, 343, 512, 729, 1000,
        1331, 1728, 2197, 2744, 3375, 4096, 4913, 5832, 6859, 8000,
        9261, 10648, 12167, 13824, 15625, 17576, 19683, 21952, 24389, 27000,
        29791, 32768, 35937, 39304, 42875, 46656, 50653, 54872, 59319, 64000,
        68921, 74088, 79507, 85184, 91125, 97336, 103823, 110592, 117649, 125000,
        132651, 140608, 148877, 157464, 166375, 175616, 185193, 195112, 205379, 216000,
        226981, 238328, 250047, 262144, 274625, 287496, 300763, 314432, 328509, 343000,
        357911, 373248, 389017, 405224, 421875, 438976, 456533, 474552, 493039, 512000,
        531441, 551368, 571787, 592704, 614125, 636056, 658503, 681472, 704969, 729000,
        753571, 778688, 804357, 830584, 857375, 884736, 912673, 941192, 970299, 1000000
    };

    private static int[] expTableMediumSlow = new int[]
    {
        0, 9, 57, 96, 135, 179, 236, 314, 419, 560,
        742, 973, 1261, 1612, 2035, 2535, 3120, 3798, 4575, 5460,
        6458, 7577, 8825, 10208, 11735, 13411, 15244, 17242, 19411, 21760,
        24294, 27021, 29949, 33084, 36435, 40007, 43808, 47846, 52127, 56660,
        61450, 66505, 71833, 77440, 83335, 89523, 96012, 102810, 109923, 117360,
        125126, 133229, 141677, 150476, 159635, 169159, 179056, 189334, 199999, 211060,
        222522, 234393, 246681, 259392, 272535, 286115, 300140, 314618, 329555, 344960,
        360838, 377197, 394045, 411388, 429235, 447591, 466464, 485862, 505791, 526260,
        547274, 568841, 590969, 613664, 636935, 660787, 685228, 710266, 735907, 762160,
        789030, 816525, 844653, 873420, 902835, 932903, 963632, 995030, 1027103, 1059860
    };

    private static int[] expTableSlow = new int[]
    {
        0, 10, 33, 80, 156, 270, 428, 640, 911, 1250,
        1663, 2160, 2746, 3430, 4218, 5120, 6141, 7290, 8573, 10000,
        11576, 13310, 15208, 17280, 19531, 21970, 24603, 27440, 30486, 33750,
        37238, 40960, 44921, 49130, 53593, 58320, 63316, 68590, 74148, 80000,
        86151, 92610, 99383, 106480, 113906, 121670, 129778, 138240, 147061, 156250,
        165813, 175760, 186096, 196830, 207968, 219520, 231491, 243890, 256723, 270000,
        283726, 297910, 312558, 327680, 343281, 359370, 375953, 393040, 410636, 428750,
        447388, 466560, 486271, 506530, 527343, 548720, 570666, 593190, 616298, 640000,
        664301, 689210, 714733, 740880, 767656, 795070, 823128, 851840, 881211, 911250,
        941963, 973360, 1005446, 1038230, 1071718, 1105920, 1140841, 1176490, 1212873, 1250000
    };

    private static int[] expTableFluctuating = new int[]
    {
        0, 4, 13, 32, 65, 112, 178, 276, 393, 540,
        745, 967, 1230, 1591, 1957, 2457, 3046, 3732, 4526, 5440,
        6482, 7666, 9003, 10506, 12187, 14060, 16140, 18439, 20974, 23760,
        26811, 30146, 33780, 37731, 42017, 46656, 50653, 55969, 60505, 66560,
        71677, 78533, 84277, 91998, 98415, 107069, 114205, 123863, 131766, 142500,
        151222, 163105, 172697, 185807, 196322, 210739, 222231, 238036, 250562, 267840,
        281456, 300293, 315059, 335544, 351520, 373744, 390991, 415050, 433631, 459620,
        479600, 507617, 529063, 559209, 582187, 614566, 639146, 673863, 700115, 737280,
        765275, 804997, 834809, 877201, 908905, 954084, 987754, 1035837, 1071552, 1122660,
        1160499, 1214753, 1254796, 1312322, 1354652, 1415577, 1460276, 1524731, 1571884, 1640000
    };

<<<<<<< HEAD
	private static int[] expTableFluctuating = new int[]{
		0,4,13,32,65,112,178,276,393,540,
		745,967,1230,1591,1957,2457,3046,3732,4526,5440,
		6482,7666,9003,10506,12187,14060,16140,18439,20974,23760,
		26811,30146,33780,37731,42017,46656,50653,55969,60505,66560,
		71677,78533,84277,91998,98415,107069,114205,123863,131766,142500,
		151222,163105,172697,185807,196322,210739,222231,238036,250562,267840,
		281456,300293,315059,335544,351520,373744,390991,415050,433631,459620,
		479600,507617,529063,559209,582187,614566,639146,673863,700115,737280,
		765275,804997,834809,877201,908905,954084,987754,1035837,1071552,1122660,
		1160499,1214753,1254796,1312322,1354652,1415577,1460276,1524731,1571884,1640000};
	#endregion

	public static PokemonData getPokemon(int ID){
		Debug.Log("Get Pokemons");
		PokemonData result = null;
		int i = 1;
		while(result == null){
			if(pokedex()[i].getID() == ID){
				Debug.Log("Pokemon DB Success");
				result = pokedex()[i];
			}
			i += 1;
			if(i >= pokedex().Length){
				Debug.Log("Pokemon DB Fail");
				return null;}
		}
		return result;
	}

	public static int getLevelExp(PokemonData.LevelingRate levelingRate, int currentLevel){
		int exp = 0;
		if (currentLevel > 100){
			currentLevel = 100;
		}
		if(levelingRate == PokemonData.LevelingRate.ERRATIC){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor((Mathf.Pow(currentLevel,3))*(160-currentLevel)/100);
			} else exp = expTableErratic[currentLevel-1]; //Because the array starts at 0, not 1.
		}
		else if(levelingRate == PokemonData.LevelingRate.FAST){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor(Mathf.Pow(currentLevel,3)*(4/5));
			} else	exp = expTableFast[currentLevel-1];
		}
		else if(levelingRate == PokemonData.LevelingRate.MEDIUMFAST){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor(Mathf.Pow(currentLevel,3));
			} else exp = expTableMediumFast[currentLevel-1];
		}
		else if(levelingRate == PokemonData.LevelingRate.MEDIUMSLOW){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor(((6/5)*Mathf.Pow(currentLevel-1,3))-(15 * Mathf.Pow(currentLevel - 1,3))+(100*(currentLevel-1))-140);
			} else exp = expTableMediumSlow[currentLevel-1];
		}
		else if(levelingRate == PokemonData.LevelingRate.SLOW){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor(Mathf.Pow(currentLevel,3)*(5/4));
			} else exp = expTableSlow[currentLevel-1];
		}
		else if(levelingRate == PokemonData.LevelingRate.FLUCTUATING){
			if(currentLevel > 100){
				exp = (int)Mathf.Floor(Mathf.Pow(currentLevel,3)*((Mathf.Floor(Mathf.Pow(currentLevel,3)/2)+32)/50));
			} else exp = expTableFluctuating[currentLevel-1];
		}

		return exp;


	}
	
}
=======
    public static PokemonData getPokemon(int ID)
    {
        PokemonData result = null;
        int i = 1;
        while (result == null)
        {
            if (pokedex[i].getID() == ID)
            {
                result = pokedex[i];
            }
            i += 1;
            if (i >= pokedex.Length)
            {
                return null;
            }
        }
        return result;
    }

    public static int getLevelExp(PokemonData.LevelingRate levelingRate, int currentLevel)
    {
        int exp = 0;
        if (currentLevel > 100)
        {
            currentLevel = 100;
        }
        if (levelingRate == PokemonData.LevelingRate.ERRATIC)
        {
            exp = expTableErratic[currentLevel - 1]; //Because the array starts at 0, not 1.
        }
        else if (levelingRate == PokemonData.LevelingRate.FAST)
        {
            exp = expTableFast[currentLevel - 1];
        }
        else if (levelingRate == PokemonData.LevelingRate.MEDIUMFAST)
        {
            exp = expTableMediumFast[currentLevel - 1];
        }
        else if (levelingRate == PokemonData.LevelingRate.MEDIUMSLOW)
        {
            exp = expTableMediumSlow[currentLevel - 1];
        }
        else if (levelingRate == PokemonData.LevelingRate.SLOW)
        {
            exp = expTableSlow[currentLevel - 1];
        }
        else if (levelingRate == PokemonData.LevelingRate.FLUCTUATING)
        {
            exp = expTableFluctuating[currentLevel - 1];
        }

        return exp;
    }
}
>>>>>>> refs/remotes/origin/master
