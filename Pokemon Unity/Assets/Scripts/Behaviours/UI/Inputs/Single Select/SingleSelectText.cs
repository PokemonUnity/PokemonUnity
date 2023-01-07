using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Text")]
public class SingleSelectText : SingleSelect<string> {
    [SerializeField] UnityEvent<string> onValueChange;
    [SerializeField] List<InputValue<string>> choices;

    public override UnityEvent<string> OnValueChange => onValueChange;
    public override List<InputValue<string>> Choices => choices;

    public override void SetGameSetting(string value) => GameSettings.GetSetting<GameSettingText>(GameSettingsKey).Set(value);
    
    public override void SetGameSetting() => GameSettings.GetSetting<GameSettingText>(GameSettingsKey).Set(currentValue);
}
