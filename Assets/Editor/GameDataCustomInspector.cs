using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

[CustomEditor(typeof(GameData))]
public class GameDataCustomInspector : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        GameData gd = (GameData)target;
        
        if(GUILayout.Button("Reset data")) {
            gd.Reset();
        }
    }
}
