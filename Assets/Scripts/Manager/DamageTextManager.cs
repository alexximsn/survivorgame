using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;  
public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;
    private ObjectPool<DamageText> damageTextPool;//用于管理伤害文本的创建和复用，减少内存分配和释放
    void Start()
    {
       damageTextPool=new ObjectPool<DamageText>(CreateFunction,ActionOnGet,ActionOnRelease,ActionOnDestroy); 
    }

    private DamageText CreateFunction()
    {
      return Instantiate(damageTextPrefab,transform);
    }
    private void ActionOnGet(DamageText damageText)
    {
       damageText.gameObject.SetActive(true);
    }
    private void ActionOnRelease(DamageText damageText)
    {
        if(damageText!=null)
      damageText.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(DamageText damageText)
    {
       Destroy(damageText.gameObject);
    }
  
    private void EnemyHitcallback(int damage,Vector2 enemyPos, bool isCriticalHit)
    {//显示伤害文本
        DamageText damageTextInstance=damageTextPool.Get();//从池中获取伤害文本
        Vector3 spawnPosition=enemyPos+Vector2.up*1f;//计算显示位置（在敌人上方1单位）
        damageTextInstance.transform.position=spawnPosition;
        damageTextInstance.Animate(damage.ToString(),isCriticalHit);//调用动画
        LeanTween.delayedCall(1,()=>damageTextPool.Release(damageTextInstance));
    }
    private void Awake()
    {
        Enemy.onDamageTaken+=EnemyHitcallback;
        PlayerHealth.onAttackDodged += AttackDodgedCallback;
    }

    private void OnDestroy()
    {
        Enemy.onDamageTaken-=EnemyHitcallback;//从 Enemy 的 onDamageTaken 事件中移除回调，以避免内存泄漏和访问已销毁对象的问题。
        PlayerHealth.onAttackDodged -= AttackDodgedCallback;
    }
    private void AttackDodgedCallback(Vector2 playerPosition)
    {
        DamageText damageTextInstance = damageTextPool.Get();//从池中获取伤害文本
        Vector3 spawnPosition = playerPosition + Vector2.up * 1f;//计算显示位置（在敌人上方1单位）
        damageTextInstance.transform.position = spawnPosition;
        damageTextInstance.Animate("Miss",false);//调用动画
        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }

}
