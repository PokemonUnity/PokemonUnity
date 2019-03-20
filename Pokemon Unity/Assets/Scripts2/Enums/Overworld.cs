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
		Starter, Fossil, Gift //(Eevee)				//= 0xC	
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
}