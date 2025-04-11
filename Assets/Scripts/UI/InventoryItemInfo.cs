using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryItemInfo : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI recyclePriceText;

    [SerializeField] private Image container;

    [SerializeField] private Transform statsParent;

    [field: SerializeField] public Button recycleButton { get; private set; }
    [SerializeField] private Button mergeButton;

    public void Configure(weapons weapon)
    {
        Configure
            (weapon.WeaponData.Sprite,
            weapon.WeaponData.Name+"(lvl"+(weapon.Level+1)+")",
            ColorHolder.GetColor(weapon.Level),
            WeaponStatsCalculator.GetRecyclePrice(weapon.WeaponData,weapon.Level),
            WeaponStatsCalculator.GetStats(weapon.WeaponData,weapon.Level)
                ) ;
        mergeButton.gameObject.SetActive(true);

        mergeButton.interactable = WeaponMerger.instance.CanMerge(weapon);
        mergeButton.onClick.RemoveAllListeners();
        mergeButton.onClick.AddListener(WeaponMerger.instance.Merge);
    }
    public void Configure(ObjectDataSO objectData)
    {
        Configure
            (objectData.Icon,
            objectData.Name,
            ColorHolder.GetColor(objectData.Rarity),
            objectData.RecyclePrice,
            objectData.BaseStats
                );
        mergeButton.gameObject.SetActive(false);
    }

    private void Configure(Sprite itemIcon,string name,Color containerColor,int recyclePrice,Dictionary<Stat,float> stats)
    {
        icon.sprite = itemIcon;
        itemNameText.text = name;
        itemNameText.color = containerColor;

        recyclePriceText.text = recyclePrice.ToString();

        container.color = containerColor;
        StatContainersManager.GeneratStatContainers(stats, statsParent);
    }
}
