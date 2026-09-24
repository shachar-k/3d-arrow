using UnityEngine;

/// <summary>
/// the singletons that have depnedencies
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="I"></typeparam>
public class Injectable<T, I>: MonoBehaviour 
    where T : MonoBehaviour 
    where I : class
{
    protected IContainerService Container  {get; private set; }

    protected virtual void Start()
    {
        this.Container = ContainerService.Instance ?? ContainerService.CreateInstance();
        this.Container.Register<T, I>(this as T);
    }

}
