using System.Collections.Generic;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
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
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
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
        Bounds bounds = this.GetBounds(other.gameObject) ?? new Bounds();
        Vector2 pos = this.GetGridPosition(other.transform.position, bounds);
        Debug.Log($"Collision at ${pos.x} {pos.y}");
    }

    private void AddPlane(int x, int y)
    {
        var obj = Instantiate(this._plane, this.GameSettings.planeStartLocation, Quaternion.identity);
        obj.transform.position = this.GetNewPosOfGrid(x, y, obj);
        obj.transform.parent = this.transform;
        this.PlaneMatrice[x, y] = obj;
    }

    private Vector2 GetGridPosition(Vector3 pos, Bounds bounds)
    {
        var size = bounds.size;
        Vector3 initialPos = this.GameSettings.planeStartLocation;
        float xPosInGrid = (pos.x - initialPos.x) / size.x;  
        float yPosInGrid = (pos.z - initialPos.z) / size.z; 
        return new Vector2(xPosInGrid,yPosInGrid);
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
