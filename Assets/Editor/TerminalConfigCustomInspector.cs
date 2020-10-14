using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

[CustomEditor(typeof(TerminalConfig))]
public class TerminalConfigCustomInspector : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();

        TerminalConfig tc = target as TerminalConfig;

        GUILayout.Space(25);

#if UNITY_EDITOR_WIN
        if(GUILayout.Button("Select VFS JSON")) {
            string prePath = Application.dataPath + "/Resources/";
            string path = EditorUtility.OpenFilePanel("Select VFS JSON", prePath, "json");
            if(path.Length != 0) {
                tc.JsonRelativePath = path.Remove(0, prePath.Length).Split('.')[0];
            }
        }
#else
        UnityEngine.Debug.LogWarning("Some TerminalConfig Inspector functions are available only for Windows Unity Editor");
#endif
    }
}
