using UnityEngine;
using UnityEngine.UI;
using System;
using GameModule;
using System.Collections;

using System.Diagnostics;


public class PlayerStats : MonoBehaviour
{
    private Animator animator;
    public Vector3 respawnPoint;
    [Header("Health")]
    public int maxHealth = 6;
    public int currentHealth = 6;
    public bool isDead;

    public event Action OnDeath;
    public static event Action onPlayerEnterSafehouse;

    public float maxPoise = 100f;
    public float currentPoise;
    public bool isInvincible = false;

    public int Level = 1;
    public int maxLevel = 30;

    public float attackPower;
    public float defensePower;

    public Equipment Armor{ get; private set; }
    public Weapon weapon{ get; private set; }


    

    public void Init(Player player)
    {
        currentHealth = maxHealth;
        currentPoise = maxPoise;
        this.animator = player.animator;
        respawnPoint = transform.position;
    }

    public void Die(){
        OnDeath?.Invoke();
    }

    public static void PlayerEnterSafehouse(){
        onPlayerEnterSafehouse?.Invoke();
    }    
    
    public void OnPoiseBreak()
    {
        isInvincible = false;
    }


    public void ResetStatus()
    {
        currentHealth = maxHealth;
    }

}

