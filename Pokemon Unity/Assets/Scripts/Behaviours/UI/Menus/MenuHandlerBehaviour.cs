using EasyButtons;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MenuHandlerBehaviour : MonoBehaviour
{
    public MenuHandler MenuHandler;

    // Update is called once per frame
    protected void Start()
    {
        MenuHandler.ChangeMenu(MenuHandler.firstMenuIndex);
    }

    public void ChangeMenu(int index) => MenuHandler.ChangeMenu(index);
    public void ChangeToPreviousMenu() => MenuHandler.ChangeToPreviousMenu();

    [Button]
    public void SeeCurrentlySelected() {
        Debug.Log("Currently selected object: " + EventSystem.current.currentSelectedGameObject.name, EventSystem.current.currentSelectedGameObject);
    }
}
