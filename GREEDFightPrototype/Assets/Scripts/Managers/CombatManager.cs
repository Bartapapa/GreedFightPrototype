using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public static CombatManager instance;

    [Header("OBJECT REFS")]
    public Transform CharacterSelect;

    [Header("BATTLECHARACTERS")]
    [ReadOnlyInspector] public BattleCharacter CurrentSelectedCharacter;
    public List<BattleCharacter> Characters = new List<BattleCharacter>();
    public List<BattleCharacter> Mercenaries = new List<BattleCharacter>();
    public List<BattleCharacter> Enemies = new List<BattleCharacter>();
    private int _currentlySelectedCharacterIndex = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        SwitchSelectToCharacterIndex(_currentlySelectedCharacterIndex);
    }

    public bool IsMercenary(BattleCharacter character)
    {
        return Mercenaries.Contains(character);
    }

    public bool IsEnemy(BattleCharacter character)
    {
        return Enemies.Contains(character);
    }

    public bool CanSendToBackline(BattleCharacter character)
    {
        int numberOfFrontliners = 0;
        if (IsMercenary(character))
        {
            foreach(BattleCharacter merc in Mercenaries)
            {
                if (merc.CurrentBattlePosition == BattlePosition.Frontline)
                {
                    numberOfFrontliners++;
                }
            }
        }
        else
        {
            foreach (BattleCharacter enemy in Enemies)
            {
                if (enemy.CurrentBattlePosition == BattlePosition.Frontline)
                {
                    numberOfFrontliners++;
                }
            }
        }
        return numberOfFrontliners > 1;
    }

    public void SwitchSelectCharacter(float value)
    {
        int switchIndex = value > 0 ? 1 : -1;
        int toIndex = _currentlySelectedCharacterIndex + switchIndex;
        if (toIndex < 0)
        {
            SwitchSelectToCharacterIndex(Characters.Count - 1);
        }
        else if(toIndex >= Characters.Count)
        {
            SwitchSelectToCharacterIndex(0);
        }
        else
        {
            SwitchSelectToCharacterIndex(toIndex);
        }
    }

    private void SwitchSelectToCharacterIndex(int index)
    {
        _currentlySelectedCharacterIndex = index;
        CurrentSelectedCharacter = Characters[index];

        CharacterSelect.position = Characters[index].transform.position;
        CharacterSelect.parent = Characters[index].transform;
    }
}
