//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapNameBoxHandler : MonoBehaviour
{
    private Transform mapName;
    private Image mapNameBox;
    private Text mapNameText;
    private Text mapNameTextShadow;

    private Coroutine mainDisplay;

    public float speed;
    private float increment;

    private Animator animator;

    void Awake()
    {
        mapName = transform.Find("MapName");
        mapNameBox = mapName.GetComponent<Image>();
        mapNameText = mapName.Find("BoxText").GetComponent<Text>();
        mapNameTextShadow = mapName.Find("BoxTextShadow").GetComponent<Text>();

        animator = mapName.GetComponent<Animator>();
    }

    void Start()
    {
        //mapName.position = new Vector3(0, 0.17f, mapName.position.z);
    }

    public void display(string name, Color textColor) {
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

    private IEnumerator displayCoroutine(string name, Color textColor)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MapName Appear"))
        {
            animator.Play("MapName Disappear", 0, 0f);
            animator.speed = 2;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("MapName Appear"))
        {
            animator.speed = 2;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        
        animator.speed = 1;

        //mapNameBox.sprite = boxTexture;
        mapNameText.text = name;
        mapNameTextShadow.text = name;
        //mapNameText.color = textColor;

        animator.Play("MapName Appear", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);

        yield return new WaitForSeconds(2f);

        animator.Play("MapName Disappear", 0, 0f);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        animator.Play("MapName Idle", 0, 0f);
    }
}