using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;


/// <summary>
/// Custom editor that adds commonly needed functionality through the addition of buttons
/// under the GameStateHandler editor inspector.
/// </summary>

[CustomEditor(typeof(GameStateHandler))]
public class GameStateHandlerCustomInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(25);

        if (GUILayout.Button("Open Save Folder"))
        {
            EditorUtility.RevealInFinder((target as GameStateHandler).SAVE_PATH);
        }

        if (GUILayout.Button("Delete Save Data"))
        {
            if (FileUtil.DeleteFileOrDirectory((target as GameStateHandler).SAVE_PATH))
            {
                Debug.Log("Save file deleted");
            }
            else
            {
                Debug.Log("Save file already deleted");
            }
        }
    }
}
