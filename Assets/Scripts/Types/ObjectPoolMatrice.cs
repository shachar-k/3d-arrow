using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMatrice<T> where T : class
{
    #region Properties

    private Dictionary<int, Dictionary<int, T>> matrice { get; set; }

    #endregion

    #region Methods

    public T this[int x, int y]
    {
        get => matrice[x][y];
        set => this.AddObject(x, y, value);
    }

    private T GetObject(int x, int y)
    {
        if(!matrice.ContainsKey(x) || !matrice[x].ContainsKey(y))
        {
            Debug.Log($"Attempting to access ${x} , ${y} which is missing from matrice");
            return null;
        }

        return matrice[x][y];
    }

    private void AddObject(int x, int y, T obj)
    {
        if (!matrice.ContainsKey(x))
        {
            matrice.Add(x, new Dictionary<int, T>());
        }

        var xAxis = matrice[x];

        if (!xAxis.ContainsKey(y))
        {
            xAxis.Add(y, obj);
        }
        else
        {
            xAxis[y] = obj;
        }
    }
    #endregion
}