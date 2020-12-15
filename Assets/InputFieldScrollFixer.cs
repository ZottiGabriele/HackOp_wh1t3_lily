using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TMP_InputField))]
public class InputFieldScrollFixer : MonoBehaviour, IScrollHandler
{
    private ScrollRect _scrollRect;

    private void Start()
    {
        _scrollRect = GetComponentInParent<ScrollRect>();
    }

    public void OnScroll(PointerEventData data)
    {
        _scrollRect.OnScroll(data);
    }
}

