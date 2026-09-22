using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

[System.Serializable]
public class GameObjectPoolList
{
    #region DataMembers

    [SerializeField]
    private List<ObjectParameters> prefabs = new List<ObjectParameters>();
    private Dictionary<int, ObjectPool<GameObject>> poolByUsedObj = new Dictionary<int, ObjectPool<GameObject>>();

    #endregion

    #region C'Tors
    public GameObjectPoolList()
    {
        foreach (var prefab in prefabs)
        {
            var pool = new ObjectPool<GameObject>(
                () => this.CreateObject(prefab.Prefab),
                OnGetFromPool,
                OnReturnToPool,
                OnDestroy,
                true,
                prefab.Defualt,
                prefab.Max
            );

            this.Pools.Add(prefab.Name, pool);
        }
    }

    #endregion

    #region Properties

    public List<string> ObjectNames => this.Pools.Keys.ToList();
    private Dictionary<string, ObjectPool<GameObject>> Pools { get; set; }

    #endregion

    #region Methods

    public void Clear()
    {
        foreach (ObjectPool<GameObject> pool in this.Pools.Values)
        {
            pool.Clear();
        }
    }

    public GameObject SpawnObject(string name, Vector3 pos)
    {
        GameObject gameObject = this.Get(name);
        gameObject.transform.position = pos;

        return gameObject;
    }

    public GameObject Get(string name)
    {
        GameObject obj = this.Pools[name].Get();
        this.poolByUsedObj.Add(obj.GetInstanceID(), this.Pools[name]);

        return obj;
    }

    public void Release(GameObject pooledObj)
    {

        int id = pooledObj.GetInstanceID();
        this.poolByUsedObj[id].Release(pooledObj);
        this.poolByUsedObj.Remove(id);
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