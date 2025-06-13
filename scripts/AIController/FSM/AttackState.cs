using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class AttackState : IState
{
    private AIController ctrl;
    private EnemyBase enemy;
    private IAttackBehavior behavior;
    private float lastAttackTime = -Mathf.Infinity;

    public AttackState(AIController controller)
    {
        ctrl     = controller;
        enemy    = controller.enemy;               
        behavior = controller.attackBehavior;
    }

    public void Enter()
    {
        Debug.Log("Attack");
        enemy.StopMoving();
    }

    public void Tick()
    {
        float now = Time.time;
        if (enemy.currentHealth <= 0)
        {
            ctrl.ChangeState(new DeadState(ctrl));
            return;
        }

        float dist = Vector2.Distance(enemy.transform.position, ctrl.player.position);

        if (dist > behavior.Range)
        {
            enemy.isInvincible = false;
            ctrl.ChangeState(new ChaseState(ctrl));
            return;
        }
        if (now >= lastAttackTime + behavior.Cooldown)
        {

            lastAttackTime = now;
            Debug.Log("AttackBehavior");
            ctrl.combatSystem.ExecuteHit(behavior);
        }
    }

    public void Exit()
    {
    }
}
