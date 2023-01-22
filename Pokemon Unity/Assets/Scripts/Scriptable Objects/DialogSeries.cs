using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trainer", menuName = "Pokemon Unity/Dialogue")]
public class DialogSeries : ScriptableObject {
    public List<DialogEpisode> Dialogue;

    Dictionary<string, DialogEpisode> dialogueEpisodes;

    void Awake() {
        foreach (DialogEpisode dialogueEvent in Dialogue)
            dialogueEpisodes.Add(dialogueEvent.Name, dialogueEvent);
    }
}
