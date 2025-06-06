using System.Collections.Generic;
using UnityEngine;
using GameModule;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Heart Bar Settings")]
    public Image[] heartImages;         // 三个 Image
    public Sprite fullHeartSprite;      // 满心图
    public Sprite halfHeartSprite;      // 半心图
    public Sprite emptyHeartSprite;     // 空心图

    [Header("Player Attributes")]
    public TMP_Text attackText;             // 攻击力文本
    public TMP_Text defenseText;            // 防御力文本
    public TMP_Text poiseText;              // 韧性文本 (如 "30 / 100")
    public TMP_Text Gold;  
    public TMP_Text Soul;  
    public Image poiseFillImage;        // 韧性进度条 (可选)

    // —— Equipment Icons —— //
    [Header("Equipment Icons")]
    public Image weaponIconImage;       // 武器图标
    public Image armourIconImage;       // 防具图标

    // —— 对玩家和背包的引用 —— //
    private Player player;
    private PlayerInventory inventory;

    public void Awake()
    {
        // 获取场景中标签为 "Player" 的物体，并缓存 Player 与 PlayerInventory
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.GetComponent<Player>();
            inventory = playerObj.GetComponent<PlayerInventory>();
        }
    }

    public void Update(){
        RefreshAllUI();
    }

    public void RefreshAllUI()
    {
        if (player == null || inventory == null) return;

        // 1. 更新心形血条
        UpdateHeartBar(player.stats.currentHealth);

        // 2. 更新属性文字和进度条
        UpdateAttributes();

        // 3. 更新装备图标
        UpdateEquipmentIcons();
    }

    public void UpdateHeartBar(int health)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = Mathf.Clamp(health - i * 2, 0, 2);
            switch (heartValue)
            {
                case 2:
                    heartImages[i].sprite = fullHeartSprite;
                    break;
                case 1:
                    heartImages[i].sprite = halfHeartSprite;
                    break;
                default:
                    heartImages[i].sprite = emptyHeartSprite;
                    break;
            }
        }
    }

    private void UpdateAttributes()
    {
        // 假设 Player.stats.attackPower、defensePower、currentPoise、maxPoise 都可访问
        attackText.text = $"AttackPower: {player.stats.attackPower}";
        defenseText.text = $"DefensePower: {player.stats.defensePower}";
        poiseText.text = $"Max Poise: {player.stats.maxPoise}";
        Gold.text = $"Gold: {inventory.Gold}";
        Soul.text = $"Soul: {inventory.Soul}";

        if (poiseFillImage != null)
        {
            float ratio = (float)player.stats.currentPoise / player.stats.maxPoise;
            poiseFillImage.fillAmount = Mathf.Clamp01(ratio);
        }
    }

    private void UpdateEquipmentIcons()
    {
        // 武器
        Weapon w = inventory.GetEquippedWeapon();
        if (w != null && w.icon != null)
        {
            weaponIconImage.sprite = w.icon;
            weaponIconImage.color = Color.white;
        }
        else
        {
            // 隐藏或显示空槽：这里设置为透明
            weaponIconImage.sprite = null;
            weaponIconImage.color = new Color(1, 1, 1, 0);
        }

        // 防具
        Equipment a = inventory.GetEquippedArmour();
        if (a != null && a.icon != null)
        {
            armourIconImage.sprite = a.icon;
            armourIconImage.color = Color.white;
        }
        else
        {
            armourIconImage.sprite = null;
            armourIconImage.color = new Color(1, 1, 1, 0);
        }
    }

}
