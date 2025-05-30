using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("冲刺设置")]
    public float dashForce = 30f;
    public float dashTime = 0.2f;
    public float doubleTapTime = 0.3f;

    private float lastLeftTapTime = -1f;
    private float lastRightTapTime = -1f;
    private float dashTimer;
    
    public bool isDashing { get; private set; }
    
    private Rigidbody2D rb;
    private Animator animator;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    // 在 Update 或 FixedUpdate 中调用冲刺检测
    public void HandleDash(bool isGrounded)
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
                isDashing = false;
        }
        if (!isDashing)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Time.time - lastRightTapTime < doubleTapTime)
                {
                    DoDash(1,isGrounded);
                }
                lastRightTapTime = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (Time.time - lastLeftTapTime < doubleTapTime)
                {
                    DoDash(-1,isGrounded);
                }
                lastLeftTapTime = Time.time;
            }
        }
    }
    
    void DoDash(int direction,bool isGrounded)
    {
        // 依据是否在地面播放 RollTrigger 或 DashTrigger
        if (isGrounded)
        {
            animator.SetTrigger("RollTrigger");
        }
        else
        {
            animator.SetTrigger("DashTrigger");
        }
        rb.velocity = new Vector2(direction * dashForce, 0);
        isDashing = true;
        dashTimer = dashTime;
    }
}
