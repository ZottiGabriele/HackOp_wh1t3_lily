using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ExportCommand", fileName = "ExportCommand")]
public class ExportCommand : ICommand
{
    public override string GetCmdDescription()
    {
        return "<b>export <VAR>=<value></b> : sets enviroment variable <$VAR> with value <value>";
    }

    public override string GetCmdMatch()
    {
        return "^ *export +\\$\\w+=[\\w\\$/:]+ *$|^ *export +\\w+=[\\w\\$/:]+ *$";
    }

    public override void OnCmdMatch()
    {
        var args = _cmd.Split(new char[]{' ','='}, System.StringSplitOptions.RemoveEmptyEntries);

        if(args[1][0] == '$') {
            TerminalHandler.Instance.DisplayOutput("ERROR: $ is used only to read the variable, not to set it.\n\nInstead of $VAR=value try VAR=value");
        }

        if(args[1] == "USER" || args[1] == "GROUP" || args[1] == "PWD") {
            TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
        } else {
            TerminalHandler.Instance.TerminalConfig.SetEnvVar("$" + args[1], args[2]);
        }
    }
}
