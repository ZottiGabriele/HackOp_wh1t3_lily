using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInteractionArea : IInteractionArea
{
    [SerializeField] GeneralUIHandler.PopUpType _popUpType;
    
    public override void OnInteraction()
    {
        GeneralUIHandler.Instance.ShowPopUp(_popUpType);
    }
}
