using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
public class DropsManager : MonoBehaviour, IPlayerStatsDependency
{
    [SerializeField] private Drops cherries;
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private Chest chestPrefab;
    
    private float luckValue;
    [SerializeField] [Range(0,100)] private int coinDropChance;
    [SerializeField] [Range(0, 100)] private int chestDropChance;
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Enemy.onBossPassedAway += BossEnemyPassedAwayCallback;
      
    }

    private void BossEnemyPassedAwayCallback(Vector2 bossPosition)
    {
        DropChest(bossPosition);
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Enemy.onBossPassedAway -= BossEnemyPassedAwayCallback;
       
    }

    void Start()
    {
       
    }

   
    void Update()
    {
        
    }
    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCoin = Random.Range(0, 101) <= coinDropChance;

        // 使用全局对象池获取实例
        DroppableCurrency droppable = shouldSpawnCoin ?
            ObjectPool.Instance.GetObject(coinPrefab.gameObject).GetComponent<Coin>() :
            ObjectPool.Instance.GetObject(cherries.gameObject).GetComponent<Drops>();

        droppable.transform.position = enemyPosition;
        TryDropChest(enemyPosition);
    }
    private void TryDropChest(Vector2 spawnPosition)
    {
        // 获取玩家当前Luck值
       // float luckValue = PlayerStatsManager.GetStatValue(Stat.Lucky);

        // 基础掉落率 + Luck值加成（例如：每1点Luck增加0.5%概率）
        float finalDropChance = chestDropChance + (luckValue * 0.5f);

        bool shouldSpawnChest = Random.Range(0, 101) <= finalDropChance;

        if (shouldSpawnChest)
            Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }
    private void DropChest(Vector2 spawnPosition)
    {
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }


    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        luckValue = playerStatsManager.GetStatValue(Stat.幸运);
    }
}
