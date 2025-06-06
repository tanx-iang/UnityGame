// using System.Collections.Generic;
// using UnityEngine;

// namespace GameModule
// {
//     public class ComboAttack
//     {
//         private List<IAttackBehavior> comboSteps = new();
//         private int currentIndex = 0;
//         private float comboCD = 1.0f;
//         private float lastUsedTime = -Mathf.Infinity;

//         public ComboAttack(List<IAttackBehavior> steps, float cooldown)
//         {
//             comboSteps = steps;
//             comboCD = cooldown;
//         }

//         /// <summary>
//         /// 执行当前的 comboStep 攻击
//         /// </summary>
//         public void ExecuteCombo(Transform attacker, Transform target)
//         {
//             if (!IsReady() || comboSteps.Count == 0) return;

//             var step = comboSteps[currentIndex];
//             if (step.IsReady())
//             {
//                 step.ExecuteAttack(attacker, target);
//                 currentIndex = (currentIndex + 1) % comboSteps.Count;
//                 lastUsedTime = Time.time;
//             }
//         }

//         public bool IsReady()
//         {
//             return Time.time - lastUsedTime >= comboCD;
//         }

//         public void Reset()
//         {
//             currentIndex = 0;
//             lastUsedTime = -Mathf.Infinity;
//         }
//     }
// }

