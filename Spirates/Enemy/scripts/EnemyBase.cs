using UnityEngine;
using System.Collections;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Enemy Config")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected Vector3 originalScale;
    protected Vector3 respawnPosition;
    protected int currentHealth;
    protected bool isDead;
    public int maxHealth = 100;
    public float knockbackForce = 1f;
    public Transform player;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        respawnPosition = transform.position;
        originalScale = transform.localScale;
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, Vector2 knockbackDirection)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} 受到攻击，伤害：{damage}，当前血量：{currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        animator.SetTrigger("Hit");
        rb.AddForce(Vector2.up * knockbackForce, ForceMode2D.Impulse);
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetBool("IsDead", true);
        animator.SetTrigger("Die");
    }

    public virtual void OnDieEnd()
    {
        StartCoroutine(DisappearThenRespawn());
    }

    protected virtual IEnumerator DisappearThenRespawn()
    {
        // 1. 将敌人移到地图外（比如 Y=-999），相当于“消失”
        transform.position = new Vector3(-999, -999, 0);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // 防止物理掉落或触发别的逻辑

        // 2. 等待复活时间
        yield return new WaitForSeconds(3f);

        // 3. 拉回原位置，重置状态
        rb.isKinematic = false;
        transform.position = respawnPosition;
        Respawn();                              // 重置状态
    }

    protected virtual void Respawn()
    {
        isDead = false;
        currentHealth = maxHealth;
        transform.position = respawnPosition;
        animator.SetBool("IsDead", false);
    }
}
