//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class ReflectiveWaterHandler : MonoBehaviour
{
    public bool autoSize = true;

    public Vector2 size;

    private BoxCollider boxCollider;
    private Transform water;


    void Awake()
    {
        boxCollider = transform.GetComponent<BoxCollider>();
        water = transform.GetChild(0);
    }

    void Start()
    {
        if (autoSize)
        {
            UpdateSettings();
        }
    }

    void UpdateSettings()
    {
        if (water == null)
        {
            water = transform.GetChild(0);
            boxCollider = transform.GetComponent<BoxCollider>();
        }
        else if (size.x > 0 && size.y > 0)
        {
            water.localScale = new Vector3(size.x, size.y, 1);
            water.localPosition = new Vector3((size.x - 1) * 0.5f, -0.3f, 0 - (size.y - 1) * 0.5f);
            boxCollider.size = new Vector3(size.x - 0.2f, 1, size.y + 0.8f);
            boxCollider.center = new Vector3((size.x - 1) * 0.5f, 0.5f, 0.5f - (size.y - 1) * 0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Application.isEditor)
        {
            if (autoSize)
            {
                UpdateSettings();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.name.Contains("map"))
        {
            if (other.transform.parent != null)
            {
                other.transform.parent.gameObject.SendMessage("reflect", true, SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.name.Contains("map"))
        {
            other.transform.parent.gameObject.SendMessage("reflect", false, SendMessageOptions.DontRequireReceiver);
        }
    }
}