using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;
public class Gunweapon : weapons
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Bullet bulletPrefab;
    private Vector2 mousePos; // 记录鼠标位置  
    private Vector2 direction; // 射击方向  
    private float flipY;
    private ObjectPool<Bullet> bulletPool;
    private float rotationSpeed = 10f; // 控制旋转速度  

    void Start()
    {
        flipY = transform.localScale.y;
        bulletPool = new ObjectPool<Bullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.x < transform.position.x)
        {
            transform.localScale = new Vector3(flipY, -flipY, 1);
        }
        else
        {
            transform.localScale = new Vector3(flipY, flipY, 1);
        }
        ManageShooting();
        SmoothRotateTowardsMouse();
    }

    private void SmoothRotateTowardsMouse()
    {
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private Bullet CreateFunction()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);
        return bulletInstance;
    }

    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
        bullet.gameObject.SetActive(true);
    }

    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }

    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet);
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
        int damage = GetDamage(out bool isCriticalHit);
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
        Bullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, direction);
    }
}