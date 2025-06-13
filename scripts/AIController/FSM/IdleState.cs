using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private AIController ctrl;
    private EnemyBase enemy;
    private Transform player;
    private float loadRange;

    public IdleState(AIController c)
    {
        ctrl      = c;
        enemy     = c.enemy;
        player    = c.player;
        loadRange = c.loadRange;
    }

    public void Enter()
    {
        Debug.Log("Idle");
        enemy.StopMoving();
        enemy.enabled = false;  
    }

    public void Tick()
    {
        if (Vector2.Distance(enemy.transform.position, player.position) <= loadRange)
        {
            ctrl.ChangeState(new PatrolState(ctrl, ctrl.patrolDistance));
        }
    }

    public void Exit()
    {
        enemy.enabled = true;
    }
}

