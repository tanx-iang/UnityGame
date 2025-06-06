using System.Collections.Generic;
using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class Weapon : Equipment
    {
        public override void Use()
        {
            // TODO: 装备武器逻辑
        }

        public override bool IsConsumable()
        {
            return false;
        }
    }
}

