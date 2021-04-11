using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public string TryGetEnvVar(string var)
    {
        string val = "";
        ENV.TryGetValue(var.Substring(1), out val);
        return val;
    }

    public void SetEnvVar(string var, string value)
    {
        var = var.Substring(1);
        if (ENV.ContainsKey(var))
        {
            ENV.Remove(var);
        }
        ENV.Add(var, value);
    }

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

    public string GetPrevInHistory()
    {
        if (_history.Count == 0) return "";
        _historyCounter--;
        if (_historyCounter < 0) _historyCounter++;
        return _history[_historyCounter];
    }

    public string GetNextInHistory()
    {
        if (_history.Count == 0) return "";
        _historyCounter++;
        if (_historyCounter == _history.Count) _historyCounter--;
        return _history[_historyCounter];
    }

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
