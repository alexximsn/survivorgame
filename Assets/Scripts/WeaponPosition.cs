using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    public weapons Weapon{ get; private set; }
 
    public void AssignWeapon(weapons weapon,int weaponLevel)
    {
        Weapon = Instantiate(weapon ,transform);
        Weapon.transform.localPosition = Vector3.zero;
        Weapon.transform.localRotation = Quaternion.identity;
        Weapon.UpgradeTo(weaponLevel);
      
    }
}
