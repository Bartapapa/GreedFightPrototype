using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    None,
    BattleOverview,
    EnemyTarget,
    MercenaryTarget,
}

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;

    [Header("CAMERAS")]
    public CameraState StartingState = CameraState.BattleOverview;
    [ReadOnlyInspector] public CameraState CurrentState = CameraState.None;
    public CinemachineVirtualCamera BattleOverviewCamera;
    public CinemachineVirtualCamera EnemyTargetCamera;
    public CinemachineVirtualCamera MercenaryTargetCamera;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        EnableCamera(StartingState);
    }

    public void EnableCamera(CameraState state)
    {
        if (state == CurrentState) return;

        switch (state)
        {
            case CameraState.None:
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.enabled = false;
                MercenaryTargetCamera.enabled = false;
                break;
            case CameraState.BattleOverview:
                BattleOverviewCamera.enabled = true;
                EnemyTargetCamera.enabled = false;
                MercenaryTargetCamera.enabled = false;
                break;
            case CameraState.EnemyTarget:
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.enabled = true;
                MercenaryTargetCamera.enabled = false;
                break;
            case CameraState.MercenaryTarget:
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.enabled = false;
                MercenaryTargetCamera.enabled = true;
                break;
        }
    }
}
