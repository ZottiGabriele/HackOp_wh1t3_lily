using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    [SerializeField] Transform _player;
    [SerializeField] float _followSpeed = 0.125f;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 fixedArea;

    private void FixedUpdate() {
        Vector3 targetPosition = _player.position + offset - transform.position;
        Vector3 targetPositionNoY = new Vector3(targetPosition.x, 0);
        transform.Translate(targetPositionNoY * Time.fixedDeltaTime * _followSpeed);
    }
}
