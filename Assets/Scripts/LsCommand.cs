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
        //TODO: actually use the active path to query the VFS for the output
        
        string output = "";
        foreach (var f in TerminalHandler.Instance.VirtualFileSystem.contents)
        {
            output += $"{f.prot} {f.user} {f.group} {f.size} {f.name}\n";
        }

        a_flag = false;
        l_flag = false;

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
