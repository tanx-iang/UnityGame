using UnityEngine;
using TMPro;
using GameModule;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
public class LockedDoorInteractable : MapInteractable
{
    public List<string> requiredKeyIds = new List<string>();

    public TMP_Text promptText;
    public float messageDuration = 2f;

    private bool _inMessage = false;
    private bool _isOpen = false;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        if (_inMessage || _isOpen) return;

        promptText.text = BuildPromptText();
        promptText.enabled = true;
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        StopAllCoroutines();
        promptText.enabled = false;
        _inMessage = false;
    }

    protected override void Interact()
    {
        if (_inMessage || _isOpen || player.inventory == null) return;

        foreach (var keyId in requiredKeyIds)
        {
            if (!player.inventory.HasKey(keyId))
            {
                StartCoroutine(ShowMessage($"Need more keys"));
                return;
            }
        }

        foreach (var keyId in requiredKeyIds)
            player.inventory.RemoveKey(keyId);

        OpenDoor();
        StartCoroutine(ShowMessage("Door has been opened, Good Luck"));
    }

    private void OpenDoor()
    {
        _isOpen = true;

        GetComponent<Collider2D>().enabled = false;
        promptText.enabled = false;
    }

    private string BuildPromptText()
    {
        if (_isOpen) return "";

        string txt = $"Print {interactKey} to use ";
        for (int i = 0; i < requiredKeyIds.Count; i++)
        {
            txt += requiredKeyIds[i];
            if (i < requiredKeyIds.Count - 1)
                txt += " + ";
        }
        return txt;
    }

    private IEnumerator ShowMessage(string msg)
    {
        _inMessage = true;
        promptText.enabled = true;
        promptText.text = msg;

        yield return new WaitForSeconds(messageDuration);

        _inMessage = false;
        if (playerInRange && !_isOpen)
            promptText.text = BuildPromptText();
        else
            promptText.enabled = false;
    }
}
