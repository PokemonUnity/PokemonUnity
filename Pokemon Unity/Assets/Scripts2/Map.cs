using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;
//using UnityEngine;
using PokemonUnity;
using PokemonUnity.Pokemon;

namespace PokemonUnity.Samples
{using UnityEngine;
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
}
}

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
			switch ((Generation)Settings.pokemonGeneration)
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
							Slot1	= 10;
							Slot2	= 10;
							Slot3	= 10;
							Slot4	= 10;
							Slot5	= 10;
							Slot6	= 10;
							Slot7	= 10;
							Slot8	= 10;
							Slot9	= 10;
							Slot10	= 5;
							Slot11	= 4;
							Slot12	= 1;
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
							Slot1	= 20;
							Slot2	= 20;
							Slot3	= 10;
							Slot4	= 10;
							Slot5	= 10;
							Slot6	= 10;
							Slot7	= 5;
							Slot8	= 5;
							Slot9	= 4;
							Slot10	= 4;
							Slot11	= 1;
							Slot12	= 1;
							break;
						case Method.OLD_ROD:
						case Method.GOOD_ROD:
						case Method.SUPER_ROD:
						case Method.SUPER_ROD_SPOTS:
						case Method.SURF:
						case Method.SURF_SPOTS:
						case Method.HEADBUTT:
						default:
							Slot1	= 0;
							Slot2	= 0;
							Slot3	= 0;
							Slot4	= 0;
							Slot5	= 0;
							Slot6	= 0;
							Slot7	= 0;
							Slot8	= 0;
							Slot9	= 0;
							Slot10	= 0;
							Slot11	= 0;
							Slot12	= 0;
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
			switch ((Generation)Settings.pokemonGeneration)
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
							Rate  = 25;
							Slot1 = 60;
							Slot2 = 30;
							Slot3 = 5;
							Slot4 = 4;
							Slot5 = 1;
							break;
						case Method.GOOD_ROD:
							Rate  = 50;
							Slot1 = 60;
							Slot2 = 30;
							Slot3 = 5;
							Slot4 = 4;
							Slot5 = 1;
							break;
						case Method.SUPER_ROD_SPOTS:
						case Method.SUPER_ROD:
							Rate  = 75;
							Slot1 = 60;
							Slot2 = 30;
							Slot3 = 5;
							Slot4 = 4;
							Slot5 = 1;
							break;
						case Method.SURF_SPOTS:
						case Method.SURF:
							Rate  = 10;
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
							Rate  = 0;
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
		foreach(PokemonEncounter mapEncounter in EncounterDatabase)
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

		float time = System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);

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
		int encounterIndex = Settings.Rand.Next(0, chanceSplitList.Length);

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
			//Settings.Rand.Next(chanceSplitList[encounterIndex].minLevel, chanceSplitList[encounterIndex].maxLevel + 1),
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
		int randomPokemon = Settings.Rand.Next(0, 100);

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
        int encounterIndex = Settings.Rand.Next(0, chanceSplitList.Length);

#if DEBUG
        string debugtext = "";
            for(int i = 0; i < chanceSplitList.Length; i++){
                debugtext += PokemonDatabase.getPokemon(chanceSplitList[i].ID).getName() + ", ";}
            Debug.Log(encounterIndex+": "+debugtext + "("+PokemonDatabase.getPokemon(chanceSplitList[encounterIndex].ID).getName()+")");
#endif
        */
		return new Pokemon();
			// list[randomPokemon].ID, PokemonOld.Gender.CALCULATE,
			//Settings.Rand.Next(list[randomPokemon].minLevel, list[randomPokemon].maxLevel + 1),
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
#endregion

namespace PokemonUnity
{
	public struct Tile
	{
		#region Variables
		/// <summary>
		/// Y measurement of this Tile's Node in Scene; 
		/// starts at Bottom and stretches Up
		/// </summary>
		public int Length 
		{
			get
			{
				switch (Shape)
				{
					case Shape.WalkPath:
						return 2;
					default:
						return 1;
				}
			}
		}
		/// <summary>
		/// X measurement of this Tile's Node in Scene;
		/// Starts at Left and stretches Right
		/// </summary>
		public int Width
		{
			get
			{
				switch (Shape)
				{
					case Shape.WalkPath:
						return 2;
					default:
						return 1;
				}
			}
		}
		/// <summary>
		/// X position of this Tile's Node in Scene
		/// </summary>
		public int X { get; private set; }
		/// <summary>
		/// Y position of this Tile's Node in Scene
		/// </summary>
		public int Y { get; private set; }
		// <summary>
		// Z position of this Tile's Node in Scene
		// </summary>
		//public int Z { get; set; }
		/// <summary>
		/// 
		/// </summary>
		/// ToDo: Enum?
		public int Map { get; private set; } 
		public bool Encounter {
			get
			{
				switch (Terrain)
				{
					case Terrain.Grass:
					case Terrain.Sand:
					case Terrain.Rock:
					case Terrain.DeepWater:
					case Terrain.StillWater:
					case Terrain.Water:
					case Terrain.TallGrass:
					case Terrain.SootGrass:
						return true;
					case Terrain.Puddle:
					default:
						return false;
				}
			}
		}
		#endregion
		#region Enums
		public Terrain Terrain { get; set; }
		public Environment Environment { get; set; }
		//public Season Texture { get; set; }
		public Shape Shape { get; set; }
		/// <summary>
		/// Direction this Tile Node is facing;
		/// Rotation to use on this Tile's Node
		/// </summary>
		public Direction Direction { get; set; }
		#endregion

		//public Tile(int x, int y, int z) { }

		#region Explicit Operators
		/// <summary>
		/// Two Tile objects are equal if they each occupy by the same physical location.
		/// </summary>
		/// <param name="tile1">Tile Location 1</param>
		/// <param name="tile2">Tile Attempting to Occupy 2</param>
		/// <returns>Returns whether or not the two Tile entities are overlapping the same physical location</returns>
		public static bool operator == (Tile tile1, Tile tile2)
		{
			for (int x = 0; x < tile1.Width; x++)
			{
				if (tile1.X + x == tile2.X && (/*tile1.Y == tile2.Y &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
					//return true;
					for (int y = 0; y < tile1.Length; y++)
					{
						if (tile1.Y + y == tile2.Y && (/*tile1.X == tile2.X &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
							return true;
					}
			}
			return tile1.X == tile2.X && tile1.Y == tile2.Y /*&& tile1.Z == tile2.Z*/ && tile1.Map == tile2.Map;
		}
		public static bool operator != (Tile tile1, Tile tile2)
		{
			if (/*tile1.Z != tile2.Z ||*/ tile1.Map != tile2.Map)
				return true;
			for (int x = 0; x < tile1.Width; x++)
			{
				if (tile1.X + x == tile2.X && (/*tile1.Y == tile2.Y &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
					//return true;
					for (int y = 0; y < tile1.Length; y++)
					{
						if (tile1.Y + y == tile2.Y && (/*tile1.X == tile2.X &&* tile1.Z == tile2.Z &&*/ tile1.Map == tile2.Map))
							return false;
					}
			}
			return tile1.X != tile2.X && tile1.Y != tile2.Y /*&& tile1.Z != tile2.Z*/ && tile1.Map != tile2.Map;
		}
		//public bool Equals(Tile obj)
		//{
		//	return this == obj;
		//}
		public bool Equals(int x, int y)
		{
			//return this.X == x && this.Y == y;
			Tile obj = new Tile() { X = x, Y = y };
			return base.Equals(obj);
		}
		public override bool Equals(object obj)
		{
			if (!(obj is Tile)) return false;
			return this == (Tile)obj;
		}
		public override int GetHashCode()
		{
			//ToDo: Test HashCode? Also, MapId should also be included...
			//Math was copied from Unity's Vector3
			//return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Z.GetHashCode() >> 2;
			//return X.GetHashCode() ^ Y.GetHashCode() << 2 ^ Map.GetHashCode() >> 2;
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}
		}
		#endregion
	}

	public class SceneTiles : UnityEngine.MonoBehaviour
	{
		public Tile[] Tiles { get; private set; }
	}

	/// ToDo: Store maps as GameObjects in code
	/// Find a way to associate code-variables to Maps
	/// Generate code to load maps based on above
	/// Consider each overworld Route (1,21,26, PalletTown...)
	/// a separate GameObject versus a single map
	/// Decide whether to load all GameObjects together or
	/// relative to/based on a player's given position
	public class Map : UnityEngine.MonoBehaviour
	{
		/// <summary>
		/// Set this variable in the inspector
		/// </summary>
		public UnityEngine.GameObject mapPrefab;
		public string MapName;

		#region Unity UI Resources
		private void Awake()
		{
			//foreach (Vector3 objectPos in objectPositions)
			//{
			//	GameObject mapObject = Instantiate(mapPrefab) as GameObject;
			//	mapObject.transform.position = objectPos;
			//}
		}

		void OnEnable()
		{
			//if mapName not null, scan directory for file/filename that matches "MapName"
			//if file exist, load map and content on screen
				//create a layer for each terrain asset
				//for each terrain, generate mesh map (from file)
				//for each asset, instantiate obj prefab (based on file)
				//Create Components with values (if terrain add SceneTile, if tallGrass add PokemonEncounter) 
				//Add Component to Tiles/Squares
			//else do nothing
		}
		#endregion

		UnityEngine.GameObject mapTile;
		//ToDo: Fetch from GameVariables
		MapMatrix map; //{ get { return GameVariables.Map; } }
 
		/// <summary>
		/// Build map from Array of GameObjects[]
		/// </summary>
		/// ToDo: How to save map as Array of Objects?
		/// ToDo: Blender-to-Unity => Read object name in if-check
		/// A tile Array for Location, Rotation, and Name
		void BuildMap(UnityEngine.GameObject layer, Terrain terrain) 
		{
			int minX = -20;
			int maxX = map.Width;
			int minY = 0;
			int maxY = map.Length;
			int minZ = -20;
			int maxZ = map.mapHeader.MapHeight;

			List<Tile> tArray = new List<Tile>();
			UnityEngine.MeshFilter mf = mapTile.AddComponent<UnityEngine.MeshFilter>();
			UnityEngine.Mesh quad = mf.mesh; //new UnityEngine.Mesh();
			//UnityEngine.MeshRenderer mr = mapTile.AddComponent<UnityEngine.MeshRenderer>();

			//Foreach Tile in Map that matches Terrain...

			// loop for every z position in the grid
			for (int z = minZ; z < maxZ; z++ )
			{
				// now loop for every x position in the grid
				for (int x = minX; x < maxX; x++ )
				{
					//loop for every y position in the grid
					for (int y = minY; y < maxY; y++ )
					{
						Tile t = map.mapHeader.MapArray[z][x, y];

						if(!tArray.Exists(i => i.Equals(t))) tArray.Add(t);

						//switch (t.Terrain)
						//{
						//	case Terrain.Grass:
						//		//floor.Add(TileToQuad(t));
						//		TileToQuad(ref quad, t);
						//		break;
						//	case Terrain.Sand:
						//	case Terrain.Rock:
						//	case Terrain.DeepWater:
						//	case Terrain.StillWater:
						//	case Terrain.Water:
						//	case Terrain.TallGrass:
						//	case Terrain.SootGrass:
						//	case Terrain.Puddle:
						//	default:
						//		break;
						//}
						switch (t.Shape)
						{
							case Shape.Flat:
								//floor.Add(TileToQuad(t));
								TileToQuad(ref quad, t);
								break;
							case Shape.CliffSide:
							case Shape.CliffCorner:
							case Shape.LedgeJump:
							case Shape.LedgeWater:
							case Shape.WalkPath:
							default:
								break;
						}
					}
				}
				TileToQuad(ref quad, tArray.ToArray(), z);
			}

			mf.mesh = quad;
			quad.RecalculateBounds();
			//quad.Optimize();
		}
 
		/// <summary>
		/// Add assets to map from Array of GameObjects[]
		/// </summary>
		/// ToDo: Save assets on map as Array of Objects?
		/// A tile Array for Location, Rotation, and Name
		void SpawnAssets() 
		{
			int minX = -20;
			int maxX = map.Width;
			int minY = 0;
			int maxY = map.Length;
			int minZ = -20;
			int maxZ = map.mapHeader.MapHeight;

			List<UnityEngine.Mesh> floor = new List<UnityEngine.Mesh>();
			UnityEngine.Mesh floorQuad = new UnityEngine.Mesh();

			UnityEngine.MeshFilter mf = mapTile.AddComponent<UnityEngine.MeshFilter>();
			UnityEngine.MeshRenderer mr = mapTile.AddComponent<UnityEngine.MeshRenderer>();

			// loop for every z position in the grid
			for (int z = minZ; z < maxZ; z++ )
			{
				// now loop for every x position in the grid
				for (int x = minX; x < maxX; x++ )
				{
					//loop for every y position in the grid
					for (int y = minY; y < maxY; y++ )
					{
						Tile t = map.mapHeader.MapArray[z][x, y];

						switch (t.Terrain)
						{
							case Terrain.Grass:
								//floor.Add(TileToQuad(t));
								TileToQuad(ref floorQuad, t);
								break;
							case Terrain.Sand:
							case Terrain.Rock:
							case Terrain.DeepWater:
							case Terrain.StillWater:
							case Terrain.Water:
							case Terrain.TallGrass:
							case Terrain.SootGrass:
							case Terrain.Puddle:
							default:
								// put it all to together to assign a position
								UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);

								// instantiate the cube into a variable, so you can do other things with it
								UnityEngine.GameObject clone = (UnityEngine.GameObject)Instantiate(mapTile, pos, new UnityEngine.Quaternion());

								//ToDO: Add 'if(tile)' => collision-map

								// change the name of the object, include the x and z position in the name
								clone.name = "Tile_" + x.ToString() + "_" + z.ToString();

								// in future, 
								// this would be stored in a 3D array for future reference
								// so you could modify the position, material, anything!
								// cubeArray[x,y,z] = clone;
								break;
						}

					}
				}
			}

			mf.mesh = floorQuad;
			floorQuad.RecalculateBounds();
		}

		#region Methods
		public UnityEngine.Mesh TileToQuad(ref UnityEngine.Mesh mesh, Tile[] tiles, int Z = 0)
		{
			//UnityEngine.Mesh mesh = new UnityEngine.Mesh();
			List<UnityEngine.Vector3> vertices = new List<UnityEngine.Vector3>();
			List<int> tri = new List<int>();
			List<UnityEngine.Vector3> normals = new List<UnityEngine.Vector3>();
			List<UnityEngine.Vector2> uv = new List<UnityEngine.Vector2>();
			//int xLo, xHi, yLo, yHi;
			
			foreach (Tile tile in tiles)
			{
				switch (tile.Shape)
				{
					case Shape.Flat:
						vertices.AddRange(new UnityEngine.Vector3[] 
						{
							new UnityEngine.Vector3 (tile.X,tile.Y,Z),
							new UnityEngine.Vector3 (tile.Width, tile.Y, Z),
							new UnityEngine.Vector3 (tile.X,tile.Length,Z),
							new UnityEngine.Vector3 (tile.Width, tile.Length, Z)
						});
						//tri.AddRange(new int[] 
						//{
						//	//Lower Left Triangle
						//	//0, 2, 1,
						//	tri.Count, tri.Count + 2, tri.Count + 1,
						//	//Upper Right Triangle
						//	//2, 3, 1
						//	tri.Count + 2, tri.Count + 3, tri.Count + 1
						//});
						tri.AddRange(new int[] 
						{
							//2, 1, 0,
							tri.Count + 2, tri.Count + 1, tri.Count,
							//2, 3, 1
							tri.Count + 2, tri.Count + 3, tri.Count + 1
						});
						normals.AddRange(new UnityEngine.Vector3[] 
						{
							//Floor Tiles Should point UP
							-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
							-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
							-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
							-UnityEngine.Vector3.up  //-UnityEngine.Vector3.forward
						});
						uv.AddRange(new UnityEngine.Vector2[]
						{
							new UnityEngine.Vector2 ((float)(tile.X) + 0f,		(float)(tile.Y) + 0f),//(0f, 0f)
							new UnityEngine.Vector2 ((float)(tile.Width) + 1f,	(float)(tile.Y) + 0f),//(1f, 0f)
							new UnityEngine.Vector2 ((float)(tile.X) + 0f,		(float)(tile.Length) + 1f),//(0f, 1f)
							new UnityEngine.Vector2 ((float)(tile.Width) + 1f,	(float)(tile.Length) + 1f) //(1f, 1f)
							//new Vector2 ((float)(vertices.Count - 4f) + 0f, (float)(vertices.Count - 4f) + 0f),//(0f, 0f)
							//new Vector2 ((float)(vertices.Count - 4f) + 1f, (float)(vertices.Count - 4f) + 0f),//(1f, 0f)
							//new Vector2 ((float)(vertices.Count - 4f) + 0f, (float)(vertices.Count - 4f) + 1f),//(0f, 1f)
							//new Vector2 ((float)(vertices.Count - 4f) + 1f, (float)(vertices.Count - 4f) + 1f) //(1f, 1f)
						});
						//xLo = Math.Min(0, tile.X);
						//yLo = Math.Min(0, tile.Y);
						//xHi = Math.Max(1, tile.X + tile.Width);
						//yHi = Math.Max(1, tile.Y + tile.Length);
						//uv = new UnityEngine.Vector2[vertices.Count];
						//for (int i = 0; i < vertices.Count; i++)
						//{
						//	//Each UV should be a fraction of the total dimension 
						//	uv[i] = new UnityEngine.Vector2((float)i / (xHi - xLo), (float)i / (yHi - yLo));
						//}
						break;
					case Shape.CliffSide:
						break;
					case Shape.CliffCorner:
						break;
					case Shape.LedgeJump:
						break;
					case Shape.LedgeWater:
						break;
					case Shape.WalkPath:
						break;
					case Shape.NULL:
						break;
					default:
						break;
				}
			}
			mesh.vertices = vertices.ToArray();
			mesh.triangles = tri.ToArray();
			mesh.normals = normals.ToArray();
			//mesh.uv = uv.ToArray();
			return mesh;
		}
		public UnityEngine.Mesh TileToQuad(ref UnityEngine.Mesh mesh, Tile tile, int Z = 0)
		{
			//UnityEngine.Mesh mesh = new UnityEngine.Mesh();
			UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[0];
			int[] tri = new int[0];
			UnityEngine.Vector3[] normals = new UnityEngine.Vector3[0];
			UnityEngine.Vector2[] uvs = new UnityEngine.Vector2[0];
			switch (tile.Shape)
			{
				case Shape.Flat:
					vertices = new UnityEngine.Vector3[] 
					{
						new UnityEngine.Vector3 (tile.X,tile.Y,Z),
						new UnityEngine.Vector3 (tile.Width, tile.Y, Z),
						new UnityEngine.Vector3 (tile.X,tile.Length,Z),
						new UnityEngine.Vector3 (tile.Width, tile.Length, Z)
					};
					tri = new int[6];
					normals = new UnityEngine.Vector3[] 
					{
						//Floor Tiles Should point UP
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up  //-UnityEngine.Vector3.forward
					};
					uvs = new UnityEngine.Vector2[]
					{
						new UnityEngine.Vector2 (0, 0),
						new UnityEngine.Vector2 (1, 0),
						new UnityEngine.Vector2 (0, 1),
						new UnityEngine.Vector2 (1, 1)
					};
					break;
				case Shape.CliffSide:
					break;
				case Shape.CliffCorner:
					break;
				case Shape.LedgeJump:
					break;
				case Shape.LedgeWater:
					break;
				case Shape.WalkPath:
					break;
				case Shape.NULL:
					break;
				default:
					break;
			}
			mesh.vertices = vertices;
			mesh.triangles = tri;
			mesh.normals = normals;
			mesh.uv = uvs;
			return mesh;
		}
		public UnityEngine.Mesh TileToQuad(Tile tile, int Z = 0)
		{
			UnityEngine.Mesh mesh = new UnityEngine.Mesh();
			UnityEngine.Vector3[] vertices = new UnityEngine.Vector3[0];
			int[] tri = new int[0];
			UnityEngine.Vector3[] normals = new UnityEngine.Vector3[0];
			switch (tile.Shape)
			{
				case Shape.Flat:
					vertices = new UnityEngine.Vector3[] 
					{
						new UnityEngine.Vector3 (tile.X,tile.Y,Z),
						new UnityEngine.Vector3 (tile.Width, tile.Y, Z),
						new UnityEngine.Vector3 (tile.X,tile.Length,Z),
						new UnityEngine.Vector3 (tile.Width, tile.Length, Z)
					};
					tri = new int[] //new int[6]; 
					{
						2, 1, 0,
						//tri.Count + 2, tri.Count + 1, tri.Count,
						2, 3, 1
						//tri.Count + 2, tri.Count + 3, tri.Count + 1
					};
					normals = new UnityEngine.Vector3[] 
					{
						//Floor Tiles Should point UP
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up, //-UnityEngine.Vector3.forward,
						-UnityEngine.Vector3.up  //-UnityEngine.Vector3.forward
					};
					break;
				case Shape.CliffSide:
					break;
				case Shape.CliffCorner:
					break;
				case Shape.LedgeJump:
					break;
				case Shape.LedgeWater:
					break;
				case Shape.WalkPath:
					break;
				case Shape.NULL:
					break;
				default:
					break;
			}
			mesh.vertices = vertices;
			mesh.triangles = tri;
			mesh.normals = normals;
			mesh.uv = new UnityEngine.Vector2[]
			{
				new UnityEngine.Vector2 (0, 0),
				new UnityEngine.Vector2 (1, 0),
				new UnityEngine.Vector2 (0, 1),
				new UnityEngine.Vector2 (1, 1)
			};
			return mesh;
		}
		#endregion
	}

	#region Idea4: 3d map chunk from 2d array
	/// <summary>
	/// Overworld excel grid of map headers 
	/// </summary>
	public class MapMatrix
	{
		/// <summary>
		/// 
		/// </summary>
		/// enum of map matrix (i.e. Custom, PokemonDiamond, PokemonEmerald, etc...)
		/// if MAP then it allows you to load mapHeaders
		/// else it's a small chunk or dungeon
		Worlds WorldId;
		/// <summary>
		/// 
		/// </summary>
		/// enum label of matrix
		/// matrix id 0 is the overworld map
		Regions MatrxId;
		public int Length { get; private set; }
		public int Width { get; private set; }
		public MapHeader mapHeader { get; private set; }
	}
	/// <summary>
	/// Header data contains map x,y size
	/// </summary>
	public class MapHeader
	{
		/// <summary>
		/// 
		/// </summary>
		/// Internal Name
		public Maps MapId;
		public string Name;
		/// <summary>
		/// Texture around Name when entering Map
		/// </summary>
		/// ToDo: Volcanic, Snow, Spring, etc...
		public int NameStyle;
		public int MapType;
		public int MapHeight;
		//int Texture1;
		//int Texture2;
		public int Scripts;
		public int MapScripts;
		public int MusicDay;
		public int MusicNight;
		public int Texts;
		/// <summary>
		/// Table or Encounter chart for pokemons expected to find on map
		/// </summary>
		public int WildPokemon;
		public int Events;
		public int Flags;
		public Weather Weather;
		public int Camera;
		/// <summary>
		/// </summary>
		/// For height loop or For width loop
		/// [Z,i] = gameobject int value
		/// each value in for loop should be rounded to nearest whole number
		/// need to map collision to X,Y value as well...
		public Tile[][,] MapArray;
	}
	#endregion
	#region Enums
	public enum Worlds
	{
		GLOBAL = 0,
		Emerald 
	}
	/// <summary>
	/// Each "world" has their own smaller regions
	/// </summary>
	public enum Regions
	{
		Overworld = 0
	}
	/// <summary>
	/// Each "region" has their own individual maps
	/// </summary>
	public enum Maps
	{
		Safari = 0
	}
	public enum Direction
	{
		/// <summary>
		/// Facing Foward, towards camera
		/// </summary>
		Down = 0,
		Up,
		Left,
		Right
	}

	public enum Shape
	{
		/// <summary>
		/// Stairs use flats too
		/// </summary>
		Flat,
		CliffSide,
		CliffCorner,
		LedgeJump,
		LedgeWater,
		WalkPath,
		NULL
	}

	public enum Season
	{
		Summer,
		Winter,
		Fall,
		Spring,
		Volcanic
	}
	#endregion
}