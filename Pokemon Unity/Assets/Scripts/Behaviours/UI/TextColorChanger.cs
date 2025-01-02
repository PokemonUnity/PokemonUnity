using UnityEngine;
using TMPro;
using System;

[AddComponentMenu("Pokemon Unity/UI/Text Color Changer")]
public class TextColorChanger : MonoBehaviour {
    public TextMeshProUGUI text;
    [Description("Call the functions on this component in other components Events")]
    [SerializeField] Color color = new Color(1f, 1f, 1f, 255f);
    [SerializeField] Color originalColor = new Color(0.0627451f, 0.0627451f, 0.0627451f, 1f);
    
    void Awake() {
        if (text is null) text = GetComponent<TextMeshProUGUI>();
        if (text is null) throw new NoTextMeshProUGUIProvided(gameObject);
        originalColor = text.color;
    }

    public void ChangeColor() {
        //originalColor = text.color;
        text.color = color;
    }

    public void ChangeToOriginalColor() {
        text.color = originalColor;
    }

    class NoTextMeshProUGUIProvided : Exception {
        public NoTextMeshProUGUIProvided(GameObject gameObject) {
            Debug.LogError("No TextMeshProUGUI was provided and none were found as a component on this object", gameObject);
        }
    }
}
