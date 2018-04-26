//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public static SaveData currentSave;

    private int fileIndex;
    private int buildID;
    public GlobalVariables.Language playerLanguage;

    //file loading data
    public string levelName;
    public SeriV3 playerPosition;
    public int playerDirection;
    public bool playerOrtho;

    //Important player data
    public string playerName;
    public bool debugMode = false;
    public bool isMale;
    public int playerID;
    public string fileCreationDate;
    public System.DateTime? lastSave;
    public System.DateTime startTime = new System.DateTime();

    public string mapName;

    public PC PC = new PC();
    public Bag Bag = new Bag();


    public string[] registeredItems = new string[4];

    public string playerOutfit;

    public int playerScore;

    public string playerShirt;
    public string playerMisc;
    public string playerHat;
    //customizables not implemented

    public int playerMoney;
    public int pokeDex; //pokedex not implemented
    public System.TimeSpan playerTime;
    public int playerHours;
    public int playerMinutes;
    public int playerSeconds;

    public bool[] gymsEncountered = new bool[12];
    /// <summary>
    /// if <see cref="gymsBeatTime"/> is null, then value is false.
    /// </summary>
    /// <remarks>This isnt needed...</remarks>
    public bool[] gymsBeaten = new bool[12];
    public System.DateTime?[] gymsBeatTime = new System.DateTime?[12];


    //Important gameplay data
    public string respawnSceneName;
    public SeriV3 respawnScenePosition;
    public int respawnSceneDirection;
    public string respawnText;
    public bool isDemo;

    private List<CVariable> cVariables = new List<CVariable>();
    public List<NonResettingList> nonResettingLists = new List<NonResettingList>();


    public SaveData(int fileIndex)
    {
        this.fileIndex = fileIndex;
    }

    public int getFileIndex()
    {
        return fileIndex;
    }


    ///returns "m/f_outfitString_"
    public string getPlayerSpritePrefix()
    {
        if (isMale)
        {
            return "m_" + playerOutfit + "_";
        }
        else
        {
            return "f_" + playerOutfit + "_";
        }
    }


    public int getNonResettingListIndex(string sceneName)
    {
        NonResettingList[] nrlArray = nonResettingLists.ToArray();

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
        CVariable[] cVariableArray = cVariables.ToArray();

        for (int i = 0; i < cVariableArray.Length; i++)
        {
            if (cVariableArray[i].name == variableName)
            {
                return i;
            }
            else if (cVariableArray[i].name == null)
            {
                GlobalVariables.global.debug("Error: Variable Not Found!");
                return -1;
            }
        }
        GlobalVariables.global.debug("Error: Variable Not Found!");
        return -1;
    }

    public float getCVariable(string variableName)
    {
        int index = getCVariableIndex(variableName);
        if (index > -1)
        {
            return cVariables[index].value;
        }
        return 0;
    }

    public void setCVariable(string variableName, float newValue)
    {
        int index = getCVariableIndex(variableName);
        if (index == -1)
        {
            cVariables.Add(new CVariable(variableName, newValue));
            GlobalVariables.global.debug("Added New Variable!");
        }
        else
        {
            cVariables[index].value = newValue;
        }
    }
}