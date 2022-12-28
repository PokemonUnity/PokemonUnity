using UnityEngine;
using TMPro;
using System;

public class TextColorChanger : MonoBehaviour {
    public TextMeshProUGUI text;
    [Description("Call the functions on this component in other components Events")]
    [SerializeField] Color color = new Color(255, 255, 255, 255f);
    Color previousColor;

    void Awake() {
        if (text is null) text = GetComponent<TextMeshProUGUI>();
        if (text is null) throw new NoTextMeshProUGUIProvided(gameObject);
        previousColor = text.color;
    }

    public void ChangeColor() {
        previousColor = text.color;
        text.color = color;
    }

    public void ChangeToPreviousColor() {
        text.color = previousColor;
    }

    class NoTextMeshProUGUIProvided : Exception {
        public NoTextMeshProUGUIProvided(GameObject gameObject) {
            Debug.LogError("No TextMeshProUGUI was provided and none were found as a component on this object", gameObject);
        }
    }
}
