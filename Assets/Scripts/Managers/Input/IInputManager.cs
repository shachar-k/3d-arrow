using UnityEngine.InputSystem;

public interface IInputManager
{
    #region Methods
        public T GetInputValue<T>(eInput inputType);
        public InputAction GetAction(eInput input);
        public void EnableMap(eInputMap inputMap);
        public void DisableMap(eInputMap inputMap);
    #endregion
}