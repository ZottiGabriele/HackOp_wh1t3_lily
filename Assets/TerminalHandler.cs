using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;

public class TerminalHandler : MonoBehaviour
{
    public static TerminalHandler Instance {
        get; private set;
    }

    // [SerializeField] public LineCmdCollection;

    void Awake()
    {
        if(!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    [SerializeField] GameObject _lineTameplate;

    public void OnCommandInputEnd(LineHandler line) {
        // parseCommand(line.cmd, line.args);
        parseCommand(line.cmd);
        instantiateNewLine();
    }

    private void instantiateNewLine() {
        var new_line = Instantiate(_lineTameplate, transform).GetComponentInChildren<TMP_InputField>();
        EventSystem.current.SetSelectedGameObject(new_line.gameObject);
    }

    private void parseCommand(string cmd) {

        Match m = Regex.Match(cmd, "^ *clear$");

        if(m.Success) {
            foreach (Transform child in transform)
            {
                if(child.tag != "Background") {
                    Destroy(child.gameObject);
                }
            }
        } else {
            Debug.LogWarning("No cmd match for \"" + cmd + "\"");
        }

        // switch (cmd)
        // {
        //     case "clear":
        //         foreach (Transform child in transform)
        //         {
        //             if(child.tag != "Background") {
        //                 Destroy(child.gameObject);
        //             }
        //         }
        //         break;
        //     default: Debug.LogWarning("No cmd match for \"" + cmd + "\""); break;
        // }
    }
}
