using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Float")]
public class SingleSelectFloat : SingleSelect<float> {
    [SerializeField] UnityEvent<float> onValueChange;
    [SerializeField] List<InputValue<float>> choices;

    public override UnityEvent<float> OnValueChange => onValueChange;
    public override List<InputValue<float>> Choices => choices;

    public override void SetGameSetting(float value) {
        Debug.Log(value.ToString());
        GameSettings.GetSetting<GameSettingFloat>(GameSettingsKey).Set(value); 
    }

    public override void SetGameSetting() => GameSettings.GetSetting<GameSettingFloat>(GameSettingsKey).Set(currentValue);
}