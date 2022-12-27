using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Canvas))]
public class StartMenuHandler : MonoBehaviour
{
    [SerializeField] MainMenuHandler mainMenuHandler;
    [SerializeField] GameObject continueButton;

    void Start()
    {
        if (mainMenuHandler is null) {
            Debug.LogError("Main Menu Handler component was not provided");
            return;
        }
        MenuHandler menuHandler = mainMenuHandler.MenuHandler;
        bool hasSaveFile = SaveLoad.Load();
        if (!hasSaveFile)
            continueButton.SetActive(false);
        Canvas canvas = GetComponent<Canvas>();
        menuHandler.ChangeMenu(menuHandler.GetCanvasIndex(canvas));
    }
}
