using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpInteractionArea : IInteractionArea
{
    public override void OnInteraction()
    {
        RoamingUIHandler.Instance.ShowPopUp();
    }
}
