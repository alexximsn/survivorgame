using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddWeapon(WeaponDataSO selectedWeapon,int weaponLevel)
    {
        Debug.Log("ѡ����" + selectedWeapon.Name+"�ȼ���"+weaponLevel);
    }
}
