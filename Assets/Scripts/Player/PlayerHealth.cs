using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerHealth : MonoBehaviour,IPlayerStatsDependency
{
    [SerializeField] private int baseMaxHealth;
    //设置最大生命值、当前生命值、生命值条、显示文字
    private int maxHealth;
   private float health;
     private float armor;
    private float lifeSteal;
   [SerializeField] private Slider healthSlider;
   [SerializeField] private TextMeshProUGUI healthText;
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
        float lifeStealValue = damage * lifeSteal;
        float healthToAdd = Math.Min(lifeStealValue, maxHealth - health);
        health += healthToAdd;
        UpdateUI();
    }

    void Start()
    {
    }
    //1.计算实际受到的伤害（防止生命值为负）2.减少3.更新UI4.检测死亡
      public void TakeDamage(int damage)
    {
        float realDamage = damage * Mathf.Clamp(1 - (armor / 1000), 0, 10000);
        realDamage=Mathf.Min(damage,health);
        health-=realDamage;
        UpdateUI();
        if(health<=0)
        {
          PassAway();
        }
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
       float addedHealth= playerStatsManager.GetStatValue(Stat.MaxHealth);
        maxHealth = baseMaxHealth + (int)addedHealth;
        maxHealth = Mathf.Max(maxHealth,1);

        health = maxHealth;
        UpdateUI();
        armor = playerStatsManager.GetStatValue(Stat.Armor);
        lifeSteal = playerStatsManager.GetStatValue(Stat.LifeSteal) / 100;
    }
}
