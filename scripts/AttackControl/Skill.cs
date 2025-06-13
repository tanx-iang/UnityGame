using UnityEngine;
using GameModule;
using System;


[CreateAssetMenu(fileName = "Skill", menuName = "AttackControl/Skills")]
public class Skill : ScriptableObject, IAttackBehavior
{
    public float baseDamage = 10f;
    public float range = 1.5f;
    public float poiseBreak = 30f;
    public float cooldown = 1f;

    public float BaseDamage => baseDamage;
    public float Range => range;
    public float PoiseBreak => poiseBreak;
    public float Cooldown => cooldown;
}

