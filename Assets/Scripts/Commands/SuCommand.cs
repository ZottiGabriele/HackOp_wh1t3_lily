using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/SuCommand", fileName = "SuCommand")]
public class SuCommand : ICommand
{
    [SerializeField] string _targetPassword = "";

    public override string GetCmdName() => "su";
    public override string GetCmdDescription() => "<b>su</b> : start a superuser shell";
    public override string GetCmdMatch() => "^ *su *$";

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ReadInput("Root password: ", checkPassword);
    }

    private void checkPassword(string password)
    {
        if (password == _targetPassword)
        {
            TerminalHandler.Instance.NewShell();
            TerminalHandler.Instance.TerminalConfig.CurrentUser = "root";
            TerminalHandler.Instance.TerminalConfig.CurrentGroup = "root";
            TerminalHandler.Instance.BuildPrompt();
            OfficeServerRoomHandler.OnThirdChallengeCompleted();
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: authorization failed.");
        }
    }
}
