using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasergun : Gunweapon
{
    [Header("Laser Settings")]
    [SerializeField] private LineRenderer laser;
    private GameObject hitEffect;
    [SerializeField] private float damageInterval = 0.2f;

    private bool isShooting;
    private float lastDamageTime;
  

    protected override void Start()
    {
        base.Start();
        laser.positionCount = 2;
        laser.enabled = false;
        hitEffect = transform.Find("Effect").gameObject;
    }

    protected override void Update()
    {
        base.Update();
        HandleLaserInput();
    }

    private void HandleLaserInput()
    {
        if (Input.GetMouseButtonDown(0))
            StartShooting();
        if (Input.GetMouseButtonUp(0))
            StopShooting();
    }

    protected override void Shoot()
    {
        // 禁用基类子弹生成逻辑
        onButtetShot?.Invoke();
        PlayAttackSound();
    }

    private void StartShooting()
    {
        isShooting = true;
        laser.enabled = true;
        hitEffect.SetActive(true);
        animator.SetBool("Shoot", true);
    }

    private void StopShooting()
    {
        isShooting = false;
        laser.enabled = false;
        hitEffect.SetActive(false);
        animator.SetBool("Shoot", false);
    }

    private void FixedUpdate()
    {
        if (!isShooting) return;

        UpdateLaserVisual();
        ApplyContinuousDamage();
    }

    private void UpdateLaserVisual()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            shootingPoint.position, // 使用基类的 shootingPoint
            direction,
            range,
            enemyMask | wallMask
        );

        laser.SetPosition(0, shootingPoint.position);
        laser.SetPosition(1, hit.point);
        hitEffect.transform.position = hit.point;
        hitEffect.transform.forward = -direction;
    }

    private void ApplyContinuousDamage()
    {
        if (Time.time - lastDamageTime < damageInterval) return;

        RaycastHit2D hit = Physics2D.Raycast(
            shootingPoint.position, // 使用基类的 shootingPoint
            direction,
            range,
            enemyMask
        );

        if (hit.collider != null)
        {
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(GetCurrentDamage(), false);
                lastDamageTime = Time.time;
            }
        }
    }

    private int GetCurrentDamage()
    {
        return Mathf.RoundToInt(damage * damageInterval);
    }

    public override void UpdateStats(PlayerStatsManager stats)
    {
        base.ConfigureStats();// 关键修复：调用基类方法更新 damage 等属性
        damage = Mathf.RoundToInt(damage * (1 + stats.GetStatValue(Stat.Attack) / 100f));
        
    }
}