using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Billboarding))]
public class BillboardingEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        Billboarding obj = (Billboarding) target;

        //GUILayout.Toggle(obj.turnX, "Turn X");
        //GUILayout.Toggle(obj.turnY, "Turn Y");
        //GUILayout.Toggle(obj.turnZ, "Turn Z");
    }
}