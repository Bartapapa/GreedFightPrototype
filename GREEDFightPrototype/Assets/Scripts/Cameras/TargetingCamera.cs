using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingCamera : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public CinemachineVirtualCamera VirtualCam;
    public BattleCharacter CharacterFocus;

    private Quaternion _originalRot;

    private void Start()
    {
        _originalRot = transform.rotation;
    }

    private void LateUpdate()
    {
        if (VirtualCam.enabled == false) return;
        HandleFocus();
    }

    private void HandleFocus()
    {
        if (CharacterFocus)
        {
            Vector3 dir = (CharacterFocus.transform.position + (Vector3.up * CharacterFocus.VerticalCameraOffset)) - transform.position;
            dir = dir.normalized;
            Quaternion toRot = Quaternion.LookRotation(dir, transform.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRot, 5f * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, _originalRot, 5f * Time.deltaTime);
        }
    }
}
