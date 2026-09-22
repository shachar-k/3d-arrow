using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObsticleSpawnManager : Injectable<ObsticleSpawnManager,IObsticleSpawnManager>, IObsticleSpawnManager
{
    [SerializeField]
    private GameObjectPoolList objectsToSpawn = new GameObjectPoolList();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
