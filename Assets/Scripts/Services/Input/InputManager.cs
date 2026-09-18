using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, IInputManager, IInitializble
{
    #region DataMembers
    private Dictionary<eInput, InputTypeValue> _inputValues = new Dictionary<eInput, InputTypeValue>();
    private Dictionary<eInputMap, InputActionMap> _inputMaps = new Dictionary<eInputMap, InputActionMap>();
    private InputActionAsset _inputAsset;

    #endregion

    #region Methods

    public void Initialize()
    {
        if (this._inputAsset == null)
        {
            this._inputAsset = InputSystem.actions;

            if (this._inputAsset == null)
            {
                throw new Exception("No InputActionAsset is configured for the project. Assign the default Input Actions asset or add a PlayerInput component with actions.");
            }
        }

        if (!this._inputMaps.ContainsKey(eInputMap.Player))
        {
            this._inputMaps[eInputMap.Player] = this._inputAsset.FindActionMap(Consts.PlayerInputMap, throwIfNotFound: true);
        }

        this.GetReferences();
    }

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

    public void EnableMap(eInputMap inputMap)
    {
        this._inputMaps[inputMap].Enable();
    }

    public void DisableMap(eInputMap inputMap)
    {
        this._inputMaps[inputMap].Disable();
    }

    // Update is called once per frame
    void Update()
    {
        var floatValueInput = this._inputValues.Where(x => x.Value.Type == typeof(float)).ToList();

        foreach (var input in floatValueInput)
        {
            (input.Value as InputTypeValue<float>).Value = GetNumberValue<float>(input.Value);
        }
    }

    private static T GetNumberValue<T>(InputTypeValue input) where T : struct
    {
        return input.Action.ReadValue<T>();
    }

    private void GetReferences()
    {
        // Project-wide actions, which the Input System enables on entering play mode.
        var player = this._inputMaps[eInputMap.Player];

        this._inputValues[eInput.playerMoveInput] = new InputTypeValue<float>()
        {
            Action = player.FindAction(Consts.MoveAction, throwIfNotFound: true)
        };
    }

    #endregion
}
