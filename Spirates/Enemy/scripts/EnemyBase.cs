using UnityEngine;
using GameModule;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour, ICombatActor
{
    [Header("Enemy Config")]
    public Rigidbody2D rb;
    protected Animator _animator;
    public Animator animator => _animator;
    protected Vector3 originalScale;
    public Vector3 respawnPosition;
    protected CombatSystem combatSystem;
    public int _maxHealth = 5;
    public int maxHealth => _maxHealth;
    private int _currentHealth;
    public int currentHealth{
        get => _currentHealth;
        set => _currentHealth = Mathf.Clamp(value, 0, _maxHealth);
    }
    protected bool isDead;

    private float _maxPoise = 100f;
    public float maxPoise => _maxPoise;

    private float _currentPoise;
    public float currentPoise{
        get => _currentPoise;
        set => _currentPoise = Mathf.Clamp(value, 0, _maxPoise);
    }
    private bool _isInvincible;
    public bool isInvincible{
        get => _isInvincible;
        set => _isInvincible = value;
    }
    public float _attackPower = 1;
    public float attackPower => _attackPower;
    public float _defensePower = 1;
    public float defensePower => _defensePower;
    public Transform player;
    public ICombatActor playerActor;

    public GameObject ItemPickup_Prefab;
    public List<Item> dropItemAssets = new List<Item>();
    public List<int> dropQuantities = new List<int>();
    public List<float> dropChances = new List<float>();
    public float minDrop = 0.8f;
    public float maxDrop = 1.2f;
    public float stackOffset = 1.0f;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        combatSystem = GetComponent<CombatSystem>();
        originalScale = transform.localScale;
        var go = GameObject.FindGameObjectWithTag("Player");
        if (go != null){
            playerActor = go.GetComponent<ICombatActor>();
            player = go.transform;
        }
        combatSystem.Init(this,playerActor,this);
        respawnPosition = transform.position;
        originalScale = transform.localScale;
        _currentHealth = _maxHealth;
        _currentPoise = _maxPoise;
        
    }

    public abstract void MoveTo(Vector2 targetPosition);

    public virtual void StopMoving(){
        rb.velocity = Vector2.zero;
    }

    public virtual void Die()
    {
        isDead = true;
        _animator.SetBool("IsDead", true);
        _animator.SetTrigger("Die");
        _isInvincible = true;
    }

    public void OnPoiseBreak()
    {
        Debug.Log(_currentHealth);
        _isInvincible = false;
    }

    public virtual void OnDieEnd()
    {
        Debug.Log("Die");
        SpawnStackedDrops();
        transform.position = new Vector3(-999, -999, 0);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void SpawnStackedDrops()
    {
        if (ItemPickup_Prefab == null || dropItemAssets == null || dropItemAssets.Count == 0)
            return;

        int count = dropItemAssets.Count;
        for (int i = 0; i < count; i++)
        {
            if (i >= dropQuantities.Count || i >= dropChances.Count)
                continue;

            Item asset = dropItemAssets[i];      
            int attempts = dropQuantities[i];    
            float chance = dropChances[i];       

            if (attempts <= 0 || chance <= 0f)
                continue;

            for (int k = 0; k < attempts; k++)
            {
                if (Random.value <= chance)
                {
                    var assetClone = Instantiate(asset);
                    Vector3 spawnPos = transform.position;
                    float offsetX = Random.Range(-stackOffset, stackOffset);
                    float offsetY = Random.Range(-stackOffset, stackOffset);
                    spawnPos.x += offsetX;
                    spawnPos.y += offsetY + 2; 

                    GameObject go = Instantiate(ItemPickup_Prefab, spawnPos, Quaternion.identity);

                    var pickup = go.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        if (assetClone is Currency currencyClone)
                        {
                            int baseAmount = currencyClone.amount;
                            float randMul = Random.Range(minDrop, maxDrop);
                            int randomAmt = Mathf.Max(1, Mathf.RoundToInt(baseAmount * randMul));
                            currencyClone.amount = randomAmt;
                        }
                        pickup.itemAsset = assetClone;
                    }

                    var sr = go.GetComponent<SpriteRenderer>();
                    if (sr != null && asset != null)
                    {
                        sr.sprite = asset.icon;
                    }
                }
            }
        }
    }

    public virtual void Respawn()
    {
        rb.isKinematic = false;
        transform.position = respawnPosition;
        isDead = false;
        _currentHealth = _maxHealth;
        _currentPoise = _maxPoise;
        _isInvincible = false; 
        transform.position = respawnPosition;
        _animator.SetBool("IsDead", false);
    }
}
