using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ShCommand", fileName = "ShCommand")]
public class ShCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>sh</b> : starts a new shell";
    }

    public override string GetCmdMatch()
    {
        return "^ *sh *$";
    }

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.NewShell();
    }
}
