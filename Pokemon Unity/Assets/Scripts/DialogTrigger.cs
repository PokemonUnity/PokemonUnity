using System;
using UnityEngine.Events;

[Serializable]
public class DialogTrigger {
    [Description("Events are Invoked from a DialogBox, and return Dialog that corresponds from a DialogEpisode with a matching name")]
    public string Name;
    public UnityEvent<DialogEpisode> OnEpisodeStart = new();
    public UnityEvent<Dialog> OnDialogueFinished = new();
    public UnityEvent<DialogEpisode> OnEpisodeEnd = new();
}