using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;

public class TerminalHandler : MonoBehaviour
{
    public bool DebugMode = false;

    public static TerminalHandler Instance {
        get; private set;
    }

    public TerminalConfig TerminalConfig {get {
        return _terminalConfig;
    }}

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
    [SerializeField] GameObject _lastLine;
    [SerializeField] TerminalConfig _terminalConfig;
    

    public void OnCommandInputEnd(LineHandler line) {
        parseCommand(line.cmd);
        instantiateNewLine();
    }

    private void instantiateNewLine() {
        var new_line = Instantiate(_lineTameplate, transform);
        var inputField = new_line.GetComponentInChildren<TMP_InputField>();
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        _lastLine = new_line.gameObject;
    }

    private void parseCommand(string cmd) {
        bool match = false;
        foreach (var command in _terminalConfig.AvailableCommands)
        {
            if(command.CheckCmdMatch(cmd)) {
                command.OnCmdMatch();
                match = true;
                if(DebugMode) Debug.Log("MATCH FOUND:\t\"" + cmd + "\"  matched  \"" + command.GetCmdMatch() + "\"");
                break;
            }
        }
        if(!match) DisplayOutput("Command \"" + cmd + "\" not found");
    }

    public void ClearScreen() {
        foreach (Transform child in transform)
        {
            if(child.tag != "Background") {
                Destroy(child.gameObject);
            }
        }
    }

    public void DisplayOutput(string output) {
        _lastLine.GetComponent<LineHandler>().DisplayOutput(output);
    }
}
