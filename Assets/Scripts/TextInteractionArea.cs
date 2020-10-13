using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class TextInteractionArea : IInteractionArea
{
    [SerializeField] string onInteractionText = "";

    private void Awake() {
        if(onInteractionText == "") Debug.LogWarning(gameObject + " has an interaction area but no interaction text!");
        gameObject.layer = LayerMask.NameToLayer("Interaction");
    }

    public override void OnInteraction()
    {
        RoamingUIHandler.Instance.ShowText(onInteractionText);
    }
}
