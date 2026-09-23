using System;
using UnityEngine;

public interface IPlaneSpawnManager
{
    #region DataMembers
    public event EventHandler<SpawnEventArgs> SpawnInSurrowndingArea;

    #endregion

    #region Methods
    public void SpawnPlanes();
    public void SpawnPlaneByCollison(Collider other);
    public Bounds GetPlaneBounds();
    #endregion
}