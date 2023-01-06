using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSelectFloat : SingleSelect<float> {
    [SerializeField] UnityEvent<float> onValueChange;
    [SerializeField] List<InputValue<float>> choices;

    public override UnityEvent<float> OnValueChange => onValueChange;
    public override List<InputValue<float>> Choices => choices;

    public override void SetPlayerPref(float value) {
        PlayerPrefs.SetFloat(PlayerPrefKeys.Keys[PlayerPreferenceKey], value);
    }

    public override void SetPlayerPref() {
        PlayerPrefs.SetFloat(PlayerPrefKeys.Keys[PlayerPreferenceKey], currentValue);
    }
}