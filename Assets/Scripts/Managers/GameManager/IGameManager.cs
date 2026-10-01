using UnityEngine;

public interface IGameManager
{
    #region Methods

    public void StopMusic();

    public bool IsGameRunning();

    public void SpawnPlaneByCollison(Collider other);

    public void SetStatus(eGameStatus status);

    public void GameOverScreen();

    public void InitGame();
    #endregion
}