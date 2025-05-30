using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("基础移动参数")]
    public float moveSpeed = 5f;
    public float jumpForce = 8f;

    [Header("跳跃设置")]
    public int maxJumps = 2;
    private int jumpCount;

    [Header("冲刺设置")]
    public float dashForce = 30f;
    public float doubleTapTime = 0.3f; // 双击间隔时间
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;

    [Header("蹲下")]
    public KeyCode crouchKey = KeyCode.S;
    public bool isCrouching;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isDashing = false;
    private float dashTime = 0.2f;
    private float dashTimer = 0f;
    private Vector3 respawnPoint;   // 出生点（初始位置）
    public float deathY = -10f;     // Y 轴低于这个值就判定为死亡
    private Vector3 originalScale;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpCount = maxJumps;
        respawnPoint = transform.position;   // 记录初始出生点
        originalScale = transform.localScale; // 保存初始缩放
        animator = GetComponent<Animator>();
    }


    void Update()
{
    if (!isDashing)
    {
        HandleMovement();
    }

    HandleJump();
    HandleDash();
    HandleCrouch();

    if (isDashing)
    {
        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0)
        {
            isDashing = false;
        }
    }

    if (transform.position.y < deathY)
    {
        Respawn();
    }

    float moveInput = Input.GetAxis("Horizontal");
    animator.SetFloat("Speed", Mathf.Abs(moveInput));
    animator.SetBool("IsGrounded", isGrounded);
    animator.SetBool("IsDashing", isDashing);
    animator.SetBool("IsCrouching", isCrouching);

    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

    // 检查 Dash 是否播放完（normalizedTime ≥ 1）
    if (stateInfo.IsName("Dash") && stateInfo.normalizedTime >= 1f)
    {
        // 让 Dash 停止（防止卡状态）
        animator.SetBool("IsDashing", false);

        // 根据是否在地面上决定跳转
        if (isGrounded)
            animator.Play("Idle");
        else
            animator.Play("JumpLoop");
    }
}


    void Respawn()
    {
        transform.position = respawnPoint;   // 传送回出生点
        rb.velocity = Vector2.zero;          // 清除当前速度
        isGrounded = false;                  // 重置状态
        jumpCount = maxJumps;                // 重置跳跃次数
        isDashing = false;                   // 取消冲刺状态（如果你有）
    }


void HandleMovement()
{
    float moveInput = Input.GetAxis("Horizontal");
    rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

    // 翻转角色朝向
    if (moveInput > 0)
    {
        transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
    else if (moveInput < 0)
    {
        transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
}


[Header("可控跳跃")]
public float jumpHoldTime = 0.4f; // 可持续上升时间
private float jumpTimer = 0f;
private bool isJumping = false;

void HandleJump()
{
    if (Input.GetButtonDown("Jump") && jumpCount > 0)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        isJumping = true;
        jumpTimer = jumpHoldTime;
        jumpCount--;
        animator.SetTrigger("JumpTrigger");
    }

    // 长按跳跃
    if (Input.GetButton("Jump") && isJumping)
    {
        if (jumpTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer -= Time.deltaTime;
        }
    }

    // 松开跳跃键，或时间到了就停止上升
    if (Input.GetButtonUp("Jump") || jumpTimer <= 0)
    {
        isJumping = false;
    }
}


    void HandleDash()
    {
        if (isDashing) return;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastRightTapTime < doubleTapTime)
            {
                rb.velocity = new Vector2(dashForce, 0);
                isDashing = true;
                dashTimer = dashTime;
            }
            lastRightTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastLeftTapTime < doubleTapTime)
            {
                rb.velocity = new Vector2(-dashForce, 0);
                isDashing = true;
                dashTimer = dashTime;
            }
            lastLeftTapTime = Time.time;
        }
    }


void HandleCrouch()
{
    Vector3 newScale = transform.localScale;

    if (Input.GetKey(crouchKey))
    {
        isCrouching = true;
        newScale.y = originalScale.y * 0.5f;
    }
    else
    {
        isCrouching = false;
        newScale.y = originalScale.y;
    }

    transform.localScale = newScale;
}



    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y > 0.5f)
            {
                isGrounded = true;
                jumpCount = maxJumps;
                break;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }
}
