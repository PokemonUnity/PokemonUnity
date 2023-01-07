using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Pokemon Unity/Settings/Settings Singleton")]
public class GameSettings : ScriptableObject {
    #region Singleton

    static GameSettings singleton;

    public static GUID GUID() => new GUID("246726d7819bc454386e461dba4af80c"); // This needs to be updated when GUID's change

    public static string SingletonPath() => AssetDatabase.GUIDToAssetPath(GUID());

    public static GameSettings LoadSingleton() {
        string path = SingletonPath();
        int resourceFolderIndex = path.IndexOf("Resources");
        if (resourceFolderIndex == -1) throw new NotInResourcesFolderError(path);
        resourceFolderIndex = resourceFolderIndex + "Resources".Length + 1;
        string resourcePath = path.Substring(resourceFolderIndex, path.Length - resourceFolderIndex);
        resourcePath = Path.GetFileNameWithoutExtension(resourcePath);
        GameSettings gameSettings = Resources.Load<GameSettings>(resourcePath);
        return gameSettings;
        //if (gameSettings == null) Debug.LogError("Failed to load ScriptableObject for some unknown reason");
        //return Instantiate(gameSettings);
    }

    class NotInResourcesFolderError : Exception {
        public NotInResourcesFolderError(string path) {
            Debug.LogError("ScriptableObject is not in a Resources folder. Cannot automatically create a Singleton. Path: " + path);
        }
    }

    public static GameSettings Singleton {
        get {
            if (singleton == null) {
                singleton = LoadSingleton();
                Debug.Log(singleton.name + " loaded");
                Initialize(singleton);
            }
            return singleton;
        }
        private set { singleton = value; }
    }

    static void Initialize(GameSettings gameSettings) {
        if (gameSettings.Settings != null && gameSettings.Settings.Count > 0) {
            foreach (var preference in gameSettings.Settings) {
                gameSettings.playerPreferences.Add(preference.Key, preference);
            }
        }
    }

    #endregion

    public List<GameSetting> Settings;

    public Dictionary<EGameSettingKey, GameSetting> playerPreferences = new();

    void Awake() {
        if (Singleton != null) Destroy(this);
    }

    public static T GetSetting<T>(EGameSettingKey key) where T : GameSetting => (T)Singleton.playerPreferences[key];
}

public enum EGameSettingKey {
    NONE,
    TEXT_SPEED,
    MUSIC_VOLUME,
    SFX_VOLUME,
    FRAME_STYLE,
    BATTLE_SCENE,
    BATTLE_STYLE,
    RESOLUTION,
    FULLSCREEN,
}
