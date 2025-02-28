using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rig;
    private Collider2D collider;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    private int damage;
    private bool isCriticalHit;
    private Gunweapon gunWeapon;
    private Enemy target;//确保只攻击一个敌人
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        //LeanTween.delayedCall(gameObject, 5, () => rangeEnemyAttack.ReleaseBullet(this));

    }
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {     
    }
    public void Shoot(int damage,Vector2 direction,bool isCriticalHit)
    {
        Invoke("Release",1);
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    public void Configure(Gunweapon gunWeapon)
    {
        this.gunWeapon = gunWeapon;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (target != null)
            return;
        if(IsInLayerMask(collider.gameObject.layer,enemyMask))
        {
            target = collider.GetComponent<Enemy>();
            CancelInvoke();
            Attack(target);
            Release();
           // Destroy(gameObject);
        }
    }
    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage,isCriticalHit);
    }
    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        return (layerMask.value&(1<<layer))!=0;
    }
    public void Reload()
    {
        target = null;
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
    private void Release()
    {
        if(!gameObject.activeSelf)
        return;
        gunWeapon.ReleaseBullet(this);
    }
}
