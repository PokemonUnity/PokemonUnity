//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class WindowLight : MonoBehaviour
{
    public float activateTime = 20f;
    public float deactivateTime = 3.5f;

    public Material lightOffMat;
    public Material lightOnMat;
    public GameObject lightObject;

    private float currentTime;
    private MeshRenderer mesh;

    void Awake()
    {
        mesh = gameObject.GetComponent<MeshRenderer>();
    }

    void Start()
    {
        StartCoroutine(CheckActivation());
    }

    private void setLightActive(bool activate)
    {
        if (lightObject != null)
        {
            lightObject.SetActive(activate);
        }
        if (activate)
        {
            mesh.material = lightOnMat;
        }
        else
        {
            mesh.material = lightOffMat;
        }
    }

    private IEnumerator CheckActivation()
    {
        //repeat every second
        while (true)
        {
            currentTime = (float) System.DateTime.Now.Hour + ((float) System.DateTime.Now.Minute / 60f);

            if (activateTime < deactivateTime)
            {
                //if time does not extend past midnight
                if (currentTime >= activateTime && currentTime < deactivateTime)
                {
                    setLightActive(true);
                }
                else
                {
                    setLightActive(false);
                }
            }
            else if (activateTime > deactivateTime)
            {
                if (currentTime >= activateTime || currentTime < deactivateTime)
                {
                    setLightActive(true);
                }
                else
                {
                    setLightActive(false);
                }
            }

            yield return new WaitForSeconds(1f); //wait a second before repeating
        }
    }
}