using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    None,
    Phys,
    Shok,
    Fir,
    Tox,
}

public enum TargetNumberCapability
{
    None,
    Single,
    Row,
}

public enum TargetTypeCapability
{
    Self,
    Ally,
    Enemy,
}

public enum AbilityRange
{
    Melee,
    Ranged,
}

public enum ConditionType
{
    Burning,
    Poisoned,
    Electrocuted,
    Energized,
    Resolute,
}

public enum EffectType
{
    Push,
    Pull,
    Fear,
    Blast,
}

[System.Serializable]
public class ConditionInfo
{
    public ConditionType Condition = ConditionType.Burning;
    public int TickDamage = 0;
    public float Duration = 1f;
}

[CreateAssetMenu(menuName = "Greed/Combat/Ability")]
public class AbilityDescription : ScriptableObject
{
    [Header("PARAMETERS")]
    public int AbilityID = -1;
    public string Name = "AbilityName";
    public Sprite Icon;
    public AbilityRange AbilityRange = AbilityRange.Melee;
    public TargetTypeCapability TargetType = TargetTypeCapability.Enemy;
    public TargetNumberCapability TargetNumber = TargetNumberCapability.Single;
    public Vector2Int BaseMinMaxFleshDamage;
    public Vector2Int BaseMinMaxStanceDamage;
    public DamageType DMGType = DamageType.Phys;
    public float FuelConversionRate = 1f;
    public bool Heal = false;
    public List<ConditionInfo> Conditions = new List<ConditionInfo>();
    public List<EffectType> Effects = new List<EffectType>();
}
