//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class SaveData
{
    public static SaveData currentSave;

    private int fileIndex;
    private int buildID;


    //file loading data
    public string levelName;
    public SeriV3 playerPosition;
    public int playerDirection;
    public GlobalVariables.Language playerLanguage;


    //Important player data
    public string playerName;
    public bool isMale;
    public int playerID;
    public System.DateTime? fileCreationDate;
    public System.DateTime? lastSave;
    public System.DateTime startTime = new System.DateTime();

    public string mapName;

    public PC PC = new PC();
    public Bag Bag = new Bag();


    public string[] registeredItems = new string[4];

    public string playerOutfit;

    public int playerScore;
    public int playerMoney;
    /// <summary>
    /// Usage:<para>
    /// <code>playerPokedex[1,0] == 0; means pokemonId #1 not seen</code>
    /// </para>
    /// <code>playerPokedex[1,1] == 0; means pokemonId #1 not captured</code>
    /// </summary>
    /// <remarks>Or can be int?[pokedex.count,1]. if null, not seen or captured</remarks>
    public int[,] playerPokedex2 = new int[PokemonDatabase.Pokedex.Count, 2];//
    /// <summary>
    /// Usage:<para>
    /// <code>playerPokedex[1] == false; means pokemonId #1 has been seen, and not captured</code>
    /// </para>
    /// <code>playerPokedex[1] == true; means pokemonId #1 has been captured</code>
    /// </summary>
    /// <remarks>if null, has not been seen or captured</remarks> 
    public bool?[] playerPokedex = new bool?[PokemonDatabase.Pokedex.Count]; 
    public int pokedexCaught = (from caught in SaveData.currentSave.playerPokedex where caught == true select caught).Count();
    public int pokedexSeen = (from seen in SaveData.currentSave.playerPokedex where seen != null select seen).Count(); 

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
            cVariables[index].value = newValue;
        }
    }
}