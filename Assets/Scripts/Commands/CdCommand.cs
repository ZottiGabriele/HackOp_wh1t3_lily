using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/CdCommand", fileName = "CdCommand")]
public class CdCommand : ICommand
{
    public override string GetCmdName() => "cd";
    public override string GetCmdDescription() => "<b>cd <directory_path></b> : changes current directory to <directory_path>";
    public override string GetCmdMatch() => "^ *cd *$|^ *cd +\\S* *$";
    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (cmd.Length != 2)
        {
            var temp = new List<string>(cmd);
            temp.Add(TerminalHandler.Instance.TerminalConfig.HomePath);
            cmd = temp.ToArray();
        }

        string arg = cmd[1];
        if (arg != "/") arg.TrimEnd('/');
        var query_item = TerminalHandler.Instance.VirtualFileSystem.Query(arg);
        if (query_item != null)
        {
            if (query_item.type != "directory")
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: The file " + arg + " is not a directory");
            }
            else if (!TerminalHandler.Instance.CheckPermissions(query_item, "r-x"))
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
            }
            else
            {
                string currentPath = (query_item.full_path == "/") ? "/" : query_item.full_path + "/";
                TerminalHandler.Instance.TerminalConfig.CurrentPath = currentPath;
                TerminalHandler.Instance.InstantiateNewLine();
            }
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: Directory " + arg + " not found");
        }
    }
}
