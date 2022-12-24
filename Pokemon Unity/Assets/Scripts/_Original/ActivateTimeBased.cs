//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class ActivateTimeBased : MonoBehaviour
{
    public float activateTime = 20f;
    public float deactivateTime = 3.5f;

    private float currentTime;

    public GameObject target;

    void Awake()
    {
        if (target == null)
        {
            target = this.transform.GetChild(0).gameObject;
        }
    }

    void Start()
    {
        StartCoroutine(CheckActivation());
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
                target.gameObject.SetActive(currentTime >= activateTime && currentTime < deactivateTime);

            }
            else if (activateTime > deactivateTime)
            {
                target.gameObject.SetActive(currentTime >= activateTime || currentTime < deactivateTime);
            }

            yield return new WaitForSeconds(1f); //wait a second before repeating
        }
    }
}