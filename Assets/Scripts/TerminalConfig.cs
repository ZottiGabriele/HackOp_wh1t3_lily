using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// ScriptableObject containing all the configuration data needed for the emulated terminal.
/// </summary>
[CreateAssetMenu(menuName = "Custom/TerminalConfig", fileName = "NewTerminalConfig")]
public class TerminalConfig : ScriptableObject
{
    public string HostName = "";
    public Dictionary<VirtualFileSystemEntry, ICommand> AvailableCommands = new Dictionary<VirtualFileSystemEntry, ICommand>();
    public string CurrentPath { get => TryGetEnvVar("$PWD"); set => SetEnvVar("$PWD", value); }
    public string HomePath { get => TryGetEnvVar("$HOME"); set => SetEnvVar("$HOME", value); }
    public string CurrentUser { get => TryGetEnvVar("$USER"); set => SetEnvVar("$USER", value); }
    public string CurrentGroup { get => TryGetEnvVar("$GROUP"); set => SetEnvVar("$GROUP", value); }
    public string VFSJsonPath { get => "vfs_ch_" + (int)_challenge; }
    public Challenge CurrentChallenge { get => _challenge; }

    [SerializeField] Challenge _challenge;

    [SerializeField]
    ENV_VAR[] _env = new ENV_VAR[]{
        new ENV_VAR("PATH", "/bin:/usr/bin:/usr/local/bin"),
        new ENV_VAR("USER", "user"),
        new ENV_VAR("GROUP", "group"),
        new ENV_VAR("HOME", "/home/user/"),
        new ENV_VAR("PWD", "/home/user/")
    };

    Dictionary<string, string> ENV = new Dictionary<string, string>();
    private List<string> _history = new List<string>();
    private int _historyCounter = -1;

    private void OnEnable()
    {
        ENV = new Dictionary<string, string>();
        foreach (var _var in _env)
        {
            ENV.Add(_var.name, _var.value);
        }
    }

    /// <summary>
    /// Returns the value of the requested envinroment variable. If the variable is not present in the envinroment an empty string is returned.
    /// </summary>
    /// <param name="var">The name of the variable. IT MUST START WITH THE "$" SIGN.</param>
    /// <returns></returns>
    public string TryGetEnvVar(string var)
    {
        string val = "";
        ENV.TryGetValue(var.Substring(1), out val);
        return val;
    }

    /// <summary>
    /// Sets a new value to the specified envinroment variable.
    /// </summary>
    /// <param name="var">The name of the variable. IT MUST START WITH THE "$" SIGN.</param>
    /// <param name="value">The value of the variable.</param>
    public void SetEnvVar(string var, string value)
    {
        var = var.Substring(1);
        if (ENV.ContainsKey(var))
        {
            ENV.Remove(var);
        }
        ENV.Add(var, value);
    }

    /// <summary>
    /// Loads the available commands based on the current value of the $PATH envinroment variable.
    /// </summary>
    public void LoadCmdsFromPATH()
    {
        AvailableCommands = new Dictionary<VirtualFileSystemEntry, ICommand>();
        var entires = TerminalHandler.Instance.VirtualFileSystem.AvailableCommands;

        foreach (var e in entires)
        {
            var cmd = Resources.Load(e.r_path) as ICommand;
            AvailableCommands.Add(e, cmd);
        }
    }

    /// <summary>
    /// Resets the emulated envinroment.
    /// </summary>
    public void ResetENV()
    {
        _env = new ENV_VAR[]{
            new ENV_VAR("PATH", "/bin:/usr/bin:/usr/local/bin"),
            new ENV_VAR("USER", "user"),
            new ENV_VAR("GROUP", "group"),
            new ENV_VAR("HOME", "/home/user/"),
            new ENV_VAR("PWD", "/home/user/")
        };
        OnEnable();
    }

    /// <summary>
    /// Returns the string containing the previous command in the emulated terminal history.
    /// </summary>
    /// <returns></returns>
    public string GetPrevInHistory()
    {
        if (_history.Count == 0) return "";
        _historyCounter--;
        if (_historyCounter < 0) _historyCounter++;
        return _history[_historyCounter];
    }

    /// <summary>
    /// Returns the string containing the next command in the emulated terminal history.
    /// </summary>
    /// <returns></returns>
    public string GetNextInHistory()
    {
        if (_history.Count == 0) return "";
        _historyCounter++;
        if (_historyCounter == _history.Count) _historyCounter--;
        return _history[_historyCounter];
    }

    /// <summary>
    /// Adds the command to the history.
    /// </summary>
    /// <param name="cmd">The command to add to the history.</param>
    public void AddToHistory(string cmd)
    {
        _history.Add(cmd);
        _historyCounter = _history.Count;
    }

    public enum Challenge
    {
        zero,
        first,
        second,
        third,
        fourth
    }

    [Serializable]
    public struct ENV_VAR
    {
        public string name;
        public string value;

        public ENV_VAR(string name, string value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
