using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/CdCommand", fileName = "CdCommand")]
public class CdCommand : ICommand
{
    char[] _separator = {' '};

    public override string GetCmdDescription()
    {
        return "<b>cd <directory_path></b> : changes current directory to <directory_path>\n";
    }

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
            if(query_item.type != "directory") {
                TerminalHandler.Instance.DisplayOutput("The file " + arg + " is not a directory");
            } else {
                TerminalHandler.Instance.TerminalConfig.CurrentPath = query_item.name.TrimStart('.') + "/";
                TerminalHandler.Instance.VirtualFileSystem.ActiveEntry = query_item;
            }
        } else {
            TerminalHandler.Instance.DisplayOutput("Directory " + arg + " not found");
        }
    }
}
