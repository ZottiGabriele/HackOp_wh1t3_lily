using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/SshCommand", fileName = "SshCommand")]
public class SshCommand : ICommand
{
    public static Action OnConnectionSuccessfull = () => {};

    [SerializeField] string _targetIP;
    [SerializeField] string _targetUser;
    [SerializeField] string _targetPsw;
    [SerializeField] TerminalConfig _targetConfig;

    bool _pswPassed = false;
    string _ip;
    string _user;


    public override string GetCmdName() => "ssh";
    public override string GetCmdDescription() => "<b>ssh</b> <user>@<ip>: remotely open a shell on another computer";
    public override string GetCmdMatch() => "^ *ssh +\\w+@\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3} *$";
    public override void OnCmdMatch()
    {
        string[] args = _cmd.Split(new char[]{' '}, System.StringSplitOptions.RemoveEmptyEntries);
        _user = args[1].Split('@')[0];
        _ip = args[1].Split('@')[1];

        if(_ip == _targetIP) {
            if(_user == _targetUser) {
                TerminalHandler.Instance.ReadInput("Password for user " + _user + ": ", checkPsw);
            } else {
                TerminalHandler.Instance.DisplayOutput("ERROR: The host " + _ip + " refused connection for user " + _user);
            }
        } else {
            TerminalHandler.Instance.DisplayOutput("ERROR: Impossible to connect to host " + _ip);
        }
    }

    private void checkPsw(string psw) {
        if(psw == _targetPsw) {
            onConnectionSuccessfull();
        } else {
            TerminalHandler.Instance.DisplayOutput("ERROR: The host " + _ip + " refused connection for user " + _user + ": WRONG PASSWORD");
        }
    }

    private void onConnectionSuccessfull() {
        TerminalHandler.Instance.NewSsh(_targetConfig);
        OnConnectionSuccessfull();
    }
}
