using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

[AddComponentMenu("Pokemon Unity/UI/Volume Bar")]
public class VolumeBar : UIInputBehaviour<float> {
    public GameSetting<float> gameSetting;
    [SerializeField] float upperBound = 0f;
    [SerializeField] float lowerBound = 100f;
    [SerializeField] float step = 5f;
    [SerializeField] GameObject slider;
    [SerializeField] GameObject textUnitPrefab;
    List<TextColorChanger> colorChangers = new();
    bool navigateIsPressed = false;
    Action continuousUpdateFunc;
    [SerializeField] float waitTimeOnButtonHold = 1f;
    [SerializeField] float updateIntervalTime = 0.1f;
    float currentWaitTime = 1f;
     
    new void OnValidate() {
        //base.OnValidate();
        if (upperBound < lowerBound) upperBound = lowerBound;
        if (lowerBound > upperBound) lowerBound = upperBound;
    }

    protected override void Start() {
        if (!Application.isPlaying) return;
        if (slider == null) Debug.LogError("No slider provided");
        if (textUnitPrefab == null) Debug.LogError("No TextUnitPrefab provided");
        if (textUnitPrefab == null || textUnitPrefab == null) return;
        SpawnUnits();
        base.Start();
        UpdateValue(GameSetting.Get());
    }

    void Update() {
        UpdateIteration();
    }

    void UpdateIteration() {
        if (navigateIsPressed && continuousUpdateFunc != null) {
            currentWaitTime -= Time.deltaTime;
            if (currentWaitTime <= 0) {
                continuousUpdateFunc();
                currentWaitTime = updateIntervalTime;
            }
        }
    }

    void SpawnUnits() {
        if (slider?.transform.childCount > 0) {
            foreach (Transform child in slider.transform) {
                Destroy(child.gameObject);
            }
        }
        float unitCount = GetUnitCount();
        for (int i = 0; i < unitCount; i++) {
            GameObject prefab = Instantiate(textUnitPrefab, slider.transform);
            TextColorChanger colorChanger = prefab.transform.FindFirst<TextColorChanger>();
            if (colorChanger != null) {
                colorChangers.Add(colorChanger);
            }
            Button button = prefab.transform.FindFirst<Button>();
            if (button != null) {
                button.onClick.AddListener(SelectSilently);
                button.onClick.AddListener(() => UpdateValue(button.transform.GetSiblingIndex()));
            }
        }
    }

    public float ToUnit(float value) => value / step;
    public float FromUnit(float value) => value * step;

    public float GetUnitCount() => (upperBound - lowerBound) / step;

    public override GameSetting<float> GameSetting => gameSetting;

    public void SyncColorChangers() {
        for (int i = 0; i < colorChangers.Count; i++) {
            if (i < ToUnit(currentValue)) 
                colorChangers[i].ChangeColor();
            else
                colorChangers[i].ChangeToOriginalColor();
        }
    }

    protected override void Navigate(CallbackContext context) {
        if (gameObject != EventSystem.current.currentSelectedGameObject) return;
        Vector2 navigationDirection = context.ReadValue<Vector2>();
        if (navigationDirection.y != 0) return;
        navigateIsPressed = context.action.IsPressed();
        if (navigateIsPressed && context.action.triggered) {
            currentWaitTime = waitTimeOnButtonHold;
            UpdateValue(navigationDirection);
        }
    }

    public override void UpdateValue(Vector2 navigationDirection) {
        if (navigationDirection.magnitude == 0) return;
        if (navigationDirection.x > 0f)
            continuousUpdateFunc = UpdateValueUp;
        else if (navigationDirection.x < 0f)
            continuousUpdateFunc = UpdateValueDown;
        else 
            continuousUpdateFunc = null;
        if (continuousUpdateFunc != null)
            continuousUpdateFunc();
    }

    public void UpdateValueUp() => UpdateValue(currentValue + step);
    
    public void UpdateValueDown() => UpdateValue(currentValue - step);

    public void UpdateValue(int siblingIndex) {
        float newValue = FromUnit(siblingIndex+1);
        UpdateValue(newValue);
    }

    public override void UpdateValue(float value) {
        float newValue = Mathf.Clamp(value, lowerBound, upperBound);
        base.UpdateValue(newValue);
        SyncColorChangers();
    }

    public override void OnDeselect(BaseEventData data) {
        continuousUpdateFunc = null;
        currentWaitTime = waitTimeOnButtonHold;
        base.OnDeselect(data);
    }
}
