using EasyButtons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[AddComponentMenu("Pokemon Unity/Dialogue Trigger")]
public class DialogTriggerBehaviour : MonoBehaviour {
    public List<DialogTrigger> DialogTriggers;

    [Button("Sync Episodes")]
    public void SyncEpisodes(DialogSeries dialogSeries) {
        if (DialogTriggers.Count > 0) {
            Debug.LogWarning("Dialog Triggers is already populated. Delete all entries to sync with a Dialog Series", gameObject);
            return;
        }

        DialogTriggers.Clear();
        foreach (DialogEpisode episode in dialogSeries.Dialogue) {
            DialogTriggers.Add(new DialogTrigger(episode.Name));
        }
    }
}
