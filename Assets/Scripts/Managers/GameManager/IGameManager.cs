using UnityEngine;

public interface IGameManager
{
    #region Methods
    public bool IsGameRunning();

    public void SpawnPlaneByCollison(Collider other);

    public void GameOver();
    #endregion
}