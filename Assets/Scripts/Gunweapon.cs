using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunweapon : weapons
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Bullet bulletPrefab;
   void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AutoAim();
    }
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy();
        Vector2 targetUpVector = Vector3.up;
        if (closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            
            ManageShooting();
            return;

        }
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
       
        
    }
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }
    private void Shoot()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Shoot(damage, transform.up);
    }
}
