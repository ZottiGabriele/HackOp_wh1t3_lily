using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solution : MonoBehaviour, IGUID
{
    public string GUID => _guid;
    [UniqueIdentifier] [SerializeField] string _guid;
    [SerializeField] GameObject _unlock;
    public bool Unlocked = false;

    HintManager _hintManager;

    private void Start()
    {
        Unlocked = GameStateHandler.Instance.GameData.UnlockedHintIDs.Contains(GUID);

        _unlock.SetActive(!Unlocked);
    }

    public void Unlock()
    {
        _unlock.SetActive(false);
        _hintManager.UnlockSolution(GUID);
    }

    public void SetHintManager(HintManager manager)
    {
        _hintManager = manager;
    }

}
