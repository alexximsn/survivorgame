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
    public void Shoot(int damage,Vector2 direction)
    {
        this.damage = damage;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(IsInLayerMask(collider.gameObject.layer,enemyMask))
        {
            Attack(collider.GetComponent<Enemy>());
            Destroy(gameObject);
        }
        
    }
    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage);
    }
    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        return (layerMask.value&(1<<layer))!=0;
    }
}
