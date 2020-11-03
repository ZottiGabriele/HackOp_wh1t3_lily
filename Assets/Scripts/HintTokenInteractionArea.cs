using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTokenInteractionArea : TextInteractionArea
{
    [SerializeField] bool isEnabled = true;

    protected override void onInteraction()
    {
        if(!isEnabled) return;
        base.onInteraction();
        PlayerController.Instance.OnHintTokenFound();
        GameStateHandler.Instance.AddHintToken(1);
        SoundsHandler.Instance.PlayHintTokenFoundSound();
        
        isEnabled = false;
    }
}
