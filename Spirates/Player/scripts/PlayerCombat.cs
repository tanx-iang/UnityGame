using UnityEngine;
using GameModule;
using System.Collections;
using System.Collections.Generic; 

public class PlayerCombat : MonoBehaviour
{
    public Transform attackPoint;
    public LayerMask enemyLayer;

    private Animator animator;
    private Player player;
    public bool IsAttacking { get; private set; }

    public List<ScriptableObject> skillAssets;
    private Dictionary<string, IAttackBehavior> skillMap = new();

    public CombatSystem combat;

    public void Init(Player player)
    {
        this.animator = player.animator;
        this.player = player;
        combat = new CombatSystem();
        combat.Init(player, null, player);
        foreach (var so in skillAssets)
        {
            if (so is IAttackBehavior skill)
                skillMap[so.name] = skill;
        }
    }

    public void HandleCombat()
    {
        if (IsAttacking) return;

        // 判断是否在地面
        if (player.movement.IsGrounded && !player.movement.isJumping)
        {
            // 地面攻击
            if (Input.GetKeyDown(KeyCode.J))
            {
                IsAttacking = true;

                if (Input.GetKey(KeyCode.W))
                {
                    animator.SetTrigger("Attack3");  // 地面普通上击
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger("Attack1");  // 地面普通下砸
                }
                else
                {
                    animator.SetTrigger("Attack2");  // 地面普通前击
                }
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                IsAttacking = true;

                if (Input.GetKey(KeyCode.W))
                {
                    animator.SetTrigger("Attack4");  // 地面重上击
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    animator.SetTrigger("Attack6");  // 地面重下劈
                }
                else
                {
                    animator.SetTrigger("Attack5");  // 地面重前冲
                }
            }
        }
        else  // 角色不在地面时
        {
            if (animator.GetInteger("WeaponType") == 1)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    IsAttacking = true;

                    if (Input.GetKey(KeyCode.W))
                    {
                        animator.SetTrigger("Attack7");  // 空中上击
                    }
                    else
                    {
                        animator.SetTrigger("Attack8");  // 空中前冲
                    }
                }
            }
        }
    }

    public void TriggerAttack(string skillName)
    {
        if (skillMap.TryGetValue(skillName, out var attack))
        {
            AttackEnemies(attack); 
        }
        else
        {
            Debug.LogWarning($"can not find：{skillName}");
        }
    }

    public void AttackEnemies(IAttackBehavior currentAttack)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, currentAttack.Range, enemyLayer);
        bool hitEnemy = false;
        foreach (Collider2D hit in hits)
        {
             ICombatActor target = hit.GetComponent<ICombatActor>();
            if (target != null)
            {
                hitEnemy = true;
                combat.Init(player, target, player); // this 是 MonoBehaviour 脚本
                combat.ExecuteHit(currentAttack);
            }
        }
        if(!hitEnemy){
            Debug.Log("Miss");
        }
    }

    public void EndAttack()
    {
        IsAttacking = false;
        if(!IsAttacking){
            animator.SetTrigger("DefaultTrigger");
        }
    }

    public void ResetCombatStatus()
    {
        IsAttacking = false;
    }
}

