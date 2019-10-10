using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;

namespace PokemonUnity.Overworld
{
	#region Maps & Overworld
	public enum Weather
	{
		NONE,
		RAINDANCE,
		HEAVYRAIN,
		SUNNYDAY,
		HARSHSUN,
		SANDSTORM,
		STRONGWINDS,
		HAIL
	}
	/// <summary>
	/// Terrain Tags or Tiles a player can be stepping on;
	/// used to contruct map floor plane
	/// </summary>
	public enum Terrain
	{
		Plain,
		Grass,
		Sand,
		Rock,
		DeepWater,
		StillWater,
		Water,
		TallGrass,
		SootGrass,
		Puddle
	}
	public enum Environment
	{
		None,
		/// <summary>
		/// Normal Grass, and Sooty Tall Grass, are both grass but different colors
		/// </summary>
		Grass,
		Cave,
		Sand,
		Rock,
		MovingWater,
		StillWater,
		Underwater,
		/// <summary>
		/// Tall Grass
		/// </summary>
		TallGrass,
		Forest,
		Snow,
		Volcano,
		Graveyard,
		Sky,
		Space
	}
		public enum EncounterTypes
	{
		Pal_Park, Egg, Hatched, Special_Event,      //= 0x0
		Tall_Grass,                                 //= 0x2
		Plot_Event, //Dialga/Palkia In-Game Event,	//= 0x4
		Cave, Hall_of_Origin,                       //= 0x5
		Surfing, Fishing,                           //= 0x7	
		Building,                                   //= 0x9	
		Great_Marsh, //(Safari Zone)				//= 0xA	
		Starter, Fossil, Gift, //(Eevee)				//= 0xC	
		Land
	}
	/*enum EncounterActions
	{
		Land,
		/// <summary>
		/// If null or empty, defaults to Land
		/// </summary>
		LandMorning,
		LandDay,
		LandNight,
		//ToDo: Missing Grass, and Tall grass...
		Cave,
		BugContest,
		Water,
		RockSmash,
		OldRod,
		GoodRod,
		SuperRod,
		HeadbuttLow,
		HeadbuttHigh
	}*/

	/// <summary>
	/// Encounter method
	/// </summary>
	public enum Method
	{
		/// <summary>
		/// Walking in tall grass or a cave
		/// </summary>
		WALK = 1,
		/// <summary>
		/// Walking in rustling grass
		/// </summary>
		GRASS_SPOTS = 9,
		/// <summary>
		/// Walking in dust clouds
		/// </summary>
		CAVE_SPOTS = 10,
		/// <summary>
		/// Walking in bridge shadows
		/// </summary>
		BRIDGE_SPOTS = 11,
		/// <summary>
		/// Walking in dark grass
		/// </summary>
		DARK_GRASS = 8,
		/// <summary>
		/// Walking in yellow flowers
		/// </summary>
		YELLOW_FLOWERS = 14,
		/// <summary>
		/// Walking in purple flowers
		/// </summary>
		PURPLE_FLOWERS = 15,
		/// <summary>
		/// Walking in red flowers
		/// </summary>
		RED_FLOWERS = 16,
		/// <summary>
		/// Walking on rough terrain
		/// </summary>
		ROUGH_TERRAIN = 17,
		/// <summary>
		/// Fishing with an <see cref="eItems.Item.OLD_ROD"/>
		/// </summary>
		OLD_ROD = 2,
		/// <summary>
		/// Fishing with a <see cref="eItems.Item.GOOD_ROD"/> 
		/// </summary>
		GOOD_ROD = 3,
		/// <summary>
		/// Fishing with a <see cref="eItems.Item.SUPER_ROD"/> 
		/// </summary>
		SUPER_ROD = 4,
		/// <summary>
		/// Fishing in dark spots
		/// </summary>
		SUPER_ROD_SPOTS = 12,
		/// <summary>
		/// Surfing
		/// </summary>
		SURF = 5,
		/// <summary>
		/// Surfing in dark spots
		/// </summary>
		SURF_SPOTS = 13,
		/// <summary>
		/// Smashing rocks
		/// </summary>
		ROCK_SMASH = 6,
		/// <summary>
		/// Headbutting trees
		/// </summary>
		HEADBUTT = 7
	}

	/// <summary>
	/// 
	/// </summary>
	public enum Condition
	{
		SWARM = 1,
		TIME = 2,
		RADAR = 3,
		SLOT = 4,
		RADIO = 5,
		SEASON = 6
	}

	/// <summary>
	/// 
	/// </summary>
	/// <remarks>
	/// default: swarm-no, time-day, radar-off, slot-none, radio-off
	/// </remarks>
	/// ToDo: Change from Enum to class with Bool values
	public enum ConditionValue
	{
		/// <summary>
		/// During a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		SWARM_YES = 1,
		/// <summary>
		/// Not during a swarm
		/// <para>
		/// <seealso cref="Condition.SWARM"/>
		/// </para>
		/// </summary>
		SWARM_NO = 2,

		/// <summary>
		/// In the morning
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_MORNING = 3,
		/// <summary>
		/// During the day
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_DAY = 4,
		/// <summary>
		/// At night
		/// <para>
		/// <seealso cref="Condition.TIME"/>
		/// </para>
		/// </summary>
		TIME_NIGHT = 5,

		/// <summary>
		/// Using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		RADAR_ON = 6,
		/// <summary>
		/// Not using PokeRadar
		/// <para>
		/// <seealso cref="Condition.RADAR"/>
		/// </para>
		/// </summary>
		RADAR_OFF = 7,

		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_NONE = 8,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_RUBY = 9,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_SAPPHIRE = 10,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_EMERALD = 11,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_FIRERED = 12,
		/// <summary>
		/// <para>
		/// <seealso cref="Condition.SLOT"/>
		/// </para>
		/// </summary>
		SLOT_LEAFGREEN = 13,

		/// <summary>
		/// Radio off
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_OFF = 14,
		/// <summary>
		/// Hoenn radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_HOENN = 15,
		/// <summary>
		/// Sinnoh radio
		/// <para>
		/// <seealso cref="Condition.RADIO"/>
		/// </para>
		/// </summary>
		RADIO_SINNOH = 16,

		/// <summary>
		/// During Spring
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_SPRING = 17,
		/// <summary>
		/// During Summer
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_SUMMER = 18,
		/// <summary>
		/// During Autumn
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_AUTUMN = 19,
		/// <summary>
		/// During Winter
		/// <para>
		/// <seealso cref="Condition.SEASON"/>
		/// </para>
		/// </summary>
		SEASON_WINTER = 20
	}
	#endregion

	public enum Shape
	{
		/// <summary>
		/// Flat surface. 
		/// </summary>
		/// Stairs use flats too
		Flat,
		/// <summary>
		/// Box(five sides)
		/// </summary>
		Entrance,
		/// <summary>
		/// 45 degree slant.
		/// </summary>
		Slant,
		/// <summary>
		/// Flat vertical surface
		/// </summary>
		CliffSide,
		/// <summary>
		/// "In" corner.
		/// </summary>
		CliffCornerIn,
		/// <summary>
		/// "Out" corner.
		/// </summary>
		CliffCornerOut,
		/// <summary>
		/// Ledge.
		/// </summary>
		LedgeJump,
		/// <summary>
		/// Ledge Corner.
		/// </summary>
		LedgeJumpCorner,
		/// <summary>
		/// Water's edge.
		/// </summary>
		LedgeWater,
		/// <summary>
		/// Water's edge "in" corner.
		/// </summary>
		LedgeWaterIn,
		/// <summary>
		/// Water's edge "out" corner.
		/// </summary>
		LedgeWaterOut,
		/// <summary>
		/// Only here because 3ds using a 2x2 dirt path
		/// </summary>
		WalkPath,
		/// <summary>
		/// Thin block. 4 Pixels wide.
		/// </summary>
		Fence,
		/// <summary>
		/// Double floor (a floor with a floor above it)
		/// </summary>
		Bridge,
		/// <summary>
		/// Two Flat pieces intersecting in their centers at 90 degree angles.
		/// </summary>
		Crossway,
		/// <summary>
		/// Box (Six Sides).
		/// </summary>
		NULL
	}
	public enum Entities
	{
		Floor,//Default?...
		/// <summary>
		/// Renders sides from all directions.
		/// </summary>
		AllSidesObject,// or 
		Cube,
		/// <summary>
		/// Creates an Apricorn Plant.
		/// <para></para>
		/// An integer defining which Apricorn is used.
		/// </summary>
		/// 0 - White
		/// 1 - Black
		/// 2 - Pink
		/// 3 - Blue
		/// 4 - Red
		/// 5 - Green
		/// 6 - Yellow
		ApricornPlant,
		BerryPlant,
		/// <summary>
		/// Allows the player to use cut on the Entity.
		/// </summary>
		CutTree,
		/// <summary>
		/// Creates long grass where the player may run into wild pokemon.
		/// <para></para>
		/// An integer defining properties of grass.
		/// </summary>
		Grass,
		/// <summary>
		/// Allows the player to use headbutt on the Entity.
		/// </summary>
		HeadbuttTree,
		/// <summary>
		/// Creates an item, if the "Action" tag = 1 then it is a "hidden" item.
		/// <para></para>
		/// int,int - the first value is the ID for the item on that map(each item should have a unique ID) The second is the item's ID.
		/// </summary>
		ItemObject,
		/// <summary>
		/// Creates a spot to grow a berry in.
		/// </summary>
		LoamySoil,
		/// <summary>
		/// Creates a block to trigger a script when it is either walked on or clicked on.
		/// <para></para>
		/// Depending on the "Action" tag the way the "AdditionalValue" tag is interpreted is changed
		/// </summary>
		/// 0 - Activates the given script when walked on. Automatically invisible.
		/// 1 - Activates the given script when clicked on.
		/// 2 - Displays the given text when clicked on.
		/// 3 - Interprets the given text as a script.
		/// 4 - Activates the given script when walked on.
		ScriptBlock,
		/// <summary>
		/// Creates an entity that either displays text or activates a script when the player "talks" to it.
		/// <para></para>
		/// Based on the "Action" tag the "AdditionalValue" tag is interpreted differently:
		/// </summary>
		/// 0 - Displays the given text as text.
		/// 1 - Activates the given script.
		/// 2 - Converts the given text to a script.
		SignBlock,
		/// <summary>
		/// Creates a stairway.
		/// </summary>
		SlideBlock,
		/// <summary>
		/// Allows the player to use rock smash on the Entity.
		/// </summary>
		SmashRock,
		/// <summary>
		/// Creates a ledge the player can lead down.
		/// </summary>
		Step,
		/// <summary>
		///  Allows the player to use strength on the Entity.
		/// </summary>
		StrengthRock,
		/// <summary>
		/// Creates a Trigger that activates when a "StrengthRock" is pushed onto it.
		/// <para></para>
		/// bool,bool, str - the bools are for if the rock is removed immediately, and if the rock is nolonger loaded with the map, the string is the script activated when the trigger is activated.
		/// </summary>
		StrengthTrigger,
		/// <summary>
		/// A sign that spins at the center.
		/// <para></para>
		/// An int defining what texture is used for the sign.
		/// </summary>
		/// 0 - PokeCenter
		/// 1 - Mart
		/// 2 - Gym Sign
		TurningSign,
		/// <summary>
		/// An entity that always shows the same face to the player(spins on the center.
		/// </summary>
		WallBill,
		/// <summary>
		/// Basic Entity.
		/// </summary>
		WallBlock,
		/// <summary>
		/// Creates a warp.
		/// <para></para>
		/// mapfilepath, X, Y, Z, R, E - The mapfile, position, number of 1/4 turns to warp, and directions from which it can be accessed separated by "|".
		/// </summary>
		WarpBlock,
		/// <summary>
		/// Creates a surf spot, ignores texture(s) given.
		/// </summary>
		Water,
		/// <summary>
		/// Creates a Waterfall, ignores texture(s) given.
		/// </summary>
		Waterfall,
		/// <summary>
		/// Creates a Whirlpool, ignores texture(s) given.
		/// </summary>
		Whirlpool,
		/// <summary>
		/// Creates a dive spot, ignores texture(s) given.
		/// </summary>
		DiveTile,
		RockClimbEntity,
		NPC,
		ModelEntity,
		RotationTile,
		AnimatedBlock,
		NetworkPokemon,
		MessageBulb,
		OverworldPokemon,
		OwnPlayer,
		Particle
	}
}


namespace PokemonUnity
{
	#region Enums
	public enum Worlds
	{
		GLOBAL = 0,
		Emerald
	}
	/// <summary>
	/// Each "world" has their own smaller regions
	/// </summary>
	public enum Regions
	{
		Overworld = 0
	}
	/// <summary>
	/// Each "region" has their own individual maps
	/// </summary>
	public enum Maps
	{
		Safari = 0
	}
	public enum Direction
	{
		/// <summary>
		/// Facing Foward, towards camera
		/// </summary>
		Down = 0,
		Up,
		Left,
		Right
	}
	public enum Season
	{
		Summer,
		Winter,
		Fall,
		Spring,
		Volcanic
	}
	#endregion
}