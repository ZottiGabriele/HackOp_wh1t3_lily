using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/EchoCommand", fileName = "EchoCommand")]
public class EchoCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>echo <message></b> : print <message> on screen.";
    }

    public override string GetCmdMatch()
    {
        return "^ *echo +\\S";
    }

    public override void OnCmdMatch()
    {
        var args = _cmd.Split(' ');

        string output = "";

        for (int i = 1; i < args.Length; i++)
        {
            var arg = args[i];

            if(!arg.StartsWith("$")) {
                output += arg + " ";
            } else {
                TerminalHandler.Instance.SendMessage("DOLLAR_" + arg.Substring(1), SendMessageOptions.DontRequireReceiver);
                output += TerminalHandler.Instance.GetDollarOutput() + " ";
            }
        }

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
