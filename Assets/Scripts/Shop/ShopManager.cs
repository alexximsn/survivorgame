using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopManager : MonoBehaviour,IGameStateListener
{
    [SerializeField] private Transform containersParent;
    [SerializeField] private ShopItemContainer shopItemContainerPrefab;

    [SerializeField] private PlayerWeapons playerweapons;
    [SerializeField] private PlayerObject playerObjects;

    [SerializeField] private Button rerollButton;
    [SerializeField] private int rerollPrice;
    [SerializeField] private TextMeshProUGUI rerollPriceText;

    private void Awake()
    {
        ShopItemContainer.onPurchased += ItemPurchasedCallback;
        CurrencyManager.onUpdated += CurrencyUpdatedCallback;
    }
    private void OnDestroy()
    {
        ShopItemContainer.onPurchased -= ItemPurchasedCallback;
        CurrencyManager.onUpdated -= CurrencyUpdatedCallback;
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
        if (gameState == GameState.SHOP)
        {
            Configure();
            UpdateRerollVisuals();
        }
           
    }
    private void Configure()
    {
        List<GameObject> toDestroy = new List<GameObject>();
        for(int i=0;i<containersParent.childCount;i++)
        {
            ShopItemContainer container = containersParent.GetChild(i).GetComponent<ShopItemContainer>();
            if (!container.IsLocked)
                toDestroy.Add(container.gameObject);
        }
        while(toDestroy.Count>0)
        {
            Transform t = toDestroy[0].transform;
            t.SetParent(null);
            Destroy(t.gameObject);
            toDestroy.RemoveAt(0);
        }

        int containersToAdd = 6-containersParent.childCount;
        int weaponContainerCount = Random.Range(Mathf.Min(2, containersToAdd), containersToAdd);
        int objectContainerCount = containersToAdd - weaponContainerCount;
        for(int i=0;i<weaponContainerCount;i++)
        {
            ShopItemContainer weaponContainerInstance =Instantiate(shopItemContainerPrefab, containersParent);
            WeaponDataSO randomWeapon = ResoursesManager.GetRandomWeapon();
            weaponContainerInstance.Configure(Random.Range(0,2),randomWeapon);
        }
        for (int i = 0; i < objectContainerCount; i++)
        {
            ShopItemContainer objectContainerInstance = Instantiate(shopItemContainerPrefab, containersParent);
            ObjectDataSO randomObject = ResoursesManager.GetRandomObject();
            objectContainerInstance.Configure(randomObject);
        }
    }
    public void Reroll()
    {
        Configure();
        CurrencyManager.instance.UseCurrency(rerollPrice);
    }
    private void UpdateRerollVisuals()
    {
        rerollPriceText.text = rerollPrice.ToString();
        rerollButton.interactable = CurrencyManager.instance.HasEnoughCurrency(rerollPrice);
    }
    private void CurrencyUpdatedCallback()
    {
        UpdateRerollVisuals();
    }
    private void ItemPurchasedCallback(ShopItemContainer container,int weaponLevel)
    {
        if (container.WeaponData != null)
            TryPurchaseWeapon(container, weaponLevel);
        else
            PurchaseObject(container);
    }
    private void TryPurchaseWeapon(ShopItemContainer container,int weaponLevel)
    {
        if (playerweapons.TryAddWeapon(container.WeaponData, weaponLevel))
        {
            int price = WeaponStatsCalculator.GetPurchasePrice(container.WeaponData, weaponLevel);
            CurrencyManager.instance.UseCurrency(price);

            Destroy(container.gameObject);
        }
    }
    private void PurchaseObject(ShopItemContainer container)
    {
        playerObjects.AddObject(container.ObjectData);
        CurrencyManager.instance.UseCurrency(container.ObjectData.Price);
        Destroy(container.gameObject);
    }
}
