using System;
using System.Collections;
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
	public partial class PCItemStorage : PokemonEssentials.Interface.Screen.IPCItemStorage, IList<Items>, ICollection<Items>
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
		int ICollection<Items>.Count { get { return items.Count; } }
		bool ICollection<Items>.IsReadOnly { get { return items.IsReadOnly; } }

		public PCItemStorage() { initialize(); }

		public PokemonEssentials.Interface.Screen.IPCItemStorage initialize()
		{
			@items = new Items[0]; //[];
			//  Start storage with a Potion
			//if (hasConst(Items,:POTION)) {
				//ItemStorageHelper.StoreItem(ref
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
			set { (this as IList<Items>).Insert(i, value); }
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

		public int Quantity(Items item)
		{
			return ItemStorageHelper.Quantity(@items.ToArray(), MAXSIZE, item);
		}

		public bool DeleteItem(Items item, int qty = 1)
		{
			Items[] i = @items.ToArray();
			//return ItemStorageHelper.DeleteItem(ref @items.ToArray(), MAXSIZE, item, qty);
			return ItemStorageHelper.DeleteItem(ref i, MAXSIZE, item, qty);
		}

		public bool CanStore(Items item, int qty = 1)
		{
			return ItemStorageHelper.CanStore(@items.ToArray(), MAXSIZE, MAXPERSLOT, item, qty);
		}

		public bool StoreItem(Items item, int qty = 1)
		{
			//Items[] i = @items.ToArray();
			//return ItemStorageHelper.StoreItem(@items.ToArray(), MAXSIZE, MAXPERSLOT, item, qty);
			//return ItemStorageHelper.StoreItem(ref items.ToArray(), MAXSIZE, MAXPERSLOT, item, qty);
			return false; //ToDo: Uncomment and Finish
		}

		#region Interface Methods
		void ICollection<Items>.Add(Items item)
		{
			//if there is space add to box
			//if (!items.Contains(item) && item != Items.NONE)
			//	//items.Add(item);
			//	ItemStorageHelper.StoreItem(ref items.ToArray(), MAXSIZE, MAXPERSLOT, item, 1);
		}

		void ICollection<Items>.Clear()
		{
			//items.Clear();
			initialize();
		}

		bool ICollection<Items>.Contains(Items item)
		{
			//should return false for both null and none
			if (item == Items.NONE) return false;
			return items.Contains(item);
		}

		void ICollection<Items>.CopyTo(Items[] array, int arrayIndex)
		{
			items.CopyTo(array, arrayIndex);
		}

		bool ICollection<Items>.Remove(Items item)
		{
			if (item == Items.NONE) return true;
			//if object is removed, it will remove nulls and empty space
			return items.Remove(item);
		}

		IEnumerator<Items> IEnumerable<Items>.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return items.GetEnumerator();
		}

		int IList<Items>.IndexOf(Items item)
		{
			if (item == Items.NONE) return -1;
			//return ((IList<PokemonEssentials.Interface.PokeBattle.Items>)items).IndexOf(item);
			return items.IndexOf(item);
		}

		void IList<Items>.Insert(int index, Items item)
		{
			if (item == Items.NONE) return;
			//return ((IList<PokemonEssentials.Interface.PokeBattle.Items>)items).Insert(index, item);
			items.Insert(index, item);
		}

		void IList<Items>.RemoveAt(int index)
		{
			//((IList<PokemonEssentials.Interface.PokeBattle.Items>)items).RemoveAt(index);
			items.RemoveAt(index);
		}
		#endregion
	}
}