using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MercPanel : MonoBehaviour
{
    [Header("OBJECT REFS")]
    public BattleCharacter RepresentedCharacter;
    public Slider Health;
    public Slider Poise;
    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI PoiseText;

    private void Update()
    {
        HandleSliders();
    }

    private void HandleSliders()
    {
        if (RepresentedCharacter == null) return;
        Health.value = RepresentedCharacter.Health.CurrentValue / RepresentedCharacter.Health.MaxValue;
        Poise.value = RepresentedCharacter.Poise.DamageValue / RepresentedCharacter.Poise.MaxValue;
        HealthText.text = RepresentedCharacter.Health.CurrentValue.ToString();
        PoiseText.text = RepresentedCharacter.Poise.DamageValue.ToString();
    }
}
