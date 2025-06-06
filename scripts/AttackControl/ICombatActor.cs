using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICombatActor
{
    bool isInvincible { get; set;}
    float attackPower { get; }
    float defensePower { get; }
    int currentHealth { get; set; }
    int maxHealth { get; }
    float maxPoise { get; }
    float currentPoise { get; set; }
    Animator animator { get; }

    void Die();
    void OnPoiseBreak();

}

