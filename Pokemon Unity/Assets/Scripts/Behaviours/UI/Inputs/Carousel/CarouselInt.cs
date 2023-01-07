using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/UI/Carousel/Carousel Int")]
public class CarouselInt : Carousel<int> {
    [SerializeField] UnityEvent<int> onValueChange;
    [SerializeField] List<InputValue<int>> choices;

    public override UnityEvent<int> OnValueChange => onValueChange;

    public override List<InputValue<int>> Choices => choices;

    public override void SetPlayerPref(int value) {
        //PlayerPreferences.SetIntPreference(PlayerPreferenceKey, value);
    }

    public override void SetPlayerPref() {
        //PlayerPreferences.SetIntPreference(PlayerPreferenceKey, currentValue);
    }
}
