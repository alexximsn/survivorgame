using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ShopManager : MonoBehaviour,IGameStateListener
{
    [SerializeField] private Transform containersParent;
    [SerializeField] private GameObject shopItemContainerPrefab;
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
        containersParent.Clear();
        int containersToAdd = 6;
        for(int i=0;i<containersToAdd;i++)
        {
            Instantiate(shopItemContainerPrefab, containersParent);
        }
    }
}
