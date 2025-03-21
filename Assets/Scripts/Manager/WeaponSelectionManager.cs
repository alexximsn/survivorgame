using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponSelectionManager : MonoBehaviour,IGameStateListener
{
    [SerializeField] private Transform containersParent;
    [SerializeField] private WeaponSelectionContainer weaponContainerPrefab;
    [SerializeField] private WeaponDataSO[] starterWeapons;
    [SerializeField] private PlayerWeapons playerWeapons;
    private WeaponDataSO selectedWeapon;
    private int initialWeaponLevel;
    public void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
            case GameState.GAME:
                if (selectedWeapon == null)
                    return;
                playerWeapons.AddWeapon(selectedWeapon,initialWeaponLevel);
                selectedWeapon = null;
                initialWeaponLevel = 0;
                break;
            case GameState.WEAPONSELECTION:
                Configure();
                break;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void Configure()
    {
        containersParent.Clear();
        for (int i = 0; i < 3; i++)
            GenerateWeaponContainer();
    }
    private void GenerateWeaponContainer()
    {
        WeaponSelectionContainer containerInstance = Instantiate(weaponContainerPrefab, containersParent);
        WeaponDataSO weaponData = starterWeapons[Random.Range(0, starterWeapons.Length)];
        int level = Random.Range(0, 4);
        initialWeaponLevel = level;
        containerInstance.Configure(weaponData.Sprite,weaponData.Name,level);
        containerInstance.Button.onClick.RemoveAllListeners();
        containerInstance.Button.onClick.AddListener(() => WeaponSelectedCallback(containerInstance, weaponData));
    }
    private void WeaponSelectedCallback(WeaponSelectionContainer containerInstance,WeaponDataSO weaponData)
    {
        selectedWeapon = weaponData;
        foreach (WeaponSelectionContainer container in containersParent.GetComponentsInChildren<WeaponSelectionContainer>())
        {
            if (container == containerInstance)
                container.Select();
            else
                container.Deselect();
        }
    }
}
