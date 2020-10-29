using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ExportCommand", fileName = "ExportCommand")]
public class ExportCommand : ICommand
{
    public override string GetCmdName() => "export";
    public override string GetCmdDescription() =>"<b>export <VAR>=<value></b> : sets enviroment variable <VAR> with value <value>";
    public override string GetCmdMatch() => "^ *export +\\$\\w+=[\\w\\$/:]+ *$|^ *export +\\w+=[\\w\\$/:]+ *$";
    public override void OnCmdMatch()
    {
        var args = _cmd.Split(new char[]{' ','='}, System.StringSplitOptions.RemoveEmptyEntries);

        if(args[1][0] == '$') {
            TerminalHandler.Instance.DisplayOutput("ERROR: $ is used only to read the variable, not to set it.\n\nInstead of $VAR=value try VAR=value");
            return;
        }

        if(args[1] == "USER" || args[1] == "GROUP" || args[1] == "PWD") {
            TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
        } else {

            bool has_preface = (args[2][0] != '$');
            var vars = args[2].Split('$');
            string value = (has_preface) ? vars[0] : "";
            for(int i = (has_preface) ? 1 : 0; i < vars.Length; i++) {
                value += TerminalHandler.Instance.TerminalConfig.TryGetEnvVar("$" + vars[i]);
            }

            TerminalHandler.Instance.TerminalConfig.SetEnvVar("$" + args[1], value);
            TerminalHandler.Instance.TerminalConfig.LoadCmdsFromPATH();
            TerminalHandler.Instance.InstantiateNewLine();
        }
    }
}
