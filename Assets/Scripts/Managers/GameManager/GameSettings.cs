using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Scriptable Objects/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    public float Movespeed = 0.3f;
    public int TurnAngle = 30;
    public float TurnRate = 0.25f;
    
}
