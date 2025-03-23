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
    [SerializeField] protected Transform shootingPoint;
    [SerializeField] protected GameObject bulletPrefab;
    protected Vector2 mousePos;
    protected Vector2 direction;
    protected float flipY;
 
    protected float rotationSpeed = 10f;

    private enum gunState { idle, SHOOT };

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        flipY = transform.localScale.y;
       
    }

    protected virtual void Update()
    {
        HandleWeaponFlip();
        GunAnim();
        ManageShooting();
        SmoothRotateTowardsMouse();
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
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
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
    }

    public override void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        ConfigureStats();
        damage = Mathf.RoundToInt(damage * (1+playerStatsManager.GetStatValue(Stat.Attack)/100));
        attackDelay/=1+(playerStatsManager.GetStatValue(Stat.AttackSpeed)/100);
        critialChance = Mathf.RoundToInt(critialChance * (1 + playerStatsManager.GetStatValue(Stat.CritialChange) / 100));
        critialPercent += playerStatsManager.GetStatValue(Stat.CritialPercent);
        range += playerStatsManager.GetStatValue(Stat.Range) / 10;
    }
}