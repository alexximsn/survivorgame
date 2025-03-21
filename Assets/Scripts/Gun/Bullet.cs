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
    private Gunweapon gunWeapon;
    private Enemy target;//ȷ��ֻ����һ������
    private weapons parentWeapon;
    private bool isReleased = false;
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
        this.damage = damage;
        this.isCriticalHit = isCriticalHit;
        transform.right = direction;
        rig.velocity = direction * moveSpeed;
        // �Ƴ� Invoke("Release", 1);
    }
    public void Configure(weapons weapon)
    {
        parentWeapon = weapon;
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (isReleased) return; // ������ͷţ����ٴ�����ײ

        // ���ȼ��ǽ��
        if (IsInLayerMask(collider.gameObject.layer, wallMask))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Release();
            return;
        }

        // ���������ײ
        if (target == null && IsInLayerMask(collider.gameObject.layer, enemyMask))
        {
            target = collider.GetComponent<Enemy>();
            CancelInvoke(); // ȡ��Shoot�е��ӳ��ͷ�
            Attack(target);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Release();
        }
    }
    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage,isCriticalHit);
        
        //Destroy(gameObject);
    }
    private bool IsInLayerMask(int layer,LayerMask layerMask)
    {
        return (layerMask.value&(1<<layer))!=0;
    }
    public void Reload()
    {
        isReleased = false;
        target = null;
        rig.velocity = Vector2.zero;
        collider.enabled = true;
    }
    private void Release()
    {
        if (isReleased) return; // ������ͷţ�ֱ�ӷ���
        isReleased = true;

        if (parentWeapon is Shotgun shotgun)
            shotgun.ReleaseBulletToPool(this);
        else if (parentWeapon is Gunweapon gun)
            gun.ReleaseBullet(this);
    }
}
