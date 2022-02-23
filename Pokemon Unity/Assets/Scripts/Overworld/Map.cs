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
	[UnityEngine.RequireComponent(typeof(UnityEngine.MeshFilter),typeof(UnityEngine.MeshRenderer))]
	public class Map : UnityEngine.MonoBehaviour
	{
		/// <summary>
		/// Set this variable in the inspector
		/// </summary>
		//public UnityEngine.GameObject mapPrefab;
		public string MapName;
		private UnityEngine.Mesh mesh { get; set; }
		public List<UnityEngine.Vector3> vertices { get; private set; }
		public List<int> tris { get; private set; }
		public List<UnityEngine.Vector3> normals { get; private set; }
		public List<UnityEngine.Vector2> uvs { get; private set; }

		#region Unity UI Resources
		private void Awake()
		{
			//If mesh is not null, use value already stored
			if (!mesh)
			{
				mesh = new UnityEngine.Mesh();
				UnityEngine.MeshFilter f = (UnityEngine.MeshFilter)GetComponent<UnityEngine.MeshFilter>();
				f.mesh = mesh;
				mesh.name = string.Format("{0}Mesh", gameObject.name);
			}
			else 
				mesh = GetComponent<UnityEngine.MeshFilter>().mesh;
			//foreach (Vector3 objectPos in objectPositions)
			//{
			//	GameObject mapObject = Instantiate(mapPrefab) as GameObject;
			//	mapObject.transform.position = objectPos;
			//}
		}

		void Start()
		{
			//CreateMesh();
			BuildMap();
		}

		void OnEnable()
		{
			UpdateMesh();
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

		//UnityEngine.GameObject mapTile;
		//ToDo: Fetch from Game
		MapMatrix map; //{ get { return Game.Map; } }
 
		/// <summary>
		/// Build map from Array of GameObjects[]
		/// </summary>
		/// ToDo: How to save map as Array of Objects?
		/// ToDo: Blender-to-Unity => Read object name in if-check
		/// A tile Array for Location, Rotation, and Name
		void BuildMap(/*UnityEngine.GameObject layer, Terrain terrain*/) 
		{
			//int minX = -20;
			int maxX = map.Width;
			//int minY = 0;
			int maxY = map.Length;
			//int minZ = -20;
			int maxZ = map.mapHeader.MapHeight;

			List<Tile> tArray = new List<Tile>();
			//UnityEngine.MeshFilter mf = mapTile.AddComponent<UnityEngine.MeshFilter>();
			UnityEngine.MeshFilter mf = transform.GetComponent<UnityEngine.MeshFilter>();
			UnityEngine.Mesh quad = mf.mesh; //new UnityEngine.Mesh();
			//UnityEngine.MeshRenderer mr = mapTile.AddComponent<UnityEngine.MeshRenderer>();

			//Foreach Tile in Map that matches Terrain...

			// loop for every z position in the grid
			//for (int z = 0; z < maxZ; z++ )
			//{
				// now loop for every x position in the grid
				for (int x = 0; x < maxX; x++ )
				{
					//loop for every y position in the grid
					for (int y = 0; y < maxY; y++ )
					{
						//Tile t = map.mapHeader.MapArray[z][x, y];
						Tile t = map.mapHeader.Tiles[x, y];

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
								//TileToQuad(ref quad, t);
								//TileToQuad(t);
								break;
							case Shape.CliffSide:
							//case Shape.CliffCorner:
							case Shape.LedgeJump:
							case Shape.LedgeWater:
							//case Shape.WalkPath:
							default:
								break;
						}
					}
				}
			//	//TileToQuad(ref quad, tArray.ToArray(), z);
			//	TileToQuad(tArray.ToArray(), z);
			//}

			mf.mesh = quad;
			//quad.RecalculateBounds();
			mf.mesh.RecalculateBounds();
			//quad.Optimize();
		}
 
		/// <summary>
		/// Add assets to map from Array of GameObjects[]
		/// </summary>
		/// ToDo: Redo method to instantiate non-floor tiles
		/// ToDo: Save assets on map as Array of Objects?
		/// A tile Array for Location, Rotation, and Name
		void SpawnAssets() 
		{
			//int minX = -20;
			//int maxX = map.Width;
			//int minY = 0;
			//int maxY = map.Length;
			//int minZ = -20;
			//int maxZ = map.mapHeader.MapHeight;
			//
			//List<UnityEngine.Mesh> floor = new List<UnityEngine.Mesh>();
			//UnityEngine.Mesh floorQuad = new UnityEngine.Mesh();
			//
			//UnityEngine.MeshFilter mf = mapTile.AddComponent<UnityEngine.MeshFilter>();
			//UnityEngine.MeshRenderer mr = mapTile.AddComponent<UnityEngine.MeshRenderer>();
			//
			//// loop for every z position in the grid
			//for (int z = minZ; z < maxZ; z++ )
			//{
			//	// now loop for every x position in the grid
			//	for (int x = minX; x < maxX; x++ )
			//	{
			//		//loop for every y position in the grid
			//		for (int y = minY; y < maxY; y++ )
			//		{
			//			Tile t = map.mapHeader.MapArray[z][x, y];
			//
			//			switch (t.Terrain)
			//			{
			//				case Terrain.Grass:
			//					//floor.Add(TileToQuad(t));
			//					//TileToQuad(ref floorQuad, t);
			//					//TileToQuad(t);
			//					break;
			//				case Terrain.Sand:
			//				case Terrain.Rock:
			//				case Terrain.DeepWater:
			//				case Terrain.StillWater:
			//				case Terrain.Water:
			//				case Terrain.TallGrass:
			//				case Terrain.SootGrass:
			//				case Terrain.Puddle:
			//				default:
			//					// put it all to together to assign a position
			//					UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, y, z);
			//
			//					// instantiate the cube into a variable, so you can do other things with it
			//					UnityEngine.GameObject clone = (UnityEngine.GameObject)Instantiate(mapTile, pos, new UnityEngine.Quaternion());
			//
			//					//ToDO: Add 'if(tile)' => collision-map
			//
			//					// change the name of the object, include the x and z position in the name
			//					clone.name = "Tile_" + x.ToString() + "_" + z.ToString();
			//
			//					// in future, 
			//					// this would be stored in a 3D array for future reference
			//					// so you could modify the position, material, anything!
			//					// cubeArray[x,y,z] = clone;
			//					break;
			//			}
			//
			//		}
			//	}
			//}
			//
			//mf.mesh = floorQuad;
			//floorQuad.RecalculateBounds();
		}

		#region Methods
		void UpdateMesh()
		{
			//int index = 0;
			//for (int z = 0; z < length; z++)
			//{
			//	for (int x = 0; x < length; x++)
			//	{
			//		vertices[index] = Tile[x, z]; //mapGrid
			//		index++;
			//	}
			//}

			//mesh.Clear();
			mesh.vertices = vertices.ToArray();
			mesh.triangles = tris.ToArray();
			mesh.uv = uvs.ToArray();

			mesh.RecalculateBounds();
			mesh.RecalculateNormals();
		}
		/// <summary>
		/// Reads the MeshFilter component of Mesh 
		/// to combine into single game object.
		/// </summary>
		/// <param name="go"></param>
		void Combine(UnityEngine.GameObject go)
		{
			#region Mesh UVs Part 1
			#endregion
			//Grab all the mesh filters (from parent + children) on Map layer
			UnityEngine.MeshFilter[] meshFilters = GetComponentsInChildren<UnityEngine.MeshFilter>();
			//An array to hold all of the pending mesh data
			UnityEngine.CombineInstance[] combine = new UnityEngine.CombineInstance[meshFilters.Length];
			//Remove collider (will add again at end)
			Destroy(this.gameObject.GetComponent<UnityEngine.MeshCollider>());

			#region Mesh UVs Part 1
			//Get current UVs before combining the multiple mesh data
			UnityEngine.Vector2[] oldUVs = transform.GetComponent<UnityEngine.MeshFilter>().mesh.uv;
			#endregion

			#region Mesh Vertices
			int i = 0;
			while(i < meshFilters.Length)
			{
				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				//Disables objects added, so that they can be merged/removed
				meshFilters[i].gameObject.SetActive(false);
				i++;
			}
			transform.GetComponent<UnityEngine.MeshFilter>().mesh = new UnityEngine.Mesh();
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.CombineMeshes(combine, true);
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.RecalculateBounds();
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.RecalculateNormals();
			//transform.GetComponent<UnityEngine.MeshFilter>().mesh.Optimize();
			//UnityEditor.MeshUtility.Optimize(transform.GetComponent<UnityEngine.MeshFilter>().mesh);
			#endregion

			#region Mesh UVs Part 2
			//Reset UV array to hold both the old and new UVs
			UnityEngine.Vector2[] newUVs = new UnityEngine.Vector2[oldUVs.Length]; //Old.count + New.count = length
			//Add the new UVs to the old UVs (the newer textures are added towards the end...)
			for (i = 0; i < oldUVs.Length; i++)
				newUVs[i] = oldUVs[i];

			//Add new UVs to array
			//SetUVs suv = go.GetComponent<UnityEngine.set>
			//go.GetComponent<UnityEngine.MeshFilter>().mesh.GetUVs()

			//Override with the new combined UVs 
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.uv = newUVs;
			#endregion

			this.gameObject.AddComponent<UnityEngine.MeshCollider>();
			transform.gameObject.SetActive(true);

			//remove object since it's been added already
			Destroy(go);
		}
		/// <summary>
		/// Use this for Mesh objects that share the same texture/material atlas.
		/// When meshes are merged into one, they use the same materials as parent.
		/// </summary>
		/// <param name="go"></param>
		public void AddGameObjectToMapMesh(UnityEngine.GameObject go)
		{
			//Makes GameObject a child of Map 
			go.transform.parent = this.transform;
			//Merges the two objects into one unit
			Combine(go);
		}
		public void TileToQuad(Tile[] tiles, int Z = 0)
		{
			//UnityEngine.MeshFilter mf = new UnityEngine.MeshFilter();
			//UnityEngine.Mesh m = new UnityEngine.Mesh();
			//All vertices are on mid point, because when player position is rounded to 0, it would be in middle of quad
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
							new UnityEngine.Vector3 ((float) Math.Round(tile.X		- .5f, MidpointRounding.AwayFromZero),	tile.Y		- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.Width	- .5f, MidpointRounding.AwayFromZero),	tile.Y		- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.X		- .5f, MidpointRounding.AwayFromZero),	tile.Length	- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.Width	- .5f, MidpointRounding.AwayFromZero),	tile.Length - .5f,	Z)
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
					//case Shape.CliffCorner:
					//	break;
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
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.vertices = vertices.ToArray();
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.triangles = tri.ToArray();
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.normals = normals.ToArray();
			transform.GetComponent<UnityEngine.MeshFilter>().mesh.uv = uv.ToArray();
			//return mesh;
		}
		#region Old Idea...
		public UnityEngine.Mesh TileToQuad(ref UnityEngine.Mesh mesh, Tile[] tiles, int Z = 0)
		{
			//UnityEngine.Mesh mesh = new UnityEngine.Mesh();
			//All vertices are on mid point, because when player position is rounded to 0, it would be in middle of quad
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
							new UnityEngine.Vector3 ((float) Math.Round(tile.X		- .5f, MidpointRounding.AwayFromZero),	tile.Y		- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.Width	- .5f, MidpointRounding.AwayFromZero),	tile.Y		- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.X		- .5f, MidpointRounding.AwayFromZero),	tile.Length	- .5f,	Z),
							new UnityEngine.Vector3 ((float) Math.Round(tile.Width	- .5f, MidpointRounding.AwayFromZero),	tile.Length - .5f,	Z)
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
					//case Shape.CliffCorner:
					//	break;
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
			mesh.uv = uv.ToArray();
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
				//case Shape.CliffCorner:
				//	break;
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
				//case Shape.CliffCorner:
				//	break;
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
		#endregion
	}

	#region Idea4: 3d map chunk from 2d array
	/// <summary>
	/// Overworld excel grid of map headers 
	/// </summary>
	public class MapMatrix
	{
		/// <summary>
		/// The first matrix (Matrix 0), resembles the present routes of Pokémon Version.
		/// </summary>
		/// enum of map matrix (i.e. Custom, PokemonDiamond, PokemonEmerald, etc...)
		/// if MAP then it allows you to load mapHeaders
		/// else it's a small chunk or dungeon
		public Worlds WorldId { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// enum label of matrix
		/// matrix id 0 is the overworld map
		public Regions MatrixId { get; private set; }
		public int Length { get; private set; }
		public int Width { get; private set; }
		public MapHeader mapHeader { get; private set; }
	}
	/// <summary>
	/// Header data contains map x,y size
	/// </summary>
	public class MapHeader
	{
		#region Level
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
		public int NameStyle { get; private set; }
		public int MapType;
		/// <summary>
		/// Lists the maps of the "edges", 
		/// ie those maps which have a block of trees, 
		/// a block of the sea, a block of rock, etc. 
		/// ... to continue the maps. 
		/// Each block has its own height.
		/// </summary>
		public int MapHeight;
		///// <summary>
		///// An Array of Ints that specify elevation for specific tile node (individual quads).
		///// Z elevation to <see cref="Tiles"/>' [X,Y]
		///// </summary>
		//public sbyte[] HeightMap;
		public int MusicDay { get; private set; }
		public int MusicNight { get; private set; }
		/// <summary>
		/// If true, Wild Pokemon can be found every step you take and not just in grass.
		/// </summary>
		public bool WildPokemon2 { get; private set; }
		/// <summary>
		/// Allows the following pokemon to show.
		/// </summary>
		public bool OverworldPokemon { get; private set; }
		#endregion
		#region Actions
		/// <summary>
		/// Allows Teleporting
		/// </summary>
		public bool CanTeleport { get; private set; }
		/// <summary>
		/// Allows Digging
		/// </summary>
		public bool CanDig { get; private set; }
		/// <summary>
		/// Allows Flying
		/// </summary>
		public bool CanFly { get; private set; }
		/// <summary>
		/// Allows Riding
		/// </summary>
		/// RideType
		/// null=> 0 - If both CanDig and CanFly = false then Cannot ride.
		/// 1	=> 1 - Can Ride.
		/// 0	=> 2 - Cannot Ride.
		public bool? CanRide { get; private set; }
		/// <summary>
		/// Determines the Environment(the background).
		/// </summary>
		/// 0 - Day/Night cycle
		/// 1 - Day
		/// 2 - Cave
		/// 3 - Night
		/// 4 - UnderWater
		/// Anything not listed defaults to 0.
		public Overworld.Environment Environment { get; set; }
		//ToDo: 0:Random, 1:Normal, Bubbles?
		public Weather Weather;
		/// <summary>
		/// A variety(0-3) of different lighting effects.
		/// </summary>
		public byte Lightning;
		/// <summary>
		/// Makes the cave dark to use flash for.
		/// </summary>
		public bool IsDark { get; private set; }
		/// <summary>
		/// Makes all wild battles into "Safari" Battles
		/// </summary>
		public bool IsSafariZone { get; private set; }
		/// <summary>
		/// Used during bug catching contest.
		/// </summary>
		public string BugCatchingContest { get; private set; }
		#endregion
		//int Texture1;
		//int Texture2;
		public int Scripts;
		/// <summary>
		/// </summary>
		/// A script to run upon entering the map(after landing if flying).
		public int MapScripts;
		public int Texts;
		/// <summary>
		/// Table or Encounter chart for pokemons expected to find on map.
		/// <para></para>
		/// Points to another array id, 
		/// where the Pokemon Encounters are expressed in detail
		/// </summary>
		public int WildPokemon;
		public int Events;
		public int Flags;
		public int Camera;
		/// <summary>
		/// </summary>
		/// For height loop or For width loop
		/// [Z,i] = gameobject int value
		/// each value in for loop should be rounded to nearest whole number
		/// need to map collision to X,Y value as well...
		public Tile[][,] MapArray;
		public Tile[,] Tiles { get; private set; }
	}
	#endregion
}