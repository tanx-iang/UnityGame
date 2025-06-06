using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class PlayerInventory : MonoBehaviour
{
    public int Gold = 0;
    public int Soul = 0;
    public List<KeyItem> keys = new List<KeyItem>();
    private Player player;
    private Weapon equippedWeapon;
    private Equipment equippedArmour;

    public GameObject droppedEquipmentPrefab;
    private List<ItemPickup> nearbyPickups = new List<ItemPickup>();
    private float dropYOffset = 2.0f;

    public void Init(Player player)
    {
        this.player = player;
        Gold = 0;
        Soul = 0;
        keys = new List<KeyItem>(); // 清空/初始化背包内容
        equippedWeapon = null;
        equippedArmour = null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickUpNearest();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            PrintNearbyList();
        }
    }
    //test
    private void PrintNearbyList()
    {
        if (nearbyPickups == null || nearbyPickups.Count == 0)
        {
            Debug.Log("附近没有可拾取物品。");
            return;
        }

        Debug.Log($"附近有 {nearbyPickups.Count} 个可拾取物品：");
        for (int i = 0; i < nearbyPickups.Count; i++)
        {
            var pickup = nearbyPickups[i];
            if (pickup == null)
            {
                Debug.Log($"  [{i}] = null");
            }
            else
            {
                // 如果你想看它的 Asset 名称：
                string assetName = pickup.itemAsset != null ? pickup.itemAsset.itemName : "null Asset";
                // 或者看它在场景里对应的 GameObject 名称： pickup.gameObject.name
                Debug.Log($"  [{i}] = {pickup.gameObject.name} （Asset：{assetName}）");
            }
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

        // 1. 获取列表中第一个 ItemPickup
        ItemPickup firstPick = nearbyPickups[0];
        nearbyPickups.RemoveAt(0);
        if (firstPick != null)
        {
            // 2. 真正执行拾取逻辑（AddItem / SwapWeapon / SwapArmour）
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

    public bool UseItem(Item item, int useCount = 1)
    {
        if (item == null) return false;
        if (item is Currency){
            if (item.itemName == "Gold"){
                if (Gold >= useCount){
                    Gold -= useCount;
                    return true;
                }
                return false;
            }
            if (item.itemName == "Soul"){
                if (Soul >= useCount){
                    Soul -= useCount;
                    return true;
                }
                return false;
            }
        }
        if (item is KeyItem key){

        }
        if (item is Weapon || item is Equipment){
            return false;
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
                //UI给玩家选择,交换返回true，否则false
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

        // —— 私有辅助：应用/移除装备属性 —— //

    private void ApplyWeaponStats(Weapon weapon)
    {
        if (player == null || weapon == null) return;
        player.stats.attackPower += weapon.attackBonus;
    }

    private void RemoveWeaponStats(Weapon weapon)
    {
        if (player == null || weapon == null) return;
        player.stats.attackPower -= weapon.attackBonus;
    }

    private void ApplyArmourStats(Equipment armour)
    {
        if (player == null || armour == null) return;
        player.stats.defensePower += armour.defenseBonus;
        player.stats.maxPoise += armour.poiseBonus;
        // TODO: 其他加成
    }

    private void RemoveArmourStats(Equipment armour)
    {
        if (player == null || armour == null) return;
        player.stats.defensePower -= armour.defenseBonus;
        player.stats.maxPoise -= armour.poiseBonus;
    }
}
