using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class TextInteractionArea : IInteractionArea
{
    [SerializeField] string onInteractionText = "";

    protected override void execute()
    {
        GeneralUIHandler.Instance.ShowText(onInteractionText);
    }
}
