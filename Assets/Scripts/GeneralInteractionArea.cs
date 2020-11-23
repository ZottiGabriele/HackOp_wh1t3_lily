using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneralInteractionArea : IInteractionArea
{
    [SerializeField] UnityEvent _onInteraction;

    protected override void execute()
    {
        _onInteraction.Invoke();
    }
}
