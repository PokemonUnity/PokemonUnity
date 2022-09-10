//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;

[System.Serializable]
public class SaveData
{
    public static SaveData currentSave;

    private int fileIndex;
    private int buildID;


    //file loading data
    public string levelName;
    public PokemonUnity.Utility.SeriV3 playerPosition;
    public int playerDirection;

    public bool followerOut;
    public PokemonUnity.Utility.SeriV3? followerPosition;
    public int followerdirection;
    public bool followerIsActive;

    //Important player data
    public string playerName;
    public bool isMale;
    public int playerID;
    public string fileCreationDate;
    
    public CosmeticData playerHaircut;
    public PokemonUnity.Utility.SeriColor playerHairColor;
    
    public PokemonUnity.Utility.SeriColor playerEyeColor;
    //

    public string mapName;

    public Pokedex pokedex = new Pokedex();
    public PC PC = new PC();
    public Bag Bag = new Bag();
    public QuestLog questLog = new QuestLog();

    public string[] registeredItems = new string[4];

    public string playerOutfit;

    public int playerScore;
    public int playerMoney;

    public int playerHours;
    public int playerMinutes;
    public int playerSeconds;

    public bool[] gymsEncountered = new bool[12];
    public bool[] gymsBeaten = new bool[12];
    public string[] gymsBeatTime = new string[12];


    //Important gameplay data
    public string respawnSceneName;
    public PokemonUnity.Utility.SeriV3 respawnScenePosition;
    public int respawnSceneDirection;
    public string[] respawnText;
    
    //Important Mystery Gift Data
    public List<string> mysteryGiftCodes = new List<string>();


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
            Debug.Log("Added New Variable!");
        }
        else
        {
            if (newValue == 0)
            {
                cVariables.Remove(cVariables.Find(x => x.name == variableName));
            }
            else
            {
                cVariables[index].value = newValue;
                Debug.Log("New Value of "+cVariables[index].name+" : "+cVariables[index].value);
            }
        }
    }
}