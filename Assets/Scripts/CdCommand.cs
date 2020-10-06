using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/CdCommand", fileName = "CdCommand")]
public class CdCommand : ICommand
{
    char[] _separator = {' '};

    public override string GetCmdMatch()
    {
        return "^ *cd *[\\w\\d\\.\\/]* *$";
    }

    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(_separator, System.StringSplitOptions.RemoveEmptyEntries);

        if(cmd.Length != 2) {
            TerminalHandler.Instance.TerminalConfig.CurrentPath = TerminalHandler.Instance.TerminalConfig.HomePath;
            TerminalHandler.Instance.VirtualFileSystem.ActiveEntry = TerminalHandler.Instance.VirtualFileSystem.HomeEntry;
            return;
        }

        string arg = cmd[1];
        if(arg != "/") arg.TrimEnd('/');
        var query_item = TerminalHandler.Instance.VirtualFileSystem.Query(arg);
        if(query_item != null) {
            Debug.Log("The file exists!");
            if(query_item.type != "directory") {
                Debug.Log("Not a directory m8");
            } else {
                Debug.Log("YAY A DIRECTORY");
                TerminalHandler.Instance.TerminalConfig.CurrentPath = query_item.name.TrimStart('.') + "/";
                TerminalHandler.Instance.VirtualFileSystem.ActiveEntry = query_item;
            }
        } else {
            Debug.Log("The file does not exists!");
        }
    }
}
