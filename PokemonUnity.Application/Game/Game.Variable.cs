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
using PokemonEssentials.Interface;
using PokemonEssentials.Interface.Field;

namespace PokemonUnity
{
	public partial class Game
	{
		public PokemonEssentials.Interface.IGlobalMetadata Global { get; private set; }
		//public PokemonEssentials.Interface.Field.IPokemonMapFactory MapFactory { get; private set; }
		public PokemonEssentials.Interface.Field.IMapFactory MapFactory { get; private set; }
		public PokemonEssentials.Interface.Field.IMapMetadata PokemonMap { get; private set; }
		public PokemonEssentials.Interface.Field.IMapMetadata MapData { get; private set; }
		public PokemonEssentials.Interface.Screen.IPokemonSystemOption PokemonSystem { get; private set; }
		public PokemonEssentials.Interface.ITempMetadata PokemonTemp { get; set; }
		public PokemonEssentials.Interface.Field.IEncounters PokemonEncounters { get; private set; }
		public PokemonEssentials.Interface.Screen.IPokemonStorage PokemonStorage { get; private set; }
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
		//public IGameMessage GameMessage { get; set; }

		//public static PokemonUnity.UX.IFrontEnd UI { get; private set; }
		
		/// <summary>
		/// Singleton Instance of Game class to store current/active play state.
		/// </summary>
		public static PokemonEssentials.Interface.IGame GameData { get; set; }
		public PokemonEssentials.Interface.IGraphics Graphics { get; set; }
		public UX.Scene Scenes { get; private set; }
		public UX.Screen Screens { get; private set; }
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
	}
}