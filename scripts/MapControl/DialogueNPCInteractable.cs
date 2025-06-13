using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueNPCInteractable : MapInteractable
{
    [Header("Dialogue Settings")]
    public string npcName;
    [TextArea] public string[] dialogueLines;

    [Header("UI References (assign in Inspector)")]
    public GameObject dialogueUI;     
    public TMP_Text dialogueText;          
    public TMP_Text promptText;          

    private int _currentLine = 0;
    private bool _inDialogue = false;

    protected void Awake()
    {
        if (dialogueUI != null) dialogueUI.SetActive(false);
        if (promptText != null) promptText.enabled = false;
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void OnPlayerEnter()
    {
        if (!_inDialogue && promptText != null)
        {
            promptText.text = $"Print {interactKey} to talk with {npcName}";
            promptText.enabled = true;
        }
    }

    protected override void OnPlayerExit()
    {
        if (!_inDialogue)
        {
            if (promptText != null) promptText.enabled = false;
        }
        else
        {
            EndDialogue();
        }
    }

    protected override void Interact()
    {
        if (!_inDialogue)
            StartDialogue();
        else
            AdvanceDialogue();
    }

    private void StartDialogue()
    {
        if (dialogueUI == null || dialogueText == null) return;
        _inDialogue = true;
        _currentLine = 0;
        dialogueUI.SetActive(true);
        ShowLine(_currentLine);
        if (promptText != null) promptText.enabled = false;
    }

    private void AdvanceDialogue()
    {
        _currentLine++;
        if (_currentLine < dialogueLines.Length)
        {
            ShowLine(_currentLine);
        }
        else
        {
            EndDialogue();
        }
    }

    private void ShowLine(int index)
    {
        dialogueText.text = dialogueLines[index];
    }

    private void EndDialogue()
    {
        _inDialogue = false;
        dialogueUI.SetActive(false);
        promptText.enabled = false;
    }
}