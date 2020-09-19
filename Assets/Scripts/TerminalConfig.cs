using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/TerminalConfig", fileName = "NewTerminalConfig")]
public class TerminalConfig : ScriptableObject
{
    public List<ICommand> AvailableCommands = new List<ICommand>();
    public string StartingPath {get {return Application.persistentDataPath + Path.DirectorySeparatorChar + _startingPath;}}
    public string CurrentPath {get {return Application.persistentDataPath + Path.DirectorySeparatorChar + _currentPath;}}
    public string HomePath {get {return Application.persistentDataPath + Path.DirectorySeparatorChar + _homePath;}}
    public string PATH = "";

    [SerializeField] string _startingPath = "";
    [SerializeField] string _currentPath = "";
    [SerializeField] string _homePath = "";
}
