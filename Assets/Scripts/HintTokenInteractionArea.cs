using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTokenInteractionArea : TextInteractionArea
{
    [SerializeField] bool isEnabled = true;

    public override void OnInteraction()
    {
        if(!isEnabled) return;
        base.OnInteraction();
        PlayerController.Instance.OnHintTokenFound();
        GameStateHandler.Instance.AddHintToken(1);
        SoundsHandler.Instance.PlayHintTokenFoundSound();
        
        isEnabled = false;
    }
}
