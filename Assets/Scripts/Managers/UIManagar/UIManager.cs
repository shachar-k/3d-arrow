using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : Injectable<UIManager, IUIManager>, IUIManager
{

    private const int SECONDS_GAME_OVER_TEXT_DISPLAYED = 3;

    #region DataMembers

    [SerializeField]
    private GameObject _gameOverTextPrefab;
    [SerializeField]
    List<GameObject> _menuStuff;
    #endregion

    #region Properties
    private TextMeshProUGUI GameOverText => this._gameOverTextPrefab.GetComponent<TextMeshProUGUI>();

    private IGameManager GameManager => this.Container.Resolve<IGameManager>();

    private IPlaneSpawnManager PlaneSpawnManager => this.Container.Resolve<IPlaneSpawnManager>();

    #endregion

    #region Methods

    public System.Collections.IEnumerator DisplayGameOverScreen()
    {
        this.GameOverText.gameObject.SetActive(true);
        this.GameOverText.text = "Game Over";
        yield return new WaitForSeconds(SECONDS_GAME_OVER_TEXT_DISPLAYED);
        this.GameOverText.gameObject.SetActive(false);
        this.GameManager.SetStatus(eGameStatus.Menu);
        this.DisplayMenu();
    }

    public void DisplayMenu()
    {
        this.SetMenuVisibility(true);
    }

    public void OnStartClick()
    {
        this.SetMenuVisibility(false);
        this.GameManager.InitGame();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        this.GameOverText.gameObject.SetActive(false);
        this.DisplayMenu();
        this.PlaneSpawnManager.SpawnPlanes();
    }

    private void SetMenuVisibility(bool visible)
    {
        foreach(var obj in this._menuStuff)
        {
            obj.SetActive(visible);
        }
    }
    #endregion
}
