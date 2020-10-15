using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameraHandler : MonoBehaviour
{
    public void PlayerCaught() {
        Debug.Log("Player has been caught by security camera");
        PlayerController.Instance.DisableInput();
    }
}
