public interface IInputManager
{
    #region Methods
        public T GetInputValue<T>(eInput inputType);
        public void enableMap(eInputMap inputMap);
        public void disableMap(eInputMap inputMap);
    #endregion
}