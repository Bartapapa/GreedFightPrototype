using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public static Vector2Int CalculateMinMaxDamage(BattleCharacter attacker, AbilityDescription ability, BattleCharacter target)
    {
        return new Vector2Int(0, 0);
    }

    public static float CalculateGeneratedFuel(int damage, AbilityDescription ability)
    {
        return 0f;
    }
}
