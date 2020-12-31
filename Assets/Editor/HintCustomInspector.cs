using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Hint), true)]
public class HintCustomInspector : Editor
{
    List<string> choises_names;
    GameData _template;

    private void OnEnable()
    {
        _template = FindObjectOfType<GameData>();
        if (_template == null) _template = ScriptableObject.CreateInstance<GameData>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space(15);

        var so = new SerializedObject(_template);
        var it = so.GetIterator();

        generatePopup(it);

        var _conditionIndex = serializedObject.FindProperty("_conditionIndex");
        var _condition = serializedObject.FindProperty("_condition");

        //Condition was prevoiusly set, fixes wrong indexing when adding new data to GameData class
        if (_condition.stringValue != "")
        {
            var prev_choice = choises_names.IndexOf(_condition.stringValue);
            if (prev_choice != -1)
            {
                _conditionIndex.intValue = prev_choice;
            }
        }

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel("Unlock Condition:");
        _conditionIndex.intValue = EditorGUILayout.Popup(_conditionIndex.intValue, choises_names.ToArray());
        _condition.stringValue = choises_names[_conditionIndex.intValue];

        EditorGUILayout.EndHorizontal();

        bool isInstance = PrefabUtility.IsPartOfPrefabInstance(serializedObject.targetObject);
        if (isInstance) EditorUtility.SetDirty(target);

        serializedObject.ApplyModifiedProperties();
    }

    private void generatePopup(SerializedProperty it)
    {
        choises_names = new List<string>();
        choises_names.Add("000");
        while (it.Next(true))
        {
            if (it.propertyType == SerializedPropertyType.Boolean)
            {
                if (it.name != "m_Enabled")
                {
                    choises_names.Add(it.name);
                }
            }
        }
        choises_names.Sort();
        choises_names[0] = "None";
    }
}
