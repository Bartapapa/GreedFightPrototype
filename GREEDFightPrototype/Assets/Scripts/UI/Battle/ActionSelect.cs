using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelect : MonoBehaviour
{
    [Header("ABILITY ICONS")]
    public Ability PrimaryAbility;
    public Ability SecondaryAbility;
    public Ability TertiaryAbility;
    public Ability CorruptionAbility;
    public Ability GuardAbility;

    public void PopulateAbilities(BattleCharacter character)
    {
        switch (character.CurrentBattlePosition)
        {
            case BattlePosition.None:
                break;
            case BattlePosition.Frontline:
                PrimaryAbility.PopulateAbility(character.PrimaryAbility_Frontline);
                SecondaryAbility.PopulateAbility(character.SecondaryAbility_Frontline);
                TertiaryAbility.PopulateAbility(character.TertiaryAbility_Frontline);
                CorruptionAbility.PopulateAbility(character.CorruptionAbility);
                break;
            case BattlePosition.Backline:
                PrimaryAbility.PopulateAbility(character.PrimaryAbility_Backline);
                SecondaryAbility.PopulateAbility(character.SecondaryAbility_Backline);
                TertiaryAbility.PopulateAbility(character.TertiaryAbility_Backline);
                CorruptionAbility.PopulateAbility(character.CorruptionAbility);
                break;
        }
    }
}
