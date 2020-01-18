//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SetMatTint : MonoBehaviour
{
    public Color[] matIDColors = new Color[1];

    private MeshRenderer mesh;

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        Material[] mats = mesh.materials;

        for (int i = 0; i < mats.Length && i < matIDColors.Length; i++)
        {
            mats[i].color = matIDColors[i];
        }
    }
}