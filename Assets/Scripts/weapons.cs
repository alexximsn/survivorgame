using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class weapons : MonoBehaviour
{
    enum State
    {
        Idle,
        Attack
    }
    private State state;
    [SerializeField] private float range;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private float aimLerp;
    [SerializeField] private Transform hitDetectionTransform;
    [SerializeField] private float hitDetectionRadius;
    [SerializeField] private int damage;//武器伤害值
    [SerializeField] private Animator animator;
    private List<Enemy> damagedEnemies = new List<Enemy>();
    [SerializeField] private float attackDelay;
    private float attackTimer;
    void Start()
    {
        state = State.Idle;
    }
   void Update()  
{  
        switch(state)
        {
            case State.Idle:
              AutoAim();
                break;
            case State.Attack:
                Attacking();
                break;
        }
    
    Attack();
}  

    private void AutoAim()
    {
        Enemy closestEnemy=GetClosestEnemy();
        Vector2 targetUpVector=Vector3.up;
        if(closestEnemy!=null)
        {
            targetUpVector=(closestEnemy.transform.position-transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack();
            
        }
        
        transform.up = Vector3.Lerp(transform.up,targetUpVector,Time.deltaTime*aimLerp);
        IncrementAttackTimer();
    }
    private void ManageAttack()
    {
        
        if(attackTimer>=attackDelay)
        {
            attackTimer = 0;
            StartAttack();
        }
    }
    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime;
    }


    [NaughtyAttributes.Button]
    private void StartAttack()
    {
        animator.Play("Attack");
        state = State.Attack;
        damagedEnemies.Clear();
        animator.speed = 1f / attackDelay;
    }

    private void Attacking()
    {
        Attack();
    }

    private void StopAttack()
    {
        state = State.Idle;
        damagedEnemies.Clear();
    }


    private void Attack()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(hitDetectionTransform.position,hitDetectionRadius, enemyMask);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            if (!damagedEnemies.Contains(enemy))

            { 
               enemy.TakeDamage(damage);
                damagedEnemies.Add(enemy);
            }
            //enemies[i].GetComponent<Enemy>().TakeDamage(damage);
        }
            
    }
    private Enemy GetClosestEnemy()
    {
     Enemy closestEnemy = null; 
     Collider2D[]  enemies = Physics2D.OverlapCircleAll(transform.position,range,enemyMask); // 使用Object.FindObjectsOfType  

     if (enemies.Length<=0)  
        return null;  
     float minDistance = range; // 最小距离初始化为最大值  
     for (int i = 0; i < enemies.Length; i++)  
    {  
        Enemy enemyChecked = enemies[i].GetComponent<Enemy>();  
        float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);  
        if (distanceToEnemy < minDistance)  
        {  
            closestEnemy = enemyChecked; // 更改为 closestEnemy 赋值  
            minDistance = distanceToEnemy; // 更新最小距离  
        }  
    }  
         return closestEnemy;
       
    }  

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.magenta;
        Gizmos.DrawWireSphere(transform.position,range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(hitDetectionTransform.position, hitDetectionRadius);
    }
}
