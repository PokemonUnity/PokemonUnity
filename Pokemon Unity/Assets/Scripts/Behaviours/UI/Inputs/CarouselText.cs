using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using System;

// TODO: make abstract
public class CarouselText : UIInputBehaviour<string>
{
    [SerializeField] UnityEvent<string> onValueChange;
    [SerializeField] TextMeshProUGUI TargetText;
    public int SelectedIndex = 0;
    int activeIndex = -1;
    public List<InputValue<string>> CarouselItems;

    public override UnityEvent<string> OnValueChange => onValueChange;

    new void Start()
    {
        base.Start();
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        UpdateValueIndex(SelectedIndex);
    }

    public void UpdateValueUp() => UpdateValueIndex(activeIndex+1);
    public void UpdateValueDown() => UpdateValueIndex(activeIndex-1);

    public void UpdateValueIndex(int index) {
        if (index == activeIndex) return;
        if (index < 0 || index >= CarouselItems.Count) return;
        activeIndex = index;
        TargetText.text = CarouselItems[index].DisplayText;
        OnValueChange.Invoke(CarouselItems[index].Value);
    }

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        Audio.SelectSoundSource.Play();
        UpdateValueIndex(activeIndex + (int)navigationDirection.x);
    }

    public override void SetPlayerPref(string value) {
        throw new NotImplementedException();
    }

    public override void SetPlayerPref() {
        throw new NotImplementedException();
    }
}
