using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using PokemonUnity.Overworld.Entity;
using PokemonUnity.Overworld.Entity.Misc;
using PokemonUnity.Overworld.Entity.Environment;
using UnityEngine;

namespace PokemonUnity.Overworld
{
	public class LevelLoader
	{
		const bool MULTITHREAD = false;

		public static List<Vector3> LoadedOffsetMapOffsets = new List<Vector3>();
		public static List<string> LoadedOffsetMapNames = new List<string>();

		private enum TagTypes
		{
			Entity,
			Floor,
			EntityField,
			Level,
			LevelActions,
			NPC,
			Shader,
			OffsetMap,
			Structure,
			Backdrop,
			None
		}

		private Vector3 Offset;
		private bool loadOffsetMap = true;
		private int offsetMapLevel = 0;
		private string MapOrigin = "";
		private List<string> sessionMapsLoaded = new List<string>(); // Prevents infinite loops when loading more than one offset map level.

		// Store these so other classes can get them:
		private List<Entity.Entity> Entities = new List<Entity.Entity>();
		private List<Entity.Entity> Floors = new List<Entity.Entity>();

		// A counter across all LevelLoader instances to count how many instances across the program are active:
		private static int Busy = 0;

		public static bool IsBusy
		{
			get
			{
				return Busy > 0;
			}
		}


		private object[] TempParams;

		/// <summary>
		/// Loads the level.
		/// </summary>
		/// <param name="Params">Params contruction: String LevelFile, bool IsOffsetMap, Vector3 Offset, int Offsetmaplevel, Str() InstanceLoadedOffsetMaps</param>
		public void LoadLevel(object[] Params)
		{
			Busy += 1;
			TempParams = Params;

			if (MULTITHREAD)
			{
				System.Threading.Thread t = new System.Threading.Thread(InternalLoad);
				t.IsBackground = true;
				t.Start();
			}
			else
				InternalLoad();
		}

		private void InternalLoad()
		{
			string levelPath = System.Convert.ToString(TempParams[0]);
			bool loadOffsetMap = System.Convert.ToBoolean(TempParams[1]);
			Vector3 offset = (Vector3)TempParams[2];
			offsetMapLevel = System.Convert.ToInt32(TempParams[3]);
			sessionMapsLoaded = (List<string>)TempParams[4];

			Stopwatch timer = new Stopwatch();
			timer.Start();

			this.loadOffsetMap = loadOffsetMap;
			MapOrigin = levelPath;

			if (!loadOffsetMap)
			{
				//ToDo: Change Screen to Scene
				Game.Level.LevelFile = levelPath;

				//Game.Player.LastSavePlace = Game.Level.LevelFile;
				//Game.Player.LastSavePlacePosition = Player.Temp.LastPosition.X + "," + Player.Temp.LastPosition.y.ToString().Replace(StringHelper.DecSeparator, ".") + "," + Player.Temp.LastPosition.z;

				Game.Level.Entities.Clear();
				Game.Level.Floors.Clear();
				Game.Level.Shaders.Clear();
				//Game.Level.BackdropRenderer.Clear();

				Game.Level.OffsetmapFloors.Clear();
				Game.Level.OffsetmapEntities.Clear();

				Game.Level.WildPokemonFloor = false;
				Game.Level.WalkedSteps = 0;

				LoadedOffsetMapNames.Clear();
				LoadedOffsetMapOffsets.Clear();
				Floor.ClearFloorTemp();

				//Player.Temp.MapSteps = 0;

				sessionMapsLoaded.Add(levelPath);
			}

			levelPath = "";//GameModeManager.GetMapPath(levelPath);

			Game.DebugLog("Loading map: " + levelPath.Remove(0, Saving.SaveManager.saveLocation.Length));
			//System.Security.FileValidation.CheckFileValid(levelPath, false, "LevelLoader.vb");

			if (!System.IO.File.Exists(levelPath))
			{
				Game.DebugLog("LevelLoader.vb: Error accessing map file \"" + levelPath + "\". File not found.", true);
				Busy -= 1;

				//if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen & !loadOffsetMap)
				//    ((OverworldScreen)CurrentScreen).Titles.Add(new OverworldScreen.Title("Couldn't find map file!", 20.0f, UnityEngine.Color.white, 6.0f, Vector2.zero, true));

				return;
			}

			List<string> Data = System.IO.File.ReadAllLines(levelPath).ToList();
			Dictionary<string, object> Tags = new Dictionary<string, object>();

			this.Offset = offset;

			foreach (string line in Data)
			{
				if (line.Contains("{"))
				{
					string l = line.Remove(0, line.IndexOf("{"));

					if (l.StartsWith("{\"Comment\"{COM"))
					{
						l = l.Remove(0, l.IndexOf("[") + 1);
						l = l.Remove(l.IndexOf("]"));

						Game.DebugLog(l);
					}
				}
			}

			int countLines = 0;

			for (var i = 0; i <= int.MaxValue; i++)
			{
				if (i > Data.Count - 1)
					break;

				string line = Data[i];
				Tags.Clear();
				if (line.Contains("{") & line.Contains("}"))
				{
					try
					{
						TagTypes TagType = TagTypes.None;
						line = line.Remove(0, line.IndexOf("{") + 2);

						if (line.ToLower().StartsWith("structure\""))
							TagType = TagTypes.Structure;

						if (TagType == TagTypes.Structure)
						{
							line = line.Remove(0, line.IndexOf("[") + 1);
							line = line.Remove(line.Length - 3, 3);

							Tags = GetTags(line);

							string[] newLines = AddStructure(Tags);

							Data.InsertRange(i + 1, newLines);
						}
					}
					catch (Exception ex)
					{
						Game.DebugLog("LevelLoader.vb: Failed to load map object! (Index: " + countLines + ") from mapfile: " + levelPath + "; Error message: " + ex.Message, false);
					}
				}
			}

			foreach (string line in Data)
			{
				Tags.Clear();
				string orgLine = line;
				countLines += 1;

				if (line.Contains("{") & line.Contains("}"))
				{
					try
					{
						TagTypes TagType = TagTypes.None;
						string l = line.Remove(0, line.IndexOf("{") + 2);

						if (l.ToLower().StartsWith("entity\""))
							TagType = TagTypes.Entity;
						if (l.ToLower().StartsWith("floor\""))
							TagType = TagTypes.Floor;
						if (l.ToLower().StartsWith("entityfield\""))
							TagType = TagTypes.EntityField;
						if (l.ToLower().StartsWith("level\""))
							TagType = TagTypes.Level;
						if (l.ToLower().StartsWith("actions\""))
							TagType = TagTypes.LevelActions;
						if (l.ToLower().StartsWith("npc\""))
							TagType = TagTypes.NPC;
						if (l.ToLower().StartsWith("shader\""))
							TagType = TagTypes.Shader;
						if (l.ToLower().StartsWith("offsetmap\""))
							TagType = TagTypes.OffsetMap;
						if (l.ToLower().StartsWith("backdrop\""))
							TagType = TagTypes.Backdrop;

						if (TagType != TagTypes.None)
						{
							l = l.Remove(0, l.IndexOf("[") + 1);
							l = l.Remove(l.Length - 3, 3);

							Tags = GetTags(l);

							switch (TagType)
							{
								case TagTypes.EntityField:
									{
										EntityField(Tags);
										break;
									}
								case TagTypes.Entity:
									{
										//AddEntity(Tags, new Size(1, 1), 1, true, new Vector3(1, 1, 1));
										break;
									}
								case TagTypes.Floor:
									{
										AddFloor(Tags);
										break;
									}
								case TagTypes.Level:
									{
										if (!loadOffsetMap)
											SetupLevel(Tags);
										break;
									}
								case TagTypes.LevelActions:
									{
										if (!loadOffsetMap)
											SetupActions(Tags);
										break;
									}
								case TagTypes.NPC:
									{
										AddNPC(Tags);
										break;
									}
								case TagTypes.Shader:
									{
										if (!loadOffsetMap)
											AddShader(Tags);
										break;
									}
								case TagTypes.OffsetMap:
									{
										//if (!loadOffsetMap | offsetMapLevel <= Core.GameOptions.MaxOffsetLevel)
										//    AddOffsetMap(Tags);
										break;
									}
								case TagTypes.Backdrop:
									{
										if (!loadOffsetMap)
											AddBackdrop(Tags);
										break;
									}
							}
						}
					}
					catch (Exception ex)
					{
						Game.DebugLog("LevelLoader.vb: Failed to load map object! (Index: " + countLines + ") (Line: " + orgLine + ") from mapfile: " + levelPath + "; Error message: " + ex.Message, false);
					}
				}
			}

			if (!loadOffsetMap)
				LoadBerries();

			foreach (Shader s in Game.Level.Shaders)
			{
				//if (!s.HasBeenApplied)
				//{
				//    s.ApplyShader(Game.Level.Entities.ToArray());
				//    s.ApplyShader(Game.Level.Floors.ToArray());
				//}
			}

			Game.DebugLog("Map loading finished: " + levelPath.Remove(0, Saving.SaveManager.saveLocation.Length));
			Game.DebugLog("Loaded textures: " + TextureManager.TextureList.Count.ToString());
			timer.Stop();
			Game.DebugLog("Map loading time: " + timer.ElapsedTicks + " Ticks; " + timer.ElapsedMilliseconds + " Milliseconds.");

			// var xmlLevelLoader = mew XmlLevelLoader();
			// xmlLevelLoader.Load(My.Computer.FileSystem.SpecialDirectories.Desktop & "\t.xml", _5DHero.XmlLevelLoader.LevelTypes.Default, Vector3.Zero)

			Busy -= 1;

			if (Busy == 0)
				Game.Level.StartOffsetMapUpdate();
		}

		private Dictionary<string, object> GetTags(string line)
		{
			Dictionary<string, object> Tags = new Dictionary<string, object>();

			var tagList = line.Split(new[] { "}{" }, StringSplitOptions.RemoveEmptyEntries);
			for (var i = 0; i <= tagList.Length - 1; i++)
			{
				string currentTag = tagList[i];
				if (!currentTag.EndsWith("}}"))
					currentTag += "}";
				if (!currentTag.StartsWith("{"))
					currentTag = "{" + currentTag;
				ProcessTag(ref Tags, currentTag);
			}

			return Tags;
		}

		private void ProcessTag(ref Dictionary<string, object> Dictionary, string Tag)
		{
			string TagName = "";
			string TagContent = "";

			Tag = Tag.Remove(0, 1);
			Tag = Tag.Remove(Tag.Length - 1, 1);

			TagName = Tag.Remove(Tag.IndexOf("{") - 1).Remove(0, 1);
			TagContent = Tag.Remove(0, Tag.IndexOf("{"));

			string[] ContentRows = TagContent.Split(System.Convert.ToChar("}"));
			foreach (string subTag in ContentRows)
			{
				if (subTag.Length > 0)
				{
					string s = subTag.Remove(0, 1);

					string subTagType = s.Remove(s.IndexOf("["));
					string subTagValue = s.Remove(0, s.IndexOf("[") + 1);
					subTagValue = subTagValue.Remove(subTagValue.Length - 1, 1);

					switch (subTagType.ToLower())
					{
						case "int":
							{
								Dictionary.Add(TagName, int.Parse(subTagValue));
								break;
							}
						case "str":
							{
								Dictionary.Add(TagName, (string)subTagValue);
								break;
							}
						case "sng":
							{
								subTagValue = subTagValue.Replace(".", StringHelper.DecSeparator);
								Dictionary.Add(TagName, float.Parse(subTagValue));
								break;
							}
						case "bool":
							{
								Dictionary.Add(TagName, bool.Parse(subTagValue));
								break;
							}
						case "intarr":
							{
								string[] values = subTagValue.Split(System.Convert.ToChar(","));
								List<int> arr = new List<int>();
								foreach (string value in values)
									arr.Add(System.Convert.ToInt32(value));
								Dictionary.Add(TagName, arr);
								break;
							}
						case "intarr2d":
							{
								string[] rows = subTagValue.Split(System.Convert.ToChar("]"));
								List<List<int>> arr = new List<List<int>>();
								foreach (string row in rows)
								{
									if (row.Length > 0)
									{
										string r = row.Remove(0, 1);
										List<int> list = new List<int>();
										foreach (var value in r.Split(System.Convert.ToChar(",")))
											list.Add(System.Convert.ToInt32(value));
										arr.Add(list);
									}
								}
								Dictionary.Add(TagName, arr);
								break;
							}
						case "rec":
							{
								string[] content = subTagValue.Split(System.Convert.ToChar(","));
								Dictionary.Add(TagName, new Vector4(System.Convert.ToInt32(content[0]), System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3])));
								break;
							}
						case "recarr":
							{
								string[] values = subTagValue.Split(System.Convert.ToChar("]"));
								List<Vector4> arr = new List<Vector4>();
								foreach (string value in values)
								{
									if (value.Length > 0)
									{
										string v = value.Remove(0, 1);

										string[] content = v.Split(System.Convert.ToChar(","));
										arr.Add(new Vector4(System.Convert.ToInt32(content[0]), System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3])));
									}
								}
								Dictionary.Add(TagName, arr);
								break;
							}
						case "sngarr":
							{
								string[] values = subTagValue.Split(System.Convert.ToChar(","));
								List<float> arr = new List<float>();
								foreach (string value in values)
								{
									string v = value.Replace(".", StringHelper.DecSeparator);
									arr.Add(System.Convert.ToSingle(v));
								}
								Dictionary.Add(TagName, arr);
								break;
							}
					}
				}
			}
		}

		private object GetTag(Dictionary<string, object> Tags, string TagName)
		{
			if (Tags.ContainsKey(TagName))
				return Tags[TagName];

			for (var i = 0; i <= Tags.Count - 1; i++)
			{
				if (Tags.ElementAt(i).Key.ToLower() == TagName.ToLower())
					return Tags.ElementAt(i).Value;
			}

			return null;
		}

		private bool TagExists(Dictionary<string, object> Tags, string TagName)
		{
			if (Tags.ContainsKey(TagName))
				return true;

			for (var i = 0; i <= Tags.Count - 1; i++)
			{
				if (Tags.ElementAt(i).Key.ToLower() == TagName.ToLower())
					return true;
			}

			return false;
		}


		private void AddOffsetMap(Dictionary<string, object> Tags)
		{
			//if (Core.GameOptions.LoadOffsetMaps > 0)
			//{
			//    List<int> OffsetList = (List<int>)GetTag(Tags, "Offset");
			//    Vector3 MapOffset = new Vector3(OffsetList[0], 0, OffsetList[1]);
			//    if (OffsetList.Count >= 3)
			//        MapOffset = new Vector3(OffsetList[0], OffsetList[1], OffsetList[2]);
			//
			//    string MapName = System.Convert.ToString(GetTag(Tags, "Map"));
			//
			//    if (loadOffsetMap)
			//    {
			//        if (sessionMapsLoaded.Contains(MapName))
			//            return;
			//    }
			//    sessionMapsLoaded.Add(MapName);
			//
			//    LoadedOffsetMapNames.Add(MapName);
			//    LoadedOffsetMapOffsets.Add(MapOffset);
			//
			//    string listName = Game.Level.LevelFile + "|" + MapName + "|" + Game.Level.World.CurrentMapWeather + "|" + World.GetCurrentRegionWeather() + "|" + World.GetTime() + "|" + World.CurrentSeason();
			//    if (!OffsetMaps.ContainsKey(listName))
			//    {
			//        List<List<Entity.Entity>> mapList = new List<List<Entity.Entity>>();
			//
			//        List<object> @params = new List<object>();
			//        @params.AddRange(new[]
			//        {
			//            (object)MapName,
			//            (object)true,
			//            (object)(MapOffset + Offset),
			//            (object)(offsetMapLevel + 1),
			//            (object)sessionMapsLoaded
			//        });
			//
			//        int offsetEntityCount = Game.Level.OffsetmapEntities.Count;
			//        int offsetFloorCount = Game.Level.OffsetmapFloors.Count;
			//
			//        LevelLoader levelLoader = new LevelLoader();
			//        levelLoader.LoadLevel(@params.ToArray());
			//        List<Entity.Entity> entList = new List<Entity.Entity>();
			//        List<Entity.Entity> floorList = new List<Entity.Entity>();
			//
			//        for (int i = offsetEntityCount; i <= Game.Level.OffsetmapEntities.Count - 1; i++)
			//            entList.Add(Game.Level.OffsetmapEntities(i));
			//        for (int i = offsetFloorCount; i <= Game.Level.OffsetmapFloors.Count - 1; i++)
			//            floorList.Add(Game.Level.OffsetmapFloors(i));
			//        mapList.AddRange(new List<List<Entity.Entity>>()
			//        {
			//            entList,
			//            floorList
			//        });
			//
			//        OffsetMaps.Add(listName, mapList);
			//    }
			//    else
			//    {
			//        Game.DebugLog("Loaded Offsetmap from store: " + MapName);
			//
			//        foreach (Entity.Entity e in OffsetMaps(listName)(0))
			//        {
			//            if (e.MapOrigin == MapName)
			//            {
			//                e.IsOffsetMapContent = true;
			//                Game.Level.OffsetmapEntities.Add(e);
			//            }
			//        }
			//        foreach (Entity.Entity e in OffsetMaps(listName)(1))
			//        {
			//            if (e.MapOrigin == MapName)
			//            {
			//                e.IsOffsetMapContent = true;
			//                Game.Level.OffsetmapFloors.Add(e);
			//            }
			//        }
			//    }
			//    Game.DebugLog("Offset maps in store: " + OffsetMaps.Count);
			//
			//    Game.Level.OffsetmapEntities = (from e in Game.Level.OffsetmapEntities
			//                                      orderby e.CameraDistance descending
			//                                      select e).ToList();
			//
			//    foreach (Entity.Entity Entity in Game.Level.OffsetmapEntities)
			//        Entity.UpdateEntity();
			//    foreach (Entity.Entity Floor in Game.Level.OffsetmapFloors)
			//        Floor.UpdateEntity();
			//}
		}


		private static Dictionary<string, List<string>> tempStructureList = new Dictionary<string, List<string>>();

		public static void ClearTempStructures()
		{
			tempStructureList.Clear();
		}

		private string[] AddStructure(Dictionary<string, object> Tags)
		{
			List<float> OffsetList = (List<float>)GetTag(Tags, "Offset");
			Vector3 MapOffset = new Vector3(OffsetList[0], 0, OffsetList[1]);
			if (OffsetList.Count >= 3)
				MapOffset = new Vector3(OffsetList[0], OffsetList[1], OffsetList[2]);

			int MapRotation = -1;
			if (TagExists(Tags, "Rotation"))
				MapRotation = System.Convert.ToInt32(GetTag(Tags, "Rotation"));

			string MapName = System.Convert.ToString(GetTag(Tags, "Map"));
			if (!MapName.EndsWith(".dat"))
				MapName = MapName + ".dat";

			bool addNPC = false;
			if (TagExists(Tags, "AddNPC"))
				addNPC = System.Convert.ToBoolean(GetTag(Tags, "AddNPC"));

			string structureKey = MapOffset.x.ToString() + "|" + MapOffset.y.ToString() + "|" + MapOffset.z.ToString() + "|" + MapName;

			if (!tempStructureList.ContainsKey(structureKey))
			{
				string filepath = "";//GameModeManager.GetMapPath(MapName);
									 //System.Security.FileValidation.CheckFileValid(filepath, false, "LevelLoader.vb/StructureSpawner");

				if (!System.IO.File.Exists(filepath))
				{
					Game.DebugLog("LevelLoader.vb: Error loading structure from \"" + filepath + "\". File not found.", true);

					return new string[] { };
				}

				string[] MapContent = System.IO.File.ReadAllLines(filepath);
				List<string> structureList = new List<string>();

				foreach (string line in MapContent)
				{
					if (line.EndsWith("}"))
					{
						bool addLine = false;
						if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Entity\"{ENT["))
							addLine = true;
						if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Floor\"{ENT["))
							addLine = true;
						if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"EntityField\"{ENT["))
							addLine = true;
						if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"NPC\"{NPC["))
							if (addNPC)
								addLine = true;
						if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Shader\"{SHA["))
							addLine = true;

						if (addLine)
						{
							string l = ReplaceStructurePosition(line, MapOffset);

							if (MapRotation > -1)
								l = ReplaceStructureRotation(l, MapRotation);

							structureList.Add(l);
						}
					}
				}

				tempStructureList.Add(structureKey, structureList);
			}

			return tempStructureList[structureKey].ToArray();
		}

		private string ReplaceStructureRotation(string line, int MapRotation)
		{
			string replaceString = "";

			if (line.ToLower().Contains("{\"rotation\"{int["))
				replaceString = "{\"rotation\"{int[";

			if (replaceString != "")
			{
				string rotationString = line.Remove(0, line.ToLower().IndexOf(replaceString));
				rotationString = rotationString.Remove(rotationString.IndexOf("]}}") + 3);

				string rotationData = rotationString.Remove(0, rotationString.IndexOf("[") + 1);
				rotationData = rotationData.Remove(rotationData.IndexOf("]"));

				int newRotation = System.Convert.ToInt32(rotationData) + MapRotation;
				while (newRotation > 3)
					newRotation -= 4;

				line = line.Replace(rotationString, "{\"rotation\"{int[" + newRotation.ToString() + "]}}");
			}

			return line;
		}

		private string ReplaceStructurePosition(string line, Vector3 MapOffset)
		{
			string replaceString = "";

			if (line.ToLower().Contains("{\"position\"{sngarr["))
				replaceString = "{\"position\"{sngarr[";
			else if (line.ToLower().Contains("{\"position\"{intarr["))
				replaceString = "{\"position\"{intarr[";

			if (replaceString != "")
			{
				string positionString = line.Remove(0, line.ToLower().IndexOf(replaceString));
				positionString = positionString.Remove(positionString.IndexOf("]}}") + 3);

				string positionData = positionString.Remove(0, positionString.IndexOf("[") + 1);
				positionData = positionData.Remove(positionData.IndexOf("]"));

				string[] posArr = positionData.Split(System.Convert.ToChar(","));
				Vector3 newPosition = new Vector3(ScriptConversion.ToSingle(posArr[0].Replace(".", StringHelper.DecSeparator)) + MapOffset.x, ScriptConversion.ToSingle(posArr[1].Replace(".", StringHelper.DecSeparator)) + MapOffset.y, System.Convert.ToSingle(posArr[2].Replace(".", StringHelper.DecSeparator)) + MapOffset.z);

				if (line.ToLower().Contains("{\"position\"{sngarr["))
					line = line.Replace(positionString, "{\"position\"{sngarr[" + newPosition.x.ToString().Replace(StringHelper.DecSeparator, ".") + "," + newPosition.y.ToString().Replace(StringHelper.DecSeparator, ".") + "," + newPosition.z.ToString().Replace(StringHelper.DecSeparator, ".") + "]}}");
				else
					line = line.Replace(positionString, "{\"position\"{intarr[" + System.Convert.ToInt32(newPosition.x).ToString().Replace(StringHelper.DecSeparator, ".") + "," + System.Convert.ToInt32(newPosition.y).ToString().Replace(StringHelper.DecSeparator, ".") + "," + System.Convert.ToInt32(newPosition.z).ToString().Replace(StringHelper.DecSeparator, ".") + "]}}");
			}

			return line;
		}

		private void EntityField(Dictionary<string, object> Tags)
		{
			List<int> SizeList = (List<int>)GetTag(Tags, "Size");
			bool Fill = true;
			if (TagExists(Tags, "Fill"))
				Fill = System.Convert.ToBoolean(GetTag(Tags, "Fill"));
			Vector3 Steps = new Vector3(1, 1, 1);
			if (TagExists(Tags, "Steps"))
			{
				List<float> StepList = (List<float>)GetTag(Tags, "Steps");
				if (StepList.Count == 3)
					Steps = new Vector3(StepList[0], StepList[1], StepList[2]);
				else
					Steps = new Vector3(StepList[0], 1, StepList[1]);
			}

			//if (SizeList.Count == 3)
			//    AddEntity(Tags, new Size(SizeList[0], SizeList[2]), SizeList[1], Fill, Steps);
			//else
			//    AddEntity(Tags, new Size(SizeList[0], SizeList[1]), 1, Fill, Steps);
		}

		private void AddNPC(Dictionary<string, object> Tags)
		{
			List<float> PosList = (List<float>)GetTag(Tags, "Position");
			Vector3 Position = new Vector3(PosList[0] + Offset.x, PosList[1] + Offset.y, PosList[2] + Offset.z);

			List<float> ScaleList;
			Vector3 Scale = new Vector3(1, 1, 1);
			if (TagExists(Tags, "Scale"))
			{
				ScaleList = (List<float>)GetTag(Tags, "Scale");
				Scale = new Vector3(ScaleList[0], ScaleList[1], ScaleList[2]);
			}

			string TextureID = System.Convert.ToString(GetTag(Tags, "TextureID"));
			int Rotation = System.Convert.ToInt32(GetTag(Tags, "Rotation"));
			int ActionValue = System.Convert.ToInt32(GetTag(Tags, "Action"));
			string AdditionalValue = System.Convert.ToString(GetTag(Tags, "AdditionalValue"));
			string Name = System.Convert.ToString(GetTag(Tags, "Name"));
			int ID = System.Convert.ToInt32(GetTag(Tags, "ID"));

			string Movement = System.Convert.ToString(GetTag(Tags, "Movement"));
			List<Vector4> MoveRectangles = (List<Vector4>)GetTag(Tags, "MoveRectangles");

			Vector3 Shader = new Vector3(1.0f, 1, 1);
			if (TagExists(Tags, "Shader"))
			{
				List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
				Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
			}

			bool AnimateIdle = false;
			if (TagExists(Tags, "AnimateIdle"))
				AnimateIdle = System.Convert.ToBoolean(GetTag(Tags, "AnimateIdle"));

			NPC NPC = (NPC)Entity.Entity.GetNewEntity(PokemonUnity.Entities.NPC, Position, null, // TODO Change to default(_) if this is not a reference type 
			new int[]
			{
			0,
			0
			}, true, new Vector3(0, 0, 0), Scale/*, UnityEngine.Mesh.BillModel*/, ActionValue, AdditionalValue, true, Shader, -1, MapOrigin, "", Offset, new[]
			{
			(object)TextureID,
			(object)Rotation,
			(object)Name,
			(object)ID,
			(object)AnimateIdle,
			(object)Movement,
			(object)MoveRectangles
			});

			if (!loadOffsetMap)
				Game.Level.Entities.Add(NPC);
			else
				Game.Level.OffsetmapEntities.Add(NPC);
		}

		private void AddFloor(Dictionary<string, object> Tags)
		{
			List<int> sizeList = (List<int>)GetTag(Tags, "Size");
			//Size Size = new Size(sizeList[0], sizeList[1]);

			List<int> PosList = (List<int>)GetTag(Tags, "Position");
			Vector3 Position = new Vector3(PosList[0] + Offset.x, PosList[1] + Offset.y, PosList[2] + Offset.z);

			string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
			Vector4 TextureRectangle = (Vector4)GetTag(Tags, "Texture");
			Texture2D Texture = TextureManager.GetTexture(TexturePath, TextureRectangle);

			bool Visible = true;
			if (TagExists(Tags, "Visible"))
				Visible = System.Convert.ToBoolean(GetTag(Tags, "Visible"));

			Vector3 Shader = new Vector3(1.0f, 1, 1);
			if (TagExists(Tags, "Shader"))
			{
				List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
				Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
			}

			bool RemoveFloor = false;
			if (TagExists(Tags, "Remove"))
				RemoveFloor = System.Convert.ToBoolean(GetTag(Tags, "Remove"));

			bool hasSnow = true;
			if (TagExists(Tags, "hasSnow"))
				hasSnow = System.Convert.ToBoolean(GetTag(Tags, "hasSnow"));

			bool hasSand = true;
			if (TagExists(Tags, "hasSand"))
				hasSand = System.Convert.ToBoolean(GetTag(Tags, "hasSand"));

			bool hasIce = false;
			if (TagExists(Tags, "isIce"))
				hasIce = System.Convert.ToBoolean(GetTag(Tags, "isIce"));

			int rotation = 0;
			if (TagExists(Tags, "Rotation"))
				rotation = System.Convert.ToInt32(GetTag(Tags, "Rotation"));

			string SeasonTexture = "";
			if (TagExists(Tags, "SeasonTexture"))
				SeasonTexture = System.Convert.ToString(GetTag(Tags, "SeasonTexture"));

			List<Entity.Entity> floorList = Game.Level.Floors;
			if (loadOffsetMap)
				floorList = Game.Level.OffsetmapFloors;

			if (!RemoveFloor)
			{
				//for (var x = 0; x <= Size.width - 1; x++)
				//{
				//    for (var z = 0; z <= Size.height - 1; z++)
				//    {
				//        bool exists = false;
				//
				//        int iZ = z;
				//        int iX = x;
				//
				//        Entity.Entity Ent = null;// TODO Change to default(_) if this is not a reference type 
				//
				//        if (loadOffsetMap)
				//        {
				//            Ent = Game.Level.OffsetmapFloors.Find(e =>
				//            {
				//                return ((Entity.Entity)e).Position == new Vector3(Position.x + iX, Position.y, Position.z + iZ);
				//            });
				//        }
				//        else
				//            Ent = Game.Level.Floors.Find(e =>
				//            {
				//                return ((Entity.Entity)e).Position == new Vector3(Position.x + iX, Position.y, Position.z + iZ);
				//            });
				//
				//        if (Ent != null)
				//        {
				//            Ent.Textures = new[] { Texture };
				//            Ent.Visible = Visible;
				//            Ent.SeasonColorTexture = SeasonTexture;
				//            Ent.LoadSeasonTextures();
				//            ((Floor)Ent).SetRotation(rotation);
				//            ((Floor)Ent).hasSnow = hasSnow;
				//            ((Floor)Ent).IsIce = hasIce;
				//            ((Floor)Ent).hasSand = hasSand;
				//            exists = true;
				//        }
				//
				//        if (!exists)
				//        {
				//            Floor f = new Floor(Position.x + x, Position.y, Position.z + z, TextureManager.GetTexture(TexturePath, TextureRectangle), new[] { 0, 0 }, false, rotation, new Vector3(1.0f, 1.0f, 1.0f)/*, UnityEngine.Mesh.FloorModel*/, 0, "", Visible, Shader, hasSnow, hasIce, hasSand);
				//            f.MapOrigin = MapOrigin;
				//            f.SeasonColorTexture = SeasonTexture;
				//            f.LoadSeasonTextures();
				//            f.IsOffsetMapContent = loadOffsetMap;
				//            floorList.Add(f);
				//        }
				//    }
				//}
			}
			else
			//for (int x = 0; x <= Size.width - 1; x++)
			{
				//    for (int z = 0; z <= Size.height - 1; z++)
				//    {
				//        for (int i = 0; i <= floorList.Count; i++)
				//        {
				//            if (i < floorList.Count)
				//            {
				//                Entity.Entity floor = floorList[i];
				//                if (floor.Position.x == Position.x + x & floor.Position.y == Position.y & floor.Position.z == Position.z + z)
				//                {
				//                    floorList.RemoveAt(i);
				//                    i -= 1;
				//                }
				//            }
				//        }
				//    }
			}
		}

		//private void AddEntity(Dictionary<string, object> Tags, Size Size, int SizeY, bool Fill, Vector3 Steps)
		//{
		//	string EntityID = System.Convert.ToString(GetTag(Tags, "EntityID"));

		//	int ID = -1;
		//	if (TagExists(Tags, "ID"))
		//		ID = System.Convert.ToInt32(GetTag(Tags, "ID"));

		//	List<float> PosList = (List<float>)GetTag(Tags, "Position");
		//	Vector3 Position = new Vector3(PosList[0] + Offset.x, PosList[1] + Offset.y, PosList[2] + Offset.z);

		//	List<Vector4> TexList = (List<Vector4>)GetTag(Tags, "Textures");
		//	List<Texture2D> TextureList = new List<Texture2D>();
		//	string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
		//	foreach (Vector4 TextureRectangle in TexList)
		//		TextureList.Add(TextureManager.GetTexture(TexturePath, TextureRectangle));
		//	Texture2D[] TextureArray = TextureList.ToArray();

		//	List<int> TextureIndexList = (List<int>)GetTag(Tags, "TextureIndex");
		//	int[] TextureIndex = TextureIndexList.ToArray();

		//	List<float> ScaleList;
		//	Vector3 Scale = new Vector3(1,1,1);
		//	if (TagExists(Tags, "Scale"))
		//	{
		//		ScaleList = (List<float>)GetTag(Tags, "Scale");
		//		Scale = new Vector3(ScaleList[0], ScaleList[1], ScaleList[2]);
		//	}

		//	bool Collision = System.Convert.ToBoolean(GetTag(Tags, "Collision"));

		//	int ModelID = System.Convert.ToInt32(GetTag(Tags, "ModelID"));

		//	int ActionValue = System.Convert.ToInt32(GetTag(Tags, "Action"));

		//	string AdditionalValue = "";
		//	if (TagExists(Tags, "AdditionalValue"))
		//		AdditionalValue = System.Convert.ToString(GetTag(Tags, "AdditionalValue"));

		//	List<List<int>> AnimationData = null;
		//	if (TagExists(Tags, "AnimationData"))
		//		AnimationData = (List<List<int>>)GetTag(Tags, "AnimationData");

		//	Vector3 Rotation = Entity.Entity.GetRotationFromInteger(System.Convert.ToInt32(GetTag(Tags, "Rotation")));

		//	bool Visible = true;
		//	if (TagExists(Tags, "Visible"))
		//		Visible = System.Convert.ToBoolean(GetTag(Tags, "Visible"));

		//	Vector3 Shader = new Vector3(1.0f,1,1);
		//	if (TagExists(Tags, "Shader"))
		//	{
		//		List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
		//		Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
		//	}

		//	//Vector3 RotationXYZ = null; // TODO Change to default(_) if this is not a reference type 
		//	if (TagExists(Tags, "RotationXYZ"))
		//	{
		//		List<float> rotationList = (List<float>)GetTag(Tags, "RotationXYZ");
		//		Rotation = new Vector3(rotationList[0], rotationList[1], rotationList[2]);
		//	}

		//	string SeasonTexture = "";
		//	if (TagExists(Tags, "SeasonTexture"))
		//		SeasonTexture = System.Convert.ToString(GetTag(Tags, "SeasonTexture"));

		//	string SeasonToggle = "";
		//	if (TagExists(Tags, "SeasonToggle"))
		//		SeasonToggle = System.Convert.ToString(GetTag(Tags, "SeasonToggle"));

		//	float Opacity = 1.0f;
		//	if (TagExists(Tags, "Opacity"))
		//		Opacity = System.Convert.ToSingle(GetTag(Tags, "Opacity"));

		//	float CameraDistanceDelta = 0.0f;
		//	if (TagExists(Tags, "CameraDistanceDelta"))
		//		CameraDistanceDelta = System.Convert.ToSingle(GetTag(Tags, "CameraDistanceDelta"));

		//	for (float X = 0; X <= Size.width - 1; X += Steps.x)
		//	{
		//		for (float Z = 0; Z <= Size.height - 1; Z += Steps.z)
		//		{
		//			for (float Y = 0; Y <= SizeY - 1; Y += Steps.y)
		//			{
		//				bool DoAdd = false;
		//				if (!Fill)
		//				{
		//					if (X == 0 | Z == 0 | Z == Size.height - 1 | X == Size.width - 1)
		//						DoAdd = true;
		//				}
		//				else
		//					DoAdd = true;

		//				if (SeasonToggle != "")
		//				{
		//					if (!SeasonToggle.Contains(","))
		//					{
		//						if (SeasonToggle.ToLower() != World.CurrentSeason.ToString().ToLower())
		//							DoAdd = false;
		//					}
		//					else
		//					{
		//						string[] seasons = SeasonToggle.ToLower().Split(System.Convert.ToChar(","));
		//						if (!seasons.Contains(World.CurrentSeason.ToString().ToLower()))
		//							DoAdd = false;
		//					}
		//				}

		//				if (AnimationData != null && AnimationData.Count == 5)
		//				{
		//				}
		//				if (DoAdd)
		//				{
		//					Entity.Entity newEnt = Entity.Entity.GetNewEntity(EntityID, new Vector3(Position.x + X, Position.y + Y, Position.z + Z), TextureArray, TextureIndex, Collision, Rotation, Scale, UnityEngine.Mesh.getModelbyID(ModelID), ActionValue, AdditionalValue, Visible, Shader, ID, MapOrigin, SeasonTexture, Offset, null, Opacity, AnimationData, CameraDistanceDelta);
		//					newEnt.IsOffsetMapContent = loadOffsetMap;

		//					if (newEnt != null)
		//					{
		//						if (!loadOffsetMap)
		//							Game.Level.Entities.Add(newEnt);
		//						else
		//							Game.Level.OffsetmapEntities.Add(newEnt);
		//					}
		//				}
		//			}
		//		}
		//	}
		//}

		private void SetupLevel(Dictionary<string, object> Tags)
		{
			string Name = System.Convert.ToString(GetTag(Tags, "Name"));
			string MusicLoop = System.Convert.ToString(GetTag(Tags, "MusicLoop"));

			if (TagExists(Tags, "WildPokemon"))
				Game.Level.WildPokemonFloor = System.Convert.ToBoolean(GetTag(Tags, "WildPokemon"));
			else
				Game.Level.WildPokemonFloor = false;

			if (TagExists(Tags, "OverworldPokemon"))
				Game.Level.ShowOverworldPokemon = System.Convert.ToBoolean(GetTag(Tags, "OverworldPokemon"));
			else
				Game.Level.ShowOverworldPokemon = true;

			if (TagExists(Tags, "CurrentRegion"))
				Game.Level.CurrentRegion = System.Convert.ToString(GetTag(Tags, "CurrentRegion"));
			else
				Game.Level.CurrentRegion = "Johto";

			if (TagExists(Tags, "HiddenAbility"))
				Game.Level.HiddenAbilityChance = System.Convert.ToInt32(GetTag(Tags, "HiddenAbility"));
			else
				Game.Level.HiddenAbilityChance = 0;
			Game.Level.MapName = Name;
			Game.Level.MusicLoop = MusicLoop;
		}

		public static string MapScript = "";

		private void SetupActions(Dictionary<string, object> Tags)
		{
			if (TagExists(Tags, "CanTeleport"))
				Game.Level.CanTeleport = System.Convert.ToBoolean(GetTag(Tags, "CanTeleport"));
			else
				Game.Level.CanTeleport = false;

			if (TagExists(Tags, "CanDig"))
				Game.Level.CanDig = System.Convert.ToBoolean(GetTag(Tags, "CanDig"));
			else
				Game.Level.CanDig = false;

			if (TagExists(Tags, "CanFly"))
				Game.Level.CanFly = System.Convert.ToBoolean(GetTag(Tags, "CanFly"));
			else
				Game.Level.CanFly = false;

			if (TagExists(Tags, "RideType"))
				Game.Level.RideType = System.Convert.ToInt32(GetTag(Tags, "RideType"));
			else
				Game.Level.RideType = 0;

			if (TagExists(Tags, "EnviromentType"))
				Game.Level.EnvironmentType = System.Convert.ToInt32(GetTag(Tags, "EnviromentType"));
			else
				Game.Level.EnvironmentType = 0;

			if (TagExists(Tags, "Weather"))
				Game.Level.WeatherType = System.Convert.ToInt32(GetTag(Tags, "Weather"));
			else
				Game.Level.WeatherType = 0;

			// It's not my fault, I swear. The keyboard was slippy, I was partly sick, and there was fog on the road and I couldn't see.
			bool lightningExists = TagExists(Tags, "Lightning");
			bool lightingExists = TagExists(Tags, "Lighting");

			if (lightningExists & lightingExists)
				Game.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lighting"));
			else if (lightingExists)
				Game.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lighting"));
			else if (lightningExists)
				Game.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lightning"));
			else
				Game.Level.LightingType = 1;

			if (TagExists(Tags, "IsDark"))
				Game.Level.IsDark = System.Convert.ToBoolean(GetTag(Tags, "IsDark"));
			else
				Game.Level.IsDark = false;

			//ToDo: Uncomment below?...
			//if (TagExists(Tags, "Terrain"))
			//    Game.Level.Terrain.TerrainType = (Terrain)(System.Convert.ToInt32(GetTag(Tags, "Terrain")));
			//else
			//    Game.Level.Terrain.TerrainType = Terrain.Plain;

			if (TagExists(Tags, "IsSafariZone"))
				Game.Level.IsSafariZone = System.Convert.ToBoolean(GetTag(Tags, "IsSafariZone"));
			else
				Game.Level.IsSafariZone = false;

			if (TagExists(Tags, "BugCatchingContest"))
			{
				Game.Level.IsBugCatchingContest = true;
				Game.Level.BugCatchingContestData = System.Convert.ToString(GetTag(Tags, "BugCatchingContest"));
			}
			else
			{
				Game.Level.IsBugCatchingContest = false;
				Game.Level.BugCatchingContestData = "";
			}

			if (TagExists(Tags, "MapScript"))
			{
				string scriptName = System.Convert.ToString(GetTag(Tags, "MapScript"));
				//if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
				//{
				//    if (((OverworldScreen)CurrentScreen).ActionScript.IsReady)
				//    {
				//        ((OverworldScreen)CurrentScreen).ActionScript.reDelay = 0.0f;
				//        ((OverworldScreen)CurrentScreen).ActionScript.StartScript(scriptName, 0);
				//    }
				//    else
				//        MapScript = scriptName;
				//}
				//else
				//    MapScript = scriptName;
			}
			else
				MapScript = "";

			if (TagExists(Tags, "RadioChannels"))
			{
				string[] channels = System.Convert.ToString(GetTag(Tags, "RadioChannels")).Split(System.Convert.ToChar(","));
				foreach (string c in channels)
					Game.Level.AllowedRadioChannels.Add(System.Convert.ToDecimal(c.Replace(".", StringHelper.DecSeparator)));
			}
			else
				Game.Level.AllowedRadioChannels.Clear();

			if (TagExists(Tags, "BattleMap"))
				Game.Level.BattleMapData = System.Convert.ToString(GetTag(Tags, "BattleMap"));
			else
				Game.Level.BattleMapData = "";

			if (TagExists(Tags, "SurfingBattleMap"))
				Game.Level.SurfingBattleMapData = System.Convert.ToString(GetTag(Tags, "SurfingBattleMap"));
			else
				Game.Level.SurfingBattleMapData = "";

			Game.Level.World = new World(Game.Level.EnvironmentType, Game.Level.WeatherType);
		}

		private void AddShader(Dictionary<string, object> Tags)
		{
			List<int> SizeList = (List<int>)GetTag(Tags, "Size");
			Vector3 Size = new Vector3(SizeList[0], 1, SizeList[1]);
			if (SizeList.Count == 3)
				Size = new Vector3(SizeList[0], SizeList[1], SizeList[2]);

			List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
			Vector3 Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);

			bool StopOnContact = System.Convert.ToBoolean(GetTag(Tags, "StopOnContact"));

			List<int> PosList = (List<int>)GetTag(Tags, "Position");
			Vector3 Position = new Vector3(PosList[0] + Offset.x, PosList[1] + Offset.y, PosList[2] + Offset.z);

			List<int> ObjectSizeList = (List<int>)GetTag(Tags, "Size");
			//Size ObjectSize = new Size(ObjectSizeList[0], ObjectSizeList[1]);

			List<int> DayTime = new List<int>();
			if (TagExists(Tags, "DayTime"))
				DayTime = (List<int>)GetTag(Tags, "DayTime");

			//if (DayTime.Contains(World.GetTime()) | DayTime.Contains(-1) | DayTime.Count == 0)
			//{
			//    Shader NewShader = new Shader(Position, Size, Shader, StopOnContact);
			//    Game.Level.Shaders.Add(NewShader);
			//}
		}

		private void AddBackdrop(Dictionary<string, object> Tags)
		{
			List<int> SizeList = (List<int>)GetTag(Tags, "Size");
			int Width = SizeList[0];
			int Height = SizeList[1];

			List<float> PosList = (List<float>)GetTag(Tags, "Position");
			Vector3 Position = new Vector3(PosList[0] + Offset.x, PosList[1] + Offset.y, PosList[2] + Offset.z);

			Vector3 Rotation = Vector3.zero;
			if (TagExists(Tags, "Rotation"))
			{
				List<float> rotationList = (List<float>)GetTag(Tags, "Rotation");
				Rotation = new Vector3(rotationList[0], rotationList[1], rotationList[2]);
			}

			string BackdropType = System.Convert.ToString(GetTag(Tags, "Type"));

			string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
			Vector4 TextureRectangle = (Vector4)GetTag(Tags, "Texture");
			Texture2D Texture = TextureManager.GetTexture(TexturePath, TextureRectangle);

			string trigger = "";
			bool isTriggered = true;

			if (TagExists(Tags, "Trigger"))
				trigger = System.Convert.ToString(GetTag(Tags, "Trigger"));
			switch (trigger.ToLower())
			{
				case "offset":
					{
						//if (Core.GameOptions.LoadOffsetMaps == 0)
						//    isTriggered = false;
						break;
					}

				case "notoffset":
					{
						//if (Core.GameOptions.LoadOffsetMaps > 0)
						//    isTriggered = false;
						break;
					}
			}

			//if (isTriggered)
			//    Game.Level.BackdropRenderer.AddBackdrop(new BackdropRenderer.Backdrop(BackdropType, Position, Rotation, Width, Height, Texture));
		}


		private void LoadBerries()
		{
			string[] Data = Game.Player.BerryData.Replace("}" + System.Environment.NewLine, "}").Split(System.Convert.ToChar("}"));
			foreach (string Berry in Data)
			{
				if (Berry.Contains("{"))
				{
					string b = Berry.Remove(0, Berry.IndexOf("{"));
					b = b.Remove(0, 1);

					List<string> BData = b.Split(System.Convert.ToChar("|")).ToList();
					string[] PData = BData[1].Split(System.Convert.ToChar(","));

					if (BData.Count == 6)
						BData.Add("0");

					if (BData[0].ToLower() == Game.Level.LevelFile.ToLower())
					{
						Entity.Entity newEnt = Entity.Entity.GetNewEntity(PokemonUnity.Entities.BerryPlant, new Vector3(System.Convert.ToSingle(PData[0]), System.Convert.ToSingle(PData[1]), System.Convert.ToSingle(PData[2])), null, // TODO Change to default(_) if this is not a reference type 
						new int[]
						{
						0,
						0
						}, true, new Vector3(0, 0, 0), new Vector3(1, 1, 1)/*, UnityEngine.Mesh.BillModel*/, 0, "", true, new Vector3(1.0f, 1, 1), -1, MapOrigin, "", Offset);
						((BerryPlant)newEnt).Initialize(System.Convert.ToInt32(BData[2]), System.Convert.ToInt32(BData[3]), System.Convert.ToString(BData[4]), BData[5], System.Convert.ToBoolean(BData[6]));

						Game.Level.Entities.Add(newEnt);
					}
				}
			}
		}
	}
}