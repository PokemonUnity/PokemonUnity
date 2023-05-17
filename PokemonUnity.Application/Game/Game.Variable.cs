using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using PokemonUnity;
using PokemonUnity.Application;
using PokemonUnity.Character;
using PokemonUnity.Monster;
using PokemonUnity.Inventory;
using PokemonUnity.Saving;
using PokemonUnity.Overworld;

namespace PokemonUnity
{
	public partial class Game : PokemonEssentials.Interface.IGame
	{
		public PokemonEssentials.Interface.IGlobalMetadata Global { get; private set; }
		//public PokemonEssentials.Interface.Field.IPokemonMapFactory MapFactory { get; private set; }
		public PokemonEssentials.Interface.Field.IMapFactory MapFactory { get; private set; }
		public PokemonEssentials.Interface.Field.IMapMetadata PokemonMap { get; private set; }
		public PokemonEssentials.Interface.Field.IMapMetadata MapData { get; private set; }
		public PokemonEssentials.Interface.Screen.IPokemonSystemOption PokemonSystem { get; private set; }
		public PokemonEssentials.Interface.Field.ITempMetadata PokemonTemp { get; set; }
		public PokemonEssentials.Interface.Field.IEncounters PokemonEncounters { get; private set; }
		public PokemonEssentials.Interface.Screen.IPCPokemonStorage PokemonStorage { get; private set; }
		public PokemonEssentials.Interface.Screen.IBag Bag { get; private set; }
		public PokemonEssentials.Interface.ISceneMap Scene { get; set; }
		public PokemonEssentials.Interface.IGameTemp GameTemp { get; private set; }
		public PokemonEssentials.Interface.IGamePlayer Player { get; set; }
		public PokemonEssentials.Interface.PokeBattle.ITrainer Trainer { get; set; }
		public PokemonEssentials.Interface.RPGMaker.Kernal.ISystem DataSystem { get; set; }
		public PokemonEssentials.Interface.ITileset[] DataTilesets { get; set; }
		public PokemonEssentials.Interface.IGameCommonEvent[] DataCommonEvents { get; set; }
		public PokemonEssentials.Interface.IGameSystem GameSystem { get; set; }
		public PokemonEssentials.Interface.IGameSwitches GameSwitches { get; set; }
		public PokemonEssentials.Interface.IGameSelfSwitches GameSelfSwitches { get; set; }
		public PokemonEssentials.Interface.IGameVariable GameVariables { get; set; }
		public PokemonEssentials.Interface.IGameScreen GameScreen { get; set; }
		public PokemonEssentials.Interface.IGamePlayer GamePlayer { get; set; }
		public PokemonEssentials.Interface.IGameMap GameMap { get; set; }
		//public PokemonEssentials.Interface.IGameMessage GameMessage { get; set; }

		//public static PokemonUnity.UX.IFrontEnd UI { get; private set; }

		/// <summary>
		/// Singleton Instance of Game class to store current/active play state.
		/// </summary>
		public static PokemonEssentials.Interface.IGame GameData { get; set; }
		public static PokemonEssentials.Interface.IGameAudioPlay Audio { get; set; }
		public PokemonEssentials.Interface.IGraphics Graphics { get; set; }
		public PokemonEssentials.Interface.IInterpreter Interpreter { get; set; }
		public PokemonEssentials.Interface.Screen.IGameScenesUI Scenes { get; set; }
		public PokemonEssentials.Interface.Screen.IGameScreensUI Screens { get; set; }
		public Feature Features { get; private set; }
		public Challenges Challenge { get; private set; }
		//public GameModes Mode { get; private set; }

		#region Player and Overworld Data
		//public Regions Region { get; private set; }
		//public Locations Location { get; private set; }
		/// <summary>
		/// Last town or Pokemon Center visited, that's used as Respawn Point upon a Player's Defeat
		/// </summary>
		///public SeriV3 respawnScenePosition;
		///public Locations respawnCenterId { get; set; }
		public Locations Checkpoint { get; set; }
		/// <summary>
		/// </summary>
		// <see cref="Character.Player.mapName"/>
		public int Area { get; private set; }
		//ToDo: Missing Variables for RepelType, Swarm
		public int RepelSteps { get; set; } // Should not stack (encourage users to deplete excessive money); reset count based on repel used.
		//public static int RepelType { get; set; } // Maybe instead of this, use Encounter.Rate or... Different repel only changes number of steps, not potency
		public string[] Rival { get; set; }
		private byte slotIndex { get; set; }
		#endregion

		#region Private Records of Player Storage Data
		//ToDo: Honey Tree, smearing honey on tree will spawn pokemon in 6hrs, for 24hrs (21 trees)
		//Honey tree timer is done in minutes (1440, spawns at 1080), only goes down while playing...
		//ToDo: a bool variable for PC background (if texture is unlocked) `bool[]`
		public string PlayerDayCareData { get; set; } //KeyValuePair<Pokemon,steps>[]
		public string PlayerItemData { get; set; }
		public string PlayerBerryData { get; set; }
		public string PlayerNPCData { get; set; }
		public string PlayerApricornData { get; set; }
		//public Pokemon[,] PC_Poke { get; set; }
		//public string[] PC_boxNames { get; set; }
		//public int[] PC_boxTexture { get; set; }
		//public List<Items> PC_Items { get; set; }
		//public List<Items> Bag_Items { get; set; }
		//public Character.PC PC { get; private set; }
		//public Character.Bag Bag { get; private set; }
		#endregion

		#region Events used by Game Interface
		/// <summary>
		/// Fires whenever the player moves to a new map. Event handler receives the old
		/// map ID or 0 if none.  Also fires when the first map of the game is loaded
		/// </summary>
		public event EventHandler OnMapChange;
		/// <summary>
		/// Fires whenever the map scene is regenerated and soon after the player moves
		/// to a new map.
		/// </summary>
		//event EventHandler<IOnMapSceneChangeEventArgs> OnMapSceneChange;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnMapSceneChangeEventArgs> OnMapSceneChange;
		/// <summary>
		/// Fires each frame during a map update.
		/// </summary>
		public event EventHandler OnMapUpdate;
		/// <summary>
		/// Fires whenever one map is about to change to a different one. Event handler
		/// receives the new map ID and the Game_Map object representing the new map.
		/// When the event handler is called, Game.GameData.GameMap still refers to the old map.
		/// </summary>
		public event EventHandler OnMapChanging;
		/// <summary>
		/// Fires whenever the player or another event leaves a tile.
		/// </summary>
		//event EventHandler<IOnLeaveTileEventArgs> OnLeaveTile;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnLeaveTileEventArgs> OnLeaveTile;
		/// <summary>
		/// Fires whenever the player takes a step.
		/// </summary>
		public event EventHandler OnStepTaken;
		/// <summary>
		/// Fires whenever the player takes a step. The event handler may possibly move
		/// the player elsewhere.
		/// </summary>
		//event EventHandler<IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnStepTakenTransferPossibleEventArgs> OnStepTakenTransferPossible;
		/// <summary>
		/// Fires whenever the player or another event enters a tile.
		/// </summary>
		//event EventHandler<IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnStepTakenFieldMovementEventArgs> OnStepTakenFieldMovement;
		/// <summary>
		/// Triggers at the start of a wild battle.  Event handlers can provide their own
		/// wild battle routines to override the default behavior.
		/// </summary>
		//event EventHandler<IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildBattleOverrideEventArgs> OnWildBattleOverride;
		/// <summary>
		/// Triggers whenever a wild Pokémon battle ends
		/// </summary>
		//event EventHandler<IOnWildBattleEndEventArgs> OnWildBattleEnd;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildBattleEndEventArgs> OnWildBattleEnd;
		/// <summary>
		/// Triggers whenever a wild Pokémon is created
		/// </summary>
		//event EventHandler<IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnWildPokemonCreateEventArgs> OnWildPokemonCreate;
		/// <summary>
		/// Triggers whenever an NPC trainer's Pokémon party is loaded
		/// </summary>
		//event EventHandler<IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnTrainerPartyLoadEventArgs> OnTrainerPartyLoad;
		/// <summary>
		/// Fires whenever a spriteset is created.
		/// </summary>
		//event EventHandler<IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnSpritesetCreateEventArgs> OnSpritesetCreate;
		public event EventHandler OnStartBattle;
		//event EventHandler OnEndBattle;
		public event Action<object, PokemonEssentials.Interface.EventArg.IOnEndBattleEventArgs> OnEndBattle;
		/// <summary>
		/// Fires whenever a map is created. Event handler receives two parameters: the
		/// map (RPG.Map) and the tileset (RPG.Tileset)
		/// </summary>
		public event EventHandler OnMapCreate;
		/// <summary>
		/// Triggers when the player presses the Action button on the map.
		/// </summary>
		public event EventHandler OnAction;
		#endregion
	}
}