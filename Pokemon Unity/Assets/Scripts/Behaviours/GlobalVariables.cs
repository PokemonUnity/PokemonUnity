﻿//Original Scripts by IIColour (IIColour_Spectrum)

using System.Collections;
using Classes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(MenuHandler))]
public class GlobalVariables : MonoBehaviour
{
    [SerializeField] GameSetting<FullScreenMode> fullscreenSetting;
    [SerializeField] GameSetting<Resolution> resolutionSetting;
    [SerializeField] GameSetting<float> sfxVolumeSetting;
    [SerializeField] GameSetting<float> musicVolumeSetting;
    [SerializeField] GameSetting<Vector3> unitVectorSetting;
    [SerializeField] DialogBoxFactory dialogBoxFactory;
    [SerializeField] GameObject dialogBoxParent;

    public static GameSetting<float> SFXVolumeSetting;
    public static GameSetting<float> MusicVolumeSetting;
    public static GameSetting<Vector3> UnitVectorSetting;

    #region Old - Property Variables
    public static GlobalVariables Singleton;

    public Vector3 playerPosition;
    public int playerDirection;

    public Vector3 followerPosition;
    public int followerDirection;
    public bool isFollowerOut = false;
    
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

    void OnDestroy() {
        SceneManager.sceneLoaded -= CheckLevelLoaded;
    }
    
    void Awake() {
        fullscreenSetting.OnValueChange.AddListener(UpdateResolution);
        resolutionSetting.OnValueChange.AddListener(UpdateResolution);
        SFXVolumeSetting = sfxVolumeSetting;
        MusicVolumeSetting = musicVolumeSetting;
        UnitVectorSetting = unitVectorSetting;

        if (dialogBoxFactory == null) Debug.LogError("No DialogBoxFactory provided", gameObject);
        else {
            DialogBoxFactory.InitializeQuickBoxes(dialogBoxFactory.DialogBoxes);
            if (dialogBoxParent == null) Debug.LogError("No DialogBox Parent GameObject provided", gameObject);
            DialogBoxFactory.DialogBoxesParent = dialogBoxParent;
        } 

        //SceneManager.sceneLoaded += CheckLevelLoaded;

        if (SaveData.currentSave == null) {
            Debug.Log("save file created");
            SaveData.currentSave = new SaveData(-1);
        }

        if (Singleton == null) {
            Singleton = this;
        } else if (Singleton != this) {
            Destroy(gameObject);
            return;
        }

        #region old code
        //debugText = transform.Find("DEBUG").GetComponent<Text>();
        //debugTextShadow = debugText.transform.Find("DEBUGShadow").GetComponent<Text>();

        //if (!PlayerPrefs.HasKey("textSpeed") || !PlayerPrefs.HasKey("musicVolume") ||
        //    !PlayerPrefs.HasKey("sfxVolume") ||
        //    !PlayerPrefs.HasKey("frameStyle") || !PlayerPrefs.HasKey("battleScene") ||
        //    !PlayerPrefs.HasKey("battleStyle") ||
        //    !PlayerPrefs.HasKey("screenSize") || !PlayerPrefs.HasKey("fullscreen"))
        //{
        //    //if a playerpref isn't set

        //    PlayerPrefs.SetInt("textSpeed", 2);
        //    float mVol = (7f / 20f) * (7f / 20f);
        //    float sVol = (14f / 20f) * (14f / 20f);
        //    PlayerPrefs.SetFloat("musicVolume", mVol);
        //    PlayerPrefs.SetFloat("sfxVolume", sVol);
        //    PlayerPrefs.SetInt("frameStyle", 1);
        //    PlayerPrefs.SetInt("battleScene", 1);
        //    PlayerPrefs.SetInt("battleStyle", 0);
        //    PlayerPrefs.SetInt("screenSize", 1);
        //    PlayerPrefs.SetInt("fullscreen", 0);
        //    PlayerPrefs.Save();
        //}
        //UpdateResolution();

        //RenderTexture.active = GUIDisplay;
        //GL.Clear(false, true, new Color(0.0f, 0.0f, 0.0f, 0.0f));

        #endregion
        SaveData.SetDebugFileData();
    }

    void CheckLevelLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "startup")
        {
            if (Singleton == this)
            {
                Player = GameObject.Find("Player");
                FollowerSettings = Player.GetComponentInChildren<FollowerMovement>();

                Debug.Log("Test Scene Loading");
                
                if (Singleton.fadeIn)
                {
                    StartCoroutine(ScreenFade.Singleton.Fade(true, ScreenFade.SlowedSpeed));

                    //if fading in to the scene.
                    Player.transform.position = Singleton.playerPosition;
                    PlayerMovement.Singleton.Direction = Singleton.playerDirection;
                    
                    if (playerExiting)
                    {
                        playerExiting = false;

                        FollowerSettings.direction = Singleton.playerDirection;
                        FollowerSettings.transform.localPosition = -Direction.Vectorize(Singleton.playerDirection);
                    }
                    else if (!respawning)
                    {
                        FollowerSettings.direction = Singleton.followerDirection;
                        FollowerSettings.transform.localPosition = -Direction.Vectorize(Singleton.followerDirection);
                    }

                    if (!respawning)
                    {
                        PlayerMovement.Singleton.PauseMovement(0.8f);
                    }
                    else
                    {
                        PlayerMovement.Singleton.Follower.Hide();
                        PlayerMovement.Singleton.PauseMovement(0.4f);
                    }
                    if (playerForwardOnLoad)
                    {
                        PlayerMovement.Singleton.Follower.Hide();
                        PlayerMovement.Singleton.forceMoveForward();
                        playerForwardOnLoad = false;
                    }
                }
                else
                {
                    ScreenFade.Singleton.SetToFadedIn();
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

    public void UpdateResolution(FullScreenMode fullScreenMode) => UpdateResolution(resolutionSetting.Get(), fullScreenMode);

    public void UpdateResolution(Resolution resolution) => UpdateResolution(resolution, fullscreenSetting.Get());

    public void UpdateResolution(Resolution resolution, FullScreenMode fullScreenMode)
    {
        Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRate);
        //if (PlayerPrefs.GetInt("fullscreen") == 0)
        //{
        //    Screen.SetResolution(684 * PlayerPrefs.GetInt("screenSize"), 384 * PlayerPrefs.GetInt("screenSize"), false);
        //}
        //else if (PlayerPrefs.GetInt("fullscreen") == 1)
        //{
        //    Screen.SetResolution(684 * PlayerPrefs.GetInt("screenSize"), 384 * PlayerPrefs.GetInt("screenSize"), true);
        //}
        //else {
        //    int resWidth = Screen.currentResolution.width + 16; //add a buffer
        //    int resHeight = Screen.currentResolution.height + 9;
        //    int maxSize = 1;
        //    if (1710 < resWidth && 960 < resHeight) {
        //        maxSize = 5;
        //    } else if (1368 < resWidth && 768 < resHeight) {
        //        maxSize = 4;
        //    } else if (1026 < resWidth && 576 < resHeight) {
        //        maxSize = 3;
        //    } else if (684 < resWidth && 384 < resHeight) {
        //        maxSize = 2;
        //    } else {
        //        maxSize = 1;
        //    }
        //    Screen.SetResolution(684 * maxSize, 384 * maxSize, true);
        //}
    }


    public AudioClip getSurfBGM()
    {
        return surfBGM;
    }
}
