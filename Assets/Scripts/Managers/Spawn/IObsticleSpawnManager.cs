using UnityEngine;

public interface IObsticleSpawnManager
{
    #region Methods

    public void SpawnObsticals(Vector2 pos);
    public void SpawnInSurrondingArea(Vector2 pos);
    public void Clear();

    #endregion
}
