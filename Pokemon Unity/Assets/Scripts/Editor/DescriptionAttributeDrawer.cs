using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DescriptionAttribute))]
public class DescriptionAttributeDrawer : PropertyDrawer {

    private GUIStyle style;

    public DescriptionAttributeDrawer() {
        style = new GUIStyle();
        style.normal.textColor = new Color(0.67f, 0.67f, 0.67f);
        style.richText = true;
        style.wordWrap = true;
        style.padding.left += 2;
        style.padding.top += 4;
        style.padding.bottom += 4;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        var descAttribute = (DescriptionAttribute)attribute;
        var content = new GUIContent(descAttribute.Description);
        return style.CalcHeight(label, EditorGUIUtility.currentViewWidth) + style.CalcHeight(content, EditorGUIUtility.currentViewWidth);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        var descAttribute = (DescriptionAttribute)attribute;
        Rect fieldPos = new Rect(position.x, position.y, position.width, 16f);
        Rect labelPos = new Rect(position.x, position.y + fieldPos.height, position.width, 16f);
        
        if (descAttribute.Position == DescriptionAttribute.EPosition.Above) 
            EditorGUI.LabelField(labelPos, descAttribute.Description, style);
        EditorGUI.PropertyField(fieldPos, property);
        if (descAttribute.Position == DescriptionAttribute.EPosition.Below)
            EditorGUI.LabelField(labelPos, descAttribute.Description, style);

        if (descAttribute.HasSpace)
            EditorGUILayout.Space();
    }
}
