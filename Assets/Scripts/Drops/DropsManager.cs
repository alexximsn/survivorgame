using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsManager : MonoBehaviour
{
    [SerializeField] private Drops cherries;
    private void Awake()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }
    private void OnDestroy()
    {
        Enemy.onPassedAway += EnemyPassedAwayCallback;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void EnemyPassedAwayCallback(Vector2 enemyPosition)
    {
        //throw new NotImplementedException();
        Instantiate(cherries, enemyPosition, Quaternion.identity, transform);
    }
}
