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
        health = maxHealth;//��ʼ����ֵ���


        movement = GetComponent<EnemyMovement>();
        attack = GetComponent<RangeEnemyAttack>();
        player = FindObjectOfType<Player>();//�ҵ���Һ͵���
        attack.StorePlayer(player);
        if (player == null)
        {

            Destroy(gameObject);
        }
        StartSpawnSequence();
     

    }
    private void StartSpawnSequence()//���ɵ���������
    {
        SetRenderersVisibility(false);
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.3f;//ָ�����ŵĴ�С
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).
            setLoopPingPong(4).setOnComplete(SpawnSequenceCompleted);//ʹ��LeanTewwn�����������������

    }
    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true);//���˳���
        hasSpawned = true;
        collider.enabled = true;//���˲�������������ײʹ������ʶ��
        movement.StorePlayer(player);
    }

    private void SetRenderersVisibility(bool visibility)//���������֣����˳��֣���֮
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
        int realDamage = Mathf.Min(damage, health);//ȷ����������ǰ����ֵ
        health -= realDamage;

        onDamageTaken?.Invoke(damage, transform.position);//�����˺��¼��������˺�ֵ��λ��
        if (health <= 0)
            PassAway();

    }
    private void PassAway()
    {
        dieParticles.transform.SetParent(null);
        dieParticles.Play();//��������Ч��
        Destroy(gameObject);//�ݻٵ���
    }
    private void TryAttack()
    {
        attack.AutoAim();
    }

    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;
        Gizmos.color = Color.red;//���Ƽ��뾶
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        //Gizmos.color = Color.White;
    }
}
