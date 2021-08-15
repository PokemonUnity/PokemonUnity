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
	/// The items that Pokémon when encountered in the wild have a chance of carring. 
	/// </summary>
	public struct PokemonWildItems
	{
		/// <summary>
		/// </summary>
		//public byte VersionId;
		public int Generation;
		/// <summary>
		/// </summary>
		public int Rarirty;
		/// <summary>
		/// </summary>
		public Items ItemId;
		public PokemonWildItems(Items itemId, int rarity = 0, int generation = 0)
		{
			this.ItemId = itemId;
			this.Rarirty = rarity;
			this.Generation = generation;
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
			foreach (PokemonWildItems item in Game.PokemonItemsData[pokemon])
			{
				//add encounter once for every Likelihood
				for (int i = 0; i < item.Rarirty; i++)
				{
					list.Add(item.ItemId);
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
		public bool Equals(Items obj)
		{
			return this.ItemId == obj;
		}
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			//return (int)this.ItemId ^ Generation.GetHashCode();
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + ((int)this.ItemId).GetHashCode();
				hash = hash * 23 + this.Generation.GetHashCode();
				return hash;
			}
		}
		#endregion
	}
}