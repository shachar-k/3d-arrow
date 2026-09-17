using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, IInputManager
{
    #region DataMembers
    private Dictionary<eInput, InputTypeValue> _inputValues = new Dictionary<eInput, InputTypeValue>();
    private Dictionary<eInputMap, InputActionMap> _inputMaps = new Dictionary<eInputMap, InputActionMap>();

    #endregion

    public T GetInputValue<T>(eInput inputType)
    {
        try
        {
            return ((InputTypeValue<T>)_inputValues[inputType]).Value;
        }
        catch (Exception e)
        {
            Debug.LogError($"wrong input type for {inputType} or input not found. Exception: {e.Message}");
            return default(T);

        }
    }


    public void enableMap(eInputMap inputMap)
    {
        _inputMaps[inputMap].Enable();
    }

    public void disableMap(eInputMap inputMap)
    {
        _inputMaps[inputMap].Disable();
    }

    void Start()
    {
        _inputMaps[eInputMap.Player] = InputSystem.actions.FindActionMap(Consts.PlayerInputMap, throwIfNotFound: true);
    }

    private void Awake()
    {
        GetReferences();

    }

    private void GetReferences()
    {
        // Project-wide actions, which the Input System enables on entering play mode.
        var player = _inputMaps[eInputMap.Player];

        this._inputValues[eInput.playerMoveInput] = new InputTypeValue<float>()
        {
            Action = player.FindAction(Consts.MoveAction, throwIfNotFound: true)
        };
    }

    // Update is called once per frame
    void Update()
    {
        var floatValueInput = _inputValues.Where(x => x.Value.Type == typeof(float)).ToList();

        foreach (var input in floatValueInput)
        {
            InputTypeValue<float> value = input.Value as InputTypeValue<float>;
            value.Value = input.Value.Action.ReadValue<float>();
        }
    }


}
