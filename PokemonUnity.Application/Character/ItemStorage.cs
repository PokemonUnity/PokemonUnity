using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonUnity;
using PokemonUnity.Monster;
using PokemonUnity.Attack;
using PokemonUnity.Inventory;
using PokemonUnity.Saving.SerializableClasses;
using PokemonUnity.Character;

namespace PokemonUnity.Character
{
	/// <summary>
	/// The PC item storage object, which actually contains all the items
	/// </summary>
	public partial class PCItemStorage : PokemonEssentials.Interface.Screen.IPCItemStorage
	{
		/// <summary>
        /// Number of different slots in storage
        /// </summary>
		public const int MAXSIZE = 50;
		/// <summary>
        /// Max. number of items per slot
        /// </summary>
		public const int MAXPERSLOT = 999;
		private IList<Items> items;


		public PCItemStorage() { initialize(); }
		public PokemonEssentials.Interface.Screen.IPCItemStorage initialize()
		{
			@items = new Items[0]; //[];
			//  Start storage with a Potion
			//if (hasConst(PBItems,:POTION)) {
				//ItemStorageHelper.pbStoreItem(ref
				//   @items.ToArray(), MAXSIZE, MAXPERSLOT, Items.POTION, 1);
			//}
			return this;
		}

		public bool empty()
		{
			return @items.Count == 0;
		}

		public int length()
		{
			return @items.Count;
		}

		public Items this[int i]
		{
			get
			{
				return @items[i];
			}
		}

		public Items getItem(int index)
		{
			if (index < 0 || index >= @items.Count)
			{
				return 0;
			}
			else
			{
				return @items[index];//[0];
				//return @items[index].Key;//[0];
			}
		}

		public int getCount(int index)
		{
			if (index < 0 || index >= @items.Count)
			{
				return 0;
			}
			else
			{
				//return @items[index].Value;//[1];
				return @items.Count;
			}
		}

		public int pbQuantity(Items item)
		{
			return ItemStorageHelper.pbQuantity(@items.ToArray(), MAXSIZE, item);
		}

		public bool pbDeleteItem(Items item, int qty = 1)
		{
			Items[] i = @items.ToArray();
			//return ItemStorageHelper.pbDeleteItem(ref @items.ToArray(), MAXSIZE, item, qty);
			return ItemStorageHelper.pbDeleteItem(ref i, MAXSIZE, item, qty);
		}

		public bool pbCanStore(Items item, int qty = 1)
		{
			return ItemStorageHelper.pbCanStore(@items.ToArray(), MAXSIZE, MAXPERSLOT, item, qty);
		}

		public bool pbStoreItem(Items item, int qty = 1)
		{
			Items[] i = @items.ToArray();
			//return ItemStorageHelper.pbStoreItem(@items.ToArray(), MAXSIZE, MAXPERSLOT, item, qty);
			return ItemStorageHelper.pbStoreItem(ref i, MAXSIZE, MAXPERSLOT, item, qty);
		}
	}
}