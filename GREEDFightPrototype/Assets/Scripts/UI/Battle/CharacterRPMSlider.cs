using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterRPMSlider : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public Slider CharacterSlider;
    public BattleCharacter Character;
    public Canvas CanvasComp;

    [ReadOnlyInspector] public float CurrentValue = 0f;

    private void Update()
    {
        CharacterSlider.value = Character.CurrentRPM / GameManager.instance.UniversalVariables.MaxRPM;
        CurrentValue = CharacterSlider.value;
    }
}
