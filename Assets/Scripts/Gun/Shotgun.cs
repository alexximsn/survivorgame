using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shotgun : Gunweapon {
    [Header("Shotgun Settings")]
    [SerializeField] private int baseBulletNum;//基础子弹数量
    [SerializeField] private float baseSpreadAngle;
    private int currentBulletNum;
    private float currentSpreadAngle;
    
    private enum ShotgunState { Idle, Shoot }

    protected override void Start()
    {
        base.Start(); // 调用父类初始化
        currentBulletNum = baseBulletNum;
        currentSpreadAngle = baseSpreadAngle;
    }


    protected override void Shoot()
    {
        bool isCritical;
        int finalDamage = GetDamage(out isCritical);
        Vector2 shootDirection = (mousePos - (Vector2)transform.position).normalized;

        for (int i = 0; i < currentBulletNum; i++)
        {
            GameObject bulletObj = ObjectPool.Instance.GetObject(bulletPrefab.gameObject);
            if (bulletObj == null)
            {
                Debug.LogError("没有从对象池得到子弹");
                continue;
            }

            bulletObj.transform.SetPositionAndRotation(
                shootingPoint.position,
                Quaternion.identity
            );

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Configure(this);

            float angleStep = currentSpreadAngle / (currentBulletNum - 1);//角度增量
            float angleOffset = -currentSpreadAngle / 2 + angleStep * i;//子弹偏移角
            Vector2 spreadDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * shootDirection;
            bullet.Shoot(finalDamage, spreadDir.normalized, isCritical);//发射
        }
        onButtetShot?.Invoke();
        PlayAttackSound();
    }
    private void UpdateAnimationState()
    {
        ShotgunState state;
        if (Input.GetMouseButton(0) && attackTimer >= attackDelay)
        {
            state = ShotgunState.Shoot;
        }
        else
        {
            state = ShotgunState.Idle;
        }
        animator.SetInteger("equal", (int)state);
    }
  

    public override void UpdateStats(PlayerStatsManager stats)
    {
        base.ConfigureStats();

        currentBulletNum = baseBulletNum;
        currentSpreadAngle = baseSpreadAngle;

        damage = Mathf.RoundToInt(damage * (1 + stats.GetStatValue(Stat.Attack) / 100f));
        attackDelay /= 1 + stats.GetStatValue(Stat.AttackSpeed) / 100f;
    }

 
}
