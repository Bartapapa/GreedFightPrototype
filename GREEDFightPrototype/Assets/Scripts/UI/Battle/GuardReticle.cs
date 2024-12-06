using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuardReticle : MonoBehaviour
{
    [Header("RECT TRANSFORM")]
    public RectTransform Self;
    public RectTransform CanvasRect;

    [Header("DATA")]
    public BattleCharacter Target;

    [Header("OBJECT REFS")]
    public Image ReticleFill;
    public Image StanceFill;
    public TextMeshProUGUI CurrentPoiseText;
    public TextMeshProUGUI PotentialStanceDamageText;

    [Header("RETICLE FILL CURVE")]
    public AnimationCurve FillCurve;

    private bool _targetConfirm = false;
    private float _targetConfirmFillAmount = 0f;

    public void ConfirmTarget(bool confirm)
    {
        _targetConfirm = confirm;
    }

    public void Populate(BattleCharacter guarder)
    {
        Target = guarder;

        StanceFill.fillAmount = guarder.Poise.DamageValue / guarder.Poise.MaxValue;
        CurrentPoiseText.text = Mathf.RoundToInt(guarder.Poise.DamageValue).ToString();

        int StanceHeal = Mathf.FloorToInt(guarder.Poise.DamageValue);
        PotentialStanceDamageText.text = "-" + StanceHeal.ToString();
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
                CombatManager.instance.ConfirmGuardOnSelf();
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
