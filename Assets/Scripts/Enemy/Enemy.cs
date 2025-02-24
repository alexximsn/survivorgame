using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    private EnemyMovement movement;

    private Player player;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] bool gizmos;

    [SerializeField] private ParticleSystem dieParticles;
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    [SerializeField] private int maxHealth;
    private int health;


    public static Action<int,Vector2> onDamageTaken;

   void Start()
    {
        health = maxHealth;//开始生命值最高
        movement = GetComponent<EnemyMovement>();
        player = FindObjectOfType<Player>();//找到玩家和敌人

        if (player == null)
        {
          
            Destroy(gameObject);
        }
        StartSpawnSequence();
        attackDelay = 1f / attackFrequency;

    }
    private void StartSpawnSequence()//生成敌人生成器
    {
        SetRenderersVisibility(false); 
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.3f;//指定缩放的大小
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).
            setLoopPingPong(4).setOnComplete(SpawnSequenceCompleted);//使用LeanTewwn库对生成器进行缩放

    }
    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true);//敌人出现
        hasSpawned = true;
        collider.enabled = true;//敌人产生后再启动碰撞使得武器识别
        movement.StorePlayer(player);
    }
   
    private void SetRenderersVisibility(bool visibility)//生成器出现，敌人出现；反之
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }
    void Update()
    {
        if (!renderer.enabled)
            return;
        if (attackTimer >= attackDelay)//检测计时器是否达到攻击延迟
            TryAttack();
        else
            Wait();
        movement.FollowPlayer();
    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;//达到攻击延迟
    }
    private void Attack()
    {
        
        attackTimer = 0;
        player.TakeDamage(damage);
    }
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);//确保不超过当前生命值
        health -= realDamage;
      
        onDamageTaken?.Invoke(damage,transform.position);//触发伤害事件，传递伤害值和位置
        if (health <= 0)
            PassAway();

    }
    private void PassAway()
    {
        dieParticles.transform.SetParent(null);
        dieParticles.Play();//播放粒子效果
        Destroy(gameObject);//摧毁敌人
    }
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);//计算二者距离
        if (distanceToPlayer <= playerDetectionRadius)
            Attack();//小于检测半径，攻击
        else
            movement.FollowPlayer();
        
    }
    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;
        Gizmos.color = Color.red;//绘制检测半径
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        //Gizmos.color = Color.White;
    }
}
