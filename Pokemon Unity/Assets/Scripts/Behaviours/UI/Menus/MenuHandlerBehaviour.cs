using EasyButtons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[AddComponentMenu("Pokemon Unity/UI/Menus/Menu Handler", order: 0)]
[RequireComponent(typeof(PlayerInput))]
public class MenuHandlerBehaviour : MonoBehaviour
{
    [SerializeField] int firstMenuIndex = 0;
    public List<MenuBehaviour> Menus;
    int activeIndex = -1;
    int previousIndex;
    public float MenuFadeTime = ScreenFade.DefaultSpeed;

    // Update is called once per frame
    protected virtual void Start()
    {
        changeMenu(firstMenuIndex);
    }

    void changeMenu(int index, bool updatePrevious = true) {
        if (Menus.Count == 0f) {
            Debug.LogError("No Canvases given to MenuHandler");
            return;
        }
        if (index == activeIndex) return;
        if (activeIndex > -1 && updatePrevious) previousIndex = activeIndex;
        DisableAllMenus();

        MenuBehaviour menu = Menus[index];
        activeIndex = index;
        menu.gameObject.SetActive(true);
        menu.GetFirstSelectable().Select();
    }

    public void DisableAllMenus() {
        for (int i = 0; i < Menus.Count; i++)
            Menus[i].gameObject.SetActive(false);
    }

    public void EnableActiveMenu() => Menus[activeIndex].gameObject.SetActive(true);

    public void DisableActiveMenu() => Menus[activeIndex].gameObject.SetActive(false);

    /// <summary>Disables all other canvases, and selects the canvas first Button</summary>
    IEnumerator changeMenuCoroutine(int index, bool updatePrevious = true) {
        yield return StartCoroutine(ScreenFade.Singleton.Fade(false, MenuFadeTime));

        changeMenu(index, updatePrevious);

        yield return StartCoroutine(ScreenFade.Singleton.Fade(true, MenuFadeTime));

    }

    public void ChangeMenu(int index) => StartCoroutine(changeMenuCoroutine(index));

    public void ChangeToPreviousMenu() => StartCoroutine(changeMenuCoroutine(previousIndex, false));

    public void ChangeMenu(string menuName) {
        int index = Menus.FindIndex((MenuBehaviour menu) => menu.MenuName == "Settings");
        if (index < 0) {
            Debug.LogError($"Menu {menuName} does not exist");
            return;
        }
        StartCoroutine(changeMenuCoroutine(index));
    }

    [Button]
    public void SeeCurrentlySelected() {
        Debug.Log("Currently selected object: " + EventSystem.current.currentSelectedGameObject.name, EventSystem.current.currentSelectedGameObject);
    }
}
