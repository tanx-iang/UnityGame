using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class SafehouseTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats.PlayerEnterSafehouse();
            var player = other.GetComponent<Player>();
            player.stats.ResetStatus();
            player.movement.ResetStatus();
            player.combat.ResetCombatStatus();
            player.stats.respawnPoint = player.transform.position;
            SaveSystem.SavePlayer(player);
        }
    }
}
