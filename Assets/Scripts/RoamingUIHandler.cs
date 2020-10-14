using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoamingUiHandler : MonoBehaviour
{
    [SerializeField] TMP_Text hintTokenCount;

    private void Start() {
        GameStateHandler.Instance.OnHintTokenUpdate += OnHintTokenUpdate;
        OnHintTokenUpdate();
    }

    private void OnDestroy() {
        GameStateHandler.Instance.OnHintTokenUpdate -= OnHintTokenUpdate;
    }

    private void OnHintTokenUpdate()
    {
        hintTokenCount.text = GameStateHandler.Instance.GameData.HintTokenCount.ToString();
    }
}
