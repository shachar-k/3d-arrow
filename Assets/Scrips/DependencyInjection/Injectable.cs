using UnityEngine;

public class Injectable<T, I>: MonoBehaviour 
    where T : MonoBehaviour 
    where I : class
{
    private IContainerService Container => ContainerService.Instance;

    void Start()
    {
        this.Container.Register<T, I>(this as T);
    }
    
}
