using UnityEngine;
using UnityEngine.UI;

public class StartMenuBehaviour : MenuBehaviour 
{
    [SerializeField] Selectable continueButton;
    [SerializeField] Selectable newGameButton;

    public override Selectable GetFirstSelectable() {
        bool hasSaveFile = SaveLoad.Load();
        if (!hasSaveFile) {
            continueButton.gameObject.SetActive(false);
            return newGameButton;
        } else return continueButton;
    }
}