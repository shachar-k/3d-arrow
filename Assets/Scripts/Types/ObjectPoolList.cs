using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;


public class GameObjectPoolList
{
    #region DataMembers

    private List<ObjectParameters> prefabs;
    private Dictionary<int, ObjectPool<GameObject>> poolByUsedObj = new Dictionary<int, ObjectPool<GameObject>>();

    #endregion

    #region C'Tors
    public GameObjectPoolList( List<ObjectParameters> prefabs)
    {
        this.prefabs = prefabs;
        this.Pools = new Dictionary<string, ObjectPool<GameObject>>();

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

        this.PrewarmAll();
    }

    #endregion

    #region Properties

    public List<string> ObjectsCanBeSpawned => this.Pools.Where(pair => pair.Value.CountInactive > 0).Select(pair => pair.Key).ToList();
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

    public GameObject SpawnRandomObject(Vector3 pos)
    {
        int i = UnityEngine.Random.Range(0, this.prefabs.Count -1);
        string name = this.prefabs[i].Name;
        return this.SpawnObject(name,pos);
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
        int key = obj.GetInstanceID();

        if(!this.poolByUsedObj.ContainsKey(key))
        {
            this.poolByUsedObj.Add(key, this.Pools[name]);
        }

        return obj;
    }

    public void Release(GameObject pooledObj)
    {

        int id = pooledObj.GetInstanceID();

        if (!poolByUsedObj.ContainsKey(id))
        {
            return;
        }

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

     private void PrewarmAll()
    {
        foreach (var pair in this.Pools)
        {
            int? max= this.prefabs.FirstOrDefault(prefab => prefab.Name == pair.Key)?.Max;
            
            for (int i = 0; i < max; i++)
            {
                var pool = pair.Value;
                var obj = pool.Get();
                pool.Release(obj);
            }
        }
    }
    #endregion
}