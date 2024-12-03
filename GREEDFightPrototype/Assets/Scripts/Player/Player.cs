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
    public float CurrentFuel = 0f;
    public float CurrentRefinedFuel = 0f;

    [Header("REVVING")]
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

    private void Start()
    {
        CurrentFuel = GameManager.instance.UniversalVariables.InitialFuel;
        CurrentGear = GameManager.instance.UniversalVariables.InitialGear;
        CurrentRefinedFuel = GameManager.instance.UniversalVariables.InitialRefinedFuel;
    }

    private void Update()
    {
        HandleRevEngineFuelValue();
        HandleRefineFuelValue();
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
                    TimerManager.instance.PauseTimers();
                    GameManager.instance.ShowActionSelect(CombatManager.instance.CurrentSelectedCharacter);
                }
            }       
        }
    }

    private void HandleRevEngineFuelValue()
    {
        if (CurrentFuel <= 0) return;
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
                CurrentFuel -= RPMincrease * .5f;
                if (CurrentFuel < 0)
                {
                    CurrentFuel = 0f;
                }
            }
        }
    }

    private void HandleRefineFuelValue()
    {
        if (CurrentFuel <= 0 || CurrentGear == 5) return;
        float refinedFuelIncrease = GameManager.instance.UniversalVariables.BasePowerLevelIncreaseThreshold * _refineFuelValue * Time.deltaTime;
        CurrentRefinedFuel += refinedFuelIncrease;
        float refinedFuelThreshold = 9999f;
        if (CurrentGear == 1)
        {
            refinedFuelThreshold = GameManager.instance.UniversalVariables.Gear2FuelThreshold;
        }
        else if (CurrentGear == 2)
        {
            refinedFuelThreshold = GameManager.instance.UniversalVariables.Gear3FuelThreshold;
        }
        else if (CurrentGear == 3)
        {
            refinedFuelThreshold = GameManager.instance.UniversalVariables.Gear4FuelThreshold;
        }
        else if (CurrentGear == 4)
        {
            refinedFuelThreshold = GameManager.instance.UniversalVariables.Gear5FuelThreshold;
        }
        
        if(CurrentRefinedFuel >= refinedFuelThreshold)
        {
            CurrentGear++;
            CurrentRefinedFuel = 0;
        }

        CurrentFuel -= refinedFuelIncrease;
        if (CurrentFuel < 0)
        {
            CurrentFuel = 0f;
        }
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

                if (_primaryActionRequested)
                {
                    
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
                    Debug.Log("SECONDARY ACTION");
                }
                else if (_tertiaryActionRequested)
                {
                    Debug.Log("TERTIARY ACTION");
                }
                else if (_corruptionActionRequested)
                {
                    Debug.Log("CORRUPTION ACTION");
                }
                else if (_analyzeActionRequested)
                {

                }
                else if (_guardActionRequested)
                {
                    Debug.Log("GUARD ACTION");
                }
            }
        }
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.ActionSelectUI.PrimaryAbility.ConfirmAbilityUse(true);
            //_actionSelectButtonPressed = true;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = true;
        }

        if (context.canceled)
        {
            GameManager.instance.ActionSelectUI.PrimaryAbility.ConfirmAbilityUse(false);
            //_actionSelectButtonPressed = false;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = false;
        }
    }

    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.ActionSelectUI.SecondaryAbility.ConfirmAbilityUse(true);
            //_actionSelectButtonPressed = true;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = true;
        }

        if (context.canceled)
        {
            GameManager.instance.ActionSelectUI.SecondaryAbility.ConfirmAbilityUse(false);
            //_actionSelectButtonPressed = false;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = false;
        }
    }

    public void OnTertiaryAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.ActionSelectUI.TertiaryAbility.ConfirmAbilityUse(true);
            //_actionSelectButtonPressed = true;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = true;
        }

        if (context.canceled)
        {
            GameManager.instance.ActionSelectUI.TertiaryAbility.ConfirmAbilityUse(false);
            //_actionSelectButtonPressed = false;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = false;
        }
    }

    public void OnCorruptionAction(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.instance.ActionSelectUI.CorruptionAbility.ConfirmAbilityUse(true);
            //_actionSelectButtonPressed = true;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = true;
        }

        if (context.canceled)
        {
            GameManager.instance.ActionSelectUI.CorruptionAbility.ConfirmAbilityUse(false);
            //_actionSelectButtonPressed = false;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = false;
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
            GameManager.instance.ActionSelectUI.GuardAbility.ConfirmAbilityUse(true);
            //_actionSelectButtonPressed = true;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = true;
        }

        if (context.canceled)
        {
            GameManager.instance.ActionSelectUI.GuardAbility.ConfirmAbilityUse(false);
            //_actionSelectButtonPressed = false;
            //_actionSelectedConfirmationTimer = 0f;

            //_primaryActionRequested = false;
        }
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            SwitchActionMap("BattleOverview");
            CombatManager.instance.CurrentSelectedCharacter.SelectCharacterForActionSelect(false);
            TimerManager.instance.SetTimeScale(1f);
            GameManager.instance.CloseActionSelect();
        }
    }

    #endregion
}
