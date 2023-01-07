using UnityEngine;

[CreateAssetMenu(fileName = "Float Setting", menuName = "Pokemon Unity/Settings/Float")]
public class GameSettingFloat : GameSetting<float> {
    public override float Get() => PlayerPrefs.HasKey(KeyString) ? PlayerPrefs.GetInt(KeyString) : DefaultValue;
    public override void Set(float value) {
        PlayerPrefs.SetFloat(KeyString, value);
        OnValueChange.Invoke(value);
    }
}