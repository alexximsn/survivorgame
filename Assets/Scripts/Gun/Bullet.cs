using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rig;
    private Collider2D collider;
    public GameObject explosionPrefab;
    [SerializeField] private float moveSpeed;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private LayerMask wallMask;
    private int damage;
    private bool isCriticalHit;
    private bool isReleased = false;

   private Gunweapon gunWeapon;
    private Enemy target;//确保只攻击一个敌人
    private weapons parentWeapon;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
    }
    public void Shoot(int damage,Vector2 direction,bool isCriticalHit)
    {
        gameObject.SetActive(true);
        collider.enabled = true;//
        isReleased = false;//
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    public void Configure(weapons weapon)
    {
        parentWeapon = weapon;//配置父武器
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //Debug.Log($"子弹碰撞到对象: {collider.gameObject.name}");
        if (isReleased) return; // 如果已释放，不再处理碰撞

        // 优先检测墙壁
        if (IsInLayerMask(collider.gameObject.layer, wallMask))
        {
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            exp.transform.rotation = Quaternion.identity;
            Release();
            return;
        }

        // 处理敌人碰撞
        if (IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();
            CancelInvoke(); // 取消Shoot中的延迟释放
            Attack(target);
            GameObject exp = ObjectPool.Instance.GetObject(explosionPrefab);
            exp.transform.position = transform.position;
            exp.transform.rotation = Quaternion.identity;
            Release();
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
    public virtual void Reload()
    {
        isReleased = false;
        target = null;
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
    public virtual void Release()
    {
        if (isReleased) return;
        isReleased = true;
        rig.velocity = Vector2.zero;
        collider.enabled = false; // 禁用碰撞体
        gameObject.SetActive(false); // 显式禁用对象
        ObjectPool.Instance.PushObject(gameObject); // 直接使用 ObjectPool 单例
    }
}
