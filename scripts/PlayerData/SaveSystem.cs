using UnityEngine;
using System.IO;
using System.Linq;

public static class SaveSystem
{
    public static void SavePlayer(Player player)
    {
        PlayerData data = new PlayerData
        {
            respawnPoint = player.stats.respawnPoint,
            maxPoise = player.stats.maxPoise,
            Level = player.stats.Level,
            attackPower = player.stats.attackPower,
            defensePower = player.stats.defensePower,
            Gold = player.inventory.Gold,
            Soul = player.inventory.Soul,
            keys = player.inventory.keys.Select(key => key.targetId).ToList(),
        };

        string json = JsonUtility.ToJson(data, true);  
        File.WriteAllText(Application.persistentDataPath + "/player.json", json); 
        Debug.Log("Create json in " + Application.persistentDataPath + "/player.json");
    }

    public static PlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            PlayerData data = JsonUtility.FromJson<PlayerData>(json); 
            return data;
        }
        else
        {
            Debug.Log("No saved player found.");
            return null;
        }
    }
}
