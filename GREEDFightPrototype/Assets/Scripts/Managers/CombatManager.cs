using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public enum CombatState
{
    None,
    BattleOverview,
    ActionSelect,
    Targeting
}

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [Header("OBJECT REFS")]
    public Transform CharacterSelect;
    public TargetReticle TargetReticleUI;

    [Header("STATE")]
    public CombatState StartingState = CombatState.BattleOverview;
    [ReadOnlyInspector] public CombatState CurrentState = CombatState.None;

    [Header("BATTLECHARACTERS")]
    [ReadOnlyInspector] public BattleCharacter CurrentSelectedCharacter;
    public List<BattleCharacter> Characters = new List<BattleCharacter>();
    public List<BattleCharacter> Mercenaries = new List<BattleCharacter>();
    public List<BattleCharacter> Enemies = new List<BattleCharacter>();
    private int _currentlySelectedCharacterIndex = 0;

    [Header("TARGETING")]
    [ReadOnlyInspector] public AbilityDescription CurrentlyTargetingAbility;
    [ReadOnlyInspector] public BattleCharacter CurrentTargetedCharacter;
    private int _currentlyTargetedCharacterIndex = 0;

    [Header("POTENTIAL TARGETS")]
    [ReadOnlyInspector] public List<BattleCharacter> PotentialTargets = new List<BattleCharacter>();

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
        SwitchSelectToCharacterIndex(_currentlySelectedCharacterIndex);
        CurrentState = StartingState;
    }

    public bool IsMercenary(BattleCharacter character)
    {
        return Mercenaries.Contains(character);
    }

    public bool IsEnemy(BattleCharacter character)
    {
        return Enemies.Contains(character);
    }

    public bool CanSendToBackline(BattleCharacter character)
    {
        int numberOfFrontliners = 0;
        if (IsMercenary(character))
        {
            foreach(BattleCharacter merc in Mercenaries)
            {
                if (merc.CurrentBattlePosition == BattlePosition.Frontline)
                {
                    numberOfFrontliners++;
                }
            }
        }
        else
        {
            foreach (BattleCharacter enemy in Enemies)
            {
                if (enemy.CurrentBattlePosition == BattlePosition.Frontline)
                {
                    numberOfFrontliners++;
                }
            }
        }
        return numberOfFrontliners > 1;
    }

    public void SwitchSelectCharacter(float value)
    {
        int switchIndex = value > 0 ? 1 : -1;
        int toIndex = _currentlySelectedCharacterIndex + switchIndex;
        if (toIndex < 0)
        {
            SwitchSelectToCharacterIndex(Characters.Count - 1);
        }
        else if(toIndex >= Characters.Count)
        {
            SwitchSelectToCharacterIndex(0);
        }
        else
        {
            SwitchSelectToCharacterIndex(toIndex);
        }
    }

    private void SwitchSelectToCharacterIndex(int index)
    {
        _currentlySelectedCharacterIndex = index;
        CurrentSelectedCharacter = Characters[index];

        MoveSelectCircleTo(Characters[index].transform);
    }

    public void ToggleSelectCircle(bool on)
    {
        CharacterSelect.gameObject.SetActive(on);
    }

    public void MoveSelectCircleTo(Transform parent)
    {
        CharacterSelect.position = parent.position;
        CharacterSelect.parent = parent;
    }

    #region AbilityUse
    public void ConfirmAbilityUse(AbilityDescription ability)
    {
        PotentialTargets = GetPotentialTargets(ability);
        if (PotentialTargets.Count == 0) return;

        //When ability is confirmed, move into
        CurrentState = CombatState.Targeting;
        GameManager.instance.CloseActionSelect();
        ToggleSelectCircle(true);
        CurrentlyTargetingAbility = ability;
        SetupCameraTargeting(CurrentlyTargetingAbility);
        SwitchTargetToCharacterIndex(0);
    }

    public void AIAct(BC_Enemy ai)
    {
        ai.CurrentRPM = 0f;
    }

    private void AIUseAbility(AbilityDescription ability)
    {

    }

    private void SetupCameraTargeting(AbilityDescription ability)
    {
        switch (ability.TargetType)
        {
            case TargetTypeCapability.Self:
                CameraManager.instance.EnableCamera(CameraState.SelfTarget);
                break;
            case TargetTypeCapability.Ally:
                CameraManager.instance.EnableCamera(CameraState.MercenaryTarget);
                break;
            case TargetTypeCapability.Enemy:
                CameraManager.instance.EnableCamera(CameraState.EnemyTarget);
                break;
        }
    }

    public void SwitchTargetCharacter(float value)
    {
        int switchIndex = value > 0 ? 1 : -1;
        int toIndex = _currentlyTargetedCharacterIndex + switchIndex;
        if (toIndex < 0)
        {
            SwitchTargetToCharacterIndex(PotentialTargets.Count - 1);
        }
        else if (toIndex >= PotentialTargets.Count)
        {
            SwitchTargetToCharacterIndex(0);
        }
        else
        {
            SwitchTargetToCharacterIndex(toIndex);
        }
    }

    private void SwitchTargetToCharacterIndex(int index)
    {
        _currentlyTargetedCharacterIndex = index;
        CurrentTargetedCharacter = PotentialTargets[index];

        ShowTargetReticle(CurrentTargetedCharacter, CurrentlyTargetingAbility);

        MoveSelectCircleTo(PotentialTargets[index].transform);
        CameraManager.instance.FocusCharacter(PotentialTargets[index]);
    }

    private void ShowTargetReticle(BattleCharacter character, AbilityDescription ability)
    {
        TargetReticleUI.gameObject.SetActive(true);
        TargetReticleUI.Populate(character, CurrentSelectedCharacter, ability);
    }

    public void ConfirmUseAbilityOnTarget(BattleCharacter attacker, AbilityDescription ability, BattleCharacter target)
    {
        CurrentSelectedCharacter.CurrentRPM = 0f;

        CloseTargetReticle();
        Player.instance.SwitchActionMap("BattleOverview");
        CurrentState = CombatState.BattleOverview;
        CameraManager.instance.EnableCamera(CameraState.BattleOverview);
        ToggleSelectCircle(true);
        MoveSelectCircleTo(CurrentSelectedCharacter.transform);
        TimerManager.instance.SetTimeScale(1f);
        GameManager.instance.CloseActionSelect();
    }

    private void CloseTargetReticle()
    {
        TargetReticleUI.gameObject.SetActive(false);
    }

    private List<BattleCharacter> GetPotentialTargets(AbilityDescription ability)
    {
        List<BattleCharacter> potentialTargets = new List<BattleCharacter>();

        switch (ability.TargetType)
        {
            case TargetTypeCapability.Self:
                potentialTargets.Add(CurrentSelectedCharacter);
                break;
            case TargetTypeCapability.Ally:
                foreach(BattleCharacter ally in Mercenaries)
                {
                    potentialTargets.Add(ally);
                }
                break;
            case TargetTypeCapability.Enemy:
                foreach (BattleCharacter enemy in Enemies)
                {
                    potentialTargets.Add(enemy);
                }
                break;
        }

        return potentialTargets;
    }

    public void CancelTargeting()
    {
        PotentialTargets.Clear();
        CurrentlyTargetingAbility = null;
        CameraManager.instance.EnableCamera(CameraState.ActionSelect);
        ToggleSelectCircle(false);
        CloseTargetReticle();
    }
    #endregion
}
