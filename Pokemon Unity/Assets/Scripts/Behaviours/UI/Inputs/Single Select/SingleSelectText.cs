using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSelectText : SingleSelect<string> {
    [SerializeField] UnityEvent<string> onValueChange;
    [SerializeField] List<InputValue<string>> choices;

    public override UnityEvent<string> OnValueChange => onValueChange;
    public override List<InputValue<string>> Choices => choices;

    public override void SetPlayerPref(string value) {
        PlayerPrefs.SetString(PlayerPrefKeys.Keys[PlayerPreferenceKey], value);
    }

    public override void SetPlayerPref() {
        PlayerPrefs.SetString(PlayerPrefKeys.Keys[PlayerPreferenceKey], currentValue);
    }
}
