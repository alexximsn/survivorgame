using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class Enemy : MonoBehaviour
{
    private EnemyMovement movement;

    private Player player;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private SpriteRenderer spawnIndicator;
    private bool hasSpawned;
    [SerializeField] private float playerDetectionRadius;
    [SerializeField] bool gizmos;

    [SerializeField] private ParticleSystem dieParticles;
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;
    void Start()
    {
        movement = GetComponent<EnemyMovement>();

        player = FindObjectOfType<Player>();

        if (player == null)
        {
          
            Destroy(gameObject);
        }
        StartSpawnSequence();
        attackDelay = 1f / attackFrequency;

    }
    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false); 
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.3f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f).
            setLoopPingPong(4).setOnComplete(SpawnSequenceCompleted);

    }
    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true);
        hasSpawned = true;
        movement.StorePlayer(player);
    }
   
    private void SetRenderersVisibility(bool visibility)
    {
        renderer.enabled = visibility;
        spawnIndicator.enabled = !visibility;
    }
    void Update()
    {
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait();
    }
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }
    private void Attack()
    {
        
        attackTimer = 0;
        player.TakeDamage(damage);
    }
    private void PassAway()
    {
        dieParticles.transform.SetParent(null);
        dieParticles.Play();
        Destroy(gameObject);
    }
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer <= playerDetectionRadius)
            Attack();
        //PassAway();
    }
    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius);
    }
}
