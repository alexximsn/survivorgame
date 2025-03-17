using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
   
    [SerializeField] private CharacterDataSO playerData;

    private Dictionary<Stat, float> playerStats = new Dictionary<Stat, float>();
    private Dictionary<Stat, float> addends = new Dictionary<Stat, float>();
    private void Awake()
    {
        playerStats = playerData.BaseStats;
        foreach (KeyValuePair<Stat, float> kvp in playerStats)
            addends.Add(kvp.Key, 0);
    }
    void Start()
    {
       
        UpdatePlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddPlayerStat(Stat stat,float value)
    {
        if (addends.ContainsKey(stat))

        {
            if (stat == Stat.Movespeed)
                Debug.Log(addends[stat] + "-Before");
              addends[stat] += value;
            if (stat == Stat.Movespeed)
                Debug.Log(addends[stat] + "-After");
        
        }
        else
           Debug.LogError($"12222");
        UpdatePlayerStats();
    }
    public float GetStatValue(Stat stat)
    {
        float value = playerStats[stat] + addends[stat];
        return value;
    }

    private void UpdatePlayerStats()
    {
        IEnumerable<IPlayerStatsDependency> playerStatsDependencies =
           FindObjectsByType<MonoBehaviour>
           (FindObjectsSortMode.None).OfType<IPlayerStatsDependency>();
        foreach (IPlayerStatsDependency dependency in
           playerStatsDependencies)
            dependency.UpdateStats(this);
    }
}


