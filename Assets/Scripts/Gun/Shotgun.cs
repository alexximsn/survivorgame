using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shotgun : weapons
{
    [Header("Shotgun Settings")]
    [SerializeField] private Transform muzzlePos;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private int baseBulletNum ;
    [SerializeField] private float baseSpreadAngle;
    [SerializeField] private float rotationSpeed = 10f; // 新增旋转速度
     private float flipY;              // 新增翻转控制

   
    private enum ShotgunState { Idle, Shoot }

    private ObjectPool<Bullet> bulletPool;
    private int currentBulletNum;
    private float currentSpreadAngle;
    private Vector2 mousePos;        // 新增鼠标坐标存储
    private Vector2 direction;       // 新增方向向量


    void Start()
    {

        flipY = transform.localScale.y;// 初始化翻转基准值
        bulletPool = new ObjectPool<Bullet>(
            CreateBullet,
            bullet => bullet.gameObject.SetActive(true),
            bullet => bullet.gameObject.SetActive(false),
            bullet => Destroy(bullet.gameObject)
        );
        currentBulletNum = baseBulletNum;
        currentSpreadAngle = baseSpreadAngle;
    }

    void Update()
    {
        // ======== 新增鼠标逻辑 ========
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 处理武器翻转
        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(flipY, -flipY, 1);
        }
        else
        {
            transform.localScale = new Vector3(flipY, flipY, 1);
        }

        // 平滑旋转
        SmoothRotateTowardsMouse();
        UpdateAnimationState();

        attackTimer += Time.deltaTime;

        if (Input.GetMouseButton(0)) // 支持连发
        {
            if (attackTimer >= attackDelay)
            {
                attackTimer = 0;
                ShootSpread();
            }
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
    // 新增旋转方法（与Gunweapon一致）
    private void SmoothRotateTowardsMouse()
    {
        direction = (mousePos - (Vector2)transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            Time.deltaTime * rotationSpeed
        );
    }

    private void ShootSpread()
    {
        bool isCritical;
        int finalDamage = GetDamage(out isCritical);

        // 使用已计算的direction（保持与旋转方向一致）
        Vector2 shootDirection = direction;

        int median = currentBulletNum / 2;
        for (int i = 0; i < currentBulletNum; i++)
        {
            Bullet bullet = bulletPool.Get();
            bullet.transform.position = muzzlePos.position;

            float angleOffset = currentBulletNum % 2 == 1 ?
                (i - median) * currentSpreadAngle :
                (i - median) * currentSpreadAngle + currentSpreadAngle / 2;

            Vector2 spreadDir = Quaternion.AngleAxis(angleOffset, Vector3.forward) * shootDirection;
            bullet.Shoot(finalDamage, spreadDir, isCritical);
        }
    }

    public override void UpdateStats(PlayerStatsManager stats)
    {
        base.ConfigureStats();

        currentBulletNum = baseBulletNum + Mathf.RoundToInt(stats.GetStatValue(Stat.SpreadCount));
        currentSpreadAngle = baseSpreadAngle;

        damage = Mathf.RoundToInt(damage * (1 + stats.GetStatValue(Stat.Attack) / 100f));
        attackDelay /= 1 + stats.GetStatValue(Stat.AttackSpeed) / 100f;
    }

    #region Bullet Pool
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(bulletPrefab);
        bullet.Configure(this);
        return bullet;
    }

    public void ReleaseBulletToPool(Bullet bullet)
    {
        bullet.Reload(); // 确保状态重置
        bulletPool.Release(bullet);
    }
    #endregion
}
