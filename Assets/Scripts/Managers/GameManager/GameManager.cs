using System.Collections.Generic;
using TMPro;
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

    private IObsticleSpawnManager ObsticleSpawnManager => this.Container.Resolve<IObsticleSpawnManager>();

    private IUIManager UIManager => this.Container.Resolve<IUIManager>() ?? this.GetComponentInChildren<UIManager>();

    #endregion

    #region Methods

    public bool IsGameRunning()
    {
        return Status == eGameStatus.Running;
    }

    public void SpawnPlaneByCollison(Collider other)
    {
        this.PlaneSpawnManager.SpawnPlaneByCollison(other);
    }

    public void SetStatus(eGameStatus status)
    {
        this.Status =status;
    }

    public void GameOverScreen()
    {
        this.StartCoroutine(this.UIManager.DisplayGameOverScreen());
    }

    protected override void Start()
    {
        base.Start();
        this.Container.RegisterScriptable(this._gameSettings);
        this.Status = eGameStatus.Menu;
    }

    public void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.PlaneSpawnManager.SpawnPlanes(true);
        this.Container.Resolve<IPlayerContoller>().Activate();
    }

    #endregion
}
