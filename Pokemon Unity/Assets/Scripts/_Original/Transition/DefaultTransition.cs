using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefaultTransition : MonoBehaviour
{
    public float speed = 1;
    
    public Image 
        vsLeft,
        vsRight,
        mugShotLeft,
        mugShotRight,
        vs,
        flash;
        
    public Text
        leftText, leftTextShadow,
        rightText, rightTextShadow;

    private float offset;
    
    // Start is called before the first frame update
    void Start()
    {
        vsLeft.rectTransform.localPosition = new Vector3(-343, 0, 0);
        vsRight.rectTransform.localPosition = new Vector3(343, 0, 0);
        LeanTween.scale(vs.gameObject, new Vector3(0.5f, 0.5f, 0.5f), 0);
    }

    // Update is called once per frame
    void Update()
    {
        offset += speed * Time.deltaTime;

        vsLeft.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
        vsRight.material.SetTextureOffset("_MainTex", new Vector2(offset * -1, 0));
    }

    public void setSprites()
    {
        
    }

    public IEnumerator animate()
    {
        float a = 1;

        LeanTween.moveLocalX(vsLeft.gameObject, -171.5f, 0.3f);
        LeanTween.moveLocalX(vsRight.gameObject, 171.5f, 0.3f);

        yield return new WaitForSeconds(0.25f);

        flash.color = Color.white;
        
        mugShotLeft.gameObject.SetActive(true);
        mugShotRight.gameObject.SetActive(true);
        
        leftText.gameObject.SetActive(true);
        leftTextShadow.gameObject.SetActive(true);
        rightText.gameObject.SetActive(true);
        rightTextShadow.gameObject.SetActive(true);
        leftText.text = "Lucas";
        leftTextShadow.text = "Lucas";
        rightText.text = "Cynthia";
        rightTextShadow.text = "Cynthia";
        
        vs.gameObject.SetActive(true);

        while (a > 0)
        {
            a -= Time.deltaTime * 2;
            flash.color = new Color(1, 1, 1, a);

            if (a < 0) a = 0;

            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        LeanTween.scale(vs.gameObject, new Vector3(2.3f, 2.3f, 2.3f), 1.5f);

        yield return new WaitForSeconds(0.5f);

        yield return StartCoroutine(ScreenFade.main.Fade(false, 1f));
        
        mugShotLeft.gameObject.SetActive(false);
        mugShotRight.gameObject.SetActive(false);
        
        leftText.gameObject.SetActive(false);
        leftTextShadow.gameObject.SetActive(false);
        rightText.gameObject.SetActive(false);
        rightTextShadow.gameObject.SetActive(false);
        
        gameObject.SetActive(false);
        
        yield return null;
    }
}
