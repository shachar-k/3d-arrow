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

    private eGameStatus Status {get;set;}

    private Dictionary<int, Dictionary<int,GameObject>> planeMatrice {get;set;}
    
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion
}
