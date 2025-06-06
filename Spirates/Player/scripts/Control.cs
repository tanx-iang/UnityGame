// using UnityEngine;

// public class Control : MonoBehaviour
// {

//     [Header("基础移动参数")]
//     public float moveSpeed = 3f;
//     public float jumpForce = 8f;

//     [Header("奔跑")]
//     bool isRunning = false;
//     public float sprintMultiplier = 2f; // 跑步速度倍率

//     [Header("跳跃设置")]
//     public int maxJumps = 2;
//     private int jumpCount;

//     [Header("冲刺设置")]
//     public float dashForce = 30f;
//     public float doubleTapTime = 0.3f;
//     private float lastLeftTapTime = -1f;
//     private float lastRightTapTime = -1f;
//     private float dashTime = 0.2f;
//     private float dashTimer = 0f;

//     [Header("可控跳跃")]
//     public float jumpHoldTime = 0.4f;
//     private float jumpTimer = 0f;
//     private bool isJumping = false;

//     [Header("战斗状态")]
//     public bool isAttacking = false;
//     //public bool isDead = false;
//     //public bool isHurt = false;

//     [Header("攻击检测")]
//     public Transform attackPoint;        // 攻击判定中心（挂在武器或手的位置）
//     public float attackRange = 1f;       // 攻击范围半径
//     public LayerMask enemyLayer;         // 指定只打敌人

//     [Header("血量设置")]
//     public int maxHealth = 6;
//     public int currentHealth = 6;

//     public HeartBarUI heartBarUI; // 拖到 Inspector 上



//     private Rigidbody2D rb;
//     private bool isGrounded;
//     private bool isDashing = false;
//     private Vector3 respawnPoint;
//     public float deathY = -10f;
//     private Vector3 originalScale;
//     private Animator animator;

//     void Awake() {
//     Application.targetFrameRate = 60;  // 锁定为每秒60帧
//     }

//     void Start()
//     {
//         rb = GetComponent<Rigidbody2D>();
//         jumpCount = maxJumps;
//         respawnPoint = transform.position;
//         originalScale = transform.localScale;
//         animator = GetComponent<Animator>();
//         heartBarUI.UpdateHearts(currentHealth);
//     }

//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Alpha1))
//         {
//             animator.SetInteger("WeaponType", 0);
//         }

//         if (Input.GetKeyDown(KeyCode.Alpha2))
//         {
//             animator.SetInteger("WeaponType", 1);
//         }

//         if (!isAttacking)
//         {
//             HandleJump();
//             HandleRollOrDash();
            
//         }
//         if (!isDashing){
//             HandleMovement();
//         }
//         HandleCombat();

//         if (isDashing)
//         {
//             dashTimer -= Time.deltaTime;
//             if (dashTimer <= 0)
//             {
//                 isDashing = false;
//             }
//         }

//         if (transform.position.y < deathY)
//         {
//             Respawn();
//         }

//         animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
//         animator.SetBool("IsGrounded", isGrounded);
//         animator.SetBool("IsDashing", isDashing);
//         animator.SetBool("IsRunning", isRunning);
//         //animator.SetBool("IsDead", isDead);
//         animator.SetBool("IsAttacking", isAttacking);
//     }

//     void Respawn()
//     {
//         currentHealth = 6;
//         heartBarUI.UpdateHearts(currentHealth);
//         transform.position = respawnPoint;
//         rb.velocity = Vector2.zero;
//         isGrounded = false;
//         jumpCount = maxJumps;
//         isDashing = false;
//     }

//     void HandleMovement()
//     {
//         float moveInput = Input.GetAxis("Horizontal");
//         float currentSpeed = moveSpeed;

//         isRunning = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(moveInput) > 0.1f;

//         if (isRunning)
//         {
//             currentSpeed *= sprintMultiplier;
//         }

//         rb.velocity = new Vector2(moveInput * currentSpeed, rb.velocity.y);

//         if (moveInput > 0)
//         {
//             transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
//         }
//         else if (moveInput < 0)
//         {
//             transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
//         }
//     }

//     void HandleJump()
//     {
//         if (Input.GetButtonDown("Jump") && jumpCount > 0)
//         {
//             rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//             isJumping = true;
//             jumpTimer = jumpHoldTime;
//             jumpCount--;
//             animator.SetTrigger("JumpTrigger");
//         }

//         if (Input.GetButton("Jump") && isJumping)
//         {
//             if (jumpTimer > 0)
//             {
//                 rb.velocity = new Vector2(rb.velocity.x, jumpForce);
//                 jumpTimer -= Time.deltaTime;
//             }
//         }

//         if (Input.GetButtonUp("Jump") || jumpTimer <= 0)
//         {
//             isJumping = false;
//         }
//     }

//     void HandleCombat()
//     {
//         if (isAttacking) return;  // 如果已经在攻击状态，直接返回

//         // 判断是否在地面
//         if (isGrounded && !isJumping)
//         {
//             // 地面攻击
//             if (Input.GetKeyDown(KeyCode.J))
//             {
//                 isAttacking = true;

//                 if (Input.GetKey(KeyCode.W))
//                 {
//                     animator.SetTrigger("Attack3");  // 地面普通上击
//                 }
//                 else if (Input.GetKey(KeyCode.S))
//                 {
//                     animator.SetTrigger("Attack1");  // 地面普通下砸
//                 }
//                 else
//                 {
//                     animator.SetTrigger("Attack2");  // 地面普通前击
//                 }
//             }
//             else if (Input.GetKeyDown(KeyCode.K))
//             {
//                 isAttacking = true;

//                 if (Input.GetKey(KeyCode.W))
//                 {
//                     animator.SetTrigger("Attack4");  // 地面重上击
//                 }
//                 else if (Input.GetKey(KeyCode.S))
//                 {
//                     animator.SetTrigger("Attack6");  // 地面重下劈
//                 }
//                 else
//                 {
//                     animator.SetTrigger("Attack5");  // 地面重前冲
//                 }
//             }
//         }
//         else  // 角色不在地面时
//         {
//             if (animator.GetInteger("WeaponType") == 1)
//             {
//                 if (Input.GetKeyDown(KeyCode.J))
//                 {
//                     isAttacking = true;

//                     if (Input.GetKey(KeyCode.W))
//                     {
//                         animator.SetTrigger("Attack7");  // 空中上击
//                     }
//                     else
//                     {
//                         animator.SetTrigger("Attack8");  // 空中前冲
//                     }
//                 }
//             }
//         }
//     }




//     void HandleRollOrDash()
//     {
//         if (isDashing) return;

//         if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
//         {
//             if (Time.time - lastRightTapTime < doubleTapTime)
//             {
//                 DoRollOrDash(1);
//             }
//             lastRightTapTime = Time.time;
//         }

//         if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
//         {
//             if (Time.time - lastLeftTapTime < doubleTapTime)
//             {
//                 DoRollOrDash(-1);
//             }
//             lastLeftTapTime = Time.time;
//         }
//     }

//     void DoRollOrDash(int direction)
//     {
//         if (isGrounded)
//         {
//             animator.SetTrigger("RollTrigger");
//             //animator.Play("Roll");
//         }
//         else
//         {
//             animator.SetTrigger("DashTrigger");
//             //animator.Play("Dash");
//         }

//         rb.velocity = new Vector2(direction * dashForce, 0);
//         isDashing = true;
//         dashTimer = dashTime;
//     }

//     private void OnCollisionEnter2D(Collision2D collision)
//     {
//         foreach (ContactPoint2D contact in collision.contacts)
//         {
//             if (contact.normal.y > 0.5f)
//             {
//                 isGrounded = true;
//                 jumpCount = maxJumps;
//                 break;
//             }
//         }
//     }

//     private void OnCollisionExit2D(Collision2D collision)
//     {
//         isGrounded = false;
//     }

//     // 可供动画末尾事件调用
//     public void EndAttack()
//     {
//         isAttacking = false;
//         if(!isAttacking){
//             animator.SetTrigger("DefaultTrigger");
//         }
//         //animator.Play("Default");
//     }

//     public void DamageEnemies()
//     {
//         // 使用 OverlapBox/OverlapCircle 检测攻击范围内的敌人
//         Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
//         foreach (Collider2D hit in hits)
//         {
//             Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
//             hit.GetComponent<EnemyBase>()?.TakeDamage(10, knockbackDir);
//         }
//     }
//     public void TakeDamage(int amount)
//     {
//         currentHealth -= amount;
//         currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

//         if (heartBarUI != null)
//         {
//             heartBarUI.UpdateHearts(currentHealth);
//         }

//         if (currentHealth <= 0)
//         {
//             Respawn(); // 或 Die()
//         }
//     }


//     // public void Die()
//     // {
//     //     isDead = true;
//     //     animator.SetBool("IsDead", true);
//     //     rb.velocity = Vector2.zero;
//     // }
// }