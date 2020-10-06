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

        TerminalConfig tc = (TerminalConfig)target;

#if UNITY_EDITOR_WIN
        
        if(GUILayout.Button("Open persistent data path")) {
            Process.Start(@Application.persistentDataPath);
        }

        if(GUILayout.Button("Create filesystem JSON")) {
            //TODO: implement correctly
            Process.Start("C:\\Windows\\System32\\bash.exe");
        }

#else
        UnityEngine.Debug.LogWarning("Some TerminalConfig Inspector functions are available only for Windows Unity Editor");
#endif
    }
}
