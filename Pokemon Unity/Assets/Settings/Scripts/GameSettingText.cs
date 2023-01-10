using System;
using UnityEngine;

[CreateAssetMenu(fileName = "String Setting", menuName = "Pokemon Unity/Settings/String")]
public class GameSettingText : GameSettingPlayerPref<string> {
    protected override Action<string, string> PrefSetter => PlayerPrefs.SetString;

    protected override Func<string, string> PrefGetter => PlayerPrefs.GetString;
}