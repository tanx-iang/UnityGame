using UnityEngine;
using TMPro;
using System.Collections;

public class SoulShrineInteractable : MapInteractable
{
    [Header("Upgrade Settings")]
    public int initialCost;
    public float costMultiplier = 2f;
    public float attributeIncrement = 0.1f;

    [Header("UI References")]
    public TMP_Text promptText;          
    public float messageDuration = 2f;  
    private int CurrentCost => Mathf.RoundToInt(initialCost * Mathf.Pow(costMultiplier, player.stats.Level));
    private bool _inMessage = false;

    private PlayerStats stats;
    private PlayerInventory inventory;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        stats = player.GetComponent<PlayerStats>();
        inventory = player.GetComponent<PlayerInventory>();
        if (!_inMessage)
            ShowPrompt();
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        stats = null;
        inventory = null;
        StopAllCoroutines();
        promptText.enabled = false;
        _inMessage = false;
    }

    protected override void Interact()
    {
        if (_inMessage) return;
        if (stats.Level >= stats.maxLevel)
        {
            StartCoroutine(ShowTemporaryMessage($"Max Lever", messageDuration));
            return;
        }

        int cost = CurrentCost;
        if (inventory.Soul >= cost)
        {
            inventory.Soul -= cost;
            stats.Level++;
            stats.attackPower += attributeIncrement;
            stats.defensePower += attributeIncrement;

            StartCoroutine(ShowTemporaryMessage(
                $"Successfully, current level: {stats.Level}", messageDuration));
        }
        else
        {
            int need = cost - inventory.Soul;
            StartCoroutine(ShowTemporaryMessage(
                $"Need {need}", messageDuration));
        }
    }

    private void ShowPrompt()
    {
        promptText.enabled = true;
        promptText.text =
            stats.Level < stats.maxLevel
            ? $"Print {interactKey} to upgrade {stats.Level} to {stats.Level+1}, need {CurrentCost} Soul"
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
