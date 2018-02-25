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