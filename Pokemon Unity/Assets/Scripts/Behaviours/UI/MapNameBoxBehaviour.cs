//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[AddComponentMenu("Pokemon Unity/UI/World/Map Name Box")]
[RequireComponent(typeof(Image))]
public class MapNameBoxBehaviour : MonoBehaviour
{
    [SerializeField] MapNameBox mapNameBox;
    [SerializeField] TextMeshProUGUI mapNameText;

    Image image;
    Coroutine mainDisplay;

    EAnimationState animationState = EAnimationState.Idle;

    float waitTime = 2f;
    //private float increment;

    [Description("If (0, 0, 0), will use the current Transforms settings. Sets the anchored position")]
    public Vector3 HoverPosition;
    [Description("This is added to the HoverPosition")]
    public Vector3 HideDisplacement = Vector3.zero;
    RectTransform rectTransform;

    void OnValidate() {
        SetMapNameBox(mapNameBox);
    }

    void Awake()
    {
        rectTransform = (RectTransform)transform;
        if (HoverPosition == Vector3.zero) HoverPosition = rectTransform.anchoredPosition;
    }

    void Start() {
        StartCoroutine(startUp());
    }

    IEnumerator startUp() {
        yield return StartCoroutine(disappear(0f));
        AppearAndDisappear(mapNameBox);
    }

    void SetMapNameBox(MapNameBox newMapNameBox = null) {
        if (!enabled) return;
        if (mapNameBox != null) mapNameBox = newMapNameBox;
        if (mapNameBox == null) {
            Debug.LogError("No MapNameBox provided", gameObject);
            return;
        }
        if (image == null) image = GetComponent<Image>();
        image.sprite = mapNameBox.Sprite;
        if (mapNameText == null) {
            Debug.LogError("No TextMeshProUGUI provided for Map Name Text", gameObject);
            return;
        }
        mapNameText.text = mapNameBox.Text;
        mapNameText.color = mapNameBox.TextColor;
    }

    public void AppearAndDisappear() => AppearAndDisappear();

    public void AppearAndDisappear(MapNameBox mapNameBox = null) {
        SetMapNameBox(mapNameBox);
        if (mainDisplay != null)
            StopCoroutine(mainDisplay);
        mainDisplay = StartCoroutine(appearAndDisappearCoroutine());
    }

    public void Disappear() => StartCoroutine(disappear());

    public void Appear() => StartCoroutine(appear());

    IEnumerator disappear(float time = 1f) {
        LeanTween.cancel(gameObject);
        LeanTween.move(rectTransform, HoverPosition + HideDisplacement, time).setEaseInCubic();
        yield return new WaitForSeconds(time);
    }

    IEnumerator appear(float time = 1f) {
        LeanTween.cancel(gameObject);
        LeanTween.move(rectTransform, HoverPosition, time).setEaseOutCubic();
        yield return new WaitForSeconds(time);
    }

    IEnumerator appearAndDisappearCoroutine()
    {
        if (animationState == EAnimationState.Appearing) {
            yield return disappear(0.5f);
        }
        
        Appear();
        yield return new WaitForSeconds(waitTime);

        Disappear();
    }

    enum EAnimationState { 
        Idle,
        Appearing,
        Disappearing
    }
}