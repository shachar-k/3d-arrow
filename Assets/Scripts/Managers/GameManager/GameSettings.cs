using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float Movespeed = 0.3f;
    public int TurnAngle = 30;
    public float TurnRate = 0.25f;
    public Vector3 planeStartLocation = new Vector3(0,0,0);
}
