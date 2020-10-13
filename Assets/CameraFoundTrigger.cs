using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFoundTrigger : MonoBehaviour
{
    [SerializeField] CinemachineTriggerAction _cutsceneTrigger;

    private void Start() {
        _cutsceneTrigger.enabled = !GameStateHandler.Instance.GameData.SecurityCameraFound;
    }
}
