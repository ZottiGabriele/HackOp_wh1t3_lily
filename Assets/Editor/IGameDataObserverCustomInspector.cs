using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UnityEngine.Events;
using System;


/// <summary>
/// Custom editor drawer for the IGameDataObserver script.
/// It's job is to draw an editor popup to chose which game data condition the script should check before executing
/// </summary>
[CustomEditor(typeof(IGameDataObserver), true)]
public class IGameDataObserverCustomInspector : Editor
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

        var _hasCondition = serializedObject.FindProperty("_hasCondition");

        EditorGUILayout.PropertyField(_hasCondition);

        if (_hasCondition.boolValue)
        {

            var so = new SerializedObject(_template);
            var it = so.GetIterator();

            generatePopup(it);

            var _conditionIndex = serializedObject.FindProperty("_conditionIndex");
            var _conditionTarget = serializedObject.FindProperty("_conditionTarget");
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

            EditorGUILayout.PrefixLabel("Condition:");
            _conditionIndex.intValue = EditorGUILayout.Popup(_conditionIndex.intValue, choises_names.ToArray());
            _condition.stringValue = choises_names[_conditionIndex.intValue];
            EditorGUILayout.Separator();
            _conditionTarget.boolValue = EditorGUILayout.Toggle(_conditionTarget.boolValue, new GUILayoutOption[] { GUILayout.Width(20) });

            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void generatePopup(SerializedProperty it)
    {
        choises_names = new List<string>();
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
    }
}
