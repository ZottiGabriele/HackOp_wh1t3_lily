using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/DecodeCommand", fileName = "DecodeCommand")]
public class DecodeCommand : ICommand
{
    public override string GetCmdName() => "decode";
    public override string GetCmdDescription() => "<b>decode [OPTIONS] <source></b> : decodes the contents of <source> based on the chosen method";
    public override string GetCmdMatch() => "^ *decode +-\\w +\\S+ *$";

    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        var option = cmd[1][1];
        var src = cmd[2];
        DecodeAlg alg;

        switch (option)
        {
            case 'a':
                alg = DecodeAlg.ASCII;
                break;
            case 'b':
                alg = DecodeAlg.Base64;
                break;
            case 'h':
                alg = DecodeAlg.Hex;
                break;
            case 'r':
                alg = DecodeAlg.Rot13;
                break;
            default:
                TerminalHandler.Instance.DisplayOutput("ERROR: Option not available. Type \"help\" to view the list of available options.");
                return;
        }

        var src_item = TerminalHandler.Instance.VirtualFileSystem.Query(src);
        if (src_item != null)
        {
            if (src_item.type == "directory")
            {
                TerminalHandler.Instance.DisplayOutput("ERROR: Source file " + src + " is a directory");
                return;
            }
            else
            {
                if (!TerminalHandler.Instance.CheckPermissions(src_item, "r--"))
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
                    return;
                }
                if (src_item.readable)
                {
                    try
                    {
                        string decodedContent = decode(src_item.content, alg);
                        TerminalHandler.Instance.DisplayOutput(decodedContent);
                    }
                    catch (Exception e)
                    {
                        TerminalHandler.Instance.DisplayOutput("ERROR: can't decode " + src + " with the selected algorithm.");
                    }
                }
                else
                {
                    TerminalHandler.Instance.DisplayOutput("ERROR: can't read file contents");
                }
            }
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: File " + src + " not found");
        }
    }

    private string decode(string input, DecodeAlg alg)
    {
        string output = "";
        input = input.Replace(" ", "");
        switch (alg)
        {
            case DecodeAlg.Base64:
                output = Encoding.UTF8.GetString(Convert.FromBase64String(input));
                break;
            case DecodeAlg.Hex:
                for (int i = 1; i < input.Length; i += 2)
                {
                    byte c = Convert.ToByte(input.Substring(i - 1, 2), 16);
                    output += (char)c;
                }
                break;
            case DecodeAlg.Rot13:
                foreach (var c in input)
                {
                    int number = (int)c;
                    if (number >= 'a' && number <= 'z')
                    {
                        if (number > 'm') { number -= 13; }
                        else { number += 13; }
                    }
                    else if (number >= 'A' && number <= 'Z')
                    {
                        if (number > 'M') { number -= 13; }
                        else { number += 13; }
                    }
                    output += (char)number;
                }
                break;
        }

        return output;
    }
}

enum DecodeAlg
{
    ASCII,
    Base64,
    Hex,
    Rot13
}