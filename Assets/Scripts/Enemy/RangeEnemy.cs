using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

[RequireComponent(typeof(EnemyMovement),typeof(RangeEnemyAttack))]
public class RangeEnemy : Enemy
{
   
    private RangeEnemyAttack attack;
   
   
    protected override void Start()
    {
        base.Start();
        attack = GetComponent<RangeEnemyAttack>();
       
        attack.StorePlayer(player);
       
     

    }
   
     void Update()
    {
        if (!CanAttack())
            return;
        ManageAttack();
        transform.localScale = player.transform.position.x > transform.position.x ? new Vector3(-2, 2, 2) : new Vector3(2, 2, 2);

    }
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        if (distanceToPlayer > playerDetectionRadius)
            movement.FollowPlayer();
        else
            TryAttack();
    }
    
    private void TryAttack()
    {
        attack.AutoAim();
    }

}
