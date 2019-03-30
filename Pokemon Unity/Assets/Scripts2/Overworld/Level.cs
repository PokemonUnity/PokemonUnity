using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PokemonUnity.Overworld;
using PokemonUnity.Overworld.Entity;
using PokemonUnity.Overworld.Entity.Misc;
using PokemonUnity.Overworld.Entity.Environment;
using UnityEngine;

namespace PokemonUnity.Overworld
{
/// <summary>
/// A class that manages the collection of entities to represent a map.
/// Contains MetaData and Map Header.
/// </summary>
public class Level
{
    //private RouteSign _routeSign = null;// TODO Change to default(_) if this is not a reference type 
	private World _world = null;// TODO Change to default(_) if this is not a reference type
	private PokemonEncounter _pokemonEncounter = null;// TODO Change to default(_) if this is not a reference type 

    /// <summary>
    /// Stores warp data for warping to a new map.
    /// </summary>
    public WarpDataStruct WarpData;

    /// <summary>
    /// Stores temporary Pokémon encounter data.
    /// </summary>
    public PokemonEcounterDataStruct PokemonEncounterData;

    // Level states:
    private bool _isSurfing = false;
    private bool _isRiding = false;
    private bool _usedStrength = false;
    private bool _isDark = false;
    private int _walkedSteps = 0;

	/// <summary>
	/// Ticks until the next Offset Map update occurs.
	/// </summary>
    private int _offsetMapUpdateDelay = 50;

    // Map properties:
    private Terrain _terrain = Terrain.Plain;
    private string _mapName = "";
    private string _musicLoop = "";
    private string _levelFile = "";
    private bool _canTeleport = true;
    private bool _canDig = false;
    private bool _canFly = false;
    private int _rideType = 0;
    private int _weatherType = 0;
    private int _environmentType = 0;
    private bool _wildPokemonGrass = true;
    private bool _wildPokemonFloor = false;
    private bool _wildPokemonWater = true;
    private bool _showOverworldPokemon = true;
    private string _currentRegion = "Johto";
    private int _hiddenabilitychance = 0;
    private int _lightingType = 0;
    private bool _isSafariZone = false;
    private bool _isBugCatchingContest = false;

    private string _bugCatchingContestData = "";
    private string _battleMapData = "";

    // Entity enumerations:
    private OwnPlayer _ownPlayer;
    private OverworldPokemon _ownOverworldPokemon;

    private List<Entity.Entity> _entities = new List<Entity.Entity>();
    private List<Entity.Entity> _floors = new List<Entity.Entity>();
    private List<Shader> _shaders = new List<Shader>();
    //private BackdropRenderer _backdropRenderer;

    private List<Entity.Misc.NetworkPlayer> _networkPlayers = new List<Entity.Misc.NetworkPlayer>();
    private List<NetworkPokemon> _networkPokemon = new List<NetworkPokemon>();

    private List<Entity.Entity> _offsetMapEntities = new List<Entity.Entity>();
    private List<Entity.Entity> _offsetMapFloors = new List<Entity.Entity>();

    // Radio:
    private bool _isRadioOn = false;
	private int? _selectedRadioStation = null;// TODO Change to default(_) if this is not a reference type 
    private List<decimal> _radioChannels = new List<decimal>();

    private System.Timers.Timer _offsetTimer = new System.Timers.Timer();
    private bool _isUpdatingOffsetMaps = false;



    /// <summary>
    /// The Terrain of this level.
    /// </summary>
    public Terrain Terrain
    {
        get
        {
            return this._terrain;
        }
    }

    ///// <summary>
    ///// A RouteSign on the top left corner of the screen to display the map's name.
    ///// </summary>
    //public RouteSign RouteSign
    //{
    //    get
    //    {
    //        return this._routeSign;
    //    }
    //}

    /// <summary>
    /// Indicates whether the player is Surfing.
    /// </summary>
    public bool Surfing
    {
        get
        {
            return _isSurfing;
        }
        set
        {
            this._isSurfing = value;
        }
    }

    /// <summary>
    /// Indicates whether the player is Riding.
    /// </summary>
    public bool Riding
    {
        get
        {
            return this._isRiding;
        }
        set
        {
            this._isRiding = value;
        }
    }

    /// <summary>
    /// Indicates whether the player used Strength already.
    /// </summary>
    public bool UsedStrength
    {
        get
        {
            return this._usedStrength;
        }
        set
        {
            this._usedStrength = value;
        }
    }

    /// <summary>
    /// The reference to the active OwnPlayer instance.
    /// </summary>
    public OwnPlayer OwnPlayer
    {
        get
        {
            return this._ownPlayer;
        }
        set
        {
            this._ownPlayer = value;
        }
    }

    /// <summary>
    /// The reference to the active OverworldPokemon instance.
    /// </summary>
    public OverworldPokemon OverworldPokemon
    {
        get
        {
            return this._ownOverworldPokemon;
        }
        set
        {
            this._ownOverworldPokemon = value;
        }
    }

    /// <summary>
    /// The array of entities composing the map.
    /// </summary>
    public List<Entity.Entity> Entities
    {
        get
        {
            return this._entities;
        }
        set
        {
            this._entities = value;
        }
    }

    /// <summary>
    /// The array of floors the player can move on.
    /// </summary>
    public List<Entity.Entity> Floors
    {
        get
        {
            return this._floors;
        }
        set
        {
            this._floors = value;
        }
    }

    /// <summary>
    /// The array of shaders that add specific lighting to the map.
    /// </summary>
    public List<Shader> Shaders
    {
        get
        {
            return this._shaders;
        }
        set
        {
            this._shaders = value;
        }
    }

    /// <summary>
    /// The array of players on the server to render.
    /// </summary>
    public List<Entity.Misc.NetworkPlayer> NetworkPlayers
    {
        get
        {
            return this._networkPlayers;
        }
        set
        {
            this._networkPlayers = value;
        }
    }

    /// <summary>
    /// The array of Pokémon on the server to render.
    /// </summary>
    public List<NetworkPokemon> NetworkPokemon
    {
        get
        {
            return this._networkPokemon;
        }
        set
        {
            this._networkPokemon = value;
        }
    }

    /// <summary>
    /// The array of entities the offset maps are composed of.
    /// </summary>
    public List<Entity.Entity> OffsetmapEntities
    {
        get
        {
            return this._offsetMapEntities;
        }
        set
        {
            this._offsetMapEntities = value;
        }
    }

    /// <summary>
    /// The array of floors the offset maps are composed of.
    /// </summary>
    public List<Entity.Entity> OffsetmapFloors
    {
        get
        {
            return this._offsetMapFloors;
        }
        set
        {
            this._offsetMapFloors = value;
        }
    }

    /// <summary>
    /// The name of the current map.
    /// </summary>
    /// <remarks>This name gets displayed on the RouteSign.</remarks>
    public string MapName
    {
        get
        {
            return this._mapName;
        }
        set
        {
            this._mapName = value;
        }
    }

    /// <summary>
    /// The default background music for this level.
    /// </summary>
    /// <remarks>Doesn't play for Surfing, Riding and Radio.</remarks>
    public string MusicLoop
    {
        get
        {
            return this._musicLoop;
        }
        set
        {
            this._musicLoop = value;
        }
    }

    /// <summary>
    /// The file this level got loaded from.
    /// </summary>
    /// <remarks>The path is relative to the \maps\ or \GameMode\[gamemode]\maps\ path.</remarks>
    public string LevelFile
    {
        get
        {
            return this._levelFile;
        }
        set
        {
            this._levelFile = value;
        }
    }

    /// <summary>
    /// Whether the player can use the move Teleport.
    /// </summary>
    public bool CanTeleport
    {
        get
        {
            return this._canTeleport;
        }
        set
        {
            this._canTeleport = value;
        }
    }

    /// <summary>
    /// Whether the player can use the move Dig or an Escape Rope.
    /// </summary>
    public bool CanDig
    {
        get
        {
            return this._canDig;
        }
        set
        {
            this._canDig = value;
        }
    }

    /// <summary>
    /// Whether the player can use the move Fly.
    /// </summary>
    public bool CanFly
    {
        get
        {
            return this._canFly;
        }
        set
        {
            this._canFly = value;
        }
    }

    /// <summary>
    /// The type of Ride the player can use on this map.
    /// </summary>
    /// <remarks>0 = Depends on CanDig and CanFly, 1 = True, 2 = False</remarks>
    public int RideType
    {
        get
        {
            return this._rideType;
        }
        set
        {
            this._rideType = value;
        }
    }

    /// <summary>
    /// The Weather on this map.
    /// </summary>
    /// <remarks>For the weather, look at the WeatherTypes enumeration in World.vb</remarks>
    public int WeatherType
    {
        get
        {
            return this._weatherType;
        }
        set
        {
            this._weatherType = value;
        }
    }

    /// <summary>
    /// The environment type for this map.
    /// </summary>
    public int EnvironmentType
    {
        get
        {
            return this._environmentType;
        }
        set
        {
            this._environmentType = value;
        }
    }

    /// <summary>
    /// Whether the player can encounter wild Pokémon in the Grass entities.
    /// </summary>
    public bool WildPokemonGrass
    {
        get
        {
            return this._wildPokemonGrass;
        }
        set
        {
            _wildPokemonGrass = value;
        }
    }

    /// <summary>
    /// Whether the player can encounter wild Pokémon on every floor tile.
    /// </summary>
    public bool WildPokemonFloor
    {
        get
        {
            return this._wildPokemonFloor;
        }
        set
        {
            this._wildPokemonFloor = value;
        }
    }

    /// <summary>
    /// Whether the player can encounter wild Pokémon while Surfing.
    /// </summary>
    public bool WildPokemonWater
    {
        get
        {
            return this._wildPokemonWater;
        }
        set
        {
            this._wildPokemonWater = value;
        }
    }

    /// <summary>
    /// Whether the map is dark, and needs to be lightened up by Flash.
    /// </summary>
    public bool IsDark
    {
        get
        {
            return this._isDark;
        }
        set
        {
            this._isDark = value;
        }
    }

    /// <summary>
    /// Whether the Overworld Pokémon is visible.
    /// </summary>
    public bool ShowOverworldPokemon
    {
        get
        {
            return this._showOverworldPokemon;
        }
        set
        {
            this._showOverworldPokemon = value;
        }
    }

    /// <summary>
    /// The amount of walked steps on this map.
    /// </summary>
    public int WalkedSteps
    {
        get
        {
            return this._walkedSteps;
        }
        set
        {
            this._walkedSteps = value;
        }
    }

    /// <summary>
    /// The region this map is assigned to.
    /// </summary>
    /// <remarks>The default is "Johto".</remarks>
    public string CurrentRegion
    {
        get
        {
            return this._currentRegion;
        }
        set
        {
            this._currentRegion = value;
        }
    }

    /// <summary>
    /// Chance of a Hidden Ability being on a wild Pokémon.
    /// </summary>
    public int HiddenAbilityChance
    {
        get
        {
            return this._hiddenabilitychance;
        }
        set
        {
            this._hiddenabilitychance = value;
        }
    }

    /// <summary>
    /// The LightingType of this map. More information in the Level\UpdateLighting.
    /// </summary>
    public int LightingType
    {
        get
        {
            return this._lightingType;
        }
        set
        {
            this._lightingType = value;
        }
    }

    /// <summary>
    /// Whether the map is a part of the Safari Zone. This changes the Battle Menu and the Menu Screen.
    /// </summary>
    public bool IsSafariZone
    {
        get
        {
            return this._isSafariZone;
        }
        set
        {
            this._isSafariZone = value;
        }
    }

    /// <summary>
    /// Whether the map is a part of the Bug Catching Contest. This changes the Battle Menu and the Menu Screen.
    /// </summary>
    public bool IsBugCatchingContest
    {
        get
        {
            return this._isBugCatchingContest;
        }
        set
        {
            this._isBugCatchingContest = value;
        }
    }

    /// <summary>
    /// Holds data for the Bug Catching Contest.
    /// </summary>
    /// <remarks>Composed of 3 values, separated by ",": 0 = script location for ending the contest, 1 = script location for selecting the remaining balls item, 2 = Menu Item name for the remaining balls item.</remarks>
    public string BugCatchingContestData
    {
        get
        {
            return this._bugCatchingContestData;
        }
        set
        {
            this._bugCatchingContestData = value;
        }
    }

    /// <summary>
    /// Used to modify the Battle Map camera position.
    /// </summary>
    /// <remarks>Data: MapName,x,y,z OR Mapname OR x,y,z OR empty</remarks>
    public string BattleMapData
    {
        get
        {
            return this._battleMapData;
        }
        set
        {
            this._battleMapData = value;
        }
    }

    /// <summary>
    /// Used to modify the Battle Map.
    /// </summary>
    /// <remarks>Data: MapName,x,y,z OR Mapname OR empty</remarks>
    public string SurfingBattleMapData { get; set; }

    /// <summary>
    /// The instance of the World class, handling time, season and weather based operations.
    /// </summary>
    public World World
    {
        get
        {
            return this._world;
        }
        set
        {
            this._world = value;
        }
    }

    /// <summary>
    /// Whether the Radio is currently activated.
    /// </summary>
    public bool IsRadioOn
    {
        get
        {
            return this._isRadioOn;
        }
        set
        {
            this._isRadioOn = value;
        }
    }

    /// <summary>
    /// The currently selected Radio station. If possible, this will replace the Music Loop.
    /// </summary>
    public int? SelectedRadioStation //PokegearScreen.RadioStation
    {
        get
        {
            return this._selectedRadioStation;
        }
        set
        {
            this._selectedRadioStation = value;
        }
    }

    /// <summary>
    /// Allowed Radio channels on this map.
    /// </summary>
    public List<decimal> AllowedRadioChannels
    {
        get
        {
            return this._radioChannels;
        }
        set
        {
            this._radioChannels = value;
        }
    }

    /// <summary>
    /// Handles wild Pokémon encounters.
    /// </summary>
    public PokemonEncounter PokemonEncounter
    {
        get
        {
            return this._pokemonEncounter;
        }
    }

    ///// <summary>
    ///// The backdrop renderer of this level.
    ///// </summary>
    //public BackdropRenderer BackdropRenderer
    //{
    //    get
    //    {
    //        return _backdropRenderer;
    //    }
    //}



    /// <summary>
    /// A structure to store warp data in.
    /// </summary>
    public struct WarpDataStruct
    {
        /// <summary>
        /// The destination map file.
        /// </summary>
        public string WarpDestination;

        /// <summary>
        /// The position to warp the player to.
        /// </summary>
        public Vector3 WarpPosition;

        /// <summary>
        /// The check to see if the player should get warped next tick.
        /// </summary>
        public bool DoWarpInNextTick;

        /// <summary>
        /// Amount of 90° rotations counterclockwise.
        /// </summary>
        public int WarpRotations;

        /// <summary>
        /// The correct camera yaw to set the camera to after the warping.
        /// </summary>
        public float CorrectCameraYaw;

        /// <summary>
        /// If the warp action got triggered by a warp block.
        /// </summary>
        public bool IsWarpBlock;
    }

    /// <summary>
    /// A structure to store wild Pokémon encounter data in.
    /// </summary>
    public struct PokemonEcounterDataStruct
    {
        /// <summary>
        /// The assumed position the player will be in when encounterning the Pokémon.
        /// </summary>
        public Vector3 Position;

        /// <summary>
        /// Whether the player encountered a Pokémon.
        /// </summary>
        public bool EncounteredPokemon;

        /// <summary>
        /// The encounter method.
        /// </summary>
        public EncounterTypes Method;

        /// <summary>
        /// The link to the .poke file used to spawn the Pokémon in.
        /// </summary>
        public string PokeFile;
    }


    /// <summary>
    /// Creates a new instance of the Level class.
    /// </summary>
    public Level()
    {
        //this._routeSign = new RouteSign();
        this.WarpData = new WarpDataStruct();
        this.PokemonEncounterData = new PokemonEcounterDataStruct();
        this._pokemonEncounter = new PokemonEncounter(this);

        this.StartOffsetMapUpdate();

        //this._backdropRenderer = new BackdropRenderer();
        //this._backdropRenderer.Initialize();
    }

    /// <summary>
    /// Initializes the offset map update cycle.
    /// </summary>
    public void StartOffsetMapUpdate()
    {
        if (this._offsetTimer != null)
            this._offsetTimer.Stop();

        this._offsetTimer = new System.Timers.Timer();
        this._offsetTimer.Interval = 16;
        this._offsetTimer.AutoReset = true;
        //this._offsetTimer.Elapsed += this.UpdateOffsetMap();
        this._offsetTimer.Start();

        GameVariables.DebugLog("Started Offset map update");
    }

    public void StopOffsetMapUpdate()
    {
        this._offsetTimer.Stop();
        while (this._isUpdatingOffsetMaps)
            Thread.Sleep(1);

        GameVariables.DebugLog("Stopped Offset map update");
    }

    /// <summary>
    /// Loads a level from a levelfile.
    /// </summary>
    /// <param name="Levelpath">The path to load the level from. Start with "|" to prevent loading a levelfile.</param>
    public void Load(string Levelpath)
    {

        // copy all changed files
        //if (GameVariables.IS_DEBUG_ACTIVE)
        //    DebugFileWatcher.TriggerReload();

        // Create a parameter array to pass over to the LevelLoader:
        List<object> @params = new List<object>();
        @params.AddRange(new[]
        {
            (object)Levelpath,
            (object)false,
            (object)new Vector3(0, 0, 0),
            (object)0,
            (object)new List<string>()
        });

        // Create the world and load the level:
        World = new World(0, 0);

        if (!Levelpath.StartsWith("|"))
        {
            this.StopOffsetMapUpdate();
            LevelLoader levelLoader = new LevelLoader();
            levelLoader.LoadLevel(@params.ToArray());
        }
        else
            GameVariables.DebugLog("Don't attempt to load a levelfile.");

        // Create own player Entity and OverworldPokémon Entity and add them to the Entity enumeration:
        //OwnPlayer = new OwnPlayer(0, 0, 0, TextureManager.DefaultTexture, GameVariables.playerTrainer.Skin, 0, 0, "", "Gold", 0);
        OverworldPokemon = new OverworldPokemon(GameVariables.Camera.Position.x, GameVariables.Camera.Position.y, GameVariables.Camera.Position.z + 1);
        OverworldPokemon.ChangeRotation();
        Entities.AddRange(new Entity.Entity[]
        {
            OwnPlayer,
            OverworldPokemon
        });

        this.Surfing = GameVariables.playerTrainer.startSurfing;
        this.StartOffsetMapUpdate();
    }

    /// <summary>
    /// Renders the level.
    /// </summary>
    public void Draw()
    {
        //this._backdropRenderer.Draw();

        // Set the effect's View and Projection matrices:
        //Screen.Effect.View = GameVariables.Camera.View;
        //Screen.Effect.Projection = GameVariables.Camera.Projection;

        // Reset the Debug values:
        //DebugDisplay.DrawnVertices = 0;
        //DebugDisplay.MaxVertices = 0;
        //DebugDisplay.MaxDistance = 0;

        List<Entity.Entity> AllEntities = new List<Entity.Entity>();
        List<Entity.Entity> AllFloors = new List<Entity.Entity>();

        AllEntities.AddRange(Entities);
        AllFloors.AddRange(Floors);

        //if (Core.GameOptions.LoadOffsetMaps > 0)
        //{
        //    AllEntities.AddRange(OffsetmapEntities);
        //    AllFloors.AddRange(OffsetmapFloors);
        //}

        AllEntities = (from f in AllEntities
                       orderby f.CameraDistance descending
                       select f).ToList();
        AllFloors = (from f in AllFloors
                     orderby f.CameraDistance descending
                     select f).ToList();

        // Render floors:
        for (var i = 0; i <= AllFloors.Count - 1; i++)
        {
            if (i <= AllFloors.Count - 1)
            {
                AllFloors[i].Render();
                //DebugDisplay.MaxVertices += AllFloors[i].VertexCount;
            }
        }

        // Render all other entities:
        for (int i = 0; i <= AllEntities.Count - 1; i++)
        {
            if (i <= AllEntities.Count - 1)
            {
                AllEntities[i].Render();
                //DebugDisplay.MaxVertices += AllEntities[i].VertexCount;
            }
        }

        if (IsDark)
            DrawFlashOverlay();
    }

    /// <summary>
    /// Updates the level's logic.
    /// </summary>
    public void Update()
    {
        //this._backdropRenderer.Update();

        this.UpdatePlayerWarp();
        this._pokemonEncounter.TriggerBattle();

        // Reload map from file (Debug or Sandbox Mode):
        if (GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
        {
            //if (KeyBoardHandler.KeyPressed(Keys.R) & Core.CurrentScreen.Identification == Screen.Identifications.OverworldScreen)
            //{
            //    Core.OffsetMaps.Clear();
            //    GameVariables.DebugLog(string.Format("Reload map file: {0}", this._levelFile));
            //    this.Load(LevelFile);
            //}
        }

        // Update all network players and Pokémon:
        //if (JoinServerScreen.Online)
        //    Core.ServersManager.PlayerManager.UpdatePlayers();

        // Call Update and UpdateEntity methods of all entities:
        this.UpdateEntities();
    }

    /// <summary>
    /// Updates all entities on the map and offset map and sorts the enumarations.
    /// </summary>
    public void UpdateEntities()
    {
        // Update and remove entities:
        if (!LevelLoader.IsBusy)
        {
            for (var i = 0; i <= Entities.Count - 1; i++)
            {
                if (i <= Entities.Count - 1)
                {
                    if (Entities.Count - 1 >= i && Entities[i].CanBeRemoved)
                    {
                        Entities.RemoveAt(i);
                        i -= 1;
                    }
                    else
                    {
                        if (Entities[i].NeedsUpdate)
                            Entities[i].Update();

                        // UpdateEntity for all entities:
                        this.Entities[i].UpdateEntity();
                    }
                }
                else
                    break;
            }
        }

        // UpdateEntity for all floors:
        for (var i = 0; i <= this.Floors.Count - 1; i++)
        {
            if (i <= this.Floors.Count - 1)
                this.Floors[i].UpdateEntity();
        }

        this.SortEntities();
    }

    /// <summary>
    /// Sorts the Entity enumerations.
    /// </summary>
    public void SortEntities()
    {
        if (!LevelLoader.IsBusy)
            Entities = (from f in Entities
                        orderby f.CameraDistance descending
                        select f).ToList();
    }

    /// <summary>
    /// Sorts and updates offset map entities.
    /// </summary>
    public void UpdateOffsetMap()
    {
        this._isUpdatingOffsetMaps = true;
        //if (Core.GameOptions.LoadOffsetMaps > 0)
        //{
        //    // The Update function of entities on offset maps are not getting called.
		//
        //    if (this._offsetMapUpdateDelay <= 0)
        //    {
        //        // Sort the list:
        //        if (!LevelLoader.IsBusy)
        //            OffsetmapEntities = (from e in OffsetmapEntities
        //                                 orderby e.CameraDistance descending
        //                                 select e).ToList();
		//
        //        //this._offsetMapUpdateDelay = Core.GameOptions.LoadOffsetMaps - 1; // Set the new delay
		//
        //        // Remove entities that CanBeRemoved (see what I did there?):
        //        // Now it also updates the remaining entities.
        //        for (var i = 0; i <= OffsetmapEntities.Count - 1; i++)
        //        {
        //            if (i <= OffsetmapEntities.Count - 1)
        //            {
        //                if (OffsetmapEntities[i].CanBeRemoved)
        //                {
        //                    OffsetmapEntities.RemoveAt(i);
        //                    i -= 1;
        //                }
        //                else
        //                    OffsetmapEntities[i].UpdateEntity();
        //            }
        //            else
        //                break;
        //        }
		//
        //        // Call UpdateEntity on all offset map floors:
        //        for (int i = this.OffsetmapFloors.Count - 1; i >= 0; i += -1)
        //        {
        //            if (i <= this.OffsetmapFloors.Count - 1)
        //                this.OffsetmapFloors[i].UpdateEntity();
        //        }
        //    }
        //    else
        //        this._offsetMapUpdateDelay -= 1;
        //}
        this._isUpdatingOffsetMaps = false;
    }

    /// <summary>
    /// Renders offset map entities.
    /// </summary>
    private void RenderOffsetMap()
    {
        // Render floors:
        for (int i = 0; i <= this.OffsetmapFloors.Count - 1; i++)
        {
            if (i <= this.OffsetmapFloors.Count - 1)
            {
                if (this.OffsetmapFloors[i] != null)
                {
                    this.OffsetmapFloors[i].Render();
                    //DebugDisplay.MaxVertices += this.OffsetmapFloors[i].VertexCount;
                }
            }
        }

        // Render entities:
        for (int i = 0; i <= this.OffsetmapEntities.Count - 1; i++)
        {
            if (i <= this.OffsetmapEntities.Count - 1)
            {
                if (this.OffsetmapEntities[i] != null)
                {
                    this.OffsetmapEntities[i].Render();
                    //DebugDisplay.MaxVertices += this.OffsetmapEntities[i].VertexCount;
                }
            }
        }
    }

    /// <summary>
    /// Draws the flash overlay to the screen.
    /// </summary>
    private void DrawFlashOverlay()
    {
        //Core.SpriteBatch.Draw(TextureManager.GetTexture(@"GUI\Overworld\flash_overlay"), new Vector4(0, 0, Core.windowSize.width, Core.windowSize.height), UnityEngine.Color.white);
    }

    /// <summary>
    /// Handles warp events for the player.
    /// </summary>
    private void UpdatePlayerWarp()
    {
        if (WarpData.DoWarpInNextTick)
        {
            // Disable wild Pokémon:
            this._wildPokemonFloor = false;
            this.PokemonEncounterData.EncounteredPokemon = false;

            // Set the Surfing flag for the next map:
            GameVariables.playerTrainer.startSurfing = Surfing;

            // Change the player position:
            GameVariables.Camera.Position = WarpData.WarpPosition;

            string tempProperties = this.CanDig.ToString() + "," + this.CanFly.ToString(); // Store properties to determine if the "enter" sound should be played.

            // Store skin values:
            bool usingGameJoltTexture = OwnPlayer.UsingGameJoltTexture;
            //GameVariables.playerTrainer.Skin = OwnPlayer.SkinName;

            // Load the new level:
            List<object> @params = new List<object>();
            @params.AddRange(new[]
            {
                (object)WarpData.WarpDestination,
                (object)false,
                (object)new Vector3(0, 0, 0),
                (object)0,
                (object)new List<string>()
            });

            World = new World(0, 0);

            this.StopOffsetMapUpdate();
            LevelLoader levelLoader = new LevelLoader();
            levelLoader.LoadLevel(@params.ToArray());

            //GameVariables.playerTrainer.AddVisitedMap(this.LevelFile); // Add new map to visited maps list.
            UsedStrength = false; // Disable Strength usuage upon map switch.
            this.Surfing = GameVariables.playerTrainer.startSurfing; // Set the Surfing property after map switch.

            // Create player and Pokémon entities:
            //OwnPlayer = new OwnPlayer(0, 0, 0, TextureManager.DefaultTexture, GameVariables.playerTrainer.Skin, 0, 0, "", "Gold", 0);
            //OwnPlayer.SetTexture(GameVariables.playerTrainer.Skin, usingGameJoltTexture);

            OverworldPokemon = new OverworldPokemon(GameVariables.Camera.Position.x, GameVariables.Camera.Position.y, GameVariables.Camera.Position.z + 1);
            OverworldPokemon.Visible = false;
            OverworldPokemon.warped = true;
            Entities.AddRange(new Entity.Entity[]
            {
                OwnPlayer,
                OverworldPokemon
            });

            // Set Ride skin, if needed:
            if (Riding & !CanRide())
            {
                Riding = false;
                //OwnPlayer.SetTexture(GameVariables.playerTrainer.TempRideSkin, true);
                //GameVariables.playerTrainer.Skin = GameVariables.playerTrainer.TempRideSkin;
            }

            // If any turns after the warp are defined, apply them:
            GameVariables.Camera.InstantTurn(WarpData.WarpRotations);

            // Make the RouteSign appear:
            //this._routeSign.Setup(MapName);

            // Play the correct music track:
            //if (IsRadioOn && GameJolt.PokegearScreen.StationCanPlay(this.SelectedRadioStation))
            //    MusicManager.Play(SelectedRadioStation.Music, true);
            //else
            //{
            //    IsRadioOn = false;
            //    if (this.Surfing)
            //        MusicManager.Play("surf", true);
            //    else if (this.Riding)
            //        MusicManager.Play("ride", true);
            //    else
            //        MusicManager.Play(MusicLoop, true);
            //}

            // Initialize the world with newly loaded environment variables:
            World.Initialize(GameVariables.Level.EnvironmentType, GameVariables.Level.WeatherType);

            // If this map is on the restplaces list, set the player's last restplace to this map:
            List<string> restplaces = System.IO.File.ReadAllLines(/*GameModeManager.GetMapPath("restplaces.dat")*/"").ToList();

            foreach (string line in restplaces)
            {
                string place = line.GetSplit(0, "|");
                if (place == LevelFile)
                {
                    //GameVariables.playerTrainer.LastRestPlace = place;
                    //GameVariables.playerTrainer.LastRestPlacePosition = line.GetSplit(1, "|");
                }
            }

            // If the warp happened through a warp block, make the player walk one step forward after switching to the new map:
            if (GameVariables.Camera.IsMoving & WarpData.IsWarpBlock)
            {
                GameVariables.Camera.StopMovement();
                GameVariables.Camera.Move(1.0f);
            }

            // Because of the map change, Roaming Pokémon are moving to their next location on the world map:
            //RoamingPokemon.ShiftRoamingPokemon(-1);

            // Check if the enter sound should be played by checking if CanDig or CanFly properties are different from the last map:
            if (tempProperties != this.CanDig.ToString() + "," + this.CanFly.ToString())
                SoundManager.PlaySound("enter", false);

            // Unlock the yaw on the camera:
            //((OverworldCamera)GameVariables.Camera).YawLocked = false;
            Entity.Misc.NetworkPlayer.ScreenRegionChanged();

            // If a warp occured, update the camera:
            GameVariables.Camera.Update();

            // Disable the warp check:
            this.WarpData.DoWarpInNextTick = false;
            WarpData.IsWarpBlock = false;

            //if (Core.ServersManager.ServerConnection.Connected)
            //    // Update network players:
            //    Core.ServersManager.PlayerManager.NeedsUpdate = true;
        }
    }

    /// <summary>
    /// Returns a list of all NPCs on the map.
    /// </summary>
    public List<NPC> GetNPCs()
    {
        List<NPC> reList = new List<NPC>();

        foreach (Entity.Entity Entity in this.Entities)
        {
            if (Entity.EntityID == PokemonUnity.Entities.NPC)
                reList.Add((NPC)Entity);
        }

        return reList;
    }

    /// <summary>
    /// Returns an NPC based on their ID.
    /// </summary>
    /// <param name="ID">The ID of the NPC to return from the level.</param>
    /// <returns>Returns either a matching NPC or Nothing.</returns>
    public NPC GetNPC(int ID)
    {
        foreach (NPC NPC in GetNPCs())
        {
            if (NPC.NPCID == ID)
                return NPC;
        }

        return null;// TODO Change to default(_) if this is not a reference type 
    }

    /// <summary>
    /// Returns an NPC based on the Entity ID.
    /// </summary>
    public Entity.Entity GetEntity(int ID)
    {
        if (ID == -1)
            throw new Exception("-1 is the default value for NOT having an ID, therefore is not a valid ID.");
        else
            foreach (Entity.Entity ent in this.Entities)
            {
                if (ent.ID == ID)
                    return ent;
            }

        return null;// TODO Change to default(_) if this is not a reference type 
    }

    /// <summary>
    /// Checks all NPCs on the map for if the player is in their line of sight.
    /// </summary>
    public void CheckTrainerSights()
    {
        foreach (Entity.Entity Entity in Entities)
        {
            if (Entity.EntityID == PokemonUnity.Entities.NPC)
            {
                NPC NPC = (NPC)Entity;
                if (NPC.IsTrainer)
                    NPC.CheckInSight();
            }
        }
    }

    /// <summary>
    /// Determines whether the player can use Ride on this map.
    /// </summary>
    public bool CanRide()
    {
        if (GameVariables.IS_DEBUG_ACTIVE | GameVariables.playerTrainer.SandBoxMode)
            return true;
        if (RideType > 0)
        {
            switch (RideType)
            {
                case 1:
                    {
                        return true;
                    }
                case 2:
                    {
                        return false;
                    }
            }
        }
        if (!GameVariables.Level.CanDig & !GameVariables.Level.CanFly)
            return false;
        else
            return true;
    }

    /// <summary>
    /// Whether the player can move based on the Entity around him.
    /// </summary>
    public bool CanMove()
    {
        foreach (Entity.Entity e in this.Entities)
        {
            if (e.Position.x == GameVariables.Camera.Position.x & e.Position.z == GameVariables.Camera.Position.z & System.Convert.ToInt32(e.Position.y) == System.Convert.ToInt32(GameVariables.Camera.Position.y))
                return e.LetPlayerMove();
        }
        return true;
    }
}
}