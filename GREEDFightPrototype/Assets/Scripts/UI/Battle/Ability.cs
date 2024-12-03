using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ability : MonoBehaviour
{
    [Header("OBJECT REFERENCES")]
    public Image AbiltiyIcon;
    public TextMeshProUGUI AbilityName;
    public Image AbilityConfirmFill;

    [Header("CURVE")]
    public AnimationCurve FillCurve;

    private float _abilityConfirm = 0f;
    private bool _abilityConfirmUse = false;

    private void Update()
    {
        HandleAbilityConfirm();
    }

    private void HandleAbilityConfirm()
    {
        if (_abilityConfirmUse)
        {
            _abilityConfirm += Time.deltaTime;
            if (_abilityConfirm > 1f)
            {
                _abilityConfirm = 1f;
                //Confirm ability use
            }
        }
        else
        {
            _abilityConfirm -= Time.deltaTime;
            if (_abilityConfirm < 0f)
            {
                _abilityConfirm = 0f;
            }
        }

        AbilityConfirmFill.fillAmount = Mathf.Lerp(0f, 1f, FillCurve.Evaluate(_abilityConfirm));
    }

    public void PopulateAbility(AbilityDescription ability)
    {
        AbiltiyIcon.sprite = ability.Icon;
        AbilityName.text = ability.Name;
    }

    public void ConfirmAbilityUse(bool confirm)
    {
        _abilityConfirmUse = confirm;
    }
}
