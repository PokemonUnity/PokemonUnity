//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class MapNameBoxBehaviour : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI mapNameText;
    [SerializeField] TextMeshProUGUI mapNameTextShadow;

    Image mapNameBox;
    Coroutine mainDisplay;

    EAnimationState animationState = EAnimationState.Idle;

    public float speed = 1f;
    //private float increment;

    void Awake()
    {
        mapNameBox = GetComponent<Image>();
        animator = GetComponent<Animator>();
        if (mapNameText == null) Debug.LogError("No TextMeshProUGUI provided for Map Name Text", gameObject);
        if (mapNameText == null) Debug.LogError("No TextMeshProUGUI provided for Map Name Text Shadow", gameObject);
    }

    void Start()
    {
        //mapName.position = new Vector3(0, 0.17f, mapName.position.z);
    }

    public void Display(string name, Color textColor) {
        //do not display when on a map of the same name
        if (mapNameText.text != name)
        {
            if (mainDisplay != null)
            {
                StopCoroutine(mainDisplay);
            }
            mainDisplay = StartCoroutine(displayCoroutine(name, textColor));
        }
    }

    void Disappear() {
        LeanTween.move();
    }

    IEnumerator displayCoroutine(string name, Color textColor)
    {
        if (animationState == EAnimationState.Appearing)
        {

            Disappear();
            yield return new WaitForSeconds(speed);
        }
        else if (animationState == EAnimationState.Appearing)
        {
            animator.speed = 2;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        
        animator.speed = 1;

        //mapNameBox.sprite = boxTexture;
        mapNameText.text = name;
        mapNameTextShadow.text = name;
        //mapNameText.color = textColor;

        animator.Play("Appear", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        yield return new WaitForSeconds(2f);

        animator.Play("Disappear", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        animator.Play("Idle", 0, 0f);
    }

    enum EAnimationState { 
        Idle,
        Appearing,
        Disappearing
    }
}