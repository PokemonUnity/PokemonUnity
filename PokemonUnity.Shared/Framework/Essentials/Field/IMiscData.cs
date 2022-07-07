using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using PokemonUnity;
using PokemonUnity.UX;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonUnity.Overworld;
using PokemonUnity.Utility;
using PokemonEssentials.Interface.Screen;

namespace PokemonEssentials.Interface.Field
{
	public enum GlobalMetadatas { 
		MetadataHome             = 1,
		MetadataWildBattleBGM    = 2,
		MetadataTrainerBattleBGM = 3,
		MetadataWildVictoryME    = 4,
		MetadataTrainerVictoryME = 5,
		MetadataSurfBGM          = 6,
		MetadataBicycleBGM       = 7,
		MetadataPlayerA          = 8,
		MetadataPlayerB          = 9,
		MetadataPlayerC          = 10,
		MetadataPlayerD          = 11,
		MetadataPlayerE          = 12,
		MetadataPlayerF          = 13,
		MetadataPlayerG          = 14,
		MetadataPlayerH          = 15
	}
	public enum MapMetadatas {
		MetadataOutdoor             = 1,
		MetadataShowArea            = 2,
		MetadataBicycle             = 3,
		MetadataBicycleAlways       = 4,
		MetadataHealingSpot         = 5,
		MetadataWeather             = 6,
		MetadataMapPosition         = 7,
		MetadataDiveMap             = 8,
		MetadataDarkMap             = 9,
		MetadataSafariMap           = 10,
		MetadataSnapEdges           = 11,
		MetadataDungeon             = 12,
		MetadataBattleBack          = 13,
		MetadataMapWildBattleBGM    = 14,
		MetadataMapTrainerBattleBGM = 15,
		MetadataMapWildVictoryME    = 16,
		MetadataMapTrainerVictoryME = 17,
		MetadataMapSize             = 18
	}

	/// <summary>
	/// Information on the player characters in the game.
	/// </summary>
	[Serializable] public struct MetadataPlayer
	{
		public string this[int charset] { get { return ""; } }
		public string Name { get; set; }
		/// <summary>
		/// The player's trainer type <see cref="TrainerTypes"/>. 
		/// This trainer type is defined in exactly the same way as any other trainer type, 
		/// and is typically only used by this player character 
		/// (although some games may turn an unused player character into a rival instead).
		/// </summary>
		public int Type { get; set; }
		/// <summary>
		/// The character's walking charset, as found in "Graphics/Characters"
		/// </summary>
		public string Walking { get; set; }
		/// <summary>
		/// The character's cycling charset, as found in "Graphics/Characters"
		/// </summary>
		public string Cycling { get; set; }
		/// <summary>
		/// The character's surfing charset, as found in "Graphics/Characters"
		/// </summary>
		public string Surfing { get; set; }
		/// <summary>
		/// The character's running charset, as found in "Graphics/Characters"
		/// </summary>
		public string Running { get; set; }
		/// <summary>
		/// The character's diving charset, as found in "Graphics/Characters"
		/// </summary>
		public string Diving { get; set; }
		/// <summary>
		/// The character's fishing-while-standing charset, as found in "Graphics/Characters"
		/// </summary>
		public string FishingStand { get; set; }
		/// <summary>
		/// The character's fishing-while-surfing charset, as found in "Graphics/Characters"
		/// </summary>
		public string FishingSurf { get; set; }
	}
	[Serializable] public struct MetadataPosition
	{
		/// <summary>
		/// Map ID
		/// </summary>
		public int MapId { get; set; }
		/// <summary>
		/// X coordinate on that map
		/// </summary>
		public float X { get; set; }
		/// <summary>
		/// Y coordinate on that map
		/// </summary>
		public float Y { get; set; }
		/// <summary>
		/// Direction the player should face 
		/// (2=down, 4=left, 6=right, 8=up, 0=retain direction)
		/// </summary>
		public int Direction { get; set; }
		//public IQuaternion Rotation { get; set; }
		//public IVector Position { get; set; }
	}
	/// <summary>
	/// Information on the map's weather chances in the game.
	/// </summary>
	[Serializable] public struct MetadataWeather
	{
		/// <summary>
		/// The weather type
		/// </summary>
		public FieldWeathers Weather { get; set; }
		/// <summary>
		/// The probability (out of 100) of that weather occurring when the player enters this map.
		/// </summary>
		public int Chance { get; set; }
	}
	[Serializable] public struct GlobalMetadata
	{
		public int MapId					{ get; set; }
		/// <summary>
		/// The charsets mentioned should depict the "default" outfit for that player character, 
		/// i.e. they have no outfit ID number at the end of their filenames.
		/// </summary>
		/// <remarks>
		/// Typically, "PlayerA" will correspond to the male player character, and "PlayerB" the female one. 
		/// </remarks>
		public MetadataPlayer[] Players { get; set; }
		/// <summary>
		/// The point that the player is transferred to when they black out but no Poké Center has been visited yet.
		/// </summary>
		/// <remarks>
		/// The map identified by this metadata must have an event page with the "Autorun" trigger, 
		/// which depends on the Game Switch called "Starting Over" (number 1 by default). 
		/// This event page, when run, must heal all Pokémon in the player's party 
		/// (use the event command "Recover All: Entire Party" for this), 
		/// and then turn that Game Switch OFF again.
		/// </remarks>
		public MetadataPosition Home { get; set; }
		/// <summary>
		/// The background music that plays while the player is cycling.
		/// </summary>
		public string BicycleBGM { get; set; }
		/// <summary>
		/// The background music that plays while the player is surfing.
		/// </summary>
		public string SurfBGM { get; set; }
		/// <summary>
		/// The default music that plays during wild Pokémon battles. 
		/// It should be placed in the "Audio/BGM" directory.
		/// </summary>
		public string WildBattleBGM { get; set; }
		/// <summary>
		/// The default music that plays during trainer battles. 
		/// It should be placed in the "Audio/BGM" directory.
		/// </summary>
		public string TrainerBattleBGM { get; set; }
		/// <summary>
		/// The default victory music that plays at the end of a won wild Pokémon battle. 
		/// It should be placed in the "Audio/ME" directory.
		/// </summary>
		public string WildVictoryME { get; set; }
		/// <summary>
		/// The default victory music that plays at the end of a won Trainer battle. 
		/// It should be placed in the "Audio/ME" directory.
		/// </summary>
		public string TrainerVictoryME { get; set; }
		/// <summary>
		/// The default capture music that plays at the end of a captured Pokémon battle. 
		/// It should be placed in the "Audio/ME" directory.
		/// </summary>
		public string WildCaptureME { get; set; }
	}
	[Serializable] public struct MapMetadata
	{
		public int MapId					{ get; set; }
		/// <summary>
		/// If this is TRUE, this map is an outdoor map. 
		/// If this is FALSE (or this line doesn't exist), this map is an indoor map. 
		/// Only outdoor maps will have day/night tinting. 
		/// The hidden move Fly can only be used on an outdoor map.
		/// <para>For which maps: Outdoor maps</para>
		/// </summary>
		public bool Outdoor					{ get; set; }
		/// <summary>
		/// If this is TRUE, the bicycle can be used on this map. 
		/// If this is FALSE, it cannot. 
		/// If this line doesn't exist, then the player will only be able to ride a bicycle if this map is an outdoor map.
		/// <para>For which maps: Non-outdoor maps that can be cycled in</para>
		/// </summary>
		/// <remarks>
		/// Note that caves are not outdoor maps, 
		/// and therefore cave maps must have this metadata (set to TRUE) 
		/// in order to allow the player to cycle in caves. 
		/// This can also apply to gatehouses.
		/// </remarks>
		public bool Bicycle					{ get; set; }
		/// <summary>
		/// If this is TRUE, the player will automatically mount their bicycle upon entering this map, 
		/// and they cannot dismount it (not even to fish or surf) while on this map.
		/// <para>For which maps: Cycling Road maps</para>
		/// </summary>
		/// <remarks>
		/// Note that the player will mount a bicycle upon entering this map even if they don't own one. 
		/// Checks should be made before the player can enter one of these maps, to allow them through only if they own a bicycle.
		/// </remarks>
		public bool BicycleAlways			{ get; set; }
		/// <summary>
		/// This is a map ID number followed by the X and Y coordinates of a particular tile in that map.
		/// When a map with this metadata is entered (e.g.the interior of a Poké Center), 
		/// the Teleport destination is changed to the spot described by this metadata. 
		/// Note that the spot itself is not usually on the same map as the one that has this metadata set; 
		/// the spot is the tile just in front of the entrance to this map (e.g.just in front of a Poké Center's entrance).
		/// <para>For which maps: Poké Centre interiors</para>
		/// </summary>
		/// <remarks>
		/// Note that the only thing this metadata affects is the Teleport destination. 
		/// It does not determine Fly destinations (those are set on the region map in the PBS file "townmap.txt"), 
		/// nor does it determine where the player goes to after blacking out 
		/// (this is either set by the script <see cref="IGameField.pbSetPokemonCenter"/> which is part of the Poké Center nurse event, 
		/// or the <see cref="GlobalMetadata.Home"/> global metadata).
		/// </remarks>
		public ITilePosition HealingSpot	{ get; set; } //map id, x, y
		/// <summary>
		/// The position on the region map where this map is located. 
		/// This metadata consists of the region number and this map's X and Y coordinates on that region map.
		/// When standing in this map and looking at the Town Map, the player's head icon will be placed on this point to show their current location. 
		/// Note that this is (pretty much) the only purpose of this setting 
		/// - it does not name the point or make it a Fly destination 
		/// - see the page Region map for those features.
		/// <para>For which maps: All maps</para>
		/// </summary>
		/// <remarks>
		/// For large maps, this should be the top left square of the map(even if that particular square isn't part of the map).
		/// </remarks>
		public ITilePosition MapPosition	{ get; set; } //map id, x, y
		/// <summary>
		/// The size and shape of this map in <see cref="IRegionMap"/> squares.
		/// <para>For which maps: Large maps</para>
		/// </summary>
		/// <example>
		/// For example, if the width of the map is 2, and the layout is "1011", then the map is a 2x2 L-shaped map with the gap in the top right corner.
		/// </example>
		public IRegionMapMetadata MapSize	{ get; set; }
		/// <summary>
		/// If this is TRUE, a location signpost stating the map's name will be displayed at the top left of the screen when it is entered. 
		/// If this is FALSE (or this line doesn't exist), it won't.
		/// Typically, this metadata should only be TRUE for outdoor maps and other important areas (e.g.some caves).
		/// <para>For which maps: Outdoor maps, other important maps</para>
		/// </summary>
		/// <remarks>
		/// There are some cases where a location signpost will not be displayed even if this metadata says it should. 
		/// See the page Map transfers for more details.
		/// </remarks>
		public bool ShowArea				{ get; set; }
		/// <summary>
		/// The weather in effect on this map, and the likelihood of it.
		/// <para>For which maps: </para>
		/// </summary>
		public MetadataWeather? Weather		{ get; set; }
		/// <summary>
		/// If this is TRUE, this map is enshrouded in darkness and a small circle of light will appear around the player. 
		/// If this is FALSE (or this line doesn't exist), there is no darkness. 
		/// The player can use the move Flash to illuminate the surroundings, 
		/// and can only use Flash on dark maps (if it has not already been used).
		/// <para>For which maps: Dark maps</para>
		/// </summary>
		public bool DarkMap					{ get; set; }
		/// <summary>
		/// The underwater layer of this map. 
		/// This metadata defines the map ID of the underwater map related to this map, 
		/// and is required if this map contains accessible patches of deep water (tiles with terrain tag 5) where the move Dive can be used.
		/// <para>For which maps: Maps with deep water to dive into</para>
		/// </summary>
		/// <remarks>
		/// Multiple maps cannot refer to the same underwater map, as the game wouldn't know which map to return the player to when surfacing.
		/// </remarks>
		public int? DiveMap					{ get; set; }
		/// <summary>
		/// If this is TRUE, this map is part of the Safari Zone.
		/// Walking in a map that is part of the Safari Zone will decrease the player's Safari Zone step count. 
		/// Wild Pokémon battles that occur in these maps will be Safari encounters 
		/// (i.e. the player doesn't use Pokémon, but instead throws bait/rocks, and the wild Pokémon either stays or flees, etc.).
		/// <para>For which maps: Safari Zone maps</para>
		/// </summary>
		public bool SafariMap				{ get; set; }
		/// <summary>
		/// If this is TRUE, then the screen cannot scroll past the edges of the map. 
		/// This avoids showing the black borders beyond the edges of maps, 
		/// but the camera will not be centred on the player if they are close to the edges.
		/// </summary>
		public bool SnapEdges				{ get; set; }
		/// <summary>
		/// If this is TRUE, this map is a randomly generated dungeon map which changes layout each time the player enters it. 
		/// This map should not be connected to any other map.
		/// <para>For which maps: Dungeon maps</para>
		/// </summary>
		public bool Dungeon					{ get; set; }
		/// <summary>
		/// The backdrop picture used for all battles that take place on this map. 
		/// This setting is a phrase which corresponds to a particular backdrop; 
		/// see the page Backgrounds and music for more information.
		/// </summary>
		public string BattleBack			{ get; set; }
		/// <summary>
		/// The environment in effect on this map. 
		/// </summary>
		public Environments Environment		{ get; set; }
		/// <summary>
		/// The music played during any wild battles that take place on this map. 
		/// See the page Backgrounds and music for more information.
		/// </summary>
		public string WildBattleBGM			{ get; set; }
		/// <summary>
		/// The music played during any trainer battle that takes place on this map. 
		/// See the page Backgrounds and music for more information.
		/// </summary>
		public string TrainerBattleBGM		{ get; set; }
		/// <summary>
		/// The music played when the player wins a wild Pokémon battle that takes place on this map. 
		/// See the page Backgrounds and music for more information.
		/// </summary>
		public string WildVictoryME			{ get; set; }
		/// <summary>
		/// The music played when the player wins a trainer Pokémon battle that takes place on this map. 
		/// See the page Backgrounds and music for more information.
		/// </summary>
		public string TrainerVictoryME		{ get; set; }
		/// <summary>
		/// The music played when the player captures a Pokémon in a wild Pokémon battle that takes place on this map.
		/// See the page Backgrounds and music for more information.
		/// </summary>
		public string WildCaptureME			{ get; set; }
	}
	/// <summary>
	/// </summary>
	/// Load both resources per map id...
	public interface IPokemonMetadata
	{
		object this[GlobalMetadatas i]	{ get; } //{ return GlobalTypes[i]; } }
		object this[MapMetadatas i]		{ get; } //{ return NonGlobalTypes[i]; } }
		
		int MapId						{ get; }
		GlobalMetadata Global			{ get; }
		MapMetadata Map					{ get; }


		#region Global Types
		//private Dictionary<GlobalMetadatas, object> GlobalTypes = new Dictionary<GlobalMetadatas, object>() {
		//	{ GlobalMetadatas.MetadataHome,"uuuu" },
		//	{ GlobalMetadatas.MetadataWildBattleBGM,"s" },
		//	{ GlobalMetadatas.MetadataTrainerBattleBGM,"s" },
		//	{ GlobalMetadatas.MetadataWildVictoryME,"s" },
		//	{ GlobalMetadatas.MetadataTrainerVictoryME,"s" },
		//	{ GlobalMetadatas.MetadataSurfBGM,"s" },
		//	{ GlobalMetadatas.MetadataBicycleBGM,"s" },
		//	{ GlobalMetadatas.MetadataPlayerA, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerB, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerC, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerD, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerE, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerF, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerG, TrainerTypes.PLAYER },
		//	{ GlobalMetadatas.MetadataPlayerH, TrainerTypes.PLAYER }
		//};

		//KeyValuePair<int, string> Home = new KeyValuePair<int, string>(MetadataHome, "uuuu"); //map id, x, y
		//KeyValuePair<int, string> WildBattleBGM = new KeyValuePair<int, string>(MetadataWildBattleBGM, "s");
		//KeyValuePair<int, string> TrainerBattleBGM = new KeyValuePair<int, string>(MetadataTrainerBattleBGM, "s");
		//KeyValuePair<int, string> WildVictoryME = new KeyValuePair<int, string>(MetadataWildVictoryME, "s");
		//KeyValuePair<int, string> TrainerVictoryME = new KeyValuePair<int, string>(MetadataTrainerVictoryME, "s");
		//KeyValuePair<int, string> SurfBGM = new KeyValuePair<int, string>(MetadataBicycleBGM, "s");
		//KeyValuePair<int, string> BicycleBGM = new KeyValuePair<int, string>(MetadataSurfBGM, "s");
		//KeyValuePair<int, TrainerTypes> PlayerA = new KeyValuePair<int, TrainerTypes>(MetadataPlayerA, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerB = new KeyValuePair<int, TrainerTypes>(MetadataPlayerB, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerC = new KeyValuePair<int, TrainerTypes>(MetadataPlayerC, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerD = new KeyValuePair<int, TrainerTypes>(MetadataPlayerD, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerE = new KeyValuePair<int, TrainerTypes>(MetadataPlayerE, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerF = new KeyValuePair<int, TrainerTypes>(MetadataPlayerF, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerG = new KeyValuePair<int, TrainerTypes>(MetadataPlayerG, TrainerTypes.PLAYER);
		//KeyValuePair<int, TrainerTypes> PlayerH = new KeyValuePair<int, TrainerTypes>(MetadataPlayerH, TrainerTypes.PLAYER);
		#endregion

		#region NonGlobalTypes
		//private Dictionary<MapMetadatas, object> NonGlobalTypes = new Dictionary<MapMetadatas, object>()
		//{
		//	{ MapMetadatas.MetadataOutdoor, "b" },
		//	{ MapMetadatas.MetadataShowArea, "b" },
		//	{ MapMetadatas.MetadataBicycle, "b" },
		//	{ MapMetadatas.MetadataBicycleAlways, "b" },
		//	{ MapMetadatas.MetadataHealingSpot, "uuu" },
		//	{ MapMetadatas.MetadataWeather, FieldWeathers.Clear },
		//	{ MapMetadatas.MetadataMapPosition, "uuu" },
		//	{ MapMetadatas.MetadataDiveMap, "u" },
		//	{ MapMetadatas.MetadataDarkMap, "b" },
		//	{ MapMetadatas.MetadataSafariMap, "b" },
		//	{ MapMetadatas.MetadataSnapEdges, "b" },
		//	{ MapMetadatas.MetadataDungeon, "b" },
		//	{ MapMetadatas.MetadataBattleBack, "s" },
		//	{ MapMetadatas.MetadataMapWildBattleBGM, "s" },
		//	{ MapMetadatas.MetadataMapTrainerBattleBGM, "s" },
		//	{ MapMetadatas.MetadataMapWildVictoryME, "s" },
		//	{ MapMetadatas.MetadataMapTrainerVictoryME, "s" },
		//	{ MapMetadatas.MetadataMapSize, "us" }
		//};

		//NonGlobalTypes={
		//   KeyValuePair<int, bool> Outdoor = new KeyValuePair<int, string>(MetadataOutdoor,"b");
		//   KeyValuePair<int, bool> ShowArea = new KeyValuePair<int, string>(MetadataShowArea,"b");
		//   KeyValuePair<int, bool> Bicycle = new KeyValuePair<int, string>(MetadataBicycle,"b");
		//   KeyValuePair<int, bool> BicycleAlways = new KeyValuePair<int, string>(MetadataBicycleAlways,"b");
		//   KeyValuePair<int, TilePosition> HealingSpot = new KeyValuePair<int, string>(MetadataHealingSpot,"uuu"); //map id, x, y
		//   KeyValuePair<int, FieldWeather> Weather = new KeyValuePair<int, >(MetadataWeather,"eu");
		//   KeyValuePair<int, TilePosition> MapPosition = new KeyValuePair<int, string>(MetadataMapPosition,"uuu"); //map id, x, y
		//   KeyValuePair<int, int> DiveMap = new KeyValuePair<int, string>(MetadataDiveMap,"u");
		//   KeyValuePair<int, bool> DarkMap = new KeyValuePair<int, string>(MetadataDarkMap,"b");
		//   KeyValuePair<int, bool> SafariMap = new KeyValuePair<int, string>(MetadataSafariMap,"b");
		//   KeyValuePair<int, bool> SnapEdges = new KeyValuePair<int, string>(MetadataSnapEdges,"b");
		//   KeyValuePair<int, bool> Dungeon = new KeyValuePair<int, string>(MetadataDungeon,"b");
		//   KeyValuePair<int, string> BattleBack = new KeyValuePair<int, string>(MetadataBattleBack,"s");
		//   KeyValuePair<int, string> WildBattleBGM = new KeyValuePair<int, string>(MetadataMapWildBattleBGM,"s");
		//   KeyValuePair<int, string> TrainerBattleBGM = new KeyValuePair<int, string>(MetadataMapTrainerBattleBGM,"s");
		//   KeyValuePair<int, string> WildVictoryME = new KeyValuePair<int, string>(MetadataMapWildVictoryME,"s");
		//   KeyValuePair<int, string> TrainerVictoryME = new KeyValuePair<int, string>(MetadataMapTrainerVictoryME,"s");
		//   KeyValuePair<int, string> MapSize = new KeyValuePair<int, string>(MetadataMapSize,"us");
		//}
		#endregion
	}

	public interface IGameMetadataMisc {
		#region Manipulation methods for metadata, phone data and Pokémon species data
		IDictionary<int, IPokemonMetadata> pbLoadMetadata();

		//object pbGetMetadata(int mapid,int metadataType);
		IPokemonMetadata pbGetMetadata(int mapid); //,int metadataType
		//object pbGetMetadata(int mapid, GlobalMetadatas metadataType);
		//object pbGetMetadata(int mapid, MapMetadatas metadataType);

		IList<int> pbLoadPhoneData();

		IList<string> pbOpenDexData(Func<IList<string>, IList<string>> block = null);

		//void pbDexDataOffset(ref IList<string> dexdata,Pokemons species,int offset) {
		//  dexdata.pos=76*(species-1)+offset;
		//}

		void pbClearData();
		#endregion
	}
}