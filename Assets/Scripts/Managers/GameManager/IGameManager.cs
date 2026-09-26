using UnityEngine;

public interface IGameManager
{
    #region Methods
    public bool IsGameRunning();

    public void SpawnPlaneByCollison(Collider other);

    public void SetStatus(eGameStatus status);

    public void GameOverScreen();

    public void InitGame();
    #endregion
}