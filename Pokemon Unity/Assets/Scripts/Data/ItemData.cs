//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Obsolete]
public class ItemDataOld
{
    private string name;
    private string description;

    public enum ItemPockets
    {
        MISC = 1,
        MEDICINE = 2,
        POKEBALL = 3,
        /// <summary>
        /// TMs
        /// </summary>
        MACHINE = 4, 
        BERRY = 5,
        MAIL = 6,
        BATTLE = 7,
        /// <summary>
        /// Items that are not 'stackable' and therefore should take up 1 individual item spot per (if multiples)
        /// Or possibly limit and restrict to one per user
        /// </summary>
        KEY = 8
    }

    /// <summary>
    /// </summary>
    public enum ItemFlags
    {
        /// <summary>
        /// Has a count in the bag
        /// </summary>
        COUNTABLE = 1,
        /// <summary>
        /// Consumed when used
        /// </summary>
        CONSUMABLE = 2,
        /// <summary>
        /// Usable outside battle
        /// </summary>
        USEABLE_OVERWORLD = 3,
        /// <summary>
        /// Usable in battle
        /// </summary>
        USEABLE_IN_BATTLE = 4,
        /// <summary>
        /// Can be held by a pokemon
        /// </summary>
        HOLDABLE = 5,
        /// <summary>
        /// Works passively when held
        /// </summary>
        HOLDABLE_PASSIVE = 6,
        /// <summary>
        /// Usable by a pokemon when held
        /// </summary>
        HOLDABLE_ACTIVE = 7,
        /// <summary>
        /// Appears in Sinnoh Underground
        /// </summary>
        UNDERGROUND = 8
    }

    /// <summary>
    /// Item Category determines both the item's effect
    /// and the bag-pocket that the item belongs to.
    /// </summary>
    /// <remarks>
    /// Can determine an <see cref="itemEffect"/> by the category it belongs to.
    /// </remarks>
    public enum ItemCategory
    {
		COLLECTIBLES = 9, //PocketId = 1
		EVOLUTION = 10, //PocketId = 1
		SPELUNKING = 11, //PocketId = 1
		HELD_ITEMS = 12, //PocketId = 1
		CHOICE = 13, //PocketId = 1
		EFFORT_TRAINING = 14, //PocketId = 1
		BAD_HELD_ITEMS = 15, //PocketId = 1
		TRAINING = 16, //PocketId = 1
		PLATES = 17, //PocketId = 1
		SPECIES_SPECIFIC = 18, //PocketId = 1
		TYPE_ENHANCEMENT = 19, //PocketId = 1
		LOOT = 24, //PocketId = 1
		MULCH = 32, //PocketId = 1
		DEX_COMPLETION = 35, //PocketId = 1
		SCARVES = 36, //PocketId = 1
		JEWELS = 42, //PocketId = 1
		MEGA_STONES = 44, //PocketId = 1

		VITAMINS = 26, //PocketId = 2
		HEALING = 27, //PocketId = 2
		PP_RECOVERY = 28, //PocketId = 2
		REVIVAL = 29, //PocketId = 2
		STATUS_CURES = 30, //PocketId = 2

		SPECIAL_BALLS = 33, //PocketId = 3
		STANDARD_BALLS = 34, //PocketId = 3
		APRICORN_BALLS = 39, //PocketId = 3

		ALL_MACHINES = 37, //PocketId = 4

		EFFORT_DROP = 2, //PocketId = 5
		MEDICINE = 3, //PocketId = 5
		OTHER = 4, //PocketId = 5
		IN_A_PINCH = 5, //PocketId = 5
		PICKY_HEALING = 6, //PocketId = 5
		TYPE_PROTECTION = 7, //PocketId = 5
		BAKING_ONLY = 8, //PocketId = 5

		ALL_MAIL = 25, //PocketId = 6

		STAT_BOOSTS = 1, //PocketId = 7
		FLUTES = 38, //PocketId = 7
		MIRACLE_SHOOTER = 43, //PocketId = 7

		EVENT_ITEMS = 20, //PocketId = 8
		GAMEPLAY = 21, //PocketId = 8
		PLOT_ADVANCEMENT = 22, //PocketId = 8
		UNUSED = 23, //PocketId = 8
		APRICORN_BOX = 40, //PocketId = 8
		DATA_CARDS = 41, //PocketId = 8
		XY_UNKNOWN = 10001, //PocketId = 8
    }

    /// <summary>
    /// Effects that occur when items are thrown at target.
    /// Not all items can be thrown(?)...
    /// </summary>
    /// <remarks>Didnt have access to variable names...</remarks>
    public enum ItemFlingEffect
    {
        /// <summary>
        /// Badly poisons the target.
        /// </summary>
        One = 1,
        /// <summary>
        /// Burns the target.
        /// </summary>
        Two = 2,
        /// <summary>
        /// Immediately activates the berry's effect on the target.
        /// </summary>
        Three = 3,
        /// <summary>
        /// Immediately activates the herb's effect on the target.
        /// </summary>
        Four = 4, 
        /// <summary>
        /// Paralyzes the target.
        /// </summary>
        Five = 5,
        /// <summary>
        /// Poisons the target.
        /// </summary>
        Six = 6,
        /// <summary>
        /// Target will flinch if it has not yet gone this turn.
        /// </summary>
        Seven = 7
    }

    #region Not Needed
    [System.Obsolete]
    public enum ItemType
    {
        ITEM,
        MEDICINE,
        BERRY,
        TM,
        KEY
    }

    [System.Obsolete]
    public enum BattleType
    {
        NONE,
        HPPPRESTORE,
        STATUSHEALER,
        POKEBALLS,
        BATTLEITEMS
    }

    [System.Obsolete]
    public enum ItemEffect
    {
        NONE,
        UNIQUE,
        HP,
        PP,
        STATUS,
        EV,
        EVOLVE,
        FLEE,
        BALL,
        STATBOOST,
        TM
    }

    /// <summary>
    /// Use <see cref="ItemCategory.ALL_MACHINES"/> or <see cref="ItemPockets.MACHINE"/> and make a "GetTMnum()"
    /// </summary>
    [System.Obsolete]
    private int tmNo;
    [System.Obsolete]
    private string stringParameter;
    [System.Obsolete]
    private float floatParameter;
    [System.Obsolete]
    private BattleType battleType;
    [System.Obsolete]
    private ItemEffect itemEffect;
    [System.Obsolete]
    private ItemType itemType;
#endregion

    private int price;


    /// <summary>
    /// Deprecated; Use <see cref="itemId"/>
    /// </summary>
    private int itemIdInt;
    private eItems.Item itemId;
    private ItemFlags[] itemFlags;
    private ItemCategory itemCategory;
    private ItemPockets itemPocket;
    private ItemFlingEffect itemFlingEffect;

    public ItemDataOld(eItems.Item itemId, ItemCategory itemCategory, /*BattleType battleType, string description,*/ int price, int? flingPower,
        ItemFlingEffect? itemEffect, /*string stringParameter, float floatParameter,*/ ItemFlags[] flags = null)
    {
        //this.name = name;
        //this.itemType = itemType;
        //this.battleType = battleType;
        //this.description = description;
        //this.price = price;
        //this.itemEffect = itemEffect;
        //this.stringParameter = stringParameter;
        //this.floatParameter = floatParameter;
    }

    public ItemDataOld(int itemId, int itemCategory, /*BattleType battleType, string description,*/ int price, int? flingPower,
        int? itemEffect, /*string stringParameter, float floatParameter,*/ int[] flags = null)
    {
        //return 
        new ItemDataOld((eItems.Item)itemId, (ItemCategory)itemType, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (ItemFlags)item));
    }

    #region oldItemData-to be removed...
    [System.Obsolete]
    public ItemDataOld(string name, ItemType itemType, BattleType battleType, string description, int price)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = ItemEffect.NONE;
    }

    [System.Obsolete]
    public ItemDataOld(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
    }

    [System.Obsolete]
    public ItemDataOld(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect, string stringParameter)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
        this.stringParameter = stringParameter;
    }

    [System.Obsolete]
    public ItemDataOld(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect, float floatParameter)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
        this.floatParameter = floatParameter;
    }

    [System.Obsolete]public ItemDataOld(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect, string stringParameter, float floatParameter)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
        this.stringParameter = stringParameter;
        this.floatParameter = floatParameter;
    }
    
    /// <summary>
    /// Initializes a new TMs instance of the <see cref="ItemDataOld"/> class.
    /// </summary>
    /// <param name="tmNo"></param>
    /// <param name="name"></param>
    /// <param name="itemType"></param>
    /// <param name="battleType"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    [System.Obsolete]public ItemDataOld(int tmNo, string name, ItemType itemType, BattleType battleType, string description, int price)
    {
        this.tmNo = tmNo;
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = ItemEffect.TM;
    }
    #endregion

#region Methods
    public string getName()
    {
        return name;
    }

    public ItemType getItemType()
    {
        return itemType;
    }

    public BattleType getBattleType()
    {
        return battleType;
    }

    public string getDescription()
    {
        return description;
    }

    public int getPrice()
    {
        return price;
    }

    public int getTMNo()
    {
        return tmNo;
    }

    public ItemEffect getItemEffect()
    {
        return itemEffect;
    }

    public ItemCategory getItemCategory()
    {
        return itemCategory;
    }

    public ItemFlags[] getItemFlags()
    {
        return itemFlags;
    }

    public eItems.Item getItemId()
    {
        return itemId;
    }

    public ItemPockets getItemPocket()
    {
        return itemPocket;
    }

    public ItemPockets? getItemPocket(ItemCategory item)
    {
        ItemPockets? itemPocket;
        switch (item)
        {//([\w]*) = [\d]*, //PocketId = ([\d]*)
            case ItemCategory.COLLECTIBLES: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.EVOLUTION: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.SPELUNKING: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.HELD_ITEMS: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.CHOICE: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.EFFORT_TRAINING: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.BAD_HELD_ITEMS: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.TRAINING: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.PLATES: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.SPECIES_SPECIFIC: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.TYPE_ENHANCEMENT: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.LOOT: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.MULCH: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.DEX_COMPLETION: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.SCARVES: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.JEWELS: itemPocket = (ItemPockets)1; break;
		    case ItemCategory.MEGA_STONES: itemPocket = (ItemPockets)1; break;

		    case ItemCategory.VITAMINS: itemPocket = (ItemPockets)2; break;
		    case ItemCategory.HEALING: itemPocket = (ItemPockets)2; break;
		    case ItemCategory.PP_RECOVERY: itemPocket = (ItemPockets)2; break;
		    case ItemCategory.REVIVAL: itemPocket = (ItemPockets)2; break;
		    case ItemCategory.STATUS_CURES: itemPocket = (ItemPockets)2; break;

		    case ItemCategory.SPECIAL_BALLS: itemPocket = (ItemPockets)3; break;
		    case ItemCategory.STANDARD_BALLS: itemPocket = (ItemPockets)3; break;
		    case ItemCategory.APRICORN_BALLS: itemPocket = (ItemPockets)3; break;

		    case ItemCategory.ALL_MACHINES: itemPocket = (ItemPockets)4; break;

		    case ItemCategory.EFFORT_DROP: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.MEDICINE: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.OTHER: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.IN_A_PINCH: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.PICKY_HEALING: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.TYPE_PROTECTION: itemPocket = (ItemPockets)5; break;
		    case ItemCategory.BAKING_ONLY: itemPocket = (ItemPockets)5; break;

		    case ItemCategory.ALL_MAIL: itemPocket = (ItemPockets)6; break;

		    case ItemCategory.STAT_BOOSTS: itemPocket = (ItemPockets)7; break;
		    case ItemCategory.FLUTES: itemPocket = (ItemPockets)7; break;
		    case ItemCategory.MIRACLE_SHOOTER: itemPocket = (ItemPockets)7; break;

		    case ItemCategory.EVENT_ITEMS: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.GAMEPLAY: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.PLOT_ADVANCEMENT: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.UNUSED: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.APRICORN_BOX: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.DATA_CARDS: itemPocket = (ItemPockets)8; break;
		    case ItemCategory.XY_UNKNOWN: itemPocket = (ItemPockets)8; break;
            default:
                itemPocket = null;
                break;
        }
        return itemPocket;
    }

    public string getStringParameter()
    {
        return stringParameter;
    }

    public float getFloatParameter()
    {
        return floatParameter;
    }
#endregion
}