﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] TMP_Text _displayText;
    [SerializeField] Cutscene _zoomOut;
    [SerializeField] Cutscene _wrongCode;
    [SerializeField] Cutscene _rightCode;
    [SerializeField] string _targetCode;
    
    public void PressedKey(string key) {
        if(_displayText.text.Length < 5) {
            _displayText.text += key;
        }
    }

    public void DeleteKey() {
        if(_displayText.text.Length > 0) {
            _displayText.text = _displayText.text.Substring(0, _displayText.text.Length - 1);
        }
    }

    public void Leave() {
        _zoomOut.Play();
    }

    public void ConfirmKey() {
        if(_displayText.text == _targetCode) {
            _rightCode.Play( _ => {
                Clear();
            });
        } else {
            _wrongCode.Play( _ => {
                Clear();
            });
        }
    }

    public void Clear() {
        _displayText.text = "";
    }
}
