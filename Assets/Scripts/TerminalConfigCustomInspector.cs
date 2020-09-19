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

        if(GUILayout.Button("Reinitialize virtual filesystem")) {
            
            Directory.Delete(Application.persistentDataPath, true);
            Directory.CreateDirectory(Application.persistentDataPath);
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "challange_1"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "challange_2"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "challange_3"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "challange_4"));
            Process.Start(@Application.persistentDataPath);
        }

#else
        UnityEngine.Debug.LogWarning("Some TerminalConfig Inspector functions are available only for Windows Unity Editor");
#endif
    }
}
