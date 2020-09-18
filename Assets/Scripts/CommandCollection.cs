using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/CommandCollection", fileName = "NewCommandCollection")]
public class CommandCollection : ScriptableObject
{
    public List<ICommand> AvailableCommands = new List<ICommand>();
}
