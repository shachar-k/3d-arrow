using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    /// <summary>
    /// True when an instance already exists. Use this instead of a null-check on <see cref="Instance"/>
    /// while tearing down (OnDestroy / OnDisable): reading Instance would resurrect the singleton by
    /// creating a fresh GameObject, which then leaks into the next play session.
    /// </summary>
    public static bool HasInstance => _instance != null;

    public static T Instance { get; private set; }

    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        CreateInstance();
    }

    public static T CreateInstance(GameObject parent = null)
    {
        if (_instance != null)
        {
            return _instance;
        }

        _instance = parent != null ? parent.GetComponent<T>() : FindAnyObjectByType<T>();

        if (_instance == null)
        {
            if (parent == null)
            {
                parent = FindAnyObjectByType<MonoBehaviour>().gameObject;
            }

            _instance = parent.AddComponent<T>();
        }

        (_instance as IInitializble)?.Initialize();
        
        return _instance;
    }

    protected virtual void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}