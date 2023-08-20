using MarkupAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class Dialog {
    public string Phrase;
    //[EnableIfGroup("Replies", "hasReplies")] 
    public List<string> PossibleReplies;
    [HideInInspector] public string Reply;

    public bool HasReplies { get => PossibleReplies.Count > 0; }
}
