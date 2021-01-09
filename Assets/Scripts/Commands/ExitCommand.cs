using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ExitCommand", fileName = "ExitCommand")]
public class ExitCommand : ICommand
{
    public override string GetCmdName() => "exit";
    public override string GetCmdDescription() => "<b>exit</b> : exits active shell";
    public override string GetCmdMatch() => "^ *exit *$";
    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ExitShell();
    }
}
