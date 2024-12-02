using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "JRPG/Game/UniversalVariables")]
public class UniversalVariables : ScriptableObject
{
    [Header("GAME VARIABLES")]
    public float BasePowerLevelIncreaseThreshold = 500f;
}
