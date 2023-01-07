using UnityEngine;

[CreateAssetMenu(fileName = "Int Setting", menuName = "Pokemon Unity/Settings/Int")]
public class GameSettingInt : GameSetting<int> {
    public override int Get() => PlayerPrefs.GetInt(KeyString);
    public override void Set(int value) => PlayerPrefs.SetInt(KeyString, value);
}
