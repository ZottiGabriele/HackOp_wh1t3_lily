using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "Custom/Commands/CatCommand", fileName = "CatCommand")]
public class CatCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>cat <file></b> : outputs the contents of <file>";
    }

    public override string GetCmdMatch()
    {
        return "^ *cat *$|^ *cat *[\\w\\d\\.\\/]+ *$";
    }

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
                var target = Resources.Load(query_item.r_full_path) as TextAsset;
                if(target == null) {
                    TerminalHandler.Instance.DisplayOutput("Can't read file contents");
                    return;
                }
                TerminalHandler.Instance.DisplayOutput(target.text);
                Resources.UnloadAsset(target);
            }
        } else {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " not found");
        }
    }
}
