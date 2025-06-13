using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class EnemyFrog : EnemyBase
{
    public float jumpVerticalForce = 10f;    
    public float jumpHorizontalForce = 5f;   

    public float airDrag = 2f;
    private bool isGrounded = false;
    private int direction = 1;
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
    }

    protected override void Awake(){
        base.Awake();
    }

    void Update()
    {
        if (isDead) return;

        rb.drag = isGrounded ? 0f : airDrag;

        if (isGrounded)
        {
            JumpOnce();
        }

        _animator.SetBool("isGrounded", isGrounded);
        _animator.SetFloat("Speed-Y", rb.velocity.y);
    }


    public override void MoveTo(Vector2 targetPosition)
    {
        if (!isGrounded) return;
        direction = targetPosition.x > transform.position.x ? 1 : -1;
        transform.localScale = new Vector3(-initialScale.x * direction, initialScale.y, initialScale.z);
    }

    private void JumpOnce()
    {
        rb.velocity = new Vector2(direction * jumpHorizontalForce, jumpVerticalForce);
        _animator.SetTrigger("Jump");
        isGrounded = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
