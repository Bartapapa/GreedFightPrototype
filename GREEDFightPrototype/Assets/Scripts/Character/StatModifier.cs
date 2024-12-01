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
    public float Duration;

    public StatModifier(float value, StatModifierType type, int order, float duration, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Duration = duration;
        Source = source;

        if (Duration > 0f)
        {
            Timer newTimer = new Timer(Duration);
        }
    }

    public StatModifier(float value, StatModifierType type) : this (value, type, (int)type, -1f, null) { }

    public StatModifier(float value, StatModifierType type, int order) : this (value, type, order, -1f, null) { }

    public StatModifier(float value, StatModifierType type, object source) : this (value, type, (int)type, -1f, source) { }

    public StatModifier(float value, StatModifierType type, float duration) : this(value, type, (int)type, duration, null) { }

    public StatModifier(float value, StatModifierType type, float duration, object source) : this(value, type, (int)type, duration, source) { }

}
