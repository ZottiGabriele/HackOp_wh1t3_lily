using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UIInteractionArea : IInteractionArea
{
    [SerializeField] UnityEvent _events;

    private void Start() {
        GetComponent<Button>().onClick.AddListener(() => {executeOnCondition();});
    }

    protected override void execute()
    {
        _events.Invoke();
    }
}
