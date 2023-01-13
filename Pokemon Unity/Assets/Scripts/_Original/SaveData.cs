//Original Scripts by IIColour (IIColour_Spectrum)

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Data;

[System.Serializable]
public class SaveData
{
    #region Old Vars

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

    #endregion

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

    public static void SetDebugFileData() {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        currentSave.isMale = true;

        if (currentSave.isMale) {
            currentSave.playerName = "Frozen";
        } else {
            currentSave.playerName = "Spirit";
        }


        currentSave.playerID = 29482;

        GlobalVariables.global.followerOut = SaveData.currentSave.followerOut;

        currentSave.playerHaircut = CosmeticDatabase.getHaircut("default", SaveData.currentSave.isMale);

        //SaveData.currentSave.playerHairColor = new SerializableColor(0.553f, 0.427f, 0.271f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.396f, 0.325f, 0.325f, 1);


        //SaveData.currentSave.playerHairColor = new SerializableColor(0.984f, 0.882f, 0.451f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.227f, 0.545f, 0.588f, 1);

        //SaveData.currentSave.playerHairColor = new SerializableColor(0.984f, 0.882f, 0.451f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.227f, 0.545f, 0.588f, 1);

        currentSave.playerHairColor = new PokemonUnity.Utility.SeriColor(0.984f, 0.882f, 0.451f, 0);
        currentSave.playerEyeColor = new PokemonUnity.Utility.SeriColor(0.227f, 0.545f, 0.588f, 0);


        currentSave.playerOutfit = "bw";

        #region PC test

        //SaveData.currentSave.PC.addPokemon(new Pokemon(150, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(483, Pokemon.Gender.CALCULATE, 100, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(484, Pokemon.Gender.CALCULATE, 100, "Poké Ball", "", SaveData.currentSave.playerName, 0));

        //SaveData.currentSave.PC.addPokemon(new Pokemon(394, Pokemon.Gender.CALCULATE, 17, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(487, Pokemon.Gender.CALCULATE, 50, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(18, Pokemon.Gender.CALCULATE, 50, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(197, Pokemon.Gender.CALCULATE, 50, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(18, Pokemon.Gender.CALCULATE, 50, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.boxes[0][0].removeHP(140);
        //SaveData.currentSave.PC.boxes[0][1].removeHP(140);

        //SaveData.currentSave.PC.addPokemon(new Pokemon(487, Pokemon.Gender.CALCULATE, 65, "Poké Ball", "", SaveData.currentSave.playerName, 0));

        //SaveData.currentSave.PC.addPokemon(new Pokemon(405, Pokemon.Gender.CALCULATE, 58, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(398, Pokemon.Gender.CALCULATE, 59, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(454, Pokemon.Gender.CALCULATE, 58, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(467, Pokemon.Gender.CALCULATE, 55, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(306, Pokemon.Gender.CALCULATE, 58, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(306, Pokemon.Gender.CALCULATE, 40, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(151, Pokemon.Gender.CALCULATE, 50, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(1115, Pokemon.Gender.CALCULATE, 5, "Poké Ball", "", SaveData.currentSave.playerName, 0));

        /*
        SaveData.currentSave.PC.addPokemon(new Pokemon(393, Pokemon.Gender.CALCULATE, 6, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(197, Pokemon.Gender.CALCULATE, 28, "Great Ball", "", SaveData.currentSave.playerName, -1));
        SaveData.currentSave.PC.addPokemon(new Pokemon(68, Pokemon.Gender.CALCULATE, 37, "Ultra Ball", "", SaveData.currentSave.playerName, -1));
        SaveData.currentSave.PC.addPokemon(new Pokemon(448, Pokemon.Gender.CALCULATE, 56, "Great Ball", "", SaveData.currentSave.playerName, 0));

        SaveData.currentSave.PC.addPokemon(new Pokemon(006, Pokemon.Gender.CALCULATE, 37, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        //SaveData.currentSave.PC.addPokemon(new Pokemon(607, Pokemon.Gender.CALCULATE, 48, "Poké Ball", "", "Bob", 0));
        //SaveData.currentSave.PC.boxes[1][1].addExp(7100);
        SaveData.currentSave.PC.addPokemon(new Pokemon(157, Pokemon.Gender.CALCULATE, 51, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(300, Pokemon.Gender.CALCULATE, 51, "Poké Ball", "", SaveData.currentSave.playerName, 0));

        SaveData.currentSave.PC.addPokemon(new Pokemon(151, null, Pokemon.Gender.NONE, 100, true, "Ultra Ball",
            "", SaveData.currentSave.playerName,
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Drill Peck", "Surf", "Growl", "Dragon Rage"}, new int[] {0, 0, 0, 3}));

        SaveData.currentSave.PC.boxes[0][1].setNickname("Greg");
        SaveData.currentSave.PC.swapPokemon(0, 5, 1, 5);
        SaveData.currentSave.PC.swapPokemon(0, 3, 1, 11);
        SaveData.currentSave.PC.swapPokemon(1, 1, 1, 12);
        SaveData.currentSave.PC.swapPokemon(1, 2, 1, 21);
        SaveData.currentSave.PC.swapPokemon(0, 5, 1, 3);

        SaveData.currentSave.PC.swapPokemon(0, 2, 1, 4);

        SaveData.currentSave.PC.boxes[0][1].setStatus(Status.POISONED);
        SaveData.currentSave.PC.boxes[0][1].addExp(420);

        SaveData.currentSave.PC.packParty();

        SaveData.currentSave.PC.swapPokemon(0, 0, 0, 2);

        SaveData.currentSave.PC.boxes[0][0].swapHeldItem("Ultra Ball");

        SaveData.currentSave.PC.boxes[0][1].removeHP(56);
        SaveData.currentSave.PC.boxes[0][4].removeHP(64);

        SaveData.currentSave.PC.boxes[0][4].removePP(0, 5);
        SaveData.currentSave.PC.boxes[0][4].removePP(1, 5);
        SaveData.currentSave.PC.boxes[0][3].removePP(0, 6);
        SaveData.currentSave.PC.boxes[0][0].removePP(2, 11);

        //PC.boxes[0][0].setStatus(Status.FROZEN);
        SaveData.currentSave.PC.boxes[0][2].setStatus(Status.PARALYZED);
        SaveData.currentSave.PC.boxes[0][3].setStatus(Status.BURNED);
        SaveData.currentSave.PC.boxes[0][4].setStatus(Status.ASLEEP);


        SaveData.currentSave.PC.addPokemon(new Pokemon(012, null, Pokemon.Gender.CALCULATE, 35, false, "Great Ball", "",
            SaveData.currentSave.playerName,
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Ominous Wind", "Sunny Day", "Gust", "Sleep Powder"}, new int[] {0, 0, 0, 0}));

        //SaveData.currentSave.PC.swapPokemon(0,1,3,1);
        SaveData.currentSave.PC.swapPokemon(0, 2, 3, 2);
        SaveData.currentSave.PC.swapPokemon(0, 3, 3, 3);
        SaveData.currentSave.PC.swapPokemon(0, 4, 3, 4);
        SaveData.currentSave.PC.swapPokemon(0, 5, 3, 5);
        */


        /*
        SaveData.currentSave.PC.addPokemon(new Pokemon(006, null, Pokemon.Gender.CALCULATE, 5, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(4).GenerateMoveset(5), new int[4]));
        */

        /*
        SaveData.currentSave.PC.addPokemon(new Pokemon(005, null, Pokemon.Gender.CALCULATE, 15, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(42), new int[4]));

        SaveData.currentSave.PC.addPokemon(new Pokemon(001, null, Pokemon.Gender.CALCULATE, 15, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(42), new int[4]));
        
        SaveData.currentSave.PC.addPokemon(new Pokemon(002, null, Pokemon.Gender.CALCULATE, 15, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(42), new int[4]));
        
        SaveData.currentSave.PC.addPokemon(new Pokemon(003, null, Pokemon.Gender.CALCULATE, 15, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(42), new int[4]));
        
        */
        /*
        SaveData.currentSave.PC.addPokemon(new Pokemon(006, null, Pokemon.Gender.CALCULATE, 15, false, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(4).GenerateMoveset(5), new int[4]));
        
        
        SaveData.currentSave.PC.addPokemon(new Pokemon(655, null, Pokemon.Gender.CALCULATE, 36, false, "Ultra Ball", "",
            "Calem",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(36), new int[4]));
        */

        #endregion

        currentSave.PC.packParty();



        //Bag test
        currentSave.Bag.addItem("Poké Ball", 9);
        currentSave.Bag.addItem("Miracle Seed", 1);
        currentSave.Bag.addItem("Poké Ball", 3);
        currentSave.Bag.addItem("Charcoal", 1);
        currentSave.Bag.addItem("Potion", 4);
        currentSave.Bag.addItem("Poké Doll", 13);
        currentSave.Bag.addItem("Escape Rope", 4);
        currentSave.Bag.addItem("Fire Stone", 2);
        currentSave.Bag.removeItem("Poké Doll", 10);
        currentSave.Bag.addItem("Stardust", 1);
        currentSave.Bag.addItem("Water Stone", 1);
        currentSave.Bag.addItem("Moon Stone", 1);
        currentSave.Bag.addItem("Super Potion", 2);
        currentSave.Bag.addItem("Great Ball", 4);
        currentSave.Bag.addItem("Master Ball", 4);
        currentSave.Bag.addItem("Psyshock", 1);
        currentSave.Bag.addItem("Bulk Up", 1);
        currentSave.Bag.addItem("Elixir", 2);
        currentSave.Bag.addItem("Ether", 1);
        currentSave.Bag.addItem("Antidote", 1);
        currentSave.Bag.addItem("Full Heal", 1);
        currentSave.Bag.addItem("Rare Candy", 100);
        currentSave.Bag.addItem("Paralyze Heal", 1);
        currentSave.Bag.addItem("Awakening", 1);
        currentSave.Bag.addItem("Burn Heal", 1);
        currentSave.Bag.addItem("Ice Heal", 1);
        currentSave.Bag.addItem("Max Potion", 1);
        currentSave.Bag.addItem("Hyper Potion", 1);


        //debug code to test custom box names/textures
        //	PC.boxName[1] = "Grassy Box";
        //	PC.boxTexture[2] = 12;
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test trainer card/save
        currentSave.fileCreationDate = "Feb. 14th, 2015";
        currentSave.playerMoney = 2481;
        currentSave.playerScore = 481;

        currentSave.playerHours = 0;
        currentSave.playerMinutes = 7;
        currentSave.playerSeconds = 12;

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test badge box
        currentSave.gymsEncountered = new bool[]
        {
            false, false, false, false, false, false,
            false, false, false, false, false, false
        };
        currentSave.gymsBeaten = new bool[]
        {
            false, false, false, false, false, false,
            false, false, false, false, false, false
        };
        currentSave.gymsBeatTime = new string[]
        {
            null, null, null, null, null, null,
            null, null, null, null, null, null
        };
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }

}