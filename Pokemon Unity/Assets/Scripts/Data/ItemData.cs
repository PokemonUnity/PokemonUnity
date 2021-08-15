//Original Scripts by IIColour (IIColour_Spectrum)

public class ItemDatass
{
    private string name;

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

    private ItemType itemType;
    private BattleType battleType;
    private string description;
    private int price;

    private int tmNo;

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

    private ItemEffect itemEffect;
    private string stringParameter;
    private float floatParameter;

    public ItemDatass(string name, ItemType itemType, BattleType battleType, string description, int price)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = ItemEffect.NONE;
    }

    public ItemDatass(string name, ItemType itemType, BattleType battleType, string description, int price,
        ItemEffect itemEffect)
    {
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = itemEffect;
    }

    public ItemDatass(string name, ItemType itemType, BattleType battleType, string description, int price,
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

    public ItemDatass(string name, ItemType itemType, BattleType battleType, string description, int price,
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

    public ItemDatass(string name, ItemType itemType, BattleType battleType, string description, int price,
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

    //TMs
    public ItemDatass(int tmNo, string name, ItemType itemType, BattleType battleType, string description, int price)
    {
        this.tmNo = tmNo;
        this.name = name;
        this.itemType = itemType;
        this.battleType = battleType;
        this.description = description;
        this.price = price;
        this.itemEffect = ItemEffect.TM;
    }

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
}