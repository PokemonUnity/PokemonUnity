namespace PokemonUnity.Shared.Enums
{
	/// <summary>
	/// Item Category determines both the item's effect
	/// and the bag-pocket that the item belongs to.
	/// </summary>
	/// <remarks>
	/// Can determine an <see cref="itemEffect"/> by the category it belongs to.
	/// </remarks>
	public class ItemCategory : Enumeration
	{
		public int PocketId { get; private set; }
		protected ItemCategory(int id, int pocket, string name) : base(id, name) { PocketId = pocket; }
		public static readonly ItemCategory COLLECTIBLES			= new ItemCategory(9,		1,	"COLLECTIBLES"); //PocketId = 1
		public static readonly ItemCategory EVOLUTION				= new ItemCategory(10,		1,	"EVOLUTION"); //PocketId = 1
		public static readonly ItemCategory SPELUNKING				= new ItemCategory(11,		1,	"SPELUNKING"); //PocketId = 1
		public static readonly ItemCategory HELD_ITEMS				= new ItemCategory(12,		1,	"HELD_ITEMS"); //PocketId = 1
		public static readonly ItemCategory CHOICE					= new ItemCategory(13,		1,	"CHOICE"); //PocketId = 1
		public static readonly ItemCategory EFFORT_TRAINING			= new ItemCategory(14,		1,	"EFFORT_TRAINING"); //PocketId = 1
		public static readonly ItemCategory BAD_HELD_ITEMS			= new ItemCategory(15,		1,	"BAD_HELD_ITEMS"); //PocketId = 1
		public static readonly ItemCategory TRAINING				= new ItemCategory(16,		1,	"TRAINING"); //PocketId = 1
		public static readonly ItemCategory PLATES					= new ItemCategory(17,		1,	"PLATES"); //PocketId = 1
		public static readonly ItemCategory SPECIES_SPECIFIC		= new ItemCategory(18,		1,	"SPECIES_SPECIFIC"); //PocketId = 1
		public static readonly ItemCategory TYPE_ENHANCEMENT		= new ItemCategory(19,		1,	"TYPE_ENHANCEMENT"); //PocketId = 1
		public static readonly ItemCategory LOOT					= new ItemCategory(24,		1,	"LOOT"); //PocketId = 1
		public static readonly ItemCategory MULCH					= new ItemCategory(32,		1,	"MULCH"); //PocketId = 1
		public static readonly ItemCategory DEX_COMPLETION			= new ItemCategory(35,		1,	"DEX_COMPLETION"); //PocketId = 1
		public static readonly ItemCategory SCARVES					= new ItemCategory(36,		1,	"SCARVES"); //PocketId = 1
		public static readonly ItemCategory JEWELS					= new ItemCategory(42,		1,	"JEWELS"); //PocketId = 1
		public static readonly ItemCategory MEGA_STONES				= new ItemCategory(44,		1,	"MEGA_STONES"); //PocketId = 1
																									
		public static readonly ItemCategory VITAMINS				= new ItemCategory(26,		2,	"VITAMINS"); //PocketId = 2
		public static readonly ItemCategory HEALING					= new ItemCategory(27,		2,	"HEALING"); //PocketId = 2
		public static readonly ItemCategory PP_RECOVERY				= new ItemCategory(28,		2,	"PP_RECOVERY"); //PocketId = 2
		public static readonly ItemCategory REVIVAL					= new ItemCategory(29,		2,	"REVIVAL"); //PocketId = 2
		public static readonly ItemCategory STATUS_CURES			= new ItemCategory(30,		2,	"STATUS_CURES"); //PocketId = 2
																									
		public static readonly ItemCategory SPECIAL_BALLS			= new ItemCategory(33,		3,	"SPECIAL_BALLS"); //PocketId = 3
		public static readonly ItemCategory STANDARD_BALLS			= new ItemCategory(34,		3,	"STANDARD_BALLS"); //PocketId = 3
		public static readonly ItemCategory APRICORN_BALLS			= new ItemCategory(39,		3,	"APRICORN_BALLS"); //PocketId = 3
																									
		public static readonly ItemCategory ALL_MACHINES			= new ItemCategory(37,		4,	"ALL_MACHINES"); //PocketId = 4
																									
		public static readonly ItemCategory EFFORT_DROP				= new ItemCategory(2,		5,	"EFFORT_DROP"); //PocketId = 5
		public static readonly ItemCategory MEDICINE				= new ItemCategory(3,		5,	"MEDICINE"); //PocketId = 5
		public static readonly ItemCategory OTHER					= new ItemCategory(4,		5,	"OTHER"); //PocketId = 5
		public static readonly ItemCategory IN_A_PINCH				= new ItemCategory(5,		5,	"IN_A_PINCH"); //PocketId = 5
		public static readonly ItemCategory PICKY_HEALING			= new ItemCategory(6,		5,	"PICKY_HEALING"); //PocketId = 5
		public static readonly ItemCategory TYPE_PROTECTION			= new ItemCategory(7,		5,	"TYPE_PROTECTION"); //PocketId = 5
		public static readonly ItemCategory BAKING_ONLY				= new ItemCategory(8,		5,	"BAKING_ONLY"); //PocketId = 5
																									
		public static readonly ItemCategory ALL_MAIL				= new ItemCategory(25,		6,	"ALL_MAIL"); //PocketId = 6
																									
		public static readonly ItemCategory STAT_BOOSTS				= new ItemCategory(1,		7,	"STAT_BOOSTS"); //PocketId = 7
		public static readonly ItemCategory FLUTES					= new ItemCategory(38,		7,	"FLUTES"); //PocketId = 7
		public static readonly ItemCategory MIRACLE_SHOOTER			= new ItemCategory(43,		7,	"MIRACLE_SHOOTER"); //PocketId = 7
																									
		public static readonly ItemCategory EVENT_ITEMS				= new ItemCategory(20,		8,	"EVENT_ITEMS"); //PocketId = 8
		public static readonly ItemCategory GAMEPLAY				= new ItemCategory(21,		8,	"GAMEPLAY"); //PocketId = 8
		public static readonly ItemCategory PLOT_ADVANCEMENT		= new ItemCategory(22,		8,	"PLOT_ADVANCEMENT"); //PocketId = 8
		public static readonly ItemCategory UNUSED					= new ItemCategory(23,		8,	"UNUSED"); //PocketId = 8
		public static readonly ItemCategory APRICORN_BOX			= new ItemCategory(40,		8,	"APRICORN_BOX"); //PocketId = 8
		public static readonly ItemCategory DATA_CARDS				= new ItemCategory(41,		8,	"DATA_CARDS"); //PocketId = 8
		public static readonly ItemCategory XY_UNKNOWN				= new ItemCategory(10001,	8,	"XY_UNKNOWN"); //PocketId = 8
	}
}