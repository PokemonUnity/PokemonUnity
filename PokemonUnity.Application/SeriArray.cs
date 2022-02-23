using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;
using PokemonEssentials.Interface.PokeBattle;

namespace PokemonUnity.Saving.SerializableClasses
{
	public static class SeriArray
	{
		public static SeriPokemon[] Serialize (this IBattler[] pkmn)
		{
			if (pkmn != null)
			{
				SeriPokemon[] p = new SeriPokemon[pkmn.Length];
				for (int i = 0; i < p.Length; i++)
					p[i] = pkmn[i].pokemon;
				return p;
			}
			else return new SeriPokemon[0];
		}
		public static IPokemon[] Deserialize (this SeriPokemon[] pkmn)
		{
			if (pkmn != null)
			{
				Pokemon[] p = new Pokemon[pkmn.Length];
				for (int i = 0; i < p.Length; i++)
					p[i] = pkmn[i];
				return p;
			}
			else return new Pokemon[0];
		}
		public static SeriPokemon[] Serialize (this IPokemon[] pkmn)
		{
			if (pkmn != null)
			{
				SeriPokemon[] p = new SeriPokemon[pkmn.Length];
				for (int i = 0; i < p.Length; i++)
					p[i] = pkmn[i];
				return p;
			}
			else return new SeriPokemon[0];
		}
		public static IPokemon[][] Deserialize (this SeriPokemon[][] pkmn)
		{
			if (pkmn != null)
			{
				Pokemon[][] pkmns = new Pokemon[pkmn.Length][];
				for (int i = 0; i < pkmns.Length; i++)
					pkmns[i] = pkmn[i].Deserialize();
				return pkmns;
			}
			else
			{
				//Pokemon[][] pkmns = new Pokemon[Core.STORAGEBOXES][];
				Pokemon[][] pkmns = new Pokemon[0][];
				for (int i = 0; i < pkmns.Length; i++)
				{
					pkmns[i] = new Pokemon[0];
					for (int j = 0; j < pkmns[i].Length; j++)
					{
						pkmns[i][j] = new Pokemon(Pokemons.NONE);
					}
				}
				return pkmns;
			}
		}
		public static SeriPokemon[][] Serialize (this IPokemon[][] pkmn)
		{
			if (pkmn != null)
			{
				SeriPokemon[][] pkmns = new SeriPokemon[pkmn.Length][];
				for (int i = 0; i < pkmns.Length; i++)
					pkmns[i] = pkmn[i].Serialize();
				return pkmns;
			}
			else
			{
				//SeriPokemon[][] pkmns = new SeriPokemon[Core.STORAGEBOXES][];
				SeriPokemon[][] pkmns = new SeriPokemon[0][];
				for (int i = 0; i < pkmns.Length; i++)
				{
					pkmns[i] = new SeriPokemon[0];
					for (int j = 0; j < pkmns[i].Length; j++)
					{
						pkmns[i][j] = (SeriPokemon)new Pokemon(Pokemons.NONE);
					}
				}
				return pkmns;
			}
		}
		public static KeyValuePair<Items, int>[] Compress (this Items[] items)
		{
				List<KeyValuePair<Items, int>> l = new List<KeyValuePair<Items, int>>();
				foreach (Items item in items.Distinct())
				{
					//int total = 0;
					int count = items.Where(x => x == item).Count();
					//int groups = (int)Math.Floor(total / 99d);
					//for (int i = 0; i < (int)Math.Floor(count / 99d); i++)
					//{
					//	l.Add(new KeyValuePair<Items, int>(item, 99));
					//}
					//int leftovers = total % 99;
					//l.Add(new KeyValuePair<Items, int>(item, leftovers));
					l.Add(new KeyValuePair<Items, int>(item, count));
				}
				return l.ToArray();
		}
		//public static SeriPC[] Serialize (this Character.PC[] item)
		//{
		//	return new SeriPokemon[]
		//	{
		//		pkmn[0],
		//		pkmn[1],
		//		pkmn[2],
		//		pkmn[3],
		//		pkmn[4],
		//		pkmn[5]
		//	};
		//}
	}
}
