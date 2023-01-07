using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using EasyButtons;

public abstract class SingleSelect<T> : UIInputBehaviour<T>, GameSettingsSetter<T> {
    public abstract List<InputValue<T>> Choices { get; }
    [SerializeField] EDirection NavigationDirection = EDirection.Horizontal;
    [SerializeField] GameObject textInputPrefab;
    [Description("If Game Setting Key is not None, this index will be overrided")]
    public int SelectedIndex = 0;
    int activeIndex = -1;
    GameObject currentChoice;

    new void Start() {
        base.Start();
        if (Choices.Count == 0) Debug.LogError("No selection choices provided");
        if (textInputPrefab == null && transform.childCount == 0) Debug.LogError("No children detected and no text prefab provided to generate children");
        CreateChildren();
        SyncChildren();
        CreateEvents();
        UpdateValueIndex(SelectedIndex);
    }

    public void CreateEvents() {
        for (int i = 0; i < Choices.Count; i++) {
            Button button = transform.GetChild(i).transform.FindFirst<Button>();
            if (button == null) throw new NoButtonFound(transform.GetChild(i));
            button.onClick.AddListener(() => UpdateValueIndex(button.transform.GetSiblingIndex()));
            button.onClick.AddListener(SilentSelect);
        }
    }

    void CreateChildren() {
        if (transform.childCount > 0) return;
        for (int i = 0; i < Choices.Count; i++) {
            GameObject choicePrefab;
            if (textInputPrefab == null) {
                // This probably doesnt work right, oh well for now
                choicePrefab = new GameObject("Single Select Option " + i.ToString());
                choicePrefab.transform.SetParent(transform);
            } else choicePrefab = Instantiate(textInputPrefab);
            choicePrefab.transform.SetParent(transform);
            choicePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    void SyncChildren() {
        for (int i = 0; i < transform.childCount; i++) {
            if (i >= Choices.Count) break;
            var choice = transform.GetChild(i);
            SingleSelectChoice choiceComponent = choice.GetComponent<SingleSelectChoice>();
            if (choiceComponent == null) throw new NoTextSingleSelectChoiceFound(i);
            choiceComponent.choiceText.text = Choices[i].DisplayText;
        }
    }
    
    public void UpdateValueIndex(int index) {
        if (index < 0 || index >= Choices.Count) return;
        UpdateValue(Choices[index]);
    }

    public override void UpdateValue(InputValue<T> value) {
        if (!Application.isPlaying) return;
        activeIndex = Choices.FindIndex((value2) => value2 == value);
        GameObject choice = transform.GetChild(activeIndex).gameObject;
        if (choice.TryGetComponent(out TextColorChanger colorChanger)) {
            colorChanger.ChangeColor();
            if (currentChoice != null && currentChoice.TryGetComponent(out colorChanger)) 
                colorChanger.ChangeToOriginalColor();
        }
        currentChoice = choice;
        Audio.SelectSoundSource.Play();
        currentValue = value.Value;
        base.UpdateValue(value);
    }

    public override void UpdateValue(Vector2 input) {
        if (input.magnitude == 0) return;
        if (NavigationDirection == EDirection.Horizontal && input.y == 0)
            UpdateValueIndex(activeIndex + (int)input.x);
        else if (NavigationDirection == EDirection.Vertical && input.x == 0)
            UpdateValueIndex(activeIndex + (int)input.y);
    }

    public enum EDirection {
        Horizontal,
        Vertical
    }

    #region Errors

    class UnknownOptionError : Exception {
        public UnknownOptionError() {
            Debug.LogError("Single select received an event call for an object it didn't have on start");
        }
    }

    class OptionChangedDuringRuntimeError : Exception {
        public OptionChangedDuringRuntimeError() {
            Debug.LogError("An options text has changed from its original value");
        }
    }

    class NoTextSingleSelectChoiceFound : Exception {
        public NoTextSingleSelectChoiceFound(int index) {
            Debug.LogError("No TextSingleSelectChoice on Text Single Select child " + index.ToString());
        }
    }

    class NoButtonFound : Exception {
        public NoButtonFound(Transform transform) {
            Debug.LogError("No Button component found on " + transform.name + " or any of its children");
        }
    }

    class NotMyChild : Exception {
        public NotMyChild() {
            Debug.LogError("TextSingleSelectChoice provided is not a child of this GameObject");
        }
    }

    #endregion
}
