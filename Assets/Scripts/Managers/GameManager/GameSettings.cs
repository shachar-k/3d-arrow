using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public float MoveSpeedDiff = 1.2f;
    public int TurnAngle = 30;
    public float TurnRate = 0.25f;
    public Vector3 PlaneStartLocation = new Vector3(0,0,0);
    public float BaseSpeed =4f;
    public float RampSpeed = 1.5f;
    public float MaxSpeed = 10f;
    public float RampFixedMinutes=1f;

}
