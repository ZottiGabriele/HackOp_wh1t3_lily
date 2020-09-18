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

    public void OnEndEdit(string cmd) {

        if(cmd == "") return;

        this.cmd = cmd;

        _inField.readOnly = true;
        _inField.interactable = false;
        _inField.GetComponent<LayoutElement>().flexibleHeight = 0;

        TerminalHandler.Instance.OnCommandInputEnd(this);
    }
}
