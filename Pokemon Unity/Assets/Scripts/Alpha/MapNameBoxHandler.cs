//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapNameBoxHandler : MonoBehaviour
{
    private Transform mapName;
    private Texture mapNameBox;
    private Text mapNameText;
    private Text mapNameTextShadow;

    private Coroutine mainDisplay;

    public float speed;
    private float increment;

    void Awake()
    {
        mapName = transform.Find("MapName");
        //mapNameBox = mapName.GetComponent<GUITexture>();
        //mapNameText = mapName.Find("BoxText").GetComponent<GUIText>();
        //mapNameTextShadow = mapName.Find("BoxTextShadow").GetComponent<GUIText>();
    }

    void Start()
    {
        mapName.position = new Vector3(0, 0.17f, mapName.position.z);
    }

    public void display(Texture boxTexture, string name, Color textColor)
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

    private IEnumerator displayCoroutine(Texture boxTexture, string name, Color textColor)
    {
        if (mapName.position.y != 0.17f)
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
        }
        //mapNameBox.texture = boxTexture;
        mapNameText.text = name;
        mapNameTextShadow.text = name;
        mapNameText.color = textColor;

        increment = 0f;
        while (increment < 1)
        {
            increment += (1 / speed) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }
            mapName.position = new Vector3(0, 0.17f - (0.17f * increment), mapName.position.z);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        increment = 0f;
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
    }
}