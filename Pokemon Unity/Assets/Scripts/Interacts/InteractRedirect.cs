//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class InteractRedirect : MonoBehaviour
{
    public GameObject target;

    private IEnumerator interact()
    {
        target.SendMessage("interact", SendMessageOptions.DontRequireReceiver);
        yield return null;
    }

    private IEnumerator bump()
    {
        target.SendMessage("bump", SendMessageOptions.DontRequireReceiver);
        yield return null;
    }
}