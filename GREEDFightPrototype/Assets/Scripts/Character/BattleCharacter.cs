using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattlePosition
{
    None,
    Frontline,
    Backline,
}
public class BattleCharacter : MonoBehaviour
{
    [Header("CAMERA OFFSET")]
    public float VerticalCameraOffset = 1f;

    [Header("BASE STATS")]
    public CharacterStat Health;
    public CharacterStat Poise;
    public CharacterStat PhysArmor;
    public CharacterStat FirArmor;
    public CharacterStat HorsePower;
    public CharacterStat AutomaticRPMGainFactor;
    [ReadOnlyInspector] public bool IsGuarding = false;
    [ReadOnlyInspector] public bool GuardBroken = false;

    [Header("CORRUPTION ABILITY")]
    public AbilityDescription CorruptionAbility;

    [Header("FRONTLINE ABILITIES")]
    public AbilityDescription PrimaryAbility_Frontline;
    public AbilityDescription SecondaryAbility_Frontline;
    public AbilityDescription TertiaryAbility_Frontline;

    [Header("BACKLINE ABILITIES")]
    public AbilityDescription PrimaryAbility_Backline;
    public AbilityDescription SecondaryAbility_Backline;
    public AbilityDescription TertiaryAbility_Backline;

    [Header("RPM")]
    [ReadOnlyInspector] public float CurrentRPM = 0f;
    public bool ReadyToAttack { get { return CurrentRPM >= 1000f ? true : false; } }

    [Header("POSITION")]
    public BattlePosition StartingBattlePosition = BattlePosition.Frontline;
    [ReadOnlyInspector] public BattlePosition CurrentBattlePosition = BattlePosition.None;
    public float MoveToPositionDuration = 1f;
    public AnimationCurve MoveToPositionCurve;
    public bool IsMovingToPosition { get { return _moveToPositionCo != null; } }

    public int CurrentPowerLevel { get { return GetCurrentPowerLevel(); } }

    private Coroutine _moveToPositionCo;
    private Vector3 _backlinePos;
    private Vector3 _frontlinePos;

    private void Start()
    {
        InitializePositions();
        ForceMoveToPosition(StartingBattlePosition);

        Health.CurrentValueReachedZero -= OnHealthReachedZero;
        Health.CurrentValueReachedZero += OnHealthReachedZero;

        Poise.CurrentValueReachedZero -= OnPoiseDamageMaxed;
        Poise.CurrentValueReachedZero += OnPoiseDamageMaxed;
    }

    private void OnHealthReachedZero()
    {
        Debug.LogWarning(this.gameObject.name + " died!");
        if (CombatManager.instance.IsMercenary(this))
        {
            CombatManager.instance.Mercenaries.Remove(this);
        }
        else
        {
            CombatManager.instance.Enemies.Remove(this);
        }

        CombatManager.instance.Characters.Remove(this);
        Destroy(this.gameObject);
    }

    private void OnPoiseDamageMaxed()
    {
        Stun();
    }

    private void Stun()
    {
        Debug.LogWarning(this.gameObject.name + " got stunned!");

        CurrentRPM = 0f;
        GuardBroken = true;
        Poise.HealToMaxValue();
    }

    protected virtual void Update()
    {
        HandleRPM();
    }

    #region Handlers
    private void HandleRPM()
    {
        if (!ReadyToAttack)
        {
            CurrentRPM += ((GameManager.instance.UniversalVariables.BasePowerLevelIncreaseThreshold * (1 - (1 / ((HorsePower.CurrentValue / 10f) + 1))) * AutomaticRPMGainFactor.CurrentValue) * Time.deltaTime * TimerManager.TimeScale);
            if (CurrentRPM >= 1000f)
            {
                CurrentRPM = 1000f;
                GuardBroken = false;
            }
        }
    }
    #endregion

    private void InitializePositions()
    {
        _backlinePos = transform.position;
        _frontlinePos = _backlinePos + (3f * transform.forward);
    }

    private void SetCurrentPosition(BattlePosition position)
    {
        CurrentBattlePosition = position;
        switch (position)
        {
            case BattlePosition.None:
                break;
            case BattlePosition.Frontline:
                StatModifier frontlineRPMGainModifier = new StatModifier(AutomaticRPMGainFactor, .2f, StatModifierType.PercentageMultiply, "frontlineRPMGainModifier");
                AutomaticRPMGainFactor.AddModifier(frontlineRPMGainModifier);
                break;
            case BattlePosition.Backline:
                AutomaticRPMGainFactor.RemoveModifierOfId("frontlineRPMGainModifier");
                break;
        }
    }

    public void MoveToPosition(BattlePosition position)
    {
        if (position == CurrentBattlePosition) return;

        switch (position)
        {
            case BattlePosition.None:
                break;
            case BattlePosition.Frontline:
                StopMoveToPosition();
                _moveToPositionCo = StartCoroutine(MoveToPositionCoroutine(_frontlinePos));
                break;
            case BattlePosition.Backline:
                if (!CombatManager.instance.CanSendToBackline(this)) return;
                StopMoveToPosition();
                _moveToPositionCo = StartCoroutine(MoveToPositionCoroutine(_backlinePos));
                break;
        }

        SetCurrentPosition(position);
    }

    public void ForceMoveToPosition(BattlePosition position)
    {
        StopMoveToPosition();

        switch (position)
        {
            case BattlePosition.None:
                break;
            case BattlePosition.Frontline:
                transform.position = _frontlinePos;
                break;
            case BattlePosition.Backline:
                transform.position = _backlinePos;
                break;
        }

        SetCurrentPosition(position);
    }

    private void StopMoveToPosition()
    {
        if (_moveToPositionCo != null)
        {
            StopCoroutine(_moveToPositionCo);
            _moveToPositionCo = null;
        }
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 position)
    {
        Vector3 fromPos = transform.position;
        Vector3 destinationPos = position;
        float timer = 0f;
        while (timer < MoveToPositionDuration)
        {
            timer += Time.deltaTime;
            Vector3 toPos = Vector3.Lerp(fromPos, destinationPos, MoveToPositionCurve.Evaluate(timer / MoveToPositionDuration));
            transform.position = toPos;
            yield return null;
        }
        transform.position = destinationPos;
        _moveToPositionCo = null;
    }

    public void IncreaseRPM(float increase)
    {
        CurrentRPM += increase;
        if (CurrentRPM > 3000f)
        {
            CurrentRPM = 3000f;
        }
    }

    private int GetCurrentPowerLevel()
    {
        int powerLevel = 0;
        int basePower = 1;
        if (CombatManager.instance.IsMercenary(this))
        {
            basePower = Player.instance.CurrentGear;
        }
        powerLevel = CurrentRPM >= 1000 ? basePower : 0;
        int powerLevelIncrease = (int)Mathf.Clamp(Mathf.Floor(((CurrentRPM - 1000f) / 1000f)), 0f, float.MaxValue);
        powerLevel += powerLevelIncrease;
        return powerLevel;
    }

    public virtual void SelectCharacterForActionSelect(bool select)
    {

    }

    public virtual void TargetSelf(bool target)
    {

    }

    public void Guard(bool up)
    {
        IsGuarding = up;
    }

    public Vector2Int CalculateMinMaxDamage(Vector2Int baseMinMaxDamage)
    {
        int minDamage = Mathf.FloorToInt(baseMinMaxDamage.x + ((baseMinMaxDamage.x * .2f) * CurrentPowerLevel-1));
        int maxDamage = Mathf.FloorToInt(baseMinMaxDamage.y + ((baseMinMaxDamage.y * .2f) * CurrentPowerLevel-1));
        return new Vector2Int(minDamage, maxDamage);
    }
}
