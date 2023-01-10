using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Setting", menuName = "Pokemon Unity/Settings/Int")]
public class GameSettingInt : GameSettingPlayerPref<int> {
    protected override Action<string, int> PrefSetter => PlayerPrefs.SetInt;

    protected override Func<string, int> PrefGetter => PlayerPrefs.GetInt;
}
