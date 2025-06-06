using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "KeyItem", menuName = "Item/KeyItem")]
    public class KeyItem : Item
    {
        public string targetId;

        public bool CanUnlock(string doorId)
        {
            return targetId == doorId;
        }

        public override void Use()
        {
            // TODO: 触发开门逻辑
        }

        public override bool IsConsumable()
        {
            return true;
        }
    }
}

