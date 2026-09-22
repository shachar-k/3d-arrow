using UnityEngine;

public interface IPlaneSpawnManager
{
    #region Methods
        public void SpawnPlanes();
        public void SpawnPlaneByCollison(Collider other);
        public Bounds GetPlaneBounds();
    #endregion
}