using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRPMSlider : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public Slider CharacterSlider;
    public BattleCharacter Character;

    private void Update()
    {
        CharacterSlider.value = Character.CurrentRPM / GameManager.instance.UniversalVariables.MaxRPM;
    }
}
