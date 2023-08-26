using MarkupAttributes.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MonoBehaviour), true), CanEditMultipleObjects]
internal class MarkedUpMonoBehaviourEditor : MarkedUpEditor {
}

[CustomEditor(typeof(ScriptableObject), true), CanEditMultipleObjects]
internal class MarkedUpScriptableObjectEditor : MarkedUpEditor {
}
