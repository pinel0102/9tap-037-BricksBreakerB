#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    private bool useAlternative = false;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (useAlternative)
        {
            bool previousGUIState = GUI.enabled;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = previousGUIState;
        }
        else
        {
            string valueStr;

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    valueStr = property.stringValue;
                    break;
                case SerializedPropertyType.Integer:
                    valueStr = property.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    valueStr = property.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    valueStr = property.floatValue.ToString("0.0000");
                    break;
                case SerializedPropertyType.Enum:
                    valueStr = property.enumDisplayNames[property.enumValueIndex];
                    break;
                case SerializedPropertyType.Rect:
                    valueStr = property.rectIntValue.ToString();
                    break;
                case SerializedPropertyType.Vector2Int:
                    valueStr = property.vector2IntValue.ToString();
                    break;
                case SerializedPropertyType.Vector3Int:
                    valueStr = property.vector3IntValue.ToString();
                    break;
                case SerializedPropertyType.Vector2:
                    valueStr = property.vector2Value.ToString();
                    break;            
                case SerializedPropertyType.Vector3:
                    valueStr = property.vector3Value.ToString();
                    break;
                case SerializedPropertyType.Vector4:
                    valueStr = property.vector4Value.ToString();
                    break;
                default:
                    //valueStr = string.Format("propertyType : {0}", property.propertyType);
                    bool previousGUIState = GUI.enabled;
                    GUI.enabled = false;
                    EditorGUI.PropertyField(position, property, label);
                    GUI.enabled = previousGUIState;
                    return;
            }

            EditorGUI.LabelField(position, label.text, valueStr);
        }
    }
}

public class ReadOnlyAttribute : PropertyAttribute { }

#endif