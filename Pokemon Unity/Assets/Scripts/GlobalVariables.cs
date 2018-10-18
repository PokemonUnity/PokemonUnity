//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GlobalVariables : MonoBehaviour
{
    public bool isEditor = false;
    public bool allowDebug = true;
        //change this setting when releasing a public build, as it allows for developer features to be accessed.
    DiscordRpc.EventHandlers handlers;
    public DiscordRpc.RichPresence presence;
    public string applicationId; //change this application id when you're testing discord rpc
    public string optionalSteamId;
    public int callbackCalls;
    public UnityEngine.Events.UnityEvent onConnect;
    public UnityEngine.Events.UnityEvent onDisconnect;

    public enum Language
    {
        /// <summary>
        /// US English
        /// </summary>
        English = 9
    }
    public static GlobalVariables global;
    //public static UnityEngine.SceneManagement.Scene SceneLoaded = SceneManager.GetActiveScene();
    public enum Language
    {
        /// <summary>
        /// US English
        /// </summary>
        English = 9
    }

    public Vector3 playerPosition;
    public int playerDirection;
    public bool playerForwardOnLoad;
    public bool playerOrtho;
    public bool fadeIn;
    public Texture fadeTex;

    public int followerIndex = 0;
    private GameObject Player;
    private FollowerMovement FollowerSettings;

    private GUIText debugText;
    private GUIText debugTextShadow;

    public Camera MainCamera;
    public MeshRenderer Display;

    public AudioClip surfBGM;
    public int surfBgmLoopStart;

    public RenderTexture GUIDisplay;
    public TextAsset versioningJSON;


    //unimportant data (reset upon load)
    public string itemUsedLast;

    //Important gameplay data
    public bool respawning = false;

<<<<<<< HEAD
    ///Works as a replacement for Debug.Log(), allowing the log to be sent to the debugText in-game.
    public void debug(string message)
    {
        Debug.Log(message);
        debugText.text = "Debug Mode\n"+message;
        debugTextShadow.text = debugText.text;
    }
    //public void getLang(string m)
    //{

    //}

    void OnDestroy()
=======
    void CheckSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode) {
        Debug.Log(scene.name + " : " + mode.ToString());
    }

    private void OnDestroy()
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
    {
        SceneManager.sceneLoaded -= CheckSceneLoaded;
    }
    void Update()
    {
        DiscordRpc.RunCallbacks();
    }
    void Awake()
    {
<<<<<<< HEAD
        Debug.Log("Discord: init");
        callbackCalls = 0;
        handlers = new DiscordRpc.EventHandlers();
        handlers.readyCallback = ReadyCallback;
        handlers.disconnectedCallback += DisconnectedCallback;
        handlers.errorCallback += ErrorCallback;
        DiscordRpc.Initialize(applicationId, ref handlers, true, optionalSteamId);
        presence.startTimestamp = System.Convert.ToInt32((int)(System.DateTime.UtcNow -
            new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds);

        SceneManager.sceneLoaded += CheckLevelLoaded;
        if (SaveData.currentSave == null)
        {
            Debug.Log("Save file created");
            SaveData.currentSave = new SaveData(-1);
=======
        SceneManager.sceneLoaded += CheckSceneLoaded;
        if (SaveDataOld.currentSave == null)
        {
            Debug.Log("save file created");
            SaveDataOld.currentSave = new SaveDataOld(-1);
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
        }
        if (global == null)
        {
            global = this;

            debugText = this.transform.Find("DEBUG").GetComponent<GUIText>();
            debugTextShadow = debugText.transform.Find("DEBUGShadow").GetComponent<GUIText>();
            MainCamera = this.transform.Find("MainCamera").GetComponent<Camera>();
            Display = MainCamera.transform.Find("Display").GetComponent<MeshRenderer>();
            //debugText.text = "build " + buildNum;
            //debugTextShadow.text = debugText.text;
            Object.DontDestroyOnLoad(this.gameObject);


            if (!PlayerPrefs.HasKey("textSpeed") || !PlayerPrefs.HasKey("musicVolume") ||
                !PlayerPrefs.HasKey("sfxVolume") ||
                !PlayerPrefs.HasKey("frameStyle") || !PlayerPrefs.HasKey("battleScene") ||
                !PlayerPrefs.HasKey("customSprites") ||
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
                PlayerPrefs.SetInt("customSprites", 0);
                PlayerPrefs.SetInt("screenSize", 1);
                PlayerPrefs.SetInt("fullscreen", 0);
                PlayerPrefs.Save();
            }
            updateResolution();

            RenderTexture.active = GUIDisplay;
            GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

            CreateFileData("Ethan",true);
            //CreateFileData("Lyra",false);
        }
        else if (global != this)
        {
            Destroy(gameObject);
        }
    }
    void OnApplicationQuit()
    {
        debug("shutdown");
        DiscordRpc.Shutdown();
    }
    public IEnumerator GetDebugText()
    {
        yield return debugText.text;
    }
    public bool EnableDebugMode()
    {
<<<<<<< HEAD
        return EnableDebugMode(SaveData.currentSave);
    }
    public bool EnableDebugMode(SaveData data)
    {
        data.debugMode = true;
        debugText.gameObject.SetActive(true);
=======
        SaveDataOld.currentSave.debugMode = true;
        debugText.text = "build " + buildNum + "\nDebugging Mode Enabled";
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
        debugTextShadow.text = debugText.text;
        return true;
    }
    public void CreateFileData(string name, bool isMale)
    {
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //EnableDebugMode();
<<<<<<< HEAD
        SaveData.currentSave.playerName = name;
        SaveData.currentSave.playerID = 29482; //not implemented
        SaveData.currentSave.isMale = isMale;
        SaveData.currentSave.playerMoney = 2481; 
        SaveData.currentSave.playerOutfit = "hgss";
        SaveData.currentSave.playerLanguage = Language.English;

        SaveData.currentSave.playerShirt = "Ethan's Shirt";
        SaveData.currentSave.playerMisc = null;
        SaveData.currentSave.playerHat = "Ethan's Hat";
        //customizables not implemented

        if(isMale){
            SaveData.currentSave.setCVariable("male",1); //custom events can check if the player is male or female, 1 meaning male, 0 meaning female
=======
        SaveDataOld.currentSave.playerName = name;
        SaveDataOld.currentSave.playerID = 29482; //not implemented
        SaveDataOld.currentSave.isMale = isMale;
        SaveDataOld.currentSave.playerMoney = 2481; 
        SaveDataOld.currentSave.playerLanguage = Language.English;

        SaveDataOld.currentSave.playerOutfit = "hgss";

        SaveDataOld.currentSave.playerShirt = "Ethan's Shirt";
        SaveDataOld.currentSave.playerMisc = null;
        SaveDataOld.currentSave.playerHat = "Ethan's Hat";
        //customizables not implemented

        if(isMale == true){
            SaveDataOld.currentSave.setCVariable("male",1); //custom events can check if the player is male or female, 1 meaning male, 0 meaning female
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
        } else {
            SaveDataOld.currentSave.setCVariable("male",0);
        }

        //PC test
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(006, null, PokemonOld.Gender.CALCULATE, 3, true, "Poké Ball", "",
            name,
            Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32), Random.Range(0, 32),
            Random.Range(0, 32),
            0, 0, 0, 0, 0, 0, "ADAMANT", 0, PokemonDatabaseOld.getPokemon(6).GenerateMoveset(42), new int[4]));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(197, PokemonOld.Gender.CALCULATE, 34, "Great Ball", "", name, 0));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(393, PokemonOld.Gender.CALCULATE, 6, "Poké Ball", "", name, 0));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(197, PokemonOld.Gender.CALCULATE, 28, "Great Ball", "", name, -1));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(68, PokemonOld.Gender.CALCULATE, 37, "Ultra Ball", "", name, -1));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(448, PokemonOld.Gender.CALCULATE, 56, "Great Ball", "", name, 0));

        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(006, PokemonOld.Gender.CALCULATE, 37, "Poké Ball", "", name, 0));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(607, PokemonOld.Gender.CALCULATE, 48, "Poké Ball", "", "Bob", 0));
        SaveDataOld.currentSave.PC.boxes[1][1].addExp(7100);
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(157, PokemonOld.Gender.CALCULATE, 51, "Poké Ball", "", name, 0));
        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(300, PokemonOld.Gender.CALCULATE, 51, "Poké Ball", "", name, 0));

        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(393, "Surf Bloke", PokemonOld.Gender.MALE, 15, false, "Ultra Ball", //starters not implemented
            "", name,
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Drill Peck", "Surf", "Growl", "Dragon Rage"}, new int[] {0, 0, 0, 3}));


        SaveDataOld.currentSave.PC.boxes[0][1].setNickname("Greg");
        SaveDataOld.currentSave.PC.swapPokemon(0, 5, 1, 5);
        SaveDataOld.currentSave.PC.swapPokemon(0, 3, 1, 11);
        SaveDataOld.currentSave.PC.swapPokemon(1, 1, 1, 12);
        SaveDataOld.currentSave.PC.swapPokemon(1, 2, 1, 21);
        SaveDataOld.currentSave.PC.swapPokemon(0, 5, 1, 3);

        SaveDataOld.currentSave.PC.swapPokemon(0, 2, 1, 4);

        SaveDataOld.currentSave.PC.boxes[0][1].setStatus(PokemonOld.Status.POISONED);
        SaveDataOld.currentSave.PC.boxes[0][1].addExp(420);

        SaveDataOld.currentSave.PC.packParty();

        SaveDataOld.currentSave.PC.swapPokemon(0, 0, 0, 2);

        SaveDataOld.currentSave.PC.boxes[0][0].swapHeldItem("Ultra Ball");

        SaveDataOld.currentSave.PC.boxes[0][1].removeHP(56);
        SaveDataOld.currentSave.PC.boxes[0][4].removeHP(64);

        SaveDataOld.currentSave.PC.boxes[0][4].removePP(0, 5);
        SaveDataOld.currentSave.PC.boxes[0][4].removePP(1, 5);
        SaveDataOld.currentSave.PC.boxes[0][3].removePP(0, 6);
        SaveDataOld.currentSave.PC.boxes[0][0].removePP(2, 11);

        //PC.boxes[0][0].setStatus(Pokemon.Status.FROZEN);
        SaveDataOld.currentSave.PC.boxes[0][2].setStatus(PokemonOld.Status.PARALYZED);
        SaveDataOld.currentSave.PC.boxes[0][3].setStatus(PokemonOld.Status.BURNED);
        SaveDataOld.currentSave.PC.boxes[0][4].setStatus(PokemonOld.Status.ASLEEP);


        SaveDataOld.currentSave.PC.addPokemon(new PokemonOld(012, null, PokemonOld.Gender.CALCULATE, 35, false, "Great Ball", "",
            name,
            31, 31, 31, 31, 31, 31, 0, 252, 0, 0, 0, 252, "ADAMANT", 0,
            new string[] {"Razor Leaf", "Sunny Day", "Gust", "Sleep Powder"}, new int[] {0, 0, 0, 0}));

        //SaveData.currentSave.PC.swapPokemon(0,1,3,1);
        SaveDataOld.currentSave.PC.swapPokemon(0, 2, 3, 2);
        SaveDataOld.currentSave.PC.swapPokemon(0, 3, 3, 3);
        SaveDataOld.currentSave.PC.swapPokemon(0, 4, 3, 4);
        SaveDataOld.currentSave.PC.swapPokemon(0, 5, 3, 5);

<<<<<<< HEAD
        SaveData.currentSave.PC.packParty();
=======

        SaveDataOld.currentSave.PC.packParty();
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5

        //Bag test
        SaveDataOld.currentSave.Bag.addItem("Poké Ball", 9);
        SaveDataOld.currentSave.Bag.addItem("Miracle Seed", 1);
        SaveDataOld.currentSave.Bag.addItem("Poké Ball", 3);
        SaveDataOld.currentSave.Bag.addItem("Charcoal", 1);
        SaveDataOld.currentSave.Bag.addItem("Potion", 4);
        SaveDataOld.currentSave.Bag.addItem("Poké Doll", 13);
        SaveDataOld.currentSave.Bag.addItem("Escape Rope", 4);
        SaveDataOld.currentSave.Bag.addItem("Fire Stone", 2);
        SaveDataOld.currentSave.Bag.removeItem("Poké Doll", 10);
        SaveDataOld.currentSave.Bag.addItem("Stardust", 1);
        SaveDataOld.currentSave.Bag.addItem("Water Stone", 1);
        SaveDataOld.currentSave.Bag.addItem("Moon Stone", 1);
        SaveDataOld.currentSave.Bag.addItem("Super Potion", 2);
        SaveDataOld.currentSave.Bag.addItem("Great Ball", 4);
        SaveDataOld.currentSave.Bag.addItem("Psyshock", 1);
        SaveDataOld.currentSave.Bag.addItem("Bulk Up", 1);
        SaveDataOld.currentSave.Bag.addItem("Elixir", 2);
        SaveDataOld.currentSave.Bag.addItem("Ether", 1);
        SaveDataOld.currentSave.Bag.addItem("Antidote", 1);
        SaveDataOld.currentSave.Bag.addItem("Full Heal", 1);
        SaveDataOld.currentSave.Bag.addItem("Rare Candy", 100);
        SaveDataOld.currentSave.Bag.addItem("Paralyze Heal", 1);
        SaveDataOld.currentSave.Bag.addItem("Awakening", 1);
        SaveDataOld.currentSave.Bag.addItem("Burn Heal", 1);
        SaveDataOld.currentSave.Bag.addItem("Ice Heal", 1);
        SaveDataOld.currentSave.Bag.addItem("Max Potion", 1);
        SaveDataOld.currentSave.Bag.addItem("Hyper Potion", 1);


        /*
        //debug code to test custom box names/textures
        PC.boxName[1] = "Grassy Box";
        PC.boxTexture[2] = 12;
        */
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        /* None of this is needed...
        //set file creation date
        string day = "";
        string month = "";
        if(System.DateTime.Now.Day == 1){day = "1st, ";}
        else if(System.DateTime.Now.Day == 2){day = "2nd, ";}
        else if(System.DateTime.Now.Day == 3){day = "3rd, ";}
        else if(System.DateTime.Now.Day == 21){day = "21st, ";}
        else if(System.DateTime.Now.Day == 22){day = "22nd, ";}
        else if(System.DateTime.Now.Day == 23){day = "23rd, ";}
        else if(System.DateTime.Now.Day == 31){day = "31st, ";}
        else{day = System.DateTime.Now.Day.ToString("D") + "th, ";}

        if(System.DateTime.Now.Month == 1){month = "Jan. ";}
        else if(System.DateTime.Now.Month == 2){month = "Feb. ";}
        else if(System.DateTime.Now.Month == 3){month = "Mar. ";}
        else if(System.DateTime.Now.Month == 4){month = "Apr. ";}
        else if(System.DateTime.Now.Month == 5){month = "May ";}
        else if(System.DateTime.Now.Month == 6){month = "June ";}
        else if(System.DateTime.Now.Month == 7){month = "July ";}
        else if(System.DateTime.Now.Month == 8){month = "Aug. ";}
        else if(System.DateTime.Now.Month == 9){month = "Sep. ";}
        else if(System.DateTime.Now.Month == 10){month = "Oct. ";}
        else if(System.DateTime.Now.Month == 11){month = "Nov. ";}
        else if(System.DateTime.Now.Month == 12){month = "Dec. ";} //probably the worst way to do this but I have no idea why ToString("MMM") doesn't work

        string date = month + day + System.DateTime.Now.Year;

<<<<<<< HEAD
        SaveData.currentSave.fileCreationDate = date;
        SaveData.currentSave.startTime = System.DateTime.UtcNow;
=======
        //SaveData.currentSave.fileCreationDate = "Aug. 2nd, 2017";
        SaveData.currentSave.fileCreationDate = date;*/
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test trainer card/save
<<<<<<< HEAD
        //SaveData.currentSave.fileCreationDate = new System.DateTime(2015, 2, 14); //"Feb. 14th, 2015";
        SaveData.currentSave.playerScore = 481;
        SaveData.currentSave.pokeDex = 0;
        SaveData.currentSave.playerHours = 0;
        SaveData.currentSave.playerMinutes = 7;
        SaveData.currentSave.playerSeconds = 12;
=======
        SaveDataOld.currentSave.fileCreationDate = new System.DateTime(System.DateTime.Now.Year, 2, 14); //"Feb. 14th, 2015";
        SaveDataOld.currentSave.playerMoney = 2481;
        SaveDataOld.currentSave.playerScore = SaveDataOld.currentSave.pokedexCaught + "/" + SaveDataOld.currentSave.pokedexSeen;// PokemonDatabase.LoadPokedex().Length;//481;
        //SaveData.currentSave.pokeDex = 0;
        
        SaveDataOld.currentSave.playerHours = 0;
        SaveDataOld.currentSave.playerMinutes = 7;
        SaveDataOld.currentSave.playerSeconds = 12;
        SaveDataOld.currentSave.playerTime = new System.TimeSpan(0,7,12);
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        //debug code to test badge box
        SaveDataOld.currentSave.gymsEncountered = new bool[]
        {
            true, true, false, true, true, true,
            false, false, false, false, false, false
        };
        SaveDataOld.currentSave.gymsBeaten = new bool[]
        {
            true, true, false, false, false, true,
            false, false, false, false, false, false
        };
<<<<<<< HEAD
        SaveData.currentSave.gymsBeatTime = new System.DateTime?[]
          {
            new System.DateTime(System.DateTime.Now.Year, 4, 27) /*"Apr. 27th, 2015"*/, new System.DateTime(System.DateTime.Now.Year, 4, 30) /*"Apr. 30th, 2015"*/, null, null, null, new System.DateTime(System.DateTime.Now.Year, 5,1) /*"May. 1st, 2015"*/,
              null, null, null, null, null, null
=======
        SaveDataOld.currentSave.gymsBeatTime = new System.DateTime?[]
        {
            new System.DateTime(System.DateTime.Now.Year, 4, 27) /*"Apr. 27th, 2015"*/, new System.DateTime(System.DateTime.Now.Year, 4, 30) /*"Apr. 30th, 2015"*/, null, null, null, new System.DateTime(System.DateTime.Now.Year, 5,1) /*"May. 1st, 2015"*/,
            null, null, null, null, null, null
>>>>>>> a9ab54ddb317d13d4624c9affb897812e9672ce5
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
    void CheckLevelLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name + " : " + mode.ToString());
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
                    PlayerMovement.player.mainCamera.orthographic = global.playerOrtho;
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

    /// Loads the new scene, placing the player in the correct position
    public void Respawn()
    {
        respawning = true;

        fadeIn = true;
        playerForwardOnLoad = false;
        playerPosition = SaveDataOld.currentSave.respawnScenePosition;
        playerDirection = SaveDataOld.currentSave.respawnSceneDirection;

        if (string.IsNullOrEmpty(SaveDataOld.currentSave.respawnSceneName))
        {
            respawning = false;
            SceneManager.LoadScene("overworldS");
        }
        else
        {
            SceneManager.LoadScene(SaveDataOld.currentSave.respawnSceneName);
        }
    }

    public void resetFollower()
    {
        if (FollowerSettings == null)
        {
            FollowerSettings = GameObject.Find("Player").GetComponentInChildren<FollowerMovement>();
        }
        for (int i = 0; i < 6; i++)
        {
            if (SaveDataOld.currentSave.PC.boxes[0][i] != null)
            {
                if (SaveDataOld.currentSave.PC.boxes[0][i].getStatus() != PokemonOld.Status.FAINTED)
                {
                    FollowerSettings.changeFollower(i);
                    if(SaveData.currentSave.PC.boxes[0][i].getName() != PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][i].getID()).getName())
                    {
                        SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][i].getName() +  " (" + PokemonDatabase.getPokemon(SaveData.currentSave.PC.boxes[0][i].getID()).getName() + ", Level " + SaveData.currentSave.PC.boxes[0][i].getLevel() + ")");  
                    }
                    else {
                        SetRPCState("Follower: " + SaveData.currentSave.PC.boxes[0][i].getName() + " (Level " + SaveData.currentSave.PC.boxes[0][i].getLevel() + ")");
                    }
                    UpdatePresence();
                    i = 6;
                }
            }
        }
        debug("Follower: " + PokemonDatabase.getPokemon(FollowerSettings.pokemonID).getName());
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
    
    //Discord RPC Support
    public void SetRPCState(string state)
    {
        presence.state = state;
        debug("Discord: state set to \"" + state + "\"");
    }
    public void SetRPCDetails(string details)
    {
        presence.details = details;
        debug("Discord: details set to \"" + details + "\"");
    }
    public void SetRPCLargeImageKey(string key, string text)
    {
        presence.largeImageKey = key;
        presence.largeImageText = text;
        debug("Discord: large image set to \"" + key + "\", \"" + text + "\"");
    }
    public void SetRPCSmallImageKey(string key, string text)
    {
        presence.smallImageKey = key;
        presence.smallImageText = text;
        debug("Discord: small image set to \"" + key + "\", \"" + text + "\"");
    }
    public void UpdatePresence()
    {
        DiscordRpc.UpdatePresence(ref presence);
        debug("Discord: updating presence");
    }
    public void ReadyCallback()
    {
        ++callbackCalls;
        debug("Discord: ready");
        onConnect.Invoke();
    }
    public void DisconnectedCallback(int errorCode, string message)
    {
        ++callbackCalls;
        debug(string.Format("Discord: disconnect {0}: {1}", errorCode, message));
        onDisconnect.Invoke();
    }

    public void ErrorCallback(int errorCode, string message)
    {
        ++callbackCalls;
        debug(string.Format("Discord: error {0}: {1}", errorCode, message));
    }
}