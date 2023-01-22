using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>Intended for us on a Prefab with a DialogueBoxBehaviour on its parent component</summary>
public interface IUseADialogBox {
    public Image Image { get; }
    public GameSetting<Sprite> FrameSetting { get; }
    public TMPro.TextMeshProUGUI Text { get; }
    public TMPro.TextMeshProUGUI TextShadow { get; }

    public void Initialize(DialogBox dialogBox);
}


[CreateAssetMenu(fileName = "New Dialog Boxes Factory", menuName = "Pokemon Unity/UI/Dialog Box Factory")]
public class DialogBoxFactory : ScriptableObject {
    //public static DialogBox Singleton;

    /// <summary>Access this list for easy access to available Dialog Boxes to Spawn</summary>
    [Description("This ScriptableObject needs to be attached to GlobalVariables for it to load the available boxes")]
    public List<DialogBox> DialogBoxes;
    public static GameObject DialogBoxesParent;
    public static Dictionary<string, DialogBox> QuickBoxes = new();
    static Dictionary<DialogBox, DialogueBoxBehaviour> pool = new();

    public static void InitializeQuickBoxes(List<DialogBox> dialogBoxes) {
        QuickBoxes.Clear();
        foreach (DialogBox dialogBox in dialogBoxes) {
            QuickBoxes.Add(dialogBox.Name, dialogBox);
        }
    }

    public static DialogueBoxBehaviour OpenDialog(string dialogBoxName) {
        if (dialogBoxName == null || dialogBoxName == "") {
            Debug.LogError("Null string provided");
            return null;
        }
        if (!QuickBoxes.ContainsKey(dialogBoxName)) {
            Debug.LogError($"The DialogBox with the name '{dialogBoxName}' does not exist in the Factories Quick Boxes. Was IniitalizeQuickBoxes ran?");
            return null;
        }

        return OpenDialog(QuickBoxes[dialogBoxName]);
    }

    public static DialogueBoxBehaviour OpenDialog(DialogBox dialogBox) {
        if (dialogBox == null) {
            Debug.LogError("Null DialogBox provided");
            return null;
        }
        if (pool.ContainsKey(dialogBox))
            return pool[dialogBox];

        if (dialogBox.Prefab == null) {
            Debug.LogError($"Dialog Box '{dialogBox.name}' does not have a Prefab set on it", dialogBox);
            return null;
        }
        
        DialogueBoxBehaviour dialogBoxObject = Instantiate(dialogBox.Prefab);
        RectTransform rectTransform = ((RectTransform)dialogBoxObject.transform);
        dialogBoxObject.Initialize(dialogBox);
        Vector2 offsetMax = rectTransform.offsetMax;
        Vector2 offsetMin = rectTransform.offsetMin;
        rectTransform.SetParent(DialogBoxesParent.transform);
        rectTransform.offsetMax = offsetMax;
        rectTransform.offsetMin = offsetMin;
        rectTransform.localScale = Vector3.one;
        return dialogBoxObject;
    }
}
