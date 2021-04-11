using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/DecodeCommand", fileName = "DecodeCommand")]
public class DecodeCommand : ICommand
{
    public override string GetCmdName() => "decode";
    public override string GetCmdDescription()
    {
        string description = "<b>decode [ALGORITHM] <target></b> : decodes <target> text based on the chosen [ALGORITHM]\n";
        description += "\nALGORITHM\n";
        description += "\t<b>-b</b> : Base64 decoder\n";
        description += "\t<b>-h</b> : Hexadecimal decoder\n";
        description += "\t<b>-r</b> : Rot13 decoder";
        return description;
    }
    public override string GetCmdMatch() => "^ *decode +-\\w +\\S+";

    public override void OnCmdMatch()
    {
        var cmd = _cmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        var option = cmd[1][1];
        var content = _cmd.Replace(cmd[0], "").Replace(cmd[1], "").TrimStart();
        DecodeAlg alg;

        switch (option)
        {
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
                TerminalHandler.Instance.DisplayOutput("ERROR: Option \"-" + option + "\" not available. Type \"help\" to view the list of available options.");
                return;
        }

        try
        {
            var output = decode(content, alg);
            TerminalHandler.Instance.DisplayOutput(output);

            if (output == "ANSWER{FORTYTWO}")
            {
                OfficeServerRoomHandler.OnFourthChallengeCompelted();
            }
        }
        catch (Exception e)
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: The string can't be decoded with the chosen algorithm.");
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
    Base64,
    Hex,
    Rot13
}