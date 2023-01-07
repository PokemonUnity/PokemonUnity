using UnityEngine;

[CreateAssetMenu(fileName = "Bool Setting", menuName = "Pokemon Unity/Settings/Bool")]
public class GameSettingBool : GameSetting<bool> {
    public override bool Get() => PlayerPrefs.GetInt(KeyString) == 1;
    public override void Set(bool value) => PlayerPrefs.SetInt(KeyString, value ? 1 : 0);
}
