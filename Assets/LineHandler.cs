using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineHandler : MonoBehaviour
{
    [SerializeField] TMP_InputField _inField;

    public string cmd;
    // public string[] args;

    public void OnEndEdit(string cmd) {

        if(cmd == "") { EventSystem.current.SetSelectedGameObject(_inField.gameObject); return; }

        this.cmd = cmd;

        // string[] cmd_strings = cmd.Split();

        // this.cmd = cmd_strings[0];
        // args = new string[cmd_strings.Length];
        
        // for (int i = 1; i < cmd_strings.Length; i++)
        // {
        //     args[i-1] = cmd_strings[i];
        // }

        _inField.readOnly = true;
        _inField.interactable = false;
        _inField.GetComponent<LayoutElement>().flexibleHeight = 0;

        TerminalHandler.Instance.OnCommandInputEnd(this);
    }
}
