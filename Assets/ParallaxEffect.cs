using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] Transform _camera;
    [SerializeField] [Range(0,5f)] float _parallaxSpeed = 2f;
    SpriteRenderer[] _childs;
    Vector3 _prevCameraPos;

    private void Start() {
        _childs = transform.GetComponentsInChildren<SpriteRenderer>();
        _prevCameraPos = _camera.position;
    }


    void Update()
    {
        float speed = (_prevCameraPos - _camera.position).x * _parallaxSpeed * Time.deltaTime;
        foreach(var c in _childs) {
            c.transform.position += Vector3.right * speed * c.sortingOrder;
        }
    }

    private void LateUpdate() {
        _prevCameraPos = _camera.position;
    }
}
