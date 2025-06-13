using UnityEngine;
using TMPro;
using System.Collections;

public class ArmourShrineInteractable : MapInteractable
{
    [Header("Upgrade Settings")]
    public int initialCost;
    public float costMultiplier = 2f;
    public float attributeIncrement = 0.1f;

    [Header("UI References")]
    public TMP_Text promptText;        
    public float messageDuration = 2f;  
    private int CurrentCost => Mathf.RoundToInt(initialCost * Mathf.Pow(costMultiplier, inventory.equippedArmour.Level));
    private bool _inMessage = false;

    private PlayerInventory inventory;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        inventory = player.GetComponent<PlayerInventory>();
        if (inventory.equippedArmour == null)
        {
            StartCoroutine(ShowTemporaryMessage("no armour", messageDuration));
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
        if (inventory.equippedArmour == null)
        {
            StartCoroutine(ShowTemporaryMessage("no armour", messageDuration));
            return;
        }
        if (_inMessage) return;
        if (inventory.equippedArmour.Level >= inventory.equippedArmour.maxLevel)
        {
            StartCoroutine(ShowTemporaryMessage($"Max Lever", messageDuration));
            return;
        }

        int cost = CurrentCost;
        if (inventory.Gold >= cost)
        {
            inventory.Gold -= cost;
            inventory.equippedArmour.Level++;
            inventory.RemoveArmourStats(inventory.equippedArmour);
            inventory.equippedArmour.defenseBonus += attributeIncrement;
            inventory.equippedArmour.poiseBonus += attributeIncrement;
            inventory.ApplyArmourStats(inventory.equippedArmour);

            StartCoroutine(ShowTemporaryMessage(
                $"Successfully, current level: {inventory.equippedArmour.Level}", messageDuration));
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
        if (inventory == null || inventory.equippedArmour == null)
        {
            promptText.enabled = false;
            return;
        }
        promptText.enabled = true;
        promptText.text =
            inventory.equippedArmour.Level < inventory.equippedArmour.maxLevel
            ? $"Print {interactKey} to upgrade {inventory.equippedArmour.Level} to {inventory.equippedArmour.Level+1}, need {CurrentCost} Gold"
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
