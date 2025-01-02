using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialog Boxes Factory", menuName = "Pokemon Unity/UI/Dialog Box Factory")]
public class DialogBoxFactory : ScriptableObject {
    //public static DialogBox Singleton;

    /// <summary>Access this list for easy access to available Dialog Boxes to Spawn</summary>
    [Description("This ScriptableObject needs to be attached to GlobalVariables for it to load the available boxes")]
    public List<DialogBox> DialogBoxes;
    public static GameObject DialogBoxesParent;
    public static Dictionary<string, DialogBox> QuickBoxes = new();
    static Dictionary<DialogBox, DialogBoxBehaviour> pool = new();

    public static void InitializeQuickBoxes(List<DialogBox> dialogBoxes) {
        QuickBoxes.Clear();
        foreach (DialogBox dialogBox in dialogBoxes) {
            QuickBoxes.Add(dialogBox.Name, dialogBox);
        }
    }

    #region Creation

    public static DialogBoxBehaviour OpenDialog(string dialogBoxName) {
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

    public static DialogBoxBehaviour OpenDialog(DialogBox dialogBox) {
        if (dialogBox == null) {
            Debug.LogError("Null DialogBox provided");
            return null;
        }
        if (dialogBox.Prefab == null) {
            Debug.LogError($"Dialog Box '{dialogBox.name}' does not have a Prefab set on it", dialogBox);
            return null;
        }

        if (pool.ContainsKey(dialogBox)) {
            DialogBoxBehaviour box = pool[dialogBox];
            box.gameObject.SetActive(true);
            return box;
        }
        
        DialogBoxBehaviour dialogBoxObject = Instantiate(dialogBox.Prefab);
        pool[dialogBox] = dialogBoxObject;
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

    #endregion

    #region Destruction

    public static void CloseDialog(DialogBox dialogBox) {
        if (!pool.ContainsKey(dialogBox)) {
            Debug.LogError("DialogBox does not exist in the pool. Use Create or OpenDialog to create DialogBoxBehaviours");
            return;
        }
        DialogBoxBehaviour dialogBoxBehaviour = pool[dialogBox];
        dialogBoxBehaviour.gameObject.SetActive(false);
    }

    #endregion

}
