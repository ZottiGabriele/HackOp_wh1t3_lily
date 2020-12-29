using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/DisableSecurityCameraCommand", fileName = "_ro__re_disable_security_camera")]
public class DisableSecurityCameraCommand : ICommand
{
    public override string GetCmdName() => "disable_security_camera";
    public override string GetCmdDescription() => "";
    public override string GetCmdMatch() => " ^ *disable_security_camera *$|^ *disable_security_camera +";
    public override void OnCmdMatch()
    {
        if (TerminalHandler.Instance.TerminalConfig.CurrentUser != "root")
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied.");
            return;
        }
        TerminalHandler.Instance.DisplayOutput("Security Camera succesfully disabled.");
        TerminalHandler.Instance.OnChallengeCompleted();
    }
}
