using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;

namespace PokemonUnity.Monster.Data
{
	/// <summary>
	/// The items that Pokémon when encountered in the wild have a chance of carrying.
	/// </summary>
	public struct PokemonWildItems : IEquatable<PokemonWildItems>, IEqualityComparer<PokemonWildItems>
	{
		/// <summary>
		/// </summary>
		//public byte VersionGroupId;
		public int Generation;
		/// <summary>
		/// </summary>
		public int Rarirty;
		/// <summary>
		/// </summary>
		public IList<Items> ItemId;
		public Pokemons Species;
		public PokemonWildItems(Pokemons pkmn, int rarity = 0, int generation = 0, params Items[] itemId)
		{
			this.ItemId = itemId??new Items[] { Items.NONE };
			this.Rarirty = rarity;
			this.Generation = generation;
			this.Species = pkmn;
			if (this.ItemId.Count==0) this.ItemId = new Items[] { Items.NONE };
		}

		/// <summary>
		/// Pools all the values into a 100% encounter chance, and selects from those results
		/// </summary>
		/// <param name="pokemon"></param>
		/// <returns></returns>
		/// RNG Bagging Technique using Dice Roll, without fallback (no matter rng, wont artificially modify results)
		public static Items GetWildHoldItem(Pokemons pokemon)
		{
			List<Items> list = new List<Items>();

			//loop through each position of list
			foreach (PokemonWildItems item in Kernal.PokemonItemsData[pokemon])
			{
				//add encounter once for every Likelihood
				for (int i = 0; i < item.Rarirty; i++)
				{
					list.Add(item.ItemId[0]);
				}
			}

			//Get list of 100 pokemons for given (specific to this) encounter...
			for(int n = list.Count; n < 100; n++)
			{
				list.Add(Items.NONE);
			}

			//From list of 100 pokemons, select 1.
			return list[Core.Rand.Next(list.Count)];
		}

		#region Explicit Operators
		public static bool operator ==(PokemonWildItems x, PokemonWildItems y)
		{
			return x.Species == y.Species;
		}
		public static bool operator !=(PokemonWildItems x, PokemonWildItems y)
		{
			return x.Species != y.Species;
		}
		//public bool Equals(Items obj)
		//{
		//	return this.ItemId == obj;
		//}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			//if (obj.GetType() == typeof(PokemonWildItems))
			//	return Equals(obj as Pokemon);
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			//return (int)this.ItemId ^ Generation.GetHashCode();
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + ((int)this.Species).GetHashCode();
				hash = hash * 23 + this.Generation.GetHashCode();
				return hash;
			}
		}
		bool IEquatable<PokemonWildItems>.Equals(PokemonWildItems other)
		{
			return Equals(obj: (object)other);
		}
		bool IEqualityComparer<PokemonWildItems>.Equals(PokemonWildItems x, PokemonWildItems y)
		{
			return x == y;
		}
		int IEqualityComparer<PokemonWildItems>.GetHashCode(PokemonWildItems obj)
		{
			return obj.GetHashCode();
		}
		#endregion
	}
}