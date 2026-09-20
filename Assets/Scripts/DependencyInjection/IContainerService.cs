using UnityEngine;

public interface IContainerService
{
    #region Methods

        public void Register<T,I>(T instance) where T : MonoBehaviour where I : class;

        public void RegisterScriptable<T>(T instance) where T : ScriptableObject;

        public T Resolve<T>() where T: class;

    #endregion
}
