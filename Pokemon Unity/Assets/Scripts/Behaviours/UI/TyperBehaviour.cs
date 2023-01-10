using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TyperBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    public GameSetting<float> TypingSpeed;
    [TextArea][SerializeField] string message = "";
    float typingSpeed;
    int currentLength = 0;
    float passedTime = 0f;

    void Awake() {
        if (TypingSpeed == null) throw new GameSetting.NoGameSettingComponentSet(typeof(GameSetting<float>), gameObject);
        typingSpeed = TypingSpeed.Get();
        TypingSpeed.OnValueChange.AddListener(UpdateTypingSpeed);
    }

    void UpdateTypingSpeed(float value) => typingSpeed = value;

    void Update() {
        if (typingSpeed == 0f) {
            text.text = message;
            return;
        }
        passedTime += Time.deltaTime;
        if (passedTime > typingSpeed) UpdateText();
    }

    public void TypeMessage(string message) {
        this.message = message;
        text.text = "";
        currentLength = 0;
    }

    public void TypeHelpText(UIInputBehaviour input) {
        TypeMessage(input.HelpText);
    }

    public void TypeHelpText(BaseEventData eventData) {
        TypeHelpText(eventData.selectedObject.GetComponent<BaseEventData>());
    }

    public void UpdateText() {
        passedTime = 0f;
        if (currentLength < message.Length) {
            currentLength++;
            text.text = message.Substring(0, currentLength);
        }
    }
}
