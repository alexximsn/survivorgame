using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
public class RangeEnemyAttack : MonoBehaviour
{
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private TreeEnemyBullet bulletPrefab;
    private Player player;

    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    private ObjectPool<TreeEnemyBullet> bulletPool;
    void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<TreeEnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }
    private TreeEnemyBullet CreateFunction()
    {
        TreeEnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);
        return bulletInstance;
       // return Instantiate(bulletPrefab, transform);
    }
    private void ActionOnGet(TreeEnemyBullet bullet)
    {
        bullet.Reload();
        bullet.transform.position = shootingPoint.position;
       bullet.gameObject.SetActive(true);
    }
    private void ActionOnRelease(TreeEnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(TreeEnemyBullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    public void ReleaseBullet(TreeEnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }
    public void StorePlayer(Player player)
    {
        this.player = player;
    }
    // Update is called once per frame
    void Update()
    {     
    }
    public void AutoAim()
    { 
        ManageShooting();
    }
    private void  ManageShooting()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer>=attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }

    }
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;
        TreeEnemyBullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage,direction);
      
    }
 
   
}
