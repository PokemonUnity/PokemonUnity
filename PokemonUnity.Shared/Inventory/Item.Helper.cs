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
/// Implements methods that act on arrays of items.  Each element in an item
/// array is itself an array of [itemID, itemCount].
/// Used by the Bag, PC item storage, and Triple Triad.
/// </summary>
public static partial class ItemStorageHelper {
//  Returns the quantity of the given item in the items array, maximum size per slot, and item ID
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

//  Deletes an item from items array, maximum size per slot, item, and number of items to delete
  public static bool pbDeleteItem(ref KeyValuePair<Items, int>[] items,int maxsize,Items item,int qty) {
    if (qty<0) throw new Exception($"Invalid value for qty: #{qty}");
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

  public static bool pbCanStore(KeyValuePair<Items, int>[] items,int maxsize,int maxPerSlot,Items item,int qty) {
    if (qty<0) throw new Exception($"Invalid value for qty: #{qty}");
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

  public static bool pbStoreItem(ref KeyValuePair<Items, int>[] items,int maxsize,int maxPerSlot,Items item,int qty,bool sorting=false) {
    if (qty<0) throw new Exception($"Invalid value for qty: #{qty}");
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