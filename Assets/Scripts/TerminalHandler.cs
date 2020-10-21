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
        var json = Resources.Load(TerminalConfig.VFSJsonPath) as TextAsset;

        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        _virtualFileSystem = VirtualFileSystem.CreateFromJson(json.text);
        _terminalConfig = ScriptableObject.Instantiate(_terminalConfig);

        Resources.UnloadAsset(json);
    }

    private void LateUpdate() {
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
        if(!match) DisplayOutput("Command \"" + cmd + "\" not found or has wrong arguments / options.\n\nType <b>help</b> to see the available commands.");
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

    public bool CheckPermissions(VirtualFileSystemEntry query_item, string flags) {

        bool p_user = TerminalHandler.Instance.TerminalConfig.CurrentUser == query_item.user;
        bool p_group = TerminalHandler.Instance.TerminalConfig.CurrentGroup == query_item.group;
        bool p_other = query_item.flags[7] == 'r' && query_item.flags[9] == 'x';

        if(flags[0] == 'r') {
            p_user = p_user && query_item.flags[1] == 'r';
            p_group = p_group && query_item.flags[4] == 'r';
        }

        if(flags[1] == 'w') {
            p_user = p_user && query_item.flags[2] == 'w';
            p_group = p_group && query_item.flags[5] == 'w';
        }

        if(flags[2] == 'x') {
            p_user = p_user && query_item.flags[3] == 'x';
            p_group = p_group && query_item.flags[6] == 'x';
        }

        return p_user || p_group || p_other || TerminalConfig.CurrentUser == "root";
    }

    private IEnumerator scrollToBottom() {
        yield return new WaitForEndOfFrame();
        _scrollRect.normalizedPosition = Vector2.zero;
    }
}
