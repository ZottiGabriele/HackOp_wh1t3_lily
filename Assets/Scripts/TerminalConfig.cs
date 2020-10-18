using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/TerminalConfig", fileName = "NewTerminalConfig")]
public class TerminalConfig : ScriptableObject
{
    public List<ICommand> AvailableCommands = new List<ICommand>();
    public string CurrentPath {get => _currentPath; set => _currentPath = value;}
    public string HomePath {get => _homePath;}
    public string VFSJsonPath {get => "VFS\\vfs_ch_" + ((int)_challenge + 1);}
    public Challenge CurrentChallenge {get => _challenge;}
    public string PATH = "";

    [SerializeField] string _currentPath = "/home/user/";
    [SerializeField] string _homePath = "/home/user/";
    [SerializeField] public Challenge _challenge;    

    public enum Challenge {
        first,
        second,
        third,
        fourth
    }
}
