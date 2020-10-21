using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/WhoAmICommand", fileName = "WhoAmICommand")]
public class WhoAmICommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>whoami</b> : print name of current user";
    }

    public override string GetCmdMatch()
    {
        return "^ *whoami *$";
    }

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.DisplayOutput(TerminalHandler.Instance.TerminalConfig.CurrentUser);
    }
}
