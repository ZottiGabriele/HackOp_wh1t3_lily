using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintTokenInteractionArea : TextInteractionArea, IGUID
{
    public string GUID => _guid;
    [UniqueIdentifier] [SerializeField] string _guid;
    [SerializeField] bool isEnabled = true;


    protected override void execute()
    {
        isEnabled = !GameStateHandler.Instance.GameData.FoundHintIDs.Contains(GUID);

        if (!isEnabled) return;
        base.execute();
        PlayerController.Instance.OnHintTokenFound();
        GameStateHandler.Instance.AddHintToken(GUID);
        SoundsHandler.Instance.PlayHintTokenFoundSound();

        isEnabled = false;
    }
}
