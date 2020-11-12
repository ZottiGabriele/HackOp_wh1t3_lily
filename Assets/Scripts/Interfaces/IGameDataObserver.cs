using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IGameDataObserver : MonoBehaviour
{
    [HideInInspector] [SerializeField] protected bool _hasCondition = false;
    [HideInInspector] [SerializeField] protected string _condition = "";
    [HideInInspector] [SerializeField] protected bool _conditionTarget;
    [HideInInspector] [SerializeField] private int _conditionIndex;

    protected virtual void executeOnCondition() {
        if(!_hasCondition) { execute(); }
        else if((bool)typeof(GameData).GetField(_condition).GetValue(GameStateHandler.Instance.GameData) == _conditionTarget) {
            execute();
        }
    }
    protected abstract void execute();
}
