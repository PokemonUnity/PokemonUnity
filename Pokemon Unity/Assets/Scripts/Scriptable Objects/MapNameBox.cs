using System;
using UnityEngine;

[Serializable]
public class MapNameBox
{
    [ReadOnly] public string Text;
    public Color TextColor = new Color(16f/255f, 16f / 255f, 16f / 255f, 1f);
    public Sprite Sprite;
}
