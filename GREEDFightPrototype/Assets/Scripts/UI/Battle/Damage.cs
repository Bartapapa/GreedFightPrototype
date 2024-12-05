using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage
{
    public static int GetDamage(Vector2Int minMaxDamage)
    {
        return UnityEngine.Random.Range(minMaxDamage.x, minMaxDamage.y+1);
    }

    public static Vector2Int CalculateMinMaxFleshDamage(BattleCharacter attacker, AbilityDescription abilityUsed, BattleCharacter target)
    {
        float armor = 0f;
        switch (abilityUsed.DMGType)
        {
            case DamageType.None:
                armor = 0f;
                break;
            case DamageType.Phys:
                armor = target.PhysArmor.CurrentValue;
                break;
            case DamageType.Shok:
                armor = 0f;
                break;
            case DamageType.Fir:
                armor = target.FirArmor.CurrentValue;
                break;
            case DamageType.Tox:
                armor = 0f;
                break;
        }

        int minDamage = Mathf.FloorToInt(
            (abilityUsed.BaseMinMaxFleshDamage.x + ((abilityUsed.BaseMinMaxFleshDamage.x * .2f) * attacker.CurrentPowerLevel - 1))
            * (1 / ((armor/8)+1))
            );
        int maxDamage = Mathf.FloorToInt(
            (abilityUsed.BaseMinMaxFleshDamage.y + ((abilityUsed.BaseMinMaxFleshDamage.y * .2f) * attacker.CurrentPowerLevel - 1))
            * (1 / ((armor / 8) + 1))
            );

        int guardBrokenDamage = 0;
        if (target.GuardBroken)
        {
            guardBrokenDamage = maxDamage;
        }

        return new Vector2Int(minDamage+guardBrokenDamage, maxDamage+guardBrokenDamage);
    }

    public static Vector2Int CalculateMinMaxStanceDamage(BattleCharacter attacker, AbilityDescription abilityUsed, BattleCharacter target)
    {
        float armor = 0f;
        switch (abilityUsed.DMGType)
        {
            case DamageType.None:
                armor = 0f;
                break;
            case DamageType.Phys:
                armor = target.PhysArmor.CurrentValue;
                break;
            case DamageType.Shok:
                armor = 0f;
                break;
            case DamageType.Fir:
                armor = target.FirArmor.CurrentValue;
                break;
            case DamageType.Tox:
                armor = 0f;
                break;
        }

        int minDamage = Mathf.FloorToInt(
            (abilityUsed.BaseMinMaxStanceDamage.x + ((abilityUsed.BaseMinMaxStanceDamage.x * .2f) * attacker.CurrentPowerLevel - 1))
            * (1 / ((armor / 8) + 1))
            );
        int maxDamage = Mathf.FloorToInt(
            (abilityUsed.BaseMinMaxStanceDamage.y + ((abilityUsed.BaseMinMaxStanceDamage.y * .2f) * attacker.CurrentPowerLevel - 1))
            * (1 / ((armor / 8) + 1))
            );
        return new Vector2Int(minDamage, maxDamage);
    }

    public static float CalculateGeneratedFuel(int damage, AbilityDescription ability)
    {
        float generatedFuel = (GameManager.instance.UniversalVariables.BaseFuelGain +
            ((GameManager.instance.UniversalVariables.BaseMaxFuelGain - GameManager.instance.UniversalVariables.BaseFuelGain)
            * (1 - (1 / ((damage/3)+1)))))
            * ability.FuelConversionRate;

        return generatedFuel;
    }
}
