//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using PokemonUnity;
using PokemonUnity.Inventory;
using PokemonUnity.Monster;

// Make sure this go execution first
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


    void Awake()
    {
        // https://docs.unity3d.com/Manual/StreamingAssets.html
        // This Init Sqlite Database
        if (System.IO.File.Exists(Application.streamingAssetsPath + "/veekun-pokedex.sqlite"))
        {
            // Reset and Init Sqlite
            Game.ResetAndOpenSql(Application.streamingAssetsPath + "/veekun-pokedex.sqlite");
        }
        else
            throw new System.Exception("The Database was not founded!");

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
            GL.Clear(false, true, new UnityEngine.Color(0.0f, 0.0f, 0.0f, 0.0f));

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
        SaveData.currentSave.Player = new Player("Gold", true);

        SaveData.currentSave.Player.playerOutfit = "hgss";

        //PC test
        SaveData.currentSave.Player.addPokemon(new Pokemon(Pokemons.UMBREON, SaveData.currentSave.Player.Trainer, 5));
        SaveData.currentSave.Player.Party[0].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon(Pokemons.CHARIZARD, SaveData.currentSave.Player.Trainer, 30));
        SaveData.currentSave.Player.Party[1].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon(Pokemons.LUCARIO, SaveData.currentSave.Player.Trainer, 100));
        SaveData.currentSave.Player.Party[2].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)393, SaveData.currentSave.Player.Trainer, 5));
        SaveData.currentSave.Player.Party[3].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)10, SaveData.currentSave.Player.Trainer, 100));
        SaveData.currentSave.Player.Party[4].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)11, SaveData.currentSave.Player.Trainer, 100));
        SaveData.currentSave.Player.Party[5].ballUsed = Items.POKE_BALL;
        
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)197, SaveData.currentSave.Player.Trainer, 12));
        SaveData.currentSave.Player.PC.AllBoxes[0][0].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)393, SaveData.currentSave.Player.Trainer, 52));
        SaveData.currentSave.Player.PC.AllBoxes[0][1].ballUsed = Items.POKE_BALL;
        SaveData.currentSave.Player.addPokemon(new Pokemon((Pokemons)68, SaveData.currentSave.Player.Trainer, 37));
        SaveData.currentSave.Player.PC.AllBoxes[0][2].ballUsed = Items.POKE_BALL;
        
        SaveData.currentSave.Player.Party[3].replaceMove(0, Moves.SURF);
        SaveData.currentSave.Player.Party[3].addMove(Moves.WATERFALL);
        SaveData.currentSave.Player.Party[3].Experience.AddExperience(100);
        SaveData.currentSave.Player.Party[3].HP -= 10;
        SaveData.currentSave.Player.Party[3].Status = Status.BURN;
        SaveData.currentSave.Player.Party[0].moves[0].PP -= 10;
        SaveData.currentSave.Player.Party[0].SwapItem(Items.POTION);

        //Bag test
        SaveData.currentSave.Bag.addItem(Items.POKE_BALL, 9);
        SaveData.currentSave.Bag.addItem(Items.MIRACLE_SEED, 1);
        SaveData.currentSave.Bag.addItem(Items.POKE_BALL, 3);
        SaveData.currentSave.Bag.addItem(Items.CHARCOAL, 1);
        SaveData.currentSave.Bag.addItem(Items.POTION, 4);
        SaveData.currentSave.Bag.addItem(Items.POKE_DOLL, 13);
        SaveData.currentSave.Bag.addItem(Items.ESCAPE_ROPE, 4);
        SaveData.currentSave.Bag.addItem(Items.FIRE_STONE, 2);
        SaveData.currentSave.Bag.removeItem(Items.POKE_DOLL, 10);
        SaveData.currentSave.Bag.addItem(Items.STARDUST, 1);
        SaveData.currentSave.Bag.addItem(Items.WATER_STONE, 1);
        SaveData.currentSave.Bag.addItem(Items.MOON_STONE, 1);
        SaveData.currentSave.Bag.addItem(Items.SUPER_POTION, 2);
        SaveData.currentSave.Bag.addItem(Items.GREAT_BALL, 4);
        //SaveData.currentSave.Bag.addItem(Moves.PSYSHOCK, 1); // TM) TODO: USE ENUM FOR MOVE?
        //SaveData.currentSave.Bag.addItem(Moves.BULK_UP, 1);  // TM) TODO: USE ENUM FOR MOVE?
        SaveData.currentSave.Bag.addItem(Items.ELIXIR, 2);
        SaveData.currentSave.Bag.addItem(Items.ETHER, 1);
        SaveData.currentSave.Bag.addItem(Items.ANTIDOTE, 1);
        SaveData.currentSave.Bag.addItem(Items.FULL_HEAL, 1);
        SaveData.currentSave.Bag.addItem(Items.RARE_CANDY, 100);
        SaveData.currentSave.Bag.addItem(Items.PARALYZE_HEAL, 1);
        SaveData.currentSave.Bag.addItem(Items.AWAKENING, 1);
        SaveData.currentSave.Bag.addItem(Items.BURN_HEAL, 1);
        SaveData.currentSave.Bag.addItem(Items.ICE_HEAL, 1);
        SaveData.currentSave.Bag.addItem(Items.MAX_POTION, 1);
        SaveData.currentSave.Bag.addItem(Items.HYPER_POTION, 1);

        //debug code to test custom box names/textures
        //	PC.boxName[1] = "Grassy Box";
        //	PC.boxTexture[2] = 12;
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test trainer card/save
        SaveData.currentSave.Player.StartDate = new System.DateTime(2015, 2, 14);
        SaveData.currentSave.Player.Money = 2481;
        SaveData.currentSave.Player.playerScore = 481;

        SaveData.currentSave.Player.PlayTime = new System.TimeSpan(0, 7, 12);

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test badge box
        SaveData.currentSave.Player.gymsEncountered = new bool[]
        {
            true, true, false, true, true, true,
            false, false, false, false, false, false
        };
        SaveData.currentSave.Player.gymsBeaten = new bool[]
        {
            true, true, false, false, false, true,
            false, false, false, false, false, false
        };
        SaveData.currentSave.Player.gymsBeatTime = new string[]
        {
            "Apr. 27th, 2015", "Apr. 30th, 2015", null, null, null, "May. 1st, 2015",
            null, null, null, null, null, null
        };
        ////////////////////////////////////////////////////////////////////////////////////////////////////
    }

    void OnLevelWasLoaded()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "startup")
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
        playerPosition = SaveData.currentSave.savefile.respawnScenePosition;
        playerDirection = SaveData.currentSave.savefile.respawnSceneDirection;

        if (string.IsNullOrEmpty(SaveData.currentSave.savefile.respawnSceneName))
        {
            respawning = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("overworldS");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(SaveData.currentSave.savefile.respawnSceneName);
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
            if (SaveData.currentSave.Player.Party[i] != null)
            {
                if (SaveData.currentSave.Player.Party[i].Status != PokemonUnity.Status.FAINT)
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