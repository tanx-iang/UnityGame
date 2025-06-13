using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnState : IState
{
    private AIController ctrl;
    private EnemyBase enemy;
    private Vector2 homePoint;
    private float threshold = 1f;

    public ReturnState(AIController c)
    {
        ctrl      = c;
        enemy     = c.enemy;
        homePoint = enemy.respawnPosition; 
    }

    public void Enter()
    {
        Debug.Log("Return");
        enemy.enabled = true;
    }

    public void Tick()
    {
        enemy.MoveTo(homePoint);
        if (Vector2.Distance(enemy.transform.position, homePoint) < threshold)
        {
            enemy.StopMoving();
            ctrl.ChangeState(new PatrolState(ctrl, ctrl.patrolDistance));
        }
    }

    public void Exit()
    {
        enemy.StopMoving();
        enemy.enabled = false;
    }
}

