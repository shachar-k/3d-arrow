using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContainerService : Singleton<ContainerService>, IContainerService, IInitializble
{
    #region DataMembers
    Dictionary<System.Type, MonoBehaviour> _instances = new Dictionary<System.Type, MonoBehaviour>();

    Dictionary<System.Type, ScriptableObject> _scriptableInstances = new Dictionary<System.Type, ScriptableObject>();


    #endregion

    #region Methods
    public void Initialize()
    {
        this.Register<ContainerService, IContainerService>(this);
        this.Register<InputManager, IInputManager>(InputManager.CreateInstance(this.gameObject));
        
    }

    public void Register<T, I>(T instance)
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

    public void RegisterScriptable<T>(T instance)
       where T : ScriptableObject
    {
        if (_scriptableInstances.ContainsKey(typeof(T)))
        {
            Debug.LogWarning($"Type {typeof(T)} is already registered.");
            return;
        }

        _scriptableInstances.Add(typeof(T), instance);
    }

    public T Resolve<T>() where T : class
    {

        if (_instances.ContainsKey(typeof(T)))
        {
            return _instances[typeof(T)] as T;
        }
        else if (_scriptableInstances.ContainsKey(typeof(T)))
        {
            return _scriptableInstances[typeof(T)] as T;
        }

        throw new System.Exception($"Type {typeof(T)} is not registered in the container.");
    }

    #endregion

}
