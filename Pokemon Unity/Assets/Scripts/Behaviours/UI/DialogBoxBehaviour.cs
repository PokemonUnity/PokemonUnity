using MarkupAttributes;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

[AddComponentMenu("Pokemon Unity/Dialogue Box")]
[RequireComponent(typeof(PlayerInputConsumer))]
[RequireComponent(typeof(Image))]
public class DialogBoxBehaviour : MonoBehaviour, IUseADialogBox {

    #region Variables

    [SerializeField] string UiActionMapName = "UI";
    [SerializeField] float heightWithoutText = 15f;
    [SerializeField] int linesToShow = 1;
    public List<DialogTrigger> DialogTriggers;
    [Foldout("Dialog Box Components")]
    [SerializeField] Image frame;
    GameSetting<Sprite> frameSetting;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI textShadow;
    [SerializeField] TypewriterBehaviour typewriter;
    DialogEpisode dialogEpisode;
    DialogBox dialogBox;
    int dialogIndex = 0;

    PlayerInputConsumer playerInputConsumer;

    public Image Image => frame;
    public TextMeshProUGUI Text => text;
    public TextMeshProUGUI TextShadow => textShadow;
    public GameSetting<Sprite> FrameSetting => frameSetting;

    /// <summary>The font height of the foreground text</summary>
    public float TextHeight { get => text.fontSize; }

    #endregion

    #region Monobehaviour Functions

    void OnValidate() {
        if (frame == null) Debug.LogError("No Frame provided");
        if (text == null) Debug.LogError("No Text provided");
        if (textShadow == null) Debug.LogError("No Text Shadow provided");
        if (typewriter == null) Debug.LogError("No Typewriter provided");
        Resize(linesToShow);
    }

    void Awake() {
        playerInputConsumer = GetComponent<PlayerInputConsumer>();
    }

    void Start() {
        frame = GetComponent<Image>();
        if (frameSetting != null) {
            frameSetting.OnValueChange.AddListener(UpdateFrame);
            UpdateFrame(frameSetting.Get());
        }
    }

    #endregion

    public void Resize(int linesToShow) {
        this.linesToShow = linesToShow;
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, rectTransform.offsetMin.y + heightWithoutText + (TextHeight * this.linesToShow));
        Canvas.ForceUpdateCanvases();
        //rectTransform.sizeDelta = new Vector2(250f, );
    }

    void UpdateFrame(Sprite frame) => this.frame.sprite = frame;

    public void Dialog(DialogEpisode dialogEpisode, DialogTrigger dialogTrigger, int linesToShow = 2) {
        dialogIndex = 0;
        DialogTriggers.Add(dialogTrigger);
        this.linesToShow = linesToShow;
        this.dialogEpisode = dialogEpisode;
        foreach (DialogTrigger trigger in DialogTriggers)
            trigger.OnEpisodeStart.Invoke(dialogEpisode);

        playerInputConsumer.SwitchActionMap(UiActionMapName);
        NextDialog();
    }

    public void NextDialog(CallbackContext context) {
        if (context.action.phase == UnityEngine.InputSystem.InputActionPhase.Performed) 
            NextDialog();
    }

    public void NextDialog() {
        if (dialogIndex == dialogEpisode.Dialogs.Count) {
            foreach (DialogTrigger trigger in DialogTriggers)
                trigger.OnEpisodeEnd.Invoke(dialogEpisode);

            DialogBoxFactory.CloseDialog(dialogBox);
            return;
        }
        Dialog dialog = dialogEpisode.Dialogs[dialogIndex];
        foreach (DialogTrigger trigger in DialogTriggers)
            trigger.OnDialogueFinished.Invoke(dialog);
        
        Resize(linesToShow);
        typewriter.Write(dialog.Phrase);
        dialogIndex++;
    }

    public void Write(string message) => typewriter.Write(message);

    public void Initialize(DialogBox dialogBox) {
        this.dialogBox = dialogBox;
        frameSetting = dialogBox.FrameSetting;
        frame.sprite = dialogBox.Frame;
        text.color = dialogBox.TextColor;
        textShadow.color = dialogBox.TextShadowColor;
    }
}
