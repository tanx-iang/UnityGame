using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class ChaseState : IState
{
    private AIController ctrl;
    private EnemyBase enemy;
    private Transform player;
    private IAttackBehavior attackBehavior;
    private float alertRange;

    public ChaseState(AIController c)
    {
        ctrl       = c;
        enemy      = c.enemy;
        player     = c.player;
        alertRange = c.alertRange;
        attackBehavior = c.attackBehavior;
    }

    public void Enter()
    {
        Debug.Log("Chase");
        enemy.enabled = true;
    }

    public void Tick()
    {
        float dist = Vector2.Distance(enemy.transform.position, player.position);
        if (dist <= attackBehavior.Range)
        {
            enemy.StopMoving();
            ctrl.ChangeState(new AttackState(ctrl));
            return;
        }

        if (dist > alertRange)
        {
            enemy.StopMoving();
            ctrl.ChangeState(new ReturnState(ctrl));
            return;
        }

        enemy.MoveTo(player.position);
    }

    public void Exit()
    {
        enemy.StopMoving();
        enemy.enabled = false;
    }
}

