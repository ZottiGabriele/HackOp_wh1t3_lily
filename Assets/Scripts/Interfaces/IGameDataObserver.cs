using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for a Game Data "Obeserver pattern" implementation as an editor extension
/// see: Editor/IGameDataObserverCustomInspector.cs
/// </summary>
public abstract class IGameDataObserver : MonoBehaviour
{
    [HideInInspector] [SerializeField] protected bool _hasCondition = false;
    [HideInInspector] [SerializeField] protected string _condition = "";
    [HideInInspector] [SerializeField] protected bool _conditionTarget;
    [HideInInspector] [SerializeField] private int _conditionIndex;

    protected virtual bool executeOnCondition()
    {
        bool shouldExecute = (!_hasCondition) || ((bool)typeof(GameData).GetField(_condition).GetValue(GameStateHandler.Instance.GameData) == _conditionTarget);
        if (shouldExecute) execute();
        return shouldExecute;
    }
    protected abstract void execute();
}
