using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChangeTextColor : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Color color;
    Color previousColor;

    public void ChangeColor() {
        previousColor = text.color;
        text.color = color;
    }

    public void ChangeToPreviousColor() {
        text.color = previousColor;
    }
}
