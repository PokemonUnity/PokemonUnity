<<<<<<< HEAD
﻿//Original Scripts by IIColour (IIColour_Spectrum)

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
        GlobalVariables.global.debug(debug);

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
        GlobalVariables.global.debug(debug);

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
=======
﻿//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[System.Obsolete]
public static class ItemDatabaseOld
{
    #region Deprecated
    //		Description Box Width 	(i is 0.2 width, l and space are 0.4 width, j is 0.6 width)
    // Until Description Boxes use the Canvas UI System, auto wrapping text is not possible
    //WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW            //the W's represent the width text should be before a new line
    //WWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWWW1234567890  
    //    e.g.
    //It restores the PP of a Pokemon's
    //moves by 10 points at most each.
    [System.Obsolete]
    private static ItemDataOld[] oldItems = new ItemDataOld[]
    {
        //Poké Balls
        new ItemDataOld("Poké Ball", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.POKEBALLS,
            "A device for capturing wild Pokémon.\nIt is designed as a capsule system.", 200, ItemDataOld.ItemEffect.BALL,
            1f),
        new ItemDataOld("Great Ball", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.POKEBALLS,
            "A high-performance Ball, providing a\nbetter catch rate than a Poké Ball.", 600, ItemDataOld.ItemEffect.BALL,
            1.5f),
        new ItemDataOld("Ultra Ball", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.POKEBALLS,
            "An ultra-performance Ball, providing\nhigher catch rates than a Great Ball.", 1200,
            ItemDataOld.ItemEffect.BALL, 2f),
        new ItemDataOld("Master Ball", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.POKEBALLS,
            "The best Ball with the ultimate level of\nperformance. It will never fail to catch.", 0,
            ItemDataOld.ItemEffect.BALL, 255f),

        //Type-Enhancers
        new ItemDataOld("Miracle Seed", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "An item to be held by Pokémon. It is a life\nimbued seed that boosts Grass moves.", 100),
        new ItemDataOld("Charcoal", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "An item to be held by Pokémon. It is a\ncombustible fuel that boosts Fire moves.", 9800),
        new ItemDataOld("Mystic Water", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "An item to be held by Pokémon. It is a\nteardrop gem that boosts Water moves.", 100),

        //Escape Items
        new ItemDataOld("Escape Rope", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A long, durable rope. Use it to escape\ninstantly from a cave or a dungeon.", 550,
            ItemDataOld.ItemEffect.UNIQUE),
        new ItemDataOld("Poké Doll", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.BATTLEITEMS,
            "A doll that attracts Pokémon. Use it to\nflee from any battle with a wild Pokémon.", 1000,
            ItemDataOld.ItemEffect.FLEE),

        //Valuable Items
        new ItemDataOld("Nugget", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A nugget of pure gold with a lustrous\ngleam. It can be sold at a high price.", 10000),
        new ItemDataOld("Stardust", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "Lovely, red-colored sand with a loose,\nsilky feel. It can be sold at a high price.", 2000),

        //Evolutionary Stones
        new ItemDataOld("Fire Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It seems to flicker like flame.", 2100,
            ItemDataOld.ItemEffect.EVOLVE),
        new ItemDataOld("Water Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is a clear shimmering blue.", 2100,
            ItemDataOld.ItemEffect.EVOLVE),
        new ItemDataOld("Thunder Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It seems to glow and crackle.", 2100,
            ItemDataOld.ItemEffect.EVOLVE),
        new ItemDataOld("Leaf Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It has an odd, soft texture.", 2100,
            ItemDataOld.ItemEffect.EVOLVE),
        new ItemDataOld("Moon Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is as black as the night sky.", 10000,
            ItemDataOld.ItemEffect.EVOLVE),
        new ItemDataOld("Dusk Stone", ItemDataOld.ItemType.ITEM, ItemDataOld.BattleType.NONE,
            "A peculiar stone that will evolve certain\nPokémon. It is as dark as dark can be.", 10000,
            ItemDataOld.ItemEffect.EVOLVE),

        //Potions
        new ItemDataOld("Potion", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 20 points.", 300, ItemDataOld.ItemEffect.HP,
            20f),
        new ItemDataOld("Super Potion", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 50 points.", 700, ItemDataOld.ItemEffect.HP,
            50f),
        new ItemDataOld("Hyper Potion", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 200 points.", 1200, ItemDataOld.ItemEffect.HP,
            200f),
        new ItemDataOld("Max Potion", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "A spray-type medicine for wounds. It\ncompletely restores a Pokémon's HP.", 2500, ItemDataOld.ItemEffect.HP,
            1f),

        //PP Restore
        new ItemDataOld("Ether", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "It restores the PP of a Pokémon's\nselected move by 10 points at most.", 1200, ItemDataOld.ItemEffect.PP, 10f),
        new ItemDataOld("Elixir", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.HPPPRESTORE,
            "It restores the PP of all of a Pokemon's\nmoves by 10 points at most each.", 3000, ItemDataOld.ItemEffect.PP,
            "ALL", 10f),

        //Status Restore
        new ItemDataOld("Antidote", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER, "A spray-type medicine. ",
            100, ItemDataOld.ItemEffect.STATUS, "POISONED"),
        new ItemDataOld("Paralyze Heal", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 200, ItemDataOld.ItemEffect.STATUS, "PARALYZED"),
        new ItemDataOld("Awakening", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 250, ItemDataOld.ItemEffect.STATUS, "ASLEEP"),
        new ItemDataOld("Burn Heal", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 250, ItemDataOld.ItemEffect.STATUS, "BURNED"),
        new ItemDataOld("Ice Heal", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER, "A spray-type medicine. ",
            250, ItemDataOld.ItemEffect.STATUS, "FROZEN"),
        new ItemDataOld("Full Heal", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.STATUSHEALER,
            "A spray-type medicine. ", 600, ItemDataOld.ItemEffect.STATUS, "ALL"),

        //TMs
        new ItemDataOld(3, "Psyshock", ItemDataOld.ItemType.TM, ItemDataOld.BattleType.NONE,
            "An odd psychic wave is\nused to attack. This\ndoes physical damage.", 0),
        new ItemDataOld(8, "Bulk Up", ItemDataOld.ItemType.TM, ItemDataOld.BattleType.NONE,
            "The user tenses up it\nmuscles, boosting both\nAttack and Defense.", 0),
        new ItemDataOld(26, "Earthquake", ItemDataOld.ItemType.TM, ItemDataOld.BattleType.NONE,
            "The user sets off an\nEarthquake that hits\nevery active Pokémon.", 0),
        new ItemDataOld(48, "Water Pulse", ItemDataOld.ItemType.TM, ItemDataOld.BattleType.NONE,
            "The user attacks with a\npulsing water blast. It\nmay confuse the foe.", 0),

        //Vitamins
        new ItemDataOld("Rare Candy", ItemDataOld.ItemType.MEDICINE, ItemDataOld.BattleType.NONE,
            "A candy that is packed with energy. It\nraises the level of a single Pokémon.", 4800,
            ItemDataOld.ItemEffect.UNIQUE)
    };
    #endregion

    /// <summary>
    /// Replaces <see cref="oldItems"/>
    /// </summary>
    /// <remarks>
    /// \((\d*)\s*(\d*)\s*(\d*)\s*([\d\w]*)\s*([\d\w]*)\s*
    /// </remarks>
    private static ItemDataOld[] items = new ItemDataOld[] {
new ItemDataOld(1, 34, 0, null, null, new int[]{1,2,4,5}),
new ItemDataOld(2, 34, 1200, null, null, new int[]{1,2,4,5}),
new ItemDataOld(3, 34, 600, null, null, new int[]{1,2,4,5}),
new ItemDataOld(4, 34, 200, null, null, new int[]{1,2,4,5}),
new ItemDataOld(5, 34, 0, null, null, new int[]{1,2,4,5}),
new ItemDataOld(6, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(7, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(8, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(9, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(10, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(11, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(12, 33, 200, null, null, new int[]{1,2,4,5}),
new ItemDataOld(13, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(14, 33, 300, null, null, new int[]{1,2,4,5}),
new ItemDataOld(15, 33, 1000, null, null, new int[]{1,2,4,5}),
new ItemDataOld(16, 33, 200, null, null, new int[]{1,2,4,5}),
new ItemDataOld(17, 27, 300, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(18, 30, 100, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(19, 30, 250, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(20, 30, 250, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(21, 30, 250, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(22, 30, 200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(23, 27, 3000, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(24, 27, 2500, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(25, 27, 1200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(26, 27, 700, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(27, 30, 600, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(28, 29, 1500, 30, null, new int[]{1,2,3,4,5,8}),
new ItemDataOld(29, 29, 4000, 30, null, new int[]{1,2,3,4,5,8}),
new ItemDataOld(30, 27, 200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(31, 27, 300, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(32, 27, 350, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(33, 27, 500, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(34, 27, 500, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(35, 27, 800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(36, 30, 450, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(37, 29, 2800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(38, 28, 1200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(39, 28, 2000, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(40, 28, 3000, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(41, 28, 4500, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(42, 30, 200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(43, 27, 100, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(44, 29, 200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(45, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(46, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(47, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(48, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(49, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(50, 26, 4800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(51, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(52, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(53, 26, 9800, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(54, 30, 200, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(55, 1, 700, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(56, 1, 650, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(57, 1, 500, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(58, 1, 550, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(59, 1, 350, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(60, 1, 950, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(61, 1, 350, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(62, 1, 350, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(63, 11, 1000, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(64, 11, 1000, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(65, 38, 100, 30, null, new int[]{1,2,3,4,5}),
new ItemDataOld(66, 38, 200, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(67, 38, 300, 30, null, new int[]{1,2,4,5}),
new ItemDataOld(68, 11, 400, 30, null, new int[]{1,2,3,5}),
new ItemDataOld(69, 11, 500, 30, null, new int[]{1,2,3,5}),
new ItemDataOld(70, 9, 20, 30, null, new int[]{1}),
new ItemDataOld(71, 9, 20, 30, null, new int[]{1}),
new ItemDataOld(72, 9, 200, 30, null, new int[]{8}),
new ItemDataOld(73, 9, 200, 30, null, new int[]{8}),
new ItemDataOld(74, 9, 200, 30, null, new int[]{8}),
new ItemDataOld(75, 9, 200, 30, null, new int[]{8}),
new ItemDataOld(76, 11, 500, 30, null, null    ),
new ItemDataOld(77, 11, 700, 30, null, null    ),
new ItemDataOld(78, 11, 550, 30, null, null    ),
new ItemDataOld(79, 11, 350, 30, null, null    ),
new ItemDataOld(80, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(81, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(82, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(83, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(84, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(85, 10, 2100, 30, null, new int[]{8}),
new ItemDataOld(86, 24, 500, 30, null, null    ),
new ItemDataOld(87, 24, 5000, 30, null, null    ),
new ItemDataOld(88, 24, 1400, 30, null, null    ),
new ItemDataOld(89, 24, 7500, 30, null, null    ),
new ItemDataOld(90, 24, 2000, 30, null, null    ),
new ItemDataOld(91, 24, 9800, 30, null, new int[]{8}),
new ItemDataOld(92, 24, 10000, 30, null, null    ),
new ItemDataOld(93, 9, 100, 30, null, new int[]{8}),
new ItemDataOld(94, 35, 100, 30, null, null    ),
new ItemDataOld(95, 32, 200, 30, null, null    ),
new ItemDataOld(96, 32, 200, 30, null, null    ),
new ItemDataOld(97, 32, 200, 30, null, null    ),
new ItemDataOld(98, 32, 200, 30, null, null    ),
new ItemDataOld(99, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(100, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(101, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(102, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(103, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(104, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(105, 35, 1000, 100, null, new int[]{8}),
new ItemDataOld(106, 24, 10000, 100, null, new int[]{8}),
new ItemDataOld(107, 10, 2100, 80, null, null    ),
new ItemDataOld(108, 10, 2100, 80, null, null    ),
new ItemDataOld(109, 10, 2100, 80, null, null    ),
new ItemDataOld(110, 10, 2100, 80, null, new int[]{8}),
new ItemDataOld(111, 35, 2100, 80, null, new int[]{8}),
new ItemDataOld(112, 18, 10000, 60, null, new int[]{5}),
new ItemDataOld(113, 18, 10000, 60, null, new int[]{5}),
new ItemDataOld(114, 25, 50, null, null, null    ),
new ItemDataOld(115, 25, 50, null, null, null    ),
new ItemDataOld(116, 25, 50, null, null, null    ),
new ItemDataOld(117, 25, 50, null, null, null    ),
new ItemDataOld(118, 25, 50, null, null, null    ),
new ItemDataOld(119, 25, 50, null, null, null    ),
new ItemDataOld(120, 25, 50, null, null, null    ),
new ItemDataOld(121, 25, 50, null, null, null    ),
new ItemDataOld(122, 25, 50, null, null, null    ),
new ItemDataOld(123, 25, 50, null, null, null    ),
new ItemDataOld(124, 25, 50, null, null, null    ),
new ItemDataOld(125, 25, 50, null, null, null    ),
new ItemDataOld(126, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(127, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(128, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(129, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(130, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(131, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(132, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(133, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(134, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(135, 3, 20, 10, 3, new int[]{7}),
new ItemDataOld(136, 6, 20, 10, 3, new int[]{7}),
new ItemDataOld(137, 6, 20, 10, 3, new int[]{7}),
new ItemDataOld(138, 6, 20, 10, 3, new int[]{7}),
new ItemDataOld(139, 6, 20, 10, 3, new int[]{7}),
new ItemDataOld(140, 6, 20, 10, 3, new int[]{7}),
new ItemDataOld(141, 8, 20, 10, null, null    ),
new ItemDataOld(142, 8, 20, 10, null, null    ),
new ItemDataOld(143, 8, 20, 10, null, null    ),
new ItemDataOld(144, 8, 20, 10, null, null    ),
new ItemDataOld(145, 8, 20, 10, null, null    ),
new ItemDataOld(146, 2, 20, 10, null, null    ),
new ItemDataOld(147, 2, 20, 10, null, null    ),
new ItemDataOld(148, 2, 20, 10, null, null    ),
new ItemDataOld(149, 2, 20, 10, null, null    ),
new ItemDataOld(150, 2, 20, 10, null, null    ),
new ItemDataOld(151, 2, 20, 10, null, null    ),
new ItemDataOld(152, 8, 20, 10, null, null    ),
new ItemDataOld(153, 8, 20, 10, null, null    ),
new ItemDataOld(154, 8, 20, 10, null, null    ),
new ItemDataOld(155, 8, 20, 10, null, null    ),
new ItemDataOld(156, 8, 20, 10, null, null    ),
new ItemDataOld(157, 8, 20, 10, null, null    ),
new ItemDataOld(158, 8, 20, 10, null, null    ),
new ItemDataOld(159, 8, 20, 10, null, null    ),
new ItemDataOld(160, 8, 20, 10, null, null    ),
new ItemDataOld(161, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(162, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(163, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(164, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(165, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(166, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(167, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(168, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(169, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(170, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(171, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(172, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(173, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(174, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(175, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(176, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(177, 7, 20, 10, null, new int[]{7}),
new ItemDataOld(178, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(179, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(180, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(181, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(182, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(183, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(184, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(185, 4, 20, 10, null, new int[]{7}),
new ItemDataOld(186, 5, 20, 10, 3, new int[]{7}),
new ItemDataOld(187, 5, 20, 10, null, new int[]{7}),
new ItemDataOld(188, 4, 20, 10, null, new int[]{7}),
new ItemDataOld(189, 4, 20, 10, null, new int[]{7}),
new ItemDataOld(190, 12, 10, 10, null, new int[]{5,7}),
new ItemDataOld(191, 12, 100, 10, 4, new int[]{5,7}),
new ItemDataOld(192, 14, 3000, 60, null, new int[]{5,7}),
new ItemDataOld(193, 16, 3000, 30, null, new int[]{5,7}),
new ItemDataOld(194, 12, 100, 80, null, new int[]{5,7}),
new ItemDataOld(195, 16, 100, 10, null, new int[]{5,7}),
new ItemDataOld(196, 12, 100, 10, 4, new int[]{5,7}),
new ItemDataOld(197, 13, 100, 10, null, new int[]{5,7}),
new ItemDataOld(198, 12, 100, 30, 7, new int[]{5,7}),
new ItemDataOld(199, 19, 100, 10, null, new int[]{5,7}),
new ItemDataOld(200, 16, 100, 30, null, new int[]{5,7}),
new ItemDataOld(201, 16, 200, 30, null, new int[]{5}),
new ItemDataOld(202, 18, 200, 30, null, new int[]{5}),
new ItemDataOld(203, 18, 200, 90, null, new int[]{5}),
new ItemDataOld(204, 18, 200, 30, null, new int[]{5}),
new ItemDataOld(205, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(206, 16, 200, 30, null, new int[]{5,7,8}),
new ItemDataOld(207, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(208, 16, 200, 30, null, new int[]{5,7}),
new ItemDataOld(209, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(210, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(211, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(212, 10, 2100, 30, null, null    ),
new ItemDataOld(213, 18, 100, 30, 5, new int[]{5}),
new ItemDataOld(214, 19, 100, 10, null, new int[]{5,7}),
new ItemDataOld(215, 19, 100, 100, null, new int[]{5,7,8}),
new ItemDataOld(216, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(217, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(218, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(219, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(220, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(221, 19, 100, 50, null, new int[]{5,7}),
new ItemDataOld(222, 19, 100, 70, 6, new int[]{5,7}),
new ItemDataOld(223, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(224, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(225, 19, 100, 30, null, new int[]{5,7}),
new ItemDataOld(226, 19, 9800, 30, null, new int[]{5,7}),
new ItemDataOld(227, 19, 100, 70, null, new int[]{5,7}),
new ItemDataOld(228, 19, 100, 10, null, new int[]{5,7}),
new ItemDataOld(229, 10, 2100, 30, null, null    ),
new ItemDataOld(230, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(231, 19, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(232, 12, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(233, 18, 10, 40, null, new int[]{5}),
new ItemDataOld(234, 18, 10, 10, null, new int[]{5}),
new ItemDataOld(235, 18, 500, 90, null, new int[]{5}),
new ItemDataOld(236, 18, 200, 60, null, new int[]{5}),
new ItemDataOld(237, 36, 100, 10, null, new int[]{7}),
new ItemDataOld(238, 36, 100, 10, null, new int[]{7}),
new ItemDataOld(239, 36, 100, 10, null, new int[]{7}),
new ItemDataOld(240, 36, 100, 10, null, new int[]{7}),
new ItemDataOld(241, 36, 100, 10, null, new int[]{7}),
new ItemDataOld(242, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(243, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(244, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(245, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(246, 12, 200, 30, null, new int[]{5,7,8}),
new ItemDataOld(247, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(248, 12, 100, 10, null, new int[]{5,7}),
new ItemDataOld(249, 15, 100, 30, 1, new int[]{5,7}),
new ItemDataOld(250, 15, 100, 30, 2, new int[]{5,7}),
new ItemDataOld(251, 18, 10, 10, null, new int[]{5}),
new ItemDataOld(252, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(253, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(254, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(255, 15, 200, 130, null, new int[]{5,7,8}),
new ItemDataOld(256, 15, 200, 10, null, new int[]{5,7}),
new ItemDataOld(257, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(258, 12, 200, 30, null, new int[]{5,7}),
new ItemDataOld(259, 12, 200, 40, null, new int[]{5,7,8}),
new ItemDataOld(260, 12, 200, 10, null, new int[]{5,7,8}),
new ItemDataOld(261, 12, 200, 60, null, new int[]{5,7,8}),
new ItemDataOld(262, 12, 200, 60, null, new int[]{5,7,8}),
new ItemDataOld(263, 12, 200, 90, null, new int[]{5,7}),
new ItemDataOld(264, 13, 200, 10, null, new int[]{5,7}),
new ItemDataOld(265, 15, 200, 80, null, new int[]{5,7}),
new ItemDataOld(266, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(267, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(268, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(269, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(270, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(271, 14, 3000, 70, null, new int[]{5,7}),
new ItemDataOld(272, 12, 100, 10, null, new int[]{5,7}),
new ItemDataOld(273, 12, 200, 10, null, new int[]{5,7}),
new ItemDataOld(274, 13, 200, 10, null, new int[]{5,7}),
new ItemDataOld(275, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(276, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(277, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(278, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(279, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(280, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(281, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(282, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(283, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(284, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(285, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(286, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(287, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(288, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(289, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(290, 17, 1000, 90, null, new int[]{5,7,8}),
new ItemDataOld(291, 19, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(292, 19, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(293, 15, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(294, 19, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(295, 19, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(296, 16, 9600, 10, null, new int[]{5,7}),
new ItemDataOld(297, 16, 9600, 10, null, new int[]{5}),
new ItemDataOld(298, 10, 2100, 80, null, null    ),
new ItemDataOld(299, 10, 2100, 80, null, null    ),
new ItemDataOld(300, 10, 2100, 80, null, null    ),
new ItemDataOld(301, 10, 2100, 50, null, null    ),
new ItemDataOld(302, 10, 2100, 10, null, null    ),
new ItemDataOld(303, 12, 2100, 80, null, new int[]{5,7}),
new ItemDataOld(304, 12, 2100, 30, 7, new int[]{5,7}),
new ItemDataOld(305, 37, 3000, null, null, null    ),
new ItemDataOld(306, 37, 3000, null, null, null    ),
new ItemDataOld(307, 37, 3000, null, null, null    ),
new ItemDataOld(308, 37, 1500, null, null, null    ),
new ItemDataOld(309, 37, 1000, null, null, null    ),
new ItemDataOld(310, 37, 3000, null, null, null    ),
new ItemDataOld(311, 37, 2000, null, null, null    ),
new ItemDataOld(312, 37, 1500, null, null, null    ),
new ItemDataOld(313, 37, 2000, null, null, null    ),
new ItemDataOld(314, 37, 2000, null, null, null    ),
new ItemDataOld(315, 37, 2000, null, null, null    ),
new ItemDataOld(316, 37, 1500, null, null, null    ),
new ItemDataOld(317, 37, 3000, null, null, null    ),
new ItemDataOld(318, 37, 5500, null, null, null    ),
new ItemDataOld(319, 37, 7500, null, null, null    ),
new ItemDataOld(320, 37, 2000, null, null, null    ),
new ItemDataOld(321, 37, 2000, null, null, null    ),
new ItemDataOld(322, 37, 2000, null, null, null    ),
new ItemDataOld(323, 37, 3000, null, null, null    ),
new ItemDataOld(324, 37, 2000, null, null, null    ),
new ItemDataOld(325, 37, 1000, null, null, null    ),
new ItemDataOld(326, 37, 3000, null, null, null    ),
new ItemDataOld(327, 37, 3000, null, null, null    ),
new ItemDataOld(328, 37, 3000, null, null, null    ),
new ItemDataOld(329, 37, 5500, null, null, null    ),
new ItemDataOld(330, 37, 3000, null, null, null    ),
new ItemDataOld(331, 37, 1000, null, null, null    ),
new ItemDataOld(332, 37, 2000, null, null, null    ),
new ItemDataOld(333, 37, 3000, null, null, null    ),
new ItemDataOld(334, 37, 3000, null, null, null    ),
new ItemDataOld(335, 37, 3000, null, null, null    ),
new ItemDataOld(336, 37, 1000, null, null, null    ),
new ItemDataOld(337, 37, 2000, null, null, null    ),
new ItemDataOld(338, 37, 3000, null, null, null    ),
new ItemDataOld(339, 37, 3000, null, null, null    ),
new ItemDataOld(340, 37, 3000, null, null, null    ),
new ItemDataOld(341, 37, 2000, null, null, null    ),
new ItemDataOld(342, 37, 5500, null, null, null    ),
new ItemDataOld(343, 37, 2000, null, null, null    ),
new ItemDataOld(344, 37, 3000, null, null, null    ),
new ItemDataOld(345, 37, 1500, null, null, null    ),
new ItemDataOld(346, 37, 3000, null, null, null    ),
new ItemDataOld(347, 37, 2000, null, null, null    ),
new ItemDataOld(348, 37, 3000, null, null, null    ),
new ItemDataOld(349, 37, 3000, null, null, null    ),
new ItemDataOld(350, 37, 2000, null, null, null    ),
new ItemDataOld(351, 37, 3000, null, null, null    ),
new ItemDataOld(352, 37, 3000, null, null, null    ),
new ItemDataOld(353, 37, 1500, null, null, null    ),
new ItemDataOld(354, 37, 5500, null, null, null    ),
new ItemDataOld(355, 37, 2000, null, null, null    ),
new ItemDataOld(356, 37, 5500, null, null, null    ),
new ItemDataOld(357, 37, 3000, null, null, null    ),
new ItemDataOld(358, 37, 2000, null, null, null    ),
new ItemDataOld(359, 37, 3000, null, null, null    ),
new ItemDataOld(360, 37, 2000, null, null, null    ),
new ItemDataOld(361, 37, 3000, null, null, null    ),
new ItemDataOld(362, 37, 2000, null, null, null    ),
new ItemDataOld(363, 37, 3000, null, null, null    ),
new ItemDataOld(364, 37, 3000, null, null, null    ),
new ItemDataOld(365, 37, 2000, null, null, null    ),
new ItemDataOld(366, 37, 3000, null, null, null    ),
new ItemDataOld(367, 37, 2000, null, null, null    ),
new ItemDataOld(368, 37, 7500, null, null, null    ),
new ItemDataOld(369, 37, 3000, null, null, null    ),
new ItemDataOld(370, 37, 3000, null, null, null    ),
new ItemDataOld(371, 37, 1000, null, null, null    ),
new ItemDataOld(372, 37, 7500, null, null, null    ),
new ItemDataOld(373, 37, 1500, null, null, null    ),
new ItemDataOld(374, 37, 1000, null, null, null    ),
new ItemDataOld(375, 37, 3000, null, null, null    ),
new ItemDataOld(376, 37, 3000, null, null, null    ),
new ItemDataOld(377, 37, 2000, null, null, null    ),
new ItemDataOld(378, 37, 3000, null, null, null    ),
new ItemDataOld(379, 37, 1500, null, null, null    ),
new ItemDataOld(380, 37, 2000, null, null, null    ),
new ItemDataOld(381, 37, 1500, null, null, null    ),
new ItemDataOld(382, 37, 1500, null, null, null    ),
new ItemDataOld(383, 37, 3000, null, null, null    ),
new ItemDataOld(384, 37, 3000, null, null, null    ),
new ItemDataOld(385, 37, 3000, null, null, null    ),
new ItemDataOld(386, 37, 1000, null, null, null    ),
new ItemDataOld(387, 37, 2000, null, null, null    ),
new ItemDataOld(388, 37, 3000, null, null, null    ),
new ItemDataOld(389, 37, 3000, null, null, null    ),
new ItemDataOld(390, 37, 3000, null, null, null    ),
new ItemDataOld(391, 37, 1500, null, null, null    ),
new ItemDataOld(392, 37, 3000, null, null, null    ),
new ItemDataOld(393, 37, 3000, null, null, null    ),
new ItemDataOld(394, 37, 2000, null, null, null    ),
new ItemDataOld(395, 37, 3000, null, null, null    ),
new ItemDataOld(396, 37, 5500, null, null, null    ),
new ItemDataOld(397, 37, 0, null, null, null    ),
new ItemDataOld(398, 37, 0, null, null, null    ),
new ItemDataOld(399, 37, 0, null, null, null    ),
new ItemDataOld(400, 37, 0, null, null, null    ),
new ItemDataOld(401, 37, 0, null, null, null    ),
new ItemDataOld(402, 37, 0, null, null, null    ),
new ItemDataOld(403, 37, 0, null, null, null    ),
new ItemDataOld(404, 37, 0, null, null, null    ),
new ItemDataOld(405, 21, 0, null, null, null    ),
new ItemDataOld(406, 23, 0, null, null, null    ),
new ItemDataOld(407, 23, 0, null, null, null    ),
new ItemDataOld(408, 21, 0, null, null, null    ),
new ItemDataOld(409, 21, 0, null, null, null    ),
new ItemDataOld(410, 21, 0, null, null, null    ),
new ItemDataOld(411, 21, 0, null, null, null    ),
new ItemDataOld(412, 21, 0, null, null, null    ),
new ItemDataOld(413, 23, 0, null, null, null    ),
new ItemDataOld(414, 21, 0, null, null, null    ),
new ItemDataOld(415, 22, 0, null, null, null    ),
new ItemDataOld(416, 22, 0, null, null, null    ),
new ItemDataOld(417, 22, 0, null, null, null    ),
new ItemDataOld(418, 23, 0, null, null, null    ),
new ItemDataOld(419, 21, 0, null, null, null    ),
new ItemDataOld(420, 21, 0, null, null, null    ),
new ItemDataOld(421, 21, 0, null, null, null    ),
new ItemDataOld(422, 21, 0, null, null, null    ),
new ItemDataOld(423, 21, 0, null, null, null    ),
new ItemDataOld(424, 21, 0, null, null, null    ),
new ItemDataOld(425, 21, 0, null, null, null    ),
new ItemDataOld(426, 21, 0, null, null, null    ),
new ItemDataOld(427, 21, 0, null, null, null    ),
new ItemDataOld(428, 20, 0, null, null, null    ),
new ItemDataOld(429, 20, 0, null, null, null    ),
new ItemDataOld(430, 20, 0, null, null, null    ),
new ItemDataOld(431, 20, 0, null, null, null    ),
new ItemDataOld(432, 20, 0, null, null, null    ),
new ItemDataOld(433, 22, 0, null, null, null    ),
new ItemDataOld(434, 21, 0, null, null, null    ),
new ItemDataOld(435, 22, 0, null, null, null    ),
new ItemDataOld(436, 22, 0, null, null, null    ),
new ItemDataOld(437, 22, 0, null, null, null    ),
new ItemDataOld(438, 22, 0, null, null, null    ),
new ItemDataOld(439, 22, 0, null, null, null    ),
new ItemDataOld(440, 22, 0, null, null, null    ),
new ItemDataOld(441, 22, 0, null, null, null    ),
new ItemDataOld(442, 18, 10000, null, null, null    ),
new ItemDataOld(443, 21, 0, null, null, null    ),
new ItemDataOld(444, 20, 0, null, null, null    ),
new ItemDataOld(445, 20, 0, null, null, null    ),
new ItemDataOld(446, 21, 0, null, null, null    ),
new ItemDataOld(447, 21, 0, null, null, null    ),
new ItemDataOld(448, 22, 0, null, null, null    ),
new ItemDataOld(449, 39, 300, null, null, null    ),
new ItemDataOld(450, 39, 300, null, null, null    ),
new ItemDataOld(451, 39, 300, null, null, null    ),
new ItemDataOld(452, 39, 300, null, null, null    ),
new ItemDataOld(453, 39, 300, null, null, null    ),
new ItemDataOld(454, 39, 300, null, null, null    ),
new ItemDataOld(455, 39, 300, null, null, null    ),
new ItemDataOld(456, 34, 0, null, null, null    ),
new ItemDataOld(457, 34, 0, null, null, null    ),
new ItemDataOld(458, 40, 0, null, null, null    ),
new ItemDataOld(459, 40, 0, null, null, null    ),
new ItemDataOld(460, 40, 0, null, null, null    ),
new ItemDataOld(461, 40, 0, null, null, null    ),
new ItemDataOld(462, 40, 0, null, null, null    ),
new ItemDataOld(463, 40, 0, null, null, null    ),
new ItemDataOld(464, 40, 0, null, null, null    ),
new ItemDataOld(465, 21, 0, null, null, null    ),
new ItemDataOld(466, 21, 0, null, null, null    ),
new ItemDataOld(467, 22, 0, null, null, null    ),
new ItemDataOld(468, 22, 0, null, null, null    ),
new ItemDataOld(469, 22, 0, null, null, null    ),
new ItemDataOld(470, 20, 0, null, null, null    ),
new ItemDataOld(471, 21, 0, null, null, null    ),
new ItemDataOld(472, 21, 0, null, null, null    ),
new ItemDataOld(473, 23, 0, null, null, null    ),
new ItemDataOld(474, 22, 0, null, null, null    ),
new ItemDataOld(475, 22, 0, null, null, null    ),
new ItemDataOld(476, 22, 0, null, null, null    ),
new ItemDataOld(477, 22, 0, null, null, null    ),
new ItemDataOld(478, 22, 0, null, null, null    ),
new ItemDataOld(479, 22, 0, null, null, null    ),
new ItemDataOld(480, 22, 0, null, null, null    ),
new ItemDataOld(481, 22, 0, null, null, null    ),
new ItemDataOld(482, 22, 0, null, null, null    ),
new ItemDataOld(483, 22, 0, null, null, null    ),
new ItemDataOld(484, 21, 0, null, null, null    ),
new ItemDataOld(485, 22, 0, null, null, null    ),
new ItemDataOld(486, 41, 0, null, null, null    ),
new ItemDataOld(487, 41, 0, null, null, null    ),
new ItemDataOld(488, 41, 0, null, null, null    ),
new ItemDataOld(489, 41, 0, null, null, null    ),
new ItemDataOld(490, 41, 0, null, null, null    ),
new ItemDataOld(491, 41, 0, null, null, null    ),
new ItemDataOld(492, 41, 0, null, null, null    ),
new ItemDataOld(493, 41, 0, null, null, null    ),
new ItemDataOld(494, 41, 0, null, null, null    ),
new ItemDataOld(495, 41, 0, null, null, null    ),
new ItemDataOld(496, 41, 0, null, null, null    ),
new ItemDataOld(497, 41, 0, null, null, null    ),
new ItemDataOld(498, 41, 0, null, null, null    ),
new ItemDataOld(499, 41, 0, null, null, null    ),
new ItemDataOld(500, 41, 0, null, null, null    ),
new ItemDataOld(501, 41, 0, null, null, null    ),
new ItemDataOld(502, 41, 0, null, null, null    ),
new ItemDataOld(503, 41, 0, null, null, null    ),
new ItemDataOld(504, 41, 0, null, null, null    ),
new ItemDataOld(505, 41, 0, null, null, null    ),
new ItemDataOld(506, 41, 0, null, null, null    ),
new ItemDataOld(507, 41, 0, null, null, null    ),
new ItemDataOld(508, 41, 0, null, null, null    ),
new ItemDataOld(509, 41, 0, null, null, null    ),
new ItemDataOld(510, 41, 0, null, null, null    ),
new ItemDataOld(511, 41, 0, null, null, null    ),
new ItemDataOld(512, 41, 0, null, null, null    ),
new ItemDataOld(513, 23, 0, null, null, null    ),
new ItemDataOld(514, 23, 0, null, null, null    ),
new ItemDataOld(515, 25, 50, null, null, null    ),
new ItemDataOld(516, 25, 50, null, null, null    ),
new ItemDataOld(517, 25, 50, null, null, null    ),
new ItemDataOld(518, 25, 50, null, null, null    ),
new ItemDataOld(519, 25, 50, null, null, null    ),
new ItemDataOld(520, 25, 50, null, null, null    ),
new ItemDataOld(521, 25, 50, null, null, null    ),
new ItemDataOld(522, 25, 50, null, null, null    ),
new ItemDataOld(523, 25, 50, null, null, null    ),
new ItemDataOld(524, 25, 50, null, null, null    ),
new ItemDataOld(525, 25, 50, null, null, null    ),
new ItemDataOld(526, 25, 0, null, null, null    ),
new ItemDataOld(527, 21, 0, null, null, null    ),
new ItemDataOld(528, 21, 0, null, null, null    ),
new ItemDataOld(529, 21, 0, null, null, null    ),
new ItemDataOld(530, 22, 0, null, null, null    ),
new ItemDataOld(531, 21, 0, null, null, null    ),
new ItemDataOld(532, 21, 0, null, null, null    ),
new ItemDataOld(533, 22, 0, null, null, null    ),
new ItemDataOld(534, 20, 0, null, null, null    ),
new ItemDataOld(535, 22, 0, null, null, null    ),
new ItemDataOld(536, 21, 0, null, null, null    ),
new ItemDataOld(537, 22, 0, null, null, null    ),
new ItemDataOld(538, 22, 0, null, null, null    ),
new ItemDataOld(539, 22, 0, null, null, null    ),
new ItemDataOld(540, 22, 0, null, null, null    ),
new ItemDataOld(541, 22, 0, null, null, null    ),
new ItemDataOld(542, 21, 0, null, null, null    ),
new ItemDataOld(543, 22, 0, null, null, null    ),
new ItemDataOld(544, 22, 0, null, null, null    ),
new ItemDataOld(545, 22, 0, null, null, null    ),
new ItemDataOld(546, 22, 0, null, null, null    ),
new ItemDataOld(547, 22, 0, null, null, null    ),
new ItemDataOld(548, 22, 0, null, null, null    ),
new ItemDataOld(549, 21, 0, null, null, null    ),
new ItemDataOld(550, 21, 0, null, null, null    ),
new ItemDataOld(551, 21, 0, null, null, null    ),
new ItemDataOld(552, 21, 0, null, null, null    ),
new ItemDataOld(553, 22, 0, null, null, null    ),
new ItemDataOld(554, 22, 0, null, null, null    ),
new ItemDataOld(555, 22, 0, null, null, null    ),
new ItemDataOld(556, 20, 0, null, null, null    ),
new ItemDataOld(557, 20, 0, null, null, null    ),
new ItemDataOld(558, 21, 0, null, null, null    ),
new ItemDataOld(559, 22, 0, null, null, null    ),
new ItemDataOld(560, 22, 0, null, null, null    ),
new ItemDataOld(561, 22, 0, null, null, null    ),
new ItemDataOld(562, 20, 0, null, null, null    ),
new ItemDataOld(563, 18, 1000, 70, null, null    ),
new ItemDataOld(564, 18, 1000, 70, null, null    ),
new ItemDataOld(565, 18, 1000, 70, null, null    ),
new ItemDataOld(566, 18, 1000, 70, null, null    ),
new ItemDataOld(567, 27, 100, 30, null, null    ),
new ItemDataOld(568, 25, 50, null, null, null    ),
new ItemDataOld(569, 25, 50, null, null, null    ),
new ItemDataOld(570, 25, 50, null, null, null    ),
new ItemDataOld(571, 25, 50, null, null, null    ),
new ItemDataOld(572, 25, 50, null, null, null    ),
new ItemDataOld(573, 25, 50, null, null, null    ),
new ItemDataOld(574, 25, 50, null, null, null    ),
new ItemDataOld(575, 25, 50, null, null, null    ),
new ItemDataOld(576, 25, 50, null, null, null    ),
new ItemDataOld(577, 25, 50, null, null, null    ),
new ItemDataOld(578, 25, 50, null, null, null    ),
new ItemDataOld(579, 25, 50, null, null, null    ),
new ItemDataOld(580, 10, 500, 30, null, null    ),
new ItemDataOld(581, 12, 200, 40, null, null    ),
new ItemDataOld(582, 12, 200, 30, null, null    ),
new ItemDataOld(583, 12, 200, 60, null, null    ),
new ItemDataOld(584, 12, 200, 10, null, null    ),
new ItemDataOld(585, 12, 200, 10, null, null    ),
new ItemDataOld(586, 12, 200, 10, null, null    ),
new ItemDataOld(587, 12, 200, 30, null, null    ),
new ItemDataOld(588, 12, 200, 30, null, null    ),
new ItemDataOld(589, 12, 200, 30, null, null    ),
new ItemDataOld(590, 12, 200, 30, null, null    ),
new ItemDataOld(591, 42, 200, null, null, null    ),
new ItemDataOld(592, 42, 200, null, null, null    ),
new ItemDataOld(593, 42, 200, null, null, null    ),
new ItemDataOld(594, 42, 200, null, null, null    ),
new ItemDataOld(595, 42, 200, null, null, null    ),
new ItemDataOld(596, 42, 200, null, null, null    ),
new ItemDataOld(597, 42, 200, null, null, null    ),
new ItemDataOld(598, 42, 200, null, null, null    ),
new ItemDataOld(599, 42, 200, null, null, null    ),
new ItemDataOld(600, 42, 200, null, null, null    ),
new ItemDataOld(601, 42, 200, null, null, null    ),
new ItemDataOld(602, 42, 200, null, null, null    ),
new ItemDataOld(603, 42, 200, null, null, null    ),
new ItemDataOld(604, 42, 200, null, null, null    ),
new ItemDataOld(605, 42, 200, null, null, null    ),
new ItemDataOld(606, 26, 3000, 20, null, null    ),
new ItemDataOld(607, 26, 3000, 20, null, null    ),
new ItemDataOld(608, 26, 3000, 20, null, null    ),
new ItemDataOld(609, 26, 3000, 20, null, null    ),
new ItemDataOld(610, 26, 3000, 20, null, null    ),
new ItemDataOld(611, 26, 3000, 20, null, null    ),
new ItemDataOld(612, 24, 200, 20, null, null    ),
new ItemDataOld(613, 35, 1000, 100, null, null    ),
new ItemDataOld(614, 35, 1000, 100, null, null    ),
new ItemDataOld(615, 20, 0, null, null, null    ),
new ItemDataOld(616, 12, 200, 30, null, null    ),
new ItemDataOld(617, 33, 0, null, null, null    ),
new ItemDataOld(618, 11, 1000, 30, null, null    ),
new ItemDataOld(619, 21, 0, null, null, null    ),
new ItemDataOld(620, 22, 0, null, null, null    ),
new ItemDataOld(621, 24, 0, 30, null, null    ),
new ItemDataOld(622, 24, 0, 30, null, null    ),
new ItemDataOld(623, 24, 0, 30, null, null    ),
new ItemDataOld(624, 24, 0, 30, null, null    ),
new ItemDataOld(625, 24, 0, 30, null, null    ),
new ItemDataOld(626, 24, 0, 30, null, null    ),
new ItemDataOld(627, 24, 0, 30, null, null    ),
new ItemDataOld(628, 24, 0, 30, null, null    ),
new ItemDataOld(629, 24, 0, 30, null, null    ),
new ItemDataOld(630, 24, 0, 30, null, null    ),
new ItemDataOld(631, 24, 0, 30, null, null    ),
new ItemDataOld(632, 30, 100, 30, null, null    ),
new ItemDataOld(633, 43, 0, null, null, null    ),
new ItemDataOld(634, 43, 0, null, null, null    ),
new ItemDataOld(635, 43, 0, null, null, null    ),
new ItemDataOld(636, 43, 0, null, null, null    ),
new ItemDataOld(637, 43, 0, null, null, null    ),
new ItemDataOld(638, 43, 0, null, null, null    ),
new ItemDataOld(639, 43, 0, null, null, null    ),
new ItemDataOld(640, 43, 0, null, null, null    ),
new ItemDataOld(641, 43, 0, null, null, null    ),
new ItemDataOld(642, 43, 0, null, null, null    ),
new ItemDataOld(643, 43, 0, null, null, null    ),
new ItemDataOld(644, 43, 0, null, null, null    ),
new ItemDataOld(645, 43, 0, null, null, null    ),
new ItemDataOld(646, 43, 0, null, null, null    ),
new ItemDataOld(647, 43, 0, null, null, null    ),
new ItemDataOld(648, 43, 0, null, null, null    ),
new ItemDataOld(649, 43, 0, null, null, null    ),
new ItemDataOld(650, 43, 0, null, null, null    ),
new ItemDataOld(651, 43, 0, null, null, null    ),
new ItemDataOld(652, 43, 0, null, null, null    ),
new ItemDataOld(653, 43, 0, null, null, null    ),
new ItemDataOld(654, 43, 0, null, null, null    ),
new ItemDataOld(655, 43, 0, null, null, null    ),
new ItemDataOld(656, 43, 0, null, null, null    ),
new ItemDataOld(657, 22, 0, null, null, null    ),
new ItemDataOld(658, 22, 0, null, null, null    ),
new ItemDataOld(659, 37, 10000, null, null, null    ),
new ItemDataOld(660, 37, 10000, null, null, null    ),
new ItemDataOld(661, 37, 10000, null, null, null    ),
new ItemDataOld(662, 21, 0, null, null, null    ),
new ItemDataOld(663, 23, 0, null, null, null    ),
new ItemDataOld(664, 22, 0, null, null, null    ),
new ItemDataOld(665, 22, 0, null, null, null    ),
new ItemDataOld(666, 22, 0, null, null, null    ),
new ItemDataOld(668, 42, 200, null, null, null    ),
new ItemDataOld(669, 42, 200, null, null, null    ),
new ItemDataOld(670, 21, 0, null, null, null    ),
new ItemDataOld(671, 21, 0, null, null, null    ),
new ItemDataOld(673, 21, 0, null, null, null    ),
new ItemDataOld(674, 21, 0, null, null, null    ),
new ItemDataOld(675, 21, 0, null, null, null    ),
new ItemDataOld(676, 22, 0, null, null, null    ),
new ItemDataOld(677, 22, 0, null, null, null    ),
new ItemDataOld(678, 22, 0, null, null, null    ),
new ItemDataOld(679, 22, 0, null, null, null    ),
new ItemDataOld(681, 21, 0, null, null, null    ),
new ItemDataOld(682, 12, 0, null, null, null    ),
new ItemDataOld(683, 12, 0, null, null, null    ),
new ItemDataOld(684, 17, 0, null, null, null    ),
new ItemDataOld(685, 26, 0, null, null, null    ),
new ItemDataOld(686, 10, 0, null, null, null    ),
new ItemDataOld(687, 10, 0, null, null, null    ),
new ItemDataOld(688, 12, 0, null, null, null    ),
new ItemDataOld(689, 12, 0, null, null, null    ),
new ItemDataOld(690, 12, 0, null, null, null    ),
new ItemDataOld(691, 32, 0, null, null, null    ),
new ItemDataOld(692, 32, 0, null, null, null    ),
new ItemDataOld(693, 32, 0, null, null, null    ),
new ItemDataOld(694, 32, 0, null, null, null    ),
new ItemDataOld(695, 44, 0, null, null, null    ),
new ItemDataOld(696, 44, 0, null, null, null    ),
new ItemDataOld(697, 44, 0, null, null, null    ),
new ItemDataOld(698, 44, 0, null, null, null    ),
new ItemDataOld(699, 44, 0, null, null, null    ),
new ItemDataOld(700, 44, 0, null, null, null    ),
new ItemDataOld(701, 44, 0, null, null, null    ),
new ItemDataOld(702, 44, 0, null, null, null    ),
new ItemDataOld(703, 44, 0, null, null, null    ),
new ItemDataOld(704, 44, 0, null, null, null    ),
new ItemDataOld(705, 44, 0, null, null, null    ),
new ItemDataOld(706, 44, 0, null, null, null    ),
new ItemDataOld(707, 44, 0, null, null, null    ),
new ItemDataOld(708, 44, 0, null, null, null    ),
new ItemDataOld(709, 44, 0, null, null, null    ),
new ItemDataOld(710, 44, 0, null, null, null    ),
new ItemDataOld(711, 44, 0, null, null, null    ),
new ItemDataOld(712, 44, 0, null, null, null    ),
new ItemDataOld(713, 44, 0, null, null, null    ),
new ItemDataOld(714, 44, 0, null, null, null    ),
new ItemDataOld(715, 44, 0, null, null, null    ),
new ItemDataOld(716, 44, 0, null, null, null    ),
new ItemDataOld(717, 44, 0, null, null, null    ),
new ItemDataOld(718, 44, 0, null, null, null    ),
new ItemDataOld(719, 44, 0, null, null, null    ),
new ItemDataOld(720, 44, 0, null, null, null    ),
new ItemDataOld(721, 44, 0, null, null, null    ),
new ItemDataOld(722, 44, 0, null, null, null    ),
new ItemDataOld(723, 7, 0, null, null, null    ),
new ItemDataOld(724, 4, 0, null, null, null    ),
new ItemDataOld(725, 4, 0, null, null, null    ),
new ItemDataOld(726, 10001, 0, null, null, null    ),
new ItemDataOld(727, 10001, 0, null, null, null    ),
new ItemDataOld(728, 30, 0, null, null, null    ),
new ItemDataOld(729, 35, 0, null, null, null    ),
new ItemDataOld(730, 35, 0, null, null, null    ),
new ItemDataOld(731, 42, 0, null, null, null    ),
new ItemDataOld(732, 21, 0, null, null, null    ),
new ItemDataOld(733, 22, 0, null, null, null    ),
new ItemDataOld(734, 21, 0, null, null, null    ),
new ItemDataOld(735, 22, 0, null, null, null    ),
new ItemDataOld(736, 22, 0, null, null, null    ),
new ItemDataOld(737, 21, 0, null, null, null    ),
new ItemDataOld(738, 22, 0, null, null, null    ),
new ItemDataOld(739, 21, 0, null, null, null    ),
new ItemDataOld(740, 22, 0, null, null, null    ),
new ItemDataOld(741, 22, 0, null, null, null    ),
new ItemDataOld(742, 21, 0, null, null, null    ),
new ItemDataOld(743, 21, 0, null, null, null    ),
new ItemDataOld(744, 21, 0, null, null, null    ),
new ItemDataOld(745, 37, 0, null, null, null    ),
new ItemDataOld(746, 37, 0, null, null, null    ),
new ItemDataOld(747, 37, 0, null, null, null    ),
new ItemDataOld(748, 37, 0, null, null, null    ),
new ItemDataOld(749, 37, 0, null, null, null    )
    };

    public static int getItemsLength()
    {
        return items.Length;
    }

    public static ItemDataOld[] getItemTypeArray(ItemDataOld.ItemType itemType)
    {
        ItemDataOld[] result = new ItemDataOld[items.Length];
        int resultPos = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getItemType() == itemType)
            {
                result[resultPos] = items[i];
                resultPos += 1;
            }
        }
        ItemDataOld[] cleanedResult = new ItemDataOld[resultPos];
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

    public static ItemDataOld[] getBattleTypeArray(ItemDataOld.BattleType battleType)
    {
        ItemDataOld[] result = new ItemDataOld[items.Length];
        int resultPos = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getBattleType() == battleType)
            {
                result[resultPos] = items[i];
                resultPos += 1;
            }
        }
        ItemDataOld[] cleanedResult = new ItemDataOld[resultPos];
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

    public static ItemDataOld getItem(int index)
    {
        if (index < items.Length)
        {
            return items[index];
        }
        return null;
    }

    /// <summary>
    /// Fetches the <seealso cref="ItemDataOld"/> from Database Array
    /// </summary>
    /// <param name="index">ItemId is Index value in array</param>
    /// <returns></returns>
    public static ItemDataOld getItem(eItems.Item index)
    {
        /*if (index < items.Length)
        {
            return items[index];
        }*/
        return items[(int)index];
    }

    public static ItemDataOld getItem(string name)
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

    /// <summary>
    /// deprecated
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
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

    public static int getIndexOf(ItemDataOld name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == name)
            {
                return i;
            }
        }
        return -1;
    }

    public static int getIndexOf(eItems.Item name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].getItemId() == name)
            {
                return i;
            }
        }
        return -1;
    }
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
}