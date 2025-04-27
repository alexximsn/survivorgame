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
    [SerializeField] private UpgradeContainer[] upgrateContainers;
    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private GameObject upgradeContainersParent;
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
            ConfigureUpgradeContainers();

    }
    private void ShowObject()
    {
        chestsCollected--;
        upgradeContainersParent.SetActive(false);
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
    [Button]
    private void ConfigureUpgradeContainers()
    {
        upgradeContainersParent.SetActive(true);
        for (int i = 0; i < upgrateContainers.Length; i++)
        {

            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            Sprite upgradeSprite = ResoursesManager.GetStatIcon(stat);
            string randomStatString = Enums.FormatStatName(stat);
            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);
            upgrateContainers[i].Configure(upgradeSprite, randomStatString, buttonString);

            upgrateContainers[i].Button.onClick.RemoveAllListeners();
            upgrateContainers[i].Button.onClick.AddListener(() => action?.Invoke());
            upgrateContainers[i].Button.onClick.AddListener(() => BonusSelectedCallback());
        }

    }
    private void BonusSelectedCallback()
    {
        GameManager.instance.WaveCompletedCallback();

    }
    private Action GetActionToPerform(Stat stat,out string buttonString)
    {
        buttonString = "";
        float value;
       
       
        switch (stat)
        {
            case Stat.Attack:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.AttackSpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CritialChange:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.CritialPercent:
                value = Random.Range(1f, 2f);
                buttonString = "+" + value.ToString("F2") + "x";
                break;
            case Stat.Movespeed:
                value = Random.Range(1, 5);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.MaxHealth:
                value = Random.Range(1, 5);
                buttonString = "+" + value;
                break;
          
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
         
            case Stat.Lucky:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Dodge:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
       
            default:
                return () => Debug.Log("Invalid stat");

        }
        //buttonString=Enums.FormatStatName(stat)+"\n"+buttonString;
        return () => playerStatsManager.AddPlayerStat(stat,value);
    }
    
    private void ChestCollectedCallback()
    {
        chestsCollected++;
    }
    public bool HasCollectedChest()
    {
        return chestsCollected > 0;
    }
}
