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
    public void AddWeapon(WeaponDataSO selectedWeapon,int weaponLevel)
    {
        // Debug.Log("ѡ����" + selectedWeapon.Name+"�ȼ���"+weaponLevel);
        weaponPositions[Random.Range(0, weaponPositions.Length)].AssignWeapon(selectedWeapon.Prefab,weaponLevel);
    }
}
