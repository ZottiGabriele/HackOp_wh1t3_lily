using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LineHandler : MonoBehaviour
{
    public TMP_InputField InField {get => _inField;}

    [SerializeField] TMP_InputField _inField;
    [SerializeField] TMP_Text _outField;

    public string cmd;

    public void OnEndEdit(string cmd) {

        if(cmd == "" || !Input.GetKeyDown(KeyCode.Return)) return;

        this.cmd = cmd;

        // _inField.interactable = false;
        _inField.readOnly = true;

        optimizeUI(transform);

        TerminalHandler.Instance.OnCommandInputEnd(this);
    }

    private void optimizeUI(Transform target) {
        
    }

    public void OnValueChanged() {
        TerminalHandler.Instance.ScrollToBottom();
    }

    public void DisplayOutput(string output) {
        _outField.text = output;
    }
}
