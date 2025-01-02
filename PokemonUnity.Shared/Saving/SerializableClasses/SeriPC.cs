using System.Collections.Generic;
using System.Linq;

namespace PokemonUnity.Saving.SerializableClasses
{
	/// <summary>
	/// Serializable version of Pokemon Unity's PC class
	/// </summary>
	[System.Serializable]
	public struct SeriPC
	{
		public SeriPokemon[][] Pokemons { get; private set; }
		public string[] BoxNames { get; private set; }
		public int[] BoxTextures { get; private set; }
		public int[] Items { get; private set; }
		//ToDo: public int[] Mail { get; private set; }
		public byte ActiveBox { get; private set; }

		public SeriPC(PokemonEssentials.Interface.PokeBattle.IPokemon[,] pokemons, string[] boxNames, int[] boxTextures, List<Inventory.Items> boxItems, byte box = 0)
		{
			ActiveBox = box;
			Pokemons = new SeriPokemon[pokemons.GetLength(0)][];
			for (int i = 0; i < Pokemons.Length; i++)
			{
				Pokemons[i] = new SeriPokemon[pokemons.GetLength(1)];
				for (int j = 0; j < Pokemons[i].Length; j++)
				{
					//Pokemons[i][j] = (SeriPokemon)pokemons[i, j];
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

		//public SeriPC(Character.PC pc)
		//{
		//	ActiveBox = pc.ActiveBox;
		//	Pokemons = new SeriPokemon[pc.AllBoxes.Length][];
		//	for (int i = 0; i < Pokemons.Length; i++)
		//	{
		//		Pokemons[i] = new SeriPokemon[pc.AllBoxes[i].Length];
		//		for (int j = 0; j < Pokemons[i].Length; j++)
		//		{
		//			Pokemons[i][j] = (SeriPokemon)pc.AllBoxes[i][j];
		//		}
		//	}
		//	BoxNames = pc.BoxNames;
		//	BoxTextures = pc.BoxTextures;
		//
		//	//Items = new int[pc.Items.Length];
		//	//for (int i = 0; i < Items.Length; i++)
		//	//{
		//	//	Items[i] = (int)pc.Items[i];
		//	//}
		//	//List<Inventory.Items> list = new List<Inventory.Items>();
		//	List<int> list = new List<int>();
		//	foreach (KeyValuePair<Inventory.Items, int> item in pc.Items)
		//		for (int i = 0; i < item.Value; i++)
		//			list.Add((int)item.Key);
		//	Items = list.ToArray();
		//}

		//public PokemonEssentials.Interface.PokeBattle.IPokemon[,] GetPokemonsFromSeri()
		//{
		//	//PokemonEssentials.Interface.PokeBattle.IPokemon[,] pkmn = new Pokemon[Pokemons.Length, Pokemons.GetLength(1)];
		//	PokemonEssentials.Interface.PokeBattle.IPokemon[,] pkmn = new Pokemon[Pokemons.Length, Pokemons[0].Length];
		//	for (int i = 0; i < Pokemons.Length; i++)
		//	{
		//		for (int j = 0; j < Pokemons[i].Length; j++)
		//		{
		//			//Easier if it only grabs actual pokemons (with values), than trying to copy everything...
		//			if((PokemonUnity.Pokemons)Pokemons[i][j].Species != PokemonUnity.Pokemons.NONE) //|| Pokemons[i][j] == null
		//				pkmn[i, j] = (PokemonEssentials.Interface.PokeBattle.IPokemon)Pokemons[i][j];
		//			else
		//				pkmn[i, j] = new PokemonEssentials.Interface.PokeBattle.IPokemon(PokemonUnity.Pokemons.NONE);
		//		}
		//	}
		//	return pkmn;
		//}

		public Inventory.Items[] GetItemsFromSeri()
		{
			Inventory.Items[] items = new Inventory.Items[0];
			if(Items != null)
				items = new Inventory.Items[Items.Length];
			for (int i = 0; i < items.Length; i++)
				items[i] = (Inventory.Items)Items[i];
			return items;
		}
		//public KeyValuePair<Inventory.Items, int>[] DeserializeItems()
		//{
		//	List<KeyValuePair<Inventory.Items, int>> l = new List<KeyValuePair<Inventory.Items, int>>();
		//	foreach (int item in Items.Distinct())
		//	{
		//		//int total = 0;
		//		int count = Items.Where(x => x == item).Count();
		//		//int groups = (int)Math.Floor(total / 99d);
		//		//for (int i = 0; i < (int)Math.Floor(count / 99d); i++)
		//		//{
		//		//	l.Add(new KeyValuePair<Items, int>(item, 99));
		//		//}
		//		//int leftovers = total % 99;
		//		//l.Add(new KeyValuePair<Items, int>(item, leftovers));
		//		l.Add(new KeyValuePair<Inventory.Items, int>(item, count));
		//	}
		//	return l.ToArray();
		//}
	}
}
