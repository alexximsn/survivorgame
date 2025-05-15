using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;
using NaughtyAttributes;
public class WaveTrasitionManager : MonoBehaviour,IGameStateListener
{
    public static WaveTrasitionManager instance;
    [SerializeField] private PlayerObject playerObjects;
   
    [SerializeField] private PlayerStatsManager playerStatsManager;

    private int chestsCollected;

    [SerializeField] private ChestObjectContainer chestContainerPrefab;
    [SerializeField] private Transform chestContainerParent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        Chest.onCollected += ChestCollectedCallback;
    }
    private void OnDestroy()
    {
        Chest.onCollected -= ChestCollectedCallback;
    }
   
    public void GameStateChangedCallback(GameState gameState)
    {
       switch(gameState)
        {
            case GameState.WAVETRANSITION:
                TryOpenChest();
                break;
        }
    }
    private void TryOpenChest()
    {
        chestContainerParent.Clear();
        if (chestsCollected > 0)
            ShowObject();
        else
       
            GameManager.instance.WaveCompletedCallback(); // 直接跳转到下一阶段

    }
    private void ShowObject()
    {
        chestsCollected--;
      
        ObjectDataSO[] objectDatas = ResoursesManager.Objects;
        ObjectDataSO randomObjectData= objectDatas[Random.Range(0, objectDatas.Length)];
        ChestObjectContainer containerInstance = Instantiate(chestContainerPrefab, chestContainerParent);
        containerInstance.Configure(randomObjectData);

        containerInstance.TakeButton.onClick.AddListener(() => TakeButtonCallback(randomObjectData));
        containerInstance.RecycleButton.onClick.AddListener(() => RecycleButtonCallback(randomObjectData));
    }
    private void TakeButtonCallback(ObjectDataSO objectToTake)
    {
        playerObjects.AddObject(objectToTake);
        TryOpenChest();
    }
    private void RecycleButtonCallback(ObjectDataSO objectToRecycle)
    {
        CurrencyManager.instance.AddCurrency(objectToRecycle.RecyclePrice);
        TryOpenChest();
    }
    public void ChestCollectedCallback()
    {
        chestsCollected++;
    }
    public bool HasCollectedChest()
    {
        return chestsCollected > 0;
    }
}
