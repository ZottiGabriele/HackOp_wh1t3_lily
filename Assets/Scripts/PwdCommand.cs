using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/PwdCommand", fileName = "PwdCommand")]
public class PwdCommand : ICommand
{
    public override string GetCmdMatch()
    {
        return "^ *pwd *$";
    }

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.DisplayOutput(TerminalHandler.Instance.TerminalConfig.CurrentPath);
    }
}
