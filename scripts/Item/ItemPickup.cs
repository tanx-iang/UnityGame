using UnityEngine;
using GameModule;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public Item itemAsset;    // 在 Inspector 里拖入一个具体的 Item Asset，比如 ConsumableItem、Equipment、KeyItem、Currency

    private SpriteRenderer spriterenderer;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        // 确保 Collider2D 是 Trigger
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        // 运行时动态把 itemAsset.icon 赋给 SpriteRenderer
        if (itemAsset != null && spriterenderer != null)
        {
            spriterenderer.sprite = itemAsset.icon;
        }
        Destroy(gameObject, 30f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (isPlayerNearby) return;
        isPlayerNearby = true;
        // 把自己加到 PlayerInventory 或单例里存的“附近可拾列表”里
        var inv = other.GetComponent<PlayerInventory>();
         if (inv == null) return;

        if (itemAsset is Currency || itemAsset is KeyItem)
        {
            inv.AddItem(itemAsset);
            Destroy(gameObject);
        }
        else
        {
            inv.RegisterNearbyPickup(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!isPlayerNearby) return;
        isPlayerNearby = false;
        // 从“附近可拾列表”里移除自己
        var inv = other.GetComponent<PlayerInventory>();
        if (inv != null)
            inv.UnregisterNearbyPickup(this);
    }

    public void PickupByPlayer(PlayerInventory inventory)
    {
        if (itemAsset == null) return;

        if (itemAsset is Weapon weapon)
        {
            inventory.Equip(weapon);
        }
        else if (itemAsset is Equipment armour)
        {
            inventory.Equip(armour);
        }

        Destroy(gameObject);
    }
}
