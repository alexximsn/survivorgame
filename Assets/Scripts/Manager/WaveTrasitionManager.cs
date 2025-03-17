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
    [SerializeField] private UpgradeContainer[] upgrateContainers;
    [SerializeField] private PlayerStatsManager playerStatsManager;


    // Start is called before the first frame update
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
                ConfigureUpgradeContainers();
                break;
        }
    }
    [Button]
    private void ConfigureUpgradeContainers()
    {
        for (int i = 0; i < upgrateContainers.Length; i++)
        {

            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString = Enums.FormatStatName(stat);
            string buttonString;
            Action action = GetActionToPerform(stat, out buttonString);
            upgrateContainers[i].Configure(null, randomStatString, buttonString);

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
            case Stat.Range:
                value = Random.Range(1f, 5f);
                buttonString = "+" + value.ToString();
                break;
            case Stat.HealthRecoverySpeed:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            case Stat.Armor:
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
            case Stat.LifeSteal:
                value = Random.Range(1, 10);
                buttonString = "+" + value.ToString() + "%";
                break;
            default:
                return () => Debug.Log("Invalid stat");

        }
        return () => playerStatsManager.AddPlayerStat(stat,value);
    }
}
