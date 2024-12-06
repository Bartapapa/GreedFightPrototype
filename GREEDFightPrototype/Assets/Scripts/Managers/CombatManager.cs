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
    public GuardReticle GuardReticleUI;

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
        switch (ai.CurrentBattlePosition)
        {
            case BattlePosition.Frontline:
                AIUseAbility(ai, ai.PrimaryAbility_Frontline);
                break;
            case BattlePosition.Backline:
                AIUseAbility(ai, ai.PrimaryAbility_Backline);
                break;
        }
    }

    private void AIUseAbility(BattleCharacter ai, AbilityDescription ability)
    {
        List<BattleCharacter> aiPotentialTargets = GetPotentialTargets(ability);
        if (aiPotentialTargets.Count == 0) return;

        int randomIndex = UnityEngine.Random.Range(0, aiPotentialTargets.Count);
        DealDamage(ai, ability, aiPotentialTargets[randomIndex]);
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
        if (ability.GuardAbility)
        {
            GuardReticleUI.gameObject.SetActive(true);
            GuardReticleUI.Populate(character);
        }
        else
        {
            TargetReticleUI.gameObject.SetActive(true);
            TargetReticleUI.Populate(character, CurrentSelectedCharacter, ability);
        }
    }

    public void ConfirmUseAbilityOnTarget(BattleCharacter attacker, AbilityDescription ability, BattleCharacter target)
    {
        CurrentSelectedCharacter.CurrentRPM = 0f;
        attacker.Guard(false);

        DealDamage(attacker, ability, target);

        CloseTargetReticle();
        Player.instance.SwitchActionMap("BattleOverview");
        CurrentState = CombatState.BattleOverview;
        CameraManager.instance.EnableCamera(CameraState.BattleOverview);
        ToggleSelectCircle(true);
        MoveSelectCircleTo(CurrentSelectedCharacter.transform);
        TimerManager.instance.SetTimeScale(1f);
        GameManager.instance.CloseActionSelect();
    }

    public void ConfirmGuardOnSelf()
    {
        CurrentSelectedCharacter.CurrentRPM = 0f;
        CurrentSelectedCharacter.Guard(true);

        CloseGuardReticle();
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

    private void CloseGuardReticle()
    {
        GuardReticleUI.gameObject.SetActive(false);
    }

    private void DealDamage(BattleCharacter attacker, AbilityDescription ability, BattleCharacter target)
    {
        int fleshDamage = Damage.GetDamage(Damage.CalculateMinMaxFleshDamage(attacker, ability, target));
        int stanceDamage = Damage.GetDamage(Damage.CalculateMinMaxStanceDamage(attacker, ability, target));

        if (ability.Heal)
        {
            target.Health.Heal(fleshDamage);
            target.Poise.Heal(stanceDamage);
        }
        else
        {
            if (target.GuardBroken)
            {
                target.GuardBroken = false;
            }

            target.Health.Damage(fleshDamage);
            target.Poise.Damage(stanceDamage);

            if (IsEnemy(target))
            {
                float generatedFuel = Damage.CalculateGeneratedFuel(fleshDamage, ability);
                Player.instance.CurrentFuel += generatedFuel;
                if (Player.instance.CurrentFuel > GameManager.instance.UniversalVariables.MaxFuelAmount)
                {
                    Player.instance.CurrentFuel = GameManager.instance.UniversalVariables.MaxFuelAmount;
                }
            }
        }
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
                    if (ability.AbilityRange == AbilityRange.Melee)
                    {
                        List<BattleCharacter> meleeTargets = new List<BattleCharacter>();
                        foreach(BattleCharacter character in potentialTargets)
                        {
                            if (character.CurrentBattlePosition == BattlePosition.Frontline)
                            {
                                meleeTargets.Add(character);
                            }
                        }
                        potentialTargets = meleeTargets;
                    }
                }
                break;
            case TargetTypeCapability.Enemy:
                foreach (BattleCharacter enemy in Enemies)
                {
                    potentialTargets.Add(enemy);
                    if (ability.AbilityRange == AbilityRange.Melee)
                    {
                        List<BattleCharacter> meleeTargets = new List<BattleCharacter>();
                        foreach (BattleCharacter character in potentialTargets)
                        {
                            if (character.CurrentBattlePosition == BattlePosition.Frontline)
                            {
                                meleeTargets.Add(character);
                            }
                        }
                        potentialTargets = meleeTargets;
                    }
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
        CloseGuardReticle();
    }
    #endregion
}
