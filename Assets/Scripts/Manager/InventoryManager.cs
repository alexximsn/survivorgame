using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InventoryManager : MonoBehaviour,IGameStateListener
{
    [SerializeField] private PlayerWeapons playerWeapons;//����
    [SerializeField] private PlayerObject playerObjects;


    [SerializeField] private Transform inventoryItemsParent;
    [SerializeField] private Transform pauseInventoryItemParent;
    [SerializeField] private InventoryItemContainer inventoryItemContainer;
    [SerializeField] private ShopManagerUI shopManagerUI;//�̵�ĵ���
    [SerializeField] private InventoryItemInfo itemInfo;//����

    private void Awake()
    {
        ShopManager.onItemPurchased += ItemPurchasedCallback;
        WeaponMerger.onMerge += WeaponMergeredCallback;

        GameManager.onGamePaused += Configure;
    }
    private void OnDestroy()
    {
        ShopManager.onItemPurchased -= ItemPurchasedCallback;
        WeaponMerger.onMerge -= WeaponMergeredCallback;

        GameManager.onGamePaused -= Configure;
    }

    private void WeaponMergeredCallback(weapons mergeWeapons)
    {
        Configure();
        itemInfo.Configure(mergeWeapons);
    }
    public void GameStateChangedCallback(GameState gameState)
    {
        if (gameState == GameState.SHOP)
            Configure();
    }
    private void Configure()
    {
        //ÿ�δ��������һ�εĽ��
        inventoryItemsParent.Clear();
        pauseInventoryItemParent.Clear();
        weapons[] weapons = playerWeapons.GetWeapons();
        //��������
        for (int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i] == null)
                continue;
           
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(weapons[i],i,()=>ShowItemInfo(container));

            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemParent);
            pauseContainer.Configure(weapons[i], i, null);
        }
        //���ɵ���
        ObjectDataSO[] objectDatas = playerObjects.Objects.ToArray();
        for(int i=0;i<objectDatas.Length;i++)
        {
            InventoryItemContainer container = Instantiate(inventoryItemContainer, inventoryItemsParent);
            container.Configure(objectDatas[i], () => ShowItemInfo(container));

            InventoryItemContainer pauseContainer = Instantiate(inventoryItemContainer, pauseInventoryItemParent);
            pauseContainer.Configure(objectDatas[i], null);
        }
      
    }
    private void ShowItemInfo(InventoryItemContainer container)
    {
        if (container.Weapon != null)
            ShowWeaponInfo(container.Weapon,container.Index);
        else
            ShowObjectInfo(container.ObjectData);
    }
    private void ShowWeaponInfo(weapons weapon,int index)
    {
        itemInfo.Configure(weapon);

        itemInfo.recycleButton.onClick.RemoveAllListeners();
        itemInfo.recycleButton.onClick.AddListener(() => RecycleWeapon(index));
        shopManagerUI.ShowItemInfo();
    }
    private void RecycleWeapon(int index)
    {
        playerWeapons.RecycleWeapon(index);
        Configure();
        shopManagerUI.HideItemInfo();
    }
    private void ShowObjectInfo(ObjectDataSO objectData)
    {
        itemInfo.Configure(objectData);
        itemInfo.recycleButton.onClick.RemoveAllListeners();
        itemInfo.recycleButton.onClick.AddListener(() => RecycleObject(objectData));
        shopManagerUI.ShowItemInfo();
    }

    private void RecycleObject(ObjectDataSO objectToRcycle)
    {
        playerObjects.RecycleObject(objectToRcycle);
        Configure();
        shopManagerUI.HideItemInfo();
    }
    private void ItemPurchasedCallback() => Configure();

}
