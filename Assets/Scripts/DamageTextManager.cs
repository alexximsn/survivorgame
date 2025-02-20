using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;  
public class DamageTextManager : MonoBehaviour
{
    [SerializeField] private DamageText damageTextPrefab;
    private ObjectPool<DamageText> damageTextPool;
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
      damageText.gameObject.SetActive(false);
    }
    private void ActionOnDestroy(DamageText damageText)
    {
       Destroy(damageText.gameObject);
    }
  
    private void EnemyHitcallback(int damage,Vector2 enemyPos)
    {
        DamageText damageTextInstance=damageTextPool.Get();
        Vector3 spawnPosition=enemyPos+Vector2.up*1f;
        damageTextInstance.transform.position=spawnPosition;
        damageTextInstance.Animate(damage);
        LeanTween.delayedCall(1,()=>damageTextPool.Release(damageTextInstance));
    }
    private void Awake()
    {
        Enemy.onDamageTaken+=EnemyHitcallback;
    }
    private void OnDestroy()
    {
        Enemy.onDamageTaken-=EnemyHitcallback;
    }
}
