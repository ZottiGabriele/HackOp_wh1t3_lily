using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoamingUIHandler : MonoBehaviour
{
    public static RoamingUIHandler Instance {get; private set;}

    [SerializeField] GameObject _textBox;
    [SerializeField] [Range(0,1)] float typingSpeed = 0.95f;
    [SerializeField] TMP_Text _text;

     private void Awake()
    {
        if(!Instance) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else if (Instance != this) {
            Destroy(this);
        }
    }

    public void ShowText(string text) {
        _textBox.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(startTypingEffect(text));
    }

    IEnumerator startTypingEffect(string text) {
        _text.text = "";
        foreach(var c in text) {
            _text.text += c;
            yield return new WaitForSeconds(1 - typingSpeed);
        }
        yield return new WaitForSeconds(2);
        _textBox.SetActive(false);
    }
}
