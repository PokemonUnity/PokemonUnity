//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class ItemData
{
    private string name;

    public enum ItemPockets
    {
        MISC = 1,
        MEDICINE = 2,
        POKEBALL = 3,
        MACHINE = 4, //TMs
        BERRY = 5,
        MAIL = 6,
        BATTLE = 7,
        KEY = 8
    }

    /// <summary>
    /// 
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
    public enum ItemType
    {
        ITEM,
        MEDICINE,
        BERRY,
        TM,
        KEY
    }

    public enum BattleType
    {
        NONE,
        HPPPRESTORE,
        STATUSHEALER,
        POKEBALLS,
        BATTLEITEMS
    }

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
#endregion

    private ItemType itemType;
    private BattleType battleType;
    private string description;
    private int price;

    private int tmNo;

    private ItemEffect itemEffect;
    private string stringParameter;
    private float floatParameter;

    private int itemId;
    private ItemFlags[] itemFlags;
    private ItemCategory itemCategory;
    //private ItemPockets itemPocket;

    public ItemData(int itemId, ItemCategory itemType, /*BattleType battleType, string description,*/ int price, int? flingPower,
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

    public ItemData(int itemId, int itemType, /*BattleType battleType, string description,*/ int price, int? flingPower,
        int? itemEffect, /*string stringParameter, float floatParameter,*/ int[] flags = null)
    {
        //return 
        new ItemData(itemId, (ItemCategory)itemType, price, flingPower, (ItemFlingEffect)itemEffect, System.Array.ConvertAll(flags, item => (ItemFlags)item));
    }

    #region oldItemData-to be removed...
    public ItemData(string name, ItemType itemType, BattleType battleType, string description, int price)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = ItemEffect.NONE;
    }

    public ItemData(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
    }

    public ItemData(string name, ItemType itemType, BattleType battleType, string description, int price,
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

    public ItemData(string name, ItemType itemType, BattleType battleType, string description, int price,
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

    public ItemData(string name, ItemType itemType, BattleType battleType, string description, int price,
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
    /// Initializes a new TMs instance of the <see cref="ItemData"/> class.
    /// </summary>
    /// <param name="tmNo"></param>
    /// <param name="name"></param>
    /// <param name="itemType"></param>
    /// <param name="battleType"></param>
    /// <param name="description"></param>
    /// <param name="price"></param>
    public ItemData(int tmNo, string name, ItemType itemType, BattleType battleType, string description, int price)
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