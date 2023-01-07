using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Carousel<T> : UIInputBehaviour<T> {
    public abstract List<InputValue<T>> Choices { get; }
    [SerializeField] TextMeshProUGUI TargetText;
    public int SelectedIndex = 0;
    int activeIndex = -1;

    new void Start() {
        base.Start();
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        UpdateValueIndex(SelectedIndex);
    }

    public void UpdateValueUp() => UpdateValueIndex(activeIndex + 1);
    public void UpdateValueDown() => UpdateValueIndex(activeIndex - 1);

    public void UpdateValueIndex(int index) {
        if (index == activeIndex) return;
        if (index < 0 || index >= Choices.Count) return;
        activeIndex = index;
        TargetText.text = Choices[index].DisplayText;
        currentValue = Choices[index].Value;
        OnValueChange.Invoke(currentValue);
    }

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        Audio.SelectSoundSource.Play();
        UpdateValueIndex(activeIndex + (int)navigationDirection.x);
    }
}
