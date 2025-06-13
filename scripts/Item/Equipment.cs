using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Equipment", menuName = "Item/Equipment")]
    public class Equipment : Item
    {
        public float attackBonus;
        public float defenseBonus;
        public float poiseBonus;
        public int Level = 1;
        public int maxLevel = 5;

        public override void Use()
        {
        }

        public override bool IsConsumable()
        {
            return false;
        }
    }
}

