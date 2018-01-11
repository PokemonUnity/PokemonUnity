//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables global;

    public Vector3 playerPosition;
    public int playerDirection;
    public bool playerForwardOnLoad;
    public bool fadeIn;
    public Texture fadeTex;

    public int followerIndex = 0;

    private GameObject Player;
    private FollowerMovement FollowerSettings;

    private GUIText debugText;
    private GUIText debugTextShadow;

    public AudioClip surfBGM;
    public int surfBgmLoopStart;

    public RenderTexture GUIDisplay;


    //unimportant data (reset upon load)
    public string itemUsedLast;


    //Important gameplay data
    public bool respawning = false;

    private void OnDestroy()
    {
        //SceneManager.sceneLoaded -= CheckSceneLoaded;
    }
    void Awake()
    {
        //SceneManager.sceneLoaded += CheckSceneLoaded;
        if (SaveData.currentSave == null)
        {
            Debug.Log("save file created");
            SaveData.currentSave = new SaveData(-1);
        }
        if (global == null)
        {
            global = this;

            debugText = this.transform.Find("DEBUG").GetComponent<GUIText>();
            debugTextShadow = debugText.transform.Find("DEBUGShadow").GetComponent<GUIText>();

            Object.DontDestroyOnLoad(this.gameObject);


            if (!PlayerPrefs.HasKey("textSpeed") || !PlayerPrefs.HasKey("musicVolume") ||
                !PlayerPrefs.HasKey("sfxVolume") ||
                !PlayerPrefs.HasKey("frameStyle") || !PlayerPrefs.HasKey("battleScene") ||
                !PlayerPrefs.HasKey("battleStyle") ||
                !PlayerPrefs.HasKey("screenSize") || !PlayerPrefs.HasKey("fullscreen"))
            {
                //if a playerpref isn't set

                PlayerPrefs.SetInt("textSpeed", 2);
                float mVol = (7f / 20f) * (7f / 20f);
                float sVol = (14f / 20f) * (14f / 20f);
                PlayerPrefs.SetFloat("musicVolume", mVol);
                PlayerPrefs.SetFloat("sfxVolume", sVol);
                PlayerPrefs.SetInt("frameStyle", 1);
                PlayerPrefs.SetInt("battleScene", 1);
                PlayerPrefs.SetInt("battleStyle", 0);
                PlayerPrefs.SetInt("screenSize", 1);
                PlayerPrefs.SetInt("fullscreen", 0);
                PlayerPrefs.Save();
            }
            updateResolution();

            RenderTexture.active = GUIDisplay;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            SetDEBUGFileData();
        }
        else if (global != this)
        {
            Destroy(gameObject);
        }
    }

    public void SetDEBUGFileData()
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        SaveData.currentSave.playerName = "Gold";
        SaveData.currentSave.playerID = 29482;
        SaveData.currentSave.isMale = true;

        SaveData.currentSave.playerOutfit = "hgss";

        //PC test
        SaveData.currentSave.PC.addPokemon(new Pokemon(006, null, Pokemon.Gender.CALCULATE, 3, true, "Poké Ball", "",
            "Gold",
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabase.getPokemon(6).GenerateMoveset(42), new int[4]));
        SaveData.currentSave.PC.addPokemon(new Pokemon(197, Pokemon.Gender.CALCULATE, 34, "Great Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(393, Pokemon.Gender.CALCULATE, 6, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(197, Pokemon.Gender.CALCULATE, 28, "Great Ball", "", SaveData.currentSave.playerName, -1));
        SaveData.currentSave.PC.addPokemon(new Pokemon(68, Pokemon.Gender.CALCULATE, 37, "Ultra Ball", "", SaveData.currentSave.playerName, -1));
        SaveData.currentSave.PC.addPokemon(new Pokemon(448, Pokemon.Gender.CALCULATE, 56, "Great Ball", "", SaveData.currentSave.playerName, 0));

        SaveData.currentSave.PC.addPokemon(new Pokemon(006, Pokemon.Gender.CALCULATE, 37, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(607, Pokemon.Gender.CALCULATE, 48, "Poké Ball", "", "Bob", 0));
        SaveData.currentSave.PC.boxes[1][1].addExp(7100);
        SaveData.currentSave.PC.addPokemon(new Pokemon(157, Pokemon.Gender.CALCULATE, 51, "Poké Ball", "", SaveData.currentSave.playerName, 0));
        SaveData.currentSave.PC.addPokemon(new Pokemon(300, Pokemon.Gender.CALCULATE, 51, "Poké Ball", "", SaveData.currentSave.playerName, 0));

        SaveData.currentSave.PC.addPokemon(new Pokemon(393, "Surf Bloke", Pokemon.Gender.MALE, 15, false, "Ultra Ball",
            "", "Gold",
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Drill Peck", "Surf", "Growl", "Dragon Rage"}, new int[] {0, 0, 0, 3}));


        SaveData.currentSave.PC.boxes[0][1].setNickname("Greg");
        SaveData.currentSave.PC.swapPokemon(0, 5, 1, 5);
        SaveData.currentSave.PC.swapPokemon(0, 3, 1, 11);
        SaveData.currentSave.PC.swapPokemon(1, 1, 1, 12);
        SaveData.currentSave.PC.swapPokemon(1, 2, 1, 21);
        SaveData.currentSave.PC.swapPokemon(0, 5, 1, 3);

        SaveData.currentSave.PC.swapPokemon(0, 2, 1, 4);

        SaveData.currentSave.PC.boxes[0][1].setStatus(Pokemon.Status.POISONED);
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

        //PC.boxes[0][0].setStatus(Pokemon.Status.FROZEN);
        SaveData.currentSave.PC.boxes[0][2].setStatus(Pokemon.Status.PARALYZED);
        SaveData.currentSave.PC.boxes[0][3].setStatus(Pokemon.Status.BURNED);
        SaveData.currentSave.PC.boxes[0][4].setStatus(Pokemon.Status.ASLEEP);


        SaveData.currentSave.PC.addPokemon(new Pokemon(012, null, Pokemon.Gender.CALCULATE, 35, false, "Great Ball", "",
            "Gold",
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Ominous Wind", "Sunny Day", "Gust", "Sleep Powder"}, new int[] {0, 0, 0, 0}));

        //SaveData.currentSave.PC.swapPokemon(0,1,3,1);
        SaveData.currentSave.PC.swapPokemon(0, 2, 3, 2);
        SaveData.currentSave.PC.swapPokemon(0, 3, 3, 3);
        SaveData.currentSave.PC.swapPokemon(0, 4, 3, 4);
        SaveData.currentSave.PC.swapPokemon(0, 5, 3, 5);


        SaveData.currentSave.PC.packParty();

        //Bag test
        SaveData.currentSave.Bag.addItem("Poké Ball", 9);
        SaveData.currentSave.Bag.addItem("Miracle Seed", 1);
        SaveData.currentSave.Bag.addItem("Poké Ball", 3);
        SaveData.currentSave.Bag.addItem("Charcoal", 1);
        SaveData.currentSave.Bag.addItem("Potion", 4);
        SaveData.currentSave.Bag.addItem("Poké Doll", 13);
        SaveData.currentSave.Bag.addItem("Escape Rope", 4);
        SaveData.currentSave.Bag.addItem("Fire Stone", 2);
        SaveData.currentSave.Bag.removeItem("Poké Doll", 10);
        SaveData.currentSave.Bag.addItem("Stardust", 1);
        SaveData.currentSave.Bag.addItem("Water Stone", 1);
        SaveData.currentSave.Bag.addItem("Moon Stone", 1);
        SaveData.currentSave.Bag.addItem("Super Potion", 2);
        SaveData.currentSave.Bag.addItem("Great Ball", 4);
        SaveData.currentSave.Bag.addItem("Psyshock", 1);
        SaveData.currentSave.Bag.addItem("Bulk Up", 1);
        SaveData.currentSave.Bag.addItem("Elixir", 2);
        SaveData.currentSave.Bag.addItem("Ether", 1);
        SaveData.currentSave.Bag.addItem("Antidote", 1);
        SaveData.currentSave.Bag.addItem("Full Heal", 1);
        SaveData.currentSave.Bag.addItem("Rare Candy", 100);
        SaveData.currentSave.Bag.addItem("Paralyze Heal", 1);
        SaveData.currentSave.Bag.addItem("Awakening", 1);
        SaveData.currentSave.Bag.addItem("Burn Heal", 1);
        SaveData.currentSave.Bag.addItem("Ice Heal", 1);
        SaveData.currentSave.Bag.addItem("Max Potion", 1);
        SaveData.currentSave.Bag.addItem("Hyper Potion", 1);


        //debug code to test custom box names/textures
        //	PC.boxName[1] = "Grassy Box";
        //	PC.boxTexture[2] = 12;
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test trainer card/save
        SaveData.currentSave.fileCreationDate = "Feb. 14th, 2015";
        SaveData.currentSave.playerMoney = 2481;
        SaveData.currentSave.playerScore = 481;

        SaveData.currentSave.playerHours = 0;
        SaveData.currentSave.playerMinutes = 7;
        SaveData.currentSave.playerSeconds = 12;

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test badge box
        SaveData.currentSave.gymsEncountered = new bool[]
        {
            true, true, false, true, true, true,
            false, false, false, false, false, false
        };
        SaveData.currentSave.gymsBeaten = new bool[]
        {
            true, true, false, false, false, true,
            false, false, false, false, false, false
        };
        SaveData.currentSave.gymsBeatTime = new string[]
        {
            "Apr. 27th, 2015", "Apr. 30th, 2015", null, null, null, "May. 1st, 2015",
            null, null, null, null, null, null
        };
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Replace with 
    /// void CheckLevelLoaded(Scene scene, LoadSceneMode mode)
    /// </remarks>
    void OnLevelWasLoaded()
    {
        if (SceneManager.GetActiveScene().name != "startup")
        {
            if (global == this)
            {
                Player = GameObject.Find("Player");
                FollowerSettings = Player.GetComponentInChildren<FollowerMovement>();
                if (global.fadeIn)
                {
                    StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));

                    //if fading in to the scene.
                    Player.transform.position = global.playerPosition;
                    PlayerMovement.player.direction = global.playerDirection;
                    if (!respawning)
                    {
                        PlayerMovement.player.pauseInput(0.6f);
                    }
                    else
                    {
                        PlayerMovement.player.pauseInput(0.4f);
                    }
                    if (playerForwardOnLoad)
                    {
                        PlayerMovement.player.forceMoveForward();
                        playerForwardOnLoad = false;
                    }
                }
                else
                {
                    ScreenFade.main.SetToFadedIn();
                }
                FollowerSettings.changeFollower(followerIndex);
            }
        }
    }

    /// Loads the new scene, placing the player in the correct position.
    public void Respawn()
    {
        respawning = true;

        fadeIn = true;
        playerForwardOnLoad = false;
        playerPosition = SaveData.currentSave.respawnScenePosition;
        playerDirection = SaveData.currentSave.respawnSceneDirection;

        if (string.IsNullOrEmpty(SaveData.currentSave.respawnSceneName))
        {
            respawning = false;
            SceneManager.LoadScene("overworldS");
        }
        else
        {
            SceneManager.LoadScene(SaveData.currentSave.respawnSceneName);
        }
    }


    public void debug(string message)
    {
        debugText.text = message;
        debugTextShadow.text = message;
    }

    public void resetFollower()
    {
        if (FollowerSettings == null)
        {
            FollowerSettings = GameObject.Find("Player").GetComponentInChildren<FollowerMovement>();
        }
        for (int i = 0; i < 6; i++)
        {
            if (SaveData.currentSave.PC.boxes[0][i] != null)
            {
                if (SaveData.currentSave.PC.boxes[0][i].getStatus() != Pokemon.Status.FAINTED)
                {
                    FollowerSettings.changeFollower(i);
                    i = 6;
                }
            }
        }
    }

    public void updateResolution()
    {
        if (PlayerPrefs.GetInt("fullscreen") == 0)
        {
            Screen.SetResolution(342 * PlayerPrefs.GetInt("screenSize"), 192 * PlayerPrefs.GetInt("screenSize"), false);
        }
        else if (PlayerPrefs.GetInt("fullscreen") == 1)
        {
            Screen.SetResolution(342 * PlayerPrefs.GetInt("screenSize"), 192 * PlayerPrefs.GetInt("screenSize"), true);
        }
        else
        {
            int resWidth = Screen.currentResolution.width + 10; //add a buffer
            int resHeight = Screen.currentResolution.height + 6;
            int maxSize = 1;
            if (1710 < resWidth && 960 < resHeight)
            {
                maxSize = 5;
            }
            else if (1368 < resWidth && 768 < resHeight)
            {
                maxSize = 4;
            }
            else if (1026 < resWidth && 576 < resHeight)
            {
                maxSize = 3;
            }
            else if (684 < resWidth && 384 < resHeight)
            {
                maxSize = 2;
            }
            else
            {
                maxSize = 1;
            }
            Screen.SetResolution(342 * maxSize, 192 * maxSize, true);
        }
    }


    public AudioClip getSurfBGM()
    {
        return surfBGM;
    }
}