using TMPro;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/Dialogue Box")]
[RequireComponent(typeof(Image))]
public class DialogueBoxBehaviour : MonoBehaviour, IUseADialogBox {
    [SerializeField] Image frame;
    [SerializeField] GameSetting<Sprite> frameSetting;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI textShadow;
    [SerializeField] TypewriterBehaviour typewriter;

    public Image Frame => frame;
    public TextMeshProUGUI Text => text;
    public TextMeshProUGUI TextShadow => textShadow;
    public GameSetting<Sprite> FrameSetting => frameSetting;

    void Start() {
        frame = GetComponent<Image>();
        if (frameSetting != null) {
            frameSetting.OnValueChange.AddListener(UpdateFrame);
            UpdateFrame(frameSetting.Get());
        }
    }

    public void UpdateFrame(Sprite frame) {
        this.frame.sprite = frame;
    }

    public void Initialize(DialogBox dialogBox) {
        frameSetting = dialogBox.FrameSetting;
        frame.sprite = dialogBox.Frame;
        text.color = dialogBox.TextColor;
        textShadow.color = dialogBox.TextShadowColor;
    }
}
