using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

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

    private IInputManager InputManager => this.Container.Resolve<IInputManager>();

    private InputAction PauseAction=> this.InputManager.GetAction(eInput.PauseInput);

    private AudioSource AudioSource => this.GetComponent<AudioSource>();

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
        this.AudioSource.enabled =false;
        int prevHighScore = PlayerPrefs.GetInt(Consts.HighScorePropName);
        this.PauseAction.performed -= this.OnPauseAction;
        if (this.Score >= prevHighScore)
        {
            PlayerPrefs.SetInt(Consts.HighScorePropName, this.Score);
        }

        this.StartCoroutine(this.GameOverScreenAsync());
    }

    public void InitGame()
    {
        this.Status = eGameStatus.Running;
        this.PlaneSpawnManager.SpawnPlanes(true);
        this.Container.Resolve<IPlayerContoller>().Activate();
        this.PauseAction.performed += this.OnPauseAction;
        this._timeSinceGameStarted = Time.time;
        this.AudioSource.enabled =true;
    }

    protected override void Start()
    {
        base.Start();
        this.Container.RegisterScriptable(this._gameSettings);
        this.Status = eGameStatus.Menu;
        this.AudioSource.enabled =false;
    }

    void FixedUpdate()
    {
        if (!this.IsGameRunning())
        {
            return;
        }
        
        this.Score = (int)((Time.time - this._timeSinceGameStarted) * 1000);
        this.UIManager.UpdateScore(this.Score);
    }

    private void OnPauseAction(CallbackContext context)
    {
        if (this.IsGameRunning())
        {
            this.Status = eGameStatus.Pause;
            this.UIManager.SetPauseMenuVisibility(true);
        }else if(this.Status == eGameStatus.Pause)
        {
             this.Status = eGameStatus.Running;
            this.UIManager.SetPauseMenuVisibility(false);
        }
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
