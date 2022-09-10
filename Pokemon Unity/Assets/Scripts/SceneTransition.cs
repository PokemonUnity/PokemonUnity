//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition gameScene;

    public float fadeSpeed = 1.2f;
    public bool fading = false;
    private bool fadingOut = false;
    private float increment = 0.5f;

    private Image screenFader;
    public Sprite defaultFadeTex;
    private Image screenFaderOnGUI;

    private bool RotatableGUI;

    void Awake()
    {
        //set up the reference to this script.
        if (transform.name == "GUI")
        {
            gameScene = this;
        }

        screenFader = transform.Find("ScreenFader").GetComponent<Image>();
        if (screenFader != null)
        {
            RotatableGUI = false;
        }
        else
        {
            RotatableGUI = true;
        }
        if (!RotatableGUI)
        {
            screenFader.rectTransform.position = new Vector2(0, 0);
            screenFader.rectTransform.sizeDelta = new Vector2(342, 192);
        }
        else
        {
            screenFaderOnGUI = transform.Find("ScreenFader").GetComponent<Image>();
            screenFaderOnGUI.rectTransform.sizeDelta = new Vector2(342, 192);
        }
    }


    void Start()
    {
        if (!GlobalVariables.global.fadeIn)
        {
            if (!fading)
            {
                if (!RotatableGUI)
                {
                    screenFader.enabled = false;
                }
                else
                {
                    screenFaderOnGUI.enabled = false;
                }
            }
        }
    }

    private IEnumerator fade(float speed)
    {
        if (!RotatableGUI)
        {
            if (screenFader.sprite == null)
            {
                screenFader.sprite = defaultFadeTex;
            }
        }
        else
        {
            if (screenFaderOnGUI.sprite == null)
            {
                screenFaderOnGUI.sprite = defaultFadeTex;
            }
        }
        if (speed == 0)
        {
            increment = 1;
            if (fadingOut)
            {
                if (!RotatableGUI)
                {
                    screenFader.color = new Color(screenFader.color.r, screenFader.color.g, screenFader.color.b,
                        (0f + increment) / 2);
                }
                else
                {
                    screenFaderOnGUI.color = new Color(screenFaderOnGUI.color.r, screenFaderOnGUI.color.g,
                        screenFaderOnGUI.color.b, increment);
                }
            }
            else
            {
                if (!RotatableGUI)
                {
                    screenFader.color = new Color(screenFader.color.r, screenFader.color.g, screenFader.color.b,
                        (1f - increment) / 2);
                }
                else
                {
                    screenFaderOnGUI.color = new Color(screenFaderOnGUI.color.r, screenFaderOnGUI.color.g,
                        screenFaderOnGUI.color.b, 1f - increment);
                }
            }
        }
        while (increment < 1)
        {
            increment += (1 / speed * 1.2f) * Time.deltaTime;
            if (fadingOut)
            {
                if (!RotatableGUI)
                {
                    screenFader.color = new Color(screenFader.color.r, screenFader.color.g, screenFader.color.b,
                        (0f + increment) / 2);
                }
                else
                {
                    screenFaderOnGUI.color = new Color(screenFaderOnGUI.color.r, screenFaderOnGUI.color.g,
                        screenFaderOnGUI.color.b, increment);
                }
            }
            else
            {
                if (!RotatableGUI)
                {
                    screenFader.color = new Color(screenFader.color.r, screenFader.color.g, screenFader.color.b,
                        (1f - increment) / 2);
                }
                else
                {
                    screenFaderOnGUI.color = new Color(screenFaderOnGUI.color.r, screenFaderOnGUI.color.g,
                        screenFaderOnGUI.color.b, 1f - increment);
                }
            }
            yield return null;
        }
        if (!fadingOut)
        {
            GlobalVariables.global.fadeTex = defaultFadeTex;
            if (!RotatableGUI)
            {
                screenFader.enabled = false;
            }
            else
            {
                screenFaderOnGUI.enabled = false;
            }
        }
        fading = false;
    }

    public float FadeIn(float speed = -1)
    {
        if (speed < 0)
            speed = fadeSpeed;

        if (!RotatableGUI)
        {
            screenFader.enabled = true;
            screenFader.sprite = GlobalVariables.global.fadeTex;
        }
        else
        {
            screenFaderOnGUI.enabled = true;
            screenFaderOnGUI.sprite = GlobalVariables.global.fadeTex;
        }
        increment = 0;
        fadingOut = false;
        fading = true;
        StartCoroutine("fade", speed);
        GlobalVariables.global.fadeIn = false;
        return speed;
    }

    public float FadeOut(float speed = -1)
    {
        if (speed < 0)
            speed = fadeSpeed;

        if (!RotatableGUI)
        {
            screenFader.enabled = true;
            screenFader.sprite = GlobalVariables.global.fadeTex;
        }
        else
        {
            screenFaderOnGUI.enabled = true;
            screenFaderOnGUI.sprite = GlobalVariables.global.fadeTex;
        }
        increment = 0;
        fadingOut = true;
        fading = true;
        StartCoroutine("fade", speed);
        GlobalVariables.global.fadeIn = true;
        return speed;
    }
}