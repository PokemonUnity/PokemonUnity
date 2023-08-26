using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Trainer", menuName = "Pokemon Unity/Dialogue")]
public class DialogSeries : ScriptableObject {
    public List<DialogEpisode> Dialogue;

    Dictionary<string, DialogEpisode> dialogueEpisodes = new();

    public Dictionary<string, DialogEpisode> DialogueEpisodes { 
        get {
            initEpisodes();
            return dialogueEpisodes;
        }
    }

    void Awake() {
        initEpisodes();
    }

    void initEpisodes() {
        if (dialogueEpisodes.Count > 0) return;
        foreach (DialogEpisode dialogueEpisode in Dialogue)
            dialogueEpisodes.Add(dialogueEpisode.Name, dialogueEpisode);
    }
}
