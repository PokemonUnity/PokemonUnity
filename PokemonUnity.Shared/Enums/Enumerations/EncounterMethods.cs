namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Encounter method
	/// </summary>
	public class EncounterMethods : Enumeration
	{
		protected EncounterMethods(int id, string name) : base(id, name) { }
		/// <summary>
		/// Walking in tall grass or a cave
		/// </summary>
		public static readonly EncounterMethods WALK			= new EncounterMethods(1,	"WALK");
		/// <summary>
		/// Walking in rustling grass
		/// </summary>
		public static readonly EncounterMethods GRASS_SPOTS			= new EncounterMethods(9,	"GRASS_SPOTS");
		/// <summary>
		/// Walking in dust clouds
		/// </summary>
		public static readonly EncounterMethods CAVE_SPOTS			= new EncounterMethods(10,	"CAVE_SPOTS");
		/// <summary>
		/// Walking in bridge shadows
		/// </summary>
		public static readonly EncounterMethods BRIDGE_SPOTS			= new EncounterMethods(11,	"BRIDGE_SPOTS");
		/// <summary>
		/// Walking in dark grass
		/// </summary>
		public static readonly EncounterMethods DARK_GRASS			= new EncounterMethods(8,	"DARK_GRASS");
		/// <summary>
		/// Walking in yellow flowers
		/// </summary>
		public static readonly EncounterMethods YELLOW_FLOWERS			= new EncounterMethods(14,	"YELLOW_FLOWERS");
		/// <summary>
		/// Walking in purple flowers
		/// </summary>
		public static readonly EncounterMethods PURPLE_FLOWERS			= new EncounterMethods(15,	"PURPLE_FLOWERS");
		/// <summary>
		/// Walking in red flowers
		/// </summary>
		public static readonly EncounterMethods RED_FLOWERS			= new EncounterMethods(16,	"RED_FLOWERS");
		/// <summary>
		/// Walking on rough terrain
		/// </summary>
		public static readonly EncounterMethods ROUGH_TERRAIN			= new EncounterMethods(17,	"ROUGH_TERRAIN");
		/// <summary>
		/// Fishing with an <see cref="Items.OLD_ROD"/>
		/// </summary>
		public static readonly EncounterMethods OLD_ROD			= new EncounterMethods(2,	"OLD_ROD");
		/// <summary>
		/// Fishing with a <see cref="Items.GOOD_ROD"/> 
		/// </summary>
		public static readonly EncounterMethods GOOD_ROD			= new EncounterMethods(3,	"GOOD_ROD");
		/// <summary>
		/// Fishing with a <see cref="Items.SUPER_ROD"/> 
		/// </summary>
		public static readonly EncounterMethods SUPER_ROD			= new EncounterMethods(4,	"SUPER_ROD");
		/// <summary>
		/// Fishing in dark spots
		/// </summary>
		public static readonly EncounterMethods SUPER_ROD_SPOTS			= new EncounterMethods(12,	"SUPER_ROD_SPOTS");
		/// <summary>
		/// Surfing
		/// </summary>
		public static readonly EncounterMethods SURF			= new EncounterMethods(5,	"SURF");
		/// <summary>
		/// Surfing in dark spots
		/// </summary>
		public static readonly EncounterMethods SURF_SPOTS			= new EncounterMethods(13,	"SURF_SPOTS");
		/// <summary>
		/// Smashing rocks
		/// </summary>
		public static readonly EncounterMethods ROCK_SMASH			= new EncounterMethods(6,	"ROCK_SMASH");
		/// <summary>
		/// Headbutting trees
		/// </summary>
		public static readonly EncounterMethods HEADBUTT			= new EncounterMethods(7,	"HEADBUTT");
		/// <summary>
		/// Receive as a gift
		/// </summary>
		public static readonly EncounterMethods GIFT			= new EncounterMethods(18,	"GIFT");
		/// <summary>
		/// Receive egg as a gift
		/// </summary>
		public static readonly EncounterMethods GIFT_EGG			= new EncounterMethods(19,	"GIFT_EGG");
	}
}