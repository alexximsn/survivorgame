using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D),typeof(Collider2D))]
public class TreeEnemyBullet : MonoBehaviour
{
    private Rigidbody2D rig;
    private Collider2D collider;
    private RangeEnemyAttack rangeEnemyAttack;
    [SerializeField] private float moveSpeed;
    private int damage;
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        LeanTween.delayedCall(gameObject, 5f, () => {
            if (gameObject.activeInHierarchy)
            {
                ObjectPool.Instance.PushObject(gameObject);
            }
        });

    }
   
    public void Configure(RangeEnemyAttack rangeEnemyAttack)
    {
        this.rangeEnemyAttack = rangeEnemyAttack;
    }
    public void Shoot(int damage,Vector2 direction)
    {
        this.damage = damage;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.TryGetComponent(out Player player))
        {
            // 命中玩家时立即回收
            LeanTween.cancel(gameObject); // 取消延迟回收
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
    public void Reload()
    {
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
}
