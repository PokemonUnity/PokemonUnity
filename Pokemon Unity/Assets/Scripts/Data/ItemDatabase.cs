//Original Scripts by IIColour (IIColour_Spectrum)

using PokemonUnity.Inventory;
using UnityEngine;
public static class ItemDatabase
{
    private static ItemData[] ProtoTypeItems = new ItemData[]
    {
        // Poke Balls
        new BallData(Items.POKE_BALL, 200,
            "A device for capturing wild Pokémon.\nIt is designed as a capsule system.", 1f),
        new BallData(Items.GREAT_BALL, 600,
            "A high-performance Ball, providing a\nbetter catch rate than a Poké Ball.", 1.5f),
        new BallData(Items.ULTRA_BALL, 1200,
            "An ultra-performance Ball, providing\nhigher catch rates than a Great Ball.", 2f),
        new BallData(Items.MASTER_BALL, 0,
            "The best Ball with the ultimate level of\nperformance. It will never fail to catch.", 255f),
        
        // Type-Enhancers
        new ItemData(Items.MIRACLE_SEED, 100,
            "An item to be held by Pokémon. It is a life\nimbued seed that boosts Grass moves.", ItemType.ITEM),
        new ItemData(Items.CHARCOAL, 9800,
            "An item to be held by Pokémon. It is a\ncombustible fuel that boosts Fire moves.", ItemType.ITEM),
        new ItemData(Items.MYSTIC_WATER, 100,
            "An item to be held by Pokémon. It is a\nteardrop gem that boosts Water moves.", ItemType.ITEM),

        // Escape Items
        new ItemData(Items.ESCAPE_ROPE, 550,
            "A long, durable rope. Use it to escape\ninstantly from a cave or a dungeon.", ItemType.ITEM),
        new ItemData(Items.POKE_DOLL, 1000,
            "A doll that attracts Pokémon. Use it to\nflee from any battle with a wild Pokémon.", ItemType.ITEM),

        // Valuable Items
        new ItemData(Items.NUGGET, 10000, "A nugget of pure gold with a lustrous\ngleam. It can be sold at a high price.", ItemType.ITEM),
        new ItemData(Items.STARDUST, 2000, "Lovely, red-colored sand with a loose,\nsilky feel. It can be sold at a high price.", ItemType.ITEM),
        
        // Evolutionary Stones

        new ItemData(Items.FIRE_STONE, 2100,
            "A peculiar stone that will evolve certain\nPokémon. It seems to flicker like flame.", ItemType.EVOLUTION),
        new ItemData(Items.WATER_STONE, 2100,
            "A peculiar stone that will evolve certain\nPokémon. It is a clear shimmering blue.", ItemType.EVOLUTION),
        new ItemData(Items.THUNDER_STONE, 2100,
            "A peculiar stone that will evolve certain\nPokémon. It seems to glow and crackle.", ItemType.EVOLUTION),
        new ItemData(Items.LEAF_STONE, 2100,
            "A peculiar stone that will evolve certain\nPokémon. It has an odd, soft texture.", ItemType.EVOLUTION),
        new ItemData(Items.MOON_STONE, 10000,
            "A peculiar stone that will evolve certain\nPokémon. It is as black as the night sky.", ItemType.EVOLUTION),
        new ItemData(Items.DUSK_STONE, 10000,
            "A peculiar stone that will evolve certain\nPokémon. It is as dark as dark can be.", ItemType.EVOLUTION),

        // Potion

        new PotionItem(Items.POTION, 300, 20f,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 20 points."),
        new PotionItem(Items.SUPER_POTION, 700, 20f,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 50 points."),
        new PotionItem(Items.HYPER_POTION, 1200, 20f,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 200 points."),
        new PotionItem(Items.MAX_POTION, 1200, 20f,
            "A spray-type medicine for wounds. It\nrestores a Pokémon's HP by 200 points."),

        // PP Restore

        new PPItem(Items.ETHER, 1200, false, 10, "It restores the PP of a Pokémon's\nselected move by 10 points at most."),
        new PPItem(Items.ELIXIR, 3000, true, 10, "It restores the PP of all of a Pokemon's\nmoves by 10 points at most each."),

        // Status Restore

        new StatusItem(Items.ANTIDOTE, 100, ItemStatus.POISON, "A spray-type medicine. "),
        new StatusItem(Items.PARALYZE_HEAL, 200, ItemStatus.PARALYSIS,
            "A spray-type medicine. "),
        new StatusItem(Items.AWAKENING, 250, ItemStatus.SLEEP,
            "A spray-type medicine. "),
        new StatusItem(Items.BURN_HEAL, 250, ItemStatus.BURN,
            "A spray-type medicine. "),
        new StatusItem(Items.ICE_HEAL, 250, ItemStatus.FROZEN,
            "A spray-type medicine. "),
        new StatusItem(Items.FULL_HEAL, 600, ItemStatus.ALL,
            "A spray-type medicine. "),

        // TMs

        new TMData(PokemonUnity.Moves.PSYSHOCK, 3, 0,
            "An odd psychic wave is\nused to attack. This\ndoes physical damage."),
        new TMData(PokemonUnity.Moves.BULK_UP, 8, 0,
            "The user tenses up it\nmuscles, boosting both\nAttack and Defense."),
        new TMData(PokemonUnity.Moves.EARTHQUAKE, 26, 0,
            "The user sets off an\nEarthquake that hits\nevery active Pokémon."),
        new TMData(PokemonUnity.Moves.WATER_PULSE, 48, 0,
            "The user attacks with a\npulsing water blast. It\nmay confuse the foe."),

        // Berry

        // Unique
        new ItemData(Items.RARE_CANDY, 4800, "A candy that is packed with energy. It\nraises the level of a single Pokémon.", ItemType.ITEM),
    };

    
    public static int getItemsLength()
    {
        return ProtoTypeItems.Length;
    }

    public static ItemData getItem(string name)
    {
        for (int i = 0; i < ProtoTypeItems.Length; i++)
        {
            if (ProtoTypeItems[i].Name == name)
            {
                return ProtoTypeItems[i];
            }
        }

        return null;
    }

    public static ItemData getItem(Items name)
    {
        for (int i = 0; i < ProtoTypeItems.Length; i++)
        {
            if (ProtoTypeItems[i].Name == name.toString())
            {
                return ProtoTypeItems[i];
            }
        }
        return null;
    }

    public static int getIndexOf(string name)
    {
        for (int i = 0; i < ProtoTypeItems.Length; i++)
        {
            if (ProtoTypeItems[i].Name == name)
            {
                return i;
            }
        }
        return -1;
    }
}