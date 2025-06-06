using UnityEngine;

public class TestEnemy : EnemyBase
{
    [Header("巡逻参数")]
    public float patrolSpeed = 2f;
    public float patrolDistance = 3f;

    [Header("玩家追击参数")]
    public float detectRange = 5f;
    public float attackRange = 1.2f;
    public float chaseSpeed = 3f;

    [Header("攻击设置")]
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -Mathf.Infinity;


    private bool movingRight = true;
    private float leftBound;
    private float rightBound;

    private enum State { Patrolling, Chasing }
    private State currentState = State.Patrolling;


    protected override void Awake()
    {
        base.Awake();
        leftBound = respawnPosition.x - patrolDistance;
        rightBound = respawnPosition.x + patrolDistance;
    }


    void Update()
    {
        if (isDead) return;  // 死亡后不移动
        UpdateState();         // ① 处理状态切换
        HandleBehavior();      // ② 根据状态执行行为
        UpdateAnimation();     // ③ 统一更新动画状态
    }

    private void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Chasing;  // 在Chasing状态中触发攻击逻辑
        }
        else if (distanceToPlayer <= detectRange)
        {
            currentState = State.Chasing;
        }
        else
        {
            currentState = State.Patrolling;
        }
    }

    private void Patrol()
    {
        float moveDir = movingRight ? 1f : -1f;

        // 移动
        rb.velocity = new Vector2(moveDir * patrolSpeed, rb.velocity.y);

        // 翻转朝向
        if (moveDir > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);

        // 到达边界反转方向
        if (transform.position.x >= rightBound && movingRight)
        {
            movingRight = false;
        }
        else if (transform.position.x <= leftBound && !movingRight)
        {
            movingRight = true;
        }
    }
    private void ChasePlayer()
    {
        Vector2 dir = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(dir.x * chaseSpeed, rb.velocity.y);

        if (dir.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    private void HandleBehavior()
    {
        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                break;

            case State.Chasing:
                float distance = Vector2.Distance(transform.position, player.position);
                if (distance <= attackRange)
                {
                    rb.velocity = Vector2.zero;
                    if (Time.time - lastAttackTime >= attackCooldown)
                    {
                        animator.SetFloat("Speed", 0);
                        //animator.SetTrigger("Attack");
                        //player.GetComponent<PlayerStats>()?.TakeDamage(1);
                        lastAttackTime = Time.time;  // 记录攻击时间
                    }
                }
                else
                {
                    ChasePlayer();
                }
                break;
        }
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
    }


}

