using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [Header("跳跃设置")]
    public float jumpForce = 8f;
    public int maxJumps = 2;
    public float jumpHoldTime = 0.4f;
    
    private int jumpCount;
    private float jumpTimer;
    private bool isJumping = false;
    private Rigidbody2D rb;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        jumpCount = maxJumps;
    }

    // 在 Update 或 FixedUpdate 中调用此方法
    public void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpTimer = jumpHoldTime;
            jumpCount--;
            animator.SetTrigger("JumpTrigger");
        }
        if (Input.GetButton("Jump") && isJumping)
        {
            if (jumpTimer > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                jumpTimer -= Time.deltaTime;
            }
        }
        if (Input.GetButtonUp("Jump") || jumpTimer <= 0)
        {
            isJumping = false;
        }
    }
    
    // 重置跳跃次数（在落地时调用）
    public void ResetJumps()
    {
        jumpCount = maxJumps;
    }
}

