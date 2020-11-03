using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] [Range(0,5f)] float _parallaxSpeed = 2f;
    SpriteRenderer[] _childs;
    Vector3 _prevCameraPos;

    private void Start() {
        _childs = transform.GetComponentsInChildren<SpriteRenderer>();
        _prevCameraPos = Camera.main.transform.position;
    }


    void Update()
    {
        float speed = (_prevCameraPos - Camera.main.transform.position).x * _parallaxSpeed * Time.deltaTime;
        foreach(var c in _childs) {
            c.transform.position += Vector3.right * speed * c.sortingOrder;
        }
    }

    private void LateUpdate() {
        _prevCameraPos = Camera.main.transform.position;
    }
}
