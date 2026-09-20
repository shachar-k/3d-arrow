using System.Collections.Generic;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
    #region Consts
    private const int MAX_PLANES = 3;
    private const int DONT_DELETE = -999;

    private const float TIME_TO_WAIT_BEFORE_SPAWN = 0.5f;

    #endregion
    #region DataMembers

    [SerializeField]
    private GameSettings _gameSettings;

    [SerializeField]
    private GameObject _plane;

    #endregion

    #region Properties

    public GameSettings GameSettings => this._gameSettings;

    private eGameStatus Status { get; set; }

    private ObjectPoolMatrice<GameObject> PlaneMatrice { get; set; } = new ObjectPoolMatrice<GameObject>();

    private float TimeSinceLastSpawn { get; set; } = 0f;

    #endregion

    #region Methods

    public bool IsGameRunning()
    {
        return Status == eGameStatus.Running;
    }

    protected override void Start()
    {
        base.Start();
        this.Container.RegisterScriptable(this._gameSettings);
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.TimeSinceLastSpawn = Time.time;
        this.SpawnPlanes();
    }

    private void SpawnPlanes()
    {
        for (int x = -1; x < MAX_PLANES - 1; x++)
        {
            for (int y = 0; y < MAX_PLANES; y++)
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

        Debug.Log($"Collision Detected with: {other.gameObject.name}");
        Bounds bounds = this.GetBounds(other.gameObject) ?? new Bounds();
        Vector2 pos = this.GetGridPosition(other.transform.position, bounds);
        var posToAddAndDelete = this.GetPosToAddAndDelete(pos);
        Debug.Log($"Pos to Add: {posToAddAndDelete.Item1}, Pos to Delete: {posToAddAndDelete.Item2}");
        this.CalcAdd(posToAddAndDelete.Item1);
        this.CalcDeletion(posToAddAndDelete.Item2);
    }

    private void CalcAdd(Vector2 posToAdd)
    {
        if (posToAdd.x != DONT_DELETE)
        {
            for (int y = 0; y < MAX_PLANES; y++)
            {
                this.AddPlane((int)posToAdd.x, y);
            }
        }

        if (posToAdd.y != DONT_DELETE)
        {
            for (int x = 0; x < MAX_PLANES; x++)
            {
                this.AddPlane(x, (int)posToAdd.y);
            }
        }
    }

    private void CalcDeletion(Vector2 posToDelete)
    {
        if (posToDelete.x != DONT_DELETE)
        {
            for (int y = 0; y < MAX_PLANES; y++)
            {
                this.DeletePlane((int)posToDelete.x, y);
            }
        }

        if (posToDelete.y != DONT_DELETE)
        {
            for (int x = 0; x < MAX_PLANES; x++)
            {
                this.DeletePlane(x, (int)posToDelete.y);
            }
        }
    }

    private System.Tuple<Vector2, Vector2> GetPosToAddAndDelete(Vector2 pos)
    {
        Vector2 max = this.PlaneMatrice.MaxPoint;
        Vector2 min = this.PlaneMatrice.MinPoint;
        Vector2 posToAdd = Vector2.zero;
        Vector2 posToDelete = Vector2.zero;

        if (pos.x == max.x)
        {
            posToAdd.x = max.x + 1;
            posToDelete.x = min.x;
        }
        else if (pos.x == min.x)
        {
            posToAdd.x = min.x - 1;
            posToDelete.x = max.x;
        }
        else
        {
            posToAdd.x = DONT_DELETE;
            posToDelete.x = DONT_DELETE;
        }

        posToAdd.y = pos.y == max.y ? max.y + 1 : DONT_DELETE;
        posToDelete.y = pos.y == max.y ? min.y : DONT_DELETE;

        return new System.Tuple<Vector2, Vector2>(posToAdd, posToDelete);
    }


    private void AddPlane(int x, int y)
    {
        var obj = Instantiate(this._plane, this.GameSettings.planeStartLocation, Quaternion.identity);
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
        Vector3 initialPos = this.GameSettings.planeStartLocation;
        float xPosInGrid = (pos.x - initialPos.x) / size.x;
        float yPosInGrid = (pos.z - initialPos.z) / size.z;
        return new Vector2(xPosInGrid, yPosInGrid);
    }

    private Vector3 GetNewPosOfGrid(int x, int y, GameObject obj)
    {
        var bounds = this.GetBounds(obj) ?? new Bounds();
        var size = bounds.size;
        Vector3 initialPos = this.GameSettings.planeStartLocation;
        float newXpos = initialPos.x + x * size.x;
        float newZpos = initialPos.z + -y * size.z;
        return new Vector3(newXpos, initialPos.y, newZpos);
    }

    private Bounds? GetBounds(GameObject gameObject)
    {
        return gameObject.GetComponent<MeshRenderer>()?.bounds;
    }

    #endregion
}
