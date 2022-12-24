using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataBase
{
    private static QuestData[] quests =
    {
        new QuestData(1, "Debug Quest", new QuestReward(100, 
            new [] {ItemDatabase.getItem("Poké Ball")},  new [] {4}))
    };

    public static QuestData GetQuestData(int i)
    {
        if (i < 0 || i >= quests.Length) return null;
        return quests[i];
    }
}
