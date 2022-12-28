using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class TextCarousel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TargetText;
    [SerializeField] Selectable selectable;
    public int ActiveIndex = 0;
    int savedIndex = -1;
    public List<string> CarouselItems;
    public UnityEvent<string> OnChange;

    // Start is called before the first frame update
    void Start()
    {
        if (TargetText is null) Debug.LogError("No TextMeshProUGUI provided");
        if (selectable is null) Debug.LogError("No Selectable provided");
        UpdateSelection(ActiveIndex, true);
    }

    public void UpdateSelectionOnClick() {
        Mouse device = InputSystem.GetDevice<Mouse>();
        if (device.rightButton.wasPressedThisFrame) {
            UpdateSelection(savedIndex - 1);
        } else if (device.leftButton.wasPressedThisFrame) {
            UpdateSelection(savedIndex + 1);
        }
    }

    public void UpdateSelection(CallbackContext context) {
        UpdateSelection(savedIndex + (int)context.ReadValue<Vector2>().x);
    }

    public void UpdateSelection(int index, bool force = false) {
        if (index == savedIndex) return;
        if (!selectable.IsSelected() && force == false) return;
        if (index < 0 || index >= CarouselItems.Count) return;
        savedIndex = index;
        OnChange.Invoke(CarouselItems[index]);
        TargetText.text = CarouselItems[index];
    }
}
