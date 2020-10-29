using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "Custom/Commands/CatCommand", fileName = "CatCommand")]
public class CatCommand : ICommand
{
    public override string GetCmdName() => "cat";
    public override string GetCmdDescription() => "<b>cat <file></b> : outputs the contents of <file>";
    public override string GetCmdMatch() => "^ *cat *$|^ *cat +[\\S]+ *$";

    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new []{' '}, System.StringSplitOptions.RemoveEmptyEntries);

        if(cmd.Length != 2) {
            TerminalHandler.Instance.DisplayOutput("ERROR: cat requires a file as input.\n\nUSAGE: cat <file>");
            return;
        }

        string arg = cmd[1];
        var query_item = TerminalHandler.Instance.VirtualFileSystem.Query(arg);

        if(query_item != null) {
            if(query_item.type == "directory") {
                TerminalHandler.Instance.DisplayOutput("ERROR: The file " + arg + " is a directory");
            } else {
                if(!TerminalHandler.Instance.CheckPermissions(query_item, "r--")) {
                    TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
                    return;
                }
                if(query_item.readable) {
                    TerminalHandler.Instance.DisplayOutput(query_item.content);
                } else {
                    TerminalHandler.Instance.DisplayOutput("ERROR: can't read file contents");
                }
            }
        } else {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " not found");
        }
    }
}
