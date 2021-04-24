using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ImgViewCommand", fileName = "ImgViewCommand")]
public class ImgViewCommand : ICommand
{
    public override string GetCmdName() => "imgview";
    public override string GetCmdDescription() => "<b>imgview <file></b> : display <file> as image";
    public override string GetCmdMatch() => "^ *imgview +[\\S]+ *$";

    [SerializeField] string _targetImgName = "";

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
                if (query_item.name == _targetImgName)
                {
                    OfficeServerRoomHandler.ShowImg();
                    TerminalHandler.Instance.InstantiateNewLine();
                }
                else
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " cannot be opened as an image");
                }
            }
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + arg + " not found");
        }
    }
}