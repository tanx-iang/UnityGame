using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

// DeadState.cs
public class DeadState : IState
{
    private AIController ctrl;
    private EnemyBase    enemy;

    public DeadState(AIController controller)
    {
        ctrl  = controller;
        enemy = controller.enemy;
    }

    public void Enter()
    {
        Debug.Log("Dead");
        enemy.Die();
        PlayerStats.onPlayerEnterSafehouse += OnPlayerBack;
    }

    public void Tick(){}

    public void Exit(){
        PlayerStats.onPlayerEnterSafehouse -= OnPlayerBack;
    }

    private void OnPlayerBack()
    {
        enemy.Respawn();
        ctrl.ChangeState(new PatrolState(ctrl, ctrl.patrolDistance));
        PlayerStats.onPlayerEnterSafehouse -= OnPlayerBack;
    }
}

