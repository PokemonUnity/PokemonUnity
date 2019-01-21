using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
 
public class MapCompiler : EditorWindow
{ 
    //private static int totalSprites = 2; // change depending on the total sprites in this batch!
    [MenuItem("PK Unity/Compile Selected Map")]
    static void Compile()
    {
        Debug.Log("Compiling map...");
        Mesh map = new Mesh();
        if(!Selection.activeGameObject.transform.GetComponent<MeshFilter>())
        {
            Selection.activeGameObject.AddComponent<MeshFilter>();
        }
        if(!Selection.activeGameObject.transform.GetComponent<MeshCollider>())
        {
            Selection.activeGameObject.AddComponent<MeshCollider>();
        }
        Selection.activeGameObject.transform.GetComponent<MeshFilter>().mesh = null;
        MeshFilter[] meshFilters = Selection.activeGameObject.transform.GetComponentsInChildren<MeshFilter>();
        Debug.Log(meshFilters.Length);
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];
        int i = 0;
        while (i < meshFilters.Length) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            i++;
        }
		map.CombineMeshes(combine);
        Selection.activeGameObject.transform.GetComponent<MeshFilter>().sharedMesh = map;
		Selection.activeGameObject.transform.GetComponent<MeshCollider>().sharedMesh = Selection.activeGameObject.transform.GetComponent<MeshFilter>().sharedMesh;
        Selection.activeGameObject.SetActive(true);
    }
    // Validate the menu item defined by the function above.
    // The menu item will be disabled if this function returns false.
    [MenuItem("PK Unity/Compile Selected Map", true)]
    static bool ValidateCompile()
    {
        // Return false if no gameobject is selected.
        return Selection.activeGameObject != null;
    }
}