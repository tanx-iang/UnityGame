using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GameModule;

public class GameManager : MonoBehaviour
{
    public Player player;

    void Start()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null)
        {
            player.transform.position = data.respawnPoint;
            player.stats.maxPoise = data.maxPoise;
            player.stats.Level = data.Level;
            player.stats.attackPower = data.attackPower;
            player.stats.defensePower = data.defensePower;
            player.inventory.Gold = data.Gold;
            player.inventory.Soul = data.Soul;
            foreach (var targetId in data.keys)
            {
                KeyItem key = new KeyItem(targetId);
                player.inventory.AddItem(key);
            }

        }
        else
        {
            Debug.Log("No save data found. Starting a new game.");
        }
    }
}
