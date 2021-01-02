using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class Hint : MonoBehaviour, IGUID
{
    public string GUID => _guid;
    [UniqueIdentifier] [SerializeField] string _guid;

    [HideInInspector] [SerializeField] private string _condition = "";
    [HideInInspector] [SerializeField] private int _conditionIndex;
    public bool Unlocked = false;
    [SerializeField] TMP_Text _unlockText;
    [SerializeField] GameObject _unlock;
    [SerializeField] GameObject _lock;



    private void Start()
    {
        Unlocked = GameStateHandler.Instance.GameData.UnlockedHintIDs.Contains(GUID);

        _unlock.SetActive(!Unlocked);
        _lock.SetActive(_condition != "None" && !getConditionValue());
    }

    private void FixedUpdate()
    {
        if (_lock.activeInHierarchy && getConditionValue())
        {
            _lock.SetActive(false);
        }
    }

    private bool getConditionValue()
    {
        return (bool)typeof(GameData).GetField(_condition).GetValue(GameStateHandler.Instance.GameData);
    }

    public void Unlock()
    {
        if (GameStateHandler.Instance.GameData.HintTokenCount > 0)
        {
            _unlock.SetActive(false);
            HintManager.Instance.UnlockHint(GUID);
        }
        else
        {
            StopAllCoroutines();
            _unlockText.text = "Not enough ";
            StartCoroutine(resetUnlockText());
        }
    }

    private IEnumerator resetUnlockText()
    {
        yield return new WaitForSeconds(1f);
        _unlockText.text = "Unlock for 1 ";
    }
}
