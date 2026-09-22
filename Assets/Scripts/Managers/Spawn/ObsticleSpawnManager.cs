using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObsticleSpawnManager : Injectable<ObsticleSpawnManager, IObsticleSpawnManager>, IObsticleSpawnManager
{
    #region DataMembers

    [SerializeField]
    private GameObjectPoolList objectsToSpawn;

    #endregion

    #region Properties
    private GameSettings GameSettings { get; set; }

    private IPlaneSpawnManager PlaneSpawnManager {get;set;}

    private ObjectMatrice<List<GameObject>> objectsSpawnedPerMatrice { get; set; }

    #endregion

    #region Methods
    protected override void Start()
    {
        base.Start();
        this.GameSettings = this.Container.Resolve<GameSettings>();
        this.PlaneSpawnManager = this.Container.Resolve<IPlaneSpawnManager>();
        this.objectsToSpawn = new GameObjectPoolList();
    }

     public void SpawnInSurrondingArea(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    public void SpawnObsticals(Vector2 pos)
    {
        throw new System.NotImplementedException();
    }

    private IEnumerator SpawnObsticalsAsync(Vector2 pos, int amount)
    {
        Bounds bounds = this.PlaneSpawnManager.GetPlaneBounds();
        for (int i = 0; i <= amount; i++)
        {
            
        }
    }
    #endregion
}
