using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoGui : MonoBehaviour
{
    public GUILayer layer;

    // Start is called before the first frame update
    void Start()
    {
        layer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPressedNewGame()
    {
        bool result = layer.enabled ? false : true;
        layer.enabled = result;
        gameObject.SetActive(result ? false : true);
    }
}
