using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class MenuHandlerBehaviour : MonoBehaviour
{
    public MenuHandler MenuHandler;

    // Start is called before the first frame update
    protected void Start()
    {
        MenuHandler.ChangeMenu(MenuHandler.firstCanvas);
    }
}
