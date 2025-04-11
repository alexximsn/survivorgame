using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMerger : MonoBehaviour
{
    public static WeaponMerger instance;
    [SerializeField] private PlayerWeapons PlayerWeapons;
    private List<weapons> weaponsToMerge = new List<weapons>();
    public static Action<weapons> onMerge;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    public bool CanMerge(weapons weapon)
    {
        if (weapon.Level >= 3)
            return false;

        weaponsToMerge.Clear();
        weaponsToMerge.Add(weapon);

        weapons[] weapont = PlayerWeapons.GetWeapons();
        foreach(weapons playerWeapon in weapont)
        {
            if (playerWeapon == null)
                continue;
            if (playerWeapon == weapon)
                continue;
            if (playerWeapon.WeaponData.Name != weapon.WeaponData.Name)
                continue;
            if (playerWeapon.Level != weapon.Level)
                continue;

            weaponsToMerge.Add(playerWeapon);
            return true;
        
        }
        return false;
    }
    public void Merge()
    {
        if(weaponsToMerge.Count<2)
        {
            return;
        }
        DestroyImmediate(weaponsToMerge[1].gameObject);

        weaponsToMerge[0].Upgrade();

        weapons weapon = weaponsToMerge[0];
        weaponsToMerge.Clear();

        onMerge?.Invoke(weapon);
    
    }
}
