using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using UnityEngine;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	#region Encounter Scripts
	/// <summary>
	/// Idea 1 for Pokemon Encounters
	/// </summary>
	class WildPokemon
	{
		public TravelGround Ground { get; private set; }
		public TravelWater Water { get; private set; }
		public TravelAir Air { get; private set; }

		public WildPokemon(TravelGround ground = null, TravelWater water = null, TravelAir air = null)
		{

		}

		#region Database
		private static WildPokemon[] EncounterDatabase
		{
			get
			{
				return new WildPokemon[]
				{
				new WildPokemon(
					new TravelGround()
				)
				};
			}
		}
		#endregion

		#region Nested Classes
		public class MapPokemon
		{
			PokemonUnity.Pokemons Pokemon { get; set; }
			int minLevel { get; set; }
			int maxLevel { get; set; }
		}
		//ToDo: Make Interface Class for Slots
		//ToDo: Overworld Tile should return which Class it belongs to
		public class TravelGround : ITravel
		{
			/// <summary>
			/// After Pokemon encounter dice has been rolled, this is the output from results
			/// </summary>
			public MapPokemon Slot { get; private set; }
			/// <summary>
			/// How often or likely to engage and encounter a pokemon
			/// </summary>
			public int Rate { get; private set; }
			#region Walking
			/// <summary>
			/// 20%
			/// </summary>
			public int Slot1 { get; set; }
			/// <summary>
			/// 20%
			/// </summary>
			public int Slot2 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			public int Slot3 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			public int Slot4 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			public int Slot5 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			public int Slot6 { get; set; }
			/// <summary>
			/// 5%
			/// </summary>
			public int Slot7 { get; set; }
			/// <summary>
			/// 5%
			/// </summary>
			public int Slot8 { get; set; }
			/// <summary>
			/// 4%
			/// </summary>
			public int Slot9 { get; set; }
			/// <summary>
			/// 4%
			/// </summary>
			public int Slot10 { get; set; }
			/// <summary>
			/// 1%
			/// </summary>
			public int Slot11 { get; set; }
			/// <summary>
			/// 1%
			/// </summary>
			public int Slot12 { get; set; }
			#endregion
			#region Swarm
			/// <summary>
			/// 20%
			/// </summary>
			int SwarmSlot1 { get; set; }
			/// <summary>
			/// 20%
			/// </summary>
			int SwarmSlot2 { get; set; }
			#endregion
			#region Day
			/// <summary>
			/// 10%
			/// </summary>
			int DaySlot1 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			int DaySlot2 { get; set; }
			#endregion
			#region Night
			/// <summary>
			/// 10%
			/// </summary>
			int NightSlot1 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			int NightSlot2 { get; set; }
			#endregion
			#region PokeRadar
			/// <summary>
			/// 10%
			/// </summary>
			int RadarSlot1 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			int RadarSlot2 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			int RadarSlot3 { get; set; }
			/// <summary>
			/// 10%
			/// </summary>
			int RadarSlot4 { get; set; }
			#endregion

			public TravelGround(Method? tile = null)
			{
				switch ((Generation)Core.pokemonGeneration)
				{
					case Generation.All:
					case Generation.RedBlueYellow:
					case Generation.GoldSilverCrystal:
					case Generation.RubySapphireEmerald:
					case Generation.DiamondPearlPlatinum:
					case Generation.BlackWhite:
					case Generation.XY:
					case Generation.SunMoon:
					case Generation.Custom:
					default:
						#region Switch Breakdown
						switch (tile.Value)
						{
							case Method.YELLOW_FLOWERS:
							case Method.PURPLE_FLOWERS:
							case Method.RED_FLOWERS:
							case Method.ROUGH_TERRAIN:
							case Method.WALK:
								Slot1 = 10;
								Slot2 = 10;
								Slot3 = 10;
								Slot4 = 10;
								Slot5 = 10;
								Slot6 = 10;
								Slot7 = 10;
								Slot8 = 10;
								Slot9 = 10;
								Slot10 = 5;
								Slot11 = 4;
								Slot12 = 1;
								break;
							case Method.ROCK_SMASH:
								Slot1 = 50;
								Slot2 = 30;
								Slot3 = 15;
								Slot4 = 4;
								Slot5 = 1;
								//Slot6 = 10;
								//Slot7 = 5;
								//Slot8 = 5;
								//Slot9 = 4;
								//Slot10 = 4;
								//Slot11 = 1;
								//Slot12 = 1;
								break;
							case Method.DARK_GRASS:
							case Method.GRASS_SPOTS:
							case Method.CAVE_SPOTS:
							case Method.BRIDGE_SPOTS:
								Slot1 = 20;
								Slot2 = 20;
								Slot3 = 10;
								Slot4 = 10;
								Slot5 = 10;
								Slot6 = 10;
								Slot7 = 5;
								Slot8 = 5;
								Slot9 = 4;
								Slot10 = 4;
								Slot11 = 1;
								Slot12 = 1;
								break;
							case Method.OLD_ROD:
							case Method.GOOD_ROD:
							case Method.SUPER_ROD:
							case Method.SUPER_ROD_SPOTS:
							case Method.SURF:
							case Method.SURF_SPOTS:
							case Method.HEADBUTT:
							default:
								Slot1 = 0;
								Slot2 = 0;
								Slot3 = 0;
								Slot4 = 0;
								Slot5 = 0;
								Slot6 = 0;
								Slot7 = 0;
								Slot8 = 0;
								Slot9 = 0;
								Slot10 = 0;
								Slot11 = 0;
								Slot12 = 0;
								break;
						}
						#endregion
						break;
				}
			}
		}
		public class TravelWater
		{
			#region Surfing
			/// <summary>
			/// How often or likely to engage and encounter a pokemon
			/// </summary>
			/// 10
			int Rate { get; set; }
			/// <summary>
			/// 60%
			/// </summary>
			byte Slot1 { get; set; }
			/// <summary>
			/// 30%
			/// </summary>
			byte Slot2 { get; set; }
			/// <summary>
			/// 5%
			/// </summary>
			byte Slot3 { get; set; }
			/// <summary>
			/// 4%
			/// </summary>
			byte Slot4 { get; set; }
			/// <summary>
			/// 1%
			/// </summary>
			byte Slot5 { get; set; }
			#endregion
			//#region Old Rod
			///// <summary>
			///// How often or likely to engage and encounter a pokemon
			///// </summary>
			///// 25
			//int Rate2 { get; set; }
			//#endregion
			//#region Good Rod
			///// <summary>
			///// How often or likely to engage and encounter a pokemon
			///// </summary>
			///// 50
			//int Rate3 { get; set; }
			//#endregion
			//#region Super Rod
			///// <summary>
			///// How often or likely to engage and encounter a pokemon
			///// </summary>
			///// 75
			//int Rate4 { get; set; }
			//#endregion

			public TravelWater(Method? tile = null)
			{
				switch ((Generation)Core.pokemonGeneration)
				{
					case Generation.All:
					case Generation.RedBlueYellow:
					case Generation.GoldSilverCrystal:
					case Generation.RubySapphireEmerald:
					case Generation.DiamondPearlPlatinum:
					case Generation.BlackWhite:
					case Generation.XY:
					case Generation.SunMoon:
					case Generation.Custom:
					default:
						#region Switch Breakdown
						switch (tile.Value)
						{
							case Method.ROCK_SMASH:
								Slot1 = 50;
								Slot2 = 30;
								Slot3 = 15;
								Slot4 = 4;
								Slot5 = 1;
								//Slot6 = 10;
								//Slot7 = 5;
								//Slot8 = 5;
								//Slot9 = 4;
								//Slot10 = 4;
								//Slot11 = 1;
								//Slot12 = 1;
								break;
							case Method.OLD_ROD:
								Rate = 25;
								Slot1 = 60;
								Slot2 = 30;
								Slot3 = 5;
								Slot4 = 4;
								Slot5 = 1;
								break;
							case Method.GOOD_ROD:
								Rate = 50;
								Slot1 = 60;
								Slot2 = 30;
								Slot3 = 5;
								Slot4 = 4;
								Slot5 = 1;
								break;
							case Method.SUPER_ROD_SPOTS:
							case Method.SUPER_ROD:
								Rate = 75;
								Slot1 = 60;
								Slot2 = 30;
								Slot3 = 5;
								Slot4 = 4;
								Slot5 = 1;
								break;
							case Method.SURF_SPOTS:
							case Method.SURF:
								Rate = 10;
								Slot1 = 60;
								Slot2 = 30;
								Slot3 = 5;
								Slot4 = 4;
								Slot5 = 1;
								break;
							case Method.YELLOW_FLOWERS:
							case Method.PURPLE_FLOWERS:
							case Method.RED_FLOWERS:
							case Method.ROUGH_TERRAIN:
							case Method.WALK:
							case Method.DARK_GRASS:
							case Method.GRASS_SPOTS:
							case Method.CAVE_SPOTS:
							case Method.BRIDGE_SPOTS:
							case Method.HEADBUTT:
							default:
								Rate = 0;
								Slot1 = 0;
								Slot2 = 0;
								Slot3 = 0;
								Slot4 = 0;
								Slot5 = 0;
								break;
						}
						#endregion
						break;
				}
			}
		}
		public class TravelAir
		{

		}
		public interface ITravel
		{
			int Rate { get; }
			MapPokemon Slot { get; }
		}
		#endregion
	}

	/// <summary>
	/// Idea 2 for Pokemon Encounters
	/// </summary>
	class PokemonEncounter
	{
		public int MapId { get; private set; }
		public Method EncounterMethod { get; private set; }
		/// <summary>
		/// Maximum of 12 Slots.
		/// Depending on <see cref="EncounterMethod"/> max slots is less
		/// </summary>
		public EncounterSlot[] EncounterSlots { get; private set; }

		const int LandTerrainSlots = 12;
		const int WaterTerrainSlots = 5;

		/// <summary>
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate { get; set; }
		#region Misc Fishing Rod Encounter Rates
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 25%
		/// </summary>
		int OldRod { get; set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 50%
		/// </summary>
		int GoodRod { get; set; }
		/// <summary>
		/// How often or likely to engage and encounter a pokemon.
		/// Default 75%
		/// </summary>
		int SuperRod { get; set; }
		#endregion
		#region Misc Species Encounter Chances
		/// <summary>
		/// Two slots of 20% each
		/// </summary>
		EncounterSlot[] SwarmSlot { get; set; }
		/// <summary>
		/// Two slots of 10% each
		/// </summary>
		EncounterSlot[] DaySlot { get; set; }
		/// <summary>
		/// Two slots of 10% each
		/// </summary>
		EncounterSlot[] NightSlot { get; set; }
		/// <summary>
		/// Four slots of 10% each
		/// </summary>
		EncounterSlot[] RadarSlot { get; set; }
		#endregion

		PokemonEncounter()
		{
			EncounterSlots = new EncounterSlot[12];
		}

		PokemonEncounter(int map) : this()
		{
			foreach (PokemonEncounter mapEncounter in EncounterDatabase)
			{
				//if (mapEncounter.MapId == map) return mapEncounter;
			}
		}

		private static PokemonEncounter[] EncounterDatabase
		{
			get
			{
				return new PokemonEncounter[]
				{

				};
			}
		}

		public Pokemon EncounterRNG()
		{
			return new Pokemon(PokemonUnity.Pokemons.NONE);
		}

		public class EncounterSlot
		{
			PokemonUnity.Pokemons Pokemon { get; set; }
			int MinLevel { get; set; }
			int MaxLevel { get; set; }
		}
	}

	/// <summary>
	/// Old Encounter Script I wrote months ago,
	/// Planning on recycling contents to generate something better/newer...
	/// </summary>
	public class WildPokemonInitialiser
	{
		public int ID;
		public int minLevel;
		public int maxLevel;

		public int encounterLikelihood;

		public Method encounterLocation;

		public bool encounterMorning = true;
		public bool encounterDay = true;
		public bool encounterNight = true;

		public WildPokemonInitialiser[] encounters = new WildPokemonInitialiser[0];

		// returns the probability of encounter
		/*public float getEncounterProbability()
		{
			float x = 1.25f;
			if (pokemonRarity == PokemonRarity.VeryCommon)
			{
				x = 10f;
			}
			else if (pokemonRarity == PokemonRarity.Common)
			{
				x = 8.5f;
			}
			else if (pokemonRarity == PokemonRarity.Uncommon)
			{
				x = 6.75f;
			}
			else if (pokemonRarity == PokemonRarity.Rare)
			{
				x = 3.33f;
			}
			return x / 187.5f;
		}*/

		public WildPokemonInitialiser[] getEncounterList(Method location)
		{
			WildPokemonInitialiser[] list = new WildPokemonInitialiser[encounters.Length];
			int listIndex = 0;

			float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);

			for (int i = 0; i < encounters.Length; i++)
			{
				if (encounters[i].encounterLocation == location)
				{
					if (time >= 20 || time < 3.5f)
					{
						//night
						if (encounters[i].encounterNight)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
					else if (time < 10f)
					{
						//morning
						if (encounters[i].encounterMorning)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
					else
					{
						//day
						if (encounters[i].encounterDay)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
				}
			}

			WildPokemonInitialiser[] packedList = new WildPokemonInitialiser[listIndex];

			for (int i = 0; i < packedList.Length; i++)
			{
				packedList[i] = list[i];
			}

			return packedList;
		}

		public Pokemon getRandomEncounter(Method location)
		{
			WildPokemonInitialiser[] list = getEncounterList(location);

			int totalEncounterLikelihood = 0; //add up the total Likelihood
			for (int i = 0; i < list.Length; i++)
			{
				totalEncounterLikelihood += list[i].encounterLikelihood;
			}

			WildPokemonInitialiser[] chanceSplitList = new WildPokemonInitialiser[totalEncounterLikelihood];
			int listIndex = 0;
			for (int i = 0; i < list.Length; i++)
			{
				//loop through each position of list
				for (int i2 = 0; i2 < list[i].encounterLikelihood; i2++)
				{
					//add encounter once for every Likelihood
					chanceSplitList[listIndex] = list[i];
					listIndex += 1;
				}
			}
			//randomly pick a number from the list's length
			int encounterIndex = Core.Rand.Next(0, chanceSplitList.Length);

#if DEBUG
			//string debugtext = "";
			for (int i = 0; i < chanceSplitList.Length; i++)
			{
				//debugtext += PokemonDatabaseOld.getPokemon(chanceSplitList[i].ID).getName() + ", ";
			}
			//Debug.Log(encounterIndex + ": " + debugtext + "(" + PokemonDatabaseOld.getPokemon(chanceSplitList[encounterIndex].ID).getName() + ")");
#endif

			return new Pokemon();
			// chanceSplitList[encounterIndex].ID, PokemonOld.Gender.CALCULATE,
			//Core.Rand.Next(chanceSplitList[encounterIndex].minLevel, chanceSplitList[encounterIndex].maxLevel + 1),
			//null, null, null, -1);
		}

		/// <summary>
		/// Random Wild Pokemon Encounter 2.0
		/// Pools all the values into a 100% encounter chance, and selects from those results
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public Pokemon getRandomEncounter(Method location, string placeholder)
		{
			//Get list of 100 pokemons for given (specific to this) encounter...
			WildPokemonInitialiser[] list = getEncounterList(location);

			//From list of 100 pokemons, select 1.
			int randomPokemon = Core.Rand.Next(0, 100);

			/*int totalEncounterLikelihood = 0; //add up the total Likelihood
			for (int i = 0; i < list.Length; i++)
			{
				totalEncounterLikelihood += list[i].encounterLikelihood;
			}

			WildPokemonInitialiser[] chanceSplitList = new WildPokemonInitialiser[totalEncounterLikelihood];
			int listIndex = 0;
			for (int i = 0; i < list.Length; i++)
			{
				//loop through each position of list
				for (int i2 = 0; i2 < list[i].encounterLikelihood; i2++)
				{
					//add encounter once for every Likelihood
					chanceSplitList[listIndex] = list[i];
					listIndex += 1;
				}
			}
			//randomly pick a number from the list's length
			int encounterIndex = Core.Rand.Next(0, chanceSplitList.Length);

	#if DEBUG
			string debugtext = "";
				for(int i = 0; i < chanceSplitList.Length; i++){
					debugtext += PokemonDatabase.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
				Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabase.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
	#endif
			*/
			return new Pokemon();
			// list[randomPokemon].ID, PokemonOld.Gender.CALCULATE,
			//Core.Rand.Next(list[randomPokemon].minLevel, list[randomPokemon].maxLevel + 1),
			//null, null, null, -1);
		}

		/// <summary>
		/// Random Encounter list for Pokemon Encounter 2.0
		/// </summary>
		/// <param name="location"></param>
		/// <returns></returns>
		public WildPokemonInitialiser[] getEncounterList(int mapId, Method method, ConditionValue[] condition = null)
		{
			WildPokemonInitialiser[] list = new WildPokemonInitialiser[100];
			int listIndex = 0;
			int version = 0; //MapId will play a part of what version
			int slots = 0; //Maybe an Array, matching Slots to given %chance ratio

			switch (method)
			{
				case Method.WALK:
				case Method.DARK_GRASS:
				case Method.GRASS_SPOTS:
				case Method.CAVE_SPOTS:
				case Method.BRIDGE_SPOTS:
				case Method.YELLOW_FLOWERS:
				case Method.PURPLE_FLOWERS:
				case Method.RED_FLOWERS:
				case Method.ROUGH_TERRAIN:
				case Method.HEADBUTT:
					slots = 12;
					for (int i = 0; i < list.Length; i++)
					{
						list[i] = new WildPokemonInitialiser { ID = 0, minLevel = 0, maxLevel = 0 };
					}
					break;
				case Method.OLD_ROD:
				case Method.GOOD_ROD:
				case Method.SUPER_ROD:
					slots = 3;
					break;
				case Method.SUPER_ROD_SPOTS:
				case Method.SURF_SPOTS:
				case Method.SURF:
					if (version == 1 || version == 2) slots = 10;
					else slots = 5;
					break;
				case Method.ROCK_SMASH:
					if (version == 10) slots = 2;
					else slots = 5;
					break;
				default:
					break;
			}

			float time = System.DateTime.Now.Hour + ((float)System.DateTime.Now.Minute / 60f);

			for (int i = 0; i < encounters.Length; i++)
			{
				if (encounters[i].encounterLocation == method)
				{
					if (time >= 20 || time < 3.5f)
					{
						//night
						if (encounters[i].encounterNight)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
					else if (time < 10f)
					{
						//morning
						if (encounters[i].encounterMorning)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
					else
					{
						//day
						if (encounters[i].encounterDay)
						{
							list[listIndex] = encounters[i];
							listIndex += 1;
						}
					}
				}
			}

			WildPokemonInitialiser[] packedList = new WildPokemonInitialiser[listIndex];

			for (int i = 0; i < packedList.Length; i++)
			{
				packedList[i] = list[i];
			}

			return packedList;
		}
	}
	/* Another Idea...
		#1
			The encounter code. Dictate how the Pokemon is encountered. There are currently 2 options:

			0 - Grass/floor
			1 - Headbutt trees

		#2
			This is the Pokemon code. Finds the Pokemon whose national dex number is given.

		#3
			The chance of encounter. This number decides how easy it will be to find this Pokemon. The higher the number the more likely it will be.

		#4
			Time code. This decides when this Pokemon will be available. There are 5 options:

			-1 - All times.
			0 - Night.
			1 - Morning.
			2 - Day.
			3 - Evening.
		#5
			The Minimum level.

		#6
			The maximum level. If the Pokemon should only appear at one level this number should be the same as ##5.
	 */
	#endregion
#pragma warning disable 0414, 0649 //Warning CS0414  ...is assigned but it's never used; 649 ...will always be null
	namespace Samples
	{
		using UnityEngine;

		/// ToDo: Store maps as GameObjects in code
		/// Find a way to associate code-variables to Maps
		/// Generate code to load maps based on above
		/// Consider each overworld Route (1,21,26, PalletTown...)
		/// a separate GameObject versus a single map
		/// Decide whether to load all GameObjects together or
		/// relative to/based on a player's given position
		class Map : MonoBehaviour
		{
			#region Idea1: Overworld section as 1 GameObject
			/// <summary>
			/// Set this variable in the inspector
			/// </summary>
			public GameObject mapPrefab;
			/// <summary>
			/// The Map-prefab's global position
			/// </summary>
			private Vector3[] objectPositions;

			private void Awake()
			{
				foreach (Vector3 objectPos in objectPositions)
				{
					GameObject mapObject = Instantiate(mapPrefab) as GameObject;
					mapObject.transform.position = objectPos;
				}
			}

			static Map()
			{
				//objectPositions = new Array[] {} //ToDo: List of map names and vectors
			}
			#endregion

			#region Idea2: Overworld tiles as individual GameObject
			// a 2D grid of cubes
			// every position in the grid must have a cube
			// the height is different for each position in the grid
			// The way would be to loop through every x position, and within that every z position.

			GameObject mapTile; 
 
			/// <summary>
			/// Build map from Array of GameObjects[]
			/// </summary>
			/// ToDo: How to save map as Array of Objects?
			/// ToDo: Blender-to-Unity => Read object name in if-check
			/// A tile Array for Location, Rotation, and Name
			void BuildMap() 
			{
				int minX = -20;
				int maxX = 20;
				int minY = 0;
				int maxY = 4;
				int minZ = -20;
				int maxZ = 20;
				// loop for every z position in the grid
				for (int z = minZ; z < maxZ; z++ )
				{
					// now loop for every x position in the grid
					for (int x = minX; x < maxX; x++ )
					{
						//loop for every y position in the grid
						for (int y = minY; y < maxY; y++ )
						{

							// put it all to together to assign a position
							Vector3 pos = new Vector3(x, y, z);

							// instantiate the cube into a variable, so you can do other things with it
							GameObject clone = (GameObject)Instantiate(mapTile, pos, Quaternion.identity);

							//ToDO: Add 'if(tile)' => collision-map

							// change the name of the object, include the x and z position in the name
							clone.name = "Tile_" + x.ToString() + "_" + z.ToString();

							// in future, 
							// this would be stored in a 3D array for future reference
							// so you could modify the position, material, anything!
							// cubeArray[x,y,z] = clone;
						}
					}
				}
			}
			#endregion

			#region Idea3: Overworld is a multidimensional ([,]) array of .scene (map chunks)
			/// <summary>
			/// .scenes is a multidimensional ([,]) array of map chunks
			/// stored as an int[,] but identified thru an enum, for organitional purposes
			/// </summary>
			class MapRegion
			{
				/// <summary>
				/// Diamond and Pearl 
				/// </summary>
				enum RegionDP { }
				int?[,] Region { get; set; }
				int?[,] RegionDiamondPearl()
				{
					return new int?[,]
					{
						{ },
						{ },
					};
				}
			}
			#endregion

			#region Idea4: 3d map chunk from 2d array
			/// <summary>
			/// Overworld excel grid of map headers 
			/// </summary>
			class MapMatrix
			{
				/// <summary>
				/// 
				/// </summary>
				/// enum of map matrix (i.e. Custom, PokemonDiamond, PokemonEmerald, etc...)
				/// if MAP then it allows you to load mapHeaders
				/// else it's a small chunk or dungeon
				int MapId;
				/// <summary>
				/// 
				/// </summary>
				/// enum label of matrix
				/// matrix id 0 is the overworld map
				int MatrxId;
				public int Height { get; private set; }
				public int Width { get; private set; }
				public MapChunk mapHeader { get; private set; }
			}
			/// <summary>
			/// Header data contains map x,y size
			/// </summary>
			class MapChunk
			{
				/// <summary>
				/// 
				/// </summary>
				/// Internal Name
				int MapId;
				string Name;
				/// <summary>
				/// Texture around Name when entering Map
				/// </summary>
				/// ToDo: Volcanic, Snow, Spring, etc...
				int NameStyle;
				int MapType;
				//int Texture1;
				//int Texture2;
				int Scripts;
				int MapScripts;
				int MusicDay;
				int MusicNight;
				int Texts;
				/// <summary>
				/// Table or Encounter chart for pokemons expected to find on map
				/// </summary>
				int WildPokemon;
				int Events;
				int Flags;
				int Weather;
				int Camera;
				/// <summary>
				/// </summary>
				/// For height loop or For width loop
				/// [Z,i] = gameobject int value
				/// each value in for loop should be rounded to nearest whole number
				/// need to map collision to X,Y value as well...
				int[,] MapArray;
			}
				#endregion

			#region Global and map metadata
			//ToDo: Each time map changes, new values are loaded/replaced below
			public class Global
			{
				/// <summary>
				/// Location you return to when you respawn
				/// </summary>
				public string MetadataHome;
				/// <summary>
				/// 
				/// </summary>
				/// String below should point to Audio/Sound files
				public string MetadataWildBattleBGM;
				public string MetadataTrainerBattleBGM;
				public string MetadataWildVictoryME;
				public string MetadataTrainerVictoryME;
				public string MetadataSurfBGM;
				public string MetadataBicycleBGM;
				/* TrainerClass
				Trainer MetadataPlayerA;
				Trainer MetadataPlayerB;
				Trainer MetadataPlayerC;
				Trainer MetadataPlayerD;
				Trainer MetadataPlayerE;
				Trainer MetadataPlayerF;
				Trainer MetadataPlayerG;
				Trainer MetadataPlayerH;*/
			}

			public class NonGlobalTypes : Global
			{
				bool MetadataOutdoor;
				bool MetadataShowArea;
				bool MetadataBicycle;
				bool MetadataBicycleAlways;
				/// <summary>
				/// 
				/// </summary>
				/// "uuu"
				int[,] MetadataHealingSpot;
				/// <summary>
				/// 
				/// </summary>
				/// return WeatherType
				bool MetadataWeather;
				/// <summary>
				/// 
				/// </summary>
				/// "uuu"
				int[] MetadataMapPosition;
				int MetadataDiveMap;
				bool MetadataDarkMap;
				bool MetadataSafariMap;
				bool MetadataSnapEdges;
				bool MetadataDungeon;
				/// <summary>
				/// 
				/// </summary>
				/// String below should point to Audio/Sound files
				public string MetadataBattleBack;
				//public string MetadataMapWildBattleBGM;
				//public string MetadataMapTrainerBattleBGM;
				//public string MetadataMapWildVictoryME;
				//public string MetadataMapTrainerVictoryME;
				int[,] MetadataMapSize;
			}
			#endregion
		}
	}
#pragma warning restore 0414, 0649 //Warning CS0414  ...is assigned but it's never used; 649 ...will always be null
}