using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
    #region DataMembers

    [SerializeField]
    private GameSettings _gameSettings;

    private float _timeSinceGameStarted;

    #endregion

    #region Properties

    public GameSettings GameSettings => this._gameSettings;

    private eGameStatus Status { get; set; }

    private int Score { get; set; }

    private IPlaneSpawnManager PlaneSpawnManager => this.Container.Resolve<IPlaneSpawnManager>();

    private IObsticleSpawnManager ObsticleSpawnManager => this.Container.Resolve<IObsticleSpawnManager>();

    private IUIManager UIManager => this.Container.Resolve<IUIManager>() ?? this.GetComponentInChildren<UIManager>();

    private Camera MainCamera => Camera.main;

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
        this.Status = status;
    }

    public void GameOverScreen()
    {
        this.StartCoroutine(this.GameOverScreenAsync());
    }

    public void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.PlaneSpawnManager.SpawnPlanes(true);
        this.Container.Resolve<IPlayerContoller>().Activate();
        this._timeSinceGameStarted = Time.time;
    }

    protected override void Start()
    {
        base.Start();
        this.Container.RegisterScriptable(this._gameSettings);
        this.Status = eGameStatus.Menu;
    }

    void FixedUpdate()
    {
        if (!this.IsGameRunning())
        {
            return;
        }

        this.Score =(int)((Time.time - this._timeSinceGameStarted) * 1000);
        this.UIManager.UpdateScore(this.Score);
    }

    private System.Collections.IEnumerator GameOverScreenAsync()
    {
        yield return this.UIManager.DisplayGameOverScreen();
        this.PlaneSpawnManager.Clear();
        this.ObsticleSpawnManager.Clear();
        this.MainCamera.transform.position = Vector2.zero;
        this.PlaneSpawnManager.SpawnPlanes();
    }

    #endregion
}
