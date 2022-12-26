using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEditor;
using EasyButtons;

public class UITextSingleSelect : MonoBehaviour
{
    [SerializeField] GameObject textInputPrefab;
    [Description(@"Spawns a prefab for each choice provided. Looks for TextMeshProUGUI components in any GameObject in the prefabs hierarchy.
If possible, sets the Navigation property on each instantiated prefab.

No layout is applied. Apply a layout component to this GameObject to organize their position.

No children will be generated if the GameObject already has Children. Use the button below to generate them on demand")]
    [SerializeField] Navigation.Mode Navigation;
    [Tooltip("Change Text Color On Select")]
    [SerializeField] bool TextColorChange = false;
    [Description("Color change will apply if the first found Selectable GameObject has a TextMeshProUGUI component")]
    [SerializeField] Color color;
    [SerializeField] List<string> choices;

    void Start()
    {
        if (choices.Count == 0) Debug.LogError("No selection choices provided");
        if (textInputPrefab == null) Debug.LogError("No text prefab provided");
        CreateChildren();
    }

    /// <summary>Feed this method into an GameObject that has TextMeshProUGUI a and a EventTrigger</summary>
    public void ChangeTextColor(BaseEventData arg0) {
        if (arg0.selectedObject.TryGetComponent(out TextMeshProUGUI text)) {
            text.color = color;
        }
    }

    public void InititalizeColorChangeEvent(Selectable selectable) {
        EventTrigger eventTrigger = selectable.GetComponent<EventTrigger>();
        if (eventTrigger is null)
            selectable.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = eventTrigger.triggers.Find((entries) => entries.eventID == EventTriggerType.Select);
        if (entry is null) {
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            eventTrigger.triggers.Add(entry);
        }
        entry.callback.AddListener(ChangeTextColor);
    }

    [Button]
    void CreateChildren() {
        if (transform.childCount > 0) return;
        for (int i = 0; i < choices.Count; i++) {
            var choicePrefab = Instantiate(textInputPrefab);
            choicePrefab.transform.SetParent(transform);
            choicePrefab.transform.localScale = new Vector3(1f, 1f, 1f);
            Selectable selectable = choicePrefab.transform.FindFirst<Selectable>();
            if (selectable is not null) {
                Navigation nav = selectable.navigation;
                nav.mode = Navigation;
                InititalizeColorChangeEvent(selectable);
            }
            TextMeshProUGUI text = choicePrefab.transform.FindFirst<TextMeshProUGUI>();
            if (text is not null)
                text.text = choices[i];
        }
    }
}
