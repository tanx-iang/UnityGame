using UnityEngine;
using GameModule;

[RequireComponent(typeof(Collider2D))]
public class ItemPickup : MonoBehaviour
{
    public Item itemAsset;    

    private SpriteRenderer spriterenderer;

    private bool isPlayerNearby = false;

    private void Awake()
    {
        spriterenderer = GetComponent<SpriteRenderer>();
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
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
