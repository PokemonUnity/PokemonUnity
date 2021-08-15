//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections.Generic;


public class SaveData
{
    [System.Serializable]
    public class SaveFile
    {
        public int fileIndex;
        public int buildID;

        public string levelName;
        public SeriV3 playerPosition = new SeriV3(new Vector3());
        public int playerDirection;
        
        public string playerName;
        public bool isMale;
        public int playerID;
        public string fileCreationDate;

        public string mapName;

        public SeriPC PC;
        public Bag Bag = new Bag();

        public string[] registeredItems;

        public string playerOutfit;

        public int playerScore;
        public int playerMoney;
        
        public int playerHours;
        public int playerMinutes;
        public int playerSeconds;
        
        public bool[] gymsEncountered;
        public bool[] gymsBeaten;
        public string[] gymsBeatTime;


        //Important gameplay data
        public string respawnSceneName;
        public SeriV3 respawnScenePosition;
        public int respawnSceneDirection;
        public string respawnText;

        public List<CVariable> cVariables = new List<CVariable>();
        public List<NonResettingList> nonResettingLists = new List<NonResettingList>();

        public SaveFile()
        {

        }

        // Need this?
        //public SaveData ToSaveData()
        //{
        //    SaveData savedata = new SaveData(fileIndex); savedata.buildID = buildID;
        //    savedata.levelName = levelName; savedata.playerPosition = playerPosition; savedata.playerDirection = playerDirection;
        //    savedata.playerName = playerName; savedata.isMale = isMale; savedata.playerID = playerID; savedata.fileCreationDate = fileCreationDate;
        //    savedata.mapName = mapName; savedata.registeredItems = registeredItems; savedata.playerOutfit = playerOutfit; savedata.playerScore = playerScore;
        //    savedata.playerMoney = playerMoney; savedata.playerHours = playerHours; savedata.playerMinutes = playerMinutes; savedata.playerSeconds = playerSeconds;
        //    savedata.gymsEncountered = gymsEncountered; savedata.gymsBeaten = gymsBeaten; savedata.gymsBeatTime = gymsBeatTime; savedata.respawnSceneName = respawnSceneName;
        //    savedata.respawnScenePosition = respawnScenePosition; savedata.respawnSceneDirection = respawnSceneDirection; savedata.respawnText = respawnText;
        //    savedata.cVariables = cVariables; savedata.nonResettingLists = nonResettingLists;
        //    //
        //    savedata.PC = PC.GetPC();
        //    savedata.Bag = Bag;
        //    return savedata;
        //}
    }

    public static SaveData currentSave;
    public SaveFile savefile;

    public PC PC;
    public Bag Bag { get { return savefile.Bag; } }

    public bool IsNotNullFile() { return savefile != null; }

    public SaveData(int fileIndex)
    {
        savefile = new SaveFile();
        savefile.fileIndex = fileIndex;
        PC = new PC();
        Save();
    }

    public SaveData(SaveFile loadData)
    {
        savefile = loadData;
        savefile.fileIndex = loadData.fileIndex;
        PC = loadData.PC.GetPC();
    }

    public void Save()
    {
        savefile.PC = new SeriPC(PC);
    }

    public int getFileIndex()
    {
        return savefile.fileIndex;
    }


    ///returns "m/f_outfitString_"
    public string getPlayerSpritePrefix()
    {
        if (savefile.isMale)
        {
            return "m_" + savefile.playerOutfit + "_";
        }
        else
        {
            return "f_" + savefile.playerOutfit + "_";
        }
    }


    public int getNonResettingListIndex(string sceneName)
    {
        NonResettingList[] nrlArray = savefile.nonResettingLists.ToArray();

        for (int i = 0; i < nrlArray.Length; i++)
        {
            if (nrlArray[i] != null)
            {
                if (nrlArray[i].sceneName == sceneName)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    private int getCVariableIndex(string variableName)
    {
        CVariable[] cVariableArray = savefile.cVariables.ToArray();

        for (int i = 0; i < cVariableArray.Length; i++)
        {
            if (cVariableArray[i].name == variableName)
            {
                return i;
            }
            else if (cVariableArray[i].name == null)
            {
                Debug.Log("Error: Variable Not Found!");
                return -1;
            }
        }
        Debug.Log("Error: Variable Not Found!");
        return -1;
    }

    public float getCVariable(string variableName)
    {
        int index = getCVariableIndex(variableName);
        if (index > -1)
        {
            return savefile.cVariables[index].value;
        }
        return 0;
    }

    public void setCVariable(string variableName, float newValue)
    {
        int index = getCVariableIndex(variableName);
        if (index == -1)
        {
            savefile.cVariables.Add(new CVariable(variableName, newValue));
            Debug.Log("Added New Variable!");
        }
        else
        {
            savefile.cVariables[index].value = newValue;
        }
    }
}