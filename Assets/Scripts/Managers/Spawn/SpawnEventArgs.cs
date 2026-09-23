using System;
using UnityEngine;

public class SpawnEventArgs : EventArgs
{
    public Vector2 Position {get;set;}

    public SpawnEventArgs(Vector2 pos)
    {
        this.Position = pos;
    }
}