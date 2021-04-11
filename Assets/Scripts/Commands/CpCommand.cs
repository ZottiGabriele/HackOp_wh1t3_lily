using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/CpCommand", fileName = "CpCommand")]
public class CpCommand : ICommand
{
    public override string GetCmdName() => "cp";
    public override string GetCmdDescription() => "<b>cp <source> <destination></b> : copies <source> to <destination>";
    public override string GetCmdMatch() => "^ *cp +\\S+ +\\S+ *$";
    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        if ((cmd[1]) == "/")
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: Can't copy root folder");
            return;
        }

        var source = TerminalHandler.Instance.VirtualFileSystem.Query(cmd[1]);

        if (source == null)
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: source file not found");
        }
        else if (source.type == "directory")
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: copying folders is not allowed.");
        }
        else
        {
            string destination_path = TerminalHandler.Instance.VirtualFileSystem.GetFinalPath(cmd[2]);
            int last_sep = destination_path.LastIndexOf('/');
            string parent_path = (last_sep == 0) ? "/" : destination_path.Remove(last_sep);
            var parent = TerminalHandler.Instance.VirtualFileSystem.Query(parent_path);
            var destination = TerminalHandler.Instance.VirtualFileSystem.Query(destination_path);

            if (destination != null && destination.type == "directory")
            {
                destination_path += "/" + source.name;
                parent = destination;
            }

            if (parent == null)
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Destination folder \"" + parent_path + "\" not found.");
                return;
            }

            if (!TerminalHandler.Instance.CheckPermissions(parent, "-w-") || !TerminalHandler.Instance.CheckPermissions(source, "r--"))
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied.");
                return;
            }

            var copy = TerminalHandler.Instance.VirtualFileSystem.CopyEntry(source, destination_path);
            //fixOwnership(copy);
            TerminalHandler.Instance.TerminalConfig.LoadCmdsFromPATH();
            TerminalHandler.Instance.InstantiateNewLine();
        }
    }

    private void fixOwnership(VirtualFileSystemEntry entry)
    {
        entry.user = TerminalHandler.Instance.TerminalConfig.CurrentUser;
        entry.group = TerminalHandler.Instance.TerminalConfig.CurrentGroup;

        if (entry.childs == null) return;
        foreach (var c in entry.childs)
        {
            fixOwnership(c);
        }
    }
}
