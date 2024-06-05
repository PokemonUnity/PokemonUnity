using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;
using PokemonEssentials.Interface.Item;
using PokemonEssentials.Interface.Screen;
using PokemonEssentials.Interface.EventArg;
using PokemonEssentials.Interface.PokeBattle;
using PokemonEssentials.Interface.PokeBattle.Effects;
//using PokemonEssentials.Interface.PokeBattle.Rules;
using PokemonEssentials.Interface.RPGMaker.Kernal;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

namespace PokemonUnity.Interface.UnityEngine
{
	/// <summary>
	/// </summary>
	/// https://www.youtube.com/watch?v=64NblGkAabk
	[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer)), RequireComponent(typeof(MeshCollider))]
	public partial class TileMapGenerator : MonoBehaviour
	{
		public int uvId = 0;
		public Texture texture;
		public SpriteAtlas spriteAtlas;
		private MeshRenderer meshRenderer;
		private MeshFilter meshFilter;
		private MeshCollider meshCollider;
		private Mesh mesh;
		private List<Vector3> vertices = new List<Vector3>();
		private List<int> triangles = new List<int>();
		private List<Vector2> uvs = new List<Vector2>();
		private GameObject prefab;
		private int Width;
		private int Height;
		private MapTileNode[][] mapData;
		//private Dictionary<int, MapTileNode> mapData;
		//private Dictionary<int, Terrains> tileData;

		//public TextAsset jsonFile; // Drag and drop your JSON file here in the inspector
		//public Vector2[][] uvMap; // Set your UV map here
		private Dictionary<int, Vector2[]> uvMap;

		private void Awake()
		{
			//parentGameObject = transform;
			//tileData = new Dictionary<int, Terrains>();
			uvMap = new Dictionary<int, Vector2[]>();
			// Populate the UV map dictionary
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					Vector2[] uvCoordinates = new Vector2[]
					{
						new Vector2(i * 0.25f, j * 0.25f),
						new Vector2((i + 1) * 0.25f, j * 0.25f),
						new Vector2((i + 1) * 0.25f, (j + 1) * 0.25f),
						new Vector2(i * 0.25f, (j + 1) * 0.25f)
					};

					int key = i + j * 4;
					uvMap.Add(key, uvCoordinates);
				}
			};
		}

		private void Start()
		{
			mesh = new Mesh();
			mesh.name = "LevelMapTiles";
			meshFilter = GetComponent<MeshFilter>();
			meshRenderer = GetComponent<MeshRenderer>();
			meshCollider = GetComponent<MeshCollider>();
			//meshCollider = gameObject.AddComponent<MeshCollider>();
			meshCollider.sharedMesh = null;
			Material material = new Material(Shader.Find("Diffuse"));
			//material.mainTexture = spriteAtlas.GetSprite("col_tile").texture;
			material.mainTexture = texture;
			meshRenderer.material = material;

			LoadMapData();
			CreateMap();
			UpdateMesh();


			meshFilter.mesh = mesh;
			meshFilter.sharedMesh = mesh;
			meshCollider.sharedMesh = mesh;
			//meshCollider.sharedMesh = meshFilter.sharedMesh;
		}

		private void LoadMapData()
		{
			string path = global::UnityEngine.Application.dataPath + "/StreamingAssets/MapJson/map001.json";
			bool canLoad = File.Exists(path);
			if (!canLoad)
			{
				Debug.Log(path);
				return;
			}
			Debug.Assert(canLoad);
			string jsonString = File.ReadAllText(path);
			JsonMap jsonMap = JsonConvert.DeserializeObject<JsonMap>(jsonString);
			Width = jsonMap.width; Height = jsonMap.height;
			//mapData = new Dictionary<int, MapTileNode>();
			mapData = new MapTileNode[Width][];
			for (int n = 0; n < Width; n++)
				mapData[n] = new MapTileNode[Height];

			//for each tile in json
			foreach (JsonTile tile in jsonMap.tiles)
			{
				// Convert the rotation from Euler angles to a quaternion
				Quaternion rotation = Quaternion.Euler(tile.rotation[0],tile.rotation[1],tile.rotation[2]);

				// Calculate tile position from object position
				int x = Mathf.FloorToInt(tile.vector[0]);
				int y = Mathf.FloorToInt(tile.vector[1]);

				// Calculate size from object scale
				int width = 1;
				int height = 1;
				Terrains terrains = Terrains.Neutral;

				// if-statement based on mesh shape...
				//width = Mathf.RoundToInt(obj.transform.localScale.x);
				//height = Mathf.RoundToInt(obj.transform.localScale.y);
				//Terrains terrains = Terrains.Neutral;

				// Update tiles covered by object
				for (int i = x; i < x + width; i++)
				{
					for (int j = y; j < y + height; j++)
					{
						//int key = i * 10000 + j; // generate unique key from x,y
						MapTileNode t = new MapTileNode()
						{
							tileRotation = rotation,
							tileLocation = new Vector3(i, j),
							tileShape = terrains,
							tileTexture = tile.material,
							tileCollision = tile.mesh
						};
						//mapData.Add(key, t);
						mapData[i][j] = t;
					}
				}
			}
		}

		private void CreateMap()
		{
			float upperLeftTile		= 0;
			float upperRightTile	= 0;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					MapTileNode tile = mapData[x][y];
					//if (tile.tileLocation.x != x || tile.tileLocation.y != x) continue; //this would create gaps in the map if tiles are not found in array
					upperLeftTile	= mapData[x][y].tileLocation.z; // Make last row of triangle vectors leveled
					upperRightTile	= mapData[x][y].tileLocation.z; // Make last row of triangle vectors leveled
					//build map from bottom left to top right
					//if not the last row of tiles at top of map
					//then match tile's bottom edge to next tile's top edge
					if (y < Height - 1)
					{
						//this should be a boolean to check if not missing tile in map array sequence
						//should replace expected tile with a default tile
						upperLeftTile	= mapData[x][y + 1].tileLocation.z;
						upperRightTile	= mapData[x][y + 1].tileLocation.z;
					}

					// Add vertices
					vertices.Add(new Vector3(x,		tile.tileLocation.z,	y));
					vertices.Add(new Vector3(x,		upperLeftTile,			y + 1));
					vertices.Add(new Vector3(x + 1,	upperRightTile,			y + 1));
					vertices.Add(new Vector3(x + 1,	tile.tileLocation.z,	y));

					// Add triangles
					if (tile.tileShape == Terrains.Rock ||
						tile.tileShape == Terrains.Ledge ||
						tile.tileShape == Terrains.Waterfall)
					{
						triangles.Add(vertices.Count - 4);
						triangles.Add(vertices.Count - 3);
						triangles.Add(vertices.Count - 1);
						triangles.Add(vertices.Count - 3);
						triangles.Add(vertices.Count - 2);
						triangles.Add(vertices.Count - 1);
					}
					else if (tile.tileTexture == "WallMat")
					{
						triangles.Add(vertices.Count - 4);
						triangles.Add(vertices.Count - 1);
						triangles.Add(vertices.Count - 3);
						triangles.Add(vertices.Count - 2);
						triangles.Add(vertices.Count - 3);
						triangles.Add(vertices.Count - 1);

						Vector3 position = new Vector3(x + .5f, 0.5f, y + .5f);
						Vector3 pivotOffset = new Vector3(-.5f, 0, -.5f); // bottom left origin and pivot point

						// Rotate the pivot offset
						Vector3 rotatedOffset = tile.tileRotation * pivotOffset;
						// Calculate the differences caused by the rotation
						Vector3 offsetDifference = rotatedOffset - pivotOffset;
						// Adjust the position of the object considering the rotation offset
						Vector3 adjustedPosition = position - offsetDifference;
						Material material = new Material(Shader.Find("Diffuse"));
						//material.mainTexture = spriteAtlas.GetSprite("col_tile").texture;
						material.mainTexture = texture;
						//Vector4 uv = new Vector4(x: uvMap[4][0].x, y: uvMap[4][0].y, z: uvMap[4][1].x, w: uvMap[4][1].y); //new Vector4(1, 1, 0, 0)
						Vector4 uv = new Vector4(x: .25f, y: .25f, z: 0, w: .25f); //WallMat
						material.SetVector("_MainTex_ST", uv);
						prefab = GameObject.CreatePrimitive(PrimitiveType.Cube);
						prefab.GetComponent<MeshRenderer>().material = material;
						prefab.transform.position = adjustedPosition;
						prefab.transform.rotation = Quaternion.Euler(tile.tileRotation.eulerAngles - new Vector3(90, 0, 0));
						prefab.transform.SetParent(transform);
						//Instantiate(prefab, adjustedPosition, Quaternion.Euler(tile.tileRotation.eulerAngles - new Vector3(90,0,0)), transform);
					}
					else
					{
						triangles.Add(vertices.Count - 4);
						triangles.Add(vertices.Count - 3);
						triangles.Add(vertices.Count - 2);
						triangles.Add(vertices.Count - 4);
						triangles.Add(vertices.Count - 2);
						triangles.Add(vertices.Count - 1);
					}

					// Add UVs
					if (tile.tileTexture == "WallMat")
						uvs.AddRange(uvMap[4]);
					else if (tile.tileTexture == "GrassMat")
						uvs.AddRange(uvMap[1]);
					else if (tile.tileTexture == "FloorMat")
						uvs.AddRange(uvMap[0]);
					else
						uvs.AddRange(uvMap[uvId]); //tile.type
				}
			}
		}

		private void UpdateMesh()
		{
			mesh.Clear();

			mesh.vertices = vertices.ToArray();
			mesh.triangles = triangles.ToArray();
			mesh.uv = uvs.ToArray();
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
		}

		//private void OnDrawGizmos()
		//{
		//	if (vertices == null) return;
		//	for (int i = 0; i < vertices.Count; i++)
		//	{
		//		Gizmos.DrawSphere(vertices[i], .1f);
		//	}
		//}

		private struct MapTileNode
		{
			public Vector3 tileLocation;
			public Quaternion tileRotation;
			public Terrains tileShape; // Mesh object
			public string tileTexture; // Texture enum or filename
			public string tileCollision; //enum here... Mesh object will determine collision mapping
		}

		private class JsonMap
		{
			public int width { get; set; }
			public int height { get; set; }
			public List<JsonTile> tiles { get; set; }
		}

		private class JsonTile
		{
			public string mesh { get; set; }
			public List<float> vector { get; set; }
			public List<float> rotation { get; set; }
			public string material { get; set; }
		}
	}
}