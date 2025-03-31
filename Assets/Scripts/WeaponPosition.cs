using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    public weapons weapons { get; private set; }
 
    public void AssignWeapon(weapons weapons,int weaponLevel)
    {
        weapons = Instantiate(weapons ,transform);
        weapons.transform.localPosition = Vector3.zero;
        weapons.transform.localRotation = Quaternion.identity;
        weapons.UpgradeTo(weaponLevel);
    }
}
