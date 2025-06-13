using UnityEngine;
using TMPro;
using System.Collections;

public class WeaponShrineInteractable : MapInteractable
{
    [Header("Upgrade Settings")]
    public int initialCost;
    public float costMultiplier = 2f;
    public float attributeIncrement = 0.1f;

    [Header("UI References")]
    public TMP_Text promptText;          
    public float messageDuration = 2f;   
    private int CurrentCost => Mathf.RoundToInt(initialCost * Mathf.Pow(costMultiplier, inventory.equippedWeapon.Level));
    private bool _inMessage = false;

    private PlayerInventory inventory;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        inventory = player.GetComponent<PlayerInventory>();
        if (inventory.equippedWeapon == null)
        {
            StartCoroutine(ShowTemporaryMessage("no weapon", messageDuration));
            return;
        }
        if (!_inMessage)
            ShowPrompt();
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        inventory = null;
        StopAllCoroutines();
        promptText.enabled = false;
        _inMessage = false;
    }

    protected override void Interact()
    {
        if (inventory.equippedWeapon == null)
        {
            StartCoroutine(ShowTemporaryMessage("no weapon", messageDuration));
            return;
        }
        if (_inMessage) return;
        if (inventory.equippedWeapon.Level >= inventory.equippedWeapon.maxLevel)
        {
            StartCoroutine(ShowTemporaryMessage($"Max Lever", messageDuration));
            return;
        }

        int cost = CurrentCost;
        if (inventory.Gold >= cost)
        {
            inventory.Gold -= cost;
            inventory.equippedWeapon.Level++;
            inventory.RemoveWeaponStats(inventory.equippedWeapon);
            inventory.equippedWeapon.attackBonus += attributeIncrement;
            inventory.ApplyWeaponStats(inventory.equippedWeapon);

            StartCoroutine(ShowTemporaryMessage(
                $"Successfully, current level: {inventory.equippedWeapon.Level}", messageDuration));
        }
        else
        {
            int need = cost - inventory.Gold;
            StartCoroutine(ShowTemporaryMessage(
                $"Need {need}", messageDuration));
        }
    }

    private void ShowPrompt()
    {
        if (inventory == null || inventory.equippedWeapon == null)
        {
            promptText.enabled = false;
            return;
        }
        promptText.enabled = true;
        promptText.text =
            inventory.equippedWeapon.Level < inventory.equippedWeapon.maxLevel
            ? $"Print {interactKey} to upgrade {inventory.equippedWeapon.Level} to {inventory.equippedWeapon.Level+1}, need {CurrentCost} Gold"
            : $"Max lever";
    }

    private IEnumerator ShowTemporaryMessage(string msg, float duration)
    {
        _inMessage = true;
        promptText.enabled = true;
        promptText.text = msg;

        yield return new WaitForSeconds(duration);

        _inMessage = false;
        if (playerInRange)
            ShowPrompt();
        else
            promptText.enabled = false;
    }
}
