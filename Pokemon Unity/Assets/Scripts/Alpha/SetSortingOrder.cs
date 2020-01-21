//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class SetSortingOrder : MonoBehaviour
{
    public string layerName;
    public int order;

    private MeshRenderer meshRenderer;

    void Awake()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();

        if (layerName.Length > 0)
        {
            meshRenderer.sortingLayerName = layerName;
        }
        meshRenderer.sortingOrder = order;
    }
}