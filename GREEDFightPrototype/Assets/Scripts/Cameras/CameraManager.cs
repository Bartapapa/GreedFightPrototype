using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    None,
    BattleOverview,
    ActionSelect,
    SelfTarget,
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
    public TargetingCamera EnemyTargetCamera;
    public TargetingCamera MercenaryTargetCamera;

    [Header("UI")]
    public InputTutorials tutorials;
    public GameObject Panels;


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
        CurrentState = state;
        PurgeFocusCharacter();

        switch (state)
        {
            case CameraState.None:
                tutorials.ActionSelect(false);
                tutorials.Targeting(false);
                tutorials.BattleOverview(false);
                Panels.SetActive(false);
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.VirtualCam.enabled = false;
                MercenaryTargetCamera.VirtualCam.enabled = false;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(false);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
                break;
            case CameraState.BattleOverview:
                tutorials.ActionSelect(false);
                tutorials.Targeting(false);
                tutorials.BattleOverview(true);
                Panels.SetActive(true);
                BattleOverviewCamera.enabled = true;
                EnemyTargetCamera.VirtualCam.enabled = false;
                MercenaryTargetCamera.VirtualCam.enabled = false;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(false);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
                break;
            case CameraState.ActionSelect:
                tutorials.ActionSelect(true);
                tutorials.Targeting(false);
                tutorials.BattleOverview(false);
                Panels.SetActive(false);
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.VirtualCam.enabled = false;
                MercenaryTargetCamera.VirtualCam.enabled = false;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(false);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(true);
                break;
            case CameraState.SelfTarget:
                tutorials.ActionSelect(false);
                tutorials.Targeting(true);
                tutorials.BattleOverview(false);
                Panels.SetActive(false);
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.VirtualCam.enabled = false;
                MercenaryTargetCamera.VirtualCam.enabled = false;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(true);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
                break;
            case CameraState.EnemyTarget:
                tutorials.ActionSelect(false);
                tutorials.Targeting(true);
                tutorials.BattleOverview(false);
                Panels.SetActive(false);
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.VirtualCam.enabled = true;
                MercenaryTargetCamera.VirtualCam.enabled = false;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(false);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
                break;
            case CameraState.MercenaryTarget:
                tutorials.ActionSelect(false);
                tutorials.Targeting(true);
                tutorials.BattleOverview(false);
                Panels.SetActive(false);
                BattleOverviewCamera.enabled = false;
                EnemyTargetCamera.VirtualCam.enabled = false;
                MercenaryTargetCamera.VirtualCam.enabled = true;
                CombatManager.instance.CurrentSelectedCharacter.TargetSelf(false);
                CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
                break;
        }
    }

    public void PurgeFocusCharacter()
    {
        EnemyTargetCamera.CharacterFocus = null;
        MercenaryTargetCamera.CharacterFocus = null;
    }

    public void FocusCharacter(BattleCharacter character)
    {
        switch (CurrentState)
        {
            case CameraState.None:
                break;
            case CameraState.BattleOverview:
                break;
            case CameraState.ActionSelect:
                break;
            case CameraState.SelfTarget:
                break;
            case CameraState.EnemyTarget:
                EnemyTargetCamera.CharacterFocus = character;
                break;
            case CameraState.MercenaryTarget:
                MercenaryTargetCamera.CharacterFocus = character;
                break;
            default:
                break;
        }
    }
}
