using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ClearCommand", fileName = "ClearCommand")]
public class ClearCommand : ICommand
{
    public override string GetCmdName() => "clear";
    public override string GetCmdDescription() => "<b>clear</b> : clear the terminal screen";
    public override string GetCmdMatch() => "^ *clear *$";
    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ClearScreen();
    }
}
