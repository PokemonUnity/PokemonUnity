using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Float")]
public class SingleSelectFloat : SingleSelect<float> {
    [SerializeField] UnityEvent<float> onValueChange;
    [SerializeField] List<InputValue<float>> choices;

    public override UnityEvent<float> OnValueChange => onValueChange;
    public override List<InputValue<float>> Choices => choices;

    public override void SetPlayerPref(float value) {
        //PlayerPreferences.SetFloatPreference(PlayerPreferenceKey, value);
    }

    public override void SetPlayerPref() {
        //PlayerPreferences.SetFloatPreference(PlayerPreferenceKey, currentValue);
    }
}