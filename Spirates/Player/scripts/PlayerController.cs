using UnityEngine;

public class playerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerJump playerJump;
    private PlayerDash playerDash;
    private PlayerCombat playerCombat;
    private Rigidbody2D rb;
    private Animator animator;
    
    public float deathY = -10f;
    private Vector3 respawnPoint;
    
    // 用于判断是否在地面，可以通过碰撞回调更新
    public bool isGrounded = false;
    
    void Awake()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        playerJump = GetComponent<PlayerJump>();
        playerDash = GetComponent<PlayerDash>();
        playerCombat = GetComponent<PlayerCombat>();
    }
    
    void Start()
    {
        respawnPoint = transform.position;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            animator.SetInteger("WeaponType", 0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            animator.SetInteger("WeaponType", 1);
        }
        // 处理移动：攻击和冲刺期间一般不干扰移动
        float moveInput = Input.GetAxis("Horizontal");
        if (!playerCombat.isAttacking)
        {
            playerJump.HandleJump();
            playerDash.HandleDash(isGrounded);
            playerCombat.HandleCombat(isGrounded);
        }
        if (!playerDash.isDashing){
            playerMovement.Move(moveInput);
        }
        
        // 如果玩家位置低于死亡阈值，则重生
        if (transform.position.y < deathY)
            Respawn();

        if (playerMovement.isRunning){
            Debug.Log("Running");
        }

        // 更新 Animator 参数
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsDashing", playerDash.isDashing);
        animator.SetBool("IsRunning", playerMovement.isRunning);
        //animator.SetBool("IsDead", isDead);
        animator.SetBool("IsAttacking", playerCombat.isAttacking);
    }
    
    void Respawn()
    {
        transform.position = respawnPoint;
        rb.velocity = Vector2.zero;
        isGrounded = false;
        playerJump.ResetJumps();
    }
    
    // 碰撞检测更新 isGrounded 状态
    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                playerJump.ResetJumps();
                break;
            }
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
