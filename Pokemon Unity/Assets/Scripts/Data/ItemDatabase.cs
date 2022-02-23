//Original Scripts by IIColour (IIColour_Spectrum)

using PokemonUnity.Inventory;
using System.Linq;


// Tempo
public enum CureStatus
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

// ToDo: Remove this?
public static class ItemDatabase
{
    private static Items[] potions = new Items[]
    {
        Items.POTION, Items.SUPER_POTION, Items.HYPER_POTION,
    };

    public static bool IsPotion(Items item)
    {
        return potions.Contains(item);
    }

    // -1 = Item is not in Potions list
    public static int GetPotionHealItem(Items item)
    {
        if (potions.Contains(item))
        {
            if (item == Items.POTION)
                return 20;
            else if (item == Items.SUPER_POTION)
                return 50;
            else if (item == Items.HYPER_POTION)
                return 200;
        }
        return -1;
    }

    public static CureStatus GetCureStatusItem(Items item)
    {
        switch (item)
        {
            case Items.ANTIDOTE:
                return CureStatus.POISON;
            case Items.AWAKENING:
                return CureStatus.SLEEP;
            case Items.BURN_HEAL:
                return CureStatus.BURN;
            case Items.FULL_HEAL:
                return CureStatus.ALL;
            case Items.ICE_HEAL:
                return CureStatus.FROZEN;
            case Items.PARALYZE_HEAL:
                return CureStatus.PARALYSIS;
        }

        return CureStatus.NONE;
    }

    public static float GetPPHealItem(Items item)
    {
        switch (item)
        {
            case Items.ETHER:
                return 10;
            case Items.ELIXIR:
                return 10;
        }
        return 0;
    }

    public static bool IsHealAllMovesPPItem(Items item)
    {
        switch (item)
        {
            case Items.ELIXIR:
                return true;
        }

        return false;
    }

    public static ItemData getItem(Items name)
    {
        return PokemonUnity.Game.ItemData[name];
    }
}