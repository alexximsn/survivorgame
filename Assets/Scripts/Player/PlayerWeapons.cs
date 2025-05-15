using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private WeaponPosition[] weaponPositions;
    private int currentWeaponIndex = 0;
    void Start()
    {
        for (int i = 0; i < weaponPositions.Length; i++)
        {
            weaponPositions[i].gameObject.SetActive(i == currentWeaponIndex);
        }
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
    public void SwitchWeapon()
    {
        // 隐藏当前武器
        weaponPositions[currentWeaponIndex].gameObject.SetActive(false);
        // 计算下一个武器索引（循环）
        currentWeaponIndex = (currentWeaponIndex + 1) % weaponPositions.Length;
        // 显示新武器
        weaponPositions[currentWeaponIndex].gameObject.SetActive(true);
    }
}
