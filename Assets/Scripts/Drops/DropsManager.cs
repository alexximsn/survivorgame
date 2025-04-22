using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;
public class DropsManager : MonoBehaviour
{
    [SerializeField] private Drops cherries;
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private Chest chestPrefab;
    private ObjectPool<Drops> cherriesPool;
    private ObjectPool<Coin> coinPool;

    [SerializeField] [Range(0,100)] private int cashDropChance;
    [SerializeField] [Range(0, 100)] private int chestDropChance;
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
        Enemy.onBossPassedAway += BossEnemyPassedAwayCallback;
        Coin.onCollected += ReleaseCoin;
        Drops.onCollected += Releasecherries;
    }

    private void BossEnemyPassedAwayCallback(Vector2 bossPosition)
    {
        DropChest(bossPosition);
    }

    private void OnDestroy()
    {
        Enemy.onPassedAway -= EnemyPassedAwayCallback;
        Enemy.onBossPassedAway -= BossEnemyPassedAwayCallback;
        Coin.onCollected -= ReleaseCoin;
        Drops.onCollected -= Releasecherries;
    }

    void Start()
    {
        cherriesPool = new ObjectPool<Drops>(cherriesCreateFunction, cherriesActionOnGet, cherriesActionOnRelease, cherriesActionOnDestroy);
        coinPool = new ObjectPool<Coin>(coinCreateFunction, coinActionOnGet, coinActionOnRelease, coinActionOnDestroy);
    }

    private Drops cherriesCreateFunction()
    {
        Drops bulletInstance = Instantiate(cherries, transform);
       
        return bulletInstance;
    }
    private void cherriesActionOnGet(Drops drops)
    {
        
        drops.gameObject.SetActive(true);
    }
    private void cherriesActionOnRelease(Drops drops)
    {
        drops.gameObject.SetActive(false);
    }
    private void cherriesActionOnDestroy(Drops drops)
    {
        Destroy(drops.gameObject);
    }
    private Coin coinCreateFunction()
    {
        Coin bulletInstance = Instantiate(coinPrefab, transform);
        return bulletInstance;
    }
    private void coinActionOnGet(Coin coin)
    {

        coin.gameObject.SetActive(true);
    }
    private void coinActionOnRelease(Coin coin)
    {
        coin.gameObject.SetActive(false);
    }
    private void coinActionOnDestroy(Coin coin)
    {
        Destroy(coin.gameObject);
    }
    void Update()
    {
        
    }
    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        bool shouldSpawnCoin = Random.Range(0, 101) <= cashDropChance;
        DroppableCurrency dropppable = shouldSpawnCoin ? coinPool.Get() : cherriesPool.Get();

        dropppable.transform.position = enemyPosition;
        TryDropChest(enemyPosition);
    }
    private void TryDropChest(Vector2 spawnPosition)
    {
        bool shouldSpawnChest = Random.Range(0, 101) <= chestDropChance;
        if (!shouldSpawnChest)
            return;
        DropChest(spawnPosition);
    }
    private void DropChest(Vector2 spawnPosition)
    {
        Instantiate(chestPrefab, spawnPosition, Quaternion.identity, transform);
    }
    private void ReleaseCoin(Coin coin) => coinPool.Release(coin);
    private void Releasecherries(Drops cherries) => cherriesPool.Release(cherries);
}
