using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/WelcomeUserCommand", fileName = "_s_welcome_user")]
public class WelcomeUserCommand : ICommand
{
    public override string GetCmdName() => "welcome_user";
    public override string GetCmdDescription() => "";
    public override string GetCmdMatch() => "^ *welcome_user *$";
    public override void OnCmdMatch()
    {
        var prev = TerminalHandler.Instance.TerminalConfig;
        TerminalHandler.Instance.TerminalConfig.CurrentUser = "root";
        TerminalHandler.Instance.TerminalConfig.CurrentGroup = "root";
        TerminalHandler.Instance.ParseCommand("cat /home/root/welcome_message.txt");

        if(TerminalHandler.Instance.TerminalConfig != prev) {
            prev.CurrentUser = "user";
            prev.CurrentGroup = "group";
        } else {
            TerminalHandler.Instance.TerminalConfig.CurrentUser = "user";
            TerminalHandler.Instance.TerminalConfig.CurrentGroup = "group";
        }

        TerminalHandler.Instance.BuildPrompt();
    }
}
