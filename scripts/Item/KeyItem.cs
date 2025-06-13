using UnityEngine;
using System.Linq;

namespace GameModule
{
    [CreateAssetMenu(fileName = "KeyItem", menuName = "Item/KeyItem")]
    public class KeyItem : Item
    {
        public string targetId;

        public KeyItem(string targetId){
            this.targetId = targetId;
        }

        public bool CanUnlock(string doorId)
        {
            return targetId == doorId;
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

