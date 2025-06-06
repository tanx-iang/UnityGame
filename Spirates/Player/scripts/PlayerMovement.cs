using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("基础移动参数")]
    public float moveSpeed = 3f;
    public float sprintMultiplier = 2f;
    public float jumpForce = 8f;
    public int maxJumps = 2;
    public float dashForce = 30f;
    public float deathY = -10f;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    private int jumpCount;
    public bool isJumping = false;
    private float jumpHoldTime = 0.4f;
    private float jumpTimer;

    public bool IsGrounded { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsRunning { get; private set; }

    private float doubleTapTime = 0.3f;
    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    private float dashTime = 0.2f;
    private float dashTimer;

    public void Init(Player player)
    {
        this.rb = player.rb;
        jumpCount = maxJumps;
        this.animator = player.animator;
        originalScale = transform.localScale;
    }

    public void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        float speed = moveSpeed;

        IsRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;
        if (IsRunning) speed *= sprintMultiplier;

        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        if (moveInput > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }

    }

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

        if (Input.GetButton("Jump") && isJumping && jumpTimer > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpTimer -= Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") || jumpTimer <= 0)
            isJumping = false;
    }

    public void HandleRollOrDash()
    {
        if (IsDashing) return;

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (Time.time - lastRightTapTime < doubleTapTime) DoRollOrDash(1);
            lastRightTapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (Time.time - lastLeftTapTime < doubleTapTime) DoRollOrDash(-1);
            lastLeftTapTime = Time.time;
        }
    }

    private void DoRollOrDash(int direction)
    {
        if (IsGrounded)
        {
            animator.SetTrigger("RollTrigger");
        }
        else
        {
            animator.SetTrigger("DashTrigger");
        }

        rb.velocity = new Vector2(direction * dashForce, 0);
        IsDashing = true;
        dashTimer = dashTime;
    }

    public void UpdateDash(float deltaTime)
    {
        if (IsDashing)
        {
            dashTimer -= deltaTime;
            if (dashTimer <= 0) IsDashing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            foreach (var contact in collision.contacts)
            {
                if (contact.normal.y > 0.9f)
                {
                    IsGrounded = true;
                    jumpCount = maxJumps;
                    break;
                }
            }
        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }


    public void ResetStatus()
    {
        jumpCount = maxJumps;
        IsDashing = false;
        IsGrounded = false;
    }
}

