using UnityEngine;

[CreateAssetMenu(fileName = "String Setting", menuName = "Pokemon Unity/Settings/String")]
public class GameSettingText : GameSetting<string> {
    public override string Get() => PlayerPrefs.GetString(KeyString);
    public override void Set(string value) => PlayerPrefs.SetString(KeyString, value);
}