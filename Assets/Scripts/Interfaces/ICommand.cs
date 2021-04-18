using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

/// <summary>
/// Interface for all the emulated terminal commands
/// </summary>
public abstract class ICommand : ScriptableObject
{
    public abstract string GetCmdName();
    public abstract string GetCmdDescription();
    public abstract string GetCmdMatch();
    protected string _cmd;

    public virtual bool CheckCmdMatch(string cmd)
    {
        Match m = Regex.Match(cmd, GetCmdMatch());
        if (m.Success) _cmd = cmd;
        return m.Success;
    }

    public abstract void OnCmdMatch();
}
