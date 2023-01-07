using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/Settings/Frame Style Provider")]
[RequireComponent(typeof(Image))]
public class FrameStyleConsumer : MonoBehaviour
{
    Texture2D frame;
    Image image;

    void Awake()
    {
        //frame = PlayerPreferences.GetFrameStyle();
        image = GetComponent<Image>();
    }
}
