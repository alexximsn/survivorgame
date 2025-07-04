using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;
public class RangeEnemyAttack : MonoBehaviour
{
    [SerializeField] public Transform shootingPoint;
    [SerializeField] private TreeEnemyBullet bulletPrefab;
    private Player player;

    [SerializeField] private int damage;
    [SerializeField] private float attackFrequency;
    private float attackDelay;
    private float attackTimer;

    private ObjectPool<TreeEnemyBullet> bulletPool;

    [SerializeField] private Animator animator;
    [SerializeField] private EnemyMovement movement;
    private bool isAttacking;
    private Coroutine attackRoutine;
    void Start()
    {
        attackDelay = 1f / attackFrequency;
        attackTimer = attackDelay;

        bulletPool = new ObjectPool<TreeEnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

   
    public void StorePlayer(Player player)
    {
        this.player = player;
    }
    private TreeEnemyBullet CreateFunction()
    {
        TreeEnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this);

        return bulletInstance;
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
    void Update()
    {     
    }
    public void AutoAim()
    {
     
        if (!isAttacking)
        {
            attackRoutine = StartCoroutine(AttackSequence());
        }
    }
    private IEnumerator AttackSequence()
    {
        isAttacking = true;

        // �׶� 1: �����ƶ������Ŷ���
        movement.SetMovementEnabled(false);
        animator.SetTrigger("Attack");

        // �׶� 2: �ȴ�����Ԥ��ʱ��
        yield return new WaitForSeconds(0.3f); // �붯��̧�ֶ���ͬ��

        // �׶� 3: ִ�����
        Shoot();

        // �׶� 4: �ȴ�������ȴ
        yield return new WaitForSeconds(1f / attackFrequency);

        // �׶� 5: �ָ�״̬
        movement.SetMovementEnabled(true);
        isAttacking = false;
    }
    
    public void OnShootEvent()
    {
        Shoot(); // ��ȷͬ�����ʱ��
    }

    public void OnAttackEnd()
    {
        // ��ȫ�ָ�����ֹЭ�̳�ͻ��
        if (isAttacking)
        {
            movement.SetMovementEnabled(true);
            StopCoroutine(attackRoutine);
            isAttacking = false;
        }
    }
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized;
        InstantShoot(direction);
      
    }
    public void InstantShoot(Vector2 direction)
    {
        TreeEnemyBullet bulletInstance = bulletPool.Get();
        bulletInstance.Shoot(damage, direction);
    }
 
   
}
