using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class IInteractionArea : MonoBehaviour
{
    public void OnInteraction() {
        if(!_hasCondition) { onInteraction(); }
        else if((bool)typeof(GameData).GetField(_condition).GetValue(GameStateHandler.Instance.GameData) == _conditionTarget) {
            onInteraction();
        }
    }
    protected abstract void onInteraction();
    [HideInInspector] [SerializeField] protected bool _hasCondition = false;
    [HideInInspector] [SerializeField] protected string _condition;
    [HideInInspector] [SerializeField] protected bool _conditionTarget;
    [HideInInspector] [SerializeField] private int _conditionIndex;
}
