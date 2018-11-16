using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using PokemonUnity;

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
    /// a 2D grid of cubes
    /// every position in the grid must have a cube
    /// the height is different for each position in the grid
    /// The way would be to loop through every x position, and within that every z position.

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

/// <summary>
/// Map GameObject will be an Array of tiles
/// mapEnum = MapId = BlenderJsonToUnity[]
/// </summary>
class BlenderJsonToUnity
{
	/// <summary>
	/// Vector3 || (float x, float y, float z)
	/// </summary>
	/// we dont need ray casting anymore, since all the x,y,z's are recorded
    float tileLocation; 
	/// <summary>
	/// Quaternion || (float x, float y, float z)
	/// </summary>
    float tileRotation;
	/// <summary>
	/// Mesh object
	/// </summary>
    string tileShape; 
	/// <summary>
	/// Texture enum or filename
	/// </summary>
    string tileTexture; 
	/// <summary>
    /// Tile Tags:
    /// 0 - Default Environment
    /// 1 - Impassable
    /// 2 - Surf Water
    /// 3 - Environment 2
    /// 4? - Dive Water
	/// </summary>
	/// ToDo: enum here... Mesh object will determine collision mapping
    int tileCollision; 

    /*
     * Read all of the json files in the blender/json/ folder
     * Each file should represent 1 map file, filled with many tilesArray
     * convert json file into an array of tiles, to be used with Map.BuildMap()
     * 
     * Consider writing unity script to convert json map tiles to a saved prefab asset
     * to view texture configuration for better hard-code adjustments
     * https://forum.unity.com/threads/saving-a-custom-map-object-hierarchy-and-keeping-prefabs-looking-for-ideas.156963/
     * 
     * What if textures were stored as a single map sprite
     * broken down into a grid and used to load the textures 
     * on the tiles x,y by sprite's x,y? Only issue is Z value
     * build from buttom up x,y,(z|0 to z|n+), if empty: skip, 
     * use sprite x,y texture for each z?
     * https://gamedev.stackexchange.com/questions/87696/how-do-i-draw-a-tilemap-within-unity3d
     * https://answers.unity.com/questions/974007/what-is-the-best-way-to-create-3d-tile-based-level.html
    private static Dictionary<int, PokemonData> LoadPokedex()
    {
        var data = new Dictionary<int, PokemonData>(); //Why not convert dictionary to Array? It's faster, more streamlined, and simpler to work with

        string[] fileEntries = Directory.GetFiles(Application.streamingAssetsPath + "/Pokemons", "*.json");  // Filter on only json files, otherwise you can also get other files (.meta)
        foreach (string fileName in fileEntries)
        {
            string dataAsJson = File.ReadAllText(fileName, Encoding.UTF8);
            PokemonData pokemonData = new PokemonData();
            JsonUtility.FromJsonOverwrite(dataAsJson, pokemonData);
            data.Add(pokemonData.ID, pokemonData);
        }

        return data; //Right here, a ".ToArray()" or maybe a for-loop Array[n] = Dictionary<n>
    }*/
}

/// <summary>
/// Idea 1 for Pokemon Encounters
/// </summary>
class WildPokemon
{
	class MapPokemon
	{
		PokemonUnity.Pokemon.Pokemons Pokemon { get; set; }
		int minLevel { get; set; }
		int maxLevel { get; set; }
	}
	class TravelGround
	{
		/// <summary>
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate { get; set; }
		#region Walking
		/// <summary>
		/// 20%
		/// </summary>
		int Slot1 { get; set; }
		/// <summary>
		/// 20%
		/// </summary>
		int Slot2 { get; set; }
		/// <summary>
		/// 10%
		/// </summary>
		int Slot3 { get; set; }
		/// <summary>
		/// 10%
		/// </summary>
		int Slot4 { get; set; }
		/// <summary>
		/// 10%
		/// </summary>
		int Slot5 { get; set; }
		/// <summary>
		/// 10%
		/// </summary>
		int Slot6 { get; set; }
		/// <summary>
		/// 5%
		/// </summary>
		int Slot7 { get; set; }
		/// <summary>
		/// 5%
		/// </summary>
		int Slot8 { get; set; }
		/// <summary>
		/// 4%
		/// </summary>
		int Slot9 { get; set; }
		/// <summary>
		/// 4%
		/// </summary>
		int Slot10 { get; set; }
		/// <summary>
		/// 1%
		/// </summary>
		int Slot11 { get; set; }
		/// <summary>
		/// 1%
		/// </summary>
		int Slot12 { get; set; }
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
	}
	class TravelWater
	{
		#region Surfing
		/// <summary>
		/// 10
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate { get; set; }
		/// <summary>
		/// 60%
		/// </summary>
		MapPokemon Slot1 { get; set; }
		/// <summary>
		/// 30%
		/// </summary>
		MapPokemon Slot2 { get; set; }
		/// <summary>
		/// 5%
		/// </summary>
		MapPokemon Slot3 { get; set; }
		/// <summary>
		/// 4%
		/// </summary>
		MapPokemon Slot4 { get; set; }
		/// <summary>
		/// 1%
		/// </summary>
		MapPokemon Slot5 { get; set; }
		#endregion
		#region Old Rod
		/// <summary>
		/// 25
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate2 { get; set; }
		#endregion
		#region Good Rod
		/// <summary>
		/// 50
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate3 { get; set; }
		#endregion
		#region Super Rod
		/// <summary>
		/// 75
		/// How often or likely to engage and encounter a pokemon
		/// </summary>
		int Rate4 { get; set; }
		#endregion
	}
	class TravelAir
	{

	}
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
		return new Pokemon(PokemonUnity.Pokemon.Pokemons.NONE);
	}

	public class EncounterSlot
	{
		PokemonUnity.Pokemon.Pokemons Pokemon { get; set; }
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

namespace PokemonUnity
{
	/*enum EncounterActions
	{
		Land,
		/// <summary>
		/// If null or empty, defaults to Land
		/// </summary>
		LandMorning,
		LandDay,
		LandNight,
		//ToDo: Missing Grass, and Tall grass...
		Cave,
		BugContest,
		Water,
		RockSmash,
		OldRod,
		GoodRod,
		SuperRod,
		HeadbuttLow,
		HeadbuttHigh
	}*/

	/// <summary>
	/// Encounter method
	/// </summary>
	public enum Method
	{
		/// <summary>
		/// Walking in tall grass or a cave
		/// </summary>
		WALK = 1,
		/// <summary>
		/// Walking in rustling grass
		/// </summary>
		GRASS_SPOTS = 9,
		/// <summary>
		/// Walking in dust clouds
		/// </summary>
		CAVE_SPOTS = 10,
		/// <summary>
		/// Walking in bridge shadows
		/// </summary>
		BRIDGE_SPOTS = 11,
		/// <summary>
		/// Walking in dark grass
		/// </summary>
		DARK_GRASS = 8,
		/// <summary>
		/// Walking in yellow flowers
		/// </summary>
		YELLOW_FLOWERS = 14,
		/// <summary>
		/// Walking in purple flowers
		/// </summary>
		PURPLE_FLOWERS = 15,
		/// <summary>
		/// Walking in red flowers
		/// </summary>
		RED_FLOWERS = 16,
		/// <summary>
		/// Walking on rough terrain
		/// </summary>
		ROUGH_TERRAIN = 17,
		/// <summary>
		/// Fishing with an <see cref="eItems.Item.OLD_ROD"/>
		/// </summary>
		OLD_ROD = 2,
		/// <summary>
		/// Fishing with a <see cref="eItems.Item.GOOD_ROD"/> 
		/// </summary>
		GOOD_ROD = 3,
		/// <summary>
		/// Fishing with a <see cref="eItems.Item.SUPER_ROD"/> 
		/// </summary>
		SUPER_ROD = 4,
		/// <summary>
		/// Fishing in dark spots
		/// </summary>
		SUPER_ROD_SPOTS = 12,
		/// <summary>
		/// Surfing
		/// </summary>
		SURF = 5,
		/// <summary>
		/// Surfing in dark spots
		/// </summary>
		SURF_SPOTS = 13,
		/// <summary>
		/// Smashing rocks
		/// </summary>
		ROCK_SMASH = 6,
		/// <summary>
		/// Headbutting trees
		/// </summary>
		HEADBUTT = 7
	}

	/// <summary>
	/// 
	/// </summary>
	public enum Condition
	{
		SWARM = 1,
		TIME = 2,
		RADAR = 3,
		SLOT = 4,
		RADIO = 5,
		SEASON = 6
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// default: swarm-no, time-day, radar-off, slot-none, radio-off
	/// </remarks>
	/// ToDo: Change from Enum to class with Bool values
	public enum ConditionValue
	{
		/// <summary>
		/// During a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		SWARM_YES = 1,
		/// <summary>
		/// Not during a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		SWARM_NO = 2,

		/// <summary>
		/// In the morning
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_MORNING = 3,
		/// <summary>
		/// During the day
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_DAY = 4,
		/// <summary>
		/// At night
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_NIGHT = 5,

		/// <summary>
		/// Using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		RADAR_ON = 6,
		/// <summary>
		/// Not using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		RADAR_OFF = 7,

		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_NONE = 8,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_RUBY = 9,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_SAPPHIRE = 10,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_EMERALD = 11,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_FIRERED = 12,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_LEAFGREEN = 13,

		/// <summary>
		/// Radio off
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_OFF = 14,
		/// <summary>
		/// Hoenn radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_HOENN = 15,
		/// <summary>
		/// Sinnoh radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_SINNOH = 16,

		/// <summary>
		/// During Spring
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_SPRING = 17,
		/// <summary>
		/// During Summer
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_SUMMER = 18,
		/// <summary>
		/// During Autumn
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_AUTUMN = 19,
		/// <summary>
		/// During Winter
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_WINTER = 20
	}
}