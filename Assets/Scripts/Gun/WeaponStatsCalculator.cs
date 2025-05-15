using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponStatsCalculator
{
    public static Dictionary<Stat, float> GetStats(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level * 0.2f;
        Dictionary<Stat, float> calculatedStats = new Dictionary<Stat, float>();
        foreach (KeyValuePair<Stat, float> kvp in weaponData.BaseStats)
        {
            
                calculatedStats.Add(kvp.Key, kvp.Value * multiplier);
        }
        return calculatedStats;
    }
    public static int GetPurchasePrice(WeaponDataSO weaponData,int level)
    {
        float multiplier = 1 + (float)level * 0.2f;
        return (int)(weaponData.PurchasePrice*multiplier);
    }
    public static int GetRecyclePrice(WeaponDataSO weaponData, int level)
    {
        float multiplier = 1 + (float)level * 0.2f;
        return (int)(weaponData.RecyclePrice * multiplier);
    }
}
