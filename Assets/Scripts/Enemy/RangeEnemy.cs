using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement),typeof(RangeEnemyAttack))]
public class RangeEnemy : MonoBehaviour
{
    private EnemyMovement movement;
    private RangeEnemyAttack attack;
    private Player player;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    [SerializeField] private Collider2D collider;
    private bool hasSpawned;
    [SerializeField] private float playerDetectionRadius;
    

    [SerializeField] private ParticleSystem dieParticles;
  

    [SerializeField] private int maxHealth;
    private int health;

    [SerializeField] bool gizmos;
    public static Action<int, Vector2> onDamageTaken;
    void Start()
    {
        health = maxHealth;//开始生命值最高


        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<RangeEnemyAttack>();
        player = FindObjectOfType<Player>();//找到玩家和敌人
        attack.StorePlayer(player);
        if (player == null)
        {

            Destroy(gameObject);
        }
        StartSpawnSequence();
     

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
        ManageAttack();
      
    }
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }
    
   
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health);//确保不超过当前生命值
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);//触发伤害事件，传递伤害值和位置
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
        attack.AutoAim();
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
