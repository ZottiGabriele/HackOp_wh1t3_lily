using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/Commands/LsCommand", fileName = "LsCommand")]
public class LsCommand : ICommand
{
    public override string GetCmdMatch()
    {
        return "^ *ls *$|^ *ls +-a?l? *$|^ *ls +-l?a? *$";
    }

    public override void OnCmdMatch()
    {
        Debug.Log("Imma show you the files in the current dir :) [NOT YET IMPLEMENTED]");
    }
}
