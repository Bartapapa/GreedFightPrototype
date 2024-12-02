using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    private PlayerInput input;

    [Header("GEAR")]
    public int CurrentGear = 1;

    [Header("FUEL")]
    public float RevEngineMaxDuration = 1f;

    private float _revEngineValue;
    private float _revEngineDuration = 0f;
    private float _refineFuelValue;

    private float _actionSelectConfirmPressTime = 1f;
    private float _actionSelectedConfirmationTimer = 0f;
    private bool _actionSelectButtonPressed = false;

    private bool _primaryActionRequested = false;
    private bool _secondaryActionRequested = false;
    private bool _tertiaryActionRequested = false;
    private bool _corruptionActionRequested = false;
    private bool _guardActionRequested = false;
    private bool _analyzeActionRequested = false;

    public bool Revving { get { return _revEngineValue >= .05f; } }

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

        input = GetComponent<PlayerInput>();
    }

    private void Update()
    {
        HandleRevEngineFuelValue();
        HandleRefineFuelValue();

        HandleConfirmAction();
    }

    private void SwitchActionMap(string toActionMap)
    {
        input.SwitchCurrentActionMap(toActionMap);
    }

    #region BATTLEOVERVIEW
    public void OnSelectCharacter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _revEngineDuration = 0f;
            CombatManager.instance.SwitchSelectCharacter(context.ReadValue<float>());
            //float value = context.ReadValue<float>();
            //Debug.Log(value);
        }
    }

    public void OnPositionCharacter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CombatManager.instance.IsMercenary(CombatManager.instance.CurrentSelectedCharacter))
            {
                float value = context.ReadValue<float>();
                //Debug.Log(value);
                if (value > 0)
                {
                    CombatManager.instance.CurrentSelectedCharacter.MoveToPosition(BattlePosition.Frontline);
                }
                else
                {
                    CombatManager.instance.CurrentSelectedCharacter.MoveToPosition(BattlePosition.Backline);
                }
            }

        }
    }

    public void OnRevEngine(InputAction.CallbackContext context)
    {
        _revEngineValue = context.ReadValue<float>();      
    }

    public void OnRefineFuel(InputAction.CallbackContext context)
    {
        _refineFuelValue = context.ReadValue<float>();
    }

    public void OnConfirmCharacter(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (CombatManager.instance.IsMercenary(CombatManager.instance.CurrentSelectedCharacter))
            {
                if (CombatManager.instance.CurrentSelectedCharacter.ReadyToAttack)
                {
                    SwitchActionMap("ActionSelect");
                    CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(true);
                }
            }       
        }
    }

    private void HandleRevEngineFuelValue()
    {
        if (CombatManager.instance.IsMercenary(CombatManager.instance.CurrentSelectedCharacter))
        {
            if (!Revving)
            {
                _revEngineDuration = 0f;
            }
            else
            {
                _revEngineDuration += Time.deltaTime * TimerManager.TimeScale;
                if (_revEngineDuration > RevEngineMaxDuration) _revEngineDuration = RevEngineMaxDuration;
                float RPMincrease = (GameManager.instance.UniversalVariables.BasePowerLevelIncreaseThreshold*1.6f) * (Mathf.Pow((1 - (_revEngineDuration / RevEngineMaxDuration)), 2f)) * _revEngineValue * Time.deltaTime * TimerManager.TimeScale;
                CombatManager.instance.CurrentSelectedCharacter.IncreaseRPM(RPMincrease);
            }
        }
    }

    private void HandleRefineFuelValue()
    {
        //BasePowerLevelIncreaseThreshold * _refineFuelValue * Time.deltaTime;
    }

    #endregion
    #region ACTION SELECT

    private void HandleConfirmAction()
    {
        if (_actionSelectButtonPressed)
        {
            if (_actionSelectedConfirmationTimer < _actionSelectConfirmPressTime)
            {
                _actionSelectedConfirmationTimer += Time.deltaTime;
            }
            else
            {
                _actionSelectButtonPressed = false;
                _actionSelectedConfirmationTimer = 0f;

                if (_primaryActionRequested)
                {
                    Debug.Log("PRIMARY ACTION");
                }
                else if (_secondaryActionRequested)
                {

                }
                else if (_tertiaryActionRequested)
                {

                }
                else if (_corruptionActionRequested)
                {

                }
                else if (_analyzeActionRequested)
                {

                }
                else if (_guardActionRequested)
                {

                }
            }
        }
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _actionSelectButtonPressed = true;
            _actionSelectedConfirmationTimer = 0f;

            _primaryActionRequested = true;
        }

        if (context.canceled)
        {
            _actionSelectButtonPressed = false;
            _actionSelectedConfirmationTimer = 0f;

            _primaryActionRequested = false;
        }
    }

    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void OnTertiaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void OnCorruptionAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void OnSelectTarget(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();
        }
    }

    public void OnAnalyze(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void OnGuard(InputAction.CallbackContext context)
    {
        if (context.performed)
        {

        }
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchActionMap("BattleOverview");
            CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
        }
    }

    #endregion
}
