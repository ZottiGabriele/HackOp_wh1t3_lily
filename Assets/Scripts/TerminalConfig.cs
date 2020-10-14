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
    public string JsonRelativePath {get => _jsonRelativePath; set => _jsonRelativePath = value;}
    public string PATH = "";

    [SerializeField] string _currentPath = "/home/user/";
    [SerializeField] string _homePath = "/home/user/";
    [SerializeField] string _jsonRelativePath = "";

    public void Initialize() {
        _currentPath = _homePath;
    }

}
