using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "Custom/Commands/UnzipCommand", fileName = "UnzipCommand")]
public class UnzipCommand : ICommand
{
    public override string GetCmdName() => "unzip";
    public override string GetCmdDescription() => "<b>unzip <archive></b> : extracts compressed files in the ZIP <archive>";
    public override string GetCmdMatch() => "^ *unzip +\\S+ *$";

    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
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
                if (query_item.name == "wh1t3_l1ly.jpg")
                {
                    OfficeServerRoomHandler.ExtractHiddenFolder();
                }
                else
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: file is not a ZIP archive");
                }
            }
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " not found");
        }
    }
}
