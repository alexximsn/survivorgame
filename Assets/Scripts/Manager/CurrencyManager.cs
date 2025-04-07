using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager instance;
    [field: SerializeField] public int Currency { get; private set; }

    public static Action onUpdated;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    void Start()
    {
        UpdateTexts();
    }  
    void Update()
    {
        
    }

    [NaughtyAttributes.Button]
    private void Add500Currency()
    {
        AddCurrency(500);
    }
    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateTexts();
        onUpdated?.Invoke();
    }
    public void UseCurrency(int price)
    {
        AddCurrency(-price);
    }
    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
}
