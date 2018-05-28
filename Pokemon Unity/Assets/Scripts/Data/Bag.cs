//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using PokemonUnity.Item;

[System.Serializable]
public class Bag
{
    private string[] orderString;
    /// <summary>
    /// in combination with quantity[], 
    /// one holds the itemId and the other has amount
    /// </summary>
    /// <remarks>if use <see cref="Items"/> might be less on memory</remarks>
    private ItemDataOld[] order;
    private int[] quantity;

    public Bag()
    {
        orderString = new string[ItemDatabaseOld.getItemsLength()];
        quantity = new int[ItemDatabaseOld.getItemsLength()];
    }

    /// <summary>
    /// deprecated
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int getIndexOf(string name)
    {
        for (int i = 0; i < orderString.Length; i++)
        {
            if (orderString[i] == name)
            {
                return i;
            }
        }
        return -1;
    }

    public int getIndexOf(ItemDataOld name)
    {
        for (int i = 0; i < order.Length; i++)
        {
            if (order[i] == name)
            {
                return i;
            }
        }
        return -1;
    }

    public int getIndexOf(eItems.Item name)
    {
        for (int i = 0; i < order.Length; i++)
        {
            if (order[i].getItemId() == name)
            {
                return i;
            }
        }
        return -1;
    }

    public void moveBehind(int targetIndex, int destinationIndex)
    {
        string temp = orderString[targetIndex];
        string[] packedOrder = new string[orderString.Length];

        orderString[targetIndex] = null;

        if (Mathf.Abs(targetIndex - destinationIndex) == 1)
        {
            orderString[targetIndex] = orderString[destinationIndex];
            orderString[destinationIndex] = temp;
        }
        else
        {
            int packedIndex = 0;
            for (int i = 0; i < orderString.Length; i++)
            {
                if (i == destinationIndex)
                {
                    packedOrder[packedIndex] = temp;
                    packedIndex += 1;
                }
                if (orderString[i] != null)
                {
                    packedOrder[packedIndex] = orderString[i];
                    packedIndex += 1;
                }
            }
            orderString = packedOrder;
        }
    }


    public int getLength()
    {
        int length = 0;
        for (int i = 0; i < orderString.Length; i++)
        {
            if (ItemDatabaseOld.getItem(orderString[i]) != null)
            {
                length += 1;
            }
        }
        return length;
    }

    public void packOrder()
    {
        string[] packedOrder = new string[orderString.Length];
        int packedIndex = 0;
        for (int i = 0; i < orderString.Length; i++)
        {
            if (orderString[i] != null)
            {
                packedOrder[packedIndex] = orderString[i];
                packedIndex += 1;
            }
        }
        orderString = packedOrder;
    }

    /// <summary>
    /// Do not use this; deprecated
    /// </summary>
    /// <param name="itemName"></param>
    /// <returns></returns>
    public int getQuantity(string itemName)
    {
        int index = ItemDatabaseOld.getIndexOf(itemName);
        if (index != -1)
        {
            return quantity[index];
        }
        return 0;
    }

    public int getQuantity(ItemDataOld itemName)
    {
        int index = ItemDatabaseOld.getIndexOf(itemName); //Bag.getIndexOf? Why go to itemdatabase?
        if (index != -1)
        {
            return quantity[index];
        }
        return 0;
    }

    public int getQuantity(eItems.Item itemName)
    {
        int index = ItemDatabaseOld.getIndexOf(itemName); //Bag.getIndexOf? Why go to itemdatabase?
        if (index != -1)
        {
            return quantity[index];
        }
        return 0;
    }

    public bool addItem(string itemName, int amount)
    {
        //returns false if will exceed the quantity limit (999)
        packOrder();
        string name = ItemDatabaseOld.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag, add it to the end
            index = getLength();
            orderString[index] = name;
        }
        index = ItemDatabaseOld.getIndexOf(orderString[index]);
        if (quantity[index] + amount > 999)
        {
            return false;
        }
        quantity[index] += amount;
        return true;
    }

    public bool addItem(ItemDataOld itemName, int amount = 1)
    {
        //returns false if will exceed the quantity limit (999)
        packOrder();
        //string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(itemName);
        if (index == -1)
        {
            //item does not exist in bag, add it to the end
            index = getLength();
            order[index] = itemName;
        }
        index = ItemDatabaseOld.getIndexOf(order[index]);
        if (quantity[index] + amount > 999)
        {
            return false;
        }
        quantity[index] += amount;
        return true;
    }

    public bool addItem(eItems.Item itemName, int amount = 1)
    {
        //returns false if will exceed the quantity limit (999)
        packOrder();
        //string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(itemName);
        if (index == -1)
        {
            //item does not exist in bag, add it to the end
            index = getLength();
            order[index] = ItemDatabaseOld.getItem(itemName);
        }
        index = ItemDatabaseOld.getIndexOf(order[index]);
        if (quantity[index] + amount > 999)
        {
            return false;
        }
        quantity[index] += amount;
        return true;
    }

    public bool removeItem(string itemName, int amount)
    {
        //returns false if trying to remove more items than exist
        packOrder();
        string name = ItemDatabaseOld.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag
            return false;
        }
        index = ItemDatabaseOld.getIndexOf(orderString[index]);
        if (quantity[index] - amount < 0)
        {
            return false;
        }
        quantity[index] -= amount;
        if (quantity[index] == 0)
        {
            orderString[getIndexOf(name)] = null;
            packOrder();
        }
        return true;
    }

    public bool removeItem(ItemDataOld itemName, int amount = 1) //ToDo: amount should default to 'ALL'
    {
        //returns false if trying to remove more items than exist
        packOrder();
        //string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(itemName);
        if (index == -1)
        {
            //item does not exist in bag
            return false;
        }
        index = ItemDatabaseOld.getIndexOf(orderString[index]);
        if (quantity[index] - amount < 0)
        {
            return false;
        }
        quantity[index] -= amount;
        if (quantity[index] == 0)
        {
            orderString[getIndexOf(itemName)] = null;
            packOrder();
        }
        return true;
    }

    public bool removeItem(eItems.Item itemName, int amount = 1)  //ToDo: amount should default to 'ALL'
    {
        //returns false if trying to remove more items than exist
        packOrder();
        //string name = ItemDatabase.getItem(itemName).getName(); //ensures that the name is correct
        int index = getIndexOf(itemName);
        if (index == -1)
        {
            //item does not exist in bag
            return false;
        }
        index = ItemDatabaseOld.getIndexOf(orderString[index]);
        if (quantity[index] - amount < 0)
        {
            return false;
        }
        quantity[index] -= amount;
        if (quantity[index] == 0)
        {
            orderString[getIndexOf(itemName)] = null;
            packOrder();
        }
        return true;
    }


    public string[] getSellableItemArray()
    {
        return getItemTypeArray(ItemDataOld.ItemType.ITEM, true);
    }

    public string[] getItemTypeArray(ItemDataOld.ItemType itemType)
    {
        return getItemTypeArray(itemType, false);
    }

    private string[] getItemTypeArray(ItemDataOld.ItemType itemType, bool allSellables)
    {
        packOrder();
        string[] result = new string[getLength()];
        int resultPos = 0;
        int length = getLength();
        //cycle through order, adding all correct ItemTypes to result
        for (int i = 0; i < length; i++)
        {
            if (!allSellables)
            {
                if (ItemDatabaseOld.getItem(orderString[i]).getItemType() == itemType)
                {
                    //if correct ItemType
                    result[resultPos] = orderString[i];
                    resultPos += 1;
                }
            }
            else
            {
                if (ItemDatabaseOld.getItem(orderString[i]).getItemType() == ItemDataOld.ItemType.ITEM ||
                    ItemDatabaseOld.getItem(orderString[i]).getItemType() == ItemDataOld.ItemType.MEDICINE ||
                    ItemDatabaseOld.getItem(orderString[i]).getItemType() == ItemDataOld.ItemType.BERRY)
                {
                    //if correct ItemType
                    result[resultPos] = orderString[i];
                    resultPos += 1;
                }
            }
        }
        string[] cleanedResult = new string[resultPos];
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            cleanedResult[i] = result[i];
        }

        return cleanedResult;
    }

    public string[] getBattleTypeArray(ItemDataOld.BattleType battleType)
    {
        packOrder();
        string[] result = new string[getLength()];
        int resultPos = 0;
        int length = getLength();
        //cycle through order, adding all correct ItemTypes to result
        for (int i = 0; i < length; i++)
        {
            if (ItemDatabaseOld.getItem(orderString[i]).getBattleType() == battleType)
            {
                //if correct ItemType
                result[resultPos] = orderString[i];
                resultPos += 1;
            }
        }
        string[] cleanedResult = new string[resultPos];
        for (int i = 0; i < cleanedResult.Length; i++)
        {
            cleanedResult[i] = result[i];
        }

        return cleanedResult;
    }
}