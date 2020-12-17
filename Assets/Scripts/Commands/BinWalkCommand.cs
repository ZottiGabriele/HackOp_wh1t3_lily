using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[CreateAssetMenu(menuName = "Custom/Commands/BinWalkCommand", fileName = "BinWalkCommand")]
public class BinWalkCommand : ICommand
{
    public override string GetCmdName() => "binwalk";
    public override string GetCmdDescription() => "<b>binwalk <file></b> : searches <file> for embedded files and executable code";
    public override string GetCmdMatch() => "^ *binwalk +\\S+ *$";

    string output = "DECIMAL       HEXADECIMAL     DESCRIPTION\n" +
"--------------------------------------------------------------------------------\n" +
"0             0x0             JPEG image data, JFIF standard 1.01\n" +
"30            0x1E            TIFF image data, little-endian offset of first image directory: 8\n" +
"27295         0x6A9F          Zip archive data, name: nothing_to_see_here/gibberish.txt\n" +
"27541         0x6B95          End of Zip archive, footer length: 22\n";

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
