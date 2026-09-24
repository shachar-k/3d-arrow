using System;
using UnityEngine;


[Serializable]
public class ObjectParameters
{
    #region Properties
    [SerializeField]
    public string Name;

    [SerializeField]
    public int Max;

    [SerializeField]
    public int Defualt;

    [SerializeField]
    public GameObject Prefab;
    #endregion
}