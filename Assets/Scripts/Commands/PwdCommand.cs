using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/PwdCommand", fileName = "PwdCommand")]
public class PwdCommand : ICommand
{
    public override string GetCmdName() => "pwd";
    public override string GetCmdDescription() => "<b>pwd</b> : print name of current working directory";
    public override string GetCmdMatch() => "^ *pwd *$";
    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.DisplayOutput(TerminalHandler.Instance.TerminalConfig.CurrentPath);
    }
}
