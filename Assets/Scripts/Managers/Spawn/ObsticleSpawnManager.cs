using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObsticleSpawnManager : Injectable<ObsticleSpawnManager, IObsticleSpawnManager>, IObsticleSpawnManager
{
    #region DataMembers

    [SerializeField]
    private List<ObjectParameters> prefabs;

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
        this.objectsToSpawn = new GameObjectPoolList(prefabs);
        SpawnInSurrondingArea(Vector2.zero);
        this.PlaneSpawnManager.SpawnInSurrowndingArea +=
         (sender,eventArgs) => this.SpawnInSurrondingArea(eventArgs.Position);
    }

     public void SpawnInSurrondingArea(Vector2 pos)
    {
        SpawnObsticals(pos);
        SpawnObsticals(pos + Vector2.up);
        SpawnObsticals(pos + Vector2.left);
        SpawnObsticals(pos + Vector2.right);
        SpawnObsticals(pos + Vector2.one);
        SpawnObsticals(pos + new Vector2(-1,1));
    }

    public void SpawnObsticals(Vector2 pos)
    {
        int amount = Random.Range(this.GameSettings.ObstaclePoolPlaneMinSize, this.GameSettings.ObstaclePoolPlaneMaxSize);
        StartCoroutine(SpawnObsticalsAsync(pos,amount));
    }

    private System.Collections.IEnumerator SpawnObsticalsAsync(Vector2 pos, int amount)
    {
        Bounds bounds = this.PlaneSpawnManager.GetPlaneBounds();

        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPosition = GetRandomPositionInBounds(bounds);
            string randomObject = this.GetRandomPoolObject();
            GameObject obj = this.objectsToSpawn.SpawnObject(randomObject, randomPosition);
            this.UpdatePerPlaneMatrice(pos, obj);
            yield return new WaitForSeconds(this.GameSettings.CooldownBetweenSpawns);
        }
    }

    private void UpdatePerPlaneMatrice(Vector2 pos, GameObject obj)
    {
        var list = this.objectsSpawnedPerMatrice[(int)pos.x, (int)pos.y];

        if (list == null)
        {
            list = new List<GameObject>();
        }

        list.Add(obj);
        this.objectsSpawnedPerMatrice[(int)pos.x, (int)pos.y] = list;
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y;

        return new Vector3(x, y, z);
    }

    private string GetRandomPoolObject()
    {
        List<string> objNames = this.objectsToSpawn.ObjectsCanBeSpawned;
        int i = Random.Range(0,objNames.Count -1);
        
        return objNames[i];
    }
    #endregion
}
