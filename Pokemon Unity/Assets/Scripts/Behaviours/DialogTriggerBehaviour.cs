using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/Dialogue Trigger")]
public class DialogTriggerBehaviour : MonoBehaviour {
    [Description("Click 'Sync Dialog Series' when the Dialog Triggers list is empty to fill it with all episodes in the provided series")]
    [SerializeField] DialogSeries dialogSeries;
    public List<DialogTrigger> DialogTriggers = new();

    public Dictionary<string, DialogTrigger> DialogEpisodeTriggers = new();

    void Awake() {
        foreach (DialogTrigger trigger in DialogTriggers) {
            DialogEpisodeTriggers.Add(trigger.Name, trigger);
        }
    }
}
