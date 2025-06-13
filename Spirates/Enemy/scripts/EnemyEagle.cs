using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEagle : EnemyBase
{
    public float speed = 3f;            
    public float segmentDistance = 2f;   

    private Vector2[] directions = {
        Vector2.right,
        Vector2.up,
        Vector2.left,
        Vector2.down
    };
    private Vector3 initialScale;

    protected override void Awake()
    {
        base.Awake();                    
        rb.gravityScale = 0f;           
        initialScale = transform.localScale;
    }

    public override void MoveTo(Vector2 targetPosition)
    {
        Vector2 dir = (targetPosition - (Vector2)transform.position).normalized;
        rb.velocity = dir * speed;
        if (dir.x > 0)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (dir.x < 0)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
       
    }
}
