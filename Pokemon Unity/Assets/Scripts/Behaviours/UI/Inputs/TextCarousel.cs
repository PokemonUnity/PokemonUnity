using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TextCarousel : UIInputBehaviour
{
    [SerializeField] TextMeshProUGUI TargetText;
    public int SelectedIndex = 0;
    int activeIndex = -1;
    public List<string> CarouselItems;

    new void Start()
    {
        base.Start();
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        UpdateValue(SelectedIndex);
    }

    public void UpdateValueUp() => UpdateValue(activeIndex+1);
    public void UpdateValueDown() => UpdateValue(activeIndex-1);

    public void UpdateValue(int index) {
        if (index == activeIndex) return;
        if (index < 0 || index >= CarouselItems.Count) return;
        activeIndex = index;
        TargetText.text = CarouselItems[index];
        OnValueChange.Invoke(CarouselItems[index]);
    }

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        Audio.SelectSoundSource.Play();
        UpdateValue(activeIndex + (int)navigationDirection.x);
    }
}
