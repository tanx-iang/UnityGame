using UnityEngine;
using GameModule;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class EnemyBoss : EnemyBase
{
    public float speed = 2f; 

    public bool _playerDodged = false;
    public bool isGrounded = true;
    public bool isSmashing = false;
    public bool _isPlayingAttack = false;
    public bool isLeapAndSmashCoroutine = false;
    public Image HealthBar;

    public float blinkForce = 30f;
    public float blinkDuration = 0.2f;
    public float blinkCooldown  = 5.0f;
    private bool isBlinking = false;
    private float blinkTimer = 0f; 
    public float blinkCooldownTimer = 0f;

    public Skill attackBehavior;

    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (isBlinking)
        {
            blinkTimer -= Time.deltaTime;
            if (blinkTimer <= 0f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                isBlinking = false;
                blinkCooldownTimer = blinkCooldown;
            }
        }
        if (blinkCooldownTimer > 0f)
        {
            blinkCooldownTimer -= Time.deltaTime;
        }
        float vx = rb.velocity.x;
        if (vx > 0.01f)
        {
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        else if (vx < -0.01f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        }
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        if (HealthBar != null)
        {
            float ratio = (float)currentHealth / maxHealth;
            HealthBar.fillAmount = Mathf.Clamp01(ratio);
        }
    }

    public void PlayAttackAnimation(string triggerName)
    {
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Attack4");
        _isPlayingAttack = true;
        animator.SetTrigger(triggerName);
    }

    public bool IsAttackPlaying()
    {
        var st = animator.GetCurrentAnimatorStateInfo(0);
        if (st.IsTag("Attack") && st.normalizedTime < 1f)
            return true;
        if (_isPlayingAttack)
            _isPlayingAttack = false;
        return false;
    }

    public override void MoveTo(Vector2 targetPosition)
    {
        float dx = targetPosition.x - transform.position.x;
        float dirX = Mathf.Sign(dx);                    
        rb.velocity = new Vector2(dirX * speed, rb.velocity.y);

        if (dirX > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (dirX < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    }

    public void RunToward (Vector2 targetPosition, float speedMul) {

        float dx = targetPosition.x - transform.position.x;
        float dirX = Mathf.Sign(dx);
        rb.velocity = new Vector2(dirX * speed * speedMul, rb.velocity.y);

        if (dirX > 0)
            transform.localScale = new Vector3(-Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
        else if (dirX < 0)
            transform.localScale = new Vector3(Mathf.Abs(originalScale.x), originalScale.y, originalScale.z);
    
    }

    public void BlinkToward(Vector2 target)
    {
        if (isBlinking || blinkCooldownTimer > 0f) return;

        float dir = (target.x > transform.position.x) ? 1f : -1f;

        animator.SetTrigger("Dash");

        rb.velocity = new Vector2(dir * blinkForce, rb.velocity.y);

        isBlinking  = true;
        blinkTimer  = blinkDuration;
    }

    public IEnumerator LeapAndSmashCoroutine(Vector2 playerPos)
    {
        isLeapAndSmashCoroutine = true;
        rb.velocity = Vector2.zero;

        animator.SetTrigger("JumpUp");
        rb.AddForce(Vector2.up * 50f, ForceMode2D.Impulse);

        yield return new WaitForSeconds(1f);

        Vector3 pos = transform.position;
        pos.x = playerPos.x;
        transform.position = pos;

        rb.velocity = Vector2.zero;
        animator.SetTrigger("JumpDown");
        rb.AddForce(Vector2.down * 50f, ForceMode2D.Impulse);
        isSmashing = true;

        yield return new WaitUntil(() => isGrounded);

        isSmashing = false;
        animator.SetTrigger("Idle");
        isLeapAndSmashCoroutine = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Debug.Log("Dodged");
            _playerDodged = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = true;

            if (isSmashing && col.otherCollider.CompareTag("Player"))
            {
                combatSystem.ExecuteHit(attackBehavior);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
