using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/HelpCommand", fileName = "HelpCommand")]
public class HelpCommand : ICommand
{
    public override string GetCmdName() => "help";
    public override string GetCmdDescription() => "<b>help</b> : print all available commands";
    public override string GetCmdMatch() => "^ *help *$";
    public override void OnCmdMatch()
    {
        string output = "";

        foreach (var c in TerminalHandler.Instance.TerminalConfig.AvailableCommands)
        {
            output += c.Value.GetCmdDescription() + "\n\n";
        }

        output = output.Remove(output.Length - 2);

        TerminalHandler.Instance.DisplayOutput(output);
    }
}
