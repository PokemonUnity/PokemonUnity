using System.Collections;
using PokemonUnity;
using PokemonUnity.Inventory;

namespace PokemonUnity.Inventory
{
	public struct ItemData
	{
		#region Variables
		//public string Name { get; private set; }
		//public string Plural { get; private set; }
		//public string Description { get; private set; }
		public int Price { get; private set; }
		//public string Use { get; private set; }
		/// <summary>
		/// Should be a description of what it does when in battle,
		/// but might be better as a bool
		/// </summary>
		public bool BattleUse { get { return this.Flags.Useable_In_Battle; } }

		public Items Id { get; private set; }
		public ItemFlag Flags { get; private set; }
		public ItemCategory Category { get; private set; }
		public ItemPockets? Pocket
		{
			get
			{
				ItemPockets? itemPocket;
				switch (this.Category)
				{//([\w]*) = [\d]*, //PocketId = ([\d]*)
					case ItemCategory.COLLECTIBLES:     //itemPocket = (ItemPockets)1; break;
					case ItemCategory.EVOLUTION:        //itemPocket = (ItemPockets)1; break;
					case ItemCategory.SPELUNKING:       //itemPocket = (ItemPockets)1; break;
					case ItemCategory.HELD_ITEMS:       //itemPocket = (ItemPockets)1; break;
					case ItemCategory.CHOICE:           //itemPocket = (ItemPockets)1; break;
					case ItemCategory.EFFORT_TRAINING:  //itemPocket = (ItemPockets)1; break;
					case ItemCategory.BAD_HELD_ITEMS:   //itemPocket = (ItemPockets)1; break;
					case ItemCategory.TRAINING:         //itemPocket = (ItemPockets)1; break;
					case ItemCategory.PLATES:           //itemPocket = (ItemPockets)1; break;
					case ItemCategory.SPECIES_SPECIFIC: //itemPocket = (ItemPockets)1; break;
					case ItemCategory.TYPE_ENHANCEMENT: //itemPocket = (ItemPockets)1; break;
					case ItemCategory.LOOT:             //itemPocket = (ItemPockets)1; break;
					case ItemCategory.MULCH:            //itemPocket = (ItemPockets)1; break;
					case ItemCategory.DEX_COMPLETION:   //itemPocket = (ItemPockets)1; break;
					case ItemCategory.SCARVES:          //itemPocket = (ItemPockets)1; break;
					case ItemCategory.JEWELS:           //itemPocket = (ItemPockets)1; break;
					case ItemCategory.MEGA_STONES: itemPocket = (ItemPockets)1; break;

					case ItemCategory.VITAMINS:         //itemPocket = (ItemPockets)2; break;
					case ItemCategory.HEALING:          //itemPocket = (ItemPockets)2; break;
					case ItemCategory.PP_RECOVERY:      //itemPocket = (ItemPockets)2; break;
					case ItemCategory.REVIVAL:          //itemPocket = (ItemPockets)2; break;
					case ItemCategory.STATUS_CURES: itemPocket = (ItemPockets)2; break;

					case ItemCategory.SPECIAL_BALLS:    //itemPocket = (ItemPockets)3; break;
					case ItemCategory.STANDARD_BALLS:   //itemPocket = (ItemPockets)3; break;
					case ItemCategory.APRICORN_BALLS: itemPocket = (ItemPockets)3; break;

					case ItemCategory.ALL_MACHINES: itemPocket = (ItemPockets)4; break;

					case ItemCategory.EFFORT_DROP:      //itemPocket = (ItemPockets)5; break;
					case ItemCategory.MEDICINE:         //itemPocket = (ItemPockets)5; break;
					case ItemCategory.OTHER:            //itemPocket = (ItemPockets)5; break;
					case ItemCategory.IN_A_PINCH:       //itemPocket = (ItemPockets)5; break;
					case ItemCategory.PICKY_HEALING:    //itemPocket = (ItemPockets)5; break;
					case ItemCategory.TYPE_PROTECTION:  //itemPocket = (ItemPockets)5; break;
					case ItemCategory.BAKING_ONLY: itemPocket = (ItemPockets)5; break;

					case ItemCategory.ALL_MAIL: itemPocket = (ItemPockets)6; break;

					case ItemCategory.STAT_BOOSTS:      //itemPocket = (ItemPockets)7; break;
					case ItemCategory.FLUTES:           //itemPocket = (ItemPockets)7; break;
					case ItemCategory.MIRACLE_SHOOTER: itemPocket = (ItemPockets)7; break;

					case ItemCategory.EVENT_ITEMS:      //itemPocket = (ItemPockets)8; break;
					case ItemCategory.GAMEPLAY:         //itemPocket = (ItemPockets)8; break;
					case ItemCategory.PLOT_ADVANCEMENT: //itemPocket = (ItemPockets)8; break;
					case ItemCategory.UNUSED:           //itemPocket = (ItemPockets)8; break;
					case ItemCategory.APRICORN_BOX:     //itemPocket = (ItemPockets)8; break;
					case ItemCategory.DATA_CARDS:       //itemPocket = (ItemPockets)8; break;
					case ItemCategory.XY_UNKNOWN: itemPocket = (ItemPockets)8; break;
					default:
						itemPocket = null; //ToDo: MISC?
						break;
				}
				return itemPocket;
			}
		}
		public ItemFlingEffect FlingEffect { get; private set; }
		public int? FlingPower { get; private set; }
		public int[] Generations { get; private set; }

		public bool IsLetter 
		{ 
			get 
			{
				return pbIsLetter(Id);
			} 
		}
		public bool IsPokeBall
		{ 
			get 
			{
				return pbIsPokeBall(Id);
			} 
		}
		public bool IsBerry 
		{ 
			get 
			{
				return pbIsBerry(Id);
			} 
		}
		public bool IsApricon 
		{ 
			get 
			{
				return pbIsApricon(Id);
			}
		}
		public bool IsMegaStone
		{
			get
			{
				return pbIsMegaStone(Id);
			}
		}
		public bool IsGem
		{
			get
			{
				return pbIsGem(Id);
			}
		}

		public string Name { get { return ToString(TextScripts.Name); } }
		public string Description { get { return ToString(TextScripts.Description); } }
		public string Plural { get { return ToString(TextScripts.Name); } }
		#endregion

		#region Constructor
		//private Item(Items itemId, ItemCategory itemCategory = ItemCategory.UNUSED, /*BattleType battleType, string description,*/ int price = 0, int? flingPower = null,
		//	ItemFlingEffect? itemEffect = null, /*string stringParameter, float floatParameter,*/ ItemFlags flags = null)
		//{
		//	ItemId = itemId;
		//	this.Price = item.Price;
		//	this.ItemCategory = item.ItemCategory;
		//	this.ItemFlag = item.ItemFlag;
		//	this.ItemFlingEffect = item.ItemFlingEffect;
		//	//this.name = name;
		//	//this.itemType = itemType;
		//	//this.battleType = battleType;
		//	//this.description = description;
		//	this.Price = price;
		//	if (itemEffect.HasValue) this.ItemFlingEffect = itemEffect.Value;
		//	//this.stringParameter = stringParameter;
		//	//this.floatParameter = floatParameter;
		//	Mail m = new Mail(itemId);
		//	if (m.IsLetter) this.mail = m;
		//}

		public ItemData(Items itemId, int price = 0, ItemCategory itemCategory = ItemCategory.UNUSED, ItemFlag itemFlag = new ItemFlag(), int? power = 0, ItemFlingEffect? itemFlingEffect = null, int[] gens = null) //: this()
		{
			Id = itemId;
			Price = price;
			Flags = itemFlag;
			Category = itemCategory;
			FlingPower = power;
			if (itemFlingEffect.HasValue) this.FlingEffect = itemFlingEffect.Value;
			else FlingEffect = ItemFlingEffect.NONE;
			//this.Name				= ToDo: load from translation
			//this.Plural			= ToDo: load from translation
			//this.Description		= ToDo: load from translation
			Generations = gens ?? new int[] { };
		}
		#endregion

		#region Methods
		public static bool pbIsPokeBall(Items item)
		{
			return item == Items.BEAST_BALL
				|| item == Items.CHERISH_BALL
				|| item == Items.DIVE_BALL
				|| item == Items.DREAM_BALL
				|| item == Items.DUSK_BALL
				|| item == Items.FAST_BALL
				|| item == Items.FRIEND_BALL
				|| item == Items.GREAT_BALL
				|| item == Items.HEAL_BALL
				|| item == Items.HEAVY_BALL
				|| item == Items.IRON_BALL
				|| item == Items.LEVEL_BALL
				|| item == Items.LIGHT_BALL
				|| item == Items.LOVE_BALL
				|| item == Items.LURE_BALL
				|| item == Items.LUXURY_BALL
				|| item == Items.MASTER_BALL
				|| item == Items.MOON_BALL
				|| item == Items.NEST_BALL
				|| item == Items.NET_BALL
				|| item == Items.PARK_BALL
				|| item == Items.POKE_BALL
				|| item == Items.PREMIER_BALL
				|| item == Items.QUICK_BALL
				|| item == Items.REPEAT_BALL
				|| item == Items.SAFARI_BALL
				|| item == Items.SMOKE_BALL
				|| item == Items.SPORT_BALL
				|| item == Items.TIMER_BALL
				|| item == Items.ULTRA_BALL;
		}
		public static bool pbIsLetter(Items item)
		{
			return item == Items.AIR_MAIL
				|| item == Items.BEAD_MAIL
				|| item == Items.BLOOM_MAIL
				|| item == Items.BRICK_MAIL
				|| item == Items.BRIDGE_MAIL_D
				|| item == Items.BRIDGE_MAIL_M
				|| item == Items.BRIDGE_MAIL_S
				|| item == Items.BRIDGE_MAIL_T
				|| item == Items.BRIDGE_MAIL_V
				|| item == Items.BUBBLE_MAIL
				|| item == Items.DREAM_MAIL
				|| item == Items.FAB_MAIL
				|| item == Items.FAVORED_MAIL
				|| item == Items.FLAME_MAIL
				|| item == Items.GLITTER_MAIL
				|| item == Items.GRASS_MAIL
				|| item == Items.GREET_MAIL
				|| item == Items.HARBOR_MAIL
				|| item == Items.HEART_MAIL
				|| item == Items.INQUIRY_MAIL
				|| item == Items.LIKE_MAIL
				|| item == Items.MECH_MAIL
				|| item == Items.MOSAIC_MAIL
				|| item == Items.ORANGE_MAIL
				|| item == Items.REPLY_MAIL
				|| item == Items.RETRO_MAIL
				|| item == Items.RSVP_MAIL
				|| item == Items.SHADOW_MAIL
				|| item == Items.SNOW_MAIL
				|| item == Items.SPACE_MAIL
				|| item == Items.STEEL_MAIL
				|| item == Items.THANKS_MAIL
				|| item == Items.TROPIC_MAIL
				|| item == Items.TUNNEL_MAIL
				|| item == Items.WAVE_MAIL
				|| item == Items.WOOD_MAIL;
		}
		public static bool pbIsApricon(Items item)
		{
			return item == Items.BLACK_APRICORN
				|| item == Items.BLUE_APRICORN
				|| item == Items.GREEN_APRICORN
				|| item == Items.PINK_APRICORN
				|| item == Items.RED_APRICORN
				|| item == Items.WHITE_APRICORN
				|| item == Items.YELLOW_APRICORN;
		}
		public static bool pbIsBerry(Items item)
		{
			return item == Items.AGUAV_BERRY
				|| item == Items.APICOT_BERRY
				|| item == Items.ASPEAR_BERRY
				|| item == Items.BABIRI_BERRY
				|| item == Items.BELUE_BERRY
				|| item == Items.BLUK_BERRY
				|| item == Items.CHARTI_BERRY
				|| item == Items.CHERI_BERRY
				|| item == Items.CHESTO_BERRY
				|| item == Items.CHILAN_BERRY
				|| item == Items.CHOPLE_BERRY
				|| item == Items.COBA_BERRY
				|| item == Items.COLBUR_BERRY
				|| item == Items.CORNN_BERRY
				|| item == Items.CUSTAP_BERRY
				|| item == Items.DURIN_BERRY
				|| item == Items.ENIGMA_BERRY
				|| item == Items.FIGY_BERRY
				|| item == Items.GANLON_BERRY
				|| item == Items.GREPA_BERRY
				|| item == Items.HABAN_BERRY
				|| item == Items.HONDEW_BERRY
				|| item == Items.IAPAPA_BERRY
				|| item == Items.JABOCA_BERRY
				|| item == Items.KASIB_BERRY
				|| item == Items.KEBIA_BERRY
				|| item == Items.KEE_BERRY
				|| item == Items.KELPSY_BERRY
				|| item == Items.LANSAT_BERRY
				|| item == Items.LEPPA_BERRY
				|| item == Items.LIECHI_BERRY
				|| item == Items.LUM_BERRY
				|| item == Items.MAGOST_BERRY
				|| item == Items.MAGO_BERRY
				|| item == Items.MARANGA_BERRY
				|| item == Items.MICLE_BERRY
				|| item == Items.NANAB_BERRY
				|| item == Items.NOMEL_BERRY
				|| item == Items.OCCA_BERRY
				|| item == Items.ORAN_BERRY
				|| item == Items.PAMTRE_BERRY
				|| item == Items.PASSHO_BERRY
				|| item == Items.PAYAPA_BERRY
				|| item == Items.PECHA_BERRY
				|| item == Items.PERSIM_BERRY
				|| item == Items.PETAYA_BERRY
				|| item == Items.PINAP_BERRY
				|| item == Items.POMEG_BERRY
				|| item == Items.QUALOT_BERRY
				|| item == Items.RABUTA_BERRY
				|| item == Items.RAWST_BERRY
				|| item == Items.RAZZ_BERRY
				|| item == Items.RINDO_BERRY
				|| item == Items.ROSELI_BERRY
				|| item == Items.ROWAP_BERRY
				|| item == Items.SALAC_BERRY
				|| item == Items.SHUCA_BERRY
				|| item == Items.SITRUS_BERRY
				|| item == Items.SPELON_BERRY
				|| item == Items.STARF_BERRY
				|| item == Items.TAMATO_BERRY
				|| item == Items.TANGA_BERRY
				|| item == Items.WACAN_BERRY
				|| item == Items.WATMEL_BERRY
				|| item == Items.WEPEAR_BERRY
				|| item == Items.WIKI_BERRY
				|| item == Items.YACHE_BERRY;
		}
		public static bool pbIsGem(Items item)
		{
			return item == Items.FIRE_GEM
				|| item == Items.WATER_GEM
				|| item == Items.ELECTRIC_GEM
				|| item == Items.GRASS_GEM
				|| item == Items.ICE_GEM
				|| item == Items.FIGHTING_GEM
				|| item == Items.POISON_GEM
				|| item == Items.GROUND_GEM
				|| item == Items.FLYING_GEM
				|| item == Items.PSYCHIC_GEM
				|| item == Items.BUG_GEM
				|| item == Items.ROCK_GEM
				|| item == Items.GHOST_GEM
				|| item == Items.DRAGON_GEM
				|| item == Items.DARK_GEM
				|| item == Items.STEEL_GEM
				|| item == Items.NORMAL_GEM
				|| item == Items.FAIRY_GEM;
		}
		public static bool pbIsMegaStone(Items item)
		{
			return item == Items.ABOMASITE
				|| item == Items.ABSOLITE
				|| item == Items.AERODACTYLITE
				|| item == Items.AGGRONITE
				|| item == Items.ALAKAZITE
				|| item == Items.ALTARIANITE
				|| item == Items.AMPHAROSITE
				|| item == Items.AUDINITE
				|| item == Items.BANETTITE
				|| item == Items.BEEDRILLITE
				|| item == Items.BLASTOISINITE
				|| item == Items.BLAZIKENITE
				|| item == Items.CAMERUPTITE
				|| item == Items.CHARIZARDITE_X
				|| item == Items.CHARIZARDITE_Y
				|| item == Items.DIANCITE
				|| item == Items.GLALITITE
				|| item == Items.GYARADOSITE
				|| item == Items.HERACRONITE
				|| item == Items.HOUNDOOMINITE
				|| item == Items.KANGASKHANITE
				|| item == Items.LATIASITE
				|| item == Items.LATIOSITE
				|| item == Items.LOPUNNITE
				|| item == Items.LUCARIONITE
				|| item == Items.MANECTITE
				|| item == Items.MAWILITE
				|| item == Items.MEDICHAMITE
				|| item == Items.METAGROSSITE
				|| item == Items.MEWTWONITE_X
				|| item == Items.MEWTWONITE_Y
				|| item == Items.PIDGEOTITE
				|| item == Items.PINSIRITE
				|| item == Items.SABLENITE
				|| item == Items.SALAMENCITE
				|| item == Items.SCEPTILITE
				|| item == Items.SCIZORITE
				|| item == Items.SHARPEDONITE
				|| item == Items.SLOWBRONITE
				|| item == Items.STEELIXITE
				|| item == Items.SWAMPERTITE
				|| item == Items.TYRANITARITE
				|| item == Items.VENUSAURITE;
		}
		#endregion

		public string ToString(TextScripts text)
		{
			return Id.ToString(text);
		}
	}
}