using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInteractionArea : IInteractionArea
{
    [SerializeField] GeneralUIHandler.PopUpType _popUpType;
    
    protected override void execute()
    {
        GeneralUIHandler.Instance.ShowPopUp(_popUpType);
    }
}
