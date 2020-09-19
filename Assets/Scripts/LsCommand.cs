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
        var content = Directory.EnumerateFileSystemEntries(TerminalHandler.Instance.TerminalConfig.CurrentPath);

        string output = "";
        foreach(var c in content) {
            FileInfo c_info = new FileInfo(c);
            if(c_info.Attributes == FileAttributes.Directory) {
                output += getDirInfo(new DirectoryInfo(c));
            } else {
                output += getFileInfo(c_info);
            }
        }

        a_flag = false;
        l_flag = false;

        TerminalHandler.Instance.DisplayOutput(output);
    }

    private string getDirInfo(DirectoryInfo c_info)
    {
        string output = "";
        if(a_flag || c_info.Name[0] != '.') {
            if(l_flag) {
                output = "drwxr-xr-x- " + c_info.GetFileSystemInfos().Length + " testuser testgroup " + c_info.Name + "\n";
            } else {
                output = c_info.Name + "\n";
            }
        }

        return output;
    }

    private string getFileInfo(FileInfo c_info)
    {
        string output = "";
        if(a_flag || c_info.Name[0] != '.') {
            if(l_flag) {
                output = "-rwxr-xr-x- 1 testuser testgroup " + c_info.Name + "\n";
            } else {
                output = c_info.Name + "\n";
            }
        }
        return output;
    }
}
