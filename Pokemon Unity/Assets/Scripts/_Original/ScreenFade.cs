//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/UI/Screen Fade Singleton")]
[RequireComponent(typeof(Image))]
public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Singleton;
    public static float DefaultSpeed = 0.4f;
    public static float SlowedSpeed = 1.2f;

    private Image image;

    public Sprite[] fadeCutouts;

    public Material fadeMaterial;

    void OnValidate() {
        if (image == null) image = GetComponent<Image>();
        SetToFadedOut();
    }

    void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);

        image = GetComponent<Image>();
        SetToFadedIn();
    }


    public IEnumerator Fade(bool fadeIn, float speed)
    {
        yield return StartCoroutine(Fade(fadeIn, speed, Color.black));
    }

    public IEnumerator Fade(bool fadeIn, float time, Color color)
    {
        image.sprite = null;
        image.material = null;

        float alpha = fadeIn ? 1f : 0f;
        image.color = new Color(color.r, color.g, color.b, alpha);
        
        LeanTween.alpha(image.GetComponent<RectTransform>(), 1-alpha, time);
        
        yield return new WaitForSeconds(time);
    }

    public IEnumerator FadeCutout(bool fadeIn, float speed, Sprite cutout)
    {
        if (cutout == null)
            cutout = fadeCutouts[Random.Range(0, fadeCutouts.Length)];

        image.sprite = cutout;
        image.material = fadeMaterial;

        float increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
                increment = 1;

            float alpha = (!fadeIn) ? 1f - increment : increment;

            image.material.SetFloat("_Cutoff", alpha);
            yield return null;
        }
    }

    public void SetToFadedOut()
    {
        image.sprite = null;
        image.material = null;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
    }

    public void SetToFadedIn()
    {
        image.sprite = null;
        image.material = null;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
    }
}