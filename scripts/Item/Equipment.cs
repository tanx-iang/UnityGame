using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Item/Equipment")]
    public class Equipment : Item
    {
        public float attackBonus;
        public float defenseBonus;
        public float poiseBonus;

        public override void Use()
        {
            // TODO: 装备穿戴逻辑
        }

        public override bool IsConsumable()
        {
            return false;
        }
    }
}

