using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
public class PlayerHealth : MonoBehaviour,IPlayerStatsDependency
{
    [SerializeField] private int baseMaxHealth;
    //设置最大生命值、当前生命值、生命值条、显示文字
    private int maxHealth;
    private float health;
    private float dodge;
    private float healthRecoverySpeed;
    private float healthRecoverValue;
    private float healthRecoverTimer;
    private float healthRecoverDuration; 
   [SerializeField] private Slider healthSlider;
   [SerializeField] private TextMeshProUGUI healthText;

    public static Action<Vector2> onAttackDodged;
    //开始等于最大生命值，更新UI
    private void Awake()
    {
        Enemy.onDamageTaken += EnemyTookDamageCallback;
    }
    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyTookDamageCallback;
    }
    private void EnemyTookDamageCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        if (health >= maxHealth)
            return;
        float lifeStealValue = damage;
        float healthToAdd = Math.Min(lifeStealValue, maxHealth - health);
        health += healthToAdd;
        UpdateUI();
    }

    void Start()
    {
    }
    private void Update()
    {
        if (health < maxHealth)
            RecoverHealth();
    }
    private void RecoverHealth()
    {
        healthRecoverTimer += Time.deltaTime;
        if(healthRecoverTimer>=healthRecoverDuration)
        {
            healthRecoverTimer = 0;
            float healthToAdd = Mathf.Min(.1f, maxHealth - health);
            health += healthToAdd;
            UpdateUI();
        }
    }
    //1.计算实际受到的伤害（防止生命值为负）2.减少3.更新UI4.检测死亡
    public void TakeDamage(int damage)
    {
        if(ShouldDodge())
        {
            onAttackDodged ?.Invoke(transform.position);
            return;
        }
        float realDamage = damage;
        realDamage=Mathf.Min(damage,health);
        health-=realDamage;
        UpdateUI();
        if(health<=0)
        {
          PassAway();
        }
    }
    private bool ShouldDodge()
    {
        return Random.Range(0f, 100f)<dodge;
    }
    private void PassAway()
    {
        GameManager.instance.SetGameState(GameState.GAMEOVER);
    }
    //1.计算当前生命值占最大生命值的比例2.更新生命条3.更新文本
    private void UpdateUI()
    {
        float healthBarValue=health/maxHealth;
        healthSlider.value=healthBarValue;
        healthText.text=(int)health+"/"+maxHealth;
    }
    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
       float addedHealth= playerStatsManager.GetStatValue(Stat.生命值);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth,1);

        health = maxHealth;
        UpdateUI();
        dodge = playerStatsManager.GetStatValue(Stat.闪避);//闪避
        healthRecoverySpeed = Mathf.Max(.0001f, playerStatsManager.GetStatValue(Stat.恢复速度));
        healthRecoverDuration = 1f / healthRecoverySpeed;
    }
}
