using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动设置")]
    public float moveSpeed = 3f;
    public float sprintMultiplier = 2f;

    private Rigidbody2D rb;
    private Vector3 originalScale;
    public bool isRunning { get; private set; }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalScale = transform.localScale;
    }

    // 接收横向输入并更新水平速度
    public void Move(float moveInput)
    {
        float currentSpeed = moveSpeed;
        isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;
        if (isRunning){
            currentSpeed *= sprintMultiplier;
        }

        // 直接设置水平速度（如果需要用 AddForce 实现加速，可修改此处逻辑）
        rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

        // 根据输入翻转角色
        if (moveInput > 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }
}
