//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class NonResettingHandler : MonoBehaviour
{
    //Non resetters are things like trainers and items. These are remembered by the save file so that they don't reappear.

    private static NonResettingHandler currentActive;

    private NonResettingList listOfNonResetters;

    private InteractTrainer[] trainers;
    private InteractItem[] items;
    private GameObject[] events;

    void Awake()
    {
        if (currentActive == null)
        {
            currentActive = this;
        }
        else if (currentActive != this)
        {
            this.enabled = false;
        }

        Transform trainersList = transform.Find("Trainers");
        Transform itemsList = transform.Find("Items");
        Transform eventsList = transform.Find("Events");

        trainers = new InteractTrainer[trainersList.childCount];
        items = new InteractItem[itemsList.childCount];
        events = new GameObject[eventsList.childCount];

        for (int i = 0; i < trainers.Length; i++)
        {
            trainers[i] = trainersList.GetChild(i).GetComponent<InteractTrainer>();
        }
        for (int i = 0; i < items.Length; i++)
        {
            items[i] = itemsList.GetChild(i).GetComponent<InteractItem>();
        }
        for (int i = 0; i < events.Length; i++)
        {
            events[i] = eventsList.GetChild(i).gameObject;
        }
    }

    void Start()
    {
        int sceneNonResettingListIndex = SaveData.currentSave.getNonResettingListIndex(Application.loadedLevelName);

        //if entry is already in global, update everything to match
        if (sceneNonResettingListIndex >= 0)
        {
            NonResettingList sceneNonResettingList = SaveData.currentSave.nonResettingLists[sceneNonResettingListIndex];

            for (int i = 0; i < trainers.Length; i++)
            {
                if (i < sceneNonResettingList.sceneTrainers.Length)
                {
                    trainers[i].defeated = sceneNonResettingList.sceneTrainers[i];
                }
                else
                {
                    Debug.Log("NonResettingList Inconsistancy for" + trainers[i].gameObject.name);
                }
            }
            for (int i = 0; i < items.Length; i++)
            {
                if (i < sceneNonResettingList.sceneItems.Length)
                {
                    if (!sceneNonResettingList.sceneItems[i])
                    {
                        items[i].disableItem();
                    }
                }
                else
                {
                    Debug.Log("NonResettingList Inconsistancy for" + items[i].name);
                }
            }
            for (int i = 0; i < events.Length; i++)
            {
                if (i < sceneNonResettingList.sceneEvents.Length)
                {
                    events[i].SetActive(sceneNonResettingList.sceneEvents[i]);
                }
                else
                {
                    Debug.Log("NonResettingList Inconsistancy for" + events[i].name);
                }
            }
        }
    }

    private NonResettingList compileListOfNonResetters()
    {
        bool[] sceneTrainers = new bool[trainers.Length];
        bool[] sceneItems = new bool[items.Length];
        bool[] sceneEvents = new bool[events.Length];

        for (int i = 0; i < trainers.Length; i++)
        {
            sceneTrainers[i] = trainers[i].defeated;
        }
        for (int i = 0; i < items.Length; i++)
        {
            sceneItems[i] = items[i].gameObject.activeSelf;
        }
        for (int i = 0; i < events.Length; i++)
        {
            sceneEvents[i] = events[i].activeSelf;
        }

        if (sceneTrainers.Length == 0 && sceneItems.Length == 0 && sceneEvents.Length == 0)
        {
            return null;
        } //return null when there actually isn't anything in any of the arrays
        return new NonResettingList(Application.loadedLevelName, sceneTrainers, sceneItems, sceneEvents);
    }

    public static void saveDataToGlobal()
    {
        if (currentActive != null)
        {
            NonResettingList thisNonResettingList = currentActive.compileListOfNonResetters();

            if (thisNonResettingList != null)
            {
                bool listUpdated = false;
                for (int i = 0; i < SaveData.currentSave.nonResettingLists.Count; i++)
                {
                    if (SaveData.currentSave.nonResettingLists[i].sceneName == thisNonResettingList.sceneName)
                    {
                        SaveData.currentSave.nonResettingLists[i] = thisNonResettingList;
                        listUpdated = true;
                        i = SaveData.currentSave.nonResettingLists.Count;
                    }
                }
                if (!listUpdated)
                {
                    SaveData.currentSave.nonResettingLists.Add(thisNonResettingList);
                }
            }
        }
    }
}

[System.Serializable]
public class NonResettingList
{
    public string sceneName;

    public bool[] sceneTrainers;
    public bool[] sceneItems;
    public bool[] sceneEvents;

    public NonResettingList(string sceneName, bool[] sceneTrainers, bool[] sceneItems, bool[] sceneEvents)
    {
        this.sceneName = sceneName;
        this.sceneTrainers = sceneTrainers;
        this.sceneItems = sceneItems;
        this.sceneEvents = sceneEvents;
    }
}