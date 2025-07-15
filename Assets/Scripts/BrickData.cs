using UnityEngine;

[CreateAssetMenu(fileName = "New Brick Data", menuName = "Brick Breaker/Brick Data")]
public class BrickData : ScriptableObject
{
    public string brickName;
    public int points;
    public int coins;
    public int hitMax;
    public bool hasMalus;
    public bool hasBonus;

}
