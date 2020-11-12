using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IInteractionArea : IGameDataObserver
{
    protected virtual void Awake() {
        gameObject.layer = LayerMask.NameToLayer("Interaction");
    }

    public void OnInteraction() {
        executeOnCondition();
    }
}
