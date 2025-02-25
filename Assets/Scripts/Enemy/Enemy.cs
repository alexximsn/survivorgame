using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected EnemyMovement movement;
    [SerializeField] protected int maxHealth;
    protected int health;

    protected Player player;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] protected SpriteRenderer spawnIndicator;
    [SerializeField] protected Collider2D collider;
    protected bool hasSpawned;

    [SerializeField] protected float playerDetectionRadius;

    [SerializeField] protected ParticleSystem dieParticles;

    [SerializeField] protected bool gizmos;
    public static Action<int, Vector2> onDamageTaken;
    protected virtual void Start()
    {
        health = maxHealth;//��ʼ����ֵ���
        movement = GetComponent<EnemyMovement>();
        player = FindObjectOfType<Player>();//�ҵ���Һ͵���
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
    protected bool CanAttack()
    {


        return renderer.enabled;
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
    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;
        Gizmos.color = Color.red;//���Ƽ��뾶
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);

        //Gizmos.color = Color.White;
    }
}
