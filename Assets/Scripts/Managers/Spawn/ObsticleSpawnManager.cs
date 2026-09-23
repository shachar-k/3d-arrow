using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObsticleSpawnManager : Injectable<ObsticleSpawnManager, IObsticleSpawnManager>, IObsticleSpawnManager
{
    private const int MAX_BLOCK_SIZE = 5;
    #region DataMembers

    [SerializeField]
    private List<ObjectParameters> prefabs;


    #endregion

    #region Properties
    private GameSettings GameSettings { get; set; }

    private IPlaneSpawnManager PlaneSpawnManager { get; set; }

    private GameObjectPoolList ObjectsToSpawn { get; set; }

    private ObjectMatrice<List<GameObject>> ObjectsSpawnedPerMatrice { get; set; } = new ObjectMatrice<List<GameObject>>();

    #endregion

    #region Methods
    protected override void Start()
    {
        base.Start();
        this.GameSettings = this.Container.Resolve<GameSettings>();
        this.PlaneSpawnManager = this.Container.Resolve<IPlaneSpawnManager>();
        this.ObjectsToSpawn = new GameObjectPoolList(prefabs);
        SpawnInSurrondingArea(Vector2.zero);
        this.PlaneSpawnManager.SpawnInSurrowndingArea += this.HandleSpawn;
        this.PlaneSpawnManager.DestroyPlane += this.HandleDestroy;
    }

    void OnDestroy()
    {
        this.PlaneSpawnManager.DestroyPlane -= this.HandleDestroy;
        this.PlaneSpawnManager.SpawnInSurrowndingArea -= this.HandleSpawn;
    }

    public void SpawnInSurrondingArea(Vector2 pos)
    {
        SpawnObsticals(pos);
        SpawnObsticals(pos + Vector2.down);
        SpawnObsticals(pos + Vector2.left);
        SpawnObsticals(pos + Vector2.right);
        SpawnObsticals(pos + new Vector2(1, -1));
        SpawnObsticals(pos - Vector2.one);
    }

    public void SpawnObsticals(Vector2 pos)
    {
        int obstaclePoolPlaneMaxSize = this.GameSettings.ObstaclePoolPlaneMaxSize;
        var objList = this.ObjectsSpawnedPerMatrice[(int)pos.x, (int)pos.y] ?? new List<GameObject>();
        int maxCanSpawn = obstaclePoolPlaneMaxSize - objList.Count;
        int amount = Random.Range(this.GameSettings.ObstaclePoolPlaneMinSize, maxCanSpawn);

        if (amount <= 0)
        {
            return;
        }

        StartCoroutine(SpawnObsticalsAsync(pos, amount));
    }

    private void HandleSpawn(object sender, SpawnEventArgs args)
    {
        this.SpawnInSurrondingArea(args.Position);
    }

    private void HandleDestroy(object sender, SpawnEventArgs args)
    {
        StartCoroutine(this.DestroyObsticlesInPlaneAsync(args.Position));
    }

    private System.Collections.IEnumerator SpawnObsticalsAsync(Vector2 pos, int amount)
    {
        Bounds bounds = this.PlaneSpawnManager.GetPlaneBounds(pos);
        Debug.Log($"bounds {bounds.min} {bounds.max}");

        for (int i = 0; i < amount; i++)
        {
            Vector3 randomPosition = GetRandomPositionInBounds(bounds);
            GameObject obj = this.ObjectsToSpawn.SpawnRandomObject(randomPosition);
            obj.transform.localScale = Vector3.one * Random.Range(1, 3);
            SetRandomColor(obj);
            this.UpdatePerPlaneMatrice(pos, obj);

            if (i % MAX_BLOCK_SIZE == 0)
            {
                yield return new WaitForSeconds(this.GameSettings.CooldownBetweenSpawns);
            }
        }
    }

    private System.Collections.IEnumerator DestroyObsticlesInPlaneAsync(Vector2 pos)
    {
        List<GameObject> gameObjects = this.ObjectsSpawnedPerMatrice[(int)pos.x, (int)pos.y] ?? new List<GameObject>();

        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject gameObject = gameObjects[i];
            this.ObjectsToSpawn.Release(gameObject);

            if (i % MAX_BLOCK_SIZE == 0)
            {
                yield return new WaitForSeconds(this.GameSettings.CooldownBetweenSpawns);
            }
        }
    }


    private void SetRandomColor(GameObject obj)
    {
        var renderer = obj.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            return;
        }

        Color color = Random.ColorHSV();
        var mat = new Material(renderer.sharedMaterial);
        mat.SetColor("_BaseColor", color);
        mat.SetColor("_EmissionColor", color);
        renderer.material = mat;
    }

    private void UpdatePerPlaneMatrice(Vector2 pos, GameObject obj)
    {
        var list = this.ObjectsSpawnedPerMatrice[(int)pos.x, (int)pos.y];

        if (list == null)
        {
            list = new List<GameObject>();
        }

        list.Add(obj);
        this.ObjectsSpawnedPerMatrice[(int)pos.x, (int)pos.y] = list;
    }

    private Vector3 GetRandomPositionInBounds(Bounds bounds)
    {
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.center.y + 0.5f;

        return new Vector3(x, y, z);
    }

    #endregion
}
