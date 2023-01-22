using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Intended for us on a Prefab with a DialogueBoxBehaviour on its parent component</summary>
public interface IUseADialogBox {
    public Image Frame { get; }
    public GameSetting<Sprite> FrameSetting { get; }
    public TMPro.TextMeshProUGUI Text { get; }
    public TMPro.TextMeshProUGUI TextShadow { get; }

    public void Initialize(DialogBox dialogBox);
}

[CreateAssetMenu(fileName = "New Dialog Boxes Factory", menuName = "Pokemon Unity/UI/Dialog Box Factory")]
public class DialogBoxFactory : ScriptableObject {
    //public static DialogBox Singleton;

    /// <summary>Access this list for easy access to available Dialog Boxes to Spawn</summary>
    public List<DialogBox> DialogBoxes;
    static Dictionary<DialogBox, DialogueBoxBehaviour> pool = new();

    public static DialogueBoxBehaviour OpenDialog(DialogBox dialogBox) {
        if (pool.ContainsKey(dialogBox))
            return pool[dialogBox];

        if (dialogBox.Prefab == null) {
            Debug.LogError($"Dialog Box '{dialogBox.name}' does not have a Prefab set on it", dialogBox);
        }
        
        DialogueBoxBehaviour dialogBoxObject = Instantiate<DialogueBoxBehaviour>(dialogBox.Prefab);
        dialogBoxObject.Initialize(dialogBox);
        return dialogBoxObject;
    }
}
