using UnityEngine;

namespace GameModule
{
    public interface IAttackBehavior
    {
        float BaseDamage { get; }
        float Range { get; }
        float PoiseBreak { get; }
        float Cooldown { get; }

        bool IsReady();
    }
}

