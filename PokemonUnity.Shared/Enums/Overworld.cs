using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;

namespace PokemonUnity.Overworld
{
	#region Maps & Overworld
	public enum DayTime : int
	{
		Night	= 0,
		Morning = 1,
		Day		= 2,
		Evening = 3
	}
	public enum FieldWeathers : int
	{
		Clear = 0,
		Rain = 1,
		Snow = 2,
		Underwater = 3,
		Sunny = 4,
		Fog = 5,
		Thunderstorm = 6,
		Sandstorm = 7,
		Ash = 8,
		Blizzard = 9
	}
	public enum Weathers
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
	public enum Terrains
	{
		Ledge           = 1,
		Grass           = 2,
		Sand            = 3,
		Rock            = 4,
		DeepWater       = 5,
		StillWater      = 6,
		Water           = 7,
		Waterfall       = 8,
		WaterfallCrest  = 9,
		TallGrass       = 10,
		UnderwaterGrass = 11,
		Ice             = 12,
		Neutral         = 13,
		SootGrass       = 14,
		Bridge          = 15,
		Puddle          = 16
		/*Plain,
		Grass,
		Sand,
		Rock,
		DeepWater,
		StillWater,
		Water,
		TallGrass,
		SootGrass,
		Puddle
/* Veekun's Database

Terrain        
-------------- 
Building       
Cave           
Desert         
Grass          
Mountain       
Ocean          
Pond           
Road           
Snow           
Tall grass     

In Pokémon Battle Revolution:

Terrain        
-------------- 
Courtyard      
Crystal        
Gateway        
Magma          
Main Street    
Neon           
Stargazer      
Sunny Park     
Sunset         
Waterfall      
*/
	}
	public enum Environments
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
		Land         = 0 ,
		Cave         = 1 ,
		Water        = 2 ,
		RockSmash    = 3 ,
		OldRod       = 4 ,
		GoodRod      = 5 ,
		SuperRod     = 6 ,
		HeadbuttLow  = 7 ,
		HeadbuttHigh = 8 ,
		LandMorning  = 9 ,
		LandDay      = 10,
		LandNight    = 11,
		BugContest   = 12
		//Pal_Park, Egg, Hatched, Special_Event,      //= 0x0
		//Tall_Grass,                                 //= 0x2
		//Plot_Event, //Dialga/Palkia In-Game Event,	//= 0x4
		//Cave, Hall_of_Origin,                       //= 0x5
		//Surfing, Fishing,                           //= 0x7	
		//Building,                                   //= 0x9	
		//Great_Marsh, //(Safari Zone)				//= 0xA	
		//Starter, Fossil, Gift, //(Eevee)			//= 0xC	
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
		/// Fishing with an <see cref="Items.OLD_ROD"/>
		/// </summary>
		OLD_ROD = 2,
		/// <summary>
		/// Fishing with a <see cref="Items.GOOD_ROD"/> 
		/// </summary>
		GOOD_ROD = 3,
		/// <summary>
		/// Fishing with a <see cref="Items.SUPER_ROD"/> 
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
		HEADBUTT = 7,
		/// <summary>
		/// Receive as a gift
		/// </summary>
		GIFT = 18,
		/// <summary>
		/// Receive egg as a gift
		/// </summary>
		GIFT_EGG = 19
	}

	/// <summary>
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
	/// </summary>
	/// <remarks>
	/// default: swarm-no, time-day, radar-off, slot-none, radio-off
	/// </remarks>
	/// ToDo: Change from Enum to class with Bool values
	public enum ConditionValue
	{
		NONE = 0,

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
	public enum MovePermissions
	{

	}
	public enum MapTiles
	{
		/// <summary>
		/// Spawns random encounter
		/// </summary>
		GRASS,
		/// <summary>
		/// Animated water
		/// </summary>
		SEA,
		/// <summary>
		/// 3-dimensional airborne encounter
		/// </summary>
		SKY,
		/// <summary>
		/// Regular walk-able path
		/// </summary>
		TILE,
		/// <summary>
		/// Impassable (invisible) collision object
		/// </summary>
		WALL,
		/// <summary>
		/// Can only cross with surfing action
		/// </summary>
		WATER,
		/// <summary>
		/// Triggers map change or used for map events
		/// </summary>
		ZONE
	}
	#endregion
	//ToDo: Rename TileShapes
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
		NOT_IN_OVERWORLD = 0
		,KANTO	= 1
		,JOHTO	= 2
		,HOENN	= 3
		,SINNOH	= 4
		,UNOVA	= 5
		,KALOS	= 6
		,ALOLA	= 7
	}
	/// <summary>
	/// Each "region" has their own individual maps.
	/// <seealso cref="PokemonUnity.Overworld.Area"/>
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

	public enum Locations
	{
		NOT_IN_OVERWORLD = 0
		,CANALAVE_CITY	= 1
		,ETERNA_CITY	= 2
		,PASTORIA_CITY	= 3
		,SUNYSHORE_CITY	= 4
		,SINNOH_POKEMON_LEAGUE	= 5
		,OREBURGH_MINE	= 6
		,VALLEY_WINDWORKS	= 7
		,ETERNA_FOREST	= 8
		,FUEGO_IRONWORKS	= 9
		,MT_CORONET	= 10
		,GREAT_MARSH	= 11
		,SOLACEON_RUINS	= 12
		,SINNOH_VICTORY_ROAD	= 13
		,RAVAGED_PATH	= 14
		,OREBURGH_GATE	= 15
		,STARK_MOUNTAIN	= 16
		,SPRING_PATH	= 17
		,TURNBACK_CAVE	= 18
		,SNOWPOINT_TEMPLE	= 19
		,WAYWARD_CAVE	= 20
		,RUIN_MANIAC_CAVE	= 22
		,TROPHY_GARDEN	= 23
		,IRON_ISLAND	= 24
		,OLD_CHATEAU	= 25
		,LAKE_VERITY	= 26
		,LAKE_VALOR	= 27
		,LAKE_ACUITY	= 28
		,VALOR_LAKEFRONT	= 29
		,ACUITY_LAKEFRONT	= 30
		,SINNOH_ROUTE_201	= 31
		,SINNOH_ROUTE_202	= 32
		,SINNOH_ROUTE_203	= 33
		,SINNOH_ROUTE_204	= 34
		,SINNOH_ROUTE_205	= 35
		,SINNOH_ROUTE_206	= 36
		,SINNOH_ROUTE_207	= 37
		,SINNOH_ROUTE_208	= 38
		,SINNOH_ROUTE_209	= 39
		,LOST_TOWER	= 40
		,SINNOH_ROUTE_210	= 41
		,SINNOH_ROUTE_211	= 42
		,SINNOH_ROUTE_212	= 43
		,SINNOH_ROUTE_213	= 44
		,SINNOH_ROUTE_214	= 45
		,SINNOH_ROUTE_215	= 46
		,SINNOH_ROUTE_216	= 47
		,SINNOH_ROUTE_217	= 48
		,SINNOH_ROUTE_218	= 49
		,SINNOH_ROUTE_219	= 50
		,SINNOH_ROUTE_221	= 51
		,SINNOH_ROUTE_222	= 52
		,SINNOH_ROUTE_224	= 53
		,SINNOH_ROUTE_225	= 54
		,SINNOH_ROUTE_227	= 55
		,SINNOH_ROUTE_228	= 56
		,SINNOH_ROUTE_229	= 57
		,TWINLEAF_TOWN	= 58
		,CELESTIC_TOWN	= 59
		,RESORT_AREA	= 60
		,SINNOH_SEA_ROUTE_220	= 61
		,SINNOH_SEA_ROUTE_223	= 62
		,SINNOH_SEA_ROUTE_226	= 63
		,SINNOH_SEA_ROUTE_230	= 64
		,BLACKTHORN_CITY	= 65
		,BURNED_TOWER	= 66
		,CELADON_CITY	= 67
		,CERULEAN_CITY	= 68
		,CHERRYGROVE_CITY	= 69
		,CIANWOOD_CITY	= 70
		,CINNABAR_ISLAND	= 71
		,DARK_CAVE	= 72
		,DIGLETTS_CAVE	= 73
		,DRAGONS_DEN	= 74
		,ECRUTEAK_CITY	= 75
		,FUCHSIA_CITY	= 76
		,ICE_PATH	= 77
		,ILEX_FOREST	= 78
		,LAKE_OF_RAGE	= 79
		,MT_MOON	= 80
		,MT_MORTAR	= 81
		,MT_SILVER	= 82
		,NATIONAL_PARK	= 83
		,NEW_BARK_TOWN	= 84
		,OLIVINE_CITY	= 85
		,PALLET_TOWN	= 86
		,ROCK_TUNNEL	= 87
		,KANTO_ROUTE_1	= 88
		,KANTO_ROUTE_10	= 89
		,KANTO_ROUTE_11	= 90
		,KANTO_ROUTE_12	= 91
		,KANTO_ROUTE_13	= 92
		,KANTO_ROUTE_14	= 93
		,KANTO_ROUTE_15	= 94
		,KANTO_ROUTE_16	= 95
		,KANTO_ROUTE_17	= 96
		,KANTO_ROUTE_18	= 97
		,KANTO_SEA_ROUTE_19	= 98
		,KANTO_ROUTE_2	= 99
		,KANTO_SEA_ROUTE_20	= 100
		,KANTO_SEA_ROUTE_21	= 101
		,KANTO_ROUTE_22	= 102
		,KANTO_ROUTE_24	= 103
		,KANTO_ROUTE_25	= 104
		,KANTO_ROUTE_26	= 105
		,KANTO_ROUTE_27	= 106
		,KANTO_ROUTE_28	= 107
		,JOHTO_ROUTE_29	= 108
		,KANTO_ROUTE_3	= 109
		,JOHTO_ROUTE_30	= 110
		,JOHTO_ROUTE_31	= 111
		,JOHTO_ROUTE_32	= 112
		,JOHTO_ROUTE_33	= 113
		,JOHTO_ROUTE_34	= 114
		,JOHTO_ROUTE_35	= 115
		,JOHTO_ROUTE_36	= 116
		,JOHTO_ROUTE_37	= 117
		,JOHTO_ROUTE_38	= 118
		,JOHTO_ROUTE_39	= 119
		,KANTO_ROUTE_4	= 120
		,JOHTO_SEA_ROUTE_40	= 121
		,JOHTO_SEA_ROUTE_41	= 122
		,JOHTO_ROUTE_42	= 123
		,JOHTO_ROUTE_43	= 124
		,JOHTO_ROUTE_44	= 125
		,JOHTO_ROUTE_45	= 126
		,JOHTO_ROUTE_46	= 127
		,JOHTO_ROUTE_47	= 128
		,JOHTO_ROUTE_48	= 129
		,KANTO_ROUTE_5	= 130
		,KANTO_ROUTE_6	= 131
		,KANTO_ROUTE_7	= 132
		,KANTO_ROUTE_8	= 133
		,KANTO_ROUTE_9	= 134
		,RUINS_OF_ALPH	= 135
		,SEAFOAM_ISLANDS	= 136
		,SLOWPOKE_WELL	= 137
		,SPROUT_TOWER	= 138
		,BELL_TOWER	= 139
		,TOHJO_FALLS	= 140
		,UNION_CAVE	= 141
		,CERULEAN_CAVE	= 147
		,UNKNOWN_ALL_POLIWAG	= 148
		,UNKNOWN_ALL_RATTATA	= 149
		,UNKNOWN_ALL_BUGS	= 150
		,VERMILION_CITY	= 151
		,KANTO_VICTORY_ROAD_1	= 152
		,VIOLET_CITY	= 153
		,VIRIDIAN_CITY	= 154
		,VIRIDIAN_FOREST	= 155
		,WHIRL_ISLANDS	= 156
		,KANTO_ROUTE_23	= 157
		,POWER_PLANT	= 158
		,KANTO_VICTORY_ROAD_2	= 159
		,POKEMON_TOWER	= 160
		,POKEMON_MANSION	= 161
		,KANTO_SAFARI_ZONE	= 162
		,SANDGEM_TOWN	= 163
		,FLOAROMA_TOWN	= 164
		,SOLACEON_TOWN	= 165
		,JUBILIFE_CITY	= 167
		,OREBURGH_CITY	= 168
		,HEARTHOME_CITY	= 169
		,VEILSTONE_CITY	= 170
		,SNOWPOINT_CITY	= 171
		,SPEAR_PILLAR	= 172
		,PAL_PARK	= 173
		,AMITY_SQUARE	= 174
		,FLOAROMA_MEADOW	= 175
		,FULLMOON_ISLAND	= 177
		,SENDOFF_SPRING	= 178
		,FLOWER_PARADISE	= 179
		,MANIAC_TUNNEL	= 180
		,GALACTIC_HQ	= 181
		,VERITY_LAKEFRONT	= 182
		,NEWMOON_ISLAND	= 183
		,SINNOH_BATTLE_TOWER	= 184
		,FIGHT_AREA	= 185
		,SURVIVAL_AREA	= 186
		,SEABREAK_PATH	= 187
		,SINNOH_HALL_OF_ORIGIN_1	= 188
		,SINNOH_HALL_OF_ORIGIN_2	= 189
		,VERITY_CAVERN	= 190
		,VALOR_CAVERN	= 191
		,ACUITY_CAVERN	= 192
		,JUBILIFE_TV	= 193
		,POKETCH_CO	= 194
		,GTS	= 195
		,TRAINERS_SCHOOL	= 196
		,MINING_MUSEUM	= 197
		,SINNOH_FLOWER_SHOP	= 198
		,SINNOH_CYCLE_SHOP	= 199
		,CONTEST_HALL	= 200
		,POFFIN_HOUSE	= 201
		,SINNOH_FOREIGN_BUILDING	= 202
		,POKEMON_DAY_CARE	= 203
		,VEILSTONE_STORE	= 204
		,SINNOH_GAME_CORNER	= 205
		,CANALAVE_LIBRARY	= 206
		,VISTA_LIGHTHOUSE	= 207
		,SUNYSHORE_MARKET	= 208
		,FOOTSTEP_HOUSE	= 209
		,SINNOH_CAFE	= 210
		,GRAND_LAKE	= 211
		,SINNOH_RESTAURANT	= 212
		,BATTLE_PARK	= 213
		,BATTLE_FRONTIER	= 214
		,BATTLE_FACTORY	= 215
		,BATTLE_CASTLE	= 216
		,BATTLE_ARCADE	= 217
		,BATTLE_HALL	= 218
		,DISTORTION_WORLD	= 219
		,SINNOH_GLOBAL_TERMINAL	= 220
		,SINNOH_VILLA	= 221
		,BATTLEGROUND	= 222
		,ROTOMS_ROOM	= 223
		,TG_ETERNA_BLDG	= 224
		,IRON_RUINS	= 225
		,ICEBERG_RUINS	= 226
		,ROCK_PEAK_RUINS	= 227
		,AZALEA_TOWN	= 228
		,GOLDENROD_CITY	= 229
		,MAHOGANY_TOWN	= 230
		,PEWTER_CITY	= 231
		,LAVENDER_TOWN	= 232
		,INDIGO_PLATEAU	= 233
		,SAFFRON_CITY	= 234
		,JOHTO_LIGHTHOUSE	= 236
		,TEAM_ROCKET_HQ	= 237
		,GOLDENROD_TUNNEL	= 238
		,MT_SILVER_CAVE	= 239
		,POKEATHLON_DOME	= 241
		,SS_AQUA	= 242
		,SAFARI_ZONE_GATE	= 243
		,CLIFF_CAVE	= 244
		,FRONTIER_ACCESS	= 245
		,BELLCHIME_TRAIL	= 246
		,SINJOH_RUINS	= 247
		,EMBEDDED_TOWER	= 248
		,POKEWALKER	= 249
		,CLIFF_EDGE_GATE	= 250
		,RADIO_TOWER	= 252
		,DAY_CARE_COUPLE	= 253
		,LINK_TRADE_ARRIVE	= 254
		,LINK_TRADE_MET	= 255
		,KANTO	= 256
		,JOHTO	= 257
		,HOENN	= 258
		,SINNOH	= 259
		,DISTANT_LAND	= 260
		,TRAVELING_MAN	= 261
		,RILEY	= 262
		,CYNTHIA	= 263
		,MYSTERY_ZONE	= 264
		,LOVELY_PLACE	= 265
		,POKEMON_RANGER	= 266
		,FARAWAY_PLACE	= 267
		,POKEMON_MOVIE	= 268
		,POKEMON_MOVIE_06	= 269
		,POKEMON_MOVIE_07	= 270
		,POKEMON_MOVIE_08	= 271
		,POKEMON_MOVIE_09	= 272
		,POKEMON_MOVIE_10	= 273
		,POKEMON_MOVIE_11	= 274
		,POKEMON_MOVIE_12	= 275
		,POKEMON_MOVIE_13	= 276
		,POKEMON_MOVIE_14	= 277
		,POKEMON_MOVIE_15	= 278
		,POKEMON_MOVIE_16	= 279
		,POKEMON_CARTOON	= 280
		,SPACE_WORLD	= 281
		,SPACE_WORLD_06	= 282
		,SPACE_WORLD_07	= 283
		,SPACE_WORLD_08	= 284
		,SPACE_WORLD_09	= 285
		,SPACE_WORLD_10	= 286
		,SPACE_WORLD_11	= 287
		,SPACE_WORLD_12	= 288
		,SPACE_WORLD_13	= 289
		,SPACE_WORLD_14	= 290
		,SPACE_WORLD_15	= 291
		,SPACE_WORLD_16	= 292
		,POKEMON_FESTA	= 293
		,POKEMON_FESTA_06	= 294
		,POKEMON_FESTA_07	= 295
		,POKEMON_FESTA_08	= 296
		,POKEMON_FESTA_09	= 297
		,POKEMON_FESTA_10	= 298
		,POKEMON_FESTA_11	= 299
		,POKEMON_FESTA_12	= 300
		,POKEMON_FESTA_13	= 301
		,POKEMON_FESTA_14	= 302
		,POKEMON_FESTA_15	= 303
		,POKEMON_FESTA_16	= 304
		,POKEPARK	= 305
		,POKEPARK_06	= 306
		,POKEPARK_07	= 307
		,POKEPARK_08	= 308
		,POKEPARK_09	= 309
		,POKEPARK_10	= 310
		,POKEPARK_11	= 311
		,POKEPARK_12	= 312
		,POKEPARK_13	= 313
		,POKEPARK_14	= 314
		,POKEPARK_15	= 315
		,POKEPARK_16	= 316
		,POKEMON_CENTER	= 317
		,PC_TOKYO	= 318
		,PC_OSAKA	= 319
		,PC_FUKUOKA	= 320
		,PC_NAGOYA	= 321
		,PC_SAPPORO	= 322
		,PC_YOKOHAMA	= 323
		,NINTENDO_WORLD	= 324
		,POKEMON_EVENT	= 325
		,POKEMON_EVENT_06	= 326
		,POKEMON_EVENT_07	= 327
		,POKEMON_EVENT_08	= 328
		,POKEMON_EVENT_09	= 329
		,POKEMON_EVENT_10	= 330
		,POKEMON_EVENT_11	= 331
		,POKEMON_EVENT_12	= 332
		,POKEMON_EVENT_13	= 333
		,POKEMON_EVENT_14	= 334
		,POKEMON_EVENT_15	= 335
		,POKEMON_EVENT_16	= 336
		,WI_FI_EVENT	= 337
		,WI_FI_GIFT	= 338
		,POKEMON_FAN_CLUB	= 339
		,EVENT_SITE	= 340
		,CONCERT_EVENT	= 341
		,MR_POKEMON	= 342
		,PRIMO	= 343
		,UNOVA_MYSTERY_ZONE	= 344
		,UNOVA_FARAWAY_PLACE	= 345
		,NUVEMA_TOWN	= 346
		,ACCUMULA_TOWN	= 347
		,STRIATON_CITY	= 348
		,NACRENE_CITY	= 349
		,CASTELIA_CITY	= 350
		,NIMBASA_CITY	= 351
		,DRIFTVEIL_CITY	= 352
		,MISTRALTON_CITY	= 353
		,ICIRRUS_CITY	= 354
		,OPELUCID_CITY	= 355
		,UNOVA_ROUTE_1	= 356
		,UNOVA_ROUTE_2	= 357
		,UNOVA_ROUTE_3	= 358
		,UNOVA_ROUTE_4	= 359
		,UNOVA_ROUTE_5	= 360
		,UNOVA_ROUTE_6	= 361
		,UNOVA_ROUTE_7	= 362
		,UNOVA_ROUTE_8	= 363
		,UNOVA_ROUTE_9	= 364
		,UNOVA_ROUTE_10	= 365
		,UNOVA_ROUTE_11	= 366
		,UNOVA_ROUTE_12	= 367
		,UNOVA_ROUTE_13	= 368
		,UNOVA_ROUTE_14	= 369
		,UNOVA_ROUTE_15	= 370
		,UNOVA_ROUTE_16	= 371
		,UNOVA_ROUTE_17	= 372
		,UNOVA_ROUTE_18	= 373
		,DREAMYARD	= 374
		,PINWHEEL_FOREST	= 375
		,DESERT_RESORT	= 376
		,RELIC_CASTLE	= 377
		,COLD_STORAGE	= 378
		,CHARGESTONE_CAVE	= 379
		,TWIST_MOUNTAIN	= 380
		,DRAGONSPIRAL_TOWER	= 381
		,UNOVA_VICTORY_ROAD	= 382
		,LACUNOSA_TOWN	= 383
		,UNDELLA_TOWN	= 384
		,ANVILLE_TOWN	= 385
		,UNOVA_POKEMON_LEAGUE	= 386
		,NS_CASTLE	= 387
		,ROYAL_UNOVA	= 388
		,GEAR_STATION	= 389
		,BATTLE_SUBWAY	= 390
		,MUSICAL_THEATER	= 391
		,BLACK_CITY	= 392
		,WHITE_FOREST	= 393
		,UNITY_TOWER	= 394
		,WELLSPRING_CAVE	= 395
		,MISTRALTON_CAVE	= 396
		,RUMINATION_FIELD	= 397
		,CELESTIAL_TOWER	= 398
		,MOOR_OF_ICIRRUS	= 399
		,UNOVA_SHOPPING_MALL	= 400
		,CHALLENGERS_CAVE	= 401
		,POKE_TRANSFER_LAB	= 402
		,GIANT_CHASM	= 403
		,LIBERTY_GARDEN	= 404
		,P2_LABORATORY	= 405
		,SKYARROW_BRIDGE	= 406
		,DRIFTVEIL_DRAWBRIDGE	= 407
		,TUBELINE_BRIDGE	= 408
		,VILLAGE_BRIDGE	= 409
		,MARVELOUS_BRIDGE	= 410
		,ENTRALINK	= 411
		,ABUNDANT_SHRINE	= 412
		,UNDELLA_BAY	= 413
		,LOSTLORN_FOREST	= 414
		,TRIAL_CHAMBER	= 415
		,GUIDANCE_CHAMBER	= 416
		,ENTREE_FOREST	= 417
		,ACCUMULA_GATE	= 418
		,UNDELLA_GATE	= 419
		,NACRENE_GATE	= 420
		,CASTELIA_GATE	= 421
		,NIMBASA_GATE	= 422
		,OPELUCID_GATE	= 423
		,BLACK_GATE	= 424
		,WHITE_GATE	= 425
		,BRIDGE_GATE	= 426
		,ROUTE_GATE	= 427
		,ABYSSAL_RUINS	= 428
		,PETALBURG_CITY	= 429
		,SLATEPORT_CITY	= 430
		,LILYCOVE_CITY	= 431
		,MOSSDEEP_CITY	= 432
		,SOOTOPOLIS_CITY	= 433
		,EVER_GRANDE_CITY	= 434
		,METEOR_FALLS	= 435
		,RUSTURF_TUNNEL	= 436
		,GRANITE_CAVE	= 437
		,PETALBURG_WOODS	= 438
		,JAGGED_PASS	= 439
		,FIERY_PATH	= 440
		,MT_PYRE	= 441
		,SEAFLOOR_CAVERN	= 442
		,CAVE_OF_ORIGIN	= 443
		,HOENN_VICTORY_ROAD	= 444
		,SHOAL_CAVE	= 445
		,NEW_MAUVILLE	= 446
		,ABANDONED_SHIP	= 447
		,SKY_PILLAR	= 448
		,HOENN_ROUTE_101	= 449
		,HOENN_ROUTE_102	= 450
		,HOENN_ROUTE_103	= 451
		,HOENN_ROUTE_104	= 452
		,HOENN_ROUTE_105	= 453
		,HOENN_ROUTE_106	= 454
		,HOENN_ROUTE_107	= 455
		,HOENN_ROUTE_108	= 456
		,HOENN_ROUTE_109	= 457
		,HOENN_ROUTE_110	= 458
		,HOENN_ROUTE_111	= 459
		,HOENN_ROUTE_112	= 460
		,HOENN_ROUTE_113	= 461
		,HOENN_ROUTE_114	= 462
		,HOENN_ROUTE_115	= 463
		,HOENN_ROUTE_116	= 464
		,HOENN_ROUTE_117	= 465
		,HOENN_ROUTE_118	= 466
		,HOENN_ROUTE_119	= 467
		,HOENN_ROUTE_120	= 468
		,HOENN_ROUTE_121	= 469
		,HOENN_ROUTE_122	= 470
		,HOENN_ROUTE_123	= 471
		,HOENN_ROUTE_124	= 472
		,HOENN_ROUTE_125	= 473
		,HOENN_ROUTE_126	= 474
		,HOENN_ROUTE_127	= 475
		,HOENN_ROUTE_128	= 476
		,HOENN_ROUTE_129	= 477
		,HOENN_ROUTE_130	= 478
		,HOENN_ROUTE_131	= 479
		,HOENN_ROUTE_132	= 480
		,HOENN_ROUTE_133	= 481
		,HOENN_ROUTE_134	= 482
		,HOENN_SAFARI_ZONE	= 483
		,DEWFORD_TOWN	= 484
		,PACIFIDLOG_TOWN	= 485
		,MAGMA_HIDEOUT	= 486
		,MIRAGE_TOWER	= 487
		,DESERT_UNDERPASS	= 488
		,ARTISAN_CAVE	= 489
		,HOENN_ALTERING_CAVE	= 490
		,MONEAN_CHAMBER	= 491
		,LIPTOO_CHAMBER	= 492
		,WEEPTH_CHAMBER	= 493
		,DILFORD_CHAMBER	= 494
		,SCUFIB_CHAMBER	= 495
		,RIXY_CHAMBER	= 496
		,VIAPOS_CHAMBER	= 497
		,SS_ANNE	= 498
		,MT_EMBER	= 500
		,BERRY_FOREST	= 501
		,ICEFALL_CAVE	= 502
		,PATTERN_BUSH	= 503
		,LOST_CAVE	= 504
		,KINDLE_ROAD	= 505
		,TREASURE_BEACH	= 506
		,CAPE_BRINK	= 507
		,BOND_BRIDGE	= 508
		,THREE_ISLE_PORT	= 509
		,RESORT_GORGEOUS	= 510
		,WATER_LABYRINTH	= 511
		,FIVE_ISLE_MEADOW	= 512
		,MEMORIAL_PILLAR	= 513
		,OUTCAST_ISLAND	= 514
		,GREEN_PATH	= 515
		,WATER_PATH	= 516
		,RUIN_VALLEY	= 517
		,TRAINER_TOWER	= 518
		,CANYON_ENTRANCE	= 519
		,SEVAULT_CANYON	= 520
		,TANOBY_RUINS	= 521
		,ONE_ISLAND	= 526
		,FOUR_ISLAND	= 527
		,FIVE_ISLAND	= 528
		,KANTO_ALTERING_CAVE	= 529
		,ASPERTIA_CITY	= 531
		,VIRBANK_CITY	= 532
		,HUMILAU_CITY	= 533
		,POKESTAR_STUDIOS	= 534
		,JOIN_AVENUE	= 535
		,FLOCCESY_TOWN	= 536
		,LENTIMAS_TOWN	= 537
		,ROUTE_19	= 538
		,ROUTE_20	= 539
		,ROUTE_21	= 540
		,ROUTE_22	= 541
		,ROUTE_23	= 542
		,CASTELIA_SEWERS	= 543
		,FLOCCESY_RANCH	= 544
		,VIRBANK_COMPLEX	= 545
		,REVERSAL_MOUNTAIN	= 546
		,STRANGE_HOUSE	= 547
		,UNOVA_VICTORY_ROAD_2	= 548
		,PLASMA_FRIGATE	= 549
		,RELIC_PASSAGE	= 550
		,CLAY_TUNNEL	= 551
		,WHITE_TREEHOLLOW	= 552
		,BLACK_TOWER	= 553
		,SEASIDE_CAVE	= 554
		,CAVE_OF_BEING	= 555
		,HIDDEN_GROTTO	= 556
		,MARINE_TUBE	= 557
		,VIRBANK_GATE	= 558
		,ASPERTIA_GATE	= 559
		,NATURE_SANCTUARY	= 560
		,MEDAL_SECRETARIAT	= 561
		,UNDERGROUND_RUINS	= 562
		,ROCKY_MOUNTAIN_ROOM	= 563
		,GLACIER_ROOM	= 564
		,IRON_ROOM	= 565
		,PLEDGE_GROVE	= 566
		,LITTLEROOT_TOWN	= 567
		,OLDALE_TOWN	= 568
		,LAVARIDGE_TOWN	= 569
		,FALLARBOR_TOWN	= 570
		,VERDANTURF_TOWN	= 571
		,MAUVILLE_CITY	= 572
		,RUSTBORO_CITY	= 573
		,FORTREE_CITY	= 574
		,UNDERWATER	= 575
		,MT_CHIMNEY	= 576
		,MIRAGE_ISLAND	= 577
		,SOUTHERN_ISLAND	= 578
		,SEALED_CHAMBER	= 579
		,SCORCHED_SLAB	= 580
		,ISLAND_CAVE	= 581
		,DESERT_RUINS	= 582
		,ANCIENT_TOMB	= 583
		,INSIDE_OF_TRUCK	= 584
		,SECRET_BASE	= 585
		,HOENN_BATTLE_TOWER	= 586
		,VANIVILLE_TOWN	= 587
		,KALOS_ROUTE_1	= 588
		,VANIVILLE_PATHWAY	= 589
		,AQUACORDE_TOWN	= 590
		,KALOS_ROUTE_2	= 591
		,AVANCE_TRAIL	= 592
		,SANTALUNE_FOREST	= 593
		,KALOS_ROUTE_3	= 594
		,OUVERT_WAY	= 595
		,SANTALUNE_CITY	= 596
		,KALOS_ROUTE_4	= 597
		,PARTERRE_WAY	= 598
		,LUMIOSE_CITY	= 599
		,PRISM_TOWER	= 600
		,LYSANDRE_LABS	= 601
		,KALOS_ROUTE_5	= 602
		,VERSANT_ROAD	= 603
		,CAMPHRIER_TOWN	= 604
		,SHABBONEAU_CASTLE	= 605
		,KALOS_ROUTE_6	= 606
		,PALAIS_LANE	= 607
		,PARFUM_PALACE	= 608
		,KALOS_ROUTE_7	= 609
		,RIVIèRE_WALK	= 610
		,CYLLAGE_CITY	= 611
		,KALOS_ROUTE_8	= 612
		,MURAILLE_COAST	= 613
		,AMBRETTE_TOWN	= 614
		,KALOS_ROUTE_9	= 615
		,SPIKES_PASSAGE	= 616
		,BATTLE_CHATEAU	= 617
		,KALOS_ROUTE_10	= 618
		,MENHIR_TRAIL	= 619
		,GEOSENGE_TOWN	= 620
		,KALOS_ROUTE_11	= 621
		,MIROIR_WAY	= 622
		,REFLECTION_CAVE	= 623
		,SHALOUR_CITY	= 624
		,TOWER_OF_MASTERY	= 625
		,KALOS_ROUTE_12	= 626
		,FOURRAGE_ROAD	= 627
		,COUMARINE_CITY	= 628
		,KALOS_ROUTE_13	= 629
		,LUMIOSE_BADLANDS	= 630
		,KALOS_ROUTE_14	= 631
		,LAVERRE_NATURE_TRAIL	= 632
		,LAVERRE_CITY	= 633
		,POKE_BALL_FACTORY	= 634
		,KALOS_ROUTE_15	= 635
		,BRUN_WAY	= 636
		,DENDEMILLE_TOWN	= 637
		,KALOS_ROUTE_16	= 638
		,MELANCOLIE_PATH	= 639
		,FROST_CAVERN	= 640
		,KALOS_ROUTE_17	= 641
		,MAMOSWINE_ROAD	= 642
		,ANISTAR_CITY	= 643
		,KALOS_ROUTE_18	= 644
		,VALLEE_ETROITE_WAY	= 645
		,COURIWAY_TOWN	= 646
		,KALOS_ROUTE_19	= 647
		,GRANDE_VALLEE_WAY	= 648
		,SNOWBELLE_CITY	= 649
		,KALOS_ROUTE_20	= 650
		,WINDING_WOODS	= 651
		,POKEMON_VILLAGE	= 652
		,KALOS_ROUTE_21	= 653
		,DERNIèRE_WAY	= 654
		,KALOS_ROUTE_22	= 655
		,DETOURNER_WAY	= 656
		,VICTORY_ROAD	= 657
		,POKEMON_LEAGUE	= 658
		,KILOUDE_CITY	= 659
		,BATTLE_MAISON	= 660
		,AZURE_BAY	= 661
		,DENDEMILLE_GATE	= 662
		,COURIWAY_GATE	= 663
		,AMBRETTE_GATE	= 664
		,LUMIOSE_GATE	= 665
		,SHALOUR_GATE	= 666
		,COUMARINE_GATE	= 667
		,LAVERRE_GATE	= 668
		,ANISTAR_GATE	= 669
		,SNOWBELLE_GATE	= 670
		,GLITTERING_CAVE	= 671
		,CONNECTING_CAVE	= 672
		,ZUBAT_ROOST	= 673
		,KALOS_POWER_PLANT	= 674
		,TEAM_FLARE_SECRET_HQ	= 675
		,TERMINUS_CAVE	= 676
		,LOST_HOTEL	= 677
		,CHAMBER_OF_EMPTINESS	= 678
		,SEA_SPIRITS_DEN	= 679
		,FRIEND_SAFARI	= 680
		,BLAZING_CHAMBER	= 681
		,FLOOD_CHAMBER	= 682
		,IRONWORKS_CHAMBER	= 683
		,DRAGONMARK_CHAMBER	= 684
		,RADIANT_CHAMBER	= 685
		,POKEMON_LEAGUE_GATE	= 686
		,LUMIOSE_STATION	= 687
		,KILOUDE_STATION	= 688
		,AMBRETTE_AQUARIUM	= 689
		,UNKNOWN_DUNGEON	= 690
	}
}