//Original Scripts by IIColour (IIColour_Spectrum)

using UnityEngine;
using System.Collections;

public class flycameraDebug : MonoBehaviour
{
    //Flies a camera between given nodes in order at constant speed.
    //Used to record camera panning trailer footage.
    //Currently no in-game application.

    public GameObject target;

    public float speed;
    public float initialDelay;

    public Transform[] nodes;
    public Vector3[] addedRotation;

    void Awake()
    {
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].GetComponent<MeshRenderer>() != null)
            {
                nodes[i].GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }

    void Start()
    {
        target.transform.position = nodes[0].position;
        StartCoroutine(flyToEachPosition());
    }

    private IEnumerator flyToEachPosition()
    {
        yield return new WaitForSeconds(initialDelay);

        for (int i = 0; i < nodes.Length; i++)
        {
            if (addedRotation.Length <= i)
            {
                yield return StartCoroutine(flyToPosition(nodes[i].position, Vector3.zero));
            }
            else
            {
                yield return StartCoroutine(flyToPosition(nodes[i].position, addedRotation[i]));
            }
        }
    }

    private IEnumerator flyToPosition(Vector3 destinationPosition, Vector3 additiveRotation)
    {
        Vector3 startPosition = target.transform.position;
        Vector3 distance = destinationPosition - startPosition;

        float time = Vector3.Distance(startPosition, destinationPosition) / speed;

        StartCoroutine(RotateCamera(additiveRotation, time));

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            target.transform.position = startPosition + (distance * increment);

            yield return null;
        }
    }

    private IEnumerator RotateCamera(Vector3 additiveRotation, float time)
    {
        Vector3 startRotation = target.transform.rotation.eulerAngles;

        float increment = 0;
        while (increment < 1)
        {
            increment += (1 / time) * Time.deltaTime;
            if (increment > 1)
            {
                increment = 1;
            }

            target.transform.rotation = Quaternion.Euler(startRotation + (additiveRotation * increment));

            yield return null;
        }
    }
}