using MarkupAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/Dialogue Box")]
[RequireComponent(typeof(Image))]
public class DialogueBoxBehaviour : MonoBehaviour, IUseADialogBox {
    [SerializeField] Image frame;
    GameSetting<Sprite> frameSetting;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI textShadow;
    [SerializeField] TypewriterBehaviour typewriter;

    public Image Image => frame;
    public TextMeshProUGUI Text => text;
    public TextMeshProUGUI TextShadow => textShadow;
    public GameSetting<Sprite> FrameSetting => frameSetting;

    void OnValidate() {
        if (frame == null) Debug.LogError("No Frame provided");
        if (text == null) Debug.LogError("No Text provided");
        if (textShadow == null) Debug.LogError("No Text Shadow provided");
        if (typewriter == null) Debug.LogError("No Typewriter provided");
    }

    void Start() {
        frame = GetComponent<Image>();
        if (frameSetting != null) {
            frameSetting.OnValueChange.AddListener(updateFrame);
            updateFrame(frameSetting.Get());
        }
    }

    void updateFrame(Sprite frame) {
        this.frame.sprite = frame;
    }

    public void Dialog(DialogEpisode message, DialogTrigger dialogTrigger) {
        foreach (Dialog dialog in message.Dialogue) {
            //typewriter.Write(message.Dialogue)
        }
    }

    public void Write(string message) => typewriter.Write(message);

    public void Initialize(DialogBox dialogBox) {
        frameSetting = dialogBox.FrameSetting;
        frame.sprite = dialogBox.Frame;
        text.color = dialogBox.TextColor;
        textShadow.color = dialogBox.TextShadowColor;
    }
}
