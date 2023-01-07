using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "Pokemon Unity/Settings/Settings Singleton")]
public class GameSettings : ScriptableObjectSingleton<GameSettings> {

    #region Inspector Editable Stuff

    public List<GameSetting> Settings;

    public Dictionary<EPlayerPrefKeys, GameSetting> playerPreferences;

    #endregion

    new void Awake() {
        base.Awake();
        if (Settings != null && Settings.Count > 0) {
            foreach (var preference in Settings) {
                playerPreferences.Add(preference.Key, preference);
            }
        }
    }

    // Maybe remove this and just use enum.ToString()?
    public static Dictionary<EPlayerPrefKeys, string> KeyStrings = new() {
        { EPlayerPrefKeys.NONE, "" },
        { EPlayerPrefKeys.TEXT_SPEED, "textSpeed" },
        { EPlayerPrefKeys.FULLSCREEN, "fullscreen" },
    };

    public static GameSetting GetPreference(EPlayerPrefKeys key) => Singleton.playerPreferences[key];

    [Serializable]
    public class PlayerPreferenceEntries {
        public EPlayerPrefKeys Key;
        public GameSetting Preference;
    }
}

public enum EPlayerPrefKeys {
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
