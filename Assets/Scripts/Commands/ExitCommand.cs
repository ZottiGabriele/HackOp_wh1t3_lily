using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ExitCommand", fileName = "ExitCommand")]
public class ExitCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>exit</b> : exits active shell";
    }

    public override string GetCmdMatch()
    {
        return "^ *exit *$";
    }

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ExitShell();
    }

    public override void AfterCmdMatch() {}
}
