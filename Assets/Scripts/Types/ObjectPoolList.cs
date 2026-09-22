using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class GameObjectPoolList 
{
    #region DataMembers

    [SerializeField]
    private List<ObjectParameters> prefabs = new List<ObjectParameters>();

    #endregion

    public GameObjectPoolList()
    {        
        foreach (var prefab in prefabs)
        {
            
        }
    }

    #region Properties

    public int CountInactive => throw new System.NotImplementedException();
    private Dictionary<string,ObjectPool<GameObject>> Pool { get; set; }

    #endregion

    #region Methods

    public void Clear()
    {
        throw new System.NotImplementedException();
    }

    public GameObject Get()
    {
        throw new System.NotImplementedException();
    }

    public PooledObject<GameObject> Get(out GameObject v)
    {
        throw new System.NotImplementedException();
    }

    public void Release(GameObject element)
    {
        throw new System.NotImplementedException();
    }

    private GameObject CreateObject(GameObject prefab)
    {
        GameObject newObject = GameObject.Instantiate(prefab);

        return newObject;
    }

    private void OnGetFromPool(GameObject pooledObj)
    {
        pooledObj.SetActive(true);
    }

     private void OnReturnToPool(GameObject pooledObj)
    {
        pooledObj.SetActive(false);
    }

    private void OnDestroy(GameObject pooledObj)
    {
        GameObject.Destroy(pooledObj);
    }
    #endregion
}