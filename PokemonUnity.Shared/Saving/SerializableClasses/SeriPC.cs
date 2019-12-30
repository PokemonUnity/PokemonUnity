using System.Collections.Generic;

namespace PokemonUnity.Saving.SerializableClasses
{
    /// <summary>
    /// Serializable version of Pokemon Unity's PC class
    /// </summary>
    [System.Serializable]
    public class SeriPC
    {
        public SeriPokemon[][] Pokemons { get; private set; }
        public string[] BoxNames { get; private set; }
        public int[] BoxTextures { get; private set; }
        public static int[] Items { get; private set; }

        public SeriPC(Monster.Pokemon[,] pokemons, string[] boxNames, int[] boxTextures, List<Inventory.Items> boxItems)
        {
            Pokemons = new SeriPokemon[pokemons.GetLength(0)][];
            for (int i = 0; i < Pokemons.GetLength(0); i++)
            {
				Pokemons[i] = new SeriPokemon[pokemons.GetLength(1)];
                for (int j = 0; j < Pokemons.GetLength(1); j++)
                {
                    Pokemons[i][j] = (SeriPokemon)pokemons[i, j];
                }
            }
            BoxNames = boxNames;
            BoxTextures = boxTextures;

            Items = new int[boxItems.Count];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i] = (int)boxItems[i];
            }
        }

		public Monster.Pokemon[,] GetPokemonsFromSeri()
		{
			Monster.Pokemon[,] pkmn = new Monster.Pokemon[Pokemons.GetLength(0), Pokemons.GetLength(1)];
			//Monster.Pokemon[,] pkmn = new Monster.Pokemon[Pokemons.Length, Pokemons[0].Length];
            for (int i = 0; i < Pokemons.GetLength(0); i++)
            {
                for (int j = 0; j < Pokemons.GetLength(1); j++)
                {
					//Easier if it only grabs actual pokemons (with values), than trying to copy everything...
                    if(Pokemons[i][j] == null || (PokemonUnity.Pokemons)Pokemons[i][j].Species != PokemonUnity.Pokemons.NONE)
						pkmn[i, j] = (Monster.Pokemon)Pokemons[i][j];
					else
						pkmn[i, j] = new Monster.Pokemon(PokemonUnity.Pokemons.NONE);
                }
            }
			return pkmn;
		}

		public Inventory.Items[] GetItemsFromSeri()
		{
			Inventory.Items[] items = new Inventory.Items[Items.Length];
			for (int i = 0; i < Items.Length; i++)
			{
				items[i] = (Inventory.Items)Items[i];
			}
			return items;
		}
    }
}
