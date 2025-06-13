using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    private AIController ctrl;
    private EnemyBase enemy;
    private Transform enemyT, player;
    private Vector2 leftPoint, rightPoint, targetPoint;
    private float patrolDist, loadRange, alertRange;

    public PatrolState(AIController c, float patrolDistance)
    {
        ctrl         = c;
        enemy        = c.enemy;
        enemyT       = enemy.transform;
        player       = c.player;
        patrolDist   = patrolDistance;
        loadRange    = c.loadRange;
        alertRange   = c.alertRange;

        Vector2 center = enemyT.position;
        leftPoint      = center + Vector2.left  * patrolDist;
        rightPoint     = center + Vector2.right * patrolDist;
        targetPoint    = rightPoint;
    }

    public void Enter()
    {
        Debug.Log("Patrol");
        enemy.enabled = true;
    }

    public void Tick()
    {
        float distToPlayer = Vector2.Distance(enemyT.position, player.position);

        if (distToPlayer <= alertRange)
        {
            enemy.StopMoving();
            ctrl.ChangeState(new ChaseState(ctrl));
            return;
        }

        if (distToPlayer > loadRange)
        {
            enemy.StopMoving();
            ctrl.ChangeState(new IdleState(ctrl));
            return;
        }

        enemy.MoveTo(targetPoint);
        if (Vector2.Distance(enemyT.position, targetPoint) < 0.1f)
            targetPoint = targetPoint == leftPoint ? rightPoint : leftPoint;
    }

    public void Exit()
    {
        enemy.StopMoving();
        enemy.enabled = false;
    }
}
