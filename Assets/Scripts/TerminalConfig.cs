using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/TerminalConfig", fileName = "NewTerminalConfig")]
public class TerminalConfig : ScriptableObject
{
    public List<ICommand> AvailableCommands = new List<ICommand>();
    public string CurrentPath {get => TryGetEnvVar("$PWD"); set => SetEnvVar("$PWD", value);}
    public string HomePath {get => TryGetEnvVar("$HOME");}
    public string CurrentUser {get => TryGetEnvVar("$USER");}
    public string CurrentGroup {get => TryGetEnvVar("$GROUP");}
    public string VFSJsonPath {get => "VFS\\vfs_ch_" + ((int)_challenge + 1);}
    public Challenge CurrentChallenge {get => _challenge;}
    
    [SerializeField] public Challenge _challenge;
    [SerializeField] ENV_VAR[] _env = new ENV_VAR[]{
        new ENV_VAR("PATH", "/usr/bin:/usr/local/bin"),
        new ENV_VAR("USER", "user"),
        new ENV_VAR("GROUP", "group"),
        new ENV_VAR("HOME", "/home/user/"),
        new ENV_VAR("PWD", "/home/user/")    
    };
    Dictionary<string,string> ENV = new Dictionary<string, string>(); 

    private void OnEnable() {
        ENV = new Dictionary<string, string>();
        foreach (var _var in _env)
        {
            ENV.Add(_var.name, _var.value);
        }
    }

    public string TryGetEnvVar(string var) {
        string val = "";
        ENV.TryGetValue(var.Substring(1), out val);
        return val;
    }

    public void SetEnvVar(string var, string value) {
        var = var.Substring(1);
        if(ENV.ContainsKey(var)) {
            ENV.Remove(var);
        }
        ENV.Add(var, value);
    }

    public void ResetENV() {
        _env = new ENV_VAR[]{
            new ENV_VAR("PATH", "/usr/bin:/usr/local/bin"),
            new ENV_VAR("USER", "user"),
            new ENV_VAR("GROUP", "group"),
            new ENV_VAR("HOME", "/home/user/"),
            new ENV_VAR("PWD", "/home/user/")    
        };
        OnEnable();
    }

    public enum Challenge {
        first,
        second,
        third,
        fourth
    }

    [Serializable]
    public struct ENV_VAR {
        public string name;
        public string value;

        public ENV_VAR(string name, string value) {
            this.name = name;
            this.value = value;
        }
    }
}
