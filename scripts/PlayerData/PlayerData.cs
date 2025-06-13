using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData
{
    public Vector3 respawnPoint;
    public float maxPoise;
    public int Level;
    public float attackPower;
    public float defensePower;
    public int Gold;
    public int Soul;
    public List<string> keys;
}
