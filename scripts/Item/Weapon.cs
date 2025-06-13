using System.Collections.Generic;
using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Item/Weapon")]
    public class Weapon : Equipment
    {
        public override void Use()
        {
        }

        public override bool IsConsumable()
        {
            return false;
        }
    }
}

