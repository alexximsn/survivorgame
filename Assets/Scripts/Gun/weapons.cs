using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public abstract class weapons : MonoBehaviour,IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    
    [SerializeField] protected float range;
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected LayerMask wallMask;
    [SerializeField] protected float aimLerp;

    [field:SerializeField]public int Level { get; private set; }

    [SerializeField] protected int damage;
    
    [SerializeField] protected Animator animator;
  
    [SerializeField] protected float attackDelay;
    protected float attackTimer;
    protected int critialChance;
    protected float critialPercent;
  
    void Start()
    {
      
    }
    protected Enemy GetClosestEnemy()
    {
     Enemy closestEnemy = null; 
     Collider2D[]  enemies = Physics2D.OverlapCircleAll(transform.position,range,enemyMask); // 使用Object.FindObjectsOfType  

     if (enemies.Length<=0)  
        return null;  
     float minDistance = range; // 最小距离初始化为最大值  
     for (int i = 0; i < enemies.Length; i++)  
    {  
        Enemy enemyChecked = enemies[i].GetComponent<Enemy>();  
        float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position);  
        if (distanceToEnemy < minDistance)  
        {  
            closestEnemy = enemyChecked; // 更改为 closestEnemy 赋值  
            minDistance = distanceToEnemy; // 更新最小距离  
        }  
    }  
         return closestEnemy;
       
    }  

    protected int GetDamage(out bool isCritialHit)
    {
        isCritialHit = false;
        if(Random.Range(0,101)<=critialChance)//现在设置成50%
        {
            isCritialHit = true;
            return Mathf.RoundToInt(damage * critialPercent);

        }
        return damage;//是否是暴击

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color=Color.magenta;
        Gizmos.DrawWireSphere(transform.position,range);
       
    }

    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);
    protected void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);
       
        damage = Mathf.RoundToInt(calculatedStats[Stat.Attack]);
        attackDelay = 1f / calculatedStats[Stat.AttackSpeed];
        critialChance = Mathf.RoundToInt(calculatedStats[Stat.CritialChange] );
        critialPercent = calculatedStats[Stat.CritialPercent] ;
      
        range = calculatedStats[Stat.Range] ;
    }

    internal void Update()
    {
        throw new System.NotImplementedException();
    }
    public void UpgradeTo(int targetLevel)
    {
        Level = targetLevel;
        ConfigureStats();
    }
    public int GetRecyclePrice()
    {
        return WeaponStatsCalculator.GetRecyclePrice(WeaponData,Level);
    }
    public void Upgrade() => UpgradeTo(Level+1);
}
