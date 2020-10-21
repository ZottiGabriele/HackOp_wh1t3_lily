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
                output += TerminalHandler.Instance.TerminalConfig.TryGetEnvVar(arg) + " ";
            }
        }

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
