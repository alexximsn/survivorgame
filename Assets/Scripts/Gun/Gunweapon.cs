using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Pool;
using UnityEngine.Windows;
using Input = UnityEngine.Input;
public class Gunweapon : weapons
{
    [SerializeField] protected Transform shootingPoint;//发射点
    [SerializeField] protected GameObject bulletPrefab;//子弹
    protected Vector2 mousePos;//鼠标位置
    protected Vector2 direction;//设计方向
    protected float flipY;//翻转
    protected float rotationSpeed = 10f;//旋转速度
    public static Action onButtetShot;//相机震动
    private enum gunState { idle, SHOOT };//动画

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        flipY = transform.localScale.y;
    }

    protected virtual void Update()
    {
        HandleWeaponFlip();//武器旋转
        GunAnim();//动画
        ManageShooting();//设计逻辑
        SmoothRotateTowardsMouse();//平滑转向
    }
    void GunAnim()
    {
        gunState states;
        if (Input.GetMouseButton(0) && attackTimer >= attackDelay)
        {
            states = gunState.SHOOT;
        }
        else
        {
            states = gunState.idle;
        }
        animator.SetInteger("equal", (int)states);
    }
   
    protected void HandleWeaponFlip()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.localScale = mousePos.x < transform.position.x
            ? new Vector3(flipY, -flipY, 1)
            : new Vector3(flipY, flipY, 1);
    }
    private void SmoothRotateTowardsMouse()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;//攻击方向
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//目标角度
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);//生成目标旋转
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);//平滑旋转到目标角度
    } 
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        // 检测鼠标左键按下，并且攻击计时器达到攻击延迟  
        if (UnityEngine.Input.GetMouseButton(0) && attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }
    protected virtual void Shoot()
    {  int damage = GetDamage(out bool isCriticalHit);
       direction = (mousePos - (Vector2) transform.position).normalized;

        // 从对象池获取子弹
        GameObject bulletObj = ObjectPool.Instance.GetObject(bulletPrefab.gameObject);
        bulletObj.transform.position = shootingPoint.position;
        bulletObj.transform.rotation = Quaternion.identity;
        Bullet bulletInstance = bulletObj.GetComponent<Bullet>();
        bulletInstance.Configure(this);
        bulletInstance.Shoot(damage, direction, isCriticalHit);
        onButtetShot?.Invoke();
        PlayAttackSound();
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        damage = Mathf.RoundToInt(damage * (1+playerStatsManager.GetStatValue(Stat.Attack)/100));
        attackDelay/=1+(playerStatsManager.GetStatValue(Stat.AttackSpeed)/100);
        critialChance = Mathf.RoundToInt(critialChance * (1 + playerStatsManager.GetStatValue(Stat.CritialChange) / 100));
        critialPercent += playerStatsManager.GetStatValue(Stat.CritialPercent);
 
    }
}