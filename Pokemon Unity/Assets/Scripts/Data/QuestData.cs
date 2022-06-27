using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class QuestData
{
    private int id;
    private string name;
    private QuestReward reward;
    
    public QuestData() : this(0, "Test", new QuestReward()) {}
    
    public QuestData(int id, string name, QuestReward reward)
    {
        this.id = id;
        this.name = name;
        this.reward = reward;
    }

    public int getMoneyReward()
    {
        return reward.getMoney();
    }
    
    public ItemData[] getItemsReward()
    {
        return reward.getItems();
    }
}

public class QuestReward
{
    private int money;
    private ItemData[] items;
    private int[] itemsAmount;

    //public QuestReward() : this(0, Array.Empty<ItemData>(), Array.Empty<int>()) {}
    public QuestReward() : this(0, new ItemData[0], new int[0]) {}
        
    public QuestReward(int money, ItemData[] items, int[] itemsAmount)
    {
        this.money = money;
        this.items = items;
        this.itemsAmount = itemsAmount;
    }

    //public QuestReward(int money) : this(money, Array.Empty<ItemData>(), Array.Empty<int>()) {}
    public QuestReward(int money) : this(money, new ItemData[0], new int[0]) {}

    public QuestReward(ItemData[] items, int[] itemsAmount) : this(0, items, itemsAmount) {}


    public int getMoney()
    {
        return money;
    }

    public ItemData[] getItems()
    {
        return items;
    }
}