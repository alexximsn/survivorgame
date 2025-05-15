using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Tabsil.SaverManager;
public class CurrencyManager : MonoBehaviour,IWantToBeSaved
{
    public static CurrencyManager instance;

    private const string premiumCurrencyKey = "PremiumCurrency";

    [field: SerializeField] public int Currency { get; private set; }
    [field: SerializeField] public int PremiumCurrency { get; private set; }
    public static Action onUpdated;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

       
       // AddPremiumCurrency(PlayerPrefs.GetInt(premiumCurrencyKey, 100),false);

        Drops.onCollected += DropsCollectedCallback;
        Coin.onCollected += CoinCollectedCallback;
    }
    private void OnDestroy()
    {
        Drops.onCollected -= DropsCollectedCallback;
        Coin.onCollected -= CoinCollectedCallback;
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
    [NaughtyAttributes.Button]
    private void Add500PCurrency()
    {
        AddPremiumCurrency(500);
    }
    public void AddCurrency(int amount)
    {
        Currency += amount;
        UpdateVisuals();

    }
    public void AddPremiumCurrency(int amount,bool save=true)
    {
        PremiumCurrency += amount;
        UpdateVisuals();
        //PlayerPrefs.SetInt(premiumCurrencyKey, PremiumCurrency);
    }
    private void UpdateVisuals()
    {
        UpdateTexts();
        onUpdated?.Invoke();
        Save();
    }
    public void UseCurrency(int price)
    {
        AddCurrency(-price);
    }
    public void UsePremiumCurrency(int price)
    {
        AddPremiumCurrency(-price);
    }
    private void UpdateTexts()
    {
        CurrencyText[] currencyTexts = FindObjectsByType<CurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (CurrencyText text in currencyTexts)
            text.UpdateText(Currency.ToString());

        PremiumCurrencyText[] premiumcurrencyTexts = FindObjectsByType<PremiumCurrencyText>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (PremiumCurrencyText text in premiumcurrencyTexts)
            text.UpdateText(PremiumCurrency.ToString());
    }

    public bool HasEnoughCurrency(int price)
    {
        return Currency >= price;
    }
    public bool HasEnoughPremiumCurrency(int price)
    {
        return PremiumCurrency >= price;
    }
    private void DropsCollectedCallback(Drops drops)
    {
        AddCurrency(1);
    }
    private void CoinCollectedCallback(Coin coin)
    {
        AddPremiumCurrency(1);
    }

    public void Load()
    {
        if (SaverManager.TryLoad(this, premiumCurrencyKey, out object premiumCurrencyValue))
            AddPremiumCurrency((int)premiumCurrencyValue,false);
        else
            AddPremiumCurrency(100,false);
    }

    public void Save()
    {
        SaverManager.Save(this,premiumCurrencyKey,PremiumCurrency);
    }
}
