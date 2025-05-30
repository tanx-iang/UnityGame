using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;
    public bool isAttacking { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 参数 isGrounded 由 PlayerController 传入
    public void HandleCombat(bool isGrounded)
    {
        if (isAttacking) return;
        
        // 地面攻击
        if (isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                isAttacking = true;
                if (Input.GetKey(KeyCode.W))
                    animator.SetTrigger("Attack3");  // 地面普通上击
                else if (Input.GetKey(KeyCode.S))
                    animator.SetTrigger("Attack1");  // 地面普通下砸
                else
                    animator.SetTrigger("Attack2");  // 地面普通前击
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                isAttacking = true;
                if (Input.GetKey(KeyCode.W))
                    animator.SetTrigger("Attack4");  // 地面重上击
                else if (Input.GetKey(KeyCode.S))
                    animator.SetTrigger("Attack6");  // 地面重下劈
                else
                    animator.SetTrigger("Attack5");  // 地面重前冲
            }
        }
        // 空中攻击：仅允许武器为1时执行
        else
        {
            if (animator.GetInteger("WeaponType") == 1 && Input.GetKeyDown(KeyCode.J))
            {
                isAttacking = true;
                if (Input.GetKey(KeyCode.W))
                    animator.SetTrigger("Attack7");  // 空中上击
                else if (Input.GetKey(KeyCode.S))
                    animator.SetTrigger("Attack8");  // 空中下劈
                else
                    animator.SetTrigger("Attack9");  // 空中前冲
            }
        }
    }
    
    // 在动画事件中调用此方法结束攻击
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetTrigger("DefaultTrigger");
    }
}
