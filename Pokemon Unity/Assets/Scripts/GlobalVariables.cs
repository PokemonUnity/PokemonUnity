//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using Classes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalVariables : MonoBehaviour
{
	#region Property Variables
	public static GlobalVariables global;

    public Vector3 playerPosition;
    public int playerDirection;

    public Vector3 followerPosition;
    public int followerDirection;
    public bool followerOut;
    
    public bool playerExiting;
    public bool playerForwardOnLoad;
    public bool fadeIn;
    public Sprite fadeTex;

    public CosmeticData playerHaitcut;

    public int followerIndex = 0;

    private GameObject Player;
    private FollowerMovement FollowerSettings;

    private Text debugText;
    private Text debugTextShadow;

    public AudioClip surfBGM;
    public int surfBgmLoopStart;

    public RenderTexture GUIDisplay;


    //unimportant data (reset upon load)
    public string itemUsedLast;


    //Important gameplay data
    public bool respawning = false;
	#endregion

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= CheckLevelLoaded;
    }
    
    void Awake()
    {
        SceneManager.sceneLoaded += CheckLevelLoaded;
        if (SaveData.currentSave == null)
        {
            Debug.Log("save file created");
            SaveData.currentSave = new SaveData(-1);
        }
        if (global == null)
        {
            global = this;

            //debugText = transform.Find("DEBUG").GetComponent<Text>();
            //debugTextShadow = debugText.transform.Find("DEBUGShadow").GetComponent<Text>();

            DontDestroyOnLoad(gameObject);


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
        SaveData.currentSave.isMale = true;

        if (SaveData.currentSave.isMale)
        {
            SaveData.currentSave.playerName = "Frozen";
        }
        else
        {
            SaveData.currentSave.playerName = "Spirit";
        }
        
        
        SaveData.currentSave.playerID = 29482;
        
        global.followerOut = SaveData.currentSave.followerOut;

        SaveData.currentSave.playerHaircut = CosmeticDatabase.getHaircut("default", SaveData.currentSave.isMale);
        
        //SaveData.currentSave.playerHairColor = new SerializableColor(0.553f, 0.427f, 0.271f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.396f, 0.325f, 0.325f, 1);
        
        
        //SaveData.currentSave.playerHairColor = new SerializableColor(0.984f, 0.882f, 0.451f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.227f, 0.545f, 0.588f, 1);
        
        //SaveData.currentSave.playerHairColor = new SerializableColor(0.984f, 0.882f, 0.451f, 1);
        //SaveData.currentSave.playerEyeColor = new SerializableColor(0.227f, 0.545f, 0.588f, 1);
        
        SaveData.currentSave.playerHairColor = new PokemonUnity.Utility.SeriColor(0.984f, 0.882f, 0.451f, 0);
        SaveData.currentSave.playerEyeColor = new PokemonUnity.Utility.SeriColor(0.227f, 0.545f, 0.588f, 0);


        SaveData.currentSave.playerOutfit = "bw";

        //PC test
        
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
        SaveData.currentSave.Bag.addItem("Master Ball", 4);
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
            false, false, false, false, false, false,
            false, false, false, false, false, false
        };
        SaveData.currentSave.gymsBeaten = new bool[]
        {
            false, false, false, false, false, false,
            false, false, false, false, false, false
        };
        SaveData.currentSave.gymsBeatTime = new string[]
        {
            null, null, null, null, null, null,
            null, null, null, null, null, null
        };
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    void CheckLevelLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "startup")
        {
            if (global == this)
            {
                Player = GameObject.Find("Player");
                FollowerSettings = Player.GetComponentInChildren<FollowerMovement>();

                Debug.Log("Test Scene Loading");
                
                if (global.fadeIn)
                {
                    StartCoroutine(ScreenFade.main.Fade(true, ScreenFade.slowedSpeed));

                    //if fading in to the scene.
                    Player.transform.position = global.playerPosition;
                    PlayerMovement.player.direction = global.playerDirection;
                    
                    if (playerExiting)
                    {
                        playerExiting = false;

                        FollowerSettings.direction = global.playerDirection;
                        FollowerSettings.transform.localPosition = -Direction.Vectorize(global.playerDirection);
                    }
                    else if (!respawning)
                    {
                        FollowerSettings.direction = global.followerDirection;
                        FollowerSettings.transform.localPosition = -Direction.Vectorize(global.followerDirection);
                    }

                    if (!respawning)
                    {
                        PlayerMovement.player.pauseInput(0.8f);
                    }
                    else
                    {
                        PlayerMovement.player.followerScript.Hide();
                        PlayerMovement.player.pauseInput(0.4f);
                    }
                    if (playerForwardOnLoad)
                    {
                        PlayerMovement.player.followerScript.Hide();
                        PlayerMovement.player.forceMoveForward();
                        playerForwardOnLoad = false;
                    }
                }
                else
                {
                    ScreenFade.main.SetToFadedIn();
                }
                if (SaveData.currentSave.PC.boxes[0][0] != null)
                {
                    FollowerSettings.changeFollower(followerIndex);
                }
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
            SceneManager.LoadScene("flowery_town_indoors");
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
                if (PokemonUnity.Game.GameData.Trainer.party[i].Status != PokemonUnity.Status.FAINT)
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
            Screen.SetResolution(684 * PlayerPrefs.GetInt("screenSize"), 384 * PlayerPrefs.GetInt("screenSize"), false);
        }
        else if (PlayerPrefs.GetInt("fullscreen") == 1)
        {
            Screen.SetResolution(684 * PlayerPrefs.GetInt("screenSize"), 384 * PlayerPrefs.GetInt("screenSize"), true);
        }
        else
        {
            int resWidth = Screen.currentResolution.width + 16; //add a buffer
            int resHeight = Screen.currentResolution.height + 9;
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
            Screen.SetResolution(684 * maxSize, 384 * maxSize, true);
        }
    }


    public AudioClip getSurfBGM()
    {
        return surfBGM;
    }
}
