using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/UniversalVariables")]
public class UniversalVariables : ScriptableObject
{
    [Header("GAME VARIABLES")]
    public float MaxRPM = 3000f;
    public float BasePowerLevelIncreaseThreshold = 500f;
    public float MaxFuelAmount = 2000f;
    public int MaxGear = 5;

    [Header("GEAR VALUES")]
    public float Gear2FuelThreshold = 1000f;
    public float Gear3FuelThreshold = 2000f;
    public float Gear4FuelThreshold = 3500f;
    public float Gear5FuelThreshold = 5000f;

    [Header("INITIAL VALUES")]
    public float InitialFuel = 500f;
    public float InitialRefinedFuel = 0f;
    public int InitialGear = 1;
    public int InitialHope = 1;
}
