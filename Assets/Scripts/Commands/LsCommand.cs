using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/LsCommand", fileName = "LsCommand")]
public class LsCommand : ICommand
{
    private bool a_flag = false;
    private bool l_flag = false;

    public override string GetCmdDescription()
    {
        string description = "<b>ls [OPTIONS]</b> : list directory contents\n";
        description += "\nOPTIONS\n";
        description += "\t<b>-a</b> :  do not ignore entries starting with .\n";
        description += "\t<b>-l</b> : use a long listing format\n";
        return description;
    }

    public override string GetCmdMatch()
    {
        return "^ *ls *$|^ *ls +-a?l? *$|^ *ls +-l?a? *$";
    }

    public override bool CheckCmdMatch(string cmd) {

        Match m = Regex.Match(cmd, GetCmdMatch());

        if(m.Success && cmd.Contains("-")) {
            string flags = cmd.Split('-')[1];
            a_flag = flags.Contains("a");
            l_flag = flags.Contains("l");
        }

        return m.Success;
    }

    public override void OnCmdMatch()
    {   
        string output = "";
        foreach (var f in TerminalHandler.Instance.VirtualFileSystem.ActiveEntry.contents)
        {
            var file_path_components = f.name.Split('/');
            string file_name = file_path_components[file_path_components.Length - 1];
            if(a_flag || file_name[0] != '.') {
                if(l_flag) {
                    output += $"{f.prot} {f.user} {f.group} {f.size} {file_name}\n";
                } else {
                    output += $"{file_name}  ";
                }
            }
        }

        a_flag = false;
        l_flag = false;

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
