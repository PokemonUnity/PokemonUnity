using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map Settings", menuName = "Pokemon Unity/Map")]
public class PokemonMap : ScriptableObject
{
    public string Name;
    public MapNameBox MapNameBox;
    public AudioTrack DayBackgroundMusic;
    public AudioTrack NightBackgroundMusic;
    public string RPCImageKey;

    void OnValidate() {
        MapNameBox.Text = Name;
    }

    public new string name { get => Name; }
}

    
