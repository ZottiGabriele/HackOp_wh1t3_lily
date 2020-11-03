using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;
using UnityEngine.Events;
using System;

[CustomEditor(typeof(IInteractionArea),true)]
public class IInteractionAreaCustomInspector : Editor
{
    List<string> choises_names;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        IInteractionArea tc = target as IInteractionArea;

        EditorGUILayout.Space(15);

        var _hasCondition = serializedObject.FindProperty("_hasCondition");

        EditorGUILayout.PropertyField(_hasCondition);

        if(_hasCondition.boolValue) {

            var gameDataPrototype = new SerializedObject(new GameData());

            var it = gameDataPrototype.GetIterator();

            generatePopup(it);
            
            var _conditionIndex = serializedObject.FindProperty("_conditionIndex");
            var _conditionTarget = serializedObject.FindProperty("_conditionTarget");
            var _condition = serializedObject.FindProperty("_condition");

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("Condition:");
            _conditionIndex.intValue = EditorGUILayout.Popup(_conditionIndex.intValue, choises_names.ToArray());
            _condition.stringValue = choises_names[_conditionIndex.intValue];
            EditorGUILayout.Separator();
            _conditionTarget.boolValue = EditorGUILayout.Toggle(_conditionTarget.boolValue, new GUILayoutOption[] {GUILayout.Width(20)});

            EditorGUILayout.EndHorizontal();

            EditorUtility.SetDirty(target);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void generatePopup(SerializedProperty it) {
        // choises = new List<SerializedProperty>();
        choises_names = new List<string>();
        while(it.Next(true)) {
            if(it.propertyType == SerializedPropertyType.Boolean) {
                if(it.name != "m_Enabled") {
                    // choises.Add(it.Copy());
                    choises_names.Add(it.name);
                }
            }
        }
    }
}
