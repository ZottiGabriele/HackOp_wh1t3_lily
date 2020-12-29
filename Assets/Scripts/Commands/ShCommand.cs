using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ShCommand", fileName = "ShCommand")]
public class ShCommand : ICommand
{
    public override string GetCmdName() => "sh";
    public override string GetCmdDescription() => "<b>sh</b> : starts a new shell";
    public override string GetCmdMatch() => "^ *sh *$";
    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.NewShell();
    }
}
