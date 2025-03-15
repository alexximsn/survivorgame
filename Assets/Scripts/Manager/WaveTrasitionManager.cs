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
    [SerializeField] private Button[] upgrateContainers;

    

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
        for(int i=0;i<upgrateContainers.Length;i++)
        {
           
            int randomIndex = Random.Range(0, Enum.GetValues(typeof(Stat)).Length);
            Stat stat = (Stat)Enum.GetValues(typeof(Stat)).GetValue(randomIndex);
            string randomStatString =Enums.FormatStatName(stat);
            upgrateContainers[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = randomStatString;
            upgrateContainers[i].onClick.RemoveAllListeners();
            upgrateContainers[i].onClick.AddListener(() => Debug.Log(randomStatString));
        }

    }
}
