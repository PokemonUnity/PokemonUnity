using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class SelectableExtensions
{
    public static bool IsSelected(this Selectable selectable) {
        return selectable.gameObject == EventSystem.current.currentSelectedGameObject;
    }
}
