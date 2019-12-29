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
						itemPocket = null;
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
				switch (Id)
				{
					case Items.AIR_MAIL:
					case Items.BEAD_MAIL:
					case Items.BLOOM_MAIL:
					case Items.BRICK_MAIL:
					case Items.BRIDGE_MAIL_D:
					case Items.BRIDGE_MAIL_M:
					case Items.BRIDGE_MAIL_S:
					case Items.BRIDGE_MAIL_T:
					case Items.BRIDGE_MAIL_V:
					case Items.BUBBLE_MAIL:
					case Items.DREAM_MAIL:
					case Items.FAB_MAIL:
					case Items.FAVORED_MAIL:
					case Items.FLAME_MAIL:
					case Items.GLITTER_MAIL:
					case Items.GRASS_MAIL:
					case Items.GREET_MAIL:
					case Items.HARBOR_MAIL:
					case Items.HEART_MAIL:
					case Items.INQUIRY_MAIL:
					case Items.LIKE_MAIL:
					case Items.MECH_MAIL:
					case Items.MOSAIC_MAIL:
					case Items.ORANGE_MAIL:
					case Items.REPLY_MAIL:
					case Items.RETRO_MAIL:
					case Items.RSVP_MAIL:
					case Items.SHADOW_MAIL:
					case Items.SNOW_MAIL:
					case Items.SPACE_MAIL:
					case Items.STEEL_MAIL:
					case Items.THANKS_MAIL:
					case Items.TROPIC_MAIL:
					case Items.TUNNEL_MAIL:
					case Items.WAVE_MAIL:
					case Items.WOOD_MAIL:
						return true;
					default:
						return false;
				}
			} 
		}
		public bool IsBerry 
		{ 
			get 
			{
				switch (Id)
				{
					case Items.AGUAV_BERRY:
					case Items.APICOT_BERRY:
					case Items.ASPEAR_BERRY:
					case Items.BABIRI_BERRY:
					case Items.BELUE_BERRY:
					case Items.BLUK_BERRY:
					case Items.CHARTI_BERRY:
					case Items.CHERI_BERRY:
					case Items.CHESTO_BERRY:
					case Items.CHILAN_BERRY:
					case Items.CHOPLE_BERRY:
					case Items.COBA_BERRY:
					case Items.COLBUR_BERRY:
					case Items.CORNN_BERRY:
					case Items.CUSTAP_BERRY:
					case Items.DURIN_BERRY:
					case Items.ENIGMA_BERRY:
					case Items.FIGY_BERRY:
					case Items.GANLON_BERRY:
					case Items.GREPA_BERRY:
					case Items.HABAN_BERRY:
					case Items.HONDEW_BERRY:
					case Items.IAPAPA_BERRY:
					case Items.JABOCA_BERRY:
					case Items.KASIB_BERRY:
					case Items.KEBIA_BERRY:
					case Items.KEE_BERRY:
					case Items.KELPSY_BERRY:
					case Items.LANSAT_BERRY:
					case Items.LEPPA_BERRY:
					case Items.LIECHI_BERRY:
					case Items.LUM_BERRY:
					case Items.MAGOST_BERRY:
					case Items.MAGO_BERRY:
					case Items.MARANGA_BERRY:
					case Items.MICLE_BERRY:
					case Items.NANAB_BERRY:
					case Items.NOMEL_BERRY:
					case Items.OCCA_BERRY:
					case Items.ORAN_BERRY:
					case Items.PAMTRE_BERRY:
					case Items.PASSHO_BERRY:
					case Items.PAYAPA_BERRY:
					case Items.PECHA_BERRY:
					case Items.PERSIM_BERRY:
					case Items.PETAYA_BERRY:
					case Items.PINAP_BERRY:
					case Items.POMEG_BERRY:
					case Items.QUALOT_BERRY:
					case Items.RABUTA_BERRY:
					case Items.RAWST_BERRY:
					case Items.RAZZ_BERRY:
					case Items.RINDO_BERRY:
					case Items.ROSELI_BERRY:
					case Items.ROWAP_BERRY:
					case Items.SALAC_BERRY:
					case Items.SHUCA_BERRY:
					case Items.SITRUS_BERRY:
					case Items.SPELON_BERRY:
					case Items.STARF_BERRY:
					case Items.TAMATO_BERRY:
					case Items.TANGA_BERRY:
					case Items.WACAN_BERRY:
					case Items.WATMEL_BERRY:
					case Items.WEPEAR_BERRY:
					case Items.WIKI_BERRY:
					case Items.YACHE_BERRY:
						return true;
					default:
						return false;
				}
			} 
		}
		public bool IsApricon 
		{ 
			get 
			{
				switch (Id)
				{
					case Items.BLACK_APRICORN:
					case Items.BLUE_APRICORN:
					case Items.GREEN_APRICORN:
					case Items.PINK_APRICORN:
					case Items.RED_APRICORN:
					case Items.WHITE_APRICORN:
					case Items.YELLOW_APRICORN:
						return true;
					default:
						return false;
				}
			} 
		}
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
	}
}