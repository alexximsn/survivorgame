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
    public void GameStateChangedCallback(GameState gameState)
    {
        switch(gameState)
        {
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
        containerInstance.Configure(weaponData.Sprite,weaponData.Name);
    }
}
