using TMPro;
using UnityEngine;

public class UIManager : Injectable<UIManager, IUIManager>, IUIManager
{

    private const int SECONDS_GAME_OVER_TEXT_DISPLAYED = 3;

    #region DataMembers

    [SerializeField]
    private GameObject _gameOverTextPrefab;
    #endregion

    #region Properties
    private TextMeshProUGUI GameOverText => this._gameOverTextPrefab.GetComponent<TextMeshProUGUI>();

    private IGameManager GameManager => this.Container.Resolve<IGameManager>();

    #endregion

    #region Methods

    public System.Collections.IEnumerator DisplayGameOverScreen()
    {
        this.GameOverText.gameObject.SetActive(true);
        this.GameOverText.text = "Game Over";
        yield return new WaitForSeconds(SECONDS_GAME_OVER_TEXT_DISPLAYED);
        this.GameOverText.gameObject.SetActive(false);
        this.GameManager.ToMenu();
    }

    public void Init()
    {
        throw new System.NotImplementedException();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        this.GameOverText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion
}
