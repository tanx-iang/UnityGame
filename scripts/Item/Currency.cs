using UnityEngine;

namespace GameModule
{
    [CreateAssetMenu(fileName = "Currency", menuName = "Item/Currency")]
    public class Currency : Item
    {
        public int amount;

        public void Add(int value)
        {
        }

        public void Consume(int value)
        {
        }

        public override void Use()
        {
        }

        public override bool IsConsumable()
        {
            return true;
        }
    }
}

