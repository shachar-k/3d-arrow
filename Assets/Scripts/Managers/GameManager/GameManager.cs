using System.Collections.Generic;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
    #region DataMembers

    [SerializeField]
    private GameSettings _gameSettings;

    #endregion

    #region Properties

    public GameSettings GameSettings => this._gameSettings;

    private eGameStatus Status { get; set; }

    private IPlaneSpawnManager PlaneSpawnManager => this.Container.Resolve<IPlaneSpawnManager>();

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
        this.InitGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.PlaneSpawnManager.SpawnPlanes();
    }

    public void SpawnPlaneByCollison(Collider other)
    {
        this.PlaneSpawnManager.SpawnPlaneByCollison(other);
    }


    #endregion
}
