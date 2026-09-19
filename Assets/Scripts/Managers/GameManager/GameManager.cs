using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Injectable<GameManager, IGameManager>
{
    [SerializeField]
    private GameSettings _gameSettings ;

    public GameSettings GameSettings => this._gameSettings;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    protected override void Start()
    {
        base.Start();
        this.Container.RegisterScriptable(this._gameSettings);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
