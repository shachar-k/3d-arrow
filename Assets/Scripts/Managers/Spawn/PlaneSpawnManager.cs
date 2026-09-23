using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneSpawnManager : Injectable<PlaneSpawnManager, IPlaneSpawnManager>, IPlaneSpawnManager
{
    #region Consts
    private const int MAX_PLANES = 3;
    private const int DONT_DELETE = -999;

    private const float TIME_TO_WAIT_BEFORE_SPAWN = 0.5f;

    #endregion

    #region DataMembers

    [SerializeField]
    public GameObject plane;

    public event EventHandler<SpawnEventArgs> SpawnInSurrowndingArea;

    public event EventHandler<SpawnEventArgs> DestroyPlane;

    #endregion

    #region Properties

    private GameSettings GameSettings => this.Container.Resolve<GameSettings>();

    private ObjectMatrice<GameObject> PlaneMatrice { get; set; } = new ObjectMatrice<GameObject>();

    private float TimeSinceLastSpawn { get; set; } = 0f;

    #endregion

    #region Methods
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
    }


    public void SpawnPlanes()
    {
        this.TimeSinceLastSpawn = Time.time;
        // this.ObsticleSpawnManager.SpawnInSurrondingArea(Vector2.zero);
        this.SpawnInSurrowndingArea?.Invoke(this, new SpawnEventArgs(Vector2.zero));
        for (int x = -1; x < MAX_PLANES - 1; x++)
        {
            for (int y = 0; y > -MAX_PLANES; y--)
            {
                if (this.PlaneMatrice[x, y] == null)
                {
                    this.AddPlane(x, y);
                }
            }
        }
    }

    public void SpawnPlaneByCollison(Collider other)
    {
        if (Time.time - this.TimeSinceLastSpawn < TIME_TO_WAIT_BEFORE_SPAWN)
        {
            return;
        }

        Bounds bounds = this.GetBounds(other.gameObject) ?? new Bounds();
        Vector2 pos = this.GetGridPosition(other.transform.position, bounds);
        var posToAddAndDelete = this.GetPosToAddAndDelete(pos);
        this.CalcAdd(posToAddAndDelete.Item1);
        this.CalcDeletion(posToAddAndDelete.Item2);
        this.SpawnInSurrowndingArea?.Invoke(this, new SpawnEventArgs(pos));
    }

    public Bounds GetPlaneBounds(Vector2 pos)
    {
        return this.GetBounds(this.PlaneMatrice[(int)pos.x, (int)pos.y]) ?? new Bounds();
    }

    private void CalcAdd(Vector2 posToAdd)
    {
        if (posToAdd.x != DONT_DELETE)
        {
            int minY = (int)this.PlaneMatrice.MinPoint.y;
            int maxY = (int)this.PlaneMatrice.MaxPoint.y;

            for (int y = minY; y <= maxY; y++)
            {
                this.AddPlane((int)posToAdd.x, y);
            }
        }

        if (posToAdd.y != DONT_DELETE)
        {
            int minX = (int)this.PlaneMatrice.MinPoint.x;
            int maxX = (int)this.PlaneMatrice.MaxPoint.x;

            for (int x = minX; x <= maxX; x++)
            {
                this.AddPlane(x, (int)posToAdd.y);
            }
        }
    }

    private void CalcDeletion(Vector2 posToDelete)
    {
        if (posToDelete.x != DONT_DELETE)
        {
            int minY = (int)this.PlaneMatrice.MinPoint.y;
            int maxY = (int)this.PlaneMatrice.MaxPoint.y;

            for (int y = minY; y <= maxY; y++)
            {
                this.DeletePlane((int)posToDelete.x, y);
                this.DestroyPlane.Invoke(this, new SpawnEventArgs(new Vector2((int)posToDelete.x, y)));
            }
        }

        if (posToDelete.y != DONT_DELETE)
        {
            int minX = (int)this.PlaneMatrice.MinPoint.x;
            int maxX = (int)this.PlaneMatrice.MaxPoint.x;

            for (int x = minX; x <= maxX; x++)
            {
                this.DestroyPlane.Invoke(this, new SpawnEventArgs(new Vector2(x, posToDelete.y)));
                this.DeletePlane(x, (int)posToDelete.y);
            }
        }
    }

    private System.Tuple<Vector2, Vector2> GetPosToAddAndDelete(Vector2 pos)
    {
        Vector2 max = this.PlaneMatrice.MaxPoint;
        Vector2 min = this.PlaneMatrice.MinPoint;
        Vector2 posToAdd = new Vector2(DONT_DELETE, DONT_DELETE);
        Vector2 posToDelete = new Vector2(DONT_DELETE, DONT_DELETE);

        if (pos.x >= max.x)
        {
            posToAdd.x = max.x + 1;
            posToDelete.x = min.x;
        }
        else if (pos.x <= min.x)
        {
            posToAdd.x = min.x - 1;
            posToDelete.x = max.x;
        }

        if (pos.y >= max.y)
        {
            posToAdd.y = max.y + 1;
            posToDelete.y = min.y;
        }
        else if (pos.y <= min.y)
        {
            posToAdd.y = min.y - 1;
            posToDelete.y = max.y;
        }

        return new System.Tuple<Vector2, Vector2>(posToAdd, posToDelete);
    }


    private void AddPlane(int x, int y)
    {
        var obj = Instantiate(this.plane, this.GameSettings.PlaneStartLocation, Quaternion.identity);
        obj.transform.position = this.GetNewPosOfGrid(x, y, obj);
        obj.transform.parent = this.transform;
        this.PlaneMatrice[x, y] = obj;
    }

    private void DeletePlane(int x, int y)
    {
        var obj = this.PlaneMatrice[x, y];
        if (obj != null)
        {
            this.PlaneMatrice[x, y] = null;
            Destroy(obj);

        }
    }

    private Vector2 GetGridPosition(Vector3 pos, Bounds bounds)
    {
        var size = bounds.size;
        Vector3 initialPos = this.GameSettings.PlaneStartLocation;
        float xPosInGrid = (pos.x - initialPos.x) / size.x;
        float yPosInGrid = (pos.z - initialPos.z) / size.z;
        return new Vector2(xPosInGrid, yPosInGrid);
    }

    private Vector3 GetNewPosOfGrid(int x, int y, GameObject obj)
    {
        var bounds = this.GetBounds(obj) ?? new Bounds();
        var size = bounds.size;
        Vector3 initialPos = this.GameSettings.PlaneStartLocation;
        float newXpos = initialPos.x + x * size.x;
        float newZpos = initialPos.z + y * size.z;
        return new Vector3(newXpos, initialPos.y, newZpos);
    }

    private Bounds? GetBounds(GameObject gameObject)
    {
        return gameObject.GetComponent<MeshRenderer>()?.bounds;
    }
    #endregion


}
