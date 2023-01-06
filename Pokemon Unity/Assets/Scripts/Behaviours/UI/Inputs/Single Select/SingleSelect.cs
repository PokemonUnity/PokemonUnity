using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using EasyButtons;

public abstract class SingleSelect<T> : UIInputBehaviour<T>, PlayerPrefSetter<T> {
    public abstract List<InputValue<T>> Choices { get; }
    [SerializeField] EDirection NavigationDirection = EDirection.Horizontal;
    [SerializeField] GameObject textInputPrefab;
    [Description(@"Spawns a prefab for each choice provided. The root of this prefab must contain the TextSingleSelectChoice component.
If possible, sets the Navigation property on each instantiated prefab.

No layout is applied. Apply a layout component to this GameObject to organize their position.

No children will be generated if the GameObject already has Children. Use the button below to generate them on demand")]
    public int SelectedIndex = 0;
    int activeIndex = -1;
    GameObject currentChoice;
    protected T currentValue;

    new void Start() {
        base.Start();
        if (Choices.Count == 0) Debug.LogError("No selection choices provided");
        if (textInputPrefab == null && transform.childCount == 0) Debug.LogError("No children detected and no text prefab provided to generate children");
        if (transform.childCount == 0) {
            CreateChildren();
            CreateEvents();
        }
        UpdateValueIndex(SelectedIndex);
    }

    public void CreateEvents() {
        for (int i = 0; i < Choices.Count; i++) {
            Button button = transform.GetChild(i).transform.FindFirst<Button>();
            button.onClick.AddListener(SilentSelect);
            button.onClick.AddListener(() => UpdateValueIndex(i));
        }
    }

    [Button]
    void CreateChildren() {
        if (transform.childCount > 0) return;
        for (int i = 0; i < Choices.Count; i++) {
            var choicePrefab = Instantiate(textInputPrefab);
            choicePrefab.transform.SetParent(transform);
            choicePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            TextSingleSelectChoice choiceComponent = choicePrefab.GetComponent<TextSingleSelectChoice>();
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
        OnValueChange.Invoke(value.Value);
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

    class NotMyChild : Exception {
        public NotMyChild() {
            Debug.LogError("TextSingleSelectChoice provided is not a child of this GameObject");
        }
    }

    #endregion
}
