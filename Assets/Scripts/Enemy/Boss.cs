using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Boss : Enemy
{
    [SerializeField] private Slider healthBar;//生命值
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Animator animator;//动画
    //private EnemyMovement movement;
    enum State { None,Idle,Moving,Attacking}

    private State state;
    private float timer;

    [SerializeField] private float maxIdleDuration;
    private float idleDuration;
    private Vector2 targetPosition;
    private RangeEnemyAttack attack;

    [SerializeField] private float movespeed;
    private int attackCounter;


    private void Awake()
    {
        state = State.None;
        healthBar.gameObject.SetActive(false);
        onSpawnSequenceComplleted += onSpawnSequenceComplletedCallback;
        onDamageTaken += DamageTakenCallback;
    }

   
    private void onSpawnSequenceComplletedCallback()
    {
        healthBar.gameObject.SetActive(true);
        UpdateHealthBar();
        SetIdleState();
    }

    private void OnDestroy()
    {
        onSpawnSequenceComplleted -= onSpawnSequenceComplletedCallback;
        onDamageTaken -= DamageTakenCallback;
    }
    protected override void Start()
    {
        base.Start();
        attack = GetComponent<RangeEnemyAttack>();
    }
    void Update()
    {
        ManageStates();
        
    }
    private void ManageStates()
    {
        switch(state)
        {
            case State.Idle:
                ManageIdleState();
                break;
            case State.Moving:
                ManageMovingState();
                break;
            case State.Attacking:
                ManageAttackingState();
                break;
            default:
                break;
        }
    }
    private void SetIdleState()
    {
        state = State.Idle;
        idleDuration = Random.Range(1f, maxIdleDuration);
        animator.Play("BossIdle");
    }
    private void ManageAttackingState()
    {
        if (attackCounter >= 8) 
        {
            SetIdleState();
        }
    }

    private void StartMovingState()
    {
        state = State.Moving;
       

        animator.Play("BossMove");
    }

    private void ManageMovingState()
    {
        movement.FollowPlayer();

        // 根据距离或其他条件触发攻击（示例逻辑）
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
        {
            StartAttackingState();
        }
    }

    private void StartAttackingState()
    {
        state = State.Attacking;
        attackCounter = 0;
        animator.Play("BossAttack");
    }

    private void ManageIdleState()
    {
        timer += Time.deltaTime;
        if (timer >= idleDuration)
        {
            timer = 0;
            StartMovingState();
        }
    }

    private void DamageTakenCallback(int damage, Vector2 position, bool iscritical)
    {
        UpdateHealthBar();
    }
    private void UpdateHealthBar()
    {
        healthBar.value = (float)health / maxHealth;
        healthText.text = $"{health}/{maxHealth}";
    }
    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.up;//回旋效果
        attack.InstantShoot(direction);
        attackCounter++;
    }
    public override void PassAway()
    {
        onBossPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();
    }

}
