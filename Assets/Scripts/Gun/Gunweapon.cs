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
    [SerializeField] protected Transform shootingPoint;//�����
    [SerializeField] protected GameObject bulletPrefab;//�ӵ�
    protected Vector2 mousePos;//���λ��
    protected Vector2 direction;//��Ʒ���
    protected float flipY;//��ת
    protected float rotationSpeed = 10f;//��ת�ٶ�
    public static Action onButtetShot;//�����
    private enum gunState { idle, SHOOT };//����

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        flipY = transform.localScale.y;
    }

    protected virtual void Update()
    {
        HandleWeaponFlip();//������ת
        GunAnim();//����
        ManageShooting();//����߼�
        SmoothRotateTowardsMouse();//ƽ��ת��
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
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;//��������
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;//Ŀ��Ƕ�
        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);//����Ŀ����ת
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);//ƽ����ת��Ŀ��Ƕ�
    } 
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        // ������������£����ҹ�����ʱ���ﵽ�����ӳ�  
        if (UnityEngine.Input.GetMouseButton(0) && attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }
    protected virtual void Shoot()
    {  int damage = GetDamage(out bool isCriticalHit);
       direction = (mousePos - (Vector2) transform.position).normalized;

        // �Ӷ���ػ�ȡ�ӵ�
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