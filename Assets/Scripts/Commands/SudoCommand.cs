using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// sudo /bin/cat /home/root/notes/../.passwd

[CreateAssetMenu(menuName = "Custom/Commands/SudoCommand", fileName = "SudoCommand")]
public class SudoCommand : ICommand
{
    public override string GetCmdName() => "sudo";
    public override string GetCmdDescription()
    {
        string description = "<b>sudo [OPTION] <cmd></b> : execute <cmd> as root\n";
        description += "\nOPTION\n";
        description += "\t<b>-l</b> :  list the allowed (and forbidden) commands for the invoking user on the current host.\n";
        return description;
    }
    public override string GetCmdMatch() => "^ *sudo +-l *$|^ *sudo +\\S";

    public override void OnCmdMatch()
    {
        var args = new List<string>(_cmd.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries));

        if (args[1] == "-l")
        {
            var user = TerminalHandler.Instance.TerminalConfig.CurrentUser;
            var host = TerminalHandler.Instance.TerminalConfig.HostName;
            var output = "Matching Defaults entries for \"" + user + "\" on " + host + ":\n" +
                         "env_reset, secure_path=/usr/local/bin\\:/usr/bin\\:/bin\n\n" +
                         "User \"" + TerminalHandler.Instance.TerminalConfig.CurrentUser + "\" may run the following commands on " + TerminalHandler.Instance.TerminalConfig.HostName + ":\n" +
                         "/bin/cat /home/root/notes/*";
            TerminalHandler.Instance.DisplayOutput(output);
            return;
        }

        if (args[1] == "/bin/cat" && args[2].Contains("/home/root/notes/"))
        {
            var prev = TerminalHandler.Instance.TerminalConfig;

            TerminalHandler.Instance.TerminalConfig.CurrentUser = "root";
            TerminalHandler.Instance.TerminalConfig.CurrentGroup = "root";

            args.RemoveAt(0);
            var cmd = rebuildCmd(args);

            TerminalHandler.Instance.ParseCommand(cmd);

            if (TerminalHandler.Instance.TerminalConfig != prev)
            {
                prev.CurrentUser = "user";
                prev.CurrentGroup = "group";
            }
            else
            {
                TerminalHandler.Instance.TerminalConfig.CurrentUser = "user";
                TerminalHandler.Instance.TerminalConfig.CurrentGroup = "group";
            }

            TerminalHandler.Instance.BuildPrompt();

        }
        else
        {
            TerminalHandler.Instance.DisplayOutput("ERROR: Permission Denied.\nCheck sudo -l for the list of available privileges.");
        }
    }

    private string rebuildCmd(List<string> args)
    {
        string output = "";
        foreach (var a in args)
        {
            output += a + " ";
        }
        return output;
    }
}
