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

    public AudioClip surfBGM;
    public int surfBgmLoopStart;

    public RenderTexture GUIDisplay;
    public TextAsset versioningJSON;


    //unimportant data (reset upon load)
    public string itemUsedLast;

    //Important gameplay data
    public bool respawning = false;
    public void CreateFileData(string name, bool isMale)
    {
    }

    /// Loads the new scene, placing the player in the correct position.
    public void Respawn()
    {
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