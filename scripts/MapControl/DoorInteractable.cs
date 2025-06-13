using UnityEngine;
using TMPro;
using System.Collections;

public class DoorInteractable : MapInteractable
{
    public Transform targetTransform;

    public TMP_Text promptText;         
    public float messageDuration = 2f;  
    private bool _inMessage = false;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        if (promptText != null)
        {
            promptText.text = $"Print {interactKey} to enter";
            promptText.enabled = true;
        }
    }

    protected override void OnPlayerExit()
    {
        base.OnPlayerExit();
        if (promptText != null)
            promptText.enabled = false;
    }

    protected override void Interact()
    {
        if (_inMessage || targetTransform == null || player == null)
            return;

        var rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
            rb.velocity = Vector2.zero;   
            player.transform.position = targetTransform.position;
    }

    private IEnumerator ShowTemporaryMessage(string msg, float seconds)
    {
        _inMessage = true;
        if (promptText != null)
        {
            promptText.text = msg;
            promptText.enabled = true;
        }
        yield return new WaitForSeconds(seconds);
        _inMessage = false;
    }
}
