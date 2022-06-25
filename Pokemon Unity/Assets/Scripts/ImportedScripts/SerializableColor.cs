using UnityEngine;
 
[System.Serializable]
public class SerializableColor
{
    public float _r;
    public float _g;
    public float _b;
    public float _a;
 
    public Color Color
    {
        get
        {
            return new Color(_r, _g, _b, _a);
        }
        set
        {
            _r = value.r;
            _g = value.g;
            _b = value.b;
            _a = value.a;
        }
    }
 
    public SerializableColor()
    {
        // (Optional) Default to white with an empty initialisation
        _r = 1f;
        _g = 1f;
        _b = 1f;
        _a = 1f;
    }
 
    public SerializableColor(float r, float g, float b, float a = 0f)
    {
        _r = r;
        _g = g;
        _b = b;
        _a = a;
    }
 
    public SerializableColor(Color color)
    {
        _r = color.r;
        _g = color.g;
        _b = color.b;
        _a = color.a;
    }
}