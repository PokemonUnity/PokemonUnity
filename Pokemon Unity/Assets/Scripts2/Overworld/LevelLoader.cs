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
        Entity.Entity,
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

        if (MULTITHREAD == true)
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

        if (loadOffsetMap == false)
        {
			//ToDo: Change Screen to Scene
            Screen.Level.LevelFile = levelPath;
.x
            GameVariables.playerTrainer.LastSavePlace = Screen.Level.LevelFile;
            GameVariables.playerTrainer.LastSavePlacePosition = Player.Temp.LastPosition.X + "," + Player.Temp.LastPosition.y.ToString().Replace(GameController.DecSeparator, ".") + "," + Player.Temp.LastPosition.z;

            Screen.Level.Entities.Clear();
            Screen.Level.Floors.Clear();
            Screen.Level.Shaders.Clear();
            Screen.Level.BackdropRenderer.Clear();

            Screen.Level.OffsetmapFloors.Clear();
            Screen.Level.OffsetmapEntities.Clear();

            Screen.Level.WildPokemonFloor = false;
            Screen.Level.WalkedSteps = 0;

            LoadedOffsetMapNames.Clear();
            LoadedOffsetMapOffsets.Clear();
            Floor.ClearFloorTemp();

            Player.Temp.MapSteps = 0;

            sessionMapsLoaded.Add(levelPath);
        }

        levelPath = GameModeManager.GetMapPath(levelPath);
		//ToDo: Logger => Debug.Log
        Logger.Debug("Loading map: " + levelPath.Remove(0, GameController.GamePath.Length));
        System.Security.FileValidation.CheckFileValid(levelPath, false, "LevelLoader.vb");

        if (System.IO.File.Exists(levelPath) == false)
        {
            Logger.Log(Logger.LogTypes.ErrorMessage, "LevelLoader.vb: Error accessing map file \"" + levelPath + "\". File not found.");
            Busy -= 1;

            if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen & loadOffsetMap == false)
                (OverworldScreen)CurrentScreen.Titles.Add(new OverworldScreen.Title("Couldn't find map file!", 20.0F, Color.White, 6.0F, Vector2.Zero, true));

            return;
        }

        List<string> Data = System.IO.File.ReadAllLines(levelPath).ToList();
        Dictionary<string, object> Tags = new Dictionary<string, object>();

        this.Offset = offset;

        foreach (string line in Data)
        {
            if (line.Contains("{") == true)
            {
                string l = line.Remove(0, line.IndexOf("{"));

                if (l.StartsWith("{\"Comment\"{COM") == true)
                {
                    l = l.Remove(0, l.IndexOf("[") + 1);
                    l = l.Remove(l.IndexOf("]"));

                    Logger.Log(Logger.LogTypes.Debug, l);
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
            if (line.Contains("{") == true & line.Contains("}") == true)
            {
                try
                {
                    TagTypes TagType = TagTypes.None;
                    line = line.Remove(0, line.IndexOf("{") + 2);

                    if(line.ToLower().StartsWith("structure\""))
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
                    Logger.Log(Logger.LogTypes.Warning, "LevelLoader.vb: Failed to load map object! (Index: " + countLines + ") from mapfile: " + levelPath + "; Error message: " + ex.Message);
                }
            }
        }

        foreach (string line in Data)
        {
            Tags.Clear();
            string orgLine = line;
            countLines += 1;

            if (line.Contains("{") == true & line.Contains("}") == true)
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
                                    AddEntity(Tags, new Size(1, 1), 1, true, new Vector3(1, 1, 1));
                                    break;
                                }
                            case TagTypes.Floor:
                                {
                                    AddFloor(Tags);
                                    break;
                                }
                            case TagTypes.Level:
                                {
                                    if (loadOffsetMap == false)
                                        SetupLevel(Tags);
                                    break;
                                }
                            case TagTypes.LevelActions:
                                {
                                    if (loadOffsetMap == false)
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
                                    if (loadOffsetMap == false)
                                        AddShader(Tags);
                                    break;
                                }
                            case TagTypes.OffsetMap:
                                {
                                    if (loadOffsetMap == false | offsetMapLevel <= Core.GameOptions.MaxOffsetLevel)
                                        AddOffsetMap(Tags);
                                    break;
                                }
                            case TagTypes.Backdrop:
                                {
                                    if (loadOffsetMap == false)
                                        AddBackdrop(Tags);
                                    break;
                                }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log(Logger.LogTypes.Warning, "LevelLoader.vb: Failed to load map object! (Index: " + countLines + ") (Line: " + orgLine + ") from mapfile: " + levelPath + "; Error message: " + ex.Message);
                }
            }
        }

        if (loadOffsetMap == false)
            LoadBerries();

        foreach (Shader s in Screen.Level.Shaders)
        {
            if (s.HasBeenApplied == false)
            {
                s.ApplyShader(Screen.Level.Entities.ToArray());
                s.ApplyShader(Screen.Level.Floors.ToArray());
            }
        }

        Logger.Debug("Map loading finished: " + levelPath.Remove(0, GameController.GamePath.Length));
        Logger.Debug("Loaded textures: " + TextureManager.TextureList.Count.ToString());
        timer.Stop();
        Logger.Debug("Map loading time: " + timer.ElapsedTicks + " Ticks; " + timer.ElapsedMilliseconds + " Milliseconds.");

        // Dim xmlLevelLoader As New XmlLevelLoader.
        // xmlLevelLoader.Load(My.Computer.FileSystem.SpecialDirectories.Desktop & "\t.xml", _5DHero.XmlLevelLoader.LevelTypes.Default, Vector3.Zero)

        Busy -= 1;

        if (Busy == 0)
            Screen.Level.StartOffsetMapUpdate();
    }

    private Dictionary<string, object> GetTags(string line)
    {
        Dictionary<string, object> Tags = new Dictionary<string, object>();

        var tagList = line.Split(new[] { "}{" }, StringSplitOptions.RemoveEmptyEntries);
        for (var i = 0; i <= tagList.Length - 1; i++)
        {
            string currentTag = tagList[i];
            if (currentTag.EndsWith("}}") == false)
                currentTag += "}";
            if (currentTag.StartsWith("{") == false)
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
                            subTagValue = subTagValue.Replace(".", GameController.DecSeparator);
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
                            Dictionary.Add(TagName, new Rectangle(System.Convert.ToInt32(content[0]), System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3])));
                            break;
                        }
                    case "recarr":
                        {
                            string[] values = subTagValue.Split(System.Convert.ToChar("]"));
                            List<Rectangle> arr = new List<Rectangle>();
                            foreach (string value in values)
                            {
                                if (value.Length > 0)
                                {
                                    string v = value.Remove(0, 1);

                                    string[] content = v.Split(System.Convert.ToChar(","));
                                    arr.Add(new Rectangle(System.Convert.ToInt32(content[0]), System.Convert.ToInt32(content[1]), System.Convert.ToInt32(content[2]), System.Convert.ToInt32(content[3])));
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
                                string v = value.Replace(".", GameController.DecSeparator);
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
        if (Tags.ContainsKey(TagName) == true)
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
        if (Tags.ContainsKey(TagName) == true)
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
        if (Core.GameOptions.LoadOffsetMaps > 0)
        {
            List<int> OffsetList = (List<int>)GetTag(Tags, "Offset");
            Vector3 MapOffset = new Vector3(OffsetList[0], 0, OffsetList[1]);
            if (OffsetList.Count >= 3)
                MapOffset = new Vector3(OffsetList[0], OffsetList[1], OffsetList[2]);

            string MapName = System.Convert.ToString(GetTag(Tags, "Map"));

            if (loadOffsetMap == true)
            {
                if (sessionMapsLoaded.Contains(MapName) == true)
                    return;
            }
            sessionMapsLoaded.Add(MapName);

            LoadedOffsetMapNames.Add(MapName);
            LoadedOffsetMapOffsets.Add(MapOffset);

            string listName = Screen.Level.LevelFile + "|" + MapName + "|" + Screen.Level.World.CurrentMapWeather + "|" + World.GetCurrentRegionWeather() + "|" + World.GetTime() + "|" + World.CurrentSeason();
            if (OffsetMaps.ContainsKey(listName) == false)
            {
                List<List<Entity.Entity>> mapList = new List<List<Entity.Entity>>();

                List<object> @params = new List<object>();
                @params.AddRange(new
                {
                    MapName,
                    true,
                    MapOffset + Offset,
                    offsetMapLevel + 1,
                    sessionMapsLoaded
                });

                int offsetEntityCount = Screen.Level.OffsetmapEntities.Count;
                int offsetFloorCount = Screen.Level.OffsetmapFloors.Count;

                LevelLoader levelLoader = new LevelLoader();
                levelLoader.LoadLevel(@params.ToArray());
                List<Entity.Entity> entList = new List<Entity.Entity>();
                List<Entity.Entity> floorList = new List<Entity.Entity>();

                for (int i = offsetEntityCount; i <= Screen.Level.OffsetmapEntities.Count - 1; i++)
                    entList.Add(Screen.Level.OffsetmapEntities(i));
                for (int i = offsetFloorCount; i <= Screen.Level.OffsetmapFloors.Count - 1; i++)
                    floorList.Add(Screen.Level.OffsetmapFloors(i));
                mapList.AddRange(
                {
                    entList,
                    floorList
                });

                OffsetMaps.Add(listName, mapList);
            }
            else
            {
                Logger.Debug("Loaded Offsetmap from store: " + MapName);

                foreach (Entity e in OffsetMaps(listName)(0))
                {
                    if (e.MapOrigin == MapName)
                    {
                        e.IsOffsetMapContent = true;
                        Screen.Level.OffsetmapEntities.Add(e);
                    }
                }
                foreach (Entity e in OffsetMaps(listName)(1))
                {
                    if (e.MapOrigin == MapName)
                    {
                        e.IsOffsetMapContent = true;
                        Screen.Level.OffsetmapFloors.Add(e);
                    }
                }
            }
            Logger.Debug("Offset maps in store: " + OffsetMaps.Count);

            Screen.Level.OffsetmapEntities = (from e in Screen.Level.OffsetmapEntities
                                              orderby e.CameraDistance descending
                                              select e).ToList();

            foreach (Entity Entity.Entity in Screen.Level.OffsetmapEntities)
                Entity.Entity.UpdateEntity();
            foreach (Entity Floor in Screen.Level.OffsetmapFloors)
                Floor.UpdateEntity();
        }
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
        if (TagExists(Tags, "Rotation") == true)
            MapRotation = System.Convert.ToInt32(GetTag(Tags, "Rotation"));

        string MapName = System.Convert.ToString(GetTag(Tags, "Map"));
        if (MapName.EndsWith(".dat") == false)
            MapName = MapName + ".dat";

        bool addNPC = false;
        if (TagExists(Tags, "AddNPC") == true)
            addNPC = System.Convert.ToB.xlean(GetTag(Tags, "AddNPC"));

        string structureKey = MapOffset.X.ToString() + "|" + MapOffset.y.ToString() + "|" + MapOffset.z.ToString() + "|" + MapName;

        if (tempStructureList.ContainsKey(structureKey) == false)
        {
            string filepath = GameModeManager.GetMapPath(MapName);
            //System.Security.FileValidation.CheckFileValid(filepath, false, "LevelLoader.vb/StructureSpawner");

            if (System.IO.File.Exists(filepath) == false)
            {
                Logger.Log(Logger.LogTypes.ErrorMessage, "LevelLoader.vb: Error loading structure from \"" + filepath + "\". File not found.");

                return new string[] { };
            }

            string[] MapContent = System.IO.File.ReadAllLines(filepath);
            List<string> structureList = new List<string>();

            foreach (string line in MapContent)
            {
                if (line.EndsWith("}") == true)
                {
                    bool addLine = false;
					if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Entity\"{ENT["))
						addLine = true;
					if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Floor\"{ENT["))
						addLine = true;
					if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"EntityField\"{ENT["))
						addLine = true;
					if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"NPC\"{NPC["))
						if (addNPC == true)
							addLine = true;
					if (line.Trim(' ', StringHelper.Tab).StartsWith("{\"Shader\"{SHA["))
						addLine = true;

                    if (addLine == true)
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

        if (line.ToLower().Contains("{\"rotation\"{int[") == true)
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

        if (line.ToLower().Contains("{\"position\"{sngarr[") == true)
            replaceString = "{\"position\"{sngarr[";
        else if (line.ToLower().Contains("{\"position\"{intarr[") == true)
            replaceString = "{\"position\"{intarr[";

        if (replaceString != "")
        {
            string positionString = line.Remove(0, line.ToLower().IndexOf(replaceString));
            positionString = positionString.Remove(positionString.IndexOf("]}}") + 3);

            string positionData = positionString.Remove(0, positionString.IndexOf("[") + 1);
            positionData = positionData.Remove(positionData.IndexOf("]"));
.x
            string[] posArr = positionData.Split(System.Convert.ToChar(","));
            Vector3 newPosition = new Vector3(ScriptConversion.ToSingle(posArr[0].Replace(".", GameController.DecSeparator)) + MapOffset.X, ScriptConversion.ToSingle(posArr[1].Replace(".", GameController.DecSeparator)) + MapOffset.y, System.Convert.ToSingle(posArr[2].Replace(".", GameController.DecSeparator)) + MapOffset.z);
.x
            if (line.ToLower().Contains("{\"position\"{sngarr[") == true)
                line = line.Replace(positionString, "{\"position\"{sngarr[" + newPosition.X.ToString().Replace(G.xeController.DecSeparator, ".") + "," + newPosition.y.ToString().Replace(GameController.DecSeparator, ".") + "," + newPosition.z.ToString().Replace(GameController.DecSeparator, ".") + "]}}");
            else
                line = line.Replace(positionString, "{\"position\"{intarr[" + System.Convert.ToInt32(newPosition.X).ToString().Replace(GameController.DecSeparator, ".") + "," + System.Convert.ToInt32(newPosition.y).ToString().Replace(GameController.DecSeparator, ".") + "," + System.Convert.ToInt32(newPosition.z).ToString().Replace(GameController.DecSeparator, ".") + "]}}");
        }

        return line;
    }

    private void EntityField(Dictionary<string, object> Tags)
    {
        List<int> SizeList = (List<int>)GetTag(Tags, "Size");
        bool Fill = true;
        if (TagExists(Tags, "Fill") == true)
            Fill = System.Convert.ToBoolean(GetTag(Tags, "Fill"));
        Vector3 Steps = new Vector3(1, 1, 1);
        if (TagExists(Tags, "Steps") == true)
        {
            List<float> StepList = (List<float>)GetTag(Tags, "Steps");
            if (StepList.Count == 3)
                Steps = new Vector3(StepList[0], StepList[1], StepList[2]);
            else
                Steps = new Vector3(StepList[0], 1, StepList[1]);
        }

        if (SizeList.Count == 3)
            AddEntity(Tags, new Size(SizeList[0], SizeList[2]), SizeList[1], Fill, Steps);
        else
            AddEntity(Tags, new Size(SizeList[0], SizeList[1]), 1, Fill, Steps);
    }

    private void AddNPC(Dictionary<string, object> Tags)
    {.x
        List<float> PosList = (List<float>)GetTag(Tags, "Position");
        Vector3 Position = new Vector3(PosList[0] + Offset.X, PosList[1] + Offset.y, PosList[2] + Offset.z);

        List<float> ScaleList;
        Vector3 Scale = new Vector3(1);
        if (TagExists(Tags, "Scale") == true)
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
        List<Rectangle> MoveRectangles = (List<Rectangle>)GetTag(Tags, "MoveRectangles");

        Vector3 Shader = new Vector3(1.0F);
        if (TagExists(Tags, "Shader") == true)
        {
            List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
            Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
        }

        bool AnimateIdle = false;
        if (TagExists(Tags, "AnimateIdle") == true)
            AnimateIdle = System.Convert.ToBoolean(GetTag(Tags, "AnimateIdle"));

        NPC NPC = (NPC)Entity.GetNewEntity("NPC", Position, null, // TODO Change to default(_) if this is not a reference type 
        new int[]
        {
            0,
            0
        }, true, new Vector3(0), Scale, BaseModel.BillModel, ActionValue, AdditionalValue, true, Shader, -1, MapOrigin, "", Offset, new
        {
            TextureID,
            Rotation,
            Name,
            ID,
            AnimateIdle,
            Movement,
            MoveRectangles
        });

        if (loadOffsetMap == false)
            Screen.Level.Entities.Add(NPC);
        else
            Screen.Level.OffsetmapEntities.Add(NPC);
    }

    private void AddFloor(Dictionary<string, object> Tags)
    {
        List<int> sizeList = (List<int>)GetTag(Tags, "Size");
        Size Size = new Size(sizeList[0], sizeList[1]);
.x
        List<int> PosList = (List<int>)GetTag(Tags, "Position");
        Vector3 Position = new Vector3(PosList[0] + Offset.X, PosList[1] + Offset.y, PosList[2] + Offset.z);

        string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
        Rectangle TextureRectangle = (Rectangle)GetTag(Tags, "Texture");
        Texture2D Texture = TextureManager.GetTexture(TexturePath, TextureRectangle);

        bool Visible = true;
        if (TagExists(Tags, "Visible") == true)
            Visible = System.Convert.ToBoolean(GetTag(Tags, "Visible"));

        Vector3 Shader = new Vector3(1.0F);
        if (TagExists(Tags, "Shader") == true)
        {
            List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
            Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
        }

        bool RemoveFloor = false;
        if (TagExists(Tags, "Remove") == true)
            RemoveFloor = System.Convert.ToBoolean(GetTag(Tags, "Remove"));

        bool hasSnow = true;
        if (TagExists(Tags, "hasSnow") == true)
            hasSnow = System.Convert.ToBoolean(GetTag(Tags, "hasSnow"));

        bool hasSand = true;
        if (TagExists(Tags, "hasSand") == true)
            hasSand = System.Convert.ToBoolean(GetTag(Tags, "hasSand"));

        bool hasIce = false;
        if (TagExists(Tags, "isIce") == true)
            hasIce = System.Convert.ToBoolean(GetTag(Tags, "isIce"));

        int rotation = 0;
        if (TagExists(Tags, "Rotation") == true)
            rotation = System.Convert.ToInt32(GetTag(Tags, "Rotation"));

        string SeasonTexture = "";
        if (TagExists(Tags, "SeasonTexture") == true)
            SeasonTexture = System.Convert.ToString(GetTag(Tags, "SeasonTexture"));

        List<Entity.Entity> floorList = Screen.Level.Floors;
        if (loadOffsetMap == true)
            floorList = Screen.Level.OffsetmapFloors;

        if (RemoveFloor == false)
        {
            for (var x = 0; x <= Size.Width - 1; x++)
            {
                for (var z = 0; z <= Size.Height - 1; z++)
                {
                    bool exists = false;

                    int iZ = z;
                    int iX = x;

                    Entity.Entity Ent = null/* TODO Change to default(_) if this is not a reference type */;

                    if (loadOffsetMap == true)
                    {
                        Ent = Screen.Level.OffsetmapFloors.Find(e =>.x
                        {
                            return ((Entity)e).Position == new Vector3(Position.X + iX, Position.y, Position.z + iZ);
                        });
                    }
                    else
                        Ent = Screen.Level.Floors.Find(e =>.x
                        {
                            return ((Entity)e).Position == new Vector3(Position.X + iX, Position.y, Position.z + iZ);
                        });

                    if (Ent != null)
                    {
                        Ent.Textures = new[] { Texture };
                        Ent.Visible = Visible;
                        Ent.SeasonColorTexture = SeasonTexture;
                        Ent.LoadSeasonTextures();
                        (Floor)Ent.SetRotation(rotation);
                        (Floor)Ent.hasSnow = hasSnow;
                        (Floor)Ent.IsIce = hasIce;
                        (Floor)Ent.hasSand = hasSand;
                        exists = true;
                    }

                    if (exists == false).x
                    {
                        Floor f = new Floor(Position.X + x, Position.y, Position.z + z, TextureManager.GetTexture(TexturePath, TextureRectangle), new[] { 0, 0 }, false, rotation, new Vector3(1.0F), BaseModel.FloorModel, 0, "", Visible, Shader, hasSnow, hasIce, hasSand);
                        f.MapOrigin = MapOrigin;
                        f.SeasonColorTexture = SeasonTexture;
                        f.LoadSeasonTextures();
                        f.IsOffsetMapContent = loadOffsetMap;
                        floorList.Add(f);
                    }
                }
            }
        }
        else
            for (var x = 0; x <= Size.Width - 1; x++)
            {
                for (var z = 0; z <= Size.Height - 1; z++)
                {
                    for (var i = 0; i <= floorList.Count; i++)
                    {
                        if (i < floorList.Count)
                        {.x.x
                            Entity.Entity floor = floorList[i];
                            if (floor.Position.X == Position.X + x & floor.Position.y == Position.y & floor.Position.z == Position.z + z)
                            {
                                floorList.RemoveAt(i);
                                i -= 1;
                            }
                        }
                    }
                }
            }
    }

    private void AddEntity(Dictionary<string, object> Tags, Size Size, int SizeY, bool Fill, Vector3 Steps)
    {
        string EntityID = System.Convert.ToString(GetTag(Tags, "EntityID"));

        int ID = -1;
        if (TagExists(Tags, "ID") == true)
            ID = System.Convert.ToInt32(GetTag(Tags, "ID"));
.x
        List<float> PosList = (List<float>)GetTag(Tags, "Position");
        Vector3 Position = new Vector3(PosList[0] + Offset.X, PosList[1] + Offset.y, PosList[2] + Offset.z);

        List<Rectangle> TexList = (List<Rectangle>)GetTag(Tags, "Textures");
        List<Texture2D> TextureList = new List<Texture2D>();
        string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
        foreach (Rectangle TextureRectangle in TexList)
            TextureList.Add(TextureManager.GetTexture(TexturePath, TextureRectangle));
        Texture2D[] TextureArray = TextureList.ToArray();

        List<int> TextureIndexList = (List<int>)GetTag(Tags, "TextureIndex");
        int[] TextureIndex = TextureIndexList.ToArray();

        List<float> ScaleList;
        Vector3 Scale = new Vector3(1);
        if (TagExists(Tags, "Scale") == true)
        {
            ScaleList = (List<float>)GetTag(Tags, "Scale");
            Scale = new Vector3(ScaleList[0], ScaleList[1], ScaleList[2]);
        }

        bool Collision = System.Convert.ToBoolean(GetTag(Tags, "Collision"));

        int ModelID = System.Convert.ToInt32(GetTag(Tags, "ModelID"));

        int ActionValue = System.Convert.ToInt32(GetTag(Tags, "Action"));

        string AdditionalValue = "";
        if (TagExists(Tags, "AdditionalValue") == true)
            AdditionalValue = System.Convert.ToString(GetTag(Tags, "AdditionalValue"));

        List<List<int>> AnimationData = null;
        if (TagExists(Tags, "AnimationData") == true)
            AnimationData = (List<List<int>>)GetTag(Tags, "AnimationData");

        Vector3 Rotation = Entity.Entity.GetRotationFromInteger(System.Convert.ToInt32(GetTag(Tags, "Rotation")));

        bool Visible = true;
        if (TagExists(Tags, "Visible") == true)
            Visible = System.Convert.ToBoolean(GetTag(Tags, "Visible"));

        Vector3 Shader = new Vector3(1.0F);
        if (TagExists(Tags, "Shader") == true)
        {
            List<float> ShaderList = (List<float>)GetTag(Tags, "Shader");
            Shader = new Vector3(ShaderList[0], ShaderList[1], ShaderList[2]);
        }

		//Vector3 RotationXYZ = null; // TODO Change to default(_) if this is not a reference type 
        if (TagExists(Tags, "RotationXYZ") == true)
        {
            List<float> rotationList = (List<float>)GetTag(Tags, "RotationXYZ");
            Rotation = new Vector3(rotationList[0], rotationList[1], rotationList[2]);
        }

        string SeasonTexture = "";
        if (TagExists(Tags, "SeasonTexture") == true)
            SeasonTexture = System.Convert.ToString(GetTag(Tags, "SeasonTexture"));

        string SeasonToggle = "";
        if (TagExists(Tags, "SeasonToggle") == true)
            SeasonToggle = System.Convert.ToString(GetTag(Tags, "SeasonToggle"));

        float Opacity = 1.0F;
        if (TagExists(Tags, "Opacity") == true)
            Opacity = System.Convert.ToSingle(GetTag(Tags, "Opacity"));

        float CameraDistanceDelta = 0.0F;
        if (TagExists(Tags, "CameraDistanceDelta") == true)
            CameraDistanceDelta = System.Convert.ToSing.x(GetTag(Tags, "CameraDistanceDelta"));

        for (var X = 0; X <= Size.Width - 1; X += Steps.X)
        {
            for (var Z = 0; Z <= Size.Height - 1; Z += Steps.z)
            {
                for (var Y = 0; Y <= SizeY - 1; Y += Steps.y)
                {
                    bool DoAdd = false;
                    if (Fill == false)
                    {
                        if (X == 0 | Z == 0 | Z == Size.Height - 1 | X == Size.Width - 1)
                            DoAdd = true;
                    }
                    else
                        DoAdd = true;

                    if (SeasonToggle != "")
                    {
                        if (SeasonToggle.Contains(",") == false)
                        {
                            if (SeasonToggle.ToLower() != World.CurrentSeason.ToString().ToLower())
                                DoAdd = false;
                        }
                        else
                        {
                            string[] seasons = SeasonToggle.ToLower().Split(System.Convert.ToChar(","));
                            if (seasons.Contains(World.CurrentSeason.ToString().ToLower()) == false)
                                DoAdd = false;
                        }
                    }

                    if (AnimationData != null && AnimationData.Count == 5)
                    {
                    }
                    if (DoAdd == true).x
                    {
                        Entity.Entity newEnt = Entity.Entity.GetNewEntity(EntityID, new Vector3(Position.X + X, Position.y + Y, Position.z + Z), TextureArray, TextureIndex, Collision, Rotation, Scale, BaseModel.getModelbyID(ModelID), ActionValue, AdditionalValue, Visible, Shader, ID, MapOrigin, SeasonTexture, Offset, null, Opacity, AnimationData, CameraDistanceDelta);
                        newEnt.IsOffsetMapContent = loadOffsetMap;

                        if (newEnt != null)
                        {
                            if (loadOffsetMap == false)
                                Screen.Level.Entities.Add(newEnt);
                            else
                                Screen.Level.OffsetmapEntities.Add(newEnt);
                        }
                    }
                }
            }
        }
    }

    private void SetupLevel(Dictionary<string, object> Tags)
    {
        string Name = System.Convert.ToString(GetTag(Tags, "Name"));
        string MusicLoop = System.Convert.ToString(GetTag(Tags, "MusicLoop"));

        if (TagExists(Tags, "WildPokemon") == true)
            Screen.Level.WildPokemonFloor = System.Convert.ToBoolean(GetTag(Tags, "WildPokemon"));
        else
            Screen.Level.WildPokemonFloor = false;

        if (TagExists(Tags, "OverworldPokemon") == true)
            Screen.Level.ShowOverworldPokemon = System.Convert.ToBoolean(GetTag(Tags, "OverworldPokemon"));
        else
            Screen.Level.ShowOverworldPokemon = true;

        if (TagExists(Tags, "CurrentRegion") == true)
            Screen.Level.CurrentRegion = System.Convert.ToString(GetTag(Tags, "CurrentRegion"));
        else
            Screen.Level.CurrentRegion = "Johto";

        if (TagExists(Tags, "HiddenAbility"))
            Screen.Level.HiddenAbilityChance = System.Convert.ToInt32(GetTag(Tags, "HiddenAbility"));
        else
            Screen.Level.HiddenAbilityChance = 0;
        Screen.Level.MapName = Name;
        Screen.Level.MusicLoop = MusicLoop;
    }

    public static string MapScript = "";

    private void SetupActions(Dictionary<string, object> Tags)
    {
        if (TagExists(Tags, "CanTeleport") == true)
            Screen.Level.CanTeleport = System.Convert.ToBoolean(GetTag(Tags, "CanTeleport"));
        else
            Screen.Level.CanTeleport = false;

        if (TagExists(Tags, "CanDig") == true)
            Screen.Level.CanDig = System.Convert.ToBoolean(GetTag(Tags, "CanDig"));
        else
            Screen.Level.CanDig = false;

        if (TagExists(Tags, "CanFly") == true)
            Screen.Level.CanFly = System.Convert.ToBoolean(GetTag(Tags, "CanFly"));
        else
            Screen.Level.CanFly = false;

        if (TagExists(Tags, "RideType") == true)
            Screen.Level.RideType = System.Convert.ToInt32(GetTag(Tags, "RideType"));
        else
            Screen.Level.RideType = 0;

        if (TagExists(Tags, "EnviromentType") == true)
            Screen.Level.EnvironmentType = System.Convert.ToInt32(GetTag(Tags, "EnviromentType"));
        else
            Screen.Level.EnvironmentType = 0;

        if (TagExists(Tags, "Weather") == true)
            Screen.Level.WeatherType = System.Convert.ToInt32(GetTag(Tags, "Weather"));
        else
            Screen.Level.WeatherType = 0;

        // It's not my fault, I swear. The keyboard was slippy, I was partly sick, and there was fog on the road and I couldn't see.
        bool lightningExists = TagExists(Tags, "Lightning");
        bool lightingExists = TagExists(Tags, "Lighting");

        if (lightningExists == true & lightingExists == true)
            Screen.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lighting"));
        else if (lightingExists == true)
            Screen.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lighting"));
        else if (lightningExists == true)
            Screen.Level.LightingType = System.Convert.ToInt32(GetTag(Tags, "Lightning"));
        else
            Screen.Level.LightingType = 1;

        if (TagExists(Tags, "IsDark") == true)
            Screen.Level.IsDark = System.Convert.ToBoolean(GetTag(Tags, "IsDark"));
        else
            Screen.Level.IsDark = false;

        if (TagExists(Tags, "Terrain") == true)
            Screen.Level.Terrain.TerrainType = Terrain.FromString(System.Convert.ToString(GetTag(Tags, "Terrain")));
        else
            Screen.Level.Terrain.TerrainType = Terrain.TerrainTypes.Plain;

        if (TagExists(Tags, "IsSafariZone") == true)
            Screen.Level.IsSafariZone = System.Convert.ToBoolean(GetTag(Tags, "IsSafariZone"));
        else
            Screen.Level.IsSafariZone = false;

        if (TagExists(Tags, "BugCatchingContest") == true)
        {
            Screen.Level.IsBugCatchingContest = true;
            Screen.Level.BugCatchingContestData = System.Convert.ToString(GetTag(Tags, "BugCatchingContest"));
        }
        else
        {
            Screen.Level.IsBugCatchingContest = false;
            Screen.Level.BugCatchingContestData = "";
        }

        if (TagExists(Tags, "MapScript") == true)
        {
            string scriptName = System.Convert.ToString(GetTag(Tags, "MapScript"));
            if (CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
            {
                if ((OverworldScreen)CurrentScreen.ActionScript.IsReady == true)
                {
                    (OverworldScreen)CurrentScreen.ActionScript.reDelay = 0.0F;
                    (OverworldScreen)CurrentScreen.ActionScript.StartScript(scriptName, 0);
                }
                else
                    MapScript = scriptName;
            }
            else
                MapScript = scriptName;
        }
        else
            MapScript = "";

        if (TagExists(Tags, "RadioChannels") == true)
        {
            string[] channels = System.Convert.ToString(GetTag(Tags, "RadioChannels")).Split(System.Convert.ToChar(","));
            foreach (string c in channels)
                Screen.Level.AllowedRadioChannels.Add(System.Convert.ToDecimal(c.Replace(".", GameController.DecSeparator)));
        }
        else
            Screen.Level.AllowedRadioChannels.Clear();

        if (TagExists(Tags, "BattleMap") == true)
            Screen.Level.BattleMapData = System.Convert.ToString(GetTag(Tags, "BattleMap"));
        else
            Screen.Level.BattleMapData = "";

        if (TagExists(Tags, "SurfingBattleMap") == true)
            Screen.Level.SurfingBattleMapData = System.Convert.ToString(GetTag(Tags, "SurfingBattleMap"));
        else
            Screen.Level.SurfingBattleMapData = "";

        Screen.Level.World = new World(Screen.Level.EnvironmentType, Screen.Level.WeatherType);
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
.x
        List<int> PosList = (List<int>)GetTag(Tags, "Position");
        Vector3 Position = new Vector3(PosList[0] + Offset.X, PosList[1] + Offset.y, PosList[2] + Offset.z);

        List<int> ObjectSizeList = (List<int>)GetTag(Tags, "Size");
        Size ObjectSize = new Size(ObjectSizeList[0], ObjectSizeList[1]);

        List<int> DayTime = new List<int>();
        if (TagExists(Tags, "DayTime") == true)
            DayTime = (List<int>)GetTag(Tags, "DayTime");

        if (DayTime.Contains(World.GetTime()) | DayTime.Contains(-1) | DayTime.Count == 0)
        {
            Shader NewShader = new Shader(Position, Size, Shader, StopOnContact);
            Screen.Level.Shaders.Add(NewShader);
        }
    }

    private void AddBackdrop(Dictionary<string, object> Tags)
    {
        List<int> SizeList = (List<int>)GetTag(Tags, "Size");
        int Width = SizeList[0];
        int Height = SizeList[1];
.x
        List<float> PosList = (List<float>)GetTag(Tags, "Position");
        Vector3 Position = new Vector3(PosList[0] + Offset.X, PosList[1] + Offset.y, PosList[2] + Offset.z);

        Vector3 Rotation = Vector3.zero;
        if (TagExists(Tags, "Rotation") == true)
        {
            List<float> rotationList = (List<float>)GetTag(Tags, "Rotation");
            Rotation = new Vector3(rotationList[0], rotationList[1], rotationList[2]);
        }

        string BackdropType = System.Convert.ToString(GetTag(Tags, "Type"));

        string TexturePath = System.Convert.ToString(GetTag(Tags, "TexturePath"));
        Rectangle TextureRectangle = (Rectangle)GetTag(Tags, "Texture");
        Texture2D Texture = TextureManager.GetTexture(TexturePath, TextureRectangle);

        string trigger = "";
        bool isTriggered = true;

        if (TagExists(Tags, "Trigger") == true)
            trigger = System.Convert.ToString(GetTag(Tags, "Trigger"));
        switch (trigger.ToLower())
        {
            case "offset":
                {
                    if (Core.GameOptions.LoadOffsetMaps == 0)
                        isTriggered = false;
                    break;
                }

            case "notoffset":
                {
                    if (Core.GameOptions.LoadOffsetMaps > 0)
                        isTriggered = false;
                    break;
                }
        }

        if (isTriggered == true)
            Screen.Level.BackdropRenderer.AddBackdrop(new BackdropRenderer.Backdrop(BackdropType, Position, Rotation, Width, Height, Texture));
    }


    private void LoadBerries()
    {
        string[] Data = GameVariables.playerTrainer.BerryData.Replace("}" + System.Environment.NewLine, "}").Split(System.Convert.ToChar("}"));
        foreach (string Berry in Data)
        {
            if (Berry.Contains("{") == true)
            {
                string b = Berry.Remove(0, Berry.IndexOf("{"));
                b = b.Remove(0, 1);

                List<string> BData = b.Split(System.Convert.ToChar("|")).ToList();
                string[] PData = BData[1].Split(System.Convert.ToChar(","));

                if (BData.Count == 6)
                    BData.Add("0");

                if (BData[0].ToLower() == Screen.Level.LevelFile.ToLower())
                {
                    Entity.Entity newEnt = Entity.Entity.GetNewEntity("BerryPlant", new Vector3(System.Convert.ToSingle(PData[0]), System.Convert.ToSingle(PData[1]), System.Convert.ToSingle(PData[2])), null, // TODO Change to default(_) if this is not a reference type 
                    new int[]
                    {
                        0,
                        0
                    }, true, new Vector3(0), new Vector3(1), BaseModel.BillModel, 0, "", true, new Vector3(1.0F), -1, MapOrigin, "", Offset);
                    ((BerryPlant)newEnt).Initialize(System.Convert.ToInt32(BData[2]), System.Convert.ToInt32(BData[3]), System.Convert.ToString(BData[4]), BData[5], System.Convert.ToBoolean(BData[6]));

                    Screen.Level.Entities.Add(newEnt);
                }
            }
        }
    }
}
}