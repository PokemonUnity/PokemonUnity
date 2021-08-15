//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;

[System.Serializable]
public class Bag
{
    private string[] order;
    private int[] quantity;

    public Bag()
    {
        order = new string[ItemDatabase.getItemsLength()];
        quantity = new int[ItemDatabase.getItemsLength()];
    }

    public int getIndexOf(string name)
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

    public void moveBehind(int targetIndex, int destinationIndex)
    {
        string temp = order[targetIndex];
        string[] packedOrder = new string[order.Length];

        order[targetIndex] = null;

        if (Mathf.Abs(targetIndex - destinationIndex) == 1)
        {
            order[targetIndex] = order[destinationIndex];
            order[destinationIndex] = temp;
        }
        else
        {
            int packedIndex = 0;
            for (int i = 0; i < order.Length; i++)
            {
                if (i == destinationIndex)
                {
                    packedOrder[packedIndex] = temp;
                    packedIndex += 1;
                }
                if (order[i] != null)
                {
                    packedOrder[packedIndex] = order[i];
                    packedIndex += 1;
                }
            }
            order = packedOrder;
        }
    }


    public int getLength()
    {
        int length = 0;
        for (int i = 0; i < order.Length; i++)
        {
            if (ItemDatabase.getItem(order[i]) != null)
            {
                length += 1;
            }
        }
        return length;
    }

    public void packOrder()
    {
        string[] packedOrder = new string[order.Length];
        int packedIndex = 0;
        for (int i = 0; i < order.Length; i++)
        {
            if (order[i] != null)
            {
                packedOrder[packedIndex] = order[i];
                packedIndex += 1;
            }
        }
        order = packedOrder;
    }

    public int getQuantity(string itemName)
    {
        int index = ItemDatabase.getIndexOf(itemName);
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
        string name = ItemDatabase.getItem(itemName).Name; //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag, add it to the end
            index = getLength();
            order[index] = name;
        }
        index = ItemDatabase.getIndexOf(order[index]);
        if (quantity[index] + amount > 999)
        {
            return false;
        }
        quantity[index] += amount;
        return true;
    }

    public bool addItem(PokemonUnity.Inventory.Items itemName, int amount)
    {
        //returns false if will exceed the quantity limit (999)
        packOrder();
        string name = ItemDatabase.getItem(itemName).Name; //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag, add it to the end
            index = getLength();
            order[index] = name;
        }
        index = ItemDatabase.getIndexOf(order[index]);
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
        string name = ItemDatabase.getItem(itemName).Name; //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag
            return false;
        }
        index = ItemDatabase.getIndexOf(order[index]);
        if (quantity[index] - amount < 0)
        {
            return false;
        }
        quantity[index] -= amount;
        if (quantity[index] == 0)
        {
            order[getIndexOf(name)] = null;
            packOrder();
        }
        return true;
    }

    public bool removeItem(PokemonUnity.Inventory.Items itemName, int amount)
    {
        //returns false if trying to remove more items than exist
        packOrder();
        string name = ItemDatabase.getItem(itemName).Name; //ensures that the name is correct
        int index = getIndexOf(name);
        if (index == -1)
        {
            //item does not exist in bag
            return false;
        }
        index = ItemDatabase.getIndexOf(order[index]);
        if (quantity[index] - amount < 0)
        {
            return false;
        }
        quantity[index] -= amount;
        if (quantity[index] == 0)
        {
            order[getIndexOf(name)] = null;
            packOrder();
        }
        return true;
    }


    public string[] getSellableItemArray()
    {
        return getItemTypeArray(ItemType.ITEM, true);
    }

    public string[] getItemTypeArray(ItemType itemType)
    {
        return getItemTypeArray(itemType, false);
    }

    public string[] getItemTypeArray(ItemType[] itemType)
    {
        return getItemTypeArrays(itemType);
    }

    private string[] getItemTypeArrays(ItemType[] itemType)
    {
        packOrder();
        string[] result = new string[getLength()];
        int resultPos = 0;
        int length = getLength();
        //cycle through order, adding all correct ItemTypes to result
        for (int i = 0; i < length; i++)
        {
            for (int num = 0; num < itemType.Length; num++)
            {
                if (ItemDatabase.getItem(order[i]).ItemType == itemType[num])
                {
                    //if correct ItemType
                    result[resultPos] = order[i];
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

    private string[] getItemTypeArray(ItemType itemType, bool allSellables)
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
                if (ItemDatabase.getItem(order[i]).ItemType == itemType)
                {
                    //if correct ItemType
                    result[resultPos] = order[i];
                    resultPos += 1;
                }
            }
            else
            {
                if (ItemDatabase.getItem(order[i]).ItemType == ItemType.ITEM ||
                    ItemDatabase.getItem(order[i]).ItemType == ItemType.MEDICINE ||
                    ItemDatabase.getItem(order[i]).ItemType == ItemType.EVOLUTION || // Evolution?
                    ItemDatabase.getItem(order[i]).ItemType == ItemType.BERRY)
                {
                    //if correct ItemType
                    result[resultPos] = order[i];
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

    //public string[] getBattleTypeArray(ItemData.BattleType battleType)
    //{
    //    packOrder();
    //    string[] result = new string[getLength()];
    //    int resultPos = 0;
    //    int length = getLength();
    //    //cycle through order, adding all correct ItemTypes to result
    //    for (int i = 0; i < length; i++)
    //    {
    //        if (ItemDatabase.getItem(order[i]).getBattleType() == battleType)
    //        {
    //            //if correct ItemType
    //            result[resultPos] = order[i];
    //            resultPos += 1;
    //        }
    //    }
    //    string[] cleanedResult = new string[resultPos];
    //    for (int i = 0; i < cleanedResult.Length; i++)
    //    {
    //        cleanedResult[i] = result[i];
    //    }
    //
    //    return cleanedResult;
    //}
}