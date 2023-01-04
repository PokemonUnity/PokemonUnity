using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using EasyButtons;

public class TextSingleSelect : UIInputBehaviour
{
    [SerializeField] EDirection NavigationDirection = EDirection.Horizontal;
    [SerializeField] GameObject textInputPrefab;
    [Description(@"Spawns a prefab for each choice provided. The root of this prefab must contain the TextSingleSelectChoice component.
If possible, sets the Navigation property on each instantiated prefab.

No layout is applied. Apply a layout component to this GameObject to organize their position.

No children will be generated if the GameObject already has Children. Use the button below to generate them on demand")]
    public int SelectedIndex = 0;
    int activeIndex = -1;
    [SerializeField] List<string> choices;
    TextSingleSelectChoice currentChoice;

    new void Start()
    {
        base.Start();
        if (choices.Count == 0) Debug.LogError("No selection choices provided");
        if (textInputPrefab == null) Debug.LogError("No text prefab provided");
        CreateChildren();
        CreateEvents();
        UpdateValue(SelectedIndex);
    }

    public void CreateEvents() {
        for (int i = 0; i < choices.Count; i++) {
            Selectable selectable = transform.GetChild(i).transform.FindFirst<Selectable>();
            EventTrigger eventTrigger = selectable.GetComponent<EventTrigger>();
            if (eventTrigger is null) selectable.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry entry = eventTrigger.triggers.Find((entries) => entries.eventID == EventTriggerType.Select);
            if (entry is null) {
                entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                eventTrigger.triggers.Add(entry);
            }
            entry.callback.AddListener(UpdateValue);
        }
    }

    [Button]
    void CreateChildren() {
        if (transform.childCount > 0) return;
        for (int i = 0; i < choices.Count; i++) {
            var choicePrefab = Instantiate(textInputPrefab);
            choicePrefab.transform.SetParent(transform);
            choicePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            TextSingleSelectChoice choiceComponent = choicePrefab.GetComponent<TextSingleSelectChoice>();
            if (choiceComponent is null) throw new NoTextSingleSelectChoiceFound(i);
            choiceComponent.choiceText.text = choices[i];
        }
    }

    public void UpdateValue(TextSingleSelectChoice choice) {
        if (choice.transform.parent != transform) throw new NotMyChild();
        if (activeIndex == choice.transform.GetSiblingIndex()) return;
        activeIndex = choice.transform.GetSiblingIndex();
        choice.colorChanger.ChangeColor();
        if (currentChoice is not null) currentChoice.colorChanger.ChangeToPreviousColor();
        choice.GetComponent<Selectable>().Select();
        currentChoice = choice;
        OnValueChange.Invoke(choice.choiceText.text);
    }

    public void UpdateValue(BaseEventData eventData) {
        if (!eventData.selectedObject.TryGetComponent(out TextSingleSelectChoice choice))
            throw new UnknownOptionError();

        UpdateValue(choice);
    }

    public void UpdateValue(int index) {
        if (index < 0 || index >= choices.Count) return;
        UpdateValue(transform.GetChild(index).GetComponent<TextSingleSelectChoice>());
    }

    public override void UpdateValue(Vector2 input) {
        if (NavigationDirection == EDirection.Horizontal)
            UpdateValue(activeIndex + (int)input.x);
        else if (NavigationDirection == EDirection.Vertical)
            UpdateValue(activeIndex + (int)input.y);
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
