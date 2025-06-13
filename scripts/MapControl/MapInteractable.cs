using UnityEngine;

public abstract class MapInteractable : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange = 2f;
    public KeyCode interactKey = KeyCode.E;

    protected bool playerInRange = false;
    protected Player player;

    protected virtual void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = true;
            player = col.GetComponent<Player>();
            OnPlayerEnter();
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerInRange = false;
            player = null;
            OnPlayerExit();
        }
    }

    protected abstract void Interact();

    protected virtual void OnPlayerEnter()
    {
    }

    protected virtual void OnPlayerExit()
    {
    }
}

