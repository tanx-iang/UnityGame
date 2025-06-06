using UnityEngine;

public class Player : MonoBehaviour, ICombatActor
{
    public PlayerMovement movement;
    public PlayerCombat combat;
    public PlayerStats stats;
    public PlayerInventory inventory;

    private Animator _animator;
    public Animator animator => _animator;  // 只读实现接口
    public Rigidbody2D rb;

    public float attackPower => stats.attackPower;
    public float defensePower => stats.defensePower;

    public int currentHealth
    {
        get => stats.currentHealth;
        set => stats.currentHealth = value;
    }

    public int maxHealth => stats.maxHealth;

    public float maxPoise => stats.maxPoise;

    public float currentPoise
    {
        get => stats.currentPoise;
        set => stats.currentPoise = Mathf.Clamp(value, 0, stats.maxPoise);
    }

    public bool isInvincible{
        get => stats.isInvincible;
        set => stats.isInvincible = value;
    }
    public void Die(){
        stats.Die();
    }
    public void OnPoiseBreak(){
        stats.OnPoiseBreak();
    }


    void Awake()
    {
        Application.targetFrameRate = 60;
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        stats.Init(this);         // 初始化生命等
        movement.Init(this);    // 设置 Rigidbody2D
        combat.Init(this);
        inventory.Init(this);

        stats.OnDeath += HandleDeath;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _animator.SetInteger("WeaponType", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _animator.SetInteger("WeaponType", 1);
        }

        if (!combat.IsAttacking)
        {
            movement.HandleJump();
            movement.HandleRollOrDash();
        }

        if (!movement.IsDashing)
        {
            movement.HandleMovement();
        }

        combat.HandleCombat();

        movement.UpdateDash(Time.deltaTime);

        if (transform.position.y < movement.deathY)
        {
            Respawn();
        }

        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsGrounded", movement.IsGrounded);
        animator.SetBool("IsDashing", movement.IsDashing);
        animator.SetBool("IsRunning", movement.IsRunning);
        animator.SetBool("IsAttacking", combat.IsAttacking);
    }

    private void HandleDeath()
    {
        Respawn();
    }

    public void Respawn()
    {
        transform.position = stats.respawnPoint;          // 位置归位
        rb.velocity = Vector2.zero;
        stats.ResetStatus();
        movement.ResetStatus();
        combat.ResetCombatStatus();
    }
}


