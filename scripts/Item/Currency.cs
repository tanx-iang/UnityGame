using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Item/Currency")]
    public class Currency : Item
    {
        public int amount;

        public void Add(int value)
        {
            // TODO: 增加货币
        }

        public void Consume(int value)
        {
            // TODO: 消耗货币
        }

        public override void Use()
        {
            // TODO: 可用于自动消耗的逻辑（如购买）
        }

        public override bool IsConsumable()
        {
            return true;
        }
    }
}

