using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TextCarousel : UIInputBehaviour
{
    [SerializeField] TextMeshProUGUI TargetText;
    [SerializeField] Selectable selectable;
    public int ActiveIndex = 0;
    int savedIndex = -1;
    public List<string> CarouselItems;

    // Start is called before the first frame update
    void Start()
    {
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        if (selectable is null) Debug.LogError("No Selectable provided");
        //UpdateSelection(ActiveIndex, true);
    }

    public void UpdateSelectionRightClick() => UpdateSelection(savedIndex - 1);
    public void UpdateSelectionLeftClick() => UpdateSelection(savedIndex + 1);

    /// <summary>For button navigation</summary>
    public void UpdateSelection(CallbackContext context) {
        UpdateSelection(savedIndex + (int)context.ReadValue<Vector2>().x);
    }

    public void UpdateSelection(int index, bool force = false) {
        if (EventSystem.current.currentSelectedGameObject != TargetText) return;
        OnSelect.Invoke(TargetText.gameObject);
        if (index == savedIndex) return;
        if (!selectable.IsSelected() && force == false) return;
        if (index < 0 || index >= CarouselItems.Count) return;
        savedIndex = index;
        TargetText.text = CarouselItems[index];
        OnChange.Invoke(CarouselItems[index]);
    }
}
