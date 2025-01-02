using UnityEditor;
using UnityEngine;
using System.Linq;

[CustomPropertyDrawer(typeof(ScriptableObjectFieldsAttribute))]
public class ScriptableObjectFieldsDrawer : PropertyDrawer {
    bool showContent = false;
    //bool showLongDescription = false;
    static float margin = 4f;
    static float height = 12f;

    private GUIStyle style;
    private GUIStyle styleFoldout;

    public ScriptableObjectFieldsDrawer() {
        style = new GUIStyle();
        style.normal.textColor = new Color(0.67f, 0.67f, 0.67f);
        style.wordWrap = true;
        styleFoldout = EditorStyles.foldout;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return (height * 2) + margin;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var classObject = property.objectReferenceValue;
        Rect nextYPos = new Rect(position.x, position.y, position.width, height);
        EditorGUI.ObjectField(nextYPos, property);
        if (classObject == null) return;
        var fieldValues = classObject.GetType()
                     .GetFields()
                     .Select(field => field.GetValue(classObject))
                     .ToList();
        var fieldNames = classObject.GetType().GetFields()
                            .Select(field => field.Name)
                            .ToList();
        nextYPos = new Rect(nextYPos.x, nextYPos.y += height, nextYPos.width, height);

        showContent = EditorGUILayout.Foldout(showContent, classObject.name, true, styleFoldout);
        if (showContent) {
            for (int i = 0; i < fieldValues.Count; i++) {
                EditorGUILayout.LabelField(fieldNames[i], fieldValues[i].ToString(), style);
            }
        }
        EditorGUILayout.Space();
    }
}
