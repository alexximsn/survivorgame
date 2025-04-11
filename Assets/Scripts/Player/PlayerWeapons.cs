using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] weaponPositions;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool TryAddWeapon(WeaponDataSO weapon, int level)
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            if (weaponPositions[i].Weapon != null)
            {
              
                continue;
            }
            if (weaponPositions[i].Weapon == null)
            {
                weaponPositions[i].AssignWeapon(weapon.Prefab, level);
            
                return true;
            }
        
    }

        return false;
    }
    public void RecycleWeapon(int weaponIndex)
    {
       for(int i=0;i<weaponPositions.Length;i++)
        {
            if (i != weaponIndex)
                continue;
            int recyclePrice = weaponPositions[i].Weapon.GetRecyclePrice();
            CurrencyManager.instance.AddCurrency(recyclePrice);
            weaponPositions[i].RemoveWeapon();
            return;
        }
    }

    public weapons[] GetWeapons()
    {
        List<weapons> weapona = new List<weapons>();
        foreach(WeaponPosition weaponPosition in weaponPositions)
        {
            if (weaponPosition.Weapon == null)
                weapona.Add(null);
         
            else

            weapona.Add(weaponPosition.Weapon);
        }
        return weapona.ToArray();
    }

}
