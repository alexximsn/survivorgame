using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
public class ShopItemContainer : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] public Button PurchaseButton;
    [SerializeField] private Image[] levelDependentImages;
    [SerializeField] private Transform statContainersParent;
    
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite lockedSprite, unlockedSprite;

    public static Action<ShopItemContainer, int> onPurchased;
    public bool IsLocked { get; private set; }
    private int weaponLevel;
    public WeaponDataSO WeaponData { get; private set; }
    public ObjectDataSO ObjectData { get; private set; }

    private void Awake()
    {
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }
    private void OnDestroy()
    {
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
    }

    private void CurrencyUpdatedCallback()
    {
        int itemPrice;
        if (WeaponData != null)
            itemPrice = WeaponStatsCalculator.GetPurchasePrice(WeaponData, weaponLevel);
        else
            itemPrice = ObjectData.Price;
        PurchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(itemPrice);
    }

    public void Configure(int level, WeaponDataSO weaponData)
    {
        weaponLevel = level;
        WeaponData = weaponData;
        icon.sprite = weaponData.Sprite;
        nameText.text = weaponData.Name + $"\nlv{level + 1}";
        int weaponPrice = WeaponStatsCalculator.GetPurchasePrice(weaponData, level);
        priceText.text = weaponPrice.ToString();
        Color imageColor = ColorHolder.GetColor(level);
        nameText.color = imageColor;
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
        Dictionary<Stat, float> calculatedStats = WeaponStatsCalculator.GetStats(weaponData, level);
        ConfigureStatContainers(calculatedStats);
        PurchaseButton.onClick.AddListener(Purchase);
        PurchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(weaponPrice);
    }
    public void Configure( ObjectDataSO objectData)
    {
        ObjectData = objectData;
        icon.sprite = objectData.Icon;
        nameText.text = objectData.Name;


        priceText.text = objectData.Price.ToString();
        Color imageColor = ColorHolder.GetColor(objectData.Rarity);
        nameText.color = imageColor;
        foreach (Image image in levelDependentImages)
            image.color = imageColor;
        ConfigureStatContainers(objectData.BaseStats);
        PurchaseButton.onClick.AddListener(Purchase);
        PurchaseButton.interactable = CurrencyManager.instance.HasEnoughCurrency(objectData.Price);
    }
    private void ConfigureStatContainers(Dictionary<Stat, float> calculatedStats)
    {
        statContainersParent.Clear();
        StatContainersManager.GeneratStatContainers(calculatedStats, statContainersParent);

    }
    private void Purchase()
    {
        onPurchased?.Invoke(this,weaponLevel);
    }

    public void LockButtonCallback()
    {
        IsLocked = !IsLocked;
        UpdateLockVisuals();
    }
    private void UpdateLockVisuals()
    {
        lockImage.sprite = IsLocked ? lockedSprite : unlockedSprite;
    }
    
}
