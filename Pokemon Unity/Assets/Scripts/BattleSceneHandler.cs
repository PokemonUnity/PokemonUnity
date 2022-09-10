using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneHandler : MonoBehaviour
{
    public GameObject[] models;

    public void Darken(float time)
    {
        StartCoroutine(IEDarken(time));
    }
    
    public IEnumerator IEDarken(float time)
    {
        StartCoroutine(IEChangeColor(Color.black, time));
        yield return new WaitForSeconds(time);
        Disable();
    }


    public void Lighten(float time)
    {
        StartCoroutine(IELighten(time));
    }

    public IEnumerator IELighten(float time)
    {
        float rgb = (float) 210 / 255;
        Enable();
        StartCoroutine(IEChangeColor(new Color(rgb, rgb, rgb), time));
        yield return null;
    }
    
    public void Disable()
    {
        foreach (GameObject obj in models)
        {
            obj.SetActive(false);
        }
    }

    public void Enable()
    {
        foreach (GameObject obj in models)
        {
            obj.SetActive(true);
        }
    }

    private IEnumerator IEChangeColor(Color c, float time)
    {
        foreach (GameObject obj in models)
        {
            LeanTween.color(obj, c, time);
        }

        yield return null;
    }
    
}
