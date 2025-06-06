using UnityEngine;

namespace GameModule
{
    public abstract class Item : ScriptableObject
    {
        public string itemName;
        public string description;
        public Sprite icon;

        public abstract void Use();
        public abstract bool IsConsumable();
    }
}

