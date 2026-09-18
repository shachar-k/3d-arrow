using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerService : Singleton<ContainerService>, IContainerService, IInitializble
{
    #region DataMembers
        Dictionary<System.Type, MonoBehaviour> _instances = new Dictionary<System.Type, MonoBehaviour>();


    #endregion

    #region Methods
    public void Initialize()
    {
        this.Register<ContainerService, IContainerService>(this);
        this.Register<InputManager, IInputManager>(InputManager.CreateInstance(this.gameObject));
    }

    public void Register<T,I>(T instance) 
        where T : MonoBehaviour 
        where I : class
    {
        if (_instances.ContainsKey(typeof(I)))
        {
            Debug.LogWarning($"Type {typeof(I)} is already registered.");
            return;
        }
        
        _instances.Add(typeof(I), instance);
    }

    public T Resolve<T>() where T : class
    {
        return _instances[typeof(T)] as T;
    }

    #endregion

}
