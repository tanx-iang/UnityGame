using System.Collections.Generic;
using UnityEngine;
using GameModule;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public int Gold = 0;
    public int Soul = 0;
    public List<KeyItem> keys = new List<KeyItem>();
    private Player player;
    public Weapon equippedWeapon;
    public Equipment equippedArmour;

    public GameObject droppedEquipmentPrefab;
    private List<ItemPickup> nearbyPickups = new List<ItemPickup>();
    private float dropYOffset = 2.0f;

    public void Init(Player player)
    {
        this.player = player;
        Gold = 0;
        Soul = 0;
        keys = new List<KeyItem>(); 
        equippedWeapon = null;
        equippedArmour = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpNearest();
        }
    }



    public void RegisterNearbyPickup(ItemPickup pickup)
    {
        if (!nearbyPickups.Contains(pickup))
            nearbyPickups.Add(pickup);
    }
    public void UnregisterNearbyPickup(ItemPickup pickup)
    {
        if (nearbyPickups.Contains(pickup))
            nearbyPickups.Remove(pickup);
    }
    private void TryPickUpNearest()
    {
        if (nearbyPickups.Count == 0) return;

        ItemPickup firstPick = nearbyPickups[0];
        nearbyPickups.RemoveAt(0);
        if (firstPick != null)
        {
            firstPick.PickupByPlayer(this);
        }
    }

    public void AddItem(Item item)
    {
        if(item == null) return;
        if (item is Currency currency){
            string name = currency.itemName;
            if (name == "Gold"){
                Gold += currency.amount;
            }else if (name == "Soul"){
                Soul += currency.amount;
            }
        return;
        }
        if (item is KeyItem key){
            keys.Add(key);
            return;
        }
        if (item is Weapon weapon){
            return;
        }
        if (item is Equipment armour){
            return;
        }
    }

    public bool HasKey(string doorId)
    {
        return keys.Any(k => k.CanUnlock(doorId));
    }

    public bool RemoveKey(string doorId)
    {
        var key = keys.FirstOrDefault(k => k.CanUnlock(doorId));
        if (key != null)
        {
            keys.Remove(key);
            return true;
        }
        return false;
    }

    public void Equip(Equipment equipment)
    {
        if (equipment == null) return;
        if (equipment is Weapon weapon){
            if (equippedWeapon == null){
                equippedWeapon = weapon;
                ApplyWeaponStats(weapon);
            }
            else{
                SwapWeapon(weapon);
                Debug.Log("Swap weapon");
            }
            return;
        }
        if (equippedArmour == null){
            equippedArmour = equipment;
            ApplyArmourStats(equipment);
        }
        else{
            SwapArmour(equipment);
            Debug.Log("Swap armour");
        }
    }

    public void SwapWeapon(Weapon weapon)
    {
        if (weapon == null) return;
        if (equippedWeapon != null && droppedEquipmentPrefab != null && player != null){
            Vector3 dropPos = player.transform.position;
            dropPos.y += dropYOffset;
            GameObject go = Instantiate(droppedEquipmentPrefab, dropPos, Quaternion.identity);
            var pickup = go.GetComponent<ItemPickup>();
            if (pickup != null){
                pickup.itemAsset = equippedWeapon;
            }
            RemoveWeaponStats(equippedWeapon);
        }
        equippedWeapon = weapon;
        ApplyWeaponStats(weapon);
    }

    public void SwapArmour(Equipment armour){
        if (armour == null) return;
        if (equippedArmour != null && droppedEquipmentPrefab != null && player != null){
            Vector3 dropPos = player.transform.position;
            dropPos.y += dropYOffset;
            GameObject go = Instantiate(droppedEquipmentPrefab, dropPos, Quaternion.identity);
            var pickup = go.GetComponent<ItemPickup>();
            if(pickup != null){
                pickup.itemAsset = equippedArmour;
            }
            RemoveArmourStats(equippedArmour);
        }
        equippedArmour = armour;
        ApplyArmourStats(armour);
    }

    public Weapon GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public Equipment GetEquippedArmour()
    {
        return equippedArmour;
    }


    public void ApplyWeaponStats(Weapon weapon)
    {
        if (player == null || weapon == null) return;
        player.stats.attackPower += weapon.attackBonus;
    }

    public void RemoveWeaponStats(Weapon weapon)
    {
        if (player == null || weapon == null) return;
        player.stats.attackPower -= weapon.attackBonus;
    }

    public void ApplyArmourStats(Equipment armour)
    {
        if (player == null || armour == null) return;
        player.stats.defensePower += armour.defenseBonus;
        player.stats.maxPoise += armour.poiseBonus;
    }

    public void RemoveArmourStats(Equipment armour)
    {
        if (player == null || armour == null) return;
        player.stats.defensePower -= armour.defenseBonus;
        player.stats.maxPoise -= armour.poiseBonus;
    }
}
