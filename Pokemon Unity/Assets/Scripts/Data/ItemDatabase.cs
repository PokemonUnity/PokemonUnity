//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public static class ItemDatabase
{
    //		Description Box Width 	(i is 0.2 width, l and space are 0.4 width, j is 0.6 width)
    // Until Description Boxes use the Canvas UI System, auto wrapping text is not possible
    //WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW            //the W's represent the width text should be before a new line
    //WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW1234567890  
    //    e.g.
    //It restores the PP of a Pokemon's
    //moves by 10 points at most each.

    private static ItemData[] items = new ItemData[]
    {
        //Poké Balls
        new ItemData("Poké Ball", ItemData.ItemType.ITEM, ItemData.BattleType.POKEBALLS,
            "A device for capturing wild Pokémon.\nIt is designed as a capsule system.", 200, ItemData.ItemEffect.BALL,
            1f),
        new ItemData("Great Ball", ItemData.ItemType.ITEM, ItemData.BattleType.POKEBALLS,
            "A high-performance Ball, providing a\nbetter catch rate than a Poké Ball.", 600, ItemData.ItemEffect.BALL,
            1.5f),
        new ItemData("Ultra Ball", ItemData.ItemType.ITEM, ItemData.BattleType.POKEBALLS,
            "An ultra-performance Ball, providing\nhigher catch rates than a Great Ball.", 1200,
            ItemData.ItemEffect.BALL, 2f),
        new ItemData("Master Ball", ItemData.ItemType.ITEM, ItemData.BattleType.POKEBALLS,
            "The best Ball with the ultimate level of\nperformance. It will never fail to catch.", 0,
            ItemData.ItemEffect.BALL, 255f),

        //Type-Enhancers
        new ItemData("Miracle Seed", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "An item to be held by Pokémon. It is a life\nimbued seed that boosts Grass moves.", 100),
        new ItemData("Charcoal", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "An item to be held by Pokémon. It is a\ncombustible fuel that boosts Fire moves.", 9800),
        new ItemData("Mystic Water", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "An item to be held by Pokémon. It is a\nteardrop gem that boosts Water moves.", 100),

        //Escape Items
        new ItemData("Escape Rope", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A long, durable rope. Use it to escape\ninstantly from a cave or a dungeon.", 550,
            ItemData.ItemEffect.UNIQUE),
        new ItemData("Poké Doll", ItemData.ItemType.ITEM, ItemData.BattleType.BATTLEITEMS,
            "A doll that attracts Pokémon. Use it to\nflee from any battle with a wild Pokémon.", 1000,
            ItemData.ItemEffect.FLEE),

        //Valuable Items
        new ItemData("Nugget", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A nugget of pure gold with a lustrous\ngleam. It can be sold at a high price.", 10000),
        new ItemData("Stardust", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "Lovely, red-colored sand with a loose,\nsilky feel. It can be sold at a high price.", 2000),

        //Evolutionary Stones
        new ItemData("Fire Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It seems to flicker like flame.", 2100,
            ItemData.ItemEffect.EVOLVE),
        new ItemData("Water Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is a clear shimmering blue.", 2100,
            ItemData.ItemEffect.EVOLVE),
        new ItemData("Thunder Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It seems to glow and crackle.", 2100,
            ItemData.ItemEffect.EVOLVE),
        new ItemData("Leaf Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It has an odd, soft texture.", 2100,
            ItemData.ItemEffect.EVOLVE),
        new ItemData("Moon Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is as black as the night sky.", 10000,
            ItemData.ItemEffect.EVOLVE),
        new ItemData("Dusk Stone", ItemData.ItemType.ITEM, ItemData.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is as dark as dark can be.", 10000,
            ItemData.ItemEffect.EVOLVE),

        //Potions
        new ItemData("Potion", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 20 points.", 300, ItemData.ItemEffect.HP,
            20f),
        new ItemData("Super Potion", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 50 points.", 700, ItemData.ItemEffect.HP,
            50f),
        new ItemData("Hyper Potion", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 200 points.", 1200, ItemData.ItemEffect.HP,
            200f),
        new ItemData("Max Potion", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\ncompletely restores a Pokémon's HP.", 2500, ItemData.ItemEffect.HP,
            1f),

        //PP Restore
        new ItemData("Ether", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "It restores the PP of a Pokémon's\nselected move by 10 points at most.", 1200, ItemData.ItemEffect.PP, 10f),
        new ItemData("Elixir", ItemData.ItemType.MEDICINE, ItemData.BattleType.HPPPRESTORE,
            "It restores the PP of all of a Pokemon's\nmoves by 10 points at most each.", 3000, ItemData.ItemEffect.PP,
            "ALL", 10f),

        //Status Restore
        new ItemData("Antidote", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER, "A spray-type medicine. ",
            100, ItemData.ItemEffect.STATUS, "POISONED"),
        new ItemData("Paralyze Heal", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 200, ItemData.ItemEffect.STATUS, "PARALYZED"),
        new ItemData("Awakening", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 250, ItemData.ItemEffect.STATUS, "ASLEEP"),
        new ItemData("Burn Heal", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 250, ItemData.ItemEffect.STATUS, "BURNED"),
        new ItemData("Ice Heal", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER, "A spray-type medicine. ",
            250, ItemData.ItemEffect.STATUS, "FROZEN"),
        new ItemData("Full Heal", ItemData.ItemType.MEDICINE, ItemData.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 600, ItemData.ItemEffect.STATUS, "ALL"),

        //TMs
        new ItemData(3, "Psyshock", ItemData.ItemType.TM, ItemData.BattleType.NONE,
            "An odd psychic wave is\nused to attack. This\ndoes physical damage.", 0),
        new ItemData(8, "Bulk Up", ItemData.ItemType.TM, ItemData.BattleType.NONE,
            "The user tenses up it\nmuscles, boosting both\nAttack and Defense.", 0),
        new ItemData(26, "Earthquake", ItemData.ItemType.TM, ItemData.BattleType.NONE,
            "The user sets off an\nEarthquake that hits\nevery active Pokémon.", 0),
        new ItemData(48, "Water Pulse", ItemData.ItemType.TM, ItemData.BattleType.NONE,
            "The user attacks with a\npulsing water blast. It\nmay confuse the foe.", 0),

        //Vitamins
        new ItemData("Rare Candy", ItemData.ItemType.MEDICINE, ItemData.BattleType.NONE,
            "A candy that is packed with energy. It\nraises the level of a single Pokémon.", 4800,
            ItemData.ItemEffect.UNIQUE)
    };

    public static int getItemsLength()
    {
        return items.Length;
    }

    public static ItemData[] getItemTypeArray(ItemData.ItemType itemType)
    {
        ItemData[] result = new ItemData[items.Length];
        int resultPos = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getItemType() == itemType)
            {
                result[resultPos] = items[i];
                resultPos += 1;
            }
        }
        ItemData[] cleanedResult = new ItemData[resultPos];
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            cleanedResult[i] = result[i];
        }
        string debug = "";
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            debug += cleanedResult[i].getName() + ", ";
        }
        Debug.Log(debug);

        return cleanedResult;
    }

    public static ItemData[] getBattleTypeArray(ItemData.BattleType battleType)
    {
        ItemData[] result = new ItemData[items.Length];
        int resultPos = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getBattleType() == battleType)
            {
                result[resultPos] = items[i];
                resultPos += 1;
            }
        }
        ItemData[] cleanedResult = new ItemData[resultPos];
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            cleanedResult[i] = result[i];
        }
        string debug = "";
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            debug += cleanedResult[i].getName() + ", ";
        }
        Debug.Log(debug);

        return cleanedResult;
    }

    public static ItemData getItem(int index)
    {
        if (index < items.Length)
        {
            return items[index];
        }
        return null;
    }

    public static ItemData getItem(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getName() == name)
            {
                return items[i];
            }
        }
        return null;
    }

    public static int getIndexOf(string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getName() == name)
            {
                return i;
            }
        }
        return -1;
    }
}