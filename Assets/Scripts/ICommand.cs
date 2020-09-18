using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public abstract class ICommand : ScriptableObject
{
    public abstract string GetCmdMatch();

    public virtual bool CheckCmdMatch(string cmd) {

        Match m = Regex.Match(cmd, GetCmdMatch());

        return m.Success;
    }

    public abstract void OnCmdMatch();
}
