using UnityEngine;

public interface IGameManager
{
    public bool IsGameRunning();

    public void SpawnPlaneByCollison(Collider other);
}