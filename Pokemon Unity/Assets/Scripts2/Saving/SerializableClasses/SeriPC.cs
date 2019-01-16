using System.Collections.Generic;

namespace PokemonUnity.Saving.SerializableClasses
{
    /// <summary>
    /// Serializable version of Pokemon Unity's PC class
    /// </summary>
    [System.Serializable]
    public class SeriPC
    {
        public SeriPokemon[,] Pokemons { get; private set; }

        public string[] BoxNames { get; private set; }
        public static int[] BoxTextures { get; private set; }
        public static int[] Items { get; private set; }

        public SeriPC(SeriPokemon[,] pokemons, string[] boxNames, int[] boxTextures, List<Item.Item> boxItems)
        {
            Pokemons = pokemons;
            BoxNames = boxNames;
            BoxTextures = boxTextures;

            Items = new int[boxItems.Count];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = (int)boxItems[i].ItemId;
            }
        }
    }
}
