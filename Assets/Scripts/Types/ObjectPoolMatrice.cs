using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectMatrice<T> where T : class
{
    #region Properties

    public Vector2 MaxPoint { get; set; } = new Vector2(0, 0);
    public Vector2 MinPoint { get; set; } = new Vector2(0, 0);

    private Dictionary<int, Dictionary<int, T>> matrice { get; set; } = new Dictionary<int, Dictionary<int, T>>();

    #endregion

    #region Methods

    public T this[int x, int y]
    {
        get => this.GetObject(x, y);
        set
        {
            if (value == null)
            {
                this.RemoveObject(x, y);
            }
            else
            {
                this.AddObject(x, y, value);
            }
        }
    }

    private T GetObject(int x, int y)
    {
        if (!matrice.ContainsKey(x) || !matrice[x].ContainsKey(y))
        {
            return null;
        }

        return matrice[x][y];
    }

    private void RemoveObject(int x, int y)
    {
        if (matrice.ContainsKey(x) && matrice[x].ContainsKey(y))
        {
            matrice[x].Remove(y);
            
            if (matrice[x].Count == 0)
            {
                matrice.Remove(x);
            }

            this.UpdateBounds();
        }
    }

    private void UpdateBoundsAfterRemoval(int y)
    {
        if (y == this.MaxPoint.y)
        {
            this.MaxPoint = new Vector2(this.MaxPoint.x, y - 1);
        }
        else if (y == this.MinPoint.y)
        {
            this.MinPoint = new Vector2(this.MinPoint.x, y + 1);
        }

        // Debug.Log($"Updated Bounds Removal: MaxPoint = {MaxPoint}, MinPoint = {MinPoint}");
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

        this.UpdateBounds();
    }

    private void UpdateBounds()
    {
        int maxX = this.matrice.Keys.Max();
        int minX = this.matrice.Keys.Min();
        int maxY = this.matrice.Values.SelectMany(v => v.Keys).Max();
        int minY = this.matrice.Values.SelectMany(v => v.Keys).Min();

        this.MaxPoint = new Vector2(maxX, maxY);
        this.MinPoint = new Vector2(minX, minY);
        Debug.Log($"Updated Bounds: MaxPoint = {MaxPoint}, MinPoint = {MinPoint}");
    }
    #endregion
}