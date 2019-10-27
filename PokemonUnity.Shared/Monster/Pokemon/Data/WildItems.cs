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

		#region Explicit Operators
		public bool Equals(Items obj)
		{
			return this.ItemId == obj;
		}
		public override bool Equals(object obj)
		{
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