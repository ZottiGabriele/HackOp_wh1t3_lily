﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


[CustomEditor(typeof(ChangeSceneInteractionArea))]
public class ChangeSceneInteractionAreaCustomEditor : IGameDataObserverCustomInspector
{
    List<string> _scenes = new List<string>();

    public override void OnInspectorGUI()
    {
        ChangeSceneInteractionArea _t = (ChangeSceneInteractionArea) target;
        SerializedProperty _sceneToLoad = serializedObject.FindProperty("_sceneToLoad");

        buildScenesList();

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.PrefixLabel("Scene to load:");
        _sceneToLoad.intValue = EditorGUILayout.Popup(_sceneToLoad.intValue, _scenes.ToArray());

        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);

        base.OnInspectorGUI();
    }

    private void buildScenesList() {
        _scenes = new List<string>();
        
        for(int i = 0; i < SceneManager.sceneCountInBuildSettings; i++) {
            var path = SceneUtility.GetScenePathByBuildIndex(i);
            _scenes.Add(Path.GetFileNameWithoutExtension(path));
        }
    }
}
