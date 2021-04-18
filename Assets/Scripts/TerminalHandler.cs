using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.IO;
using System;


/// <summary>
/// Script that handles all interactions with the ingame emulated terminal.
/// </summary>
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
    public Action<string> OnInputProcessed = (string _) => { };

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
        if (!_readingInput)
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
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        }
    }

    /// <summary>
    /// Method that starts the parsing of a LineHandler text.
    /// </summary>
    /// <param name="line">LineHandler containing the text to parse.</param>
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

        OnInputProcessed(line.cmd);
    }

    /// <summary>
    /// Creates a new line on the emulated terminal.
    /// </summary>
    public void InstantiateNewLine()
    {
        _currentLine = Instantiate(_lineTameplate, transform);
        _currentInputField = _currentLine.GetComponent<LineHandler>().InField;
        EventSystem.current.SetSelectedGameObject(_currentInputField.gameObject);
        BuildPrompt();
        StartCoroutine(scrollToBottom());
    }

    /// <summary>
    /// Method that emulates the parsing of the given text as a command of the emulated terminal.
    /// </summary>
    /// <param name="cmd">The text to parse.</param>
    public void ParseCommand(string cmd)
    {
        bool match = false;

        //Exit always available
        Match m = Regex.Match(cmd, "^ *exit *$");
        if (m.Success)
        {
            ExitShell();
            return;
        }

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
                DisplayOutput("ERROR: Cannot be executed.");
                return;
            }
            else if (!CheckPermissions(target, "r-x"))
            {
                DisplayOutput("ERROR: Permission denied.");
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

    /// <summary>
    /// Clears the emulated terminal's screen and creates a new empty line.
    /// </summary>
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

    /// <summary>
    /// Displays the given output on the emulated terminal screen and creates a new empty line.
    /// </summary>
    /// <param name="output">The output to display.</param>
    public void DisplayOutput(string output)
    {
        _currentLine.GetComponent<LineHandler>().DisplayOutput(output);
        InstantiateNewLine();
    }

    /// <summary>
    /// Creates an input field and passes the inputted text to the given callback function.
    /// </summary>
    /// <param name="prompt">Text to display before the input field.</param>
    /// <param name="callback">Callback function that will receive the inputted text.</param>
    public void ReadInput(string prompt, Action<string> callback)
    {
        _readingInput = true;
        InstantiateNewLine();
        _currentPrompt.color = Color.white;
        _currentPrompt.text = prompt;
        _onInputRead = callback;
    }

    /// <summary>
    /// <para>Checks the permissions on a VirtualFileSystemEntry against the given flags. This will automatically handle the check for the "user", the "group" and the "others" flags based on the current terminal configuration.</para>
    /// 
    /// NOTE: The check will return True only if ALL OF THE PERMISSIONS asked are verified.
    /// </summary>
    /// <param name="query_item">The entry that requires the permissions check.</param>
    /// <param name="flags"><para>Use the following rules to check for permissions: "r--" will check for read, "-w-" will check for write, "--x" will check for execute. </para>
    /// All combinations are possible to check multiple permissions at once (example: "rw-" will check for read AND write permissions).</param>
    /// <returns></returns>
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

    /// <summary>
    /// Emulated the creation of a SSH connection to another terminal.
    /// </summary>
    /// <param name="tc">The TerminalConfig with the configuration of the SSH terminal.</param>
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

    /// <summary>
    /// Emulates the creation of a new shell. Note that there is a limit to the number of cuncurrent shells active.
    /// </summary>
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


    /// <summary>
    /// Emulates the killing of the current shell process. If there is only one shell left, it will close the ingame terminal UI as well.
    /// </summary>
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

    /// <summary>
    /// Completely resets the terminal by closing all shells and ssh
    /// </summary>
    public void ResetTerminal()
    {
        while (_sshCount > 0 || _configs.Count > 1)
        {
            ExitShell();
        }

        ExitShell();
    }

    /// <summary>
    /// Builds the prompt of the emulated terminal based on the current user and hostname.
    /// </summary>
    public void BuildPrompt()
    {
        _currentPrompt = _currentLine.GetComponentInChildren<TMP_Text>();
        var user = TerminalConfig.TryGetEnvVar("$USER");
        _currentPrompt.text = user + "@" + TerminalConfig.HostName + " #";
        _currentPrompt.color = (user == "root") ? new Color(0.31f, 0.94f, 0.13f) : new Color(0.4f, 0.8078431f, 0.8392157f);
    }

    /// <summary>
    /// Forces the emulated terminal to scroll to the last line.
    /// </summary>
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
