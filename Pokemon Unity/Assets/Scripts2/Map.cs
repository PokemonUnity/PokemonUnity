using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                    GameObject clone = Instantiate(mapTile, pos, Quaternion.identity);

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
}

/// <summary>
/// Map GameObject will be an Array of tiles
/// mapEnum = MapId = BlenderJsonToUnity[]
/// </summary>
class BlenderJsonToUnity
{
    Vector3 tileLocation; //we dont need ray casting anymore, since all the x,y,z's are recorded
    Quaternion tileRotation;
    string tileShape; // Mesh object
    string tileTexture; // Texture enum or filename
    /// Tile Tags:
    /// 0 - Default Environment
    /// 1 - Impassable
    /// 2 - Surf Water
    /// 3 - Environment 2
    /// 4? - Dive Water
    int tileCollision; //enum here... Mesh object will determine collision mapping

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