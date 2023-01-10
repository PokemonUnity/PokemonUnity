using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Float Setting", menuName = "Pokemon Unity/Settings/Float")]
public class GameSettingFloat : GameSettingPlayerPref<float> {
    protected override Func<string, float> PrefGetter => PlayerPrefs.GetFloat;
    protected override Action<string, float> PrefSetter => PlayerPrefs.SetFloat;
}