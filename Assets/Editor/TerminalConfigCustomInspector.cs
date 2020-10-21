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

        if(GUILayout.Button("Reset ENV")) {
            tc.ResetENV();
        };

#if UNITY_EDITOR_WIN
        if(GUILayout.Button("Generate VFS jsons")) {
            string cmd = "C:\\Users\\Zotti\\Documents\\0_UNITY\\HackOp_wh1t3_lily\\Helper\\VFSGenerator.py";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\Zotti\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
            start.Arguments = string.Format("{0}", cmd);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process.Start(start);

            UnityEngine.Debug.Log("VFS file generated");
        }
#else
        UnityEngine.Debug.LogWarning("Some TerminalConfig Inspector functions are available only for Windows Unity Editor");
#endif
    }
}

public class TerminalConfigMenuItem {
    [MenuItem("TerminalConfig/Generate VFS jsons")]
    private static void NewMenuOption() {
        string cmd = "C:\\Users\\Zotti\\Documents\\0_UNITY\\HackOp_wh1t3_lily\\Helper\\VFSGenerator.py";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "C:\\Users\\Zotti\\AppData\\Local\\Programs\\Python\\Python39\\python.exe";
            start.Arguments = string.Format("{0}", cmd);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            Process.Start(start);

            UnityEngine.Debug.Log("VFS file generated");
    }
}
