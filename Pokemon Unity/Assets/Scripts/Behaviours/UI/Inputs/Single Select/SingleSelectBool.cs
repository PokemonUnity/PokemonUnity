using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SingleSelectBool : SingleSelect<bool> {
    [SerializeField] UnityEvent<bool> onValueChange;
    [SerializeField] List<InputValue<bool>> choices;

    public override UnityEvent<bool> OnValueChange => onValueChange;
    public override List<InputValue<bool>> Choices => choices;

    public override void SetPlayerPref(bool value) {
        PlayerPrefs.SetInt(PlayerPrefKeys.Keys[PlayerPreferenceKey], value ? 1 : 0);
    }

    public override void SetPlayerPref() {
        PlayerPrefs.SetInt(PlayerPrefKeys.Keys[PlayerPreferenceKey], currentValue ? 1 : 0);
    }
}
