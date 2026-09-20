using System.Collections.Generic;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
    private const int MAX_PLANES = 3;
    private const int DONT_DELETE = -999;
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
        SpawnPlanes();
    }

    private void SpawnPlanes()
    {
        for (int x = 0; x < MAX_PLANES; x++)
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
        Debug.Log($"Collision Detected with: {other.gameObject.name}");
        Bounds bounds = this.GetBounds(other.gameObject) ?? new Bounds();
        Vector2 pos = this.GetGridPosition(other.transform.position, bounds);
        this.CalcAdd(pos);
        this.CalcDeletion(pos);
    }

    private void CalcAdd(Vector2 pos)
    {
        Vector2 posToAdd = GetPosToAdd(pos);
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

    private void CalcDeletion(Vector2 pos)
    {
        Vector2 posToDelete = GetPosToDelete(pos);

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

    private Vector2 GetPosToAdd(Vector2 pos)
    {
        Vector2 max = this.PlaneMatrice.MaxPoint;
        Vector2 min = this.PlaneMatrice.MinPoint;
        Vector2 posToAdd = Vector2.zero;

        if (pos.x == max.x)
        {
            posToAdd.x = max.x + 1;
        }
        else if (pos.x == min.x)
        {
            posToAdd.x = min.x - 1;
        }
        else
        {
            posToAdd.x = DONT_DELETE;
        }

        posToAdd.y = pos.y == max.y ? max.y + 1 : DONT_DELETE;

        return posToAdd;
    }

    private Vector2 GetPosToDelete(Vector2 pos)
    {
        Vector2 max = this.PlaneMatrice.MaxPoint;
        Vector2 min = this.PlaneMatrice.MinPoint;
        Vector2 posToDelete = Vector2.zero;

        if (pos.x == max.x)
        {
            posToDelete.x = min.x;
        }
        else if (pos.x == min.x)
        {
            posToDelete.x = max.x;
        }
        else
        {
            posToDelete.x = DONT_DELETE;
        }

        posToDelete.y = pos.y == max.y ? min.y : DONT_DELETE;

        return posToDelete;
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
        float newXpos = initialPos.x + (-1 + x) * size.x;
        float newZpos = initialPos.z + -y * size.z;
        return new Vector3(newXpos, initialPos.y, newZpos);
    }

    private Bounds? GetBounds(GameObject gameObject)
    {
        return gameObject.GetComponent<MeshRenderer>()?.bounds;
    }

    #endregion
}
