using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Map : MonoBehaviour
{
    /// <summary>
    /// Set this variable in the inspector
    /// </summary>
    public GameObject mapPrefab;
    private Vector3[] objectPositions;

    private void Awake()
    {
        foreach (Vector3 objectPos in objectPositions)
        {
            GameObject mapObject = Instantiate(mapPrefab) as GameObject;
            mapObject.transform.position = objectPos;
        }
    }

    static Map()
    {
        //objectPositions = new Array[] {} //ToDo: List of map names and vectors
    }
}
