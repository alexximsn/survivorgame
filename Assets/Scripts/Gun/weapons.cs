using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

//攻击、伤害计算、敌人检测、状态更新
public abstract class weapons : MonoBehaviour,IPlayerStatsDependency
{
    [field: SerializeField] public WeaponDataSO WeaponData { get; private set; }
    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected LayerMask wallMask;
    [field:SerializeField]public int Level { get; private set; }

    [SerializeField] protected int damage; 
    [SerializeField] protected Animator animator;
    [SerializeField] protected float attackDelay;//攻击延迟
    protected float attackTimer;//计时器
    protected int critialChance;//暴击几率
    protected float critialPercent;//暴击百分比
    private AudioSource audioSource;//枪声
    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = WeaponData.AttackSound;
    }
    protected void PlayAttackSound()
    {
       
        if (!AudioManager.instance.IsSFXOn || GameManager.CurrentGameState!= GameState.GAME)
            return;
        audioSource.pitch = UnityEngine.Random.Range(.95f, 1.05f);//随机音调
        audioSource.Play();
    }
    void Start()
    {
      
    }
    protected int GetDamage(out bool isCritialHit)
    {
        isCritialHit = false;
        if(Random.Range(0,101)<=critialChance)//暴击几率判断
        {
            isCritialHit = true;
            return Mathf.RoundToInt(damage * critialPercent);
        }
        return damage;//是否是暴击

    }
    public abstract void UpdateStats(PlayerStatsManager playerStatsManager);
    protected void ConfigureStats()
    {
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(WeaponData, Level);
        damage = Mathf.RoundToInt(calculatedStats[Stat.攻击]);
        attackDelay = 1f / calculatedStats[Stat.攻击速度];
        critialChance = Mathf.RoundToInt(calculatedStats[Stat.暴击率] );
        critialPercent = calculatedStats[Stat.暴击伤害] ;
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
