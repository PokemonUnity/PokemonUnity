using System.Collections;
using PokemonUnity;
using PokemonUnity.Inventory;

public class PBItems
{
    public static string getName(PokemonUnity.Inventory.Items item)
    {
        return string.Empty;
    }
    //public static int MaxValue { get { return MoveData.Count; } }
}

namespace PokemonUnity.Inventory
{
    public partial class Item
    {
        #region Variables
        public string Name { get; private set; }
        public string Plural { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }
        public string Use { get; private set; }
        /// <summary>
        /// Should be a description of what it does when in battle,
        /// but might be better as a bool
        /// </summary>
        public bool BattleUse { get { return this.ItemFlag.Useable_In_Battle; } }

        public Items ItemId { get; private set; }
        public ItemFlags ItemFlag { get; private set; }
        public ItemCategory ItemCategory { get; private set; }
        public ItemPockets? ItemPocket
        {
            get
            {
                ItemPockets? itemPocket;
                switch (this.ItemCategory)
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
        public ItemFlingEffect ItemFlingEffect { get; private set; }

        #region Mail
		public string MailData { get; set; }
        /// <summary>
        /// Mail?...
        /// </summary>
        private Mail mail { get; set; }
		/// <summary>
		/// Returns message stored on this letter, if item <see cref="IsMail"/>;
        /// If anything other than null, there is a message. 
		/// Try <seealso cref="System.String.IsNullOrEmpty(string)"/>
		/// </summary>
        public string MailText
        {
            get
            {
                if (!IsMail) return null; //If empty return null
                //if (mail.Message.Length == 0 || this.Item == 0)//|| this.item.Category != Items.Category.Mail )
                //{
                //	//mail = null;
                //	return null;
                //}
                //if (Item)
                return mail.Message;
            }
            set
            {
                if (IsMail)
                    mail.Message = value;
            }
        }
        /// <summary>
        /// Performs a check to see if this Item belongs in the 
		/// Mail Pocket, <seealso cref="ItemPockets.MAIL"/>;
		/// and if the item is also a letter, <seealso cref="Mail.IsLetter"/>
        /// </summary
        /// ToDo: Item category
        public bool IsMail
        {
            get
            {
                if ((ItemPocket.HasValue && ItemPocket.Value == ItemPockets.MAIL) && mail.IsLetter) return true;
                else return false;
            }
        }
        #endregion
        #endregion

		#region Constructor
		public Item(Items itemId)
        {
            ItemId = itemId;
            Item item = GetItem(itemId);
            this.Price = item.Price;
            this.ItemCategory = item.ItemCategory;
            this.ItemFlag = item.ItemFlag;
            this.ItemFlingEffect = item.ItemFlingEffect;
			//this.Name				= ToDo: load from translation
			//this.Plural			= ToDo: load from translation
			//this.Description		= ToDo: load from translation
			Mail m = new Mail(itemId);
			if (m.IsLetter) this.mail = m;
        }

        public static Item GetItem(Items item)
        {
            for (int i = 0; i < Database.Length; i++)
            {
                if (Database[i].ItemId == item)
                {
                    return Database[i];
                }
            }
            //Zero is masterball; Items in enum dont match order of database.
            //return Database[0]; 
            return new Item(Items.NONE);
        }

        /// <summary>
        /// Returns int value of Pokemon from PokemonData[] <see cref="Database"/>
        /// </summary>
        public int ArrayId
        {//(Pokemon ID)
            get
            {
                //Debug.Log("Get Pokemons");
                /*PokemonData result = null;
                int i = 1;
                while(result == null){
                    if(Database[i].ID == ID){
                        //Debug.Log("Pokemon DB Success");
                        return result = Database[i];
                    }
                    i += 1;
                    if(i >= Database.Length){
                        Debug.Log("Pokemon DB Fail");
                        return null;}
                }
                return result;*/
                /*foreach(PokemonData pokemon in Database)
                {
                    if (pokemon.ID == ID) return pokemon;
                }*/
                for (int i = 0; i < Database.Length; i++)
                {
                    if (Database[i].ItemId == this.ItemId)
                    {
                        return i;
                    }
                }
                //throw new System.Exception("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.");
                Game.DebugLog("Pokemon ID doesnt exist in the database. Please check PokemonData constructor.", true);
                return -1;
            }
        }

        private Item(Items itemId, ItemCategory itemCategory = ItemCategory.UNUSED, /*BattleType battleType, string description,*/ int price = 0, int? flingPower = null,
            ItemFlingEffect? itemEffect = null, /*string stringParameter, float floatParameter,*/ ItemFlags flags = null)
        {
            //this.name = name;
            //this.itemType = itemType;
            //this.battleType = battleType;
            //this.description = description;
            this.Price = price;
            if (itemEffect.HasValue) this.ItemFlingEffect = itemEffect.Value;
            //this.stringParameter = stringParameter;
            //this.floatParameter = floatParameter;
        }

        //private Item(int itemId, int itemCategory, /*BattleType battleType, string description,;*/ int price, int? flingPower,
        //    int? itemEffect, /*string stringParameter, float floatParameter,*/ int[] flags = null) : this((Items)itemId, (ItemCategory)itemCategory, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (PokemonUnity.Item.ItemFlags)item))
        //{
        //return 
        //new Item((Items)itemId, (ItemCategory)itemCategory, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (ItemFlags)item));
        //}
        #endregion

        #region ItemDatabase
        /// <summary>
        /// Replaces <see cref="oldItems"/>
        /// </summary>
        /// <remarks>
        /// \((\d*)\s*(\d*)\s*(\d*)\s*([\d\w]*)\s*([\d\w]*)\s*
        /// </remarks>
        public static readonly Item[] Database; //= new Item[] {
        static Item()
        {
            Database = new Item[] {
new Item(Items.MASTER_BALL,         ItemCategory.STANDARD_BALLS,    0, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.ULTRA_BALL,          ItemCategory.STANDARD_BALLS,    1200, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.GREAT_BALL,          ItemCategory.STANDARD_BALLS,    600, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.POKE_BALL,           ItemCategory.STANDARD_BALLS,    200, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.SAFARI_BALL,         ItemCategory.STANDARD_BALLS,    0, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.NET_BALL,            ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.DIVE_BALL,           ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.NEST_BALL,           ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.REPEAT_BALL,         ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.TIMER_BALL,          ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.LUXURY_BALL,         ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.PREMIER_BALL,        ItemCategory.SPECIAL_BALLS, 200, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.DUSK_BALL,           ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.HEAL_BALL,           ItemCategory.SPECIAL_BALLS, 300, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.QUICK_BALL,          ItemCategory.SPECIAL_BALLS, 1000, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.CHERISH_BALL,        ItemCategory.SPECIAL_BALLS, 200, null, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.POTION,              ItemCategory.HEALING,           300, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ANTIDOTE,            ItemCategory.STATUS_CURES,  100, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.BURN_HEAL,           ItemCategory.STATUS_CURES,  250, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ICE_HEAL,            ItemCategory.STATUS_CURES,  250, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.AWAKENING,           ItemCategory.STATUS_CURES,  250, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.PARALYZE_HEAL,       ItemCategory.STATUS_CURES,  200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.FULL_RESTORE,        ItemCategory.HEALING,           3000, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.MAX_POTION,          ItemCategory.HEALING,           2500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.HYPER_POTION,        ItemCategory.HEALING,           1200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.SUPER_POTION,        ItemCategory.HEALING,           700, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.FULL_HEAL,           ItemCategory.STATUS_CURES,    600, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.REVIVE,              ItemCategory.REVIVAL,           1500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true,underground: true)),
new Item(Items.MAX_REVIVE,          ItemCategory.REVIVAL,           4000, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true,underground: true)),
new Item(Items.FRESH_WATER,         ItemCategory.HEALING,           200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.SODA_POP,            ItemCategory.HEALING,           300, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.LEMONADE,            ItemCategory.HEALING,           350, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.MOOMOO_MILK,         ItemCategory.HEALING,           500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ENERGY_POWDER,       ItemCategory.HEALING,           500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ENERGY_ROOT,         ItemCategory.HEALING,           800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.HEAL_POWDER,         ItemCategory.STATUS_CURES,    450, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.REVIVAL_HERB,        ItemCategory.REVIVAL,           2800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ETHER,               ItemCategory.PP_RECOVERY,     1200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.MAX_ETHER,           ItemCategory.PP_RECOVERY,     2000, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ELIXIR,              ItemCategory.PP_RECOVERY,     3000, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.MAX_ELIXIR,          ItemCategory.PP_RECOVERY,     4500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.LAVA_COOKIE,         ItemCategory.STATUS_CURES,    200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.BERRY_JUICE,         ItemCategory.HEALING,           100, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.SACRED_ASH,          ItemCategory.REVIVAL,           200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.HP_UP,               ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.PROTEIN,             ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.IRON,                ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.CARBOS,              ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.CALCIUM,             ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.RARE_CANDY,          ItemCategory.VITAMINS,      4800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.PP_UP,               ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.ZINC,                ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.PP_MAX,              ItemCategory.VITAMINS,      9800, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.OLD_GATEAU,          ItemCategory.STATUS_CURES,    200, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.GUARD_SPEC,          ItemCategory.STAT_BOOSTS,     700, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.DIRE_HIT,            ItemCategory.STAT_BOOSTS,     650, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_ATTACK,            ItemCategory.STAT_BOOSTS,     500, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_DEFENSE,           ItemCategory.STAT_BOOSTS,     550, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_SPEED,             ItemCategory.STAT_BOOSTS,     350, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_ACCURACY,          ItemCategory.STAT_BOOSTS,     950, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_SP_ATK,            ItemCategory.STAT_BOOSTS,     350, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.X_SP_DEF,            ItemCategory.STAT_BOOSTS,     350, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.POKE_DOLL,           ItemCategory.SPELUNKING,        1000, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.FLUFFY_TAIL,         ItemCategory.SPELUNKING,        1000, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.BLUE_FLUTE,          ItemCategory.FLUTES,            100, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,useableBattle: true,holdable: true)),
new Item(Items.YELLOW_FLUTE,        ItemCategory.FLUTES,            200, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.RED_FLUTE,           ItemCategory.FLUTES,            300, 30, null, new ItemFlags(countable: true,consumable: true,useableBattle: true,holdable: true)),
new Item(Items.BLACK_FLUTE,         ItemCategory.SPELUNKING,        400, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,holdable: true)),
new Item(Items.WHITE_FLUTE,         ItemCategory.SPELUNKING,        500, 30, null, new ItemFlags(countable: true,consumable: true,useableOverworld: true,holdable: true)),
new Item(Items.SHOAL_SALT,          ItemCategory.COLLECTIBLES,    20, 30, null, new ItemFlags(countable: true)),
new Item(Items.SHOAL_SHELL,         ItemCategory.COLLECTIBLES,    20, 30, null, new ItemFlags(countable: true)),
new Item(Items.RED_SHARD,           ItemCategory.COLLECTIBLES,    200, 30, null, new ItemFlags(underground: true)),
new Item(Items.BLUE_SHARD,          ItemCategory.COLLECTIBLES,    200, 30, null, new ItemFlags(underground: true)),
new Item(Items.YELLOW_SHARD,        ItemCategory.COLLECTIBLES,    200, 30, null, new ItemFlags(underground: true)),
new Item(Items.GREEN_SHARD,         ItemCategory.COLLECTIBLES,    200, 30, null, new ItemFlags(underground: true)),
new Item(Items.SUPER_REPEL,         ItemCategory.SPELUNKING,        500, 30, null, null    ),
new Item(Items.MAX_REPEL,           ItemCategory.SPELUNKING,        700, 30, null, null    ),
new Item(Items.ESCAPE_ROPE,         ItemCategory.SPELUNKING,        550, 30, null, null    ),
new Item(Items.REPEL,               ItemCategory.SPELUNKING,        350, 30, null, null    ),
new Item(Items.SUN_STONE,           ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.MOON_STONE,          ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.FIRE_STONE,          ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.THUNDER_STONE,       ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.WATER_STONE,         ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.LEAF_STONE,          ItemCategory.EVOLUTION,     2100, 30, null, new ItemFlags(underground: true)),
new Item(Items.TINY_MUSHROOM,       ItemCategory.LOOT,          500, 30, null, null    ),
new Item(Items.BIG_MUSHROOM,        ItemCategory.LOOT,          5000, 30, null, null    ),
new Item(Items.PEARL,               ItemCategory.LOOT,          1400, 30, null, null    ),
new Item(Items.BIG_PEARL,           ItemCategory.LOOT,          7500, 30, null, null    ),
new Item(Items.STARDUST,            ItemCategory.LOOT,          2000, 30, null, null    ),
new Item(Items.STAR_PIECE,          ItemCategory.LOOT,          9800, 30, null, new ItemFlags(underground: true)),
new Item(Items.NUGGET,              ItemCategory.LOOT,          10000, 30, null, null    ),
new Item(Items.HEART_SCALE,         ItemCategory.COLLECTIBLES,    100, 30, null, new ItemFlags(underground: true)),
new Item(Items.HONEY,               ItemCategory.DEX_COMPLETION,  100, 30, null, null    ),
new Item(Items.GROWTH_MULCH,        ItemCategory.MULCH,         200, 30, null, null    ),
new Item(Items.DAMP_MULCH,          ItemCategory.MULCH,         200, 30, null, null    ),
new Item(Items.STABLE_MULCH,        ItemCategory.MULCH,         200, 30, null, null    ),
new Item(Items.GOOEY_MULCH,         ItemCategory.MULCH,         200, 30, null, null    ),
new Item(Items.ROOT_FOSSIL,         ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.CLAW_FOSSIL,         ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.HELIX_FOSSIL,        ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.DOME_FOSSIL,         ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.OLD_AMBER,           ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.ARMOR_FOSSIL,        ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.SKULL_FOSSIL,        ItemCategory.DEX_COMPLETION,  1000, 100, null, new ItemFlags(underground: true)),
new Item(Items.RARE_BONE,           ItemCategory.LOOT,          10000, 100, null, new ItemFlags(underground: true)),
new Item(Items.SHINY_STONE,         ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.DUSK_STONE,          ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.DAWN_STONE,          ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.OVAL_STONE,          ItemCategory.EVOLUTION,     2100, 80, null, new ItemFlags(underground: true)),
new Item(Items.ODD_KEYSTONE,        ItemCategory.DEX_COMPLETION,  2100, 80, null, new ItemFlags(underground: true)),
new Item(Items.ADAMANT_ORB,         ItemCategory.SPECIES_SPECIFIC,10000, 60, null, new ItemFlags(holdable: true)),
new Item(Items.LUSTROUS_ORB,        ItemCategory.SPECIES_SPECIFIC,10000, 60, null, new ItemFlags(holdable: true)),
new Item(Items.GRASS_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.FLAME_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.BUBBLE_MAIL,         ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.BLOOM_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.TUNNEL_MAIL,         ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.STEEL_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.HEART_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.SNOW_MAIL,           ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.SPACE_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.AIR_MAIL,            ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.MOSAIC_MAIL,         ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.BRICK_MAIL,          ItemCategory.ALL_MAIL,      50, null, null, null    ),
new Item(Items.CHERI_BERRY,         ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.CHESTO_BERRY,        ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.PECHA_BERRY,         ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.RAWST_BERRY,         ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.ASPEAR_BERRY,        ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.LEPPA_BERRY,         ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.ORAN_BERRY,          ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.PERSIM_BERRY,        ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.LUM_BERRY,           ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.SITRUS_BERRY,        ItemCategory.MEDICINE,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.FIGY_BERRY,          ItemCategory.PICKY_HEALING,   20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.WIKI_BERRY,          ItemCategory.PICKY_HEALING,   20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.MAGO_BERRY,          ItemCategory.PICKY_HEALING,   20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.AGUAV_BERRY,         ItemCategory.PICKY_HEALING,   20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.IAPAPA_BERRY,        ItemCategory.PICKY_HEALING,   20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.RAZZ_BERRY,          ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.BLUK_BERRY,          ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.NANAB_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.WEPEAR_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.PINAP_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.POMEG_BERRY,         ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.KELPSY_BERRY,        ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.QUALOT_BERRY,        ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.HONDEW_BERRY,        ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.GREPA_BERRY,         ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.TAMATO_BERRY,        ItemCategory.EFFORT_DROP,     20, 10, null, null    ),
new Item(Items.CORNN_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.MAGOST_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.RABUTA_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.NOMEL_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.SPELON_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.PAMTRE_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.WATMEL_BERRY,        ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.DURIN_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.BELUE_BERRY,         ItemCategory.BAKING_ONLY,     20, 10, null, null    ),
new Item(Items.OCCA_BERRY,          ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.PASSHO_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.WACAN_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.RINDO_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.YACHE_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.CHOPLE_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.KEBIA_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.SHUCA_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.COBA_BERRY,          ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.PAYAPA_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.TANGA_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.CHARTI_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.KASIB_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.HABAN_BERRY,         ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.COLBUR_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.BABIRI_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.CHILAN_BERRY,        ItemCategory.TYPE_PROTECTION, 20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.LIECHI_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.GANLON_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.SALAC_BERRY,         ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.PETAYA_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.APICOT_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.LANSAT_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.STARF_BERRY,         ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.ENIGMA_BERRY,        ItemCategory.OTHER,         20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.MICLE_BERRY,         ItemCategory.IN_A_PINCH,      20, 10, ItemFlingEffect.Three, new ItemFlags(holdableActive: true)),
new Item(Items.CUSTAP_BERRY,        ItemCategory.IN_A_PINCH,      20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.JABOCA_BERRY,        ItemCategory.OTHER,         20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.ROWAP_BERRY,         ItemCategory.OTHER,         20, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.BRIGHT_POWDER,       ItemCategory.HELD_ITEMS,        10, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.WHITE_HERB,          ItemCategory.HELD_ITEMS,        100, 10, ItemFlingEffect.Four, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.MACHO_BRACE,         ItemCategory.EFFORT_TRAINING, 3000, 60, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.EXP_SHARE,           ItemCategory.TRAINING,      3000, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.QUICK_CLAW,          ItemCategory.HELD_ITEMS,        100, 80, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SOOTHE_BELL,         ItemCategory.TRAINING,      100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.MENTAL_HERB,         ItemCategory.HELD_ITEMS,        100, 10, ItemFlingEffect.Four, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.CHOICE_BAND,         ItemCategory.CHOICE,            100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.KINGS_ROCK,          ItemCategory.HELD_ITEMS,        100, 30, ItemFlingEffect.Seven, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SILVER_POWDER,       ItemCategory.TYPE_ENHANCEMENT,100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.AMULET_COIN,         ItemCategory.TRAINING,      100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.CLEANSE_TAG,         ItemCategory.TRAINING,      200, 30, null, new ItemFlags(holdable: true)),
new Item(Items.SOUL_DEW,            ItemCategory.SPECIES_SPECIFIC,200, 30, null, new ItemFlags(holdable: true)),
new Item(Items.DEEP_SEA_TOOTH,      ItemCategory.SPECIES_SPECIFIC,200, 90, null, new ItemFlags(holdable: true)),
new Item(Items.DEEP_SEA_SCALE,      ItemCategory.SPECIES_SPECIFIC,200, 30, null, new ItemFlags(holdable: true)),
new Item(Items.SMOKE_BALL,          ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.EVERSTONE,           ItemCategory.TRAINING,      200, 30, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.FOCUS_BAND,          ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LUCKY_EGG,           ItemCategory.TRAINING,      200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SCOPE_LENS,          ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.METAL_COAT,          ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LEFTOVERS,           ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.DRAGON_SCALE,        ItemCategory.EVOLUTION,     2100, 30, null, null    ),
new Item(Items.LIGHT_BALL,          ItemCategory.SPECIES_SPECIFIC,100, 30, ItemFlingEffect.Five, new ItemFlags(holdable: true)),
new Item(Items.SOFT_SAND,           ItemCategory.TYPE_ENHANCEMENT,100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.HARD_STONE,          ItemCategory.TYPE_ENHANCEMENT,100, 100, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.MIRACLE_SEED,        ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.BLACK_GLASSES,       ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.BLACK_BELT,          ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.MAGNET,              ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.MYSTIC_WATER,        ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SHARP_BEAK,          ItemCategory.TYPE_ENHANCEMENT,100, 50, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POISON_BARB,         ItemCategory.TYPE_ENHANCEMENT,100, 70, ItemFlingEffect.Six, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.NEVER_MELT_ICE,      ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SPELL_TAG,           ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.TWISTED_SPOON,       ItemCategory.TYPE_ENHANCEMENT,100, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.CHARCOAL,            ItemCategory.TYPE_ENHANCEMENT,9800, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.DRAGON_FANG,         ItemCategory.TYPE_ENHANCEMENT,100, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SILK_SCARF,          ItemCategory.TYPE_ENHANCEMENT,100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.UP_GRADE,            ItemCategory.EVOLUTION,     2100, 30, null, null    ),
new Item(Items.SHELL_BELL,          ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SEA_INCENSE,         ItemCategory.TYPE_ENHANCEMENT,9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LAX_INCENSE,         ItemCategory.HELD_ITEMS,        9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LUCKY_PUNCH,         ItemCategory.SPECIES_SPECIFIC,10, 40, null, new ItemFlags(holdable: true)),
new Item(Items.METAL_POWDER,        ItemCategory.SPECIES_SPECIFIC,10, 10, null, new ItemFlags(holdable: true)),
new Item(Items.THICK_CLUB,          ItemCategory.SPECIES_SPECIFIC,500, 90, null, new ItemFlags(holdable: true)),
new Item(Items.STICK,               ItemCategory.SPECIES_SPECIFIC,200, 60, null, new ItemFlags(holdable: true)),
new Item(Items.RED_SCARF,           ItemCategory.SCARVES,           100, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.BLUE_SCARF,          ItemCategory.SCARVES,           100, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.PINK_SCARF,          ItemCategory.SCARVES,           100, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.GREEN_SCARF,         ItemCategory.SCARVES,           100, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.YELLOW_SCARF,        ItemCategory.SCARVES,           100, 10, null, new ItemFlags(holdableActive: true)),
new Item(Items.WIDE_LENS,           ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.MUSCLE_BAND,         ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.WISE_GLASSES,        ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.EXPERT_BELT,         ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LIGHT_CLAY,          ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.LIFE_ORB,            ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_HERB,          ItemCategory.HELD_ITEMS,        100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.TOXIC_ORB,           ItemCategory.BAD_HELD_ITEMS,  100, 30, ItemFlingEffect.One, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.FLAME_ORB,           ItemCategory.BAD_HELD_ITEMS,  100, 30, ItemFlingEffect.Two, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.QUICK_POWDER,        ItemCategory.SPECIES_SPECIFIC,10, 10, null, new ItemFlags(holdable: true)),
new Item(Items.FOCUS_SASH,          ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.ZOOM_LENS,           ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.METRONOME,           ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.IRON_BALL,           ItemCategory.BAD_HELD_ITEMS,    200, 130, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.LAGGING_TAIL,        ItemCategory.BAD_HELD_ITEMS,  200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.DESTINY_KNOT,        ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.BLACK_SLUDGE,        ItemCategory.HELD_ITEMS,        200, 30, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.ICY_ROCK,            ItemCategory.HELD_ITEMS,        200, 40, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.SMOOTH_ROCK,         ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.HEAT_ROCK,           ItemCategory.HELD_ITEMS,        200, 60, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.DAMP_ROCK,           ItemCategory.HELD_ITEMS,        200, 60, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.GRIP_CLAW,           ItemCategory.HELD_ITEMS,        200, 90, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.CHOICE_SCARF,        ItemCategory.CHOICE,            200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.STICKY_BARB,         ItemCategory.BAD_HELD_ITEMS,  200, 80, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_BRACER,        ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_BELT,          ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_LENS,          ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_BAND,          ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_ANKLET,        ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.POWER_WEIGHT,        ItemCategory.EFFORT_TRAINING, 3000, 70, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.SHED_SHELL,          ItemCategory.HELD_ITEMS,        100, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.BIG_ROOT,            ItemCategory.HELD_ITEMS,        200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.CHOICE_SPECS,        ItemCategory.CHOICE,            200, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.FLAME_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.SPLASH_PLATE,        ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.ZAP_PLATE,           ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.MEADOW_PLATE,        ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.ICICLE_PLATE,        ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.FIST_PLATE,          ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.TOXIC_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.EARTH_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.SKY_PLATE,           ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.MIND_PLATE,          ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.INSECT_PLATE,        ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.STONE_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.SPOOKY_PLATE,        ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.DRACO_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.DREAD_PLATE,         ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.IRON_PLATE,          ItemCategory.PLATES,            1000, 90, null, new ItemFlags(holdable: true,holdableActive: true,underground: true)),
new Item(Items.ODD_INCENSE,         ItemCategory.TYPE_ENHANCEMENT,9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.ROCK_INCENSE,        ItemCategory.TYPE_ENHANCEMENT,9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.FULL_INCENSE,        ItemCategory.BAD_HELD_ITEMS,  9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.WAVE_INCENSE,        ItemCategory.TYPE_ENHANCEMENT,9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.ROSE_INCENSE,        ItemCategory.TYPE_ENHANCEMENT,9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.LUCK_INCENSE,        ItemCategory.TRAINING,      9600, 10, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.PURE_INCENSE,        ItemCategory.TRAINING,      9600, 10, null, new ItemFlags(holdable: true)),
new Item(Items.PROTECTOR,           ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.ELECTIRIZER,         ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.MAGMARIZER,          ItemCategory.EVOLUTION,     2100, 80, null, null    ),
new Item(Items.DUBIOUS_DISC,        ItemCategory.EVOLUTION,     2100, 50, null, null    ),
new Item(Items.REAPER_CLOTH,        ItemCategory.EVOLUTION,     2100, 10, null, null    ),
new Item(Items.RAZOR_CLAW,          ItemCategory.HELD_ITEMS,        2100, 80, null, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.RAZOR_FANG,          ItemCategory.HELD_ITEMS,        2100, 30, ItemFlingEffect.Seven, new ItemFlags(holdable: true,holdableActive: true)),
new Item(Items.TM01,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM02,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM03,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM04,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM05,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM06,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM07,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM08,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM09,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM10,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM11,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM12,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM13,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM14,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.TM15,                ItemCategory.ALL_MACHINES,    7500, null, null, null    ),
new Item(Items.TM16,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM17,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM18,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM19,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM20,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM21,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM22,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM23,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM24,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM25,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.TM26,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM27,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM28,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM29,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM30,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM31,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM32,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM33,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM34,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM35,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM36,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM37,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM38,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.TM39,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM40,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM41,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM42,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM43,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM44,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM45,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM46,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM47,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM48,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM49,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM50,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.TM51,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM52,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.TM53,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM54,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM55,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM56,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM57,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM58,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM59,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM60,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM61,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM62,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM63,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM64,                ItemCategory.ALL_MACHINES,    7500, null, null, null    ),
new Item(Items.TM65,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM66,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM67,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM68,                ItemCategory.ALL_MACHINES,    7500, null, null, null    ),
new Item(Items.TM69,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM70,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM71,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM72,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM73,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM74,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM75,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM76,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM77,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM78,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM79,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM80,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM81,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM82,                ItemCategory.ALL_MACHINES,    1000, null, null, null    ),
new Item(Items.TM83,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM84,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM85,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM86,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM87,                ItemCategory.ALL_MACHINES,    1500, null, null, null    ),
new Item(Items.TM88,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM89,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM90,                ItemCategory.ALL_MACHINES,    2000, null, null, null    ),
new Item(Items.TM91,                ItemCategory.ALL_MACHINES,    3000, null, null, null    ),
new Item(Items.TM92,                ItemCategory.ALL_MACHINES,    5500, null, null, null    ),
new Item(Items.HM01,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM02,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM03,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM04,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM05,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM06,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM07,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.HM08,                ItemCategory.ALL_MACHINES,    0, null, null, null    ),
new Item(Items.EXPLORER_KIT,        ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.LOOT_SACK,           ItemCategory.UNUSED,            0, null, null, null    ),
new Item(Items.RULE_BOOK,           ItemCategory.UNUSED,            0, null, null, null    ),
new Item(Items.POKE_RADAR,          ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.POINT_CARD,          ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.JOURNAL,             ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SEAL_CASE,           ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.FASHION_CASE,        ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SEAL_BAG,            ItemCategory.UNUSED,            0, null, null, null    ),
new Item(Items.PAL_PAD,             ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.WORKS_KEY,           ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.OLD_CHARM,           ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.GALACTIC_KEY,        ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.RED_CHAIN,           ItemCategory.UNUSED,            0, null, null, null    ),
new Item(Items.TOWN_MAP,            ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.VS_SEEKER,           ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.COIN_CASE,           ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.OLD_ROD,             ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.GOOD_ROD,            ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SUPER_ROD,           ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SPRAYDUCK,           ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.POFFIN_CASE,         ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.BICYCLE,             ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SUITE_KEY,           ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.OAKS_LETTER,         ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.LUNAR_WING,          ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.MEMBER_CARD,         ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.AZURE_FLUTE,         ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.SS_TICKET,           ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.CONTEST_PASS,        ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.MAGMA_STONE,         ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.PARCEL,              ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.COUPON_1,            ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.COUPON_2,            ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.COUPON_3,            ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.STORAGE_KEY,         ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.SECRET_POTION,       ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.GRISEOUS_ORB,        ItemCategory.SPECIES_SPECIFIC,10000, null, null, null    ),
new Item(Items.VS_RECORDER,         ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.GRACIDEA,            ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.SECRET_KEY,          ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.APRICORN_BOX,        ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.BERRY_POTS,          ItemCategory.GAMEPLAY,      0, null, null, null    ),
new Item(Items.SQUIRT_BOTTLE,       ItemCategory.PLOT_ADVANCEMENT,0, null, null, null    ),
new Item(Items.LURE_BALL,           ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.LEVEL_BALL,          ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.MOON_BALL,           ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.HEAVY_BALL,          ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.FAST_BALL,           ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.FRIEND_BALL,         ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.LOVE_BALL,           ItemCategory.APRICORN_BALLS,  300, null, null, null    ),
new Item(Items.PARK_BALL,           ItemCategory.STANDARD_BALLS,  0, null, null, null    ),
new Item(Items.SPORT_BALL,          ItemCategory.STANDARD_BALLS,  0, null, null, null    ),
new Item(Items.RED_APRICORN,        ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.BLUE_APRICORN,       ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.YELLOW_APRICORN,     ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.GREEN_APRICORN,      ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.PINK_APRICORN,       ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.WHITE_APRICORN,      ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.BLACK_APRICORN,      ItemCategory.APRICORN_BOX,     0, null, null, null    ),
new Item(Items.DOWSING_MACHINE,     ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.RAGE_CANDY_BAR,      ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.RED_ORB,             ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.BLUE_ORB,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.JADE_ORB,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.ENIGMA_STONE,        ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.UNOWN_REPORT,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.BLUE_CARD,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.SLOWPOKE_TAIL,       ItemCategory.UNUSED,     0, null, null, null    ),
new Item(Items.CLEAR_BELL,          ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.CARD_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.BASEMENT_KEY,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RED_SCALE,           ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.LOST_ITEM,           ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.PASS,                ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.MACHINE_PART,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.SILVER_WING,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RAINBOW_WING,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.MYSTERY_EGG,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GB_SOUNDS,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TIDAL_BELL,          ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.DATA_CARD_01,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_02,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_03,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_04,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_05,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_06,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_07,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_08,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_09,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_10,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_11,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_12,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_13,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_14,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_15,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_16,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_17,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_18,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_19,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_20,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_21,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_22,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_23,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_24,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_25,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_26,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.DATA_CARD_27,        ItemCategory.DATA_CARDS,     0, null, null, null    ),
new Item(Items.LOCK_CAPSULE,        ItemCategory.UNUSED,     0, null, null, null    ),
new Item(Items.PHOTO_ALBUM,         ItemCategory.UNUSED,     0, null, null, null    ),
new Item(Items.ORANGE_MAIL,         ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.HARBOR_MAIL,         ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.GLITTER_MAIL,        ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.MECH_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.WOOD_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.WAVE_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BEAD_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.SHADOW_MAIL,         ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.TROPIC_MAIL,         ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.DREAM_MAIL,          ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.FAB_MAIL,            ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.RETRO_MAIL,          ItemCategory.ALL_MAIL,     0, null, null, null    ),
new Item(Items.MACH_BIKE,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.ACRO_BIKE,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.WAILMER_PAIL,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.DEVON_GOODS,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.SOOT_SACK,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.POKEBLOCK_CASE,      ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.LETTER,              ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.EON_TICKET,          ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.SCANNER,             ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GO_GOGGLES,          ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.METEORITE,           ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RM_1_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RM_2_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RM_4_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RM_6_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.DEVON_SCOPE,         ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.OAKS_PARCEL,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.POKE_FLUTE,          ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.BIKE_VOUCHER,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GOLD_TEETH,          ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.LIFT_KEY,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.SILPH_SCOPE,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.FAME_CHECKER,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TM_CASE,             ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.BERRY_POUCH,         ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TEACHY_TV,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TRI_PASS,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.RAINBOW_PASS,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.TEA,                 ItemCategory.EFFORT_DROP,     0, null, null, null    ),
new Item(Items.MYSTICTICKET,        ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.AURORATICKET,        ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.POWDER_JAR,          ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.RUBY,                ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.SAPPHIRE,            ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.MAGMA_EMBLEM,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.OLD_SEA_MAP,         ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.DOUSE_DRIVE,         ItemCategory.SPECIES_SPECIFIC,     1000, 70, null, null    ),
new Item(Items.SHOCK_DRIVE,         ItemCategory.SPECIES_SPECIFIC,     1000, 70, null, null    ),
new Item(Items.BURN_DRIVE,          ItemCategory.SPECIES_SPECIFIC,     1000, 70, null, null    ),
new Item(Items.CHILL_DRIVE,         ItemCategory.SPECIES_SPECIFIC,     1000, 70, null, null    ),
new Item(Items.SWEET_HEART,         ItemCategory.HEALING,     100, 30, null, null    ),
new Item(Items.GREET_MAIL,          ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.FAVORED_MAIL,        ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.RSVP_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.THANKS_MAIL,         ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.INQUIRY_MAIL,        ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.LIKE_MAIL,           ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.REPLY_MAIL,          ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BRIDGE_MAIL_S,       ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BRIDGE_MAIL_D,       ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BRIDGE_MAIL_T,       ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BRIDGE_MAIL_V,       ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.BRIDGE_MAIL_M,       ItemCategory.ALL_MAIL,     50, null, null, null    ),
new Item(Items.PRISM_SCALE,         ItemCategory.EVOLUTION,     500, 30, null, null    ),
new Item(Items.EVIOLITE,            ItemCategory.HELD_ITEMS,     200, 40, null, null    ),
new Item(Items.FLOAT_STONE,         ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.ROCKY_HELMET,        ItemCategory.HELD_ITEMS,     200, 60, null, null    ),
new Item(Items.AIR_BALLOON,         ItemCategory.HELD_ITEMS,     200, 10, null, null    ),
new Item(Items.RED_CARD,            ItemCategory.HELD_ITEMS,     200, 10, null, null    ),
new Item(Items.RING_TARGET,         ItemCategory.HELD_ITEMS,     200, 10, null, null    ),
new Item(Items.BINDING_BAND,        ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.ABSORB_BULB,         ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.CELL_BATTERY,        ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.EJECT_BUTTON,        ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.FIRE_GEM,            ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.WATER_GEM,           ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.ELECTRIC_GEM,        ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.GRASS_GEM,           ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.ICE_GEM,             ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.FIGHTING_GEM,        ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.POISON_GEM,          ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.GROUND_GEM,          ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.FLYING_GEM,          ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.PSYCHIC_GEM,         ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.BUG_GEM,             ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.ROCK_GEM,            ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.GHOST_GEM,           ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.DARK_GEM,            ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.STEEL_GEM,           ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.HEALTH_WING,         ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.MUSCLE_WING,         ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.RESIST_WING,         ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.GENIUS_WING,         ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.CLEVER_WING,         ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.SWIFT_WING,          ItemCategory.VITAMINS,     3000, 20, null, null    ),
new Item(Items.PRETTY_WING,         ItemCategory.LOOT,     200, 20, null, null    ),
new Item(Items.COVER_FOSSIL,        ItemCategory.DEX_COMPLETION,     1000, 100, null, null    ),
new Item(Items.PLUME_FOSSIL,        ItemCategory.DEX_COMPLETION,     1000, 100, null, null    ),
new Item(Items.LIBERTY_PASS,        ItemCategory.EVENT_ITEMS,     0, null, null, null    ),
new Item(Items.PASS_ORB,            ItemCategory.HELD_ITEMS,     200, 30, null, null    ),
new Item(Items.DREAM_BALL,          ItemCategory.SPECIAL_BALLS,     0, null, null, null    ),
new Item(Items.POKE_TOY,            ItemCategory.SPELUNKING,     1000, 30, null, null    ),
new Item(Items.PROP_CASE,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.DRAGON_SKULL,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.BALM_MUSHROOM,       ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.BIG_NUGGET,          ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.PEARL_STRING,        ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.COMET_SHARD,         ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_COPPER,        ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_SILVER,        ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_GOLD,          ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_VASE,          ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_BAND,          ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_STATUE,        ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.RELIC_CROWN,         ItemCategory.LOOT,     0, 30, null, null    ),
new Item(Items.CASTELIACONE,        ItemCategory.STATUS_CURES,     100, 30, null, null    ),
new Item(Items.DIRE_HIT_2,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SPEED_2,           ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_ATK_2,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_DEF_2,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_DEFENSE_2,         ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ATTACK_2,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ACCURACY_2,        ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SPEED_3,           ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_ATK_3,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_DEF_3,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_DEFENSE_3,         ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ATTACK_3,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ACCURACY_3,        ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SPEED_6,           ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_ATK_6,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_SP_DEF_6,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_DEFENSE_6,         ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ATTACK_6,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.X_ACCURACY_6,        ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.ABILITY_URGE,        ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.ITEM_DROP,           ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.ITEM_URGE,           ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.RESET_URGE,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.DIRE_HIT_3,          ItemCategory.MIRACLE_SHOOTER,     0, null, null, null    ),
new Item(Items.LIGHT_STONE,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.DARK_STONE,          ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.TM93,                ItemCategory.ALL_MACHINES,     10000, null, null, null    ),
new Item(Items.TM94,                ItemCategory.ALL_MACHINES,     10000, null, null, null    ),
new Item(Items.TM95,                ItemCategory.ALL_MACHINES,     10000, null, null, null    ),
new Item(Items.XTRANSCEIVER,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.GOD_STONE,           ItemCategory.UNUSED,     0, null, null, null    ),
new Item(Items.GRAM_1,              ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GRAM_2,              ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GRAM_3,              ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.DRAGON_GEM,          ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.NORMAL_GEM,          ItemCategory.JEWELS,     200, null, null, null    ),
new Item(Items.MEDAL_BOX,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.DNA_SPLICERS,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.PERMIT,              ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.OVAL_CHARM,          ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.SHINY_CHARM,         ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.PLASMA_CARD,         ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.GRUBBY_HANKY,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.COLRESS_MACHINE,     ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.DROPPED_ITEM,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.REVEAL_GLASS,        ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.WEAKNESS_POLICY,     ItemCategory.HELD_ITEMS,     0, null, null, null    ),
new Item(Items.ASSAULT_VEST,        ItemCategory.HELD_ITEMS,     0, null, null, null    ),
new Item(Items.PIXIE_PLATE,         ItemCategory.PLATES,     0, null, null, null    ),
new Item(Items.ABILITY_CAPSULE,     ItemCategory.VITAMINS,     0, null, null, null    ),
new Item(Items.WHIPPED_DREAM,       ItemCategory.EVOLUTION,     0, null, null, null    ),
new Item(Items.SACHET,              ItemCategory.EVOLUTION,     0, null, null, null    ),
new Item(Items.LUMINOUS_MOSS,       ItemCategory.HELD_ITEMS,     0, null, null, null    ),
new Item(Items.SNOWBALL,            ItemCategory.HELD_ITEMS,     0, null, null, null    ),
new Item(Items.SAFETY_GOGGLES,      ItemCategory.HELD_ITEMS,     0, null, null, null    ),
new Item(Items.RICH_MULCH,          ItemCategory.MULCH,     0, null, null, null    ),
new Item(Items.SURPRISE_MULCH,      ItemCategory.MULCH,     0, null, null, null    ),
new Item(Items.BOOST_MULCH,         ItemCategory.MULCH,     0, null, null, null    ),
new Item(Items.AMAZE_MULCH,         ItemCategory.MULCH,     0, null, null, null    ),
new Item(Items.GENGARITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.GARDEVOIRITE,        ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.AMPHAROSITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.VENUSAURITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.CHARIZARDITE_X,      ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.BLASTOISINITE,       ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.MEWTWONITE_X,        ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.MEWTWONITE_Y,        ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.BLAZIKENITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.MEDICHAMITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.HOUNDOOMINITE,       ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.AGGRONITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.BANETTITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.TYRANITARITE,        ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.SCIZORITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.PINSIRITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.AERODACTYLITE,       ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.LUCARIONITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.ABOMASITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.KANGASKHANITE,       ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.GYARADOSITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.ABSOLITE,            ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.CHARIZARDITE_Y,      ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.ALAKAZITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.HERACRONITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.MAWILITE,            ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.MANECTITE,           ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.GARCHOMPITE,         ItemCategory.MEGA_STONES,     0, null, null, null    ),
new Item(Items.ROSELI_BERRY,        ItemCategory.TYPE_PROTECTION,      0, null, null, null    ),
new Item(Items.KEE_BERRY,           ItemCategory.OTHER,      0, null, null, null    ),
new Item(Items.MARANGA_BERRY,       ItemCategory.OTHER,      0, null, null, null    ),
new Item(Items.DISCOUNT_COUPON,     ItemCategory.XY_UNKNOWN,  0, null, null, null    ),
new Item(Items.STRANGE_SOUVENIR,    ItemCategory.XY_UNKNOWN,  0, null, null, null    ),
new Item(Items.LUMIOSE_GALETTE,     ItemCategory.STATUS_CURES,     0, null, null, null    ),
new Item(Items.JAW_FOSSIL,          ItemCategory.DEX_COMPLETION,     0, null, null, null    ),
new Item(Items.SAIL_FOSSIL,         ItemCategory.DEX_COMPLETION,     0, null, null, null    ),
new Item(Items.FAIRY_GEM,           ItemCategory.JEWELS,     0, null, null, null    ),
new Item(Items.ADVENTURE_RULES,     ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.ELEVATOR_KEY,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.HOLO_CASTER,         ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.HONOR_OF_KALOS,      ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.INTRIGUING_STONE,    ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.LENS_CASE,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.LOOKER_TICKET,       ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.MEGA_RING,           ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.POWER_PLANT_PASS,    ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.PROFS_LETTER,        ItemCategory.PLOT_ADVANCEMENT,     0, null, null, null    ),
new Item(Items.ROLLER_SKATES,       ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.SPRINKLOTAD,         ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TMV_PASS,            ItemCategory.GAMEPLAY,     0, null, null, null    ),
new Item(Items.TM96,                ItemCategory.ALL_MACHINES,     0, null, null, null    ),
new Item(Items.TM97,                ItemCategory.ALL_MACHINES,     0, null, null, null    ),
new Item(Items.TM98,                ItemCategory.ALL_MACHINES,     0, null, null, null    ),
new Item(Items.TM99,                ItemCategory.ALL_MACHINES,     0, null, null, null    ),
new Item(Items.TM100,               ItemCategory.ALL_MACHINES,     0, null, null, null    )
};
        }
        #endregion
    }
}