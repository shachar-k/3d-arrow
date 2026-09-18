using UnityEngine;

public class Containerble<T> : Singleton<Containerble<T>> where T : MonoBehaviour
{
    #region Name
        private IContainerService Container => ContainerService.Instance;
    #endregion
}
