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

        public SeriPC(Pokemon.Pokemon[,] pokemons, string[] boxNames, int[] boxTextures, List<Item.Item> boxItems)
        {
            Pokemons = new SeriPokemon[pokemons.GetUpperBound(0), pokemons.GetUpperBound(1)];
            for (int i = 0; i < Pokemons.GetUpperBound(0); i++)
            {
                for (int j = 0; j < Pokemons.GetUpperBound(1); j++)
                {
                    Pokemons[i, j] = pokemons[i, j];
                }
            }
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
