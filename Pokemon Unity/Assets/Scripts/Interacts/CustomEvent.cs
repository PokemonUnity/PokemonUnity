//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;


[System.Serializable]
public class CVariable
{
    public string name;
    public float value;

    public CVariable(string name, float value)
    {
        this.name = name;
        this.value = value;
    }
}