using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;
using System;

public class TerminalHandler : MonoBehaviour
{
    public bool DebugMode = false;

    public static TerminalHandler Instance
    {
        get; private set;
    }

    public TerminalConfig TerminalConfig { get => _configs.Peek(); }
    public VirtualFileSystem VirtualFileSystem { get => _virtualFileSystem.Peek(); }

    public Action OnChallengeCompleted = () => { };

    [SerializeField] GameObject _terminalUI;
    [SerializeField] GameObject _lineTameplate;
    [SerializeField] GameObject _currentLine;
    [SerializeField] TerminalConfig _startingTerminalConfig;
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] List<GameObject> _hints = new List<GameObject>();

    Stack<TerminalConfig> _configs = new Stack<TerminalConfig>();
    TMP_InputField _currentInputField;
    TMP_Text _currentPrompt;
    Stack<VirtualFileSystem> _virtualFileSystem = new Stack<VirtualFileSystem>();
    bool _readingInput = false;
    int _sshCount = 0;
    Action<string> _onInputRead = _ => { };

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            // DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Debug.LogWarning("ATTENTION: " + this + " has been destroyed because of double singleton");
            Destroy(this);
        }
    }

    private void Start()
    {
        var json = Resources.Load(_startingTerminalConfig.VFSJsonPath) as TextAsset;
        _virtualFileSystem.Push(VirtualFileSystem.CreateFromJson(json.text));
        Resources.UnloadAsset(json);

        _startingTerminalConfig = ScriptableObject.Instantiate(_startingTerminalConfig); ;
        _configs.Push(ScriptableObject.Instantiate(_startingTerminalConfig));
        TerminalConfig.LoadCmdsFromPATH();

        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        BuildPrompt();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _currentInputField.text = TerminalConfig.GetPrevInHistory();
            _currentInputField.stringPosition = _currentInputField.text.Length;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _currentInputField.text = TerminalConfig.GetNextInHistory();
            _currentInputField.stringPosition = _currentInputField.text.Length;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        }
    }

    public void OnCommandInputEnd(LineHandler line)
    {
        if (!_readingInput)
        {
            TerminalConfig.AddToHistory(line.cmd);
            ParseCommand(line.cmd);
        }
        else
        {
            _onInputRead(line.cmd);
            _readingInput = false;
        }
    }

    public void InstantiateNewLine()
    {
        _currentLine = Instantiate(_lineTameplate, transform);
        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        BuildPrompt();
        StartCoroutine(scrollToBottom());
    }

    public void ParseCommand(string cmd)
    {
        bool match = false;

        //TARGETING SPECIFIC FILE
        if (cmd.StartsWith("./") || cmd.StartsWith("/") || cmd.StartsWith("../"))
        {
            var args = cmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            var t_path = (cmd.StartsWith("./")) ? TerminalConfig.CurrentPath + args[0].Substring(2) : args[0];
            var target = VirtualFileSystem.Query(t_path);
            if (target == null)
            {
                DisplayOutput("ERROR: file " + cmd + " not found.");
                return;
            }
            else if (target.type != "cmd")
            {
                DisplayOutput("ERROR: Cannot execute.");
                return;
            }
            else if (!CheckPermissions(target, "r-x"))
            {
                DisplayOutput("ERROR: Permission deined.");
                return;
            }
            else
            {
                var command = Resources.Load(target.r_path) as ICommand;
                if (command != null)
                {
                    cmd = (args.Length > 1) ? command.GetCmdName() + " " + cmd.Substring(args[0].Length) : command.GetCmdName();
                    if (command.CheckCmdMatch(cmd))
                    {
                        command.OnCmdMatch();
                        match = true;
                        if (DebugMode) Debug.Log("MATCH FOUND:\t\"" + cmd + "\"  matched  \"" + command.GetCmdMatch() + "\"");
                    }
                }
                Resources.UnloadAsset(command);
            }
        }
        else
        {
            var args = cmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var c in TerminalConfig.AvailableCommands)
            {
                if (args[0] == c.Key.name)
                {
                    var command = c.Value;
                    cmd = (args.Length > 1) ? command.GetCmdName() + " " + cmd.Substring(args[0].Length) : command.GetCmdName();
                    if (command.CheckCmdMatch(cmd))
                    {
                        command.OnCmdMatch();
                        match = true;
                        if (DebugMode) Debug.Log("MATCH FOUND:\t\"" + cmd + "\"  matched  \"" + command.GetCmdMatch() + "\"");
                    }
                    break;
                }
            }
        }

        if (!match)
        {
            DisplayOutput("Command \"" + cmd + "\" not found or has wrong arguments / options.\n\nType <b>help</b> to see the available commands.");
        }
    }

    public void ClearScreen()
    {
        foreach (Transform child in transform)
        {
            if (child.tag != "Background")
            {
                Destroy(child.gameObject);
            }
        }
        InstantiateNewLine();
    }

    public void DisplayOutput(string output)
    {
        _currentLine.GetComponent<LineHandler>().DisplayOutput(output);
        InstantiateNewLine();
    }

    public void ReadInput(string prompt, Action<string> callback)
    {
        _readingInput = true;
        InstantiateNewLine();
        _currentPrompt.color = Color.white;
        _currentPrompt.text = prompt;
        _onInputRead = callback;
    }

    public bool CheckPermissions(VirtualFileSystemEntry query_item, string flags)
    {

        bool p_user = TerminalHandler.Instance.TerminalConfig.CurrentUser == query_item.user;
        bool p_group = TerminalHandler.Instance.TerminalConfig.CurrentGroup == query_item.group;
        bool p_other = true;

        if (flags[0] == 'r')
        {
            p_user = p_user && query_item.flags[1] == 'r';
            p_group = p_group && query_item.flags[4] == 'r';
            p_other = p_other && query_item.flags[7] == 'r';
        }

        if (flags[1] == 'w')
        {
            p_user = p_user && query_item.flags[2] == 'w';
            p_group = p_group && query_item.flags[5] == 'w';
            p_other = p_other && query_item.flags[8] == 'w';
        }

        if (flags[2] == 'x')
        {
            p_user = p_user && query_item.flags[3] == 'x';
            p_group = p_group && query_item.flags[6] == 'x';
            p_other = p_other && query_item.flags[9] == 'x';
        }

        return p_user || p_group || p_other || TerminalConfig.CurrentUser == "root";
    }

    public void NewSsh(TerminalConfig tc)
    {
        _sshCount++;
        _configs.Push(ScriptableObject.Instantiate(tc));
        var json = Resources.Load(tc.VFSJsonPath) as TextAsset;
        _virtualFileSystem.Push(VirtualFileSystem.CreateFromJson(json.text));
        Resources.UnloadAsset(json);
        InstantiateNewLine();
        TerminalConfig.LoadCmdsFromPATH();
    }

    public void NewShell()
    {
        if (_configs.Count <= 5)
        {
            var config = ScriptableObject.Instantiate(_startingTerminalConfig);
            config.CurrentUser = TerminalConfig.CurrentUser;
            config.CurrentGroup = TerminalConfig.CurrentGroup;
            config.CurrentPath = TerminalConfig.CurrentPath;
            _configs.Push(config);
            InstantiateNewLine();
            TerminalConfig.LoadCmdsFromPATH();
        }
        else
        {
            DisplayOutput("Too many shells are active.");
        }
    }

    public void ExitShell()
    {
        if (_sshCount > 0)
        {
            _virtualFileSystem.Pop();
            _sshCount--;
        }
        if (_configs.Count == 1)
        {
            ScriptableObject.Destroy(_configs.Pop());
            _configs.Push(ScriptableObject.Instantiate(_startingTerminalConfig));
            TerminalConfig.LoadCmdsFromPATH();
            ClearScreen();
            _terminalUI.SetActive(false);
        }
        else
        {
            ScriptableObject.Destroy(_configs.Pop());
            InstantiateNewLine();
        }
    }

    public void BuildPrompt()
    {
        _currentPrompt = _currentLine.GetComponentInChildren<TMP_Text>();
        var user = TerminalConfig.TryGetEnvVar("$USER");
        _currentPrompt.text = user + "@" + TerminalConfig.HostName + " #";
        _currentPrompt.color = (user == "root") ? new Color(0.31f, 0.94f, 0.13f) : new Color(0.4f, 0.8078431f, 0.8392157f);
    }

    public void RevealHint()
    {
        //TODO:
    }

    public void ScrollToBottom()
    {
        StartCoroutine(scrollToBottom());
    }

    private IEnumerator scrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        _scrollRect.normalizedPosition = Vector2.zero;
    }
}
