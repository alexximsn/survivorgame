using System;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(EnemyMovement))]
public class Meleeenemy : Enemy
{
  
   
  
    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;



    protected override void Start()
    {
        base.Start();
        attackDelay = 1f / attackFrequency;

    }
 
     void Update()
    {
        if (!CanAttack())
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
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);//计算二者距离
        if (distanceToPlayer <= playerDetectionRadius)
            Attack();//小于检测半径，攻击
        else
            movement.FollowPlayer();
        
    }
    
}
