using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/GenerateDailyCodeCommand", fileName = "generate_daily_code")]
public class GenerateDailyCodeCommand : ICommand
{
    public override string GetCmdName() => "generate_daily_code";
    public override string GetCmdDescription() => "";
    public override string GetCmdMatch() => "^ *generate_daily_code *$";
    public static Action OnDailyCodeGenerated = () => { };

    public override void OnCmdMatch()
    {
        TerminalHandler.Instance.ReadInput("Master Password: ", checkCorrectPassword);
    }

    private void checkCorrectPassword(string psw)
    {
        if (psw == "Alea Iacta Est")
        {
            TerminalHandler.Instance.DisplayOutput("New daily code generated: 74421");
            OnDailyCodeGenerated();
        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: wrong master password.");
        }
    }
}
