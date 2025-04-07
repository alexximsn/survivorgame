using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryManager : MonoBehaviour,IGameStateListener
{
    [SerializeField] private PlayerWeapons playerWeapons;
    [SerializeField] private PlayerObject playerObjects;


    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;

   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }
    private void Configure()
    {
        inventoryItemsParent.Clear();

        weapons[] weapons = playerWeapons.GetWeapons();

        for (int i = 0; i < weapons.Length; i++)
        {
           
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);

            container.Configure(weapons[i],()=>ShowItemInfo(container));
        }

        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();
        for(int i=0;i<objectDatas.Length;i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(objectDatas[i], () => ShowItemInfo(container));
        }
      
    }
    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon);
        else
            ShowObjectInfo(container.ObjectData);
    }
    private void ShowWeaponInfo(weapons weapon)
    {
        Debug.Log(weapon.WeaponData.Name);
    }
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        Debug.Log(objectData.Name);
    }
}
