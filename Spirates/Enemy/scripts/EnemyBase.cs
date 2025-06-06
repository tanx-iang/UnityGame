using UnityEngine;
using GameModule;
using System.Collections;
using System.Collections.Generic;

public abstract class EnemyBase : MonoBehaviour, ICombatActor
{
    [Header("Enemy Config")]
    public Rigidbody2D rb;
    private Animator _animator;
    public Animator animator => _animator;
    protected Vector3 originalScale;
    protected Vector3 respawnPosition;
    private int _maxHealth = 1;
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
    private float _attackPower;
    public float attackPower => _attackPower;
    private float _defensePower = 2;
    public float defensePower => _defensePower;
    public Transform player;

    public GameObject ItemPickup_Prefab;
    public List<Item> dropItemAssets = new List<Item>();
    public List<int> dropQuantities = new List<int>();
    public List<float> dropChances = new List<float>();
    public float minDrop = 0.8f;
    public float maxDrop = 1.2f;
    public float stackOffset = 1.0f;

    

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }
    }


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        respawnPosition = transform.position;
        originalScale = transform.localScale;
        _currentHealth = _maxHealth;
        _currentPoise = _maxPoise;
        
    }

    public virtual void Die()
    {
        isDead = true;
        _animator.SetBool("IsDead", true);
        _animator.SetTrigger("Die");
    }

    public void OnPoiseBreak()
    {
        StartCoroutine(DisableInvincibilityAfterDelay());
    }

    private IEnumerator DisableInvincibilityAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        _isInvincible = false;
    }

    public virtual void OnDieEnd()
    {
        Debug.Log("Die");
        SpawnStackedDrops();
        StartCoroutine(DisappearThenRespawn());
    }

    private void SpawnStackedDrops()
    {
        if (ItemPickup_Prefab == null || dropItemAssets == null || dropItemAssets.Count == 0)
            return;

        int count = dropItemAssets.Count;
        for (int i = 0; i < count; i++)
        {
            // 确保索引在 dropQuantities 和 dropChances 范围内
            if (i >= dropQuantities.Count || i >= dropChances.Count)
                continue;

            Item asset = dropItemAssets[i];      // 这个 Asset 本身可能包含一个 internal count（如：一次 100 枚）
            int attempts = dropQuantities[i];    // “尝试生成 Prefab 的次数”
            float chance = dropChances[i];       // 每次尝试的掉落概率

            // 0 或以下就不生成
            if (attempts <= 0 || chance <= 0f)
                continue;

            for (int k = 0; k < attempts; k++)
            {
                // 每一次都做一次独立概率判定
                if (Random.value <= chance)
                {
                    // 随机偏移后实例化一个 Prefab
                    Vector3 spawnPos = transform.position;
                    float offsetX = Random.Range(-stackOffset, stackOffset);
                    float offsetY = Random.Range(-stackOffset, stackOffset);
                    spawnPos.x += offsetX;
                    spawnPos.y += offsetY + 2.0f; 
                    // +0.1f 防止一生成就卡在地面里

                    GameObject go = Instantiate(ItemPickup_Prefab, spawnPos, Quaternion.identity);

                    // 拿到 ItemPickup 脚本，把 Asset 赋进去
                    var pickup = go.GetComponent<ItemPickup>();
                    if (pickup != null)
                    {
                        if (asset is Currency currencyAsset)
                        {
                            // 先拿到资产里配置的基础数量
                            int baseAmount = currencyAsset.amount;
                            // 随机倍数 [minDrop, maxDrop]
                            float randMul = Random.Range(minDrop, maxDrop);
                            // 计算最终数量，四舍五入并且至少 1
                            int randomAmt = Mathf.Max(1, Mathf.RoundToInt(baseAmount * randMul));
                            // 给 asset.amount 赋上随机值
                            currencyAsset.amount = randomAmt;
                        }
                        pickup.itemAsset = asset;
                        // 不再手动拆分数量，保持 asset 里定义的堆叠数量（例如 asset.count = 100）
                        // 这里不修改 pickup.quantity，让它默认使用 asset 里配置的值
                    }

                    // 给 Prefab 的 SpriteRenderer 赋图
                    var sr = go.GetComponent<SpriteRenderer>();
                    if (sr != null && asset != null)
                    {
                        sr.sprite = asset.icon;
                    }
                }
            }
        }
    }   



    protected virtual IEnumerator DisappearThenRespawn()
    {
        // 1. 将敌人移到地图外（比如 Y=-999），相当于“消失”
        transform.position = new Vector3(-999, -999, 0);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // 防止物理掉落或触发别的逻辑

        // 2. 等待复活时间
        yield return new WaitForSeconds(3f);

        // 3. 拉回原位置，重置状态
        rb.isKinematic = false;
        transform.position = respawnPosition;
        Respawn();                              // 重置状态
    }

    protected virtual void Respawn()
    {
        isDead = false;
        _currentHealth = _maxHealth;
        _currentPoise = _maxPoise;
        _isInvincible = false; 
        transform.position = respawnPosition;
        _animator.SetBool("IsDead", false);
    }
}
