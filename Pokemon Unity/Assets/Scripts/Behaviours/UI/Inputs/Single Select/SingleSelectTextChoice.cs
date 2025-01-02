using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[AddComponentMenu("Pokemon Unity/UI/Single Select/Single Select Choice")]
[RequireComponent(typeof(TextColorChanger))]
[RequireComponent(typeof(Selectable))]
public class SingleSelectTextChoice : MonoBehaviour
{
    public TextMeshProUGUI choiceText;
    [HideInInspector] public TextColorChanger colorChanger;

    void Start()
    {
        colorChanger = GetComponent<TextColorChanger>();
        colorChanger.text = choiceText;
    }
}
