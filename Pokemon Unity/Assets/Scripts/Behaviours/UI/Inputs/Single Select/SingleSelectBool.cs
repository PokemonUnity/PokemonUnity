using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Bool")]
public class SingleSelectBool : SingleSelect<bool> {
    [SerializeField] UnityEvent<bool> onValueChange;
    [SerializeField] List<InputValue<bool>> choices;

    public override UnityEvent<bool> OnValueChange => onValueChange;
    public override List<InputValue<bool>> Choices => choices;

    public override void SetGameSetting(bool value) => GameSettings.GetSetting<GameSettingBool>(GameSettingsKey).Set(value);

    public override void SetGameSetting() => GameSettings.GetSetting<GameSettingBool>(GameSettingsKey).Set(currentValue);
}
