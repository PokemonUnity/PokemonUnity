using System;
using System.Collections;
using System.Collections.Generic;
//using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

namespace PokemonUnity
{

	/// <summary>
	/// Map GameObject will be an Array of tiles
	/// mapEnum = MapId = BlenderJsonToUnity[]
	/// </summary>
	public class JsonToUnity
	{
		#region Variables
		/// <summary>
		/// Vector3 || (float x, float y, float z)
		/// </summary>
		/// we dont need ray casting anymore, since all the x,y,z's are recorded
		double tileLocation;
		/// <summary>
		/// Quaternion || (float x, float y, float z)
		/// ACtually, dont need any of the other rotations except for Z. 
		/// </summary>
		double tileRotation;
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
		#endregion

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
		 */
		public static Tile[] ImportBlender(string filename)
		{
			List<Tile> data = new List<Tile>(); //Why not convert dictionary to Array? It's faster, more streamlined, and simpler to work with

			// Filter on only json files, otherwise you can also get other files (.meta)
			string[] fileEntries = System.IO.Directory.GetFiles(UnityEngine.Application.streamingAssetsPath + "/Pokemons", "*.json");
			foreach (string fileName in fileEntries)
			{
				if (filename == fileName)
				{
					string dataAsJson = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
					#region Direction & Rotation
					//Either the tile's rotation is based on the actual Z rotation of object, 
					//OR based on name of tile (i.e. CornerBottomLeft, CliffSideRight)
					Direction direction = 0;
					if (Math.Floor(double.Parse(dataAsJson)) == 0d)//.RotationZ
						direction = 0;
					else if (Math.Floor(double.Parse(dataAsJson)) == 0d)//.RotationZ
						direction = 0;
					#endregion Direction & Rotation
					#region Tile Shape/Piece
					Shape shape = 0;
					switch (dataAsJson)
					{
						default:
							shape = Shape.NULL;
							break;
					}
					#endregion Tile Shape/Piece
					#region Direction
					#endregion
					Tile pokemonData = new Tile()
					{
						X = 0,
						Y = 0//, Z = 0
						,
						Direction = (Direction)direction
					};
					UnityEngine.JsonUtility.FromJsonOverwrite(dataAsJson, pokemonData);
					data.Add(pokemonData);
				}
			}

			return data.ToArray();
		}

		private static Dictionary<int, Tile> LoadMaps()
		{
			var data = new Dictionary<int, Tile>(); //Why not convert dictionary to Array? It's faster, more streamlined, and simpler to work with

			// Filter on only json files, otherwise you can also get other files (.meta)
			string[] fileEntries = System.IO.Directory.GetFiles(UnityEngine.Application.streamingAssetsPath + "/Pokemons", "*.json");
			foreach (string fileName in fileEntries)
			{
				string dataAsJson = System.IO.File.ReadAllText(fileName, Encoding.UTF8);
				Tile pokemonData = new Tile();
				UnityEngine.JsonUtility.FromJsonOverwrite(dataAsJson, pokemonData);
				//data.Add(pokemonData.ID, pokemonData);
			}

			return data; //Right here, a ".ToArray()" or maybe a for-loop Array[n] = Dictionary<n>
		}

		/// <summary>
		/// Builds an <see cref="JsonLocalizationDictionary" /> from given file.
		/// </summary>
		/// <param name="filePath">Path of the file</param>
		public static Dictionary<int, Tile> BuildFromFile(string filePath)
		{
			try
			{
				return BuildFromJsonString(File.ReadAllText(filePath));
			}
			catch (Exception ex)
			{
				throw new Exception("Invalid localization file format! " + filePath, ex);
			}
		}

		/// <summary>
		/// Builds an <see cref="JsonLocalizationDictionary" /> from given json string.
		/// </summary>
		/// <param name="jsonString">Json string</param>
		public static Dictionary<int, Tile> BuildFromJsonString(string jsonString)
		{
			JsonLocalizationFile jsonFile = new JsonLocalizationFile();
			try
			{
#if (DEBUG == false || UNITY_EDITOR == true)
				UnityEngine.JsonUtility.FromJsonOverwrite(jsonString, jsonFile);
			} catch (Exception ex) { 
#else
				jsonFile = JsonConvert.DeserializeObject<JsonLocalizationFile>(
					jsonString,
					new JsonSerializerSettings
					{
						ContractResolver = new CamelCasePropertyNamesContractResolver()
					});
			}
			catch (JsonException ex)
			{
				throw new Exception("Can not parse json string. " + ex.Message);
#endif
			}

			var cultureCode = jsonFile.MapHeader;
			if (string.IsNullOrEmpty(cultureCode))
			{
				throw new Exception("Culture is empty in language json file.");
			}

			var dictionary = new Dictionary<int, Tile>();// CultureInfo.GetCultureInfo(cultureCode));
			var dublicateNames = new List<string>();
			foreach (var item in jsonFile.Texts)
			{
				if (string.IsNullOrEmpty(item.Key))
				{
					throw new Exception("The key is empty in given json string.");
				}

				if (dictionary.Contains(item.Key))
				{
					dublicateNames.Add(item.Key);
				}

				dictionary[item.Key] = item.Value.NormalizeLineEndings();
			}

			if (dublicateNames.Count > 0)
			{
				throw new Exception(
					"A dictionary can not contain same key twice. There are some duplicated names: " +
					dublicateNames.JoinAsString(", "));
			}

			return dictionary;
		}

		/// <summary>
		/// Use it to serialize json file
		/// </summary>
		public class JsonLocalizationFile
		{
			/// <summary>
			/// Constructor
			/// </summary>
			public JsonLocalizationFile()
			{
				Texts = new Dictionary<string, string>();
			}

			/// <summary>
			/// get or set the culture name; eg : en , en-us, zh-CN
			/// </summary>
			public string MapHeader { get; set; }

			/// <summary>
			///  Key value pairs
			/// </summary>
			public Dictionary<string, string> Texts { get; private set; }
		}
	}
	public class LocalizationDictionary //: IEnumerable<LocalizedString>
	{
		public virtual string this[string name]
		{
			get
			{
				var localizedString = GetOrNull(name);
				return localizedString == null? null : localizedString.Value;
			}
			set
			{
				_dictionary[name] = new LocalizedString(name, value, "");
			}
		}


		private readonly Dictionary<string, LocalizedString> _dictionary;

		/// <summary>
		/// Creates a new <see cref="LocalizationDictionary"/> object.
		/// </summary>
		/// <param name="cultureInfo">Culture of the dictionary</param>
		public LocalizationDictionary(string cultureInfo)
		{
			//CultureInfo = cultureInfo;
			_dictionary = new Dictionary<string, LocalizedString>();
		}
		
		public virtual LocalizedString GetOrNull(string name)
		{
			LocalizedString localizedString = null;
			return _dictionary.TryGetValue(name, out localizedString) ? localizedString : null;
		}
		
		//public virtual IReadOnlyList<LocalizedString> GetAllStrings()
		//{
		//	return _dictionary.Values.ToImmutableList();
		//}
		//
		//public virtual IEnumerator<LocalizedString> GetEnumerator()
		//{
		//	return GetAllStrings().GetEnumerator();
		//}
		//
		//IEnumerator IEnumerable.GetEnumerator()
		//{
		//	return GetAllStrings().GetEnumerator();
		//}

		protected bool Contains(string name)
		{
			return _dictionary.ContainsKey(name);
		}
	}

	/// <summary>
	/// Represents a localized string.
	/// </summary>
	public class LocalizedString
	{
		/// <summary>
		/// Culture info for this string.
		/// </summary>
		public string CultureInfo { get; internal set; }

		/// <summary>
		/// Unique Name of the string.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Value for the <see cref="Name"/>.
		/// </summary>
		public string Value { get; private set; }

		/// <summary>
		/// Creates a localized string instance.
		/// </summary>
		/// <param name="cultureInfo">Culture info for this string</param>
		/// <param name="name">Unique Name of the string</param>
		/// <param name="value">Value for the <paramref name="name"/></param>
		public LocalizedString(string name, string value, string cultureInfo)
		{
			Name = name;
			Value = value;
			CultureInfo = cultureInfo;
		}
	}
}