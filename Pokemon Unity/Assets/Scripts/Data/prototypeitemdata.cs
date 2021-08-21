using PokemonUnity.Inventory;

// Enum
public enum ItemType
{
    NONE = 0,
    ITEM, // ?
    BALL,
    MEDICINE,
    EVOLUTION,
    BERRY,
    TM,
    KEY,
    EV,
}

public enum MedicineType
{
    POTION,
    STATUS,
    PP
}

public enum Stats
{
    HP,
    ATK,
    DEF,
    SPA,
    SPD,
    SPE,
}

public class ItemData
{
    // Variables
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int BasePrice { get; private set; }
    public ItemType ItemType { get; private set; }

    //
    public ItemData(string name, ItemType itemType)
        : this(name, 0, null, itemType)
    { 
    }
    public ItemData(Items item, ItemType itemType)
        : this(item, 0, null, itemType)
    {
    }

    //
    public ItemData(string name, string description, ItemType itemType)
        : this(name, 0, description, itemType)
    {
    }
    public ItemData(Items item, string description, ItemType itemType)
        : this(item, 0, description, itemType)
    { 
    }

    //
    public ItemData(Items item, int price, string description, ItemType itemType)
        : this(item.toString(), price, description, itemType)
    {
    }
    public ItemData(string name, int price, string description, ItemType itemType)
    {
        Name = name; BasePrice = price; Description = description; ItemType = itemType;
    }

    // Functions
    void SetBasePrice(int price) { BasePrice = price; }
    void SetDescription(string description) { Description = description; }
}

public class BallData : ItemData
{
    public BallData(Items item, int Price, string Description, float CatchRatio)
        : base(item, Price, Description, ItemType.BALL)
    {
        this.CatchRatio = CatchRatio;
    }

    public float CatchRatio { get; private set; }
}

public class TMData : ItemData
{
    public TMData(PokemonUnity.Moves move, int No)
        : this(move, No, 0, null)
    {
        getTMNo = No;
        MoveID = move;
    }

    public TMData(PokemonUnity.Moves move, int No, int Price, string Description)
        : base(move.toString()/*string.Format("TM " + No)*/, Price, Description, ItemType.TM)
    {
        getTMNo = No;
        MoveID = move;
    }

    public int getTMNo { get; private set; }
    public PokemonUnity.Moves MoveID { get; private set; }
}

#region Medicine
public class MedicineData : ItemData
{
    public MedicineData(Items item, int Price, string Description, MedicineType MedicineType)
        : base(item.toString(), Price, Description, ItemType.MEDICINE)
    {
        this.MedicineType = MedicineType;
    }
    public MedicineData(string name, int Price, string Description, MedicineType MedicineType)
        : base(name, Price, Description, ItemType.MEDICINE)
    {
        this.MedicineType = MedicineType;
    }
    public MedicineType MedicineType { get; private set; }
}

public class PotionItem : MedicineData
{
    public PotionItem(string potion, int Price, float HealValue, string Description)
        : base(potion, Price, Description, MedicineType.POTION)
    {
        this.HealValue = HealValue;
    }
    public PotionItem(Items potion, int Price, float HealValue, string Description)
        : this(potion.toString(), Price, HealValue, Description)
    {
        this.HealValue = HealValue;
    }

    public float HealValue { get; private set; }
}

// Tempo
public enum ItemStatus
{
    NONE = 0,
    SLEEP = 1,
    POISON = 2,
    PARALYSIS = 3,
    BURN = 4,
    FROZEN = 5,
    FAINT = 6,
    ALL = 7
}

public class StatusItem : MedicineData
{
    public StatusItem(Items item, int Price, ItemStatus CureStatus, string Description)
        : base(item, Price, Description, MedicineType.STATUS)
    {
        this.CureStatus = CureStatus;
    }

    public ItemStatus CureStatus { get; private set; } 
}

public class PPItem : MedicineData
{
    public PPItem(Items item, int Price, bool HealAllMove, float AmountHeal, string Description)
        : base(item, Price, Description, MedicineType.PP)
    {
        IsHealAllMoves = HealAllMove; AmountToHealMove = AmountHeal;
    }

    public bool IsHealAllMoves { get; private set; }
    public float AmountToHealMove { get; private set; }
}

#endregion


