using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/SortCommand", fileName = "SortCommand")]
public class SortCommand : ICommand
{
    public override string GetCmdName() => "sort";
    public override string GetCmdDescription() => "<b>sort <file></b> : sort lines of text <file>";
    public override string GetCmdMatch() => "^ *sort *$|^ *sort +[\\S]+ *$";
    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (cmd.Length != 2)
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: sort requires a file as input.\n\nUSAGE: sort <file>");
            return;
        }

        string arg = cmd[1];
        var query_item = TerminalHandler.Instance.VirtualFileSystem.Query(arg);

        if (query_item != null)
        {
            if (query_item.type == "directory")
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: The file " + arg + " is a directory");
            }
            else
            {
                if (!TerminalHandler.Instance.CheckPermissions(query_item, "r--"))
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
                    return;
                }
                if (query_item.readable)
                {
                    var lines = new List<string>(query_item.content.Split('\n'));
                    lines.Sort();
                    var output = "";
                    foreach (var l in lines)
                    {
                        output += l + "\n";
                    }
                    TerminalHandler.Instance.DisplayOutput(output);
                }
                else
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: can't read file contents");
                }
            }
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " not found");
        }
    }
}
