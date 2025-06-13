using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameModule;

public class AIController : MonoBehaviour
{
    public enum AITemplate { FSM, BehaviorTree }
    public AITemplate template = AITemplate.FSM;

    public EnemyBase      enemy;          
    private EnemyBoss      boss;
    public CombatSystem   combatSystem;   
    public Skill[] skills; 
    public int defaultSkillIndex = 0;
    public IAttackBehavior attackBehavior;
    public Transform      player;         

    public float loadRange;
    public float alertRange;
    public float patrolDistance;

    private IState currentState;

    public BehaviorTreeAsset btAsset;
    private Node rootNode;

    void Awake()
    {
        enemy         = GetComponent<EnemyBase>();
        combatSystem  = GetComponent<CombatSystem>();
        player        = GameObject.FindWithTag("Player").transform;
        if (skills != null && skills.Length > 0 && defaultSkillIndex < skills.Length){
            attackBehavior = skills[defaultSkillIndex];
        }
        else
            attackBehavior = null;
        combatSystem.Init(attacker: enemy, target: player.GetComponent<ICombatActor>(), enemy);

        if (template == AITemplate.FSM)
            InitFSM();
        else if (template == AITemplate.BehaviorTree && btAsset != null){
            rootNode = BehaviorTreeFactory.Create(btAsset, this);
            boss = (EnemyBoss)enemy;
        }
    }

    void Update()
    {
        if (template == AITemplate.FSM)
            currentState?.Tick();
        else
            rootNode?.Tick();
    }

    private void InitFSM()
    {
        ChangeState(new IdleState(this));
    }

    public void ChangeState(IState next)
    {
        currentState?.Exit();
        currentState = next;
        currentState.Enter();
    }

    public bool EvaluateCondition(string key)
    {
        bool result;
        switch (key)
        {
            case "HPAbove50":
                result = boss.currentHealth > boss.maxHealth * 0.5f;
                break;
            case "HPBelowOrEqual50":
                result = boss.currentHealth <= boss.maxHealth * 0.5f;
                break;
            case "InMeleeRange":
                result = Vector2.Distance(boss.transform.position, player.position) 
                        <= attackBehavior.Range;
                break;
            case "OutOfRange":
                result = Vector2.Distance(boss.transform.position, player.position)
                        > alertRange && boss.blinkCooldownTimer <= 0f;
                break;
            case "PlayerDodged":
                result = boss._playerDodged;
                if (result)
                    boss._playerDodged = false;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }


    public NodeState ExecuteAction(string key)
    {
        switch (key)
        {
            case "WalkTowardPlayer":
                boss.MoveTo(player.position);
                return NodeState.Success;

            case "BlinkToPlayer":
                if(boss.blinkCooldownTimer <= 0f){
                    boss.BlinkToward(player.position);
                    return NodeState.Success;
                }else return NodeState.Failure;

            case "Attack1":
            if (!boss._isPlayingAttack)
            {
                boss.rb.velocity = Vector2.zero;
                boss.PlayAttackAnimation("Attack1");
                StartCoroutine(WaitForAnimationToFinish("Attack1", () => 
                {
                    combatSystem.ExecuteHit(skills[0]);
                }));
                return NodeState.Running;
            }
            return NodeState.Success;

            case "Attack2":
            if (!boss._isPlayingAttack)
            {
                boss.rb.velocity = Vector2.zero;
                boss.PlayAttackAnimation("Attack2");
                StartCoroutine(WaitForAnimationToFinish("Attack2", () => 
                {
                    combatSystem.ExecuteHit(skills[1]);
                }));
                return NodeState.Running;
            }
            return NodeState.Success;

            case "RunTowardPlayer":
                boss.RunToward(player.position, 1.5f);
                return NodeState.Success;

            case "Attack3":
                if (!boss._isPlayingAttack)
                {
                boss.rb.velocity = Vector2.zero;
                boss.PlayAttackAnimation("Attack3");
                StartCoroutine(WaitForAnimationToFinish("Attack3", () => 
                {
                combatSystem.ExecuteHit(skills[2]);
                }));
                return NodeState.Running;
            }
            return NodeState.Success;

            case "Attack4":
            if (!boss._isPlayingAttack)
            {
                boss.rb.velocity = Vector2.zero;
                boss.PlayAttackAnimation("Attack4");
                StartCoroutine(WaitForAnimationToFinish("Attack4", () => 
                {
                    combatSystem.ExecuteHit(skills[3]);
                }));
                return NodeState.Running;
            }
            return NodeState.Success;

            case "LeapTeleportAttack":
                if(!boss.isLeapAndSmashCoroutine){
                    StartCoroutine(HandleLeapAndSmash());
                    return NodeState.Running;
                }
                return NodeState.Success;
            default:
                Debug.LogWarning($"Unknown ActionKey: {key}");
                return NodeState.Failure;
        }
    }

    private IEnumerator HandleLeapAndSmash()
    {
        yield return StartCoroutine(boss.LeapAndSmashCoroutine(player.position));
    }

    private IEnumerator WaitForAnimationToFinish(string triggerName, System.Action onAnimationFinished)
    {
        while (boss.IsAttackPlaying())
        {
            yield return null;
        }

        onAnimationFinished?.Invoke();
    }

    
}

