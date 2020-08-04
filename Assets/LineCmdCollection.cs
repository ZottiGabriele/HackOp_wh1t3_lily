using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="New LineCmdCollection", menuName="LineCmd/New LineCmdCollection")]
public class LineCmdCollection : ScriptableObject {
    public List<LineCmd> cmds;
}

[CreateAssetMenu(fileName="New LineCmd", menuName="LineCmd/New LineCmd")]
public class LineCmd : ScriptableObject
{
    public string Pattern = "^ *$";
    public string ErrorMsg = "ERROR: ";
}