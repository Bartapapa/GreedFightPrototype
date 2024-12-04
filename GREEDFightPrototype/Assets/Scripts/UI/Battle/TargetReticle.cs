using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetReticle : MonoBehaviour
{
    [Header("RECT TRANSFORM")]
    public RectTransform Self;
    public RectTransform CanvasRect;

    [Header("OBJECT REFS")]
    public BattleCharacter Target;
    public BattleCharacter Attacker;
    public AbilityDescription AbilityUsed;

    [Header("OBJECT REFS")]
    public Image ReticleFill;
    public Image HealthFill;
    public Image StanceFill;
    public TextMeshProUGUI CurrentHealthText;
    public TextMeshProUGUI CurrentPoiseText;
    public TextMeshProUGUI PotentialFleshDamageText;
    public TextMeshProUGUI PotentialStanceDamageText;

    [Header("RETICLE FILL CURVE")]
    public AnimationCurve FillCurve;

    private bool _targetConfirm = false;
    private float _targetConfirmFillAmount = 0f;

    public void ConfirmTarget(bool confirm)
    {
        _targetConfirm = confirm;
    }

    public void Populate(BattleCharacter target, BattleCharacter attacker, AbilityDescription abilityUsed)
    {
        Target = target;
        Attacker = attacker;
        AbilityUsed = abilityUsed;

        HealthFill.fillAmount = target.Health.CurrentValue / target.Health.MaxValue;
        StanceFill.fillAmount = target.Poise.DamageValue / target.Poise.MaxValue;
        CurrentHealthText.text = Mathf.RoundToInt(target.Health.CurrentValue).ToString();
        CurrentPoiseText.text = Mathf.RoundToInt(target.Poise.DamageValue).ToString();

        string plusOrMinus = "";
        string minusOrPlus = "";
        if (abilityUsed.Heal)
        {
            plusOrMinus = "+";
            minusOrPlus = "-";
        }
        else
        {
            plusOrMinus = "-";
            minusOrPlus = "+";
        }
        Vector2Int minMaxFleshDamage = attacker.CalculateMinMaxDamage(abilityUsed.BaseMinMaxFleshDamage);
        Vector2Int minMaxStanceDamage = attacker.CalculateMinMaxDamage(abilityUsed.BaseMinMaxStanceDamage);
        PotentialFleshDamageText.text = plusOrMinus + minMaxFleshDamage.x.ToString() + "-" + minMaxFleshDamage.y.ToString() + " " + abilityUsed.DMGType.ToString();
        PotentialStanceDamageText.text = minusOrPlus + minMaxStanceDamage.x.ToString() + "-" + minMaxStanceDamage.y.ToString();
    }

    private void LateUpdate()
    {
        HandleUIPosition();
        HandleReticleFill();
    }

    private void HandleUIPosition()
    {
        if (Target)
        {
            Vector2 viewportPos = Camera.main.WorldToViewportPoint(Target.transform.position + (Vector3.up * Target.VerticalCameraOffset));
            Vector2 targetScreenPosition = new Vector2(
                ((viewportPos.x * CanvasRect.sizeDelta.x) - (CanvasRect.sizeDelta.x * .5f)),
                ((viewportPos.y * CanvasRect.sizeDelta.y) - (CanvasRect.sizeDelta.y * .5f)));
            Self.anchoredPosition = targetScreenPosition;
        }
    }

    private void HandleReticleFill()
    {
        if (_targetConfirm)
        {
            _targetConfirmFillAmount += Time.deltaTime;
            if (_targetConfirmFillAmount > 1f)
            {
                _targetConfirmFillAmount = 0f;
                CombatManager.instance.ConfirmUseAbilityOnTarget(Attacker, AbilityUsed, Target);
                //CombatManager.instance.ConfirmAbilityUse(AbilityDesc);
            }
        }
        else
        {
            _targetConfirmFillAmount -= Time.deltaTime;
            if (_targetConfirmFillAmount < 0f)
            {
                _targetConfirmFillAmount = 0f;
            }
        }

        ReticleFill.fillAmount = Mathf.Lerp(0f, 1f, FillCurve.Evaluate(_targetConfirmFillAmount));
    }
}
