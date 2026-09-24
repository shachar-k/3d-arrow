using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>, IGameManager
{
    private const int SECONDS_GAME_OVER_TEXT_DISPLAYED = 3;
    #region DataMembers

    [SerializeField]
    private GameSettings _gameSettings;

    #endregion

    #region Properties

    public GameSettings GameSettings => this._gameSettings;

    private eGameStatus Status { get; set; }

    private IPlaneSpawnManager PlaneSpawnManager => this.Container.Resolve<IPlaneSpawnManager>();

    private IUIManager UIManager => this.Container.Resolve<IUIManager>();

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

    public void GameOver()
    {
        this.Status = eGameStatus.GameOver;
    }

    public void ToMenu()
    {
        this.Status = eGameStatus.Menu;
        this.StartCoroutine(this.DisplayGameOverScreen());
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

    private System.Collections.IEnumerator DisplayGameOverScreen()
    {
        
        yield return new WaitForSeconds(SECONDS_GAME_OVER_TEXT_DISPLAYED);
        this.ToMenu();
    }

    private void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.PlaneSpawnManager.SpawnPlanes();
    }

    #endregion
}
