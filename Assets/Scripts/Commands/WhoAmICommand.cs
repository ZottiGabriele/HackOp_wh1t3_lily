using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/WhoAmICommand", fileName = "WhoAmICommand")]
public class WhoAmICommand : ICommand
{
    public override string GetCmdName() => "whoami";
    public override string GetCmdDescription() => "<b>whoami</b> : print name of current user";
    public override string GetCmdMatch() => "^ *whoami *$";

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.DisplayOutput(TerminalHandler.Instance.TerminalConfig.CurrentUser);
    }
}
