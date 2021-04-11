using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/ExportCommand", fileName = "ExportCommand")]
public class ExportCommand : ICommand
{
    public override string GetCmdName() => "export";
    public override string GetCmdDescription() => "<b>export <VAR>=<value></b> : sets enviroment variable <VAR> with value <value>";
    public override string GetCmdMatch() => "^ *export +\\$\\w+=[\\w\\$/:]+ *$|^ *export +\\w+=[\\w\\$/:\\.]+ *$";
    public override void OnCmdMatch()
    {
        var args = _cmd.Split(new char[] { ' ', '=' }, System.StringSplitOptions.RemoveEmptyEntries);

        if (args[1][0] == '$')
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: $ is used only to read the variable, not to set it.\n\nInstead of $VAR=value try VAR=value");
            return;
        }

        if (args[1] == "USER" || args[1] == "GROUP" || args[1] == "PWD")
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: Permission denied");
        }
        else
        {

            string value = subEnvVars(args[2]);

            TerminalHandler.Instance.TerminalConfig.SetEnvVar("$" + args[1], value);
            TerminalHandler.Instance.TerminalConfig.LoadCmdsFromPATH();
            TerminalHandler.Instance.InstantiateNewLine();
        }
    }

    private string subEnvVars(string input)
    {
        string output = input;
        int i = input.IndexOf("$");

        if (i != -1)
        {
            string pre = input.Substring(0, i);
            int j = input.IndexOf(":", i);
            if (j == -1) j = input.Length - i - 1; else j = j - i - 1;
            string envVar = input.Substring(i + 1, j);
            string post = input.Substring(i + 1 + j, input.Length - (i + 1 + j));

            output = subEnvVars(pre + TerminalHandler.Instance.TerminalConfig.TryGetEnvVar("$" + envVar) + post);
        }

        return output;
    }
}