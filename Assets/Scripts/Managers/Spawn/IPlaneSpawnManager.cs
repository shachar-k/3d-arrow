using System;
using UnityEngine;

public interface IPlaneSpawnManager
{
    #region DataMembers
    public event EventHandler<SpawnEventArgs> SpawnInSurrowndingArea;
    public event EventHandler<SpawnEventArgs> SpawnInPosition;

    public event EventHandler<SpawnEventArgs> DestroyPlane;

    #endregion

    #region Methods
    public void SpawnPlanes(bool spawnObsticles = false);
    public void SpawnPlaneByCollison(Collider other);
    public Bounds GetPlaneBounds(Vector2 pos);
    public void Clear();
    #endregion
}