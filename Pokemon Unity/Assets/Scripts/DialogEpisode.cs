using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogEpisode {
    /// <summary>The name of the event that the dialogue is related to. For example, "Confronted", "Defeated", "Approached"</summary>
    [SerializeField] string name;
    public List<Dialog> Dialogs;

    public string Name { get => name; }
}
