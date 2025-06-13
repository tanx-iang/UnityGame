using System.Collections.Generic;
using UnityEngine;
using GameModule;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("Heart Bar Settings")]
    public Image[] heartImages;         
    public Sprite fullHeartSprite;      
    public Sprite halfHeartSprite;      
    public Sprite emptyHeartSprite;     

    [Header("Player Attributes")]
    public TMP_Text attackText;             
    public TMP_Text defenseText;            
    public TMP_Text poiseText;              
    public TMP_Text Gold;  
    public TMP_Text Soul;  
    public Image poiseFillImage;        
    
    [Header("Equipment Icons")]
    public Image weaponIconImage;       
    public Image armourIconImage;       

    private Player player;
    private PlayerInventory inventory;

    public void Awake()
    {
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

        UpdateHeartBar(player.stats.currentHealth);

        UpdateAttributes();

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
        Weapon w = inventory.GetEquippedWeapon();
        if (w != null && w.icon != null)
        {
            weaponIconImage.sprite = w.icon;
            weaponIconImage.color = Color.white;
        }
        else
        {
            weaponIconImage.sprite = null;
            weaponIconImage.color = new Color(1, 1, 1, 0);
        }

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
