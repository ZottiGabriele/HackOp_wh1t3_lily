using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;

public class TerminalHandler : MonoBehaviour
{
    public bool DebugMode = false;

    public static TerminalHandler Instance {
        get; private set;
    }

    public TerminalConfig TerminalConfig {get => _terminalConfig;}
    public VirtualFileSystem VirtualFileSystem {get => _virtualFileSystem;}

    [SerializeField] GameObject _lineTameplate;
    [SerializeField] GameObject _currentLine;
    [SerializeField] TerminalConfig _terminalConfig;
    [SerializeField] ScrollRect _scrollRect;
    TMP_InputField _currentInputField;
    VirtualFileSystem _virtualFileSystem;

    private void Awake()
    {
        if(!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    private void Start() {
        string json = File.ReadAllText(Path.Combine(Application.persistentDataPath, "tree.json"));

        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        _virtualFileSystem = VirtualFileSystem.CreateFromJSON(json);
    }

    private void Update() {
        if(EventSystem.current.currentSelectedGameObject != _currentInputField) {
            EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        }
    }

    public void OnCommandInputEnd(LineHandler line) {
        parseCommand(line.cmd);
        instantiateNewLine();
    }

    private void instantiateNewLine() {
        _currentLine = Instantiate(_lineTameplate, transform);
        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        StartCoroutine(scrollToBottom());
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
        _currentLine.GetComponent<LineHandler>().DisplayOutput(output);
    }

    private IEnumerator scrollToBottom() {
        yield return new WaitForEndOfFrame();
        _scrollRect.normalizedPosition = Vector2.zero;
    }
}
