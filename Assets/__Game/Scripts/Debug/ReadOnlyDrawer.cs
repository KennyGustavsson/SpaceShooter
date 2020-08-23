using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

#if UNITY_EDITOR 
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer {

    public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label) {
        string valueStr;

        switch (prop.propertyType) {
            case SerializedPropertyType.Integer:
                valueStr = prop.intValue.ToString();
                break;
            case SerializedPropertyType.Boolean:
                valueStr = prop.boolValue.ToString();
                break;
            case SerializedPropertyType.Float:
                valueStr = prop.floatValue.ToString("0.00");
                break;
            case SerializedPropertyType.String:
                valueStr = prop.stringValue;
                break;
            case SerializedPropertyType.Enum:
                valueStr = prop.enumDisplayNames[prop.enumValueIndex];
                break;
            case SerializedPropertyType.Vector3:
                valueStr = prop.vector3Value.ToString();
                break;
            case SerializedPropertyType.Quaternion:
                valueStr = prop.quaternionValue.ToString();
                break;
            case SerializedPropertyType.ObjectReference:
                valueStr = prop.objectReferenceValue.name;
                break;
            default:
                valueStr = "(not supported)";
                break;
        }

        EditorGUI.LabelField(position, label.text, valueStr);
    }

}
#endif
