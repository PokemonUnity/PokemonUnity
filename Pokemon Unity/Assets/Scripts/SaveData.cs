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
        public Player player;

        public string levelName;

        public string mapName;

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
    }

    public static SaveData currentSave;
    public SaveFile savefile;

    public PokemonUnity.Character.PC PC { get { return Player.PC; } }
    
    public Bag Bag { get { return Player.Bag; } }

    public Player Player { get; set; }

    public bool IsNotNullFile() { return savefile != null; }

    public SaveData(int fileIndex)
    {
        savefile = new SaveFile();
        savefile.fileIndex = fileIndex;
        //PC = new PC();
        Player = new Player();
        Save();
    }

    public SaveData(SaveFile loadData)
    {
        savefile = loadData;
        savefile.fileIndex = loadData.fileIndex;
        Player = loadData.player;
    }

    public void Save()
    {
        savefile.player = Player;
    }

    public int getFileIndex()
    {
        return savefile.fileIndex;
    }


    ///returns "m/f_outfitString_"
    public string getPlayerSpritePrefix()
    {
        if (Player.IsMale)
        {
            return "m_" + Player.playerOutfit + "_";
        }
        else
        {
            return "f_" + Player.playerOutfit + "_";
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