using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BC_Enemy : BattleCharacter
{
    protected override void Update()
    {
        base.Update();
        if (CurrentRPM >= 1000)
        {
            CombatManager.instance.AIAct(this);
        }
    }
}
