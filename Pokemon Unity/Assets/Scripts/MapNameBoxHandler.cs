//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class MapNameBoxHandler : MonoBehaviour
{
    private Transform mapName;
    private Image mapNameBox;
    private Text mapNameText;
    private Text mapNameTextShadow;

    private Coroutine mainDisplay;

    public float speed;
    private float increment;

    void Awake()
    {
        
        mapName = GameObject.Find("MapName").transform;
        if (mapName == null){
            Debug.Log(gameObject.transform.Find("MapName").transform);
        }
        mapNameBox = mapName.GetComponent<Image>();
        mapNameText = mapNameBox.transform.Find("BoxText").GetComponent<Text>();
        //mapNameTextShadow = mapName.Find("BoxTextShadow").GetComponent<GUIText>();
    }

    void Start()
    {
        mapNameBox.rectTransform.DOAnchorPos(new Vector3(-114f, 118f, mapName.position.z),0f);
    }

    public void display(Sprite boxTexture, string name, Color textColor)
    {
        //do not display when on a map of the same name
        if (mapNameText.text != name)
        {
            if (mainDisplay != null)
            {
                StopCoroutine(mainDisplay);
            }
            mainDisplay = StartCoroutine(displayCoroutine(boxTexture, name, textColor));
        }
    }

    private IEnumerator displayCoroutine(Sprite boxTexture, string name, Color textColor)
    {
        //Useless part, using DOTween solves it easily
        /* if (mapName.position.y != 0.17f)
        {
            increment = mapName.position.y / 0.17f;
            while (increment < 1)
            {
                increment += (1 / speed) * Time.deltaTime;
                if (increment > 1)
                {
                    increment = 1;
                }
                mapName.position = new Vector3(0, 0.17f * increment, mapName.position.z);
                yield return null;
            }
        } */
        mapNameBox.rectTransform.DOAnchorPos(new Vector3(-114f, 74f, mapName.position.z),speed);
        //mapNameBox.texture = boxTexture;
        mapNameBox.sprite = boxTexture;
        mapNameText.text = name;

        //Useless part, using DOTween solves it easily
        //mapNameTextShadow.text = name;
        mapNameText.color = textColor;

        /* increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            mapName.position = new Vector3(0, 0.17f - (0.17f * increment), mapName.position.z);
            yield return null;
        } */

        yield return new WaitForSeconds(2f);

        mapNameBox.rectTransform.DOAnchorPos(new Vector3(-114f, 118f, mapName.position.z),speed);

        //Same as upper
        /* increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            mapName.position = new Vector3(0, 0.17f * increment, mapName.position.z);
            yield return null;
        } */
    }
}