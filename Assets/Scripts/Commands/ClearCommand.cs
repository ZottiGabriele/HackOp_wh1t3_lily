using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ClearCommand", fileName = "ClearCommand")]
public class ClearCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>clear</b> : clear the terminal screen";
    }

    public override string GetCmdMatch()
    {
        return "^ *clear *$";
    }

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ClearScreen();
    }
}
