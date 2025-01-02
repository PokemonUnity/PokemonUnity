using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Bool Setting", menuName = "Pokemon Unity/Settings/Bool")]
public class GameSettingBool : GameSettingPlayerPref<bool> {
    protected override Action<string, bool> PrefSetter => (string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);

    protected override Func<string, bool> PrefGetter => (string key) => PlayerPrefs.GetInt(Key) == 1;
}
