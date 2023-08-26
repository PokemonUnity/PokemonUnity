using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Fullscreen Mode Setting", menuName = "Pokemon Unity/Settings/Enums/Fullscreen Mode")]
public class GameSettingFullscreenMode : GameSettingPlayerPrefArray<FullScreenMode> {
    //protected override Action<string, FullScreenMode> PrefSetter => (string key, FullScreenMode value) => PlayerPrefs.SetInt(key, (int)value);
    //protected override Func<string, FullScreenMode> PrefGetter => (string key) => (FullScreenMode)PlayerPrefs.GetInt(key);
}
