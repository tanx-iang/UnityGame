using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using GameModule;

public class ChestInteractable : MapInteractable
{
    public GameObject ItemPickup_Prefab;      
    public List<Item> dropItemAssets;       
    public List<int> dropQuantities;         
    public List<float> dropChances;         
    public float minDrop = 0.8f;          
    public float maxDrop = 1.2f;           
    public float stackOffset = 1.0f;        

    public TMP_Text promptText;            
    public float messageDuration = 2f;     

    private bool _isOpened = false;
    private bool _inMessage = false;

    protected override void OnPlayerEnter()
    {
        base.OnPlayerEnter();
        if (_inMessage) return;

        if (!_isOpened)
        {
            promptText.text = $"print {interactKey} to open";
        }
        else
        {
            promptText.text = "openned chest";
        }
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
        if (_inMessage) return;

        if (!_isOpened)
        {
            OpenChest();
            StartCoroutine(ShowMessage("Open chest", messageDuration));
        }
        else
        {
            StartCoroutine(ShowMessage("openned chest", messageDuration));
        }
    }

    private void OpenChest()
    {
        _isOpened = true;
        SpawnStackedDrops();
    }

    private void SpawnStackedDrops()
    {
        if (ItemPickup_Prefab == null || dropItemAssets.Count == 0)
            return;

        int count = dropItemAssets.Count;
        for (int i = 0; i < count; i++)
        {
            if (i >= dropQuantities.Count || i >= dropChances.Count)
                continue;

            var asset    = dropItemAssets[i];
            int attempts = dropQuantities[i];
            float chance = dropChances[i];

            if (attempts <= 0 || chance <= 0f)
                continue;

            for (int k = 0; k < attempts; k++)
            {
                if (Random.value <= chance)
                {
                    var assetClone = Instantiate(asset);
                    Vector3 pos = transform.position;
                    pos.x += Random.Range(-stackOffset, stackOffset);
                    pos.y += Random.Range(-stackOffset, stackOffset) + 2.0f;

                    var go = Instantiate(ItemPickup_Prefab, pos, Quaternion.identity);
                    var pickup = go.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        if (assetClone is Currency cur)
                        {
                            int baseAmt = cur.amount;
                            float mul = Random.Range(minDrop, maxDrop);
                            cur.amount = Mathf.Max(1, Mathf.RoundToInt(baseAmt * mul));
                        }
                        pickup.itemAsset = assetClone;
                    }
                    var sr = go.GetComponent<SpriteRenderer>();
                    if (sr != null && asset != null)
                        sr.sprite = asset.icon;
                }
            }
        }
    }

    private IEnumerator ShowMessage(string msg, float sec)
    {
        _inMessage = true;
        promptText.enabled = true;
        promptText.text = msg;

        yield return new WaitForSeconds(sec);

        _inMessage = false;
        if (playerInRange && !_isOpened)
        {
            promptText.text = $"print {interactKey} to open";
        }
        else
        {
            promptText.enabled = false;
        }
    }
}
