using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[AddComponentMenu("Pokemon Unity/UI/Menus/Menu Handler", order: 0)]
[RequireComponent(typeof(PlayerInput))]
public class MenuHandler : MonoBehaviour
{
    public static MenuHandler Singleton;
    public List<MenuBehaviour> Menus;
    MenuBehaviour activeMenu;
    MenuBehaviour previousMenu;
    [SerializeField] float menuFadeTime = ScreenFade.DefaultSpeed;
    public static float MenuFadeTime = ScreenFade.DefaultSpeed;

    void Awake() {
        MenuFadeTime = menuFadeTime;
        if (Singleton == null) {
            Singleton = this;
        } else {
            Destroy(this);
            return;
        }
    }

    public void ChangeMenuInstantly(string name) => changeMenu(name);

    /// <summary>Disables all other canvases, and selects the canvas first Button</summary>
    void changeMenu(string name, bool updatePrevious = true) {
        int index = Menus.FindIndex((MenuBehaviour menu) => menu.MenuName == name);
        if (index < 0) {
            Debug.LogError($"The menu '{name}' does not exist");
            return;
        }

        if (Menus.Count == 0f) {
            Debug.LogError("No Canvases given to MenuHandler");
            return;
        }
        if (Menus[index] == activeMenu) return;
        if (activeMenu != null && updatePrevious) previousMenu = activeMenu;
        DisableAllMenus();

        MenuBehaviour menu = Menus[index];
        activeMenu = menu;
        menu.gameObject.SetActive(true);
        menu.GetFirstSelectable().Select();
    }

    void changeMenu(MenuBehaviour menu, bool updatePrevious = true) => changeMenu(menu.MenuName, updatePrevious);

    public void DisableAllMenus() {
        for (int i = 0; i < Menus.Count; i++)
            Menus[i].gameObject.SetActive(false);
    }

    public void EnableActiveMenu() => activeMenu.gameObject.SetActive(true);

    public void DisableActiveMenu() => activeMenu.gameObject.SetActive(false);

    public void ChangeToPreviousMenu() => StartCoroutine(changeMenuCoroutine(previousMenu.MenuName, false));

    public void ChangeMenu(string menuName) => StartCoroutine(changeMenuCoroutine(menuName));

    IEnumerator changeMenuCoroutine(string name, bool updatePrevious = true, float? time = null) {
        time ??= MenuFadeTime;
        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, time.Value));

        changeMenu(name, updatePrevious);

        yield return StartCoroutine(ScreenFade.Singleton.Fade(true, time.Value));
    }

    [Button]
    public void SeeCurrentlySelected() {
        Debug.Log("Currently selected object: " + EventSystem.current.currentSelectedGameObject.name, EventSystem.current.currentSelectedGameObject);
    }
}
