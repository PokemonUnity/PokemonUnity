using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Name Box", menuName = "Pokemon Unity/Map/Map Name Box")]
public class MapNameBox : ScriptableObject
{
    public string Text = "Map";
    public Color TextColor = new Color(16f/255f, 16f / 255f, 16f / 255f, 1f);
    public Sprite Sprite;
}
