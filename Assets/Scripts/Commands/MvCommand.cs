using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/MvCommand", fileName = "MvCommand")]
public class MvCommand : ICommand
{
    public override string GetCmdName() => "mv";
    public override string GetCmdDescription() => "<b>mv <source> <destination></b> : moves <source> to <destination>";
    public override string GetCmdMatch() => "^ *mv +\\S+ +\\S+ *$";
    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        var source = TerminalHandler.Instance.VirtualFileSystem.Query(cmd[1]);
        int last_sep = 0;

        if (source == null)
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: source file not found");
        }
        else
        {

            if (source.type == "directory")
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: moving / renaming folders is not allowed.");
                return;
            }
            else
            {
                last_sep = source.full_path.LastIndexOf('/');
                string source_parent_path = (last_sep == 0) ? "/" : source.full_path.Remove(last_sep);
                var source_parent = TerminalHandler.Instance.VirtualFileSystem.Query(source_parent_path);
                if (!TerminalHandler.Instance.CheckPermissions(source_parent, "-wx"))
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied.");
                    return;
                }
            }

            string destination_path = TerminalHandler.Instance.VirtualFileSystem.GetFinalPath(cmd[2]);
            last_sep = destination_path.LastIndexOf('/');
            string destination_parent_path = (last_sep == 0) ? "/" : destination_path.Remove(last_sep);
            var destination_parent = TerminalHandler.Instance.VirtualFileSystem.Query(destination_parent_path);
            var destination = TerminalHandler.Instance.VirtualFileSystem.Query(destination_path);

            if (destination != null && destination.type == "directory")
            {
                destination_path += "/" + source.name;
                destination_parent = destination;
            }

            if (destination_parent == null)
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Destination folder \"" + destination_parent_path + "\" not found.");
                return;
            }

            if (!TerminalHandler.Instance.CheckPermissions(destination_parent, "-wx"))
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied.");
                return;
            }

            var copy = TerminalHandler.Instance.VirtualFileSystem.MoveEntry(source, destination_path);
            TerminalHandler.Instance.TerminalConfig.LoadCmdsFromPATH();
            TerminalHandler.Instance.InstantiateNewLine();
        }
    }
}
