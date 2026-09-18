public interface IInputManager
{
    #region Methods
        public T GetInputValue<T>(eInput inputType);
        public void EnableMap(eInputMap inputMap);
        public void DisableMap(eInputMap inputMap);
    #endregion
}