using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class CombatSystem
{
    private ICombatActor attacker;
    private ICombatActor target;
    private MonoBehaviour context;

    public void Init(ICombatActor attacker, ICombatActor target, MonoBehaviour context){
        this.attacker = attacker;
        this.target = target;
        this.context = context;
    }

    public void ExecuteHit(IAttackBehavior attack){
        if (target.isInvincible) return;
        int damage = Mathf.RoundToInt(attack.BaseDamage * Mathf.Max(0, (attacker.attackPower - target.defensePower)));
        ReducePoise(attack.PoiseBreak); //放攻击者的削韧
        TakeDamage(target,damage);
    }

    private void TakeDamage(ICombatActor target, int damage){
        target.currentHealth -= damage;
        target.currentHealth = Mathf.Clamp(target.currentHealth, 0, target.maxHealth);
        if (target.currentHealth <= 0)
        {
            target.Die();
        }
    }

    private void ReducePoise(float amount){
        target.currentPoise -= amount;
        Debug.Log(target.currentPoise);
        if (target.currentPoise <= 0){
            BreakPoise();
        }
    }

    private void BreakPoise(){
        target.isInvincible = true;
        target.animator.SetTrigger("Hit");
        target.currentPoise = target.maxPoise;
    }
}
