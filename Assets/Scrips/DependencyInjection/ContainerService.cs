using System.Collections.Generic;
using UnityEngine;

public class ContainerService : Singleton<ContainerService>, IContainerService
{
    #region DataMembers
        Dictionary<System.Type, MonoBehaviour> _instances = new Dictionary<System.Type, MonoBehaviour>();
    
    #endregion

    #region Methods

    public void Register<T,I>(T instance) 
        where T : MonoBehaviour 
        where I : class
    {
        _instances.Add(typeof(I), instance);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.Register<ContainerService, IContainerService>(this);
    }

    #endregion

}
