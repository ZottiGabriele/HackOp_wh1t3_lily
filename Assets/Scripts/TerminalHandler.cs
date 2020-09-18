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
    [SerializeField] CommandCollection _commandCollection;

    public void OnCommandInputEnd(LineHandler line) {
        parseCommand(line.cmd);
        instantiateNewLine();
    }

    private void instantiateNewLine() {
        var new_line = Instantiate(_lineTameplate, transform).GetComponentInChildren<TMP_InputField>();
        EventSystem.current.SetSelectedGameObject(new_line.gameObject);
    }

    private void parseCommand(string cmd) {
        foreach (var command in _commandCollection.AvailableCommands)
        {
            if(command.CheckCmdMatch(cmd)) {
                command.OnCmdMatch();
                Debug.Log("MATCH FOUND:\t\"" + cmd + "\"  matched  \"" + command.GetCmdMatch() + "\"");
                break;
            }
        }
    }

    public void ClearScreen() {
        foreach (Transform child in transform)
        {
            if(child.tag != "Background") {
                Destroy(child.gameObject);
            }
        }
    }
}
