using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocketdan : Gunweapon
{
    [Header("Shotgun Settings")]
    [SerializeField] private int baseRocketNum;
    [SerializeField] private float baseSpreadAngle;
    private int currentRocketNum;
    private float currentSpreadAngle;

    private enum ShotgunState { Idle, Shoot }

    protected override void Start()
    {
        base.Start(); // 调用父类初始化
        currentRocketNum = baseRocketNum;
        currentSpreadAngle = baseSpreadAngle;
    }


    protected override void Shoot()
    {
        bool isCritical;
        int finalDamage = GetDamage(out isCritical);
        Vector2 shootDirection = (mousePos - (Vector2)transform.position).normalized;

        for (int i = 0; i < currentRocketNum; i++)
        {
            GameObject bulletObj = ObjectPool.Instance.GetObject(bulletPrefab.gameObject);
            if (bulletObj == null)
            {
                Debug.LogError("Failed to get bullet from pool!");
                continue;
            }

            bulletObj.transform.SetPositionAndRotation(
                shootingPoint.position,
                Quaternion.identity
            );

            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.Configure(this);

            float angleStep = currentSpreadAngle / (currentRocketNum - 1);
            float angleOffset = -currentSpreadAngle / 2 + angleStep * i;
            Vector2 spreadDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * shootDirection;
            bullet.Shoot(finalDamage, spreadDir.normalized, isCritical);
        }
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

        currentRocketNum = baseRocketNum ;
        currentSpreadAngle = baseSpreadAngle;

        damage = Mathf.RoundToInt(damage * (1 + stats.GetStatValue(Stat.Attack) / 100f));
        attackDelay /= 1 + stats.GetStatValue(Stat.AttackSpeed) / 100f;
    }

}
