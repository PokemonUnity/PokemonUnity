using PokemonUnity.Inventory;
using PokemonUnity.Character;
using System;
using System.Collections.Generic;
using System.Linq;
using PokemonUnity.Overworld;
using PokemonUnity.Monster;
using PokemonUnity;

namespace PokemonUnity.Inventory
{
	/// <summary>
	/// Implements methods that act on arrays of items. Each element in an item
	/// array is itself an array of [itemID, itemCount]. <see cref="KeyValuePair{Items, int}"/>
	/// Used by the Bag, PC item storage, and Triple Triad.
	/// </summary>
	public static partial class ItemStorageHelper {
		/// <summary>
		/// Returns the quantity of the given item in the items array, maximum size per slot, and item ID
		/// </summary>
		/// <param name="items"></param>
		/// <param name="maxsize"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int pbQuantity(Items[] items,int maxsize,Items item) {
			int ret=0;
			for (int i = 0; i < maxsize; i++) {
				if (items[i]==item) {
					ret++;
				}
			}
			return ret;
		}

		/// <summary>
		/// Returns the quantity of the given item in the items array, maximum size per slot, and item ID
		/// </summary>
		/// <param name="items"></param>
		/// <param name="maxsize"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int pbQuantity(KeyValuePair<Items, int>[] items,int maxsize,Items item) {
			int ret=0;
			for (int i = 0; i < maxsize; i++) {
				KeyValuePair<Items, int>? itemslot=items[i];
				if (itemslot != null && itemslot.Value.Key==item) {
					ret+=itemslot.Value.Value;
				}
			}
			return ret;
		}

		/// <summary>
		/// Deletes an item from items array, maximum size per slot, item, and number of items to delete
		/// </summary>
		/// <param name="items"></param>
		/// <param name="maxsize"></param>
		/// <param name="item"></param>
		/// <param name="qty"></param>
		/// <returns></returns>
		public static bool pbDeleteItem(ref Items[] items,int maxsize,Items item,int qty) {
			if (qty<0)
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			bool ret=false;
			for (int i = 0; i < maxsize; i++, qty--) {
				if (items != null && items[i]==item) {
					//if (items[i]==0) //items[i]=null;
					{ var _ = items.ToList(); _.RemoveAt(i); items = _.ToArray(); }
					if (qty==0) {
						ret=true;
						break;
					}
				}
			}
			//items.compact();
			return ret;
		}

		/// <summary>
		/// Deletes an item from items array, maximum size per slot, item, and number of items to delete
		/// </summary>
		/// <param name="items"></param>
		/// <param name="maxsize"></param>
		/// <param name="item"></param>
		/// <param name="qty"></param>
		/// <returns></returns>
		public static bool pbDeleteItem(ref KeyValuePair<Items, int>[] items,int maxsize,Items item,int qty) {
			if (qty<0)
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			bool ret=false;
			for (int i = 0; i < maxsize; i++) {
				KeyValuePair<Items, int>? itemslot=items[i];
				if (itemslot != null && itemslot.Value.Key==item) {
					int amount=Math.Min(qty,itemslot.Value.Value);
					//itemslot.Value.Value-=amount;
					itemslot=new KeyValuePair<Items,int>(itemslot.Value.Key,itemslot.Value.Value-amount);
					qty-=amount;
					//if (itemslot.Value.Value==0) items[i]=null;
					if (itemslot.Value.Value==0) { var _ = items.ToList(); _.RemoveAt(i); items = _.ToArray(); }
					if (qty==0) {
						ret=true;
						break;
					}
				}
			}
			//items.compact();
			return ret;
		}

		public static bool pbCanStore(Items[] items,int maxsize,int maxPerSlot,Items item,int qty) {
			if (qty<0) 
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			for (int i = 0, count = 0; i < items.Length && count<maxsize * maxPerSlot; i++) {
				Items itemslot=items[i];
				if (itemslot == Items.NONE) {
					qty--;
					if (qty==0) return true;
				} else if (itemslot==item) { //&& count<maxsize
					count++; //qty--;
					if (qty==0) return true;
				}
			}
			return false;
		}

		public static bool pbCanStore(KeyValuePair<Items, int>[] items,int maxsize,int maxPerSlot,Items item,int qty) {
			if (qty<0)
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			for (int i = 0; i < maxsize; i++) {
				KeyValuePair<Items, int>? itemslot=items[i];
				if (itemslot == null) {
					qty-= Math.Min(qty,maxPerSlot);
					if (qty==0) return true;
				} else if (itemslot.Value.Key==item && itemslot.Value.Value<maxPerSlot) {
					int newamt=itemslot.Value.Value;
					newamt=Math.Min(newamt+qty,maxPerSlot);
					qty-=(newamt-itemslot.Value.Value);
					if (qty==0) return true;
				}
			}
			return false;
		}

		[System.Obsolete("Use keyvaluepair array as input param for array")] //ToDo: Refactor and Finish below...
		public static bool pbStoreItem(ref Items[] items,int maxsize,int maxPerSlot,Items item,int qty,bool sorting=false) {
			if (qty<0)
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			List<Items> list = items.ToList();
			for (int i = 0, count = 0; i < items.Length && count < maxsize * maxPerSlot; i++) {
				Items itemslot=items[i];
				if (itemslot == Items.NONE) {
					//items[i]=new KeyValuePair<Items, int> (item, Math.Min(qty, maxPerSlot));
					//qty-=items[i].Value;
					if (sorting) {
						//if (Core.POCKETAUTOSORT[ItemData[item][ITEMPOCKET]]) items.Sort();
						if (Core.POCKETAUTOSORT[(int)(Game.ItemData[item].Pocket??0)]) items.OrderBy(x => x);
					}
					if (qty==0) return true;
				} else if (itemslot==item && count<maxPerSlot) {
					//int newamt=itemslot.Value.Value;
					//newamt=Math.Min(newamt+qty,maxPerSlot);
					//qty-=(newamt-itemslot.Value.Value);
					count++;
					//itemslot.Value.Value=newamt;
					//itemslot=new KeyValuePair<Items,int>(itemslot.Value.Key,newamt);
					if (qty==0) return true;
				}
			}
			return false;
		}

		public static bool pbStoreItem(ref KeyValuePair<Items, int>[] items,int maxsize,int maxPerSlot,Items item,int qty,bool sorting=false) {
			if (qty<0)
				//throw new Exception($"Invalid value for qty: #{qty}");
				GameDebug.LogWarning($"Invalid value for qty: #{qty}");
			if (qty==0) return true;
			for (int i = 0; i < maxsize; i++) {
				KeyValuePair<Items, int>? itemslot=items[i];
				if (itemslot == null) {
					items[i]=new KeyValuePair<Items, int> (item, Math.Min(qty, maxPerSlot));
					qty-=items[i].Value;
					if (sorting) {
						//if (Core.POCKETAUTOSORT[ItemData[item][ITEMPOCKET]]) items.Sort();
						if (Core.POCKETAUTOSORT[(int)(Game.ItemData[item].Pocket??0)]) items.OrderBy(x => x.Key);
					}
					if (qty==0) return true;
				} else if (itemslot.Value.Key==item && itemslot.Value.Value<maxPerSlot) {
					int newamt=itemslot.Value.Value;
					newamt=Math.Min(newamt+qty,maxPerSlot);
					qty-=(newamt-itemslot.Value.Value);
					//itemslot.Value.Value=newamt;
					itemslot=new KeyValuePair<Items,int>(itemslot.Value.Key,newamt);
					if (qty==0) return true;
				}
			}
			return false;
		}
	}
}