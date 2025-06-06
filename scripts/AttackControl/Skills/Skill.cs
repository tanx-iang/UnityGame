using UnityEngine;
using GameModule;


[CreateAssetMenu(fileName = "Skill", menuName = "AttackControl/Skills")]
public class Skill : ScriptableObject, IAttackBehavior
{
    [Header("技能参数")]
    public float baseDamage = 10f;
    public float range = 1.5f;
    public float poiseBreak = 30f;
    public float cooldown = 1f;

    private float lastUsedTime = -Mathf.Infinity;

    public float BaseDamage => baseDamage;
    public float Range => range;
    public float PoiseBreak => poiseBreak;
    public float Cooldown => cooldown;

    public bool IsReady()
    {
        return Time.time - lastUsedTime >= cooldown;
    }
}

