using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class weapons : MonoBehaviour
{
    
    [SerializeField] private float range;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected float aimLerp;
   
    [SerializeField] protected int damage;//武器伤害值
    [SerializeField] protected Animator animator;
  
    [SerializeField] protected float attackDelay;
    protected float attackTimer;
    void Start()
    {
      
    }
   void Update()  
{  
      
   
}  

   

   
    protected Enemy GetClosestEnemy()
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
       
    }
}
