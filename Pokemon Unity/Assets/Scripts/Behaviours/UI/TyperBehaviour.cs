using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TyperBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [TextArea][SerializeField] string message = "";
    float typingSpeed = 0.1f;
    int currentLength = 0;
    float passedTime = 0f;

    void Awake() {
        if (PlayerPrefs.HasKey("Text Speed")) typingSpeed = PlayerPrefs.GetFloat("Text Speed");
    }

    void Update() {
        if (typingSpeed == 0f) {
            text.text = message;
            return;
        }
        passedTime += Time.deltaTime;
        if (passedTime > typingSpeed) UpdateText();
    }

    public void TypeMessage(string message) {
        typingSpeed = PlayerPrefs.GetFloat(PlayerPrefKeys.Keys[EPlayerPrefKeys.TEXTSPEED]);
        this.message = message;
        text.text = "";
        currentLength = 0;
    }

    public void TypeHelpText(UIInputBehaviour input) {
        TypeMessage(input.HelpText);
    }

    public void UpdateText() {
        passedTime = 0f;
        if (currentLength < message.Length) {
            currentLength++;
            text.text = message.Substring(0, currentLength);
        }
    }
}
