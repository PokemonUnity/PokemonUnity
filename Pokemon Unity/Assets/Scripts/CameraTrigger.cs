//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class CameraTrigger : MonoBehaviour
{

    public Vector2 size;

    private BoxCollider boxCollider;
    private GameObject camera;
    private GameObject view;


    void Awake()
    {
        boxCollider = transform.GetComponent<BoxCollider>();
        camera = GameObject.Find("Player/Camera").gameObject;
        view = transform.Find("View").gameObject;
    }

    void Start()
    {
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            camera.SendMessage("setCamera", view.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            camera.SendMessage("unfreeCamera");
        }
    }
}