using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/HelpCommand", fileName = "HelpCommand")]
public class HelpCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>help</b> : print all available commands";
    }

    public override string GetCmdMatch()
    {
        return "^ *help *$";
    }

    public override void OnCmdMatch()
    {
        string output = "";

        foreach(var c in TerminalHandler.Instance.TerminalConfig.AvailableCommands) {
            output += c.GetCmdDescription() + "\n\n";
        }

        output = output.Remove(output.Length - 2);

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
