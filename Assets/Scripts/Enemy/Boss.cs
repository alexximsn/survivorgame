using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public class Boss : Enemy
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Animator animator;
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

    // Update is called once per frame
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
        if (attackCounter >= 8) // Ê¾Àý£º¹¥»÷3´ÎºóÍ£Ö¹
        {
            SetIdleState();
        }
    }

    private void StartMovingState()
    {
        state = State.Moving;
        targetPosition = GetRandomPosition();

        animator.Play("BossMove");
    }

    private void ManageMovingState()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, movespeed * Time.deltaTime);
        if (Vector2.Distance(transform.position,targetPosition)<.01f)
            StartAttackingState();
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

    private Vector2 GetRandomPosition()
    {
        Vector2 targetPosition = Vector2.zero;
        targetPosition.x =Random.Range( -10f, 10f);
        targetPosition.y = Random.Range( -4f, 4f);
        return targetPosition;
    }
    private void Attack()
    {
        Vector2 direction = Quaternion.Euler(0, 0, -45 * attackCounter) * Vector2.up;
        attack.InstantShoot(direction);
        attackCounter++;
    }
    public override void PassAway()
    {
        onBossPassedAway?.Invoke(transform.position);
        PassAwayAfterWave();
    }

}
