using UnityEngine;
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
}
