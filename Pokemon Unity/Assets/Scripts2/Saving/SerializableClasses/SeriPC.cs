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
        public int[] BoxTextures { get; private set; }
        public static int[] Items { get; private set; }

        public SeriPC(Pokemon.Pokemon[,] pokemons, string[] boxNames, int[] boxTextures, List<Item.Item> boxItems)
        {
            Pokemons = new SeriPokemon[pokemons.GetLength(0), pokemons.GetLength(1)];
            for (int i = 0; i < Pokemons.GetLength(0); i++)
            {
                for (int j = 0; j < Pokemons.GetLength(1); j++)
                {
                    Pokemons[i, j] = (SeriPokemon)pokemons[i, j];
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

		public Pokemon.Pokemon[,] GetPokemonsFromSeri()
		{
			Pokemon.Pokemon[,] pkmn = new Pokemon.Pokemon[Pokemons.GetLength(0), Pokemons.GetLength(1)];
            for (int i = 0; i < Pokemons.GetLength(0); i++)
            {
                for (int j = 0; j < Pokemons.GetLength(1); j++)
                {
					//Easier if it only grabs actual pokemons (with values), than trying to copy everything...
                    if(Pokemons[i,j] == null || (PokemonUnity.Pokemons)Pokemons[i, j].Species != PokemonUnity.Pokemons.NONE)
						pkmn[i, j] = (Pokemon.Pokemon)Pokemons[i, j];
					else
						pkmn[i, j] = new Pokemon.Pokemon(PokemonUnity.Pokemons.NONE);
                }
            }
			return pkmn;
		}

		public Item.Item[] GetItemsFromSeri()
		{
			Item.Item[] items = new Item.Item[Items.Length];
			for (int i = 0; i < Items.Length; i++)
			{
				items[i] = new Item.Item((Item.Items)Items[i]);
			}
			return items;
		}
    }
}
