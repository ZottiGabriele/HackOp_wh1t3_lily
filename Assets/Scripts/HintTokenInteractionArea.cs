using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTokenInteractionArea : TextInteractionArea
{
    public int ID = -1;
    [SerializeField] bool isEnabled = true;

    protected override void execute()
    {
        isEnabled = !GameStateHandler.Instance.GameData.FoundHintIDs.Contains(ID);

        if (!isEnabled) return;
        base.execute();
        PlayerController.Instance.OnHintTokenFound();
        GameStateHandler.Instance.AddHintToken(ID);
        SoundsHandler.Instance.PlayHintTokenFoundSound();

        isEnabled = false;
    }
}
