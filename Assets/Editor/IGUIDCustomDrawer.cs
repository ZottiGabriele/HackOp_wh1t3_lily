using UnityEngine;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(UniqueIdentifierAttribute))]
public class IGUIDCustomDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.stringValue != "")
        {
            var textPos = new Rect(position.x, position.y, position.width - 50, position.height);
            var btnPos = new Rect(position.width - 50, position.y, 50, position.height);

            if (GUI.Button(btnPos, "Reset"))
            {
                property.stringValue = "";
            }

            EditorGUI.LabelField(textPos, label, new GUIContent(property.stringValue));
            return;
        }

        Rect _position = position;
        _position.height = 16;

        var target = property.serializedObject.targetObject as MonoBehaviour;
        bool isInstance = PrefabUtility.IsPartOfPrefabInstance(target.gameObject);
        bool isPrefab = PrefabUtility.IsPartOfAnyPrefab(target.gameObject);
        if (!isInstance && isPrefab)
        {
            property.stringValue = "";
            EditorGUI.LabelField(_position, label, new GUIContent("NO GUID FOR PREFAB"));
            return;
        }

        if (property.stringValue == "")
        {
            Guid guid = Guid.NewGuid();
            property.stringValue = guid.ToString();
        }

        EditorGUI.LabelField(position, label, new GUIContent(property.stringValue));
    }
}
