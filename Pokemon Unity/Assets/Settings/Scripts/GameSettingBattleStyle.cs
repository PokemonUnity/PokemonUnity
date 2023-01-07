using UnityEngine;

[CreateAssetMenu(fileName = "Battle Style Setting", menuName = "Pokemon Unity/Settings/Enums/Battle Style")]
public class GameSettingBattleStyle : GameSetting<EBattleStyle> {
    public override EBattleStyle Get() => (EBattleStyle)PlayerPrefs.GetInt(KeyString);
    public override void Set(EBattleStyle value) => PlayerPrefs.SetInt(KeyString, (int)value);
}

public enum EBattleStyle {
    SWITCH,
    SET
}