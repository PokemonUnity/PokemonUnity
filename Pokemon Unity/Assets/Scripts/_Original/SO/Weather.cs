using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeatherObject", menuName = "ScriptableObjects/Weather")]
public class Weather : ScriptableObject
{
    public enum WeatherType
    {
        Rain,
        Sand,
        Snow,
        Spore,
        Heat
    }
    
    public string name;
    public WeatherType type;
    public GameObject particle;
    public AudioClip ambientSound;
    public bool fogEnabled;
    public FogSettings fogSettings;
    
    [System.Serializable]
    public class FogSettings
    {
        public Color color;
        public FogMode mode = FogMode.Linear;
        public float start;
        public float end;
    }
}
