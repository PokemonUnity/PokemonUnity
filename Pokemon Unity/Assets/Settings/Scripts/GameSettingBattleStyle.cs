using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Battle Style Setting", menuName = "Pokemon Unity/Settings/Enums/Battle Style")]
public class GameSettingBattleStyle : GameSettingPlayerPref<EBattleStyle> {
    protected override Action<string, EBattleStyle> PrefSetter => (string key, EBattleStyle value) => PlayerPrefs.SetInt(key, (int)value);

    protected override Func<string, EBattleStyle> PrefGetter => (string key) => (EBattleStyle)PlayerPrefs.GetInt(key);
}

public enum EBattleStyle {
    SWITCH,
    SET
}