using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatModifierType
{
    Flat = 100,
    PercentageAdditive = 200,
    PercentageMultiply = 300,
}
public class StatModifier
{
    public StatModifierType Type;
    public float Value = 0f;
    public int Order;
    public object Source;
    public string ID;
    public float Duration;
    public CharacterStat CharacterStat;

    public StatModifier(CharacterStat stat, float value, StatModifierType type, int order, float duration, object source, string modID)
    {
        this.CharacterStat = stat;
        Value = value;
        Type = type;
        Order = order;
        Duration = duration;
        Source = source;
        ID = modID;

        if (Duration > 0f)
        {
            Timer newTimer = new Timer(Duration, () => this.CharacterStat.RemoveModifier(this));
        }
    }

    public StatModifier(CharacterStat stat, float value, StatModifierType type) : this (stat, value, type, (int)type, -1f, null, "") { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, string id) : this(stat, value, type, (int)type, -1f, null, id) { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, int order) : this (stat, value, type, order, -1f, null, "") { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, int order, string id) : this(stat, value, type, order, -1f, null, id) { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, object source) : this (stat, value, type, (int)type, -1f, source, "") { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, float duration) : this(stat, value, type, (int)type, duration, null, "") { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, float duration, string id) : this(stat, value, type, (int)type, duration, null, id) { }

    public StatModifier(CharacterStat stat, float value, StatModifierType type, float duration, object source) : this(stat, value, type, (int)type, duration, source, "") { }

}
